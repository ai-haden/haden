using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class LegacyAutonomousLightSeekEngineTests
    {
        [Test]
        public void Step_FirstIteration_AppliesInitialDisturbanceAndSeedsPeak()
        {
            var engine = new LegacyAutonomousLightSeekEngine();

            LegacyAutonomousLightSeekStep step = engine.Step(50);

            Assert.That(step.Iteration, Is.EqualTo(1));
            Assert.That(step.Turn, Is.EqualTo(TurnDirection.Right));
            Assert.That(step.PeakLightValue, Is.EqualTo(50));
            Assert.That(step.Reward, Is.EqualTo(1));
            Assert.That(step.CurrentValue, Is.EqualTo(50));
        }

        [Test]
        public void Step_SecondIteration_WithLowerLightTurnsLeft()
        {
            var engine = new LegacyAutonomousLightSeekEngine();
            engine.Step(50);

            LegacyAutonomousLightSeekStep step = engine.Step(45);

            Assert.That(step.Iteration, Is.EqualTo(2));
            Assert.That(step.Turn, Is.EqualTo(TurnDirection.Left));
            Assert.That(step.CurrentValue, Is.EqualTo(45));
            Assert.That(step.PeakLightValue, Is.EqualTo(50));
        }

        [Test]
        public void Step_SubsequentIteration_TracksPeakAndReward()
        {
            var engine = new LegacyAutonomousLightSeekEngine();
            engine.Step(30);
            engine.Step(25);

            LegacyAutonomousLightSeekStep step = engine.Step(40);

            Assert.That(step.Iteration, Is.EqualTo(3));
            Assert.That(step.Turn, Is.EqualTo(TurnDirection.Right));
            Assert.That(step.PeakLightValue, Is.EqualTo(40));
            Assert.That(step.Reward, Is.GreaterThanOrEqualTo(2));
            Assert.That(engine.Then.Count, Is.EqualTo(2));
        }
    }
}
