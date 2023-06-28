namespace MyClassLibrary.ChartJs
{
    public class Chart
    {
        /// <summary>
        /// The main type of Chart.Js chart to be used.
        /// </summary>
        /// <remarks>
        /// (This can be overriden within dataset and is how mixed charts can specify type different types.) <br/> 
        /// Area <br/>
        /// Bar <br/>
        /// Bubble <br/>
        /// Doughnut/Pie <br/>
        /// Line <br/>
        /// Mixed <br/>
        /// Polar <br/>
        /// Radar <br/>
        /// Scatter <br/>
        /// </remarks>
        public string type { get; set; } = "bar";

        /// <summary>
        /// The data 
        /// </summary>
        public Data data { get; set; } = new Data();

        /// <summary>
        /// The option to be applied to the chart.
        /// </summary>
        public Options options { get; set; } = new Options();

        /// <summary>
        /// Inline plugins to be applied to the chart.
        /// </summary>
        public Plugins plugins { get; set; } = new Plugins();

        public Chart()
        {
        }

        public Chart(string type)
        {
            this.type = type;
        }


    }
}
