using System;
using System.Collections.Generic;

namespace Haden.RobotBehavior
{
    public sealed class LightSignalSmoother
    {
        private readonly Queue<int> _samples;
        private readonly int _windowSize;
        private int _sum;

        public LightSignalSmoother(int windowSize = 3)
        {
            if (windowSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Window size must be positive.");
            }

            _windowSize = windowSize;
            _samples = new Queue<int>(windowSize);
        }

        public int WindowSize => _windowSize;
        public int SampleCount => _samples.Count;

        public int AddSample(int value)
        {
            _samples.Enqueue(value);
            _sum += value;

            while (_samples.Count > _windowSize)
            {
                _sum -= _samples.Dequeue();
            }

            return (int)Math.Round((double)_sum / _samples.Count, MidpointRounding.AwayFromZero);
        }
    }
}
