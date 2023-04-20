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

        public Guid? ConflictID { get; set; }

        public DateTime Created { get; set; }

        public DateTime? UpdatedOnServer { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        public bool IsActive { get; set; }

       public LocalServerIdentity(Guid? id = null)
            {
                
            Id = id ?? new Guid();
            
                IsActive = true;
                Created = DateTime.UtcNow;
                //CreatedBy TODO: add CreatedBy to LocalServerIdentity constructor
            }

        public List<T> GetHistory<T>() where T : LocalServerIdentity
        {
            LocalServerIdentityList<T> output = new LocalServerIdentityList<T>();
                
            output.PopulateObjects(Id);
            output.SortByIdAndCreated();

            return output.Objects;
        }


        public void Save<T>(List<T> objects) where T : LocalServerIdentity
        {
            LocalServerIdentityList<T> List = new LocalServerIdentityList<T>(objects);

            List.Save();
        }

     
    }
}