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

        public DatasetBuilder AddFormating(Color borderColor, Color backgroundColor, bool hidden = false)
        {
            AddFormating(borderColor.ToHex(), backgroundColor.ToHex(), hidden);
            return this;
        }

        public DatasetBuilder AddFormating(string borderColor, string backgroundColor, bool hidden = false)
        {
            _dataset.borderColor = borderColor;
            _dataset.backgroundColor = backgroundColor;
            _dataset.hidden = hidden;
            return this;
        }


        public Dataset Build()
        {
            return _dataset;
        }

    }
}
