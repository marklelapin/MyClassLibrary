

using MyClassLibrary.Interfaces;
using MyClassLibrary.LocalServerMethods;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace MyClassLibrary.LocalServerMethods
{
    public class LocalServerEngine<T> : ILocalServerEngine<T> where T : LocalServerIdentityUpdate
    {
        //Properties
        /// <summary>
        /// The interface providing access to Local Data Storage
        /// </summary>
        private ILocalDataAccess _localDataAccess;
        /// <summary>
        /// The interface providing access to Server Data Storage
        /// </summary>
        private IServerDataAccess _serverDataAccess;

        /// <summary>
        /// Constructor for LocalServerIdentiy
        /// In Production environment services will supply IServerDataAcces and ILocalDataAccess. These can be added in for testing through optional parameters.
        /// </summary>
        public LocalServerEngine(IServerDataAccess? serverDataAccess = null, ILocalDataAccess? localDataAccess = null)
        {
            _serverDataAccess = serverDataAccess ?? new ServerSQLConnector("No Connection");
            _localDataAccess = localDataAccess ?? new LocalSQLConnector("No Connection");
        }

        //Public Methods
        /// <summary>
        /// Sorts the list by Id
        /// </summary>
        public void SortById(List<T> updates)
        {
            updates.Sort((x, y) => x.Id.CompareTo(y.Id));
        }
        /// <summary>
        /// Sorts the list by Created
        /// </summary>
        public void SortByCreated(List<T> updates)
        {
            updates.Sort((x, y) => x.Created.CompareTo(y.Created));
        }
        /// <summary>
        /// Sorts the list by ID and then by Created
        /// </summary>
        public void SortByIdAndCreated(List<T> updates)
        {
            updates.Sort((x, y) =>
                        {
                            int result = x.Id.CompareTo(y.Id);

                            if (result == 0)
                            {
                                result = x.Created.CompareTo(y.Created);
                            }
                            return result;
                        });
        }




        /// <summary>
        /// save a Updates to Local and Server if it can. Throws error if can do neither.
        /// </summary>
        public void Save(T update)
        {
            Save(new List<T> { update });
        }
        /// <summary>
        /// save a Updates to Local and Server if it can. Throws error if can do neither.
        /// </summary>
        public void Save(List<T> updates)
        {
            bool serverSuccess = TrySaveToServer(updates);
            bool localSuccess = TrySaveToLocal(updates);
            if (!serverSuccess && !localSuccess) { throw new Exception("Failed to Save to Server or Local Storage. Check configuration of data access"); }

        }
        /// <summary>
        /// Produces a list of Updates filtered for the latest Created date plus any with a conflictID.
        /// </summary>
        public List<T> FilterLatest(List<T> updates)
        {
            List<T> output;

            //filters for latest created for each Id and removes inactive Updates.
            var filter = updates.GroupBy(x => x.Id).Select(g => new { Id = g.Key, Created = g.Max(x => x.Created) }).ToList();

            output = (List<T>)(from o in updates
                               join f in filter
                               on new { o.Id, o.Created } equals new { f.Id, f.Created }
                               select o);
            output = output.Where(x => x.IsActive == true).ToList();

            //then adds back in any conflicted 

            var conflicts = updates.Where(x => x.ConflictId != null).ToList();

            output.Union(conflicts);

            return output;
        }


        /// <summary>
        /// Tries to sync Local and Server returning false if it fails. Identifies and saves conflicts.
        /// </summary>
        /// <returns></returns>
        public bool TrySync()
        {
            List<T> changesFromServer = new List<T>();
            DateTime lastUpdatedOnServer = DateTime.MinValue;
            List<T> changesFromLocal = new List<T>();


            try
            {
                DateTime lastSyncDate = _localDataAccess.GetLocalLastSyncDate<T>();

                changesFromLocal = _localDataAccess.GetChangesFromLocal<T>();

                (changesFromServer, lastUpdatedOnServer) = _serverDataAccess.GetChangesFromServer<T>(lastSyncDate);

            }
            catch { return false; } //if it can't access server and local then stops sync until another attempt is made and it is called again


            List<Conflict> conflictIds = FindConflicts(changesFromServer, changesFromLocal);




            if (!TrySaveServerChangesToLocal(changesFromServer, lastUpdatedOnServer)) return false;

            if (!TrySaveLocalChangesToServer(changesFromLocal)) return false;

            if (!SaveConflictIds(conflictIds)) return false;


            return true;

        }


        /// <summary>
        /// Populates the Updates list with all of the Updates matching Id from Local (or Server if Local unavailable)
        /// </summary>
        public List<T> GetAllUpdates(Guid id)
        {
            return GetAllUpdates(new List<Guid> { id });
        }
        /// <summary>
        /// Populates the Updates list with all of the Updates matching Id from Local (or Server if Local unavailable)
        /// </summary>
        public List<T> GetAllUpdates(List<Guid> ids)
        {
            List<T> output = new List<T>();

            TrySync();

            try
            {
                if (_localDataAccess != null) { output = _localDataAccess.GetFromLocal<T>(ids); }
            }
            catch (Exception)
            {
                if (_serverDataAccess != null) { output = _serverDataAccess.GetFromServer<T>(ids); }
            }

            return output;
        }

        //Private Methods
        /// <summary>
        /// Tries to Save Updates to Local. If it fails returns false.
        /// </summary>
        private bool TrySaveToLocal(List<T> updates)
        {
            try
            {
                _localDataAccess.SaveToLocal(updates);
                return true;
            }
            catch
            {
                return false;
                //TODO Log error
            }
        }
        /// <summary>
        /// Tries to Save Updates to Server. If it fails returns false.
        /// </summary>
        private bool TrySaveToServer(List<T> updates)
        {
            try
            {
                _serverDataAccess.SaveToServer(updates);
                return true;
            }
            catch
            {
                return false;
                //TODO Log error
            }
        }

        /// <summary>
        /// Finds changesFromServer with the same Id as changesFromLocal. For each conflicted Id it generates a ConflictID
        /// </summary>
        public List<Conflict> FindConflicts(List<T> changesFromServer, List<T> changesFromLocal)
        {

            List<Conflict> output = new List<Conflict>();

            List<T> conflictedUpdates = new List<T>();

            conflictedUpdates.AddRange(changesFromServer.Where(x => x.ConflictId != null).ToList());

            List<T> joinedList = (
                              from s in changesFromServer
                              join l in changesFromLocal
                              on s.Id equals l.Id
                              select s
                              ).ToList();

            conflictedUpdates.AddRange(joinedList);

            Dictionary<Guid, Guid> conflictDictionary = CreateConflictIds(conflictedUpdates);

            List<T> changes = new List<T>();

            changes.AddRange(changesFromServer);
            changes.AddRange(changesFromLocal);

            changes = (from c in changes
                       join f in conflictedUpdates
                       on c.Id equals f.Id
                       select c).Distinct().ToList();

            foreach (T obj in changes)
            {
                conflictDictionary.TryGetValue(obj.Id, out Guid conflictID);
                output.Add(new Conflict(obj.Id, obj.Created, obj.ConflictId ?? conflictID));
            }


            return output;
        }
        /// <summary>
        /// Saves the ConflictID associates with it to each object with matching Id on both server and local.
        /// </summary>
        private bool SaveConflictIds(List<Conflict> conflicts)
        {
            try
            {
                _localDataAccess.SaveConflictIdsToLocal<T>(conflicts);
                _serverDataAccess.SaveConflictIdsToServer<T>(conflicts);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save conflicts", ex);
            }
            return true;
        }

        /// <summary>
        /// Tries to save changesFromServer and if successfull saves lastUpdatedOnServer. (if this final step fails it throws an error)
        /// </summary>
        private bool TrySaveServerChangesToLocal(List<T> changesFromServer, DateTime lastUpdatedOnServer)
        {
            try
            {
                _localDataAccess.SaveToLocal(changesFromServer);
                try
                {
                    _localDataAccess.SaveLocalLastSyncDate<T>(lastUpdatedOnServer);
                }
                catch (Exception ex)
                {
                    _localDataAccess.DeleteFromLocal(changesFromServer);
                    throw new NotImplementedException("Need to log this error.", ex); //TODO Add error logging to sync
                    return false;

                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occured syncing local with server. Rollback on Local wasn't possible causing permanent errors.", ex);
            }
            return true;
        }

        /// <summary>
        /// Tries to save changesFromLocal and if successfull updates UpdatedOnServer to local Updates. (if this final step fails it throws an error)
        /// </summary>
        private bool TrySaveLocalChangesToServer(List<T> changesFromLocal)
        {
            DateTime? newUpdatedOnServer = null;

            try
            {
                newUpdatedOnServer = _serverDataAccess.SaveToServer(changesFromLocal);
                try
                {
                    if (newUpdatedOnServer != null)
                    {
                        _localDataAccess.SaveUpdatedOnServerToLocal(changesFromLocal, (DateTime)newUpdatedOnServer);
                    };
                }
                catch (Exception ex)
                {
                    _serverDataAccess.DeleteFromServer(changesFromLocal);
                    throw new NotImplementedException("Failed to Save Updated to local storage. Changes from server have been rolled back.", ex);
                    return false;
                } //TODO Log Error

            }
            catch (Exception ex)
            {
                throw new Exception("An error has occured syncing server with local. Rollback on the server wasnt possible causing permament errors.", ex);
            }

            return true;

        }


        private Dictionary<Guid, Guid> CreateConflictIds(List<T> conflictedUpdates)
        {
            Dictionary<Guid, Guid> output = new Dictionary<Guid, Guid>();

            List<Guid> conflictedObjectIds = conflictedUpdates.Select(x => x.Id).Distinct().ToList();


            Dictionary<Guid, Guid> conflictIds = new Dictionary<Guid, Guid>();
            foreach (Guid id in conflictedObjectIds)
            {
                conflictIds.Add(id, Guid.NewGuid());
            }

            return output;
        }

 
    }

}

