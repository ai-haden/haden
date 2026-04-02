using System;

namespace Haden.RobotBehavior
{
    public static class LightSeekRewardSignal
    {
        // Reward shape intended for online RL updates during hardware seek episodes.
        public static double Compute(int delta, bool bumpPressed)
        {
            if (bumpPressed)
            {
                return 10.0;
            }

            if (delta > 0)
            {
                return 1.0 + (delta * 0.1);
            }

            if (delta < 0)
            {
                return -0.5 + (delta * 0.05);
            }

            return -0.05;
        }
    }
}
