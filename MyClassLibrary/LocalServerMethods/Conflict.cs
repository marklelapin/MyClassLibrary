using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public class Conflict
    {
        public Guid? ConflictId { get; set; }

        public Guid UpdateId { get; set; }

        public DateTime UpdateCreated { get; set; }

        [JsonConstructor]
        public Conflict(Guid updateId,DateTime updateCreated,Guid? conflictId = null)
        {
            this.UpdateId = updateId;
            this.UpdateCreated = updateCreated;
            if (conflictId != null )
            {
                ConflictId = conflictId;
            }
                
        }

    }
}
