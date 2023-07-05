namespace MyExtensions
{
    public static class DateExtensions
    {
        public static int DateDiff(this DateTime date1, DateTime date2, string datePart)
        {
            TimeSpan diff = date1 - date2;

            switch (datePart)
            {
                case "years":
                    {
                        int diffYears = date1.Year - date2.Year;
                        if (date1.AddYears(-diffYears) < date2) diffYears--;
                        Console.WriteLine(diffYears);

                        Console.WriteLine(date1.AddYears(-diffYears));
                        Console.WriteLine(date2);
                        return diffYears;
                    };
                case "months":
                    {
                        int diffMonths = date1.Month - date2.Month;
                        if (date1 < date2.AddMonths(diffMonths)) diffMonths--;
                        return diffMonths;
                    };
                case "days": return diff.Days;
                case "hours": return diff.Hours;
                case "minutes": return diff.Minutes;
                case "seconds": return diff.Seconds;
                case "milliseconds": return diff.Milliseconds;
                default: throw new ArgumentException("value entered for datePart did not correspond with 'years','months','hours','minutes','seconds','milliseconds'");

            }

        }

        /// <summary>
        /// Converts a UTC DateTime to Javascript TimeStamp
        /// </summary>
        public static double ToJavascriptTimeStamp(this DateTime utcDate)
        {
            return utcDate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }



    }
}
