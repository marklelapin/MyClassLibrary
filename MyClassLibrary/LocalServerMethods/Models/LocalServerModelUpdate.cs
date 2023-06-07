using Microsoft.VisualBasic.FileIO;
using MyClassLibrary.Interfaces;
using MyClassLibrary.LocalServerMethods.Interfaces;
using MyExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MyClassLibrary.LocalServerMethods.Models
{

    /// <summary>
    /// Gives objects inherting from it an Id and LocalTempID and functionality to synchronise between local and server versions for working offline.
    /// </summary>
    /// 
    public class LocalServerModelUpdate : IHasId<Guid>, ILocalServerModelUpdate
    {
        /// <summary>
        /// The Id of the model that the update relates to.
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// An Id indicating a conflict between this and another object with same Id save at different times locally. This is identifed and created when Syncing.
        /// </summary>
        public bool IsConflicted { get; set; } = false;

        /// <summary>
        /// The DateTime the update was saved to local/server.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.MinValue;
        /// <summary>
        /// The DateTime the update was saved onto the Server
        /// </summary>
        public DateTime? UpdatedOnServer { get; set; }
        /// <summary>
        /// The user who created the update.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;
        /// <summary>
        /// Whether or not the update is generally available or not.
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Indicates whether the update is sample data (used for testing and/or providing samples)
        /// </summary>
        public bool IsSample { get; set; } = false;

        public LocalServerModelUpdate()
        {
        }

        public LocalServerModelUpdate(Guid id)
        {
            Id = (Guid)id;
        }

        public LocalServerModelUpdate(Guid id, DateTime? created = null, string? createdBy = null, DateTime? updatedOnServer = null,bool isConflicted = false, bool isActive = true,bool isSample = false)
        {
            Id = id;
            Created = created ?? DateTime.MinValue;
            CreatedBy = createdBy ?? string.Empty;
            IsConflicted = isConflicted;
            UpdatedOnServer = updatedOnServer;
            IsActive = isActive;
            IsSample = isSample;    
        }
    }
}