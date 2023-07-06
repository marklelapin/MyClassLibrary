using MyClassLibrary.Extensions;
using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class LineBuilder
    {
        private Line _line;

        public LineBuilder()
        {
            _line = new Line();
        }

        public LineBuilder AddLineStyle(int borderWidth, int[]? borderDash = null)
        {
            _line.borderWidth = borderWidth;
            _line.borderDash = borderDash;

            return this;
        }

        public LineBuilder AddBorderAndBackGroundColor(Color borderColor, Color backgroundColor)
        {
            _line.borderColor = borderColor.ToHex();
            _line.backgroundColor = backgroundColor.ToHex();
            return this;
        }
        public LineBuilder AddBorderAndBackGroundColor(string borderColor, string backgroundColor)
        {
            _line.borderColor = borderColor;
            _line.backgroundColor = backgroundColor;
            return this;
        }


        public Line Build()
        {
            return _line;
        }

    }
}
