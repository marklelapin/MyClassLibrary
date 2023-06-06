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
    /// Model of the data postedBack after a save to Local
    /// </summary>
    /// 
    public class ServerToLocalPostBack
    {
        /// <summary>
        /// The Id of the model that the update relates to.
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// The DateTime the update was saved to local.
        /// </summary>
        public DateTime Created { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Identifies if the save to Local identified a conflict
        /// </summary>
        public bool IsConflicted { get; set; } = false;
        /// <summary>
        /// The DateTime the update was saved onto the Server
        /// </summary>
        public DateTime? UpdatedOnServer { get; set; }

        public ServerToLocalPostBack() { }

        public ServerToLocalPostBack(Guid id,DateTime created,bool isConflicted,DateTime updatedOnServer)
        {
            Id = id;
            Created = created;
            IsConflicted = isConflicted;
            UpdatedOnServer = updatedOnServer;
        }



    }

}