

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
    public class LocalServerIdentityList<T> where T : LocalServerIdentity
    {
        //Properties
        private  ILocalDataAccess _localDataAccess;
        private  IServerDataAccess _serverDataAccess;

        public List<T> Objects { get; private set; }  
        
        //Constructor
        public LocalServerIdentityList (List<T>? objects = null,IServerDataAccess? serverDataAccess = null, ILocalDataAccess? localDataAccess = null)
        {
           
                _serverDataAccess = serverDataAccess ?? new ServerSQLConnector("No Connection");
                _localDataAccess = localDataAccess ?? new LocalSQLConnector("No Connection");  
            
               Objects = objects ?? new List<T>(); 
        }
  

        //Public Methods
        public void SortById()
        {
            Objects.Sort((x, y) => x.Id.CompareTo(y.Id));
        }
        public void SortByCreated()
        {
            Objects.Sort((x, y) => x.Created.CompareTo(y.Created));
        }
        public void SortByIdAndCreated()
        {
            Objects.Sort((x, y) =>
                        {
                            int result = x.Id.CompareTo(y.Id);

                            if (result == 0)
                            {
                                result = x.Created.CompareTo(y.Created);
                            }
                            return result;
                        });
        }

        public void Save()
        {
            if (!TrySaveToServer() && !TrySaveToLocal()) { throw new Exception("Failed to Save to Server or Local Storage. Check configuration of data access"); }
             
        }

        public List<T> FilterLatest()
        {
            List<T> output;

            //filters for latest created for each Id and removes inactive objects.
            var filter = Objects.GroupBy(x => x.Id).Select(g => new { Id = g.Key, Created = g.Max(x => x.Created) }).ToList();

            output = (List<T>)(from o in Objects
                               join f in filter
                               on new { o.Id, o.Created } equals new { f.Id, f.Created }
                               select o);
            output = output.Where(x => x.IsActive == true).ToList();

            //then adds back in any conflicted 

            var conflicts = Objects.Where(x => x.ConflictID != null).ToList();
            
            output.Union(conflicts);
            
            return output;
        }

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


            Dictionary<Guid, Guid> conflictIds = FindConflictedIds(changesFromServer, changesFromLocal);


            if (!TrySaveServerChangesToLocal(changesFromServer, lastUpdatedOnServer)) return false;

            if (!TrySaveLocalChangesToServer(changesFromLocal)) return false;

            if (!SaveConflictIds(conflictIds)) return false;


            return true;

        }
        public void PopulateObjects(Guid id)
                {
                    PopulateObjects(new List<Guid> { id });
                }
        public void PopulateObjects(List<Guid> ids)
        {
            List<T>? output = new List<T>();

            try
            {
                if (_localDataAccess != null) { output = _localDataAccess.GetFromLocal<T>(ids); }
            }
            catch (Exception)
            {
                if (_serverDataAccess != null) { output = _serverDataAccess.GetFromServer<T>(ids); }
            }

            Objects = output;
        }
        
        //Private Methods
        private bool TrySaveToLocal()
        {
            try
            {
                _localDataAccess.SaveToLocal(Objects);
                return true;
            }
            catch
            {
                return false;
                //TODO Log error
            }
        }
        private bool TrySaveToServer()
        {
            try
            {
                _serverDataAccess.SaveToServer(Objects);
                return true;
            }
            catch
            {
                return false;
                //TODO Log error
            }
        }

        private Dictionary<Guid,Guid> FindConflictedIds(List<T> changesFromServer,List<T> changesFromLocal)
        {
            Dictionary<Guid,Guid> output = new Dictionary<Guid,Guid>();
            List<Guid> conflictedIds = new List<Guid>();

            conflictedIds.AddRange(changesFromServer.Where(x => x.ConflictID != null).Select(x => x.Id).ToList());

            var joinedList = (
                              from s in changesFromServer
                              join l in changesFromLocal
                              on s.Id equals l.Id
                              select s.Id
                              );

            conflictedIds.AddRange(joinedList);

            conflictedIds.Distinct();

            foreach (Guid id in conflictedIds)
            {
                output.Add(id,Guid.NewGuid());
            }

            return output;
        }
        private bool SaveConflictIds(Dictionary<Guid,Guid> dictionary)
        {
            try
            {
                _localDataAccess.SaveConflictIds<T>(dictionary);
                _serverDataAccess.SaveConflictIds<T>(dictionary);               
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save conflicts", ex);
            }
            return true;  
        }


        private bool TrySaveServerChangesToLocal(List<T> changesFromServer,DateTime lastUpdatedOnServer)
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
                        _localDataAccess.SaveUpdatedOnServerDate(changesFromLocal, (DateTime)newUpdatedOnServer);
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
                throw new Exception("An error has occured syncing server with local. Rollback on the server wasnt possible causing permament errors.",ex);
            }

            return true;

        }
   
            
            
}
            
}

