using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class CartesianAxisBuilder
    {
        private CartesianAxis _axis;

        public CartesianAxisBuilder(string? type = null)
        {
            _axis = new CartesianAxis();
            _axis.type = type;
        }

        public CartesianAxisBuilder AddTitle(string title, string align = "center", Color? color = null, Font? font = null)
        {
            _axis.title = new Title(title, align, color, font);
            return this;
        }

        public CartesianAxisBuilder AddType(string type)
        {
            _axis.type = type
             ; return this;
        }

        public CartesianAxisBuilder AddAbsoluteScaleLimits(double? min, double? max)
        {
            _axis.min = min.ToString();
            _axis.max = max.ToString();
            return this;
        }

        public CartesianAxisBuilder AddAbsoluteScaleLimits(string? min, string? max)
        {
            _axis.min = min;
            _axis.max = max;
            return this;
        }

        public CartesianAxisBuilder AddSuggestedScaleLimts(string? suggestedMin, string? suggestedMax)
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
