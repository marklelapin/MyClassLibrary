using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public class Conflict
    {
        public Guid? ConflictId { get; set; }

        public Guid ObjectId { get; set; }

        public DateTime ObjectCreated { get; set; }


        public Conflict(Guid objectId,DateTime objectCreated,Guid? conflictID = null)
        {
            this.ObjectId = objectId;
            this.ObjectCreated = objectCreated;
            if (conflictID != null )
            {
                ConflictId = conflictID;
            }
                
        }

    }
}
