using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyExtensions
{
    public static class StringExtensions
    {

        static Dictionary<string, bool> stringBooleans = new Dictionary<string, bool>()
        {
            {"Yes", true },
            {"yes", true},
            {"YES", true},
            {"Y", true },
            {"y", true },
            {"True", true},
            {"true", true },
            {"TRUE", true},
            {"Yep", true },
            {"yep", true },
            {"1", true },

            {"No", false },
            {"no", false},
            {"NO", false },
            {"N", false },
            {"n", false },
            {"False", false},
            {"false", false },
            {"FALSE", false},
            {"Nope", false },
            {"nope", false },
            {"0", false}
        };
        
        /// <summary>
        /// Converts string to boolean from a broad range of strings. Returns null if can't convert.
        /// </summary>
        /// <param name="str">A string such as "y", "yes", "nope","0","1" etc.</param>
        /// <returns></returns>
        public static bool? ToBool(this string str) 
        {
            if (stringBooleans.ContainsKey(str) ) return stringBooleans[str];
            return null;
        }

        public static List<Guid> ToListGuid(this string str,string separator = ",")
        {
            List<Guid> output;
            
            try
            {
                output = str.Split(separator).Select(s => Guid.Parse(s)).ToList();
            }
            catch
            {
                throw new ArgumentException("Can't convert string to List<Guid> as the list contains Guids that aren't valid.");
            }
        
            return output;
        
        }
    }
}
