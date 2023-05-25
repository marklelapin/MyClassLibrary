using MyClassLibrary.LocalServerMethods.Interfaces;
using MyClassLibrary.LocalServerMethods.Models;

namespace MyClassLibrary.LocalServerMethods.Models
{
    public class LocalServerModelFactory<T, U> : ILocalServerModelFactory<T, U> where T : LocalServerModel<U>,new() where U : LocalServerModelUpdate, new()
    {

        private ILocalServerEngine<U> _localServerEngine;

        public LocalServerModelFactory(ILocalServerEngine<U> localServerEngine)
        {
            _localServerEngine = localServerEngine;
        }

        /// <summary>
        /// Populates the properties of the LocalServerIdentity
        /// </summary>
        /// <returns></returns>
        public async Task<T> CreateModel(Guid? Id = null)
        {
            T model = new T();

            model._localServerEngine = _localServerEngine;

            if (Id == null)
            {
                model.Id = Guid.NewGuid();
            }
            else
            {
                model.Id = (Guid)Id;
                Task.Run(() => AddHistory(model)).Wait(); //synchronous as AddLatest requires AddHistory to have run.
                Task.Run(() => AddLatest(model)).Wait(); //sychnronous as 
                await AddConflicts(model);
            }
            return model;
        }
        /// <summary>
        /// Adds all updates relating to the Id into History property
        /// </summary>
        /// <returns></returns>
        private async Task AddHistory(T model)
        {
            model.History = await _localServerEngine.GetAllUpdates(model.Id);
        }
        /// <summary>
        /// Adds the latest update relating to the Id into the Latest property and also populates Conflicts if present.
        /// </summary>
        /// <returns></returns>
        private void AddLatest(T model)
        {
            if (model.History != null)
            {
                var listLatest = _localServerEngine.FilterLatest(model.History);
                model.Latest = listLatest.Result.FirstOrDefault();
            }

        }
        /// <summary>
        /// Adds any conflicts to the Conflicts property that have the same conflictId as Latest update.
        /// </summary>
        /// <returns></returns>
        private async Task AddConflicts(T model)
        {
            if (model.Latest != null && model.History != null)
            {
                await Task.Run(() =>
                   {
                       if (model.Latest?.ConflictId != null)
                       {
                           model.Conflicts = model.History.Where(x => x.ConflictId.Equals(model.Latest?.ConflictId)).ToList();
                       }
                   });
            }

        }



        public async Task<List<T>> CreateModelsList(List<Guid>? ids = null)
        {

            //TODO IMPROVE THE WAY THIS WORKS (THink the asynchronicity can be improve plus do I really want to send history down each time? mayb I do fo undo purposes)
            List<T> output = new List<T>();

            var allUpdates = _localServerEngine.GetAllUpdates(ids);
            allUpdates.Wait();
            var latestUpdates = _localServerEngine.FilterLatest(allUpdates.Result);
            latestUpdates.Wait();

            foreach (var update in latestUpdates.Result)
            {
                T model = new T();
                model.Id = update.Id;
                model.Latest = update;
                model.History = allUpdates.Result.Where(x => x.Id == update.Id).ToList();

                await AddConflicts(model);

                output.Add(model);
            }
            return output;
       
        }
        
    }
}
