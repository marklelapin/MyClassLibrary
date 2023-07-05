namespace MyClassLibrary.ChartJs
{
    public class CategoryBubbleChartData
    {

        public Dictionary<string, int> xLabels { get; private set; } = new Dictionary<string, int>();

        public Dictionary<string, int> yLabels { get; private set; } = new Dictionary<string, int>();

        public List<Coordinate> coordinates { get; private set; }

        public CategoryBubbleChartData(List<CategoryCoordinate> categoryCoordinates, double maxRadius)
        {
            CreateXLables(categoryCoordinates);
            CreateYLables(categoryCoordinates);
            CreateCoordinates(categoryCoordinates, maxRadius);

        }

        private void CreateXLables(List<CategoryCoordinate> categoryCoordinates)
        {
            var labels = categoryCoordinates.Select(x => x.xCategory).Distinct().OrderBy(x => x).ToArray();

            int i = 1;
            foreach (var label in labels)
            {
                xLabels.Add(label, i);
                i++;
            }

        }
        private void CreateYLables(List<CategoryCoordinate> categoryCoordinates)
        {
            var labels = categoryCoordinates.Select(x => x.yCategory).Distinct().OrderBy(x => x).ToArray();

            int i = 1;
            foreach (var label in labels)
            {
                yLabels.Add(label, i);
                i++;
            }
        }

        private void CreateCoordinates(List<CategoryCoordinate> categoryCoordinates, double maxRadius)
        {
            double maxR = categoryCoordinates.Max(x => x.r);

            double radiusMultiplier = maxRadius / maxR;


            var output = categoryCoordinates.Select(x => new Coordinate(xLabels[x.xCategory], yLabels[x.yCategory], x.r * radiusMultiplier)).ToList();

            coordinates = output;

        }




    }
}
