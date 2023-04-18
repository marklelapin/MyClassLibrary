using Microsoft.VisualBasic.FileIO;
using MyClassLibrary.Interfaces;
using MyExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyClassLibrary.LocalServerMethods
{

    /// <summary>
    /// Gives objects inherting from it an Id and LocalTempID and functionality to synchronise between local and server versions for working offline.
    /// </summary>
    /// 
    public class LocalServerIdentity : IHasId<Guid>
    {
        public Guid Id { get; set; }

        public Guid ConflictID { get; set; }

        public DateTime Created { get; set; }

        public DateTime? UpdatedOnServer { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public bool IsActive { get; set; }


        

        public List<T> History<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public T Latest<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Sync<T>(List<T> objects) where T : LocalServerIdentity
        {
            throw new NotImplementedException();
        }

        public LocalServerIdentity(Guid? id = null, bool isActive = true)
        {
            Id = id ?? new Guid();
            if (id == null)
            {
                IsActive = isActive;
            }
            Created = DateTime.UtcNow;
            //CreatedBy TODO: add CreatedBy to LocalServerIdentity constructor
        }
    }
}