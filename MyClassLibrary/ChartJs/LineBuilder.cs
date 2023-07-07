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

        public LineBuilder AddColors(ColorSet colorSet)
        {
            _line.borderColor = colorSet.borderColor;
            _line.backgroundColor = colorSet.backgroundColor;
            return this;
        }


        public Line Build()
        {
            return _line;
        }

    }
}
