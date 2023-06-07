using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods.Models
{
    public class ServerSyncLog
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
        /// The Id of the particular local copy of data.
        /// </summary>
        public Guid CopyId { get; set; } = Guid.Empty;

        public ServerSyncLog(Guid id, DateTime created, Guid copyId)
        {
            Id = id;
            Created = created;
            CopyId = copyId;
        }

    }

}
