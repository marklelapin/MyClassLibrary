using MyClassLibrary.Extensions;
using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class ColorSet
    {
        /// <summary>
        /// Line color
        /// </summary>
        public string? borderColor { get; set; }

        /// <summary>
        /// Background color
        /// </summary>
        public string? backgroundColor { get; set; }

        /// <summary>
        /// Font colot
        /// </summary>
        public string? color { get; set; }

        public ColorSet(string borderColor, string backgroundColor)
        {
            this.borderColor = borderColor;
            this.backgroundColor = backgroundColor;
        }

        public ColorSet(Color borderColor, Color backgroundColor)
        {
            this.borderColor = borderColor.ToHex();
            this.backgroundColor = backgroundColor.ToHex();
        }

        public ColorSet(string borderColor, string backgroundColor, string fontColor)
        {
            this.borderColor = borderColor;
            this.backgroundColor = backgroundColor;
            this.color = fontColor;
        }

        public ColorSet(Color borderColor, Color backgroundColor, Color fontColor)
        {
            this.borderColor = borderColor.ToHex();
            this.backgroundColor = backgroundColor.ToHex();
            this.color = fontColor.ToHex();
        }

    }
}
