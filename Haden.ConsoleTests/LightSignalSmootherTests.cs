using Haden.RobotBehavior;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class LightSignalSmootherTests
    {
        [Test]
        public void AddSample_WindowThree_AveragesMostRecentValues()
        {
            var smoother = new LightSignalSmoother(windowSize: 3);

            int first = smoother.AddSample(30);
            int second = smoother.AddSample(36);
            int third = smoother.AddSample(42);
            int fourth = smoother.AddSample(48);

            Assert.That(first, Is.EqualTo(30));
            Assert.That(second, Is.EqualTo(33));
            Assert.That(third, Is.EqualTo(36));
            Assert.That(fourth, Is.EqualTo(42));
        }

        [Test]
        public void Constructor_RejectsNonPositiveWindow()
        {
            Assert.That(() => new LightSignalSmoother(0), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }
    }
}
