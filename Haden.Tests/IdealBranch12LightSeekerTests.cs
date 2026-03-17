using Haden.Library.Algorithm;
using NUnit.Framework;

namespace Haden.Tests
{
    [TestFixture]
    public class IdealBranch12LightSeekerTests
    {
        [Test]
        public void Branch01_EmbodiedLoop_TransitionsToBoredAndSwitchesExperiment()
        {
            var seeker = new IdealEmbodiedLightSeeker(
                experiments: new[] { "scan-left", "scan-right" },
                initialExperiment: "scan-left",
                boredomThreshold: 3);

            EmbodiedStepTrace t0 = seeker.Step(DeterministicEnvironment);
            EmbodiedStepTrace t1 = seeker.Step(DeterministicEnvironment);
            EmbodiedStepTrace t2 = seeker.Step(DeterministicEnvironment);
            EmbodiedStepTrace t3 = seeker.Step(DeterministicEnvironment);
            EmbodiedStepTrace t4 = seeker.Step(DeterministicEnvironment);

            Assert.That(t0.Experiment, Is.EqualTo("scan-left"));
            Assert.That(t0.Mood, Is.EqualTo(EmbodiedMood.Frustrated));
            Assert.That(t1.Mood, Is.EqualTo(EmbodiedMood.SelfSatisfied));
            Assert.That(t2.Mood, Is.EqualTo(EmbodiedMood.SelfSatisfied));
            Assert.That(t3.Mood, Is.EqualTo(EmbodiedMood.Bored));
            Assert.That(t4.Experiment, Is.EqualTo("scan-right"));
        }

        [Test]
        public void Branch02_InteractionalMotivation_PrefersBrightResult()
        {
            var seeker = new IdealEmbodiedLightSeeker(
                experiments: new[] { "scan-left", "scan-right" },
                initialExperiment: "scan-left",
                boredomThreshold: 2);

            seeker.SetResultValence("dark", -1.0);
            seeker.SetResultValence("bright", 1.0);

            // Gather first experiences for both experiments.
            seeker.Step(DeterministicEnvironment);
            seeker.Step(DeterministicEnvironment);
            EmbodiedStepTrace t2 = seeker.Step(DeterministicEnvironment);
            EmbodiedStepTrace t3 = seeker.Step(DeterministicEnvironment);

            Assert.That(t2.Experiment, Is.EqualTo("scan-right"));
            Assert.That(t2.Result, Is.EqualTo("bright"));
            Assert.That(t2.Mood, Is.EqualTo(EmbodiedMood.Pleased));
            Assert.That(t3.Experiment, Is.EqualTo("scan-right"));
            Assert.That(t3.Mood, Is.EqualTo(EmbodiedMood.Pleased));
        }

        private static string DeterministicEnvironment(string experiment)
        {
            return experiment == "scan-left" ? "dark" : "bright";
        }
    }
}
