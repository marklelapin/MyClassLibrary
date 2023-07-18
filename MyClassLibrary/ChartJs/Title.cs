namespace MyClassLibrary.ChartJs
{
    public class Title
    {
        /// <summary>
        /// Wether or not to display the title.
        /// </summary>
        public bool? display { get; set; } = true;

        /// <summary>
        /// The Title alignment ('start','center','end')
        /// </summary>
        public string? align { get; set; }

        /// <summary>
        /// The color of the title as hex string.
        /// </summary>
        public string? color { get; set; }

        /// <summary>
        /// The font of the title
        /// </summary>
        public Font? font { get; set; }

        /// <summary>
        /// The text of the title.
        /// </summary>
        public string? text { get; set; }

        public Title() { }

        public Title(string text, string? align, string? color = null, Font? font = null)
        {
            this.text = text;
            this.align = align ?? "center";
            this.color = color;
            this.font = font;
        }

    }
}
