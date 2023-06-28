namespace MyClassLibrary.ChartJs
{
    public class Data
    {
        /// <summary>
        /// An array of the datasets to appear in the chart.
        /// </summary>
        public Dataset[]? datasets { get; set; }

        /// <summary>
        /// Array of string labels that if used must match the same amount of elements as the dataset with most values.
        /// </summary>
        public string[]? labels { get; set; }

    }
}
