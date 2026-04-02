using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class PeakLightSteeringPolicyTests
    {
        [Test]
        public void Advance_FirstSample_SeedsPeakAndKeepsBalancedWheelPower()
        {
            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: 20,
                scanDegreesMin: 10,
                scanDegreesMax: 20,
                wheelBasePower: 30,
                wheelMaxPower: 60,
                wheelTurnGain: 2,
                wheelTurnFloor: 4,
                deltaDeadband: 2,
                peakTolerance: 2);

            PeakLightSteeringStep step = policy.Advance(40);

            Assert.That(step.PeakLightValue, Is.EqualTo(40));
            Assert.That(step.Delta, Is.EqualTo(0));
            Assert.That(step.LeftWheelPower, Is.EqualTo(30));
            Assert.That(step.RightWheelPower, Is.EqualTo(30));
            Assert.That(step.ScanMotorPower, Is.EqualTo(20));
        }

        [Test]
        public void Advance_PositiveDelta_KeepsScanDirectionAndTurnsTowardGradient()
        {
            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: 20,
                scanDegreesMin: 10,
                scanDegreesMax: 30,
                scanDegreesStep: 5,
                wheelBasePower: 30,
                wheelMaxPower: 70,
                wheelTurnGain: 2,
                wheelTurnFloor: 4,
                deltaDeadband: 2,
                peakTolerance: 2);

            policy.Advance(30);
            PeakLightSteeringStep step = policy.Advance(38);

            Assert.That(step.Delta, Is.EqualTo(8));
            Assert.That(step.ScanDirection, Is.EqualTo(1));
            Assert.That(step.ScanDegrees, Is.EqualTo(15));
            Assert.That(step.RightWheelPower, Is.GreaterThan(step.LeftWheelPower));
            Assert.That(step.PeakLightValue, Is.EqualTo(38));
        }

        [Test]
        public void Advance_NegativeDelta_FlipsScanDirectionAndTracksRecovery()
        {
            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: 20,
                scanDegreesMin: 10,
                scanDegreesMax: 30,
                scanDegreesStep: 5,
                wheelBasePower: 30,
                wheelMaxPower: 70,
                wheelTurnGain: 2,
                wheelTurnFloor: 4,
                deltaDeadband: 2,
                peakTolerance: 2);

            policy.Advance(50);
            PeakLightSteeringStep drop = policy.Advance(40);
            PeakLightSteeringStep recover = policy.Advance(49);

            Assert.That(drop.ScanDirection, Is.EqualTo(-1));
            Assert.That(drop.LeftWheelPower, Is.GreaterThan(drop.RightWheelPower));
            Assert.That(drop.PeakStableTicks, Is.EqualTo(0));
            Assert.That(recover.RecoveryEvents, Is.EqualTo(1));
            Assert.That(recover.PeakStableTicks, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void Advance_SmallDelta_StaysInDeadbandAndKeepsWheelsBalanced()
        {
            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: 18,
                scanDegreesMin: 10,
                scanDegreesMax: 25,
                scanDegreesStep: 5,
                wheelBasePower: 32,
                wheelMaxPower: 70,
                wheelTurnGain: 2,
                wheelTurnFloor: 6,
                deltaDeadband: 2,
                peakTolerance: 2);

            policy.Advance(60);
            PeakLightSteeringStep step = policy.Advance(61);

            Assert.That(step.Delta, Is.EqualTo(1));
            Assert.That(step.LeftWheelPower, Is.EqualTo(32));
            Assert.That(step.RightWheelPower, Is.EqualTo(32));
        }
    }
}
