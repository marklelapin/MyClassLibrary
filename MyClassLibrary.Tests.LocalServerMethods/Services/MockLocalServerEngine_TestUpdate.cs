using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Interfaces;
using MyClassLibrary.Tests.LocalServerMethods.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.Tests.LocalServerMethods.Services
{
    internal class MockLocalServerEngine_TestUpdate : IMockLocalServerEngine<TestUpdate>, ILocalServerEngine<TestUpdate>
    {

        private List<TestUpdate> LocalSampleData { get; set; }
        public List<TestUpdate> ServerSampleData { get; set; }


        public MockLocalServerEngine_TestUpdate()
        {
            LocalSampleData = TestContent.LocalStartingData;
            ServerSampleData = TestContent.ServerStartingData;
        }

      
        public async Task<bool> ClearConflictIds(Guid Id)
        {
            return await ClearConflictIds(new List<Guid> { Id });
        }

        public async Task<bool> ClearConflictIds(List<Guid> Ids)
        {
            await Task.Run(() =>
                            {
                                LocalSampleData.Where(x => Ids.Contains(x.Id)).ToList().ForEach(update =>
                            {
                                update.IsConflicted = false;
                            });
                            });
            return true;
        }

        public async Task<List<TestUpdate>> GetAllUpdates()
        {
            List<Guid>? ids = null;
            return await GetAllUpdates(ids);
        }

        public async Task<List<TestUpdate>> GetAllUpdates(Guid id)
        {
            return await GetAllUpdates(new List<Guid>() { id });
        }

        public async Task<List<TestUpdate>> GetAllUpdates(List<Guid>? ids)
        {
            List<TestUpdate> output = new List<TestUpdate>();
            await Task.Run(() =>
            {

                if (ids == null)
                {
                    output = LocalSampleData;
                }
                else
                {
                    output = LocalSampleData.Where(x => ids.Contains(x.Id)).ToList();
                }
            });
                
            return output; 
        }

        public async Task<List<TestUpdate>> GetConflictedUpdates()
        {
            List<Guid>? ids = null; 
            return await GetConflictedUpdates(ids); 
        }

        public async Task<List<TestUpdate>> GetConflictedUpdates(Guid id)
        {
            return await GetConflictedUpdates(new List<Guid> { id });   
        }

        public async Task<List<TestUpdate>> GetConflictedUpdates(List<Guid>? ids)
        {
            List<TestUpdate> output = new List<TestUpdate>();

            await Task.Run(() =>
            {
                if (ids == null)
                {
                    output = LocalSampleData.Where(x => x.IsConflicted = true).ToList();
                } else
                {
                    output = LocalSampleData.Where(x => ids.Contains(x.Id) && (x.IsConflicted = true)).ToList();
                }
            });

            return output;
        }

        public async Task<List<TestUpdate>> GetLatestUpdates()
        {
            List<Guid>? ids = null;
            return await GetLatestUpdates(ids);
        }

        public async Task<List<TestUpdate>> GetLatestUpdates(Guid id)
        {
            return await GetLatestUpdates(new List<Guid>() { id }); 
        }

        public async Task<List<TestUpdate>> GetLatestUpdates(List<Guid>? ids)
        {
            List<TestUpdate> output = new List<TestUpdate>();
            await Task.Run(() =>
            {
                if (ids == null)
                {
                    ids = LocalSampleData.Select(x => x.Id).Distinct().ToList();
                }

                output = LocalSampleData.Where(x=>ids.Contains(x.Id))
                                        .GroupBy(x => x.Id)
                                        .Select(g => g.OrderByDescending(x => x.Created).First())
                                        .ToList();
            });

            return output;


        }

        public async Task SaveUpdates(TestUpdate update, bool syncAfterwards = true)
        {
            await Task.Run(() => LocalSampleData.Add(update));
        }

        public async Task SaveUpdates(List<TestUpdate> updates, bool syncAfterwards = true)
        {
            await Task.Run(() =>
            {
                updates.ForEach(update =>
                {
                    LocalSampleData.Add(update);
                });
            });
        }

        public async Task<(DateTime? syncedDateTime, bool success)> TrySync()
        {
            DateTime outputSyncDateTime = DateTime.Now;
            bool outputSuccess = true;
            (DateTime? syncedDateTime, bool success) output = (DateTime.UtcNow, true);
            await Task.Run(()=> { output = (outputSyncDateTime, outputSuccess); });

            return (output.syncedDateTime,output.success);
        }



       public void ChangeLocalDataAccess(ILocalDataAccess<TestUpdate> localDataAccess)
        {
            throw new NotImplementedException();
        }

        public void ChangeServerDataAccess(IServerDataAccess<TestUpdate> serverDataAccess)
        {
            throw new NotImplementedException();
        } 
   
    }
}
