namespace MyClassLibrary.ChartJs
{
    public class CartesianAxis : Axis
    {
        /// <summary>
        /// Determines the scale bounds. Default = 'ticks' if null.
        /// </summary>
        /// <remarks>
        /// 'data' makes sure data are fully visible, labels outside are removed <br/>
        /// 'ticks' make sure ticks are fully visible, data outside are truncated
        /// </remarks>
        public string? bounds { get; set; }

        /// <summary>
        /// The positionf of the azis.
        /// </summary>
        /// <remarks>
        /// either absolute = 'top','left','bottom','right' <br/>
        /// or relative = '{ x: -20 }' or { y: 10} etc
        /// </remarks>
        public string? position { get; set; }

        /// <summary>
        /// Stack group. Axes at the same position with same stack are stacked.
        /// </summary>
        public string? stack { get; set; }

        /// <summary>
        /// Weight of the scale in stack group. Used to determine the amount of allocated space for the scale within the group.
        /// </summary>
        public int? stackWeight { get; set; }

        /// <summary>
        /// What type of axis this is 'x' or 'y'. if null will infer it from first letter of ScaledID
        /// </summary>
        public string? axis { get; set; }

        /// <summary>
        /// If true, extra space is added to the both edges and the axis is scaled to fit into the chart area. This is set to true for a bar chart by default.
        /// </summary>
        public bool? offset { get; set; }

        /// <summary>
        ///  The Title object associated with the axis.
        /// </summary>
        public Title title { get; set; } = new Title();


        /// <summary>
        /// Configuration of time options for time scale.
        /// </summary>
        public Time time { get; set; } = new Time();


        /// <summary>
        /// String array of category labels associated with axis.
        /// </summary>
        public string[]? labels { get; set; }



        /// <summary>
        /// Determines whether or not ChartJs skip labels automatically. The Default is true.
        /// </summary>
        public bool? autoSkip { get; set; }

        /// <summary>
        /// The padding to apply around each label when working out if it should be skipped or not.
        /// </summary>
        public int? autoSkipPadding { get; set; }

    }
}
