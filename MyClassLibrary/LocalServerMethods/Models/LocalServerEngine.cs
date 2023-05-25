

using MyClassLibrary.LocalServerMethods.Interfaces;
using System.Data;


namespace MyClassLibrary.LocalServerMethods.Models
{
    public class LocalServerEngine<T> : ILocalServerEngine<T> where T : LocalServerModelUpdate, new()
    {

        //TODO Test GetAllIdentities function
        //TODO Change sync process - moving away from UpdatedonServer to separate table in server.
        //TODO Test Sync


        //Properties
        /// <summary>
        /// The interface providing access to Local Data Storage
        /// </summary>
        private ILocalDataAccess<T> _localDataAccess;
        /// <summary>
        /// The interface providing access to Server Data Storage
        /// </summary>
        private IServerDataAccess<T> _serverDataAccess;

        /// <summary>
        /// Constructor for LocalServerIdentiy
        /// In Production environment services will supply IServerDataAcces and ILocalDataAccess.
        /// </summary>
        public LocalServerEngine(IServerDataAccess<T> serverDataAccess, ILocalDataAccess<T> localDataAccess)
        {
            _serverDataAccess = serverDataAccess;
            _localDataAccess = localDataAccess;
        }

        //Public Methods
        /// <summary>
        /// Sorts the list by Id
        /// </summary>
        public Task SortById(List<T> updates)
        {
            updates.Sort((x, y) => x.Id.CompareTo(y.Id));
            return Task.CompletedTask;
        }
        /// <summary>
        /// Sorts the list by Created
        /// </summary>
        public Task SortByCreated(List<T> updates)
        {
            updates.Sort((x, y) => x.Created.CompareTo(y.Created));
            return Task.CompletedTask;
        }
        /// <summary>
        /// Sorts the list by ID and then by Created
        /// </summary>
        public Task SortByIdAndCreated(List<T> updates)
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
            return Task.CompletedTask;
        }




        /// <summary>
        /// save a Updates to Local and Server if it can. Throws error if can do neither.
        /// </summary>
        public async Task Save(T update)
        {
            await Save(new List<T> { update });
        }
        /// <summary>
        /// save a Updates to Local and Server if it can. Throws error if can do neither.
        /// </summary>
        public async Task Save(List<T> updates)
        {
            Task<bool> serverSuccess = TrySaveToServer(updates); //Have to wait for this to finish as adds in UpdatedOnServerDate to Updates
            serverSuccess.Wait();
            Task<bool> localSuccess = TrySaveToLocal(updates);

            await Task.WhenAll(serverSuccess, localSuccess);

            if (!serverSuccess.Result && !localSuccess.Result) { throw new Exception("Failed to Save to Server or Local Storage. Check configuration of data access"); }
          
        }
        /// <summary>
        /// Produces a list of Updates filtered for the latest Created date plus any with a conflictID.
        /// </summary>
        public  async Task<List<T>> FilterLatest(List<T> updates)
        {
            List<T> output;

            //filters for latest created for each Id and removes inactive Updates.
            var filter = await Task.Run(()=>
            
                updates.GroupBy(x => x.Id).Select(g => new { Id = g.Key, Created = g.Max(x => x.Created) }).ToList());

            var sort = await Task.Run(() =>
                        (List<T>)(from o in updates
                                  join f in filter
                                  on new { o.Id, o.Created } equals new { f.Id, f.Created }
                                  select o));


            var active = await Task.Run(()=>sort.Where(x => x.IsActive == true).ToList());

            ////then adds back in any conflicted 

            //var conflicts = await Task.Run(() => updates.Where(x => x.ConflictId != null).ToList());

            //var output = await Task.Run(() => (List<T>)active.Union(conflicts));
            output = active;

            return output;
        }


        /// <summary>
        /// Tries to sync Local and Server returning false if it fails. Identifies and saves conflicts.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TrySync() //TODO Change method of syncing to remove locallastsyncdate. USe copy Guid to identify the local copy and then have table on server that links copyguid with server guid.
        {
            List<T> changesFromServer = new List<T>();
            DateTime lastUpdatedOnServer = DateTime.MinValue;
            List<T> changesFromLocal = new List<T>();


            try
            {
                DateTime lastSyncDate = await _localDataAccess.GetLocalLastSyncDate();

                changesFromLocal = await _localDataAccess.GetChangesFromLocal();

                (changesFromServer, lastUpdatedOnServer) = await _serverDataAccess.GetChangesFromServer(lastSyncDate);

            }
            catch { return false; } //if it can't access server and local then stops sync until another attempt is made and it is called again
            //TODO add in pass back of error information.when syncing if not to do with connection.

            List<Conflict> conflictIds = await FindConflicts(changesFromServer, changesFromLocal);


            var saveServerChangesTask = TrySaveServerChangesToLocal(changesFromServer, lastUpdatedOnServer);
            var saveLocalChangesTask = TrySaveLocalChangesToServer(changesFromLocal);
            var saveConflictIdsTask = SaveConflictIds(conflictIds);

            await Task.WhenAll(saveLocalChangesTask, saveServerChangesTask,saveConflictIdsTask);

            if (saveLocalChangesTask.Result && saveServerChangesTask.Result && saveConflictIdsTask.Result)
            {
                return true;
            } else
            {
                return false;
            }

        }

        /// <summary>
        /// Populates the Updates list with all updates from Local (or Server if Local unavailable)
        /// </summary>
        public async Task<List<T>> GetAllUpdates()
        {
            List<Guid>? ids = null;
            return await GetAllUpdates(ids);
        }
        

        /// <summary>
        /// Populates the Updates list with all of the Updates matching Id from Local (or Server if Local unavailable)
        /// </summary>
        public async Task<List<T>> GetAllUpdates(Guid id)
        {
            return await GetAllUpdates(new List<Guid> { id });
        }
        /// <summary>
        /// Populates the Updates list with all of the Updates matching Id from Local (or Server if Local unavailable)
        /// </summary>
        public async Task<List<T>> GetAllUpdates(List<Guid>? ids = null)
        {
            List<T> output = new List<T>();

            TrySync();

            try
            {
                if (_localDataAccess != null) { output = await _localDataAccess.GetFromLocal(ids); }
            }
            catch (Exception)
            {
                if (_serverDataAccess != null) { output = await _serverDataAccess.GetFromServer(ids); }
            }

            return output;
        }








        //Private Methods
        /// <summary>
        /// Tries to Save Updates to Local. If it fails returns false.
        /// </summary>
        private async Task<bool> TrySaveToLocal(List<T> updates)
        {
            try
            {
                await _localDataAccess.SaveToLocal(updates);
                return true;
            }
            catch
            {
                return false;
                //TODO Try Log Save To Local Error
            }
        }
        /// <summary>
        /// Tries to Save Updates to Server. If it fails returns false.
        /// </summary>
        private async Task<bool> TrySaveToServer(List<T> updates)
        {
            try
            {
               DateTime updatedOnServer = await _serverDataAccess.SaveToServer(updates);
               foreach(T t in updates)
                {
                    t.UpdatedOnServer = updatedOnServer;
                }
                return true;
            }
            catch
            {
                return false;
                //TODO Try Log Save to Server Error
            }
        }

        /// <summary>
        /// Finds changesFromServer with the same Id as changesFromLocal. For each conflicted Id it generates a ConflictID
        /// </summary>
        public async Task<List<Conflict>> FindConflicts(List<T> changesFromServer, List<T> changesFromLocal)
        {

            List<Conflict> output = new List<Conflict>();

            List<T> conflictedUpdates = new List<T>();

            await Task.Run(() =>
            {
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

            });

            return output;
        }
        /// <summary>
        /// Saves the ConflictID associates with it to each object with matching Id on both server and local.
        /// </summary>
        public async Task<bool> SaveConflictIds(List<Conflict> conflicts)
        {
            try
            {
                await _localDataAccess.SaveConflictIdsToLocal(conflicts);
                await _serverDataAccess.SaveConflictIdsToServer(conflicts);
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
        private async Task<bool> TrySaveServerChangesToLocal(List<T> changesFromServer, DateTime lastUpdatedOnServer)
        {
            try
            {
                await _localDataAccess.SaveToLocal(changesFromServer);
                try
                {
                    await _localDataAccess.SaveLocalLastSyncDate(lastUpdatedOnServer);
                }
                catch (Exception ex)
                {
                    await _localDataAccess.DeleteFromLocal(changesFromServer);
                    throw new NotImplementedException("Need to log this error.", ex); //TODO Add error logging to sync.
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
        private async Task<bool> TrySaveLocalChangesToServer(List<T> changesFromLocal)
        {
            DateTime? newUpdatedOnServer = null;

            try
            {
                newUpdatedOnServer = await _serverDataAccess.SaveToServer(changesFromLocal);
                try
                {
                    if (newUpdatedOnServer != null)
                    {
                        await _localDataAccess.SaveUpdatedOnServerToLocal(changesFromLocal, (DateTime)newUpdatedOnServer);
                    };
                }
                catch (Exception ex)
                {
                    await _serverDataAccess.DeleteFromServer(changesFromLocal);
                    throw new NotImplementedException("Failed to Save Updated to local storage. Changes from server have been rolled back.", ex);
                    
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

