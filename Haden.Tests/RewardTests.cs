using Haden.Library.Algorithm;
using NUnit.Framework;

namespace Haden.Tests
{
    [TestFixture]
    public class RewardTests
    {
        [Test]
        public void CheckTemporalDifference()
        {
            var model = new IdealReinforcementModel(learningRate: 0.5, discountFactor: 0.9, seed: 7);
            string[] actions = { "left", "right" };

            model.Update("s1", "right", reward: 2.0, nextState: "s2", nextActions: actions);
            model.Update("s2", "right", reward: 3.0, nextState: "goal", nextActions: new string[0]);
            double tdError = model.Update("s1", "right", reward: 2.0, nextState: "s2", nextActions: actions);

            double qS2Right = model.GetQValue("s2", "right");
            double qS1Right = model.GetQValue("s1", "right");

            Assert.That(qS2Right, Is.EqualTo(1.5).Within(0.000001));
            Assert.That(tdError, Is.EqualTo(2.35).Within(0.000001));
            Assert.That(qS1Right, Is.EqualTo(2.175).Within(0.000001));
        }

        [Test]
        public void SeekReward()
        {
            var model = new IdealReinforcementModel(learningRate: 0.3, discountFactor: 0.8, seed: 42);
            string[] actions = { "left", "right" };
            string state = "seek-light";

            for (int i = 0; i < 400; i++)
            {
                string action = model.SelectAction(state, actions, epsilon: 0.10);
                double reward = action == "right" ? 1.0 : -0.2;
                model.Update(state, action, reward, state, actions);
            }

            string greedy = model.GetGreedyAction(state, actions);
            double qLeft = model.GetQValue(state, "left");
            double qRight = model.GetQValue(state, "right");

            Assert.That(greedy, Is.EqualTo("right"));
            Assert.That(qRight, Is.GreaterThan(qLeft));
            Assert.That(qRight, Is.GreaterThan(2.5));
        }
    }
}
