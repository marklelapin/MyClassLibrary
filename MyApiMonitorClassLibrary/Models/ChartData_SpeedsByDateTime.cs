namespace MyApiMonitorClassLibrary.Models
{
    public class ChartData_SpeedsByDateTime
    {
        /// <summary>
        /// The Date and Time the Tests were run.
        /// </summary>
        public DateTime TestDateTime { get; set; }

        /// <summary>
        /// The average speed in milliseconds of tests run at TestDateTime
        /// </summary>
        public int? AvgSpeed { get; set; }

        /// <summary>
        /// The maximum speed in milliseconds of tests run at TestDateTime
        /// </summary>
        public int? MaxSpeed { get; set; }

        /// <summary>
        /// The min speed in milliseconds of tests run at TestDateTime
        /// </summary>
        public int? MinSpeed { get; set; }



        public ChartData_SpeedsByDateTime(DateTime testDateTime, int? avgSpeed, int? maxSpeed, int? minSpeed)
        {
            TestDateTime = testDateTime;
            AvgSpeed = avgSpeed;
            MaxSpeed = maxSpeed;
            MinSpeed = minSpeed;
        }
    }
}
