using Microsoft.Identity.Client;
using MyClassLibrary.LocalServerMethods.Interfaces;


namespace MyClassLibrary.LocalServerMethods.Models
{
    //TODO - Unit Test Functionallity of LocalServerIdentity
    public class LocalServerModel<T> : ILocalServerModel<T> where T : LocalServerModelUpdate, new()
    {
        public ILocalServerEngine<T>? _localServerEngine { get; set; }

        /// <summary>
        /// The Identity of the LocalServerIdentity
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

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
        public List<T>? Conflicts { get; set; }
        /// <summary>
        /// All updates relating to the LocalServerIdentity
        /// </summary>
        public List<T>? History { get; set; }


        public LocalServerModel()
        {
        
        }

        /// <summary>
        /// Updates the LocalServerIdentity with the update passed in.
        /// </summary>
        public async Task Update(T update)
        {
            if (_localServerEngine == null)
            {
                throw new InvalidOperationException("Local Server Engine Unknown to Method. LocalServerModel needs to be created by LocalServerModelFactory");
            }
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

        



    }

}

