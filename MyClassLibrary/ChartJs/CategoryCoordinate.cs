namespace MyClassLibrary.ChartJs
{
    public class CategoryCoordinate
    {
        /// <summary>
        /// Category along the x axis this should go against.
        /// </summary>
        public string xCategory { get; set; }

        /// <summary>
        /// The Category along the y axis this should go against.
        /// </summary>
        public string yCategory { get; set; }

        /// <summary>
        /// The size of the bubble
        /// </summary>
        public double r { get; set; }


        public CategoryCoordinate(string xCategory, string yCategory, double r = 1)
        {
            this.xCategory = xCategory;
            this.yCategory = yCategory;
            this.r = r;
        }


    }
}
