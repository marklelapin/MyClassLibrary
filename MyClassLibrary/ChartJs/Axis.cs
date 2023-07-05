namespace MyClassLibrary.ChartJs
{
    public class Axis
    {
        /// <summary>
        /// Type of scale being employed. Custom scales can be created and registered with a string key. This allows changing the type of an axis for a chart.
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// Align pixel values to device pixels.
        /// </summary>
        public bool? alignToPixels { get; set; }

        /// <summary>
        ///Background color of the scale area.
        /// </summary>
        public string? backgroundColor { get; set; }

        /// <summary>
        /// Border Configuration
        /// </summary>
        public Border? border { get; set; }

        /// <summary>
        /// Whether or not to display the axis. ('true','false' or 'auto'). Auto displays if values exist. 
        /// </summary>
        public string? display { get; set; }

        /// <summary>
        /// Grid Configuration
        /// </summary>
        public Grid? grid { get; set; }

        public string? min { get; set; }

        public string? max { get; set; }

        /// <summary>
        /// Adjustment used when calculating the maximum value. the axes will use this if values do not exceed the max.
        /// </summary>
        public string? suggestedMax { get; set; }

        /// <summary>
        /// Adjustment used when calculating the minimum value. The axes will not go below this minimum value.
        /// </summary>
        public string? suggestedMin { get; set; }

        /// <summary>
        /// Reverses the Scale
        /// </summary>
        public bool? reverse { get; set; }

        /// <summary>
        /// 'true' will stack positve and negative values separately, single combine the two, false(default) won't stack data.
        /// </summary>
        public string? stacked { get; set; }

        /// <summary>
        /// Tick Configuration
        /// </summary>
        public Ticks? ticks { get; set; }

        /// <summary>
        /// The weight used to sort the axis. Higher weights are further away from the chart area.
        /// </summary>
        public int? weight { get; set; }


        public Axis()
        {

        }

        public Axis(Double min, Double max)
        {
            this.min = min.ToString();
            this.max = max.ToString();

        }

        public Axis(string min, string max)
        {
            this.min = min;
            this.max = max;

        }

    }
}
