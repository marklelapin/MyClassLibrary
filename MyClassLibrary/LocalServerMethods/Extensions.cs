using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClassLibrary.LocalServerMethods
{
    public static class Extensions
    {

        public static string GetUpdateType<T>(this T obj) where T : LocalServerIdentityUpdate
        {
            string output;

            output = typeof(T).Name;

            if (!output.Contains("Update"))
            {
                throw new ArgumentException("The given LocalServerIdentityUpdate must follow the convention of class name in the form {type}Update.");
            }
            return output.Replace("Update", "");
        }
    }
}
