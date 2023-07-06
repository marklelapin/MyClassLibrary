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
    }
}
