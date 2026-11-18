using Haden.RobotBehavior;
using Haden.Simulation;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class PeakLightTrialSimulationTests
    {
        [Test]
        public void Run_ConvergesTowardLightPeakWithinBudget()
        {
            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: 18,
                scanDegreesMin: 10,
                scanDegreesMax: 35,
                scanDegreesStep: 5,
                wheelBasePower: 35,
                wheelMaxPower: 70,
                wheelTurnGain: 2,
                wheelTurnFloor: 6,
                deltaDeadband: 2,
                peakTolerance: 2);

            var simulation = new PeakLightTrialSimulation(policy);
            var config = new PeakLightTrialConfig
            {
                Iterations = 60,
                LightSourceHeadingDegrees = 45,
                LightSourceIntensity = 100,
                BeamHalfWidthDegrees = 30,
                HeadingGainPerWheelDelta = 0.2,
                PeakTolerance = 3
            };

            PeakLightTrialMetrics metrics = simulation.Run(config);

            Assert.That(metrics.TimeToPeakIteration, Is.GreaterThanOrEqualTo(0));
            Assert.That(metrics.TimeToPeakIteration, Is.LessThan(60));
            Assert.That(metrics.PeakLightValue, Is.GreaterThanOrEqualTo(85));
            Assert.That(metrics.MaxStableTicks, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Run_WithPerturbation_RecordsRecoveryEvents()
        {
            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: 18,
                scanDegreesMin: 10,
                scanDegreesMax: 35,
                scanDegreesStep: 5,
                wheelBasePower: 35,
                wheelMaxPower: 70,
                wheelTurnGain: 2,
                wheelTurnFloor: 6,
                deltaDeadband: 2,
                peakTolerance: 2);

            var simulation = new PeakLightTrialSimulation(policy);
            var config = new PeakLightTrialConfig
            {
                Iterations = 80,
                LightSourceHeadingDegrees = 30,
                PerturbationIteration = 35,
                PerturbationHeadingDegrees = 35,
                LightSourceIntensity = 100,
                BeamHalfWidthDegrees = 32,
                HeadingGainPerWheelDelta = 0.2,
                PeakTolerance = 3
            };

            PeakLightTrialMetrics metrics = simulation.Run(config);

            Assert.That(metrics.RecoveryEvents, Is.GreaterThanOrEqualTo(1));
            Assert.That(metrics.PeakLightValue, Is.GreaterThanOrEqualTo(85));
        }
    }
}
