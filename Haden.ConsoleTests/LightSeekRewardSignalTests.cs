using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class LightSeekRewardSignalTests
    {
        [Test]
        public void Compute_BumpPressed_ReturnsTerminalReward()
        {
            double reward = LightSeekRewardSignal.Compute(delta: -20, bumpPressed: true);

            Assert.That(reward, Is.EqualTo(10.0));
        }

        [Test]
        public void Compute_PositiveDelta_ReturnsPositiveReward()
        {
            double reward = LightSeekRewardSignal.Compute(delta: 6, bumpPressed: false);

            Assert.That(reward, Is.GreaterThan(1.0));
        }

        [Test]
        public void Compute_NegativeDelta_ReturnsPenalty()
        {
            double reward = LightSeekRewardSignal.Compute(delta: -6, bumpPressed: false);

            Assert.That(reward, Is.LessThan(0.0));
        }
    }
}
