using System;

namespace Haden.RobotBehavior
{
    public sealed class PeakLightSteeringPolicy
    {
        private readonly int _deltaDeadband;
        private readonly int _scanDegreesMin;
        private readonly int _scanDegreesMax;
        private readonly int _scanDegreesStep;
        private readonly int _scanMotorPower;
        private readonly int _wheelBasePower;
        private readonly int _wheelMaxPower;
        private readonly int _wheelTurnGain;
        private readonly int _wheelTurnFloor;
        private readonly int _peakTolerance;

        private int _previousLightValue;
        private bool _initialized;
        private int _scanDirection;
        private int _scanDegrees;
        private int _recoveryPending;

        public PeakLightSteeringPolicy(
            int scanMotorPower = 18,
            int scanDegreesMin = 10,
            int scanDegreesMax = 40,
            int scanDegreesStep = 5,
            int wheelBasePower = 35,
            int wheelMaxPower = 70,
            int wheelTurnGain = 2,
            int wheelTurnFloor = 6,
            int deltaDeadband = 2,
            int peakTolerance = 2)
        {
            if (scanDegreesMin <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scanDegreesMin), "Minimum scan degrees must be positive.");
            }

            if (scanDegreesMax < scanDegreesMin)
            {
                throw new ArgumentOutOfRangeException(nameof(scanDegreesMax), "Maximum scan degrees must be greater than or equal to minimum.");
            }

            if (scanDegreesStep <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(scanDegreesStep), "Scan degree step must be positive.");
            }

            if (wheelBasePower < 0 || wheelBasePower > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(wheelBasePower), "Wheel base power must be between 0 and 100.");
            }

            if (wheelMaxPower < wheelBasePower || wheelMaxPower > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(wheelMaxPower), "Wheel max power must be between base power and 100.");
            }

            if (wheelTurnGain <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(wheelTurnGain), "Wheel turn gain must be positive.");
            }

            if (wheelTurnFloor < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(wheelTurnFloor), "Wheel turn floor must be non-negative.");
            }

            if (deltaDeadband < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaDeadband), "Delta deadband must be non-negative.");
            }

            if (peakTolerance < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(peakTolerance), "Peak tolerance must be non-negative.");
            }

            _scanMotorPower = Math.Clamp(scanMotorPower, 1, 100);
            _scanDegreesMin = scanDegreesMin;
            _scanDegreesMax = scanDegreesMax;
            _scanDegreesStep = scanDegreesStep;
            _wheelBasePower = wheelBasePower;
            _wheelMaxPower = wheelMaxPower;
            _wheelTurnGain = wheelTurnGain;
            _wheelTurnFloor = wheelTurnFloor;
            _deltaDeadband = deltaDeadband;
            _peakTolerance = peakTolerance;

            _scanDirection = 1;
            _scanDegrees = _scanDegreesMin;
            PeakLightValue = int.MinValue;
        }

        public int PeakLightValue { get; private set; }
        public int PeakStableTicks { get; private set; }
        public int RecoveryEvents { get; private set; }
        public int Iteration { get; private set; }

        public PeakLightSteeringStep Advance(int currentLightValue)
        {
            int delta = _initialized ? currentLightValue - _previousLightValue : 0;

            if (!_initialized)
            {
                PeakLightValue = currentLightValue;
                PeakStableTicks = 1;
                _initialized = true;
            }
            else
            {
                if (currentLightValue > PeakLightValue)
                {
                    PeakLightValue = currentLightValue;
                    PeakStableTicks = 1;
                    if (_recoveryPending == 1)
                    {
                        RecoveryEvents++;
                        _recoveryPending = 0;
                    }
                }
                else if (Math.Abs(PeakLightValue - currentLightValue) <= _peakTolerance)
                {
                    PeakStableTicks++;
                    if (_recoveryPending == 1)
                    {
                        RecoveryEvents++;
                        _recoveryPending = 0;
                    }
                }
                else
                {
                    PeakStableTicks = 0;
                    _recoveryPending = 1;
                }
            }

            int steeringDirection = 0;
            if (delta > _deltaDeadband)
            {
                _scanDegrees = Math.Min(_scanDegrees + _scanDegreesStep, _scanDegreesMax);
                steeringDirection = _scanDirection;
            }
            else if (delta < -_deltaDeadband)
            {
                _scanDirection *= -1;
                _scanDegrees = Math.Max(_scanDegrees - _scanDegreesStep, _scanDegreesMin);
                steeringDirection = _scanDirection;
            }
            else
            {
                steeringDirection = 0;
            }

            int turnMagnitude = Math.Clamp(Math.Abs(delta) * _wheelTurnGain + _wheelTurnFloor, 0, _wheelMaxPower - _wheelBasePower);
            if (steeringDirection == 0)
            {
                turnMagnitude = 0;
            }

            int leftWheelPower = _wheelBasePower;
            int rightWheelPower = _wheelBasePower;

            if (steeringDirection > 0)
            {
                leftWheelPower = Math.Clamp(_wheelBasePower - turnMagnitude, 0, 100);
                rightWheelPower = Math.Clamp(_wheelBasePower + turnMagnitude, 0, 100);
            }
            else if (steeringDirection < 0)
            {
                leftWheelPower = Math.Clamp(_wheelBasePower + turnMagnitude, 0, 100);
                rightWheelPower = Math.Clamp(_wheelBasePower - turnMagnitude, 0, 100);
            }

            _previousLightValue = currentLightValue;
            Iteration++;

            return new PeakLightSteeringStep(
                currentLightValue,
                delta,
                _scanDirection,
                _scanMotorPower * _scanDirection,
                _scanDegrees,
                leftWheelPower,
                rightWheelPower,
                PeakLightValue,
                PeakStableTicks,
                RecoveryEvents,
                Iteration);
        }
    }

    public readonly struct PeakLightSteeringStep
    {
        public PeakLightSteeringStep(
            int sensorValue,
            int delta,
            int scanDirection,
            int scanMotorPower,
            int scanDegrees,
            int leftWheelPower,
            int rightWheelPower,
            int peakLightValue,
            int peakStableTicks,
            int recoveryEvents,
            int iteration)
        {
            SensorValue = sensorValue;
            Delta = delta;
            ScanDirection = scanDirection;
            ScanMotorPower = scanMotorPower;
            ScanDegrees = scanDegrees;
            LeftWheelPower = leftWheelPower;
            RightWheelPower = rightWheelPower;
            PeakLightValue = peakLightValue;
            PeakStableTicks = peakStableTicks;
            RecoveryEvents = recoveryEvents;
            Iteration = iteration;
        }

        public int SensorValue { get; }
        public int Delta { get; }
        public int ScanDirection { get; }
        public int ScanMotorPower { get; }
        public int ScanDegrees { get; }
        public int LeftWheelPower { get; }
        public int RightWheelPower { get; }
        public int PeakLightValue { get; }
        public int PeakStableTicks { get; }
        public int RecoveryEvents { get; }
        public int Iteration { get; }
    }
}
