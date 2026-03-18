using System;
using Haden.Library;
using Haden.RobotBehavior;

namespace Haden.Simulation
{
    public interface ITurnChoiceSource
    {
        int NextOneOrTwo();
    }

    public sealed class RandomTurnChoiceSource : ITurnChoiceSource
    {
        private readonly Random _random;

        public RandomTurnChoiceSource(int seed = 0)
        {
            _random = seed == 0 ? new Random() : new Random(seed);
        }

        public int NextOneOrTwo()
        {
            return _random.Next(1, 3);
        }
    }

    public sealed class SimulationState
    {
        public int CurrentValue { get; set; }
        public int OptimalValue { get; set; }
        public int DifferenceValue { get; set; }
        public int Iteration { get; set; }
        public TurnDirection LastTurn { get; set; }
        public int RobotX { get; set; }
        public int RobotY { get; set; }
        public double HeadingDegrees { get; set; }
    }

    public sealed class SimulationWhirlResult
    {
        public SimulationWhirlResult(SimulationState state, DifferenceStepResult decision, WhirlTickResult whirl)
        {
            State = state;
            Decision = decision;
            Whirl = whirl;
        }

        public SimulationState State { get; }
        public DifferenceStepResult Decision { get; }
        public WhirlTickResult Whirl { get; }
    }

    // Headless simulation engine extracted from legacy simulator forms:
    // - random first turn
    // - sensor-difference steering
    // - bounded left/right turn kinematics updates
    public sealed class LightSeekSimulationEngine
    {
        private readonly ITurnChoiceSource _turnChoiceSource;
        private readonly WhirlEngine _whirlEngine;

        public LightSeekSimulationEngine(ITurnChoiceSource turnChoiceSource = null)
        {
            _turnChoiceSource = turnChoiceSource ?? new RandomTurnChoiceSource();
            _whirlEngine = new WhirlEngine();
            State = new SimulationState
            {
                CurrentValue = 0,
                OptimalValue = 0,
                DifferenceValue = 0,
                Iteration = 0,
                LastTurn = TurnDirection.None,
                RobotX = 0,
                RobotY = 0,
                HeadingDegrees = 0
            };
        }

        public SimulationState State { get; }
        public WhirlState CurrentWhirlState => _whirlEngine.CurrentState;

        public TurnDirection BeginSeek()
        {
            TurnDirection initial = LightSeekBehavior.DecideInitialTurn(_turnChoiceSource.NextOneOrTwo());
            ApplyTurn(initial);
            State.LastTurn = initial;
            return initial;
        }

        public SimulationWhirlResult AdvanceWhirl(int currentSensorValue)
        {
            WhirlTickResult whirl = _whirlEngine.AdvanceTick();
            State.CurrentValue = currentSensorValue;

            DifferenceStepResult decision = LightSeekBehavior.EvaluateSensorDifference(
                currentSensorValue,
                State.OptimalValue,
                State.LastTurn,
                State.Iteration);

            State.DifferenceValue = decision.Difference;
            State.OptimalValue = decision.OptimalValue;
            State.Iteration = decision.Iteration;

            if (decision.NextTurn != TurnDirection.None)
            {
                ApplyTurn(decision.NextTurn);
                State.LastTurn = decision.NextTurn;
            }

            return new SimulationWhirlResult(CloneState(), decision, whirl);
        }

        public void ApplyTurn(TurnDirection direction)
        {
            if (direction == TurnDirection.Left)
            {
                State.RobotX += 5;
                State.HeadingDegrees += 5.0;
                return;
            }

            if (direction == TurnDirection.Right)
            {
                State.RobotY += 5;
                State.HeadingDegrees -= 5.0;
            }
        }

        private SimulationState CloneState()
        {
            return new SimulationState
            {
                CurrentValue = State.CurrentValue,
                OptimalValue = State.OptimalValue,
                DifferenceValue = State.DifferenceValue,
                Iteration = State.Iteration,
                LastTurn = State.LastTurn,
                RobotX = State.RobotX,
                RobotY = State.RobotY,
                HeadingDegrees = State.HeadingDegrees
            };
        }
    }
}
