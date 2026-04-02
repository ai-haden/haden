using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class BoredomDirectionPolicyTests
    {
        [Test]
        public void Next_FlatUncertainSequence_TriggersBiasFlipExploration()
        {
            var policy = new BoredomDirectionPolicy(
                flatDeltaThreshold: 1,
                flatLimit: 2,
                uncertainLimit: 2,
                peakUnsureMargin: 1,
                peakConfirmTicks: 3,
                uncertainAlternateSteps: 5);
            var state = new BoredomDirectionState(
                biasDirection: 1,
                flatCount: 0,
                uncertainCount: 0,
                lastActionDirection: 0,
                sameDirectionStuckCount: 0,
                uncertainExploreCount: 0);

            BoredomDirectionStep first = policy.Next(
                state,
                hasDirectionalEvidence: false,
                evidenceDirection: 0,
                delta: 0,
                smoothedLight: 20,
                peakLightValue: 40,
                peakStableTicks: 0);
            BoredomDirectionStep second = policy.Next(
                first.State,
                hasDirectionalEvidence: false,
                evidenceDirection: 0,
                delta: 0,
                smoothedLight: 20,
                peakLightValue: 40,
                peakStableTicks: 0);

            Assert.That(first.Exploring, Is.False);
            Assert.That(second.Exploring, Is.True);
            Assert.That(second.BoredomTriggered, Is.True);
            Assert.That(second.ActionDirection, Is.EqualTo(-1));
            Assert.That(second.State.BiasDirection, Is.EqualTo(-1));
        }

        [Test]
        public void Next_HasEvidence_UsesEvidenceDirectionAndResetsCounters()
        {
            var policy = new BoredomDirectionPolicy(flatLimit: 3, uncertainLimit: 2);
            var state = new BoredomDirectionState(
                biasDirection: -1,
                flatCount: 2,
                uncertainCount: 2,
                lastActionDirection: -1,
                sameDirectionStuckCount: 1,
                uncertainExploreCount: 0);

            BoredomDirectionStep step = policy.Next(
                state,
                hasDirectionalEvidence: true,
                evidenceDirection: 1,
                delta: 2,
                smoothedLight: 30,
                peakLightValue: 32,
                peakStableTicks: 3);

            Assert.That(step.ActionDirection, Is.EqualTo(1));
            Assert.That(step.Exploring, Is.False);
            Assert.That(step.State.FlatCount, Is.EqualTo(0));
            Assert.That(step.State.UncertainCount, Is.EqualTo(0));
            Assert.That(step.State.BiasDirection, Is.EqualTo(1));
        }

        [Test]
        public void Next_NearPeak_DoesNotForceExploration()
        {
            var policy = new BoredomDirectionPolicy(
                flatDeltaThreshold: 1,
                flatLimit: 1,
                uncertainLimit: 1,
                peakUnsureMargin: 2,
                peakConfirmTicks: 2);
            var state = new BoredomDirectionState(
                biasDirection: 1,
                flatCount: 0,
                uncertainCount: 0,
                lastActionDirection: 0,
                sameDirectionStuckCount: 0,
                uncertainExploreCount: 0);

            BoredomDirectionStep step = policy.Next(
                state,
                hasDirectionalEvidence: false,
                evidenceDirection: 0,
                delta: 0,
                smoothedLight: 31,
                peakLightValue: 32,
                peakStableTicks: 2);

            Assert.That(step.NearPeakConfirmed, Is.True);
            Assert.That(step.Exploring, Is.False);
            Assert.That(step.ActionDirection, Is.EqualTo(0));
        }

        [Test]
        public void Next_RepeatedSameDirectionWithoutImprovement_ForcesFlip()
        {
            var policy = new BoredomDirectionPolicy(
                flatDeltaThreshold: 1,
                flatLimit: 6,
                uncertainLimit: 2,
                peakUnsureMargin: 1,
                peakConfirmTicks: 3,
                stuckSameDirectionLimit: 2,
                uncertainAlternateSteps: 5);
            var state = new BoredomDirectionState(
                biasDirection: 1,
                flatCount: 0,
                uncertainCount: 1,
                lastActionDirection: 1,
                sameDirectionStuckCount: 1,
                uncertainExploreCount: 1);

            BoredomDirectionStep step = policy.Next(
                state,
                hasDirectionalEvidence: false,
                evidenceDirection: 0,
                delta: 0,
                smoothedLight: 20,
                peakLightValue: 30,
                peakStableTicks: 0);

            Assert.That(step.ForcedFlip, Is.True);
            Assert.That(step.ActionDirection, Is.EqualTo(-1));
            Assert.That(step.State.BiasDirection, Is.EqualTo(-1));
            Assert.That(step.State.SameDirectionStuckCount, Is.EqualTo(0));
        }
    }
}
