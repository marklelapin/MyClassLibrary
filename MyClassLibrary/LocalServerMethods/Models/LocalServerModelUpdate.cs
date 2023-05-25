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
    public class LocalServerModelUpdate : IHasId<Guid>
    {
        
        public Guid Id { get; set; }
        /// <summary>
        /// An Id indicating a conflict between this and another object with same Id save at different times locally. This is identifed and created when Syncing.
        /// </summary>
        /// [JsonInclude]
        
        public Guid? ConflictId { get; set; }

        /// <summary>
        /// The DateTime the object was created with the application.
        /// </summary>

        
        public DateTime Created { get; set; } = DateTime.MinValue;
        /// <summary>
        /// The DateTime the object was saved onto the Server
        /// </summary>
       
        public DateTime? UpdatedOnServer { get; set; }
        /// <summary>
        /// The user who created the object.
        /// </summary>

        public string CreatedBy { get; set; } = string.Empty;
        /// <summary>
        /// Whether or not the object is generally available or not.
        /// </summary>
        
        public bool IsActive { get; set; }

        public LocalServerModelUpdate()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }

        public LocalServerModelUpdate(Guid id)
        {
            Id = (Guid)id;
            Created = DateTime.UtcNow;
            IsActive = true;
        }

        public LocalServerModelUpdate(Guid id,Guid? conflictId,DateTime created,DateTime? updatedOnServer,string? createdBy,bool? isActive)
        {
            Id = id;
            ConflictId = conflictId;
            Created = created;
            UpdatedOnServer = updatedOnServer; 
            CreatedBy = createdBy ?? string.Empty;
            IsActive = isActive ?? false;
        }
    }
}