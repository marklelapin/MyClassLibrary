
namespace MyClassLibrary.Colors
{
    public static class MyColors
    {
        public static string TrafficOrange(double? transparency = 1)
        {
            return $"rgba(255,102,0,{transparency})";
        }

        public static string TrafficGreen(double? transparency = 1)
        {
            return $"rgba(102,153,0,{transparency})";
        }

        public static string TrafficRed(double? transparency = 1)
        {
            return $"rgba(255,0,0,{transparency})";
        }

        public static string TrafficOrangeRed(double? transparency = 1)
        {
            return $"rgba(255,60,0,{transparency})";
        }

        public static string OffWhite(double? transparency = 1)
        {
            return $"rgba(220,220,220,{transparency})"; //Gainsboro
        }

        public static string Transparent()
        {
            return ("rgba(0,0,0,0)");
        }
    }

}
