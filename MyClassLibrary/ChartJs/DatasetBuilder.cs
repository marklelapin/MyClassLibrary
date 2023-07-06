using MyClassLibrary.Extensions;
using System.Drawing;

namespace MyClassLibrary.ChartJs
{
    public class DatasetBuilder
    {
        private Dataset _dataset;

        public DatasetBuilder()
        {
            _dataset = new Dataset();
        }

        public DatasetBuilder(string label)
        {
            _dataset = new Dataset();
            AddLabel(label);
        }
        public DatasetBuilder AddLabel(string label)
        {
            _dataset.label = label;
            return this;

        }

        public DatasetBuilder AddValues<T>(List<T> list)
        {
            _dataset.data = list.ToListObject().ToArray();
            return this;
        }

        public DatasetBuilder AddCoordinates(List<Coordinate> coordinates)
        {
            _dataset.data = coordinates.ToArray();
            return this;
        }

        public DatasetBuilder AddOrder(int orderNo)
        {
            _dataset.order = orderNo;
            return this;
        }

        public DatasetBuilder AddStackId(string stackId)
        {
            _dataset.stack = stackId;
            return this;

        }

        public DatasetBuilder SpecifyAxes(string? xscaleId, string? yScaleId)
        {
            _dataset.xAxisID = xscaleId;
            _dataset.yAxisID = yScaleId;
            return this;
        }

        public DatasetBuilder ShowLine()
        {
            _dataset.showLine = true;
            return this;
        }

        public DatasetBuilder HideLine()
        {
            _dataset.showLine = false;
            return this;
        }

        public DatasetBuilder AddBorderAndBackgroundColor(Color borderColor, Color backgroundColor, bool hidden = false)
        {
            AddBorderAndBackgroundColor(borderColor.ToHex(), backgroundColor.ToHex(), hidden);
            return this;
        }

        public DatasetBuilder AddBorderAndBackgroundColor(string borderColor, string backgroundColor, bool hidden = false)
        {
            _dataset.borderColor = borderColor;
            _dataset.backgroundColor = backgroundColor;
            _dataset.hidden = hidden;
            return this;
        }


        public DatasetBuilder AddLine(int borderWidth, int[]? borderDash = null)
        {
            _dataset.type = "line";
            _dataset.borderWidth = borderWidth;
            _dataset.borderDash = borderDash;
            return this;
        }

        public DatasetBuilder AddArea(string fillTo, int borderWidth, int[]? borderDash = null)
        {
            _dataset.type = "line";
            _dataset.borderWidth = borderWidth;
            _dataset.fill = fillTo;
            return this;
        }

        public Dataset Build()
        {
            return _dataset;
        }

    }
}
