namespace MyClassLibrary.ChartJs
{
    public class AxisBuilder
    {
        private Axis _axes;
        public AxisBuilder(double min, double max)
        {
            _axes = new Axis(min, max);
        }

        public Axis Build()
        {
            return _axes;
        }
    }
}
