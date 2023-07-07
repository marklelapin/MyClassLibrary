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
    }
}
