namespace MyClassLibrary.ChartJs
{
    public class Point
    {
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
        /// Piint Fill Color
        /// </summary>
        public string? backgroundColor { get; set; }

        /// <summary>
        /// Piint Fill Color
        /// </summary>
        public string? borderColor { get; set; }

        /// <summary>
        /// Point borderWidth
        /// </summary>
        public int? borderWidth { get; set; }

        /// <summary>
        /// Extra radius added to point radius to hit detection
        /// </summary>
        public int? hitRadius { get; set; }

        /// <summary>
        /// Point radius when hovered
        /// </summary>
        public int? hoverRadius { get; set; }

        /// <summary>
        /// Borderwidth when hovered
        /// </summary>
        public int? hoverBorderWidth { get; set; }

    }
}
