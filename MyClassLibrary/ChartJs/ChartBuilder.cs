using System.Text.Json;

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



        public Chart Build()
        {
            return _chart;
        }

        public string BuildJson()
        {


            return JsonSerializer.Serialize(_chart, new JsonSerializerOptions
            {
                // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });
        }
    }
}
