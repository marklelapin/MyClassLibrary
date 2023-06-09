using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods.Models
{
    internal class SQLObjectData<T> where T : LocalServerModelUpdate
    {
        internal List<T> ObjectData { get; set; } = new List<T>();

        [JsonConstructor]
        internal SQLObjectData(List<T> objectData)
        {
            ObjectData = objectData;

        }
         
    }
}
