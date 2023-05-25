
using System.Text.Json.Serialization;


namespace MyClassLibrary.LocalServerMethods.Models

{
    public class Conflict
    {
        public Guid? ConflictId { get; set; }

        public Guid UpdateId { get; set; }

        public DateTime UpdateCreated { get; set; }

        [JsonConstructor]
        public Conflict(Guid updateId, DateTime updateCreated, Guid? conflictId = null)
        {
            UpdateId = updateId;
            UpdateCreated = updateCreated;
            if (conflictId != null)
            {
                ConflictId = conflictId;
            }

        }

    }
}
