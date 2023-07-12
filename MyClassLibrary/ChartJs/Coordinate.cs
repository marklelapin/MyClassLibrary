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


        /// <summary>
        /// An Id that can be used by event handlers.
        /// </summary>
        public string? Id { get; set; }


        /// <summary>
        /// The lable to be shown on tooltip or specific datapoint.
        /// </summary>
        public string? Label { get; set; }

        public Coordinate(double x, double y, double? r = null, string? label = null, string? id = null)
        {
            this.x = x;
            this.y = y;
            this.r = r;
            this.Label = label;
            this.Id = id;
        }

        public Coordinate(string xdate, double y, double? r = null, string? label = null, string? id = null)
        {
            this.x = xdate.ToJavascriptTimeStamp();
            this.y = y;
            this.r = r;
            this.Label = label;
            this.Id = id;
        }

        public Coordinate(double x, string ydate, double? r = null, string? label = null, string? id = null)
        {
            this.x = x;
            this.y = ydate.ToJavascriptTimeStamp();
            this.r = r;
            this.Label = label;
            this.Id = id;
        }


        public Coordinate(DateTime xdate, double y, double? r = null, string? label = null, string? id = null)
        {
            this.x = xdate.ToJavascriptTimeStamp();
            this.y = y;
            this.r = r;
            this.Label = label;
            this.Id = id;
        }

        public Coordinate(double x, DateTime ydate, double? r = null, string? label = null, string? id = null)
        {
            this.x = x;
            this.y = ydate.ToJavascriptTimeStamp();
            this.r = r;
            this.Label = label;
            this.Id = id;
        }

    }
}
