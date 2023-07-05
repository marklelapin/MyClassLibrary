using MyExtensions;

namespace MyClassLibrary.ChartJs
{
    public class Coordinate
    {
        /// <summary>
        /// The x coordinate.
        /// </summary>
        public double x { get; set; }
        /// <summary>
        ///  The y coordinate.
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Bubble radius in pixels
        /// </summary>
        public double? r { get; set; }

        public Coordinate(double x, double y, double? r = null)
        {
            this.x = x;
            this.y = y;
            this.r = r;
        }

        public Coordinate(string xdate, double y, double? r = null)
        {
            this.x = xdate.ToJavascriptTimeStamp();
            this.y = y;
            this.r = r;
        }

        public Coordinate(double x, string ydate, double? r = null)
        {
            this.x = x;
            this.y = ydate.ToJavascriptTimeStamp();
            this.r = r;
        }


        public Coordinate(DateTime xdate, double y, double? r = null)
        {
            this.x = xdate.ToJavascriptTimeStamp();
            this.y = y;
            this.r = r;
        }

        public Coordinate(double x, DateTime ydate, double? r = null)
        {
            this.x = x;
            this.y = ydate.ToJavascriptTimeStamp();
            this.r = r;
        }

    }
}
