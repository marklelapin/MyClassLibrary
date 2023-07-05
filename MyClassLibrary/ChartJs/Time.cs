namespace MyClassLibrary.ChartJs
{
    public class Time
    {
        /// <summary>
        /// The unit of time to use in the chart.
        /// </summary>
        /// <remarks>
        /// millisecond <br/>
        /// second <br/>
        /// minute <br/>
        /// hour <br/>
        /// day <br/>
        /// week <br/>
        /// month <br/>
        /// quarter <br/>
        /// year
        /// </remarks>
        public string? unit { get; set; }

        /// <summary>
        /// The formats to be used for different time units.
        /// </summary>
        public DisplayFormats? displayformats { get; set; }

        /// <summary>
        /// The time format to use in tooltips.
        /// </summary>
        public string? tooltipFormat { get; set; }
    }
}
