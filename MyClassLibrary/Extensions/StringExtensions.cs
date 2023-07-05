using MyClassLibrary.ErrorHandling;

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
            if (stringBooleans.ContainsKey(str)) return stringBooleans[str];
            return null;
        }

        public static List<Guid> ToListGuid(this string str, string separator = ",")
        {
            List<Guid> output;

            try
            {
                output = str.Split(separator).Select(s => Guid.Parse(s)).ToList();
            }
            catch (Exception)
            {
                throw new IdentifiedException("Can't convert string to List<Guid> as the list contains Guids that aren't valid.");
            }

            return output;

        }

        /// <summary>
        /// Converts a string of a utc date to JavaScriptTimeStamp
        /// </summary>
        /// <param name="strUtcDate"></param>
        /// <returns></returns>
        public static double ToJavascriptTimeStamp(this string strUtcDate)
        {
            DateTime.TryParse(strUtcDate, out DateTime dt);

            double output = dt.Subtract(new DateTime(19070, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            return output;
        }


    }
}
