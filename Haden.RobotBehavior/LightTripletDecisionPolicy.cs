using System;

namespace Haden.RobotBehavior
{
    public sealed class LightTripletDecisionPolicy
    {
        private readonly int _basePower;
        private readonly int _maxPower;
        private readonly int _turnGain;
        private readonly int _turnFloor;
        private readonly int _deadband;

        public LightTripletDecisionPolicy(
            int basePower = 35,
            int maxPower = 70,
            int turnGain = 2,
            int turnFloor = 6,
            int deadband = 2)
        {
            if (basePower < 0 || basePower > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(basePower), "Base power must be between 0 and 100.");
            }

            if (maxPower < basePower || maxPower > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(maxPower), "Max power must be between base power and 100.");
            }

            if (turnGain <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(turnGain), "Turn gain must be positive.");
            }

            if (turnFloor < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(turnFloor), "Turn floor must be non-negative.");
            }

            if (deadband < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deadband), "Deadband must be non-negative.");
            }

            _basePower = basePower;
            _maxPower = maxPower;
            _turnGain = turnGain;
            _turnFloor = turnFloor;
            _deadband = deadband;
        }

        public LightTripletDecision Decide(int left, int center, int right)
        {
            int leftDelta = left - center;
            int rightDelta = right - center;
            TurnDirection direction = TurnDirection.None;
            int chosenLight = center;

            if (leftDelta > _deadband && left >= right)
            {
                direction = TurnDirection.Left;
                chosenLight = left;
            }
            else if (rightDelta > _deadband && right > left)
            {
                direction = TurnDirection.Right;
                chosenLight = right;
            }

            int contrast = Math.Max(leftDelta, rightDelta);
            int turnMagnitude = 0;
            if (direction != TurnDirection.None)
            {
                turnMagnitude = Math.Clamp(_turnFloor + (Math.Max(0, contrast) * _turnGain), 0, _maxPower - _basePower);
            }

            int leftWheel = _basePower;
            int rightWheel = _basePower;
            if (direction == TurnDirection.Left)
            {
                leftWheel = Math.Clamp(_basePower - turnMagnitude, 0, 100);
                rightWheel = Math.Clamp(_basePower + turnMagnitude, 0, 100);
            }
            else if (direction == TurnDirection.Right)
            {
                leftWheel = Math.Clamp(_basePower + turnMagnitude, 0, 100);
                rightWheel = Math.Clamp(_basePower - turnMagnitude, 0, 100);
            }

            double confidence = 0.0;
            if (direction != TurnDirection.None)
            {
                confidence = Math.Clamp((double)Math.Abs(chosenLight - center) / Math.Max(1.0, Math.Abs(chosenLight)), 0.0, 1.0);
            }

            return new LightTripletDecision(
                direction,
                leftWheel,
                rightWheel,
                contrast,
                confidence,
                chosenLight);
        }
    }

    public readonly struct LightTripletDecision
    {
        public LightTripletDecision(
            TurnDirection direction,
            int leftWheelPower,
            int rightWheelPower,
            int contrast,
            double confidence,
            int chosenLight)
        {
            Direction = direction;
            LeftWheelPower = leftWheelPower;
            RightWheelPower = rightWheelPower;
            Contrast = contrast;
            Confidence = confidence;
            ChosenLight = chosenLight;
        }

        public TurnDirection Direction { get; }
        public int LeftWheelPower { get; }
        public int RightWheelPower { get; }
        public int Contrast { get; }
        public double Confidence { get; }
        public int ChosenLight { get; }
    }
}
