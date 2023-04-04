using Microsoft.VisualBasic.FileIO;
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
    public class LocalServerIdentity
    {
        public Guid Id { get; set; }

        public Guid ConflictID { get; set; }

        public DateTime UpdatedLocally { get; set; }

        public DateTime UpdatedOnServer { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        ILocalServerDataAccess _localServerDataAccess;

        public void SaveToLocalAndServer<T>(List<T> objects)
        {
           throw new NotImplementedException();
        }

        public void SaveLocalChangesToMainAndServer<T>(List<T> objects)
        {
            throw new NotImplementedException();
        }

        public void Sync<T>()
        {

        }

        public void DeleteDirectoryOption<T>(Guid id, DateTime updatedLocally)
        {

        }

        public List<T> GetHistory<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetLatestUpdate<T>(List<Guid> Ids)
        {
            throw new NotImplementedException();
        }

        private List<T> GetConflicts<T>(List<T> objects)
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
        }

    }
}