namespace MyClassLibrary.Extensions
{
    public static class NumberExtensions
    {
        public static DateTime JavascriptTicksToDate(this Int64 javascriptTicks)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(javascriptTicks);
        }


        public static DateTime JavascriptTicksToDate(this double javascriptTicks)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(javascriptTicks);
        }

    }
}