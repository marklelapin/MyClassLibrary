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
        public bool? hidden { get; set; }

        /// <summary
        /// The Border Color as hexidecimal string.
        /// </summary>
        public string? borderColor { get; set; }

        /// <summary>
        /// The Background Color as hexidecimal string. 
        /// </summary>
        public string? backgroundColor { get; set; }

        /// <summary>
        /// Specifies the bezier curve tension. 0 = no bezier curve
        /// </summary>
        public int? tension { get; set; }

        /// <summary>
        /// The width of the line.
        /// </summary>
        public int? borderWidth { get; set; }

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
        /// The target dataset to fill to
        /// </summary>
        /// <remarks>
        /// '+1' targets the next dataset up,'-2' target next data set down. <br/>
        /// '3' targets dataset 3 specifically.
        /// 'origin' targets the bottom of the chart.<br/>
        /// 'end' target the top of the chart.<br/>
        /// If the target can't be found (doesn't exist or is hidden then then no fill)
        /// </remarks>
        public string? fill { get; set; }

        /// <summary>
        /// Determines wether the line is stepped or draws a direct line betwee points.
        /// </summary>
        public bool? stepped { get; set; }

        /// <summary>
        /// Determines whether to continue the line (true) or break it (false) when crossing a null data point. 
        /// </summary>
        public bool? spanGaps { get; set; }


        /// <summary>
        /// The ScaleId of the x axis to use for this dataset.
        /// </summary>
        public string? xAxisID { get; set; }

        /// <summary>
        /// The ScaleId of the y axis to use for this dataset.
        /// </summary>
        public string? yAxisID { get; set; }

        /// <summary>
        /// Point radius
        /// </summary>
        public int? radius { get; set; }


        /// <summary>
        /// The shape of the point.
        /// </summary>
        /// <remarks>
        /// 'circle' <br/>
        ///'cross' <br/>
        ///'crossRot' <br/>
        ///'dash' <br/>
        ///'line' <br/>
        ///'rect' <br/>
        ///'rectRounded' <br/>
        ///'rectRot' <br/>
        ///'star' <br/>
        ///'triangle' <br/>
        ///false
        /// </remarks>
        public string? pointStyle { get; set; }

        /// <summary>
        /// Point rotation in degrees.
        /// </summary>
        public int? rotation { get; set; }

        /// <summary>
        /// Extra radius added to point radius to hit detection
        /// </summary>
        public int? hitRadius { get; set; }

        /// <summary>
        /// Point radius when hovered.
        /// </summary>
        public int? hoverRadius { get; set; }

        /// <summary>
        /// Borderwidth when hovered.
        /// </summary>
        public int? hoverBorderWidth { get; set; }

        /// <summary>
        /// The border radius of bar chart when hovered over.
        /// </summary>
        public int? hoverBorderRadius { get; set; }
        /// <summary>
        /// Color of border when hovered.
        /// </summary>
        public string? hoverBorderColor { get; set; }

        /// <summary>
        /// Color of background when hovered.
        /// </summary>
        public string? hoverBackgroundColor { get; set; }
        /// <summary>
        /// The type of chart (when mixed chart types being used)
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// Determines whether line is drawn between points.
        /// </summary>
        public bool? showLine { get; set; }
    }
}
