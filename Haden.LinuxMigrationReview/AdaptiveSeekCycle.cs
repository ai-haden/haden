using System;

namespace Haden.LinuxMigrationReview
{
    public sealed class AdaptiveSeekCycle
    {
        public AdaptiveSeekCycle(int initialPower = 15, int initialDegrees = 15, int initialDirection = 1)
        {
            if (initialDirection != 1 && initialDirection != -1)
            {
                throw new ArgumentOutOfRangeException(nameof(initialDirection), "Direction must be 1 or -1.");
            }

            Power = initialPower;
            Degrees = initialDegrees;
            Direction = initialDirection;
        }

        public int Power { get; }
        public int Degrees { get; private set; }
        public int Direction { get; private set; }

        public AdaptiveSeekStep Advance(int previousLightValue, int currentLightValue)
        {
            int delta = currentLightValue - previousLightValue;

            if (delta > 2)
            {
                Degrees = Math.Min(Degrees + 5, 35);
            }
            else
            {
                Direction *= -1;
                Degrees = Math.Max(Degrees - 5, 10);
            }

            return new AdaptiveSeekStep(Power, Degrees, Direction, delta);
        }
    }

    public readonly struct AdaptiveSeekStep
    {
        public AdaptiveSeekStep(int power, int degrees, int direction, int delta)
        {
            Power = power;
            Degrees = degrees;
            Direction = direction;
            Delta = delta;
        }

        public int Power { get; }
        public int Degrees { get; }
        public int Direction { get; }
        public int Delta { get; }
    }
}
