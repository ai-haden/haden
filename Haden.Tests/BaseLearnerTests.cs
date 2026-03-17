using System.Collections.Generic;
using Haden.Library.Algorithm;
using NUnit.Framework;

namespace Haden.Tests
{
    [TestFixture]
    public class BaseLearnerTests
    {
        [Test]
        public void GetNextAction_ReturnsActionIndexFromPolicyLabel()
        {
            var learner = new BaseLearner(new TestStateActionSpace());
            learner.Policy[1] = "right";

            int actionIndex = learner.GetNextAction(1);

            Assert.That(actionIndex, Is.EqualTo(1));
        }

        private sealed class TestStateActionSpace : StateActionSpace
        {
            public override List<int> GetListofStates()
            {
                return new List<int> { 1, 2 };
            }

            public override List<string> GetListofActions()
            {
                return new List<string> { "left", "right" };
            }

            public override List<string> GetEligibleActions(int state)
            {
                return new List<string> { "left", "right" };
            }
        }
    }
}
