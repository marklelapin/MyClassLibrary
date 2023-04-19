

using MyClassLibrary.Interfaces;
using MyClassLibrary.LocalServerMethods;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;

namespace MyClassLibrary.LocalServerMethods
{
    public class LocalServerIdentityList<T> where T : LocalServerIdentity
    {

        private  ILocalDataAccess _localDataAccess;
        private  IServerDataAccess _serverDataAccess;

        public List<T> Objects { get; private set; }  

        public LocalServerIdentityList (List<T>? objects = null,IServerDataAccess? serverDataAccess = null, ILocalDataAccess? localDataAccess = null)
        {
           
                _serverDataAccess = serverDataAccess ?? new ServerSQLConnector("Error");
                _localDataAccess = localDataAccess ?? new LocalSQLConnector("Error");  
            
               Objects = objects ?? new List<T>(); 
        }
  



        public void SortById()
        {
            Objects.Sort((x, y) => x.Id.CompareTo(y.Id));
        }

        public void SortByCreated()
        {
            Objects.Sort((x, y) => x.Created.CompareTo(y.Created));
        }

        public void Save()
        {
           
         if (_localDataAccess == null || _serverDataAccess == null )
            {
                throw new InvalidOperationException("Services need to be set up for both ILocalDataAccess and IServerDataAccess");
            }
         
            bool WasSyncSuccessful = TrySync();
            
            if (WasSyncSuccessful == true)
            {
               if (HasConflict())
               {
                    throw new NotImplementedException(); //HaveConflict
                    //2. check for conflicts - are there any lines locally with
                    //a locally updated date greater than updated on server date
                     //       AND
                     //       updatedonServerDate less than maxupdated on server date for that ID.

                    //if ther are conflicts end the method here.
               } else
                {
                    try
                    {
                        if (_serverDataAccess != null) {_serverDataAccess.SaveToServer(Objects);} 
                    }
                    catch (Exception)
                    {
                        //Do Nothing. The system will go on to save locally if it can't reach the server
                        //and sync at a later date.
                        //TODO pick up more specific error on SAve to Server if not to do with connection?
                    }
                }
            }

            try
            {
                if (_localDataAccess != null) { _localDataAccess.SaveToLocal(Objects); }
            }
            catch (Exception e)
            {
                if (WasSyncSuccessful==true)
                {
                    //DoNothing. It's saved to the server so the local can be updated at a later date
                } else
                {
                    throw new Exception("Failed to Save Locally or To Server",e);
                }
            }     
        }
       
        public void Delete()
        {
            throw new NotImplementedException(); //TODO Delete function (not entirely sure if this is necessary? should it be against 
        }

        public List<T> History()
        {
            List<T> output = new List<T>();

            Objects.Sort((x, y) =>
            {
                int result = x.Id.CompareTo(y.Id);

                if (result == 0)
                {
                    result = x.Created.CompareTo(y.Created);
                }
                return result;
            });
            
            output = Objects;

            return output;
        }



        public List<T> Latest()
        {
            List<T> output;

            var filter = Objects.GroupBy(x => x.Id).Select(g=> new { Id = g.Key, Created = g.Max(x => x.Created) }).ToList();

            output = (List<T>)(from o in Objects
                                join f in filter
                                on new { o.Id, o.Created } equals new { f.Id, f.Created }
                                select o);

            output = output.Where(x=>x.IsActive == true).ToList();  
            
            //TODO ADD IN ANY CONFLICTS THAT EXIST

            return output;
        }

        public static bool HasConflict()
        {
            throw new NotImplementedException(); //TODO HasConflict
        }

        public static int AddConflictIds(List<T> serverChanges)
        {
            //Find objects in the list where Updated
            throw new NotImplementedException(); //TODO AddConflictIDs
           // List<Guid> conflictedIds = objects.GroupBy(x => x.Id).Where(x => x.Count() < 1).ToList().Where();
        }

        public bool TrySync()
        {
            List<T> changesFromServer = new List<T>();
            DateTime lastUpdatedOnServer = DateTime.MinValue;
            List<T> changesFromLocal = new List<T>();
            DateTime newUpdatedOnServer = DateTime.MinValue;

            try
            {
                DateTime lastSyncDate = _localDataAccess.GetLocalLastSyncDate<T>();

                (changesFromServer,lastUpdatedOnServer) = _serverDataAccess.GetChangesFromServer<T>(lastSyncDate);
            }
            catch { return false; } //TODO pick up specific errors

            try
            {
               _localDataAccess.SaveToLocal(changesFromServer);
            }
            catch { return false; }

            _localDataAccess.SaveLocalLastSyncDate<T>(lastUpdatedOnServer);

            try
            {
               changesFromLocal =  _localDataAccess.GetChangesFromLocal<T>();
            }
            catch { throw New Exception 
                    
                    return false; }

            try
            {
                newUpdatedOnServer = _serverDataAccess.SaveToServer(changesFromLocal);
            }
            catch { return false; }

            List<Guid> conflictIds = FindConflictedIds(changesFromServer, changesFromLocal);

            try
            {

            }
            catch
            {

            }
            try
            {
                if (newUpdatedOnServer != DateTime.MinValue)
                {
                    _localDataAccess.SaveUpdatedOnServerDate(changesFromLocal, newUpdatedOnServer);
                };
            }
            catch { return false; }


            return true;

        }
    
        public void GetObjects(List<Guid> ids) 
        {
            List<T>? output = new List<T>();
            
            try {
                if (_localDataAccess != null) { output = _localDataAccess.GetFromLocal<T>(ids); }
                }
            catch (Exception)
                {
                if (_serverDataAccess != null) { output = _serverDataAccess.GetFromServer<T>(ids); }
                }

            Objects = output;
        }

        public void GetObjects(Guid id)
        {
            GetObjects(new List<Guid> { id });
        }
           

        public List<Guid> FindConflictedIds(List<T> changesFromServer,List<T> changesFromLocal)
        {
            List<Guid> output = new List<Guid>();

            output.AddRange(changesFromServer.Where(x => x.ConflictID != null).Select(x => x.Id).ToList());

            var joinedList = (
                              from s in changesFromServer
                              join l in changesFromLocal
                              on s.Id equals l.Id
                              select s.Id
                              );

            output.AddRange(joinedList);

            return output;
        }
    }
}

