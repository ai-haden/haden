using Haden.RobotBehavior;
using Haden.Simulation;
using Haden.Library;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class SimulationWhirlTests
    {
        [Test]
        public void BeginSeek_UsesChoiceSourceAndMovesRobot()
        {
            var engine = new LightSeekSimulationEngine(new FixedChoiceSource(1));

            TurnDirection direction = engine.BeginSeek();

            Assert.That(direction, Is.EqualTo(TurnDirection.Left));
            Assert.That(engine.State.LastTurn, Is.EqualTo(TurnDirection.Left));
            Assert.That(engine.State.RobotX, Is.EqualTo(5));
            Assert.That(engine.State.RobotY, Is.EqualTo(0));
            Assert.That(engine.State.HeadingDegrees, Is.EqualTo(5.0));
        }

        [Test]
        public void AdvanceWhirl_PositiveDifference_UpdatesOptimalAndKeepsDirection()
        {
            var engine = new LightSeekSimulationEngine(new FixedChoiceSource(1));
            engine.BeginSeek();

            SimulationWhirlResult result = engine.AdvanceWhirl(40);

            Assert.That(result.Whirl.State, Is.EqualTo(WhirlState.One));
            Assert.That(result.Decision.Difference, Is.EqualTo(40));
            Assert.That(result.State.OptimalValue, Is.EqualTo(40));
            Assert.That(result.State.LastTurn, Is.EqualTo(TurnDirection.Left));
            Assert.That(result.State.RobotX, Is.EqualTo(10));
        }

        [Test]
        public void AdvanceWhirl_NegativeDifference_FlipsDirection()
        {
            var engine = new LightSeekSimulationEngine(new FixedChoiceSource(1));
            engine.BeginSeek();
            engine.AdvanceWhirl(60);

            SimulationWhirlResult result = engine.AdvanceWhirl(40);

            Assert.That(result.Whirl.State, Is.EqualTo(WhirlState.Two));
            Assert.That(result.Decision.Difference, Is.EqualTo(-20));
            Assert.That(result.State.LastTurn, Is.EqualTo(TurnDirection.Right));
            Assert.That(result.State.RobotY, Is.EqualTo(5));
            Assert.That(result.State.HeadingDegrees, Is.EqualTo(5.0));
        }

        [Test]
        public void AdvanceWhirl_ZeroDifference_IncrementsIteration()
        {
            var engine = new LightSeekSimulationEngine(new FixedChoiceSource(2));
            engine.BeginSeek();
            engine.AdvanceWhirl(55);

            SimulationWhirlResult result = engine.AdvanceWhirl(55);

            Assert.That(result.Decision.Difference, Is.EqualTo(0));
            Assert.That(result.State.Iteration, Is.EqualTo(1));
            Assert.That(result.State.LastTurn, Is.EqualTo(TurnDirection.Right));
        }

        private sealed class FixedChoiceSource : ITurnChoiceSource
        {
            private readonly int _value;

            public FixedChoiceSource(int value)
            {
                _value = value;
            }

            public int NextOneOrTwo()
            {
                return _value;
            }
        }
    }
}
