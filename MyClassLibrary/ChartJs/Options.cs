namespace MyClassLibrary.ChartJs
{
    public class Options
    {

        public Elements elements { get; set; } = new Elements();

        public Scales scales { get; set; } = new Scales();

        public Plugins plugins { get; set; } = new Plugins();


        /// <summary>
        /// The name of the function in scripts that will handle the click event.
        /// </summary>
        public string? onClick { get; set; }

        /// <summary>
        /// Determines whether the chart resizes when its container does.
        /// </summary>
        public bool? responsive { get; set; }

        /// <summary>
        /// Maintains the original canvas aspect ratio when resizing.
        /// </summary>
        public bool? maintainAspectRatio { get; set; }


        /// <summary>
        /// The aspect ratio (width/height) of the chart.
        /// </summary>
        public double? aspectRatio { get; set; }


    }


}
