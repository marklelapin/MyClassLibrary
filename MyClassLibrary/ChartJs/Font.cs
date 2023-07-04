namespace MyClassLibrary.ChartJs
{
    public class Font
    {
        /// <summary>
        /// Default font family for all text, follows CSS font-family options.
        /// </summary>
        public string? family { get; set; }

        /// <summary>
        /// Default font size(in px) for text.Does not apply to radialLinear scale point labels.
        /// </summary>
        public Double? size { get; set; }

        /// <summary>
        /// Default font style. Does not apply to tooltip title or footer. Does not apply to chart title.
        /// </summary>
        /// <remarks>
        /// Follows CSS font-style options (i.e. normal, italic, oblique, initial, inherit).
        /// </remarks>
        public string? style { get; set; }
        /// <summary>
        /// Default font weight (normal,bold,lighter,bolder,100 to 900)
        /// </summary>
        public string? weight { get; set; }

        public string? lineHeight { get; set; }
    }
}
