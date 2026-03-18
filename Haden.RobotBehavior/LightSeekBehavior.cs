using System;

namespace Haden.RobotBehavior
{
    public enum TurnDirection
    {
        None,
        Left,
        Right
    }

    public sealed class DifferenceStepResult
    {
        public DifferenceStepResult(int difference, int optimalValue, TurnDirection nextTurn, int iteration)
        {
            Difference = difference;
            OptimalValue = optimalValue;
            NextTurn = nextTurn;
            Iteration = iteration;
        }

        public int Difference { get; }
        public int OptimalValue { get; }
        public TurnDirection NextTurn { get; }
        public int Iteration { get; }
    }

    public sealed class CompareStepResult
    {
        public CompareStepResult(
            TurnDirection nextTurn,
            bool greaterNow,
            bool lessNow,
            double peakLightValue,
            int reward,
            double currentValue,
            double difference)
        {
            NextTurn = nextTurn;
            GreaterNow = greaterNow;
            LessNow = lessNow;
            PeakLightValue = peakLightValue;
            Reward = reward;
            CurrentValue = currentValue;
            Difference = difference;
        }

        public TurnDirection NextTurn { get; }
        public bool GreaterNow { get; }
        public bool LessNow { get; }
        public double PeakLightValue { get; }
        public int Reward { get; }
        public double CurrentValue { get; }
        public double Difference { get; }
    }

    public static class LightSeekBehavior
    {
        // Legacy simulator/autonomy behavior: initial random side selection.
        public static TurnDirection DecideInitialTurn(int randomOneOrTwo)
        {
            if (randomOneOrTwo == 1)
            {
                return TurnDirection.Left;
            }

            if (randomOneOrTwo == 2)
            {
                return TurnDirection.Right;
            }

            throw new ArgumentOutOfRangeException(nameof(randomOneOrTwo), "Expected 1 or 2.");
        }

        // Legacy simulator behavior: positive delta keeps direction, negative delta flips, zero keeps and increments iteration.
        public static DifferenceStepResult EvaluateSensorDifference(
            int currentValue,
            int optimalValue,
            TurnDirection lastTurn,
            int iteration)
        {
            int difference = currentValue - optimalValue;
            TurnDirection nextTurn = lastTurn;
            int updatedOptimal = optimalValue;
            int updatedIteration = iteration;

            if (difference > 0)
            {
                updatedOptimal = currentValue;
            }
            else if (difference < 0)
            {
                nextTurn = Opposite(lastTurn);
            }
            else
            {
                updatedIteration++;
            }

            return new DifferenceStepResult(difference, updatedOptimal, nextTurn, updatedIteration);
        }

        // Legacy manual form behavior extracted from Compare().
        public static CompareStepResult CompareNowToCurrent(
            double currentValue,
            double now,
            double peakLightValue,
            TurnDirection lastTurn,
            int reward)
        {
            if (currentValue == 0 && now == 0)
            {
                return new CompareStepResult(
                    TurnDirection.None,
                    false,
                    false,
                    peakLightValue,
                    reward,
                    currentValue,
                    0);
            }

            bool greaterNow = false;
            bool lessNow = false;
            TurnDirection nextTurn = TurnDirection.None;

            if (currentValue > now)
            {
                nextTurn = TurnDirection.Left;
                greaterNow = false;
                lessNow = true;
            }

            if (currentValue < now)
            {
                nextTurn = TurnDirection.Right;
                greaterNow = true;
                lessNow = false;
            }

            if (now > peakLightValue)
            {
                peakLightValue = now;
            }

            if (now == peakLightValue)
            {
                reward++;
            }

            double difference = Math.Abs(currentValue - now);

            return new CompareStepResult(
                nextTurn,
                greaterNow,
                lessNow,
                peakLightValue,
                reward,
                now,
                difference);
        }

        public static int SensorTemporalValueDifference(int currentValue, int peakValue)
        {
            return currentValue - peakValue;
        }

        public static TurnDirection Opposite(TurnDirection direction)
        {
            if (direction == TurnDirection.Left)
            {
                return TurnDirection.Right;
            }

            if (direction == TurnDirection.Right)
            {
                return TurnDirection.Left;
            }

            return TurnDirection.None;
        }
    }
}
