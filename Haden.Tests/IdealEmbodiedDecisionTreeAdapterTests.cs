using Haden.Library.Algorithm;
using Haden.Library.DecisionTree;
using NUnit.Framework;

namespace Haden.Tests
{
    [TestFixture]
    public class IdealEmbodiedDecisionTreeAdapterTests
    {
        [Test]
        public void Export_CreatesExperimentFeatureWithExpectedValueNodes()
        {
            var seeker = new IdealEmbodiedLightSeeker(
                experiments: new[] { "scan-left", "scan-right" },
                initialExperiment: "scan-left");

            seeker.SetResultValence("dark", -1.0);
            seeker.SetResultValence("bright", 1.0);
            seeker.Step(exp => exp == "scan-left" ? "dark" : "bright");
            seeker.Step(exp => exp == "scan-left" ? "dark" : "bright");

            TreeNode root = IdealEmbodiedDecisionTreeAdapter.Export(seeker);

            Assert.That(root.Type, Is.EqualTo(TreeNodeType.Root));
            Assert.That(root.SubNodes.Count, Is.EqualTo(1));
            Assert.That(root.SubNodes[0].Name, Is.EqualTo("experiment"));
            Assert.That(root.SubNodes[0].SubNodes.Count, Is.EqualTo(2));
        }
    }
}
