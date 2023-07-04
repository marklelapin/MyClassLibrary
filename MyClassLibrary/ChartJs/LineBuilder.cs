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

        public LineBuilder AddBasicDetails(int borderWidth, Color borderColor, Color backgroundColor, int[]? borderDash = null)
        {
            _line.borderWidth = borderWidth;
            _line.borderColor = borderColor.ToHex();
            _line.backgroundColor = backgroundColor.ToHex();
            _line.borderDash = borderDash;

            return this;
        }

        public Line Build()
        {
            return _line;
        }

    }
}
