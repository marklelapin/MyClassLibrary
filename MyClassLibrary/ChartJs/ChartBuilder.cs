using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyClassLibrary.ChartJs
{
    public class ChartBuilder
    {
        private Chart _chart;

        public ChartBuilder(string type)
        {
            _chart = new Chart(type);
        }



        public ChartBuilder AddLables(List<string> labels)
        {
            return AddLabels(labels.ToArray());
        }
        public ChartBuilder AddLabels(string[] labels)
        {
            _chart.data.labels = labels;
            return this;
        }

        public ChartBuilder HideLegend()
        {
            _chart.options.plugins.legend.display = false;
            return this;
        }

        public ChartBuilder AddDataset(string label, Action<DatasetBuilder> builderActions)
        {
            var datasetBuilder = new DatasetBuilder(label);
            builderActions(datasetBuilder);
            Dataset newDataset = datasetBuilder.Build();

            if (_chart.data.datasets == null)
            {
                _chart.data.datasets = new Dataset[] { newDataset };
            }
            else
            {
                _chart.data.datasets = _chart.data.datasets.Append(newDataset).ToArray();
            }

            return this;
        }


        public ChartBuilder AddDefaultLineStyle(int borderWidth, Color borderColor, Color backgroundColor, int[]? borderDash = null)
        {
            var line = new LineBuilder().AddBasicDetails(borderWidth, borderColor, backgroundColor, borderDash).Build();
            _chart.options.elements.line = line;

            return this;

        }


        public ChartBuilder AddDefaultPointStyle(Action<PointBuilder> builderActions)
        {
            var pointBuilder = new PointBuilder();
            builderActions(pointBuilder);
            _chart.options.elements.point = pointBuilder.Build();
            return this;
        }



        //Axis Configuration
        public ChartBuilder ConfigureXAxis(Action<CartesianAxisBuilder> builderActions)
        {
            var axis = CreateCartesianAxis(builderActions);
            axis.axis = "x";
            _chart.options.scales.x = axis;

            return this;
        }

        public ChartBuilder ConfigureYAxis(Action<CartesianAxisBuilder> builderActions)
        {
            var axis = CreateCartesianAxis(builderActions);
            axis.axis = "y";
            _chart.options.scales.y = axis;
            return this;
        }

        public ChartBuilder ConfigureSecondaryXAxis(Action<CartesianAxisBuilder> builderActions)
        {
            var axis = CreateCartesianAxis(builderActions);
            axis.axis = "x";
            _chart.options.scales.secondaryXAxis = axis;
            return this;
        }

        public ChartBuilder ConfigureSecondyYAxis(Action<CartesianAxisBuilder> builderActions)
        {
            var axis = CreateCartesianAxis(builderActions);
            axis.axis = "y";
            _chart.options.scales.secondaryYAxis = axis;
            return this;
        }



        private CartesianAxis CreateCartesianAxis(Action<CartesianAxisBuilder> builderActions)
        {
            var axisBuilder = new CartesianAxisBuilder();
            if (builderActions != null) { builderActions(axisBuilder); };
            CartesianAxis newAxis = axisBuilder.Build();

            return newAxis;
        }





        public Chart Build()
        {
            return _chart;
        }

        public string BuildJson()
        {
            string json = JsonSerializer.Serialize(_chart, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            json.Replace("\"false\"", "false");
            json.Replace("\"true\"", "true");

            return json;
        }

    }
}
