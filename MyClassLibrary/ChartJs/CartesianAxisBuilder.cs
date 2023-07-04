using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class CartesianAxisBuilder
    {
        private CartesianAxis _axis;

        public CartesianAxisBuilder()
        {
            _axis = new CartesianAxis();
        }

        public CartesianAxisBuilder AddTitle(string title, string align = "center", Color? color = null, Font? font = null)
        {
            _axis.title = new Title(title, align, color, font);
            return this;
        }

        public CartesianAxisBuilder AddAbsoluteScaleLimits(Double? min, Double? max)
        {
            _axis.min = min;
            _axis.max = max;
            return this;
        }

        public CartesianAxisBuilder AddSuggestedScaleLimts(Double suggestedMin, Double suggestedMax)
        {
            _axis.suggestedMin = suggestedMin;
            _axis.suggestedMax = suggestedMax;
            return this;
        }

        public CartesianAxisBuilder Stacked(string stacked)
        {
            _axis.stacked = stacked;
            return this;
        }


        public CartesianAxisBuilder AddGrid()
        {
            throw new NotImplementedException();
        }


        public CartesianAxisBuilder AddTicks()
        {
            throw new NotImplementedException();
        }


        public CartesianAxis Build()
        {
            return _axis;
        }

    }




}
