using System;
using System.Collections.Generic;

namespace Haden.RobotBehavior
{
    public sealed class LegacyAutonomousLightSeekStep
    {
        public LegacyAutonomousLightSeekStep(
            int sensorValue,
            int iteration,
            double currentValue,
            double nowValue,
            double peakLightValue,
            int reward,
            TurnDirection turn)
        {
            SensorValue = sensorValue;
            Iteration = iteration;
            CurrentValue = currentValue;
            NowValue = nowValue;
            PeakLightValue = peakLightValue;
            Reward = reward;
            Turn = turn;
        }

        public int SensorValue { get; }
        public int Iteration { get; }
        public double CurrentValue { get; }
        public double NowValue { get; }
        public double PeakLightValue { get; }
        public int Reward { get; }
        public TurnDirection Turn { get; }
    }

    // Stateful migration of the core autonomous light-seeking flow from legacy HadenManualControl:
    // Iteration bootstrap (PeekValue), Compare-based turn choice, peak/reward tracking, and last-turn memory.
    public sealed class LegacyAutonomousLightSeekEngine
    {
        private bool _initialDisturbancePending = true;

        public LegacyAutonomousLightSeekEngine()
        {
            Then = new List<double>();
            LastTurn = TurnDirection.None;
        }

        public int Iteration { get; private set; }
        public double Now { get; private set; }
        public double CurrentValue { get; private set; }
        public double PeakLightValue { get; private set; }
        public int Reward { get; private set; }
        public TurnDirection LastTurn { get; private set; }
        public List<double> Then { get; }

        public LegacyAutonomousLightSeekStep Step(int sensorValue)
        {
            Now = sensorValue;
            TurnDirection turn = TurnDirection.None;

            if (Iteration == 0)
            {
                CurrentValue = Now;
                Then.Add(Now);

                if (_initialDisturbancePending)
                {
                    CurrentValue = 10;
                    turn = CompareAndChooseTurn();
                    _initialDisturbancePending = false;
                }

                Iteration++;
                return Snapshot(sensorValue, turn);
            }

            if (Iteration == 1)
            {
                if (Now != CurrentValue)
                {
                    turn = DanceRoutineTurn();
                    Iteration++;
                }

                return Snapshot(sensorValue, turn);
            }

            turn = CompareAndChooseTurn();
            Then.Add(Now);
            Iteration++;
            return Snapshot(sensorValue, turn);
        }

        private TurnDirection DanceRoutineTurn()
        {
            if (Now > CurrentValue && Now > PeakLightValue)
            {
                PeakLightValue = Now;
                Reward++;
            }

            return CompareAndChooseTurn();
        }

        private TurnDirection CompareAndChooseTurn()
        {
            if (CurrentValue == 0 && Now == 0)
            {
                return TurnDirection.None;
            }

            TurnDirection nextTurn = TurnDirection.None;
            if (CurrentValue > Now)
            {
                nextTurn = TurnDirection.Left;
            }

            if (CurrentValue < Now)
            {
                nextTurn = TurnDirection.Right;
            }

            if (Now > PeakLightValue)
            {
                PeakLightValue = Now;
            }

            if (Now == PeakLightValue)
            {
                Reward++;
            }

            CurrentValue = Now;

            if (nextTurn != TurnDirection.None)
            {
                LastTurn = nextTurn;
            }

            return nextTurn;
        }

        private LegacyAutonomousLightSeekStep Snapshot(int sensorValue, TurnDirection turn)
        {
            return new LegacyAutonomousLightSeekStep(
                sensorValue,
                Iteration,
                CurrentValue,
                Now,
                PeakLightValue,
                Reward,
                turn);
        }
    }
}
