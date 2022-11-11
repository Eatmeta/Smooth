using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentially(this IEnumerable<DataPoint> data, double alpha)
        {
            var counter = 0;
            var lastDataPoint = new DataPoint(0, 0);

            foreach (var dataPoint in data)
            {
                lastDataPoint = counter == 0
                    ? dataPoint.WithExpSmoothedY(dataPoint.OriginalY)
                    : GetExpSmoothing(dataPoint, alpha, lastDataPoint);
                yield return lastDataPoint;
                counter++;
            }
        }

        private static DataPoint GetExpSmoothing(DataPoint dataPoint, double alpha, DataPoint lastDataPoint)
        {
            return dataPoint.WithExpSmoothedY(lastDataPoint.ExpSmoothedY +
                                              alpha * (dataPoint.OriginalY - lastDataPoint.ExpSmoothedY));
        }
    }
}