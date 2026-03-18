using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class RobotBehaviorTests
    {
        [Test]
        public void DecideInitialTurn_MapsLegacyCoinFlip()
        {
            Assert.That(LightSeekBehavior.DecideInitialTurn(1), Is.EqualTo(TurnDirection.Left));
            Assert.That(LightSeekBehavior.DecideInitialTurn(2), Is.EqualTo(TurnDirection.Right));
        }

        [Test]
        public void EvaluateSensorDifference_PositiveDelta_UpdatesOptimalAndKeepsTurn()
        {
            DifferenceStepResult result = LightSeekBehavior.EvaluateSensorDifference(
                currentValue: 60,
                optimalValue: 55,
                lastTurn: TurnDirection.Left,
                iteration: 4);

            Assert.That(result.Difference, Is.EqualTo(5));
            Assert.That(result.OptimalValue, Is.EqualTo(60));
            Assert.That(result.NextTurn, Is.EqualTo(TurnDirection.Left));
            Assert.That(result.Iteration, Is.EqualTo(4));
        }

        [Test]
        public void EvaluateSensorDifference_NegativeDelta_FlipsTurn()
        {
            DifferenceStepResult result = LightSeekBehavior.EvaluateSensorDifference(
                currentValue: 40,
                optimalValue: 55,
                lastTurn: TurnDirection.Left,
                iteration: 8);

            Assert.That(result.Difference, Is.EqualTo(-15));
            Assert.That(result.NextTurn, Is.EqualTo(TurnDirection.Right));
            Assert.That(result.Iteration, Is.EqualTo(8));
        }

        [Test]
        public void EvaluateSensorDifference_ZeroDelta_IncrementsIteration()
        {
            DifferenceStepResult result = LightSeekBehavior.EvaluateSensorDifference(
                currentValue: 55,
                optimalValue: 55,
                lastTurn: TurnDirection.Right,
                iteration: 2);

            Assert.That(result.Difference, Is.EqualTo(0));
            Assert.That(result.NextTurn, Is.EqualTo(TurnDirection.Right));
            Assert.That(result.Iteration, Is.EqualTo(3));
        }

        [Test]
        public void CompareNowToCurrent_WhenNowGreater_ChoosesRightAndRewardsAtPeak()
        {
            CompareStepResult result = LightSeekBehavior.CompareNowToCurrent(
                currentValue: 30,
                now: 35,
                peakLightValue: 34,
                lastTurn: TurnDirection.Left,
                reward: 1);

            Assert.That(result.NextTurn, Is.EqualTo(TurnDirection.Right));
            Assert.That(result.GreaterNow, Is.True);
            Assert.That(result.LessNow, Is.False);
            Assert.That(result.PeakLightValue, Is.EqualTo(35));
            Assert.That(result.Reward, Is.EqualTo(2));
            Assert.That(result.CurrentValue, Is.EqualTo(35));
            Assert.That(result.Difference, Is.EqualTo(5));
        }

        [Test]
        public void CompareNowToCurrent_WhenNowLess_ChoosesLeft()
        {
            CompareStepResult result = LightSeekBehavior.CompareNowToCurrent(
                currentValue: 42,
                now: 40,
                peakLightValue: 50,
                lastTurn: TurnDirection.Right,
                reward: 0);

            Assert.That(result.NextTurn, Is.EqualTo(TurnDirection.Left));
            Assert.That(result.GreaterNow, Is.False);
            Assert.That(result.LessNow, Is.True);
            Assert.That(result.PeakLightValue, Is.EqualTo(50));
            Assert.That(result.Reward, Is.EqualTo(0));
            Assert.That(result.CurrentValue, Is.EqualTo(40));
            Assert.That(result.Difference, Is.EqualTo(2));
        }
    }
}
