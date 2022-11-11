using System;
using System.Collections.Generic;

namespace yield
{
    public static class MovingMaxTask
    {
        private static double _localMax;
        private static LinkedList<double> _dequeMaximums;
        private static Queue<double> _queueWindow;

        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var counter = 0;
            _localMax = 0.0;
            _dequeMaximums = new LinkedList<double>();
            _queueWindow = new Queue<double>();

            foreach (var dataPoint in data)
            {
                if (counter == 0)
                {
                    AddFirstPointToLists(dataPoint);
                }
                else if (counter < windowWidth)
                {
                    DeleteSmallerPoints(dataPoint);
                    AddAsFirstOrAddAsLast(dataPoint);
                }
                else
                {
                    if (IsMaximumShouldGone())
                    {
                        _queueWindow.Dequeue();
                        _dequeMaximums.RemoveFirst();
                        _localMax = _dequeMaximums.Count != 0 ? _dequeMaximums.First.Value : dataPoint.OriginalY;
                        DeleteSmallerPoints(dataPoint);
                        AddAsFirstOrAddAsLast(dataPoint);
                    }
                    else
                    {
                        _queueWindow.Dequeue();
                        DeleteSmallerPoints(dataPoint);
                        AddAsFirstOrAddAsLast(dataPoint);
                    }
                }
                counter++;
                yield return dataPoint.WithMaxY(_dequeMaximums.First.Value);
            }
        }

        private static bool IsMaximumShouldGone()
        {
            return Math.Abs(_queueWindow.Peek() - _localMax) < 1e-7;
        }

        private static void AddAsFirstOrAddAsLast(DataPoint dataPoint)
        {
            if (_dequeMaximums.Count == 0)
                AddFirstPointToLists(dataPoint);
            else
                AddLastPointToLists(dataPoint);
        }

        private static void DeleteSmallerPoints(DataPoint dataPoint)
        {
            while (_dequeMaximums.Count != 0)
            {
                if (_dequeMaximums.Last.Value < dataPoint.OriginalY)
                    _dequeMaximums.RemoveLast();
                else
                    return;
            }
        }

        private static void AddFirstPointToLists(DataPoint dataPoint)
        {
            _localMax = dataPoint.OriginalY;
            _queueWindow.Enqueue(_localMax);
            _dequeMaximums.AddFirst(_localMax);
        }

        private static void AddLastPointToLists(DataPoint dataPoint)
        {
            _dequeMaximums.AddLast(dataPoint.OriginalY);
            _queueWindow.Enqueue(dataPoint.OriginalY);
        }
    }
}