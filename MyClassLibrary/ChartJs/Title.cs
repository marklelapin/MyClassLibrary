using MyClassLibrary.Extensions;
using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class Title
    {
        /// <summary>
        /// Wether or not to display the title.
        /// </summary>
        public bool display { get; set; }

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


        public Title(string text, string? align, Color? color = null, Font? font = null)
        {
            this.text = text;
            this.align = align ?? "center";
            if (color != null)
            {
                Color newColor = (Color)color;
                this.color = newColor.ToHex();
            }
            this.font = font;
        }

    }
}
