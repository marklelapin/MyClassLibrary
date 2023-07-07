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


        public PointBuilder AddColors(ColorSet colorSet)
        {
            _point.backgroundColor = colorSet.backgroundColor;
            _point.borderColor = colorSet.borderColor;
            return this;
        }

        public PointBuilder AddWidthAndHoverWidth(int? borderWidth = null, int? hoverBorderWidth = null)
        {
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
