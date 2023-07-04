using MyClassLibrary.Extensions;
using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class PointBuilder
    {
        private Point _point { get; set; }
        public PointBuilder()
        {
            _point = new Point();
        }

        public PointBuilder AddStyleAndRadius(string style, int radius, int? rotation = null)
        {
            _point.radius = radius;
            _point.pointStyle = style;
            return this;
        }


        public PointBuilder FormatPoint(Color backgroundColor, Color borderColor, int? borderWidth = null, int? hoverBorderWidth = null)
        {
            _point.backgroundColor = backgroundColor.ToHex();
            _point.borderColor = borderColor.ToHex();
            _point.borderWidth = borderWidth;
            _point.hoverBorderWidth = hoverBorderWidth;
            return this;
        }


        public PointBuilder AddHover(int hoverRadius, int? hoverBorderWidth = null)
        {
            _point.hoverRadius = hoverRadius;
            _point.hoverBorderWidth = hoverBorderWidth;
            return this;
        }

        public PointBuilder AddHit(int hitRadius)
        {
            _point.hitRadius = hitRadius;
            return this;
        }

        public Point Build()
        {
            return _point;
        }
    }


}
