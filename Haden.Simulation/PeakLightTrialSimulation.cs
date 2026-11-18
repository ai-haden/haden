using System;
using Haden.RobotBehavior;

namespace Haden.Simulation
{
    public sealed class PeakLightTrialConfig
    {
        public int Iterations { get; set; } = 60;
        public double InitialRobotHeadingDegrees { get; set; } = 0;
        public double InitialSensorScanDegrees { get; set; } = 0;
        public double LightSourceHeadingDegrees { get; set; } = 50;
        public double LightSourceIntensity { get; set; } = 100;
        public double BeamHalfWidthDegrees { get; set; } = 28;
        public int PerturbationIteration { get; set; } = -1;
        public double PerturbationHeadingDegrees { get; set; } = 0;
        public double HeadingGainPerWheelDelta { get; set; } = 0.12;
        public double SensorScanLimitDegrees { get; set; } = 90;
        public int PeakTolerance { get; set; } = 2;
    }

    public sealed class PeakLightTrialMetrics
    {
        public int TimeToPeakIteration { get; set; }
        public int MaxStableTicks { get; set; }
        public int RecoveryEvents { get; set; }
        public int PeakLightValue { get; set; }
        public int FinalSensorValue { get; set; }
        public double FinalHeadingDegrees { get; set; }
    }

    public sealed class PeakLightTrialSimulation
    {
        private readonly PeakLightSteeringPolicy _policy;

        public PeakLightTrialSimulation(PeakLightSteeringPolicy policy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public PeakLightTrialMetrics Run(PeakLightTrialConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            double robotHeading = config.InitialRobotHeadingDegrees;
            double sensorScanHeading = config.InitialSensorScanDegrees;
            double lightSourceHeading = config.LightSourceHeadingDegrees;
            int timeToPeak = -1;
            int maxStableTicks = 0;
            PeakLightSteeringStep step = default;

            for (int i = 0; i < config.Iterations; i++)
            {
                if (config.PerturbationIteration >= 0 && i == config.PerturbationIteration)
                {
                    lightSourceHeading += config.PerturbationHeadingDegrees;
                }

                int sensorValue = SenseLight(
                    robotHeading + sensorScanHeading,
                    lightSourceHeading,
                    config.LightSourceIntensity,
                    config.BeamHalfWidthDegrees);

                step = _policy.Advance(sensorValue);
                maxStableTicks = Math.Max(maxStableTicks, step.PeakStableTicks);

                int peakThreshold = _policy.PeakLightValue - Math.Max(0, config.PeakTolerance);
                if (timeToPeak < 0 && sensorValue >= peakThreshold)
                {
                    timeToPeak = i;
                }

                sensorScanHeading = Math.Clamp(
                    sensorScanHeading + (step.ScanDirection * step.ScanDegrees),
                    -Math.Abs(config.SensorScanLimitDegrees),
                    Math.Abs(config.SensorScanLimitDegrees));

                double wheelDelta = step.RightWheelPower - step.LeftWheelPower;
                robotHeading += wheelDelta * config.HeadingGainPerWheelDelta;
            }

            return new PeakLightTrialMetrics
            {
                TimeToPeakIteration = timeToPeak,
                MaxStableTicks = maxStableTicks,
                RecoveryEvents = step.RecoveryEvents,
                PeakLightValue = _policy.PeakLightValue,
                FinalSensorValue = step.SensorValue,
                FinalHeadingDegrees = robotHeading
            };
        }

        private static int SenseLight(
            double sensorHeadingDegrees,
            double sourceHeadingDegrees,
            double sourceIntensity,
            double beamHalfWidthDegrees)
        {
            double offset = NormalizeDegrees(sourceHeadingDegrees - sensorHeadingDegrees);
            double normalized = Math.Max(0, 1.0 - (Math.Abs(offset) / Math.Max(1.0, beamHalfWidthDegrees)));
            return (int)Math.Round(sourceIntensity * normalized);
        }

        private static double NormalizeDegrees(double degrees)
        {
            double normalized = degrees % 360.0;
            if (normalized > 180.0)
            {
                normalized -= 360.0;
            }
            else if (normalized < -180.0)
            {
                normalized += 360.0;
            }

            return normalized;
        }
    }
}
