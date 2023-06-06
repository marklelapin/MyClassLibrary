using Microsoft.AspNetCore.Mvc.Abstractions;
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
        /// Returns a new Model of type T populated with the Latest update relating to Id if Id is not null.
        /// </summary>
        /// <returns></returns>
        public async Task<T> CreateModel(Guid? Id = null)
        {
            T model = new T();

            if (Id == null)
            {
                model.Id = Guid.NewGuid();
            }
            else
            {
                model.Id = (Guid)Id;
                await RefreshLatest(model);
            }
            return model;
        }

        public async Task<List<T>> CreateModelsList(List<Guid>? ids = null)
                {
                    List<T> output = new List<T>();

                    List<U> latestUpdates = await _localServerEngine.GetLatestUpdates(ids);
         
                    var taskList = latestUpdates.Select(

                                                 update => AddUpdateToListModel(output,update)

                                                 ).ToList();
            
                    await Task.WhenAll(taskList);
              
                    return output;
      
                }


        public async Task RefreshHistory(T model)
        {
            model.History = await _localServerEngine.GetAllUpdates(model.Id);
        }
        public async Task RefreshHistory(List<T> models)
        {

            var taskList = models.Select(model => RefreshHistory(model)).ToList();
            
            await Task.WhenAll(taskList);
        }



        public async Task RefreshLatest(T model)
        {
            List<U> listLatest = await _localServerEngine.GetLatestUpdates(model.Id) ;

            model.Latest = listLatest.FirstOrDefault();
        }
        public async Task RefreshLatest(List<T> models)
        {
            var taskList = models.Select(async model => await RefreshLatest(model)).ToList();

            await Task.WhenAll(taskList);
        }


        public async Task RefreshConflicts(T model)
        {
            if (model.Latest?.IsConflicted != null)
            {
                model.Conflicts = await _localServerEngine.GetConflictedUpdates(model.Latest.Id);
            } else
            {
                model.Conflicts?.Clear();
            }
        }
        public async Task RefreshConflicts(List<T> models)
        {
            var taskList = models.Select(model => RefreshConflicts(model)).ToList();

            await Task.WhenAll(taskList);
        }



        public async Task Sync()
        {
            await _localServerEngine.TrySync();
        }


        public async Task Update(T model,U update)
        {
            update.Created = DateTime.UtcNow;
            update.UpdatedOnServer = null;

            await _localServerEngine.SaveUpdates(update);

            model.Latest = update;

            model.History = model.History ?? new List<U>().Prepend(update).ToList();
        }
     
        public async Task ResolveConflict(T model,U update)
        {
            await Update(model,update);
            await _localServerEngine.ClearConflictIds(update.Id);
            update.IsConflicted = false;
            model.Conflicts?.Clear();
        }
    
        public async Task DeActivate(T model)
        {
            U? update = model.Latest;
            if (update != null)
            {
                update.IsActive = false;
                await Update(model,update);
            }            
        }

        public async Task ReActivate(T model)
        {
            if (model.Latest != null)
            {
                U update = model.Latest;
                update.IsActive = true;
                await Update(model,update); 
            }
        }
      
        public async Task Restore(T model, U update)
        {
            update.IsActive = true;
            await Update(model, update);
        }

        public async Task DeleteEntirely(List<U> updates,bool areYouSure)
        {
            if (areYouSure)
            {
                await _localServerEngine.DeleteEntirely(updates);
            }
           
        }





        //Helper methods

        /// <summary>
        /// Takes an update, converts it to Model with Latest = update and adds it to the passed in List of Models.
        /// </summary>
        private async Task AddUpdateToListModel(List<T> listModel, U update)
        {
            T model = new T();

            await Task.Run(() =>
            {
                model.Id = update.Id;
                model.Latest = update;
                listModel.Add(model);
            });

        }

       
    }
}
