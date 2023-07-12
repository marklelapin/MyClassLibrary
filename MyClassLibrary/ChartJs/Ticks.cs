namespace MyClassLibrary.ChartJs
{
    public class Ticks
    {

        /// <summary>
        /// Reference string for CallBackFunctionLibrary to pass in actual function for call back when building Chart Configuration in ChartBuilder.
        /// </summary>
        public string? callback { get; set; }



        /// <summary>
        /// String array containing labels for ticks.
        /// </summary>
        public string[]? labels { get; set; }



        /// <summary>
        /// Determines whether ticks are automatically skipped or not.
        /// </summary>
        public bool? autoSkip { get; set; }


        /// <summary>
        /// The minimum allowed rotation for tick labels
        /// </summary>
        public int? minRotation { get; set; }

        /// <summary>
        /// The Maximum allowed rotation for tick labels.
        /// </summary>
        public int? maxRotation { get; set; }
    }
}
