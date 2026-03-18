using Haden.LinuxMigrationReview;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class AdaptiveSeekCycleTests
    {
        [Test]
        public void IncreaseBrightness_IncreasesDegreesWithoutDirectionFlip()
        {
            var cycle = new AdaptiveSeekCycle(initialPower: 15, initialDegrees: 15, initialDirection: 1);

            AdaptiveSeekStep step = cycle.Advance(previousLightValue: 10, currentLightValue: 14);

            Assert.That(step.Delta, Is.EqualTo(4));
            Assert.That(step.Direction, Is.EqualTo(1));
            Assert.That(step.Degrees, Is.EqualTo(20));
            Assert.That(step.Power, Is.EqualTo(15));
        }

        [Test]
        public void WeakOrNegativeDelta_FlipsDirectionAndReducesDegreesWithinBounds()
        {
            var cycle = new AdaptiveSeekCycle(initialPower: 15, initialDegrees: 15, initialDirection: 1);

            AdaptiveSeekStep step = cycle.Advance(previousLightValue: 20, currentLightValue: 20);

            Assert.That(step.Delta, Is.EqualTo(0));
            Assert.That(step.Direction, Is.EqualTo(-1));
            Assert.That(step.Degrees, Is.EqualTo(10));
        }

        [Test]
        public void SeekCycle_RespectsDegreeBounds()
        {
            var cycle = new AdaptiveSeekCycle(initialPower: 15, initialDegrees: 35, initialDirection: 1);

            AdaptiveSeekStep maxed = cycle.Advance(previousLightValue: 10, currentLightValue: 20);
            Assert.That(maxed.Degrees, Is.EqualTo(35));

            cycle = new AdaptiveSeekCycle(initialPower: 15, initialDegrees: 10, initialDirection: 1);
            AdaptiveSeekStep mined = cycle.Advance(previousLightValue: 10, currentLightValue: 10);
            Assert.That(mined.Degrees, Is.EqualTo(10));
        }
    }
}
