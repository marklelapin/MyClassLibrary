

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
    public class LocalServerIdentity<T> where T : LocalServerIdentityUpdate
    {
        /// <summary>
        /// The Identity of the LocalServerIdentity
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The Latest update to the LocalServerIdentity.
        /// </summary>
        /// <return>
        /// Returns null if the latest update is deactivated.
        /// </return>
        public T? Latest { get; set; }
        /// <summary>
        /// All the updates with the same conflictId as the latest update for the LocalServerIdentity
        /// </summary>
        public List<T> Conflicts { get; set; } = new List<T>();
        /// <summary>
        /// All updates relating to the LocalServerIdentity
        /// </summary>
        public List<T> History { get; set; } = new List<T>();

        /// <summary>
        /// The enging containing all LocalServerIdentity processes.
        /// </summary>
        private ILocalServerEngine<T> _localServerEngine;

        public ILocalServerEngine<T> LocalServerEngine
        {
            get { return _localServerEngine; }
            set { _localServerEngine = value; }
        }


        public LocalServerIdentity(ILocalServerEngine<T> localServerEngine,Guid? id = null)
        {
            _localServerEngine = localServerEngine;
            Id = id ?? Guid.NewGuid();
        }

        /// <summary>
        /// Updates the LocalServerIdentity with the update passed in.
        /// </summary>
        public async Task Update(T update)
        {
            update.Created = DateTime.UtcNow;
            update.UpdatedOnServer = null;

            await _localServerEngine.Save(update);

            Latest = update;

            History = History ?? new List<T>().Prepend(update).ToList();
        }
        /// <summary>
        /// Removes the conflictId from the update passed in and makes this the chosen update going forward.
        /// </summary>
        public async Task ResolveConflict(T update)
        {
            update.ConflictId = null;
            Conflicts.Clear();
            await Update(update);
            
        }
        /// <summary>
        /// Makes the LocalServerIdentity InActive by creating a new update with IsActive = false.
        /// </summary>
        /// <returns></returns>
        public async Task DeActivate()
        {
            T update = Latest!;
            update.IsActive = false;
            await Update(update);
        }
        /// <summary>
        /// Adds a new update to the LocalServerIdentity using the update passed in and making it IsActive = true.
        /// </summary>
        public async Task ReActivate(T update)
        {
            update.IsActive = true;
            await Update(update);
        }
        /// <summary>
        /// Adds a new update to the LocalServerIdentity using the update passed in and making it IsActive = true.
        /// </summary>
        public async Task Restore(T update)
        {
            update.IsActive = true;
            await Update(update);
        }

        /// <summary>
        /// Populates the properties of the LocalServerIdentity
        /// </summary>
        /// <returns></returns>
        public async Task AddDataAsync()
        {
            var addHistory = AddHistory();
            Task.WaitAll(addHistory);
            var addLatest = AddLatest();
            Task.WaitAll(addLatest);
            await AddConflicts();
        }
        /// <summary>
        /// Adds all updates relating to the Id into History property
        /// </summary>
        /// <returns></returns>
        private async Task AddHistory()
        {
            History = await _localServerEngine.GetAllUpdates(Id);
        }
        /// <summary>
        /// Adds the latest update relating to the Id into the Latest property and also populates Conflicts if present.
        /// </summary>
        /// <returns></returns>
        private async Task AddLatest()
        {
            var listLatest = await _localServerEngine.FilterLatest(History);
            Latest = listLatest.FirstOrDefault();
        }
        /// <summary>
        /// Adds any conflicts to the Conflicts property that have the same conflictId as Latest update.
        /// </summary>
        /// <returns></returns>
        public async Task AddConflicts()
        {
            await Task.Run(() =>
            {
                if (Latest?.ConflictId != null)
                {
                    Conflicts = History.Where(x => x.ConflictId.Equals(Latest?.ConflictId)).ToList();
                }
            });
        }



    }
            
}

