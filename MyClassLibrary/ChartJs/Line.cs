namespace MyClassLibrary.ChartJs
{
    public class Line
    {
        /// <summary>
        /// Specifies the bezier curve tension. 0 = no bezier curve
        /// </summary>
        public int? tension { get; set; }

        /// <summary>
        /// The width of the line.
        /// </summary>
        public int borderWidth { get; set; }

        /// <summary>
        /// Line fill color
        /// </summary>
        public string? backgroundColor { get; set; }

        /// <summary>
        /// Line color
        /// </summary>
        public string? borderColor { get; set; }

        /// <summary>
        /// Line cap style (butt,round or square)
        /// </summary>
        public string? borderCapStyle { get; set; } = "butt";

        /// <summary>
        /// Line dash style [width of dash, width of gap]
        /// </summary>
        public int[]? borderDash { get; set; }
        /// <summary>
        /// Offset before dashes begin.
        /// </summary>
        public int? borderDashOffset { get; set; }

        /// <summary>
        /// The style of join (round,bevel or mitre)
        /// </summary>
        public string? borderJoinStyle { get; set; }

        /// <summary>
        /// True to keep Bezier control inside the chart, false for not restriction.
        /// </summary>
        public bool? capBezierPoints { get; set; }


        /// <summary>
        /// Interpolation mode to apply.
        /// </summary>
        public string? cubicInterpolationMode { get; set; }

        /// <summary>
        /// How to fill the area under the line. (in Chart.Js this can either be boolean (false = don't fill) or string 
        /// </summary>
        public string? fill { get; set; }

        /// <summary>
        /// Determines wether the line is stepped or draws a direct line betwee points.
        /// </summary>
        public bool? stepped { get; set; }

        /// <summary>
        /// Determines whether to continue the line (true) or break it (false) when crossing a null data point. 
        /// </summary>
        public bool? spanGaps { get; set; }

    }
}
