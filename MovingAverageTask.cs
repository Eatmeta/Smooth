using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var isFirst = true;
            var queue = new Queue<double>();
            var sumWindowElements = 0.0;

            foreach (var dataPoint in data)
            {
                if (isFirst || windowWidth == 1)
                {
                    isFirst = false;
                    queue.Enqueue(dataPoint.OriginalY);
                    sumWindowElements += dataPoint.OriginalY;
                    yield return dataPoint.WithAvgSmoothedY(dataPoint.OriginalY);
                }
                else if (queue.Count < windowWidth)
                {
                    sumWindowElements += dataPoint.OriginalY;
                    queue.Enqueue(dataPoint.OriginalY);
                    yield return dataPoint.WithAvgSmoothedY(sumWindowElements / queue.Count);
                }
                else
                {
                    sumWindowElements -= queue.Dequeue();
                    sumWindowElements += dataPoint.OriginalY;
                    queue.Enqueue(dataPoint.OriginalY);
                    yield return dataPoint.WithAvgSmoothedY(sumWindowElements / windowWidth);
                }
            }
        }
    }
}