namespace MyClassLibrary.ChartJs
{
    public class Dataset
    {

        /// <summary>
        /// The label for the dataset which appears in the legend and tooltips.
        /// </summary>
        public string? label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object[]? data { get; set; }

        /// <summary>
        /// The drawing order of the dataset. Also affects order for stacking, tooltip and legend
        /// </summary>
        public int? order { get; set; }

        /// <summary>
        /// The ID of the group to which this dataset belongs to (when stacked, each group will be a separate stack). Defaults to dataset type.
        /// </summary>
        public string? stack { get; set; }

        /// <summary>
        /// Configure the visibility of the dataset. Using hidden: true will hide the dataset from being rendered in the Chart.
        /// </summary>
        public bool hidden { get; set; } = false;

        /// <summary
        /// The Border Color as hexidecimal string.
        /// </summary>
        public string? borderColor { get; set; }

        /// <summary>
        /// Teh Background Color as hexidecimal string. 
        /// </summary>
        public string? backgroundColor { get; set; }

    }
}
