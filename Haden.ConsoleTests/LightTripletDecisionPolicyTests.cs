using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class LightTripletDecisionPolicyTests
    {
        [Test]
        public void Decide_LeftBrighter_ChoosesLeftTurningWheelCommand()
        {
            var policy = new LightTripletDecisionPolicy(
                basePower: 35,
                maxPower: 70,
                turnGain: 2,
                turnFloor: 6,
                deadband: 2);

            LightTripletDecision decision = policy.Decide(left: 60, center: 45, right: 40);

            Assert.That(decision.Direction, Is.EqualTo(TurnDirection.Left));
            Assert.That(decision.RightWheelPower, Is.GreaterThan(decision.LeftWheelPower));
            Assert.That(decision.Confidence, Is.GreaterThan(0.0));
        }

        [Test]
        public void Decide_RightBrighter_ChoosesRightTurningWheelCommand()
        {
            var policy = new LightTripletDecisionPolicy(deadband: 2);

            LightTripletDecision decision = policy.Decide(left: 30, center: 40, right: 58);

            Assert.That(decision.Direction, Is.EqualTo(TurnDirection.Right));
            Assert.That(decision.LeftWheelPower, Is.GreaterThan(decision.RightWheelPower));
        }

        [Test]
        public void Decide_ContrastBelowDeadband_StaysStraight()
        {
            var policy = new LightTripletDecisionPolicy(deadband: 3);

            LightTripletDecision decision = policy.Decide(left: 41, center: 40, right: 42);

            Assert.That(decision.Direction, Is.EqualTo(TurnDirection.None));
            Assert.That(decision.LeftWheelPower, Is.EqualTo(35));
            Assert.That(decision.RightWheelPower, Is.EqualTo(35));
            Assert.That(decision.Confidence, Is.EqualTo(0.0));
        }
    }
}
