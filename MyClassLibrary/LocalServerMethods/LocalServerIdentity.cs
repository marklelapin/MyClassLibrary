

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
        /// A List of all Updates relating to Id
        /// </summary>
        public  List<T> Updates { get; private set; }

        /// <summary>
        /// The enging containing all LocalServerIdentity processes.
        /// </summary>
        private ILocalServerEngine<T> _localServerEngine;

        public LocalServerIdentity(LocalServerEngine<T> localServerEngine,Guid? id = null)
        {
            _localServerEngine = localServerEngine;

            if (id == null)
            {
                Id = Guid.NewGuid();
                Updates = new List<T>();
            } else
            {
                Id = (Guid)id;
               Updates = _localServerEngine.GetAllUpdates(Id);
                _localServerEngine.SortByCreated(Updates);
            }
        }

        /// <summary>
        /// Returns the latest update relating to Id. Can return multiple updates if conflicts exist.
        /// </summary>
        public List<T> Latest()
        {
            return _localServerEngine.FilterLatest(Updates);
        }
        
        public void SaveUpdate(T update)
        {
            _localServerEngine.Save(update);
        }
}
            
}

