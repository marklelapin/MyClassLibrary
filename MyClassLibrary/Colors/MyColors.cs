
namespace MyClassLibrary.Colors
{
    public class MyColors
    {
        public string TrafficOrange(double? transparency = 1)
        {
            return $"rgba(255,102,0,{transparency})";
        }

        public string TrafficGreen(double? transparency = 1)
        {
            return $"rgba(102,153,0,{transparency})";
        }

        public string TrafficRed(double? transparency = 1)
        {
            return $"rgba(255,0,0,{transparency})";
        }

        public string TrafficOrangeRed(double? transparency = 1)
        {
            return $"rgba(255,60,0,{transparency})";
        }


        public string Transparent()
        {
            return ("rgba(0,0,0,0)");
        }
    }

}
