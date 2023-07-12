namespace MyClassLibrary.ChartJs
{
    public class CategoryBubbleChartData
    {
        private double radiusMultiplier;

        public Dictionary<string, int> XLabels { get; private set; } = new Dictionary<string, int>();

        public Dictionary<string, int> YLabels { get; private set; } = new Dictionary<string, int>();

        public Dictionary<string, List<Coordinate>> Coordinates { get; private set; } = new Dictionary<string, List<Coordinate>>();

        public CategoryBubbleChartData(string title, List<CategoryCoordinate> categoryCoordinates, double maxRadius)
        {
            SetRadiusMultiplier(categoryCoordinates, maxRadius);
            CreateLabels(categoryCoordinates);
            CreateCoordinates(title, categoryCoordinates);
        }

        public CategoryBubbleChartData(Dictionary<string, List<CategoryCoordinate>> multipleCategoryCoordinates, double maxRadius)
        {
            List<CategoryCoordinate> combinedCoordinates = new List<CategoryCoordinate>();

            foreach (var categoryCoordinates in multipleCategoryCoordinates.Values)
            {
                combinedCoordinates.AddRange(categoryCoordinates);
            }

            SetRadiusMultiplier(combinedCoordinates, maxRadius);
            CreateLabels(combinedCoordinates);
            CreateCoordinates(multipleCategoryCoordinates);
        }


        private void CreateLabels(List<CategoryCoordinate> combinedCoordinates)
        {
            CreateXLabels(combinedCoordinates);
            CreateYLabels(combinedCoordinates);
        }

        private void CreateXLabels(List<CategoryCoordinate> combinedCoordinates)
        {
            var labels = combinedCoordinates.Select(x => x.XCategory).Distinct().OrderBy(x => x).ToArray();

            int i = 1;
            foreach (var label in labels)
            {
                XLabels.Add(label, i);
                i++;
            }

        }
        private void CreateYLabels(List<CategoryCoordinate> combinedCoordinates)
        {
            var labels = combinedCoordinates.Select(x => x.YCategory).Distinct().OrderBy(x => x).ToArray();

            int i = 1;
            foreach (var label in labels)
            {
                YLabels.Add(label, i);
                i++;
            }
        }



        private void SetRadiusMultiplier(List<CategoryCoordinate> combinedCoordinates, double maxRadius)
        {
            double maxR = combinedCoordinates.Max(x => x.R);

            radiusMultiplier = maxRadius / maxR;

        }



        private void CreateCoordinates(string title, List<CategoryCoordinate> categoryCoordinates)
        {
            var convertedCoordinates = categoryCoordinates.Select(c => new Coordinate(XLabels[c.XCategory], YLabels[c.YCategory], c.R * radiusMultiplier, c.Label, c.Id)).ToList();

            Coordinates.Add(title, convertedCoordinates);

        }

        private void CreateCoordinates(Dictionary<string, List<CategoryCoordinate>> multipleCategoryCoordinates)
        {
            foreach (var item in multipleCategoryCoordinates)
            {
                CreateCoordinates(item.Key, item.Value);
            }
        }
    }
}
