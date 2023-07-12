namespace MyClassLibrary.ChartJs
{
    public class CategoryCoordinate
    {
        /// <summary>
        /// Category along the x axis this should go against.
        /// </summary>
        public string XCategory { get; set; }

        /// <summary>
        /// The Category along the y axis this should go against.
        /// </summary>
        public string YCategory { get; set; }

        /// <summary>
        /// The size of the bubble
        /// </summary>
        public double R { get; set; }

        /// <summary>
        /// An Id that can be used by event handlers.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// The data to show in tooltips or datalabels for the specific data point.
        /// </summary>
        public string? Label { get; set; }


        public CategoryCoordinate(string xCategory, string yCategory, double r = 1, string? label = null, string? id = null)
        {
            this.XCategory = xCategory;
            this.YCategory = yCategory;
            this.R = r;
            this.Label = label;
            this.Id = id;
        }


    }
}
