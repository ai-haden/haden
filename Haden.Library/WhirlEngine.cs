using System;

namespace Haden.Library
{
    public enum WhirlState
    {
        Zero,
        One,
        Two,
        Three,
        Four
    }

    public sealed class WhirlTickResult
    {
        public WhirlTickResult(WhirlState state, string action, bool shouldSeekLight)
        {
            State = state;
            Action = action;
            ShouldSeekLight = shouldSeekLight;
        }

        public WhirlState State { get; }
        public string Action { get; }
        public bool ShouldSeekLight { get; }
    }

    public sealed class WhirlEngine
    {
        private int _numberOfWhirls;

        public bool ConnectedHaden { get; set; }
        public WhirlState CurrentState { get; private set; }

        public WhirlEngine()
        {
            Reset();
        }

        public void Reset()
        {
            CurrentState = WhirlState.Zero;
        }

        public WhirlTickResult GetCurrentTick()
        {
            string action = GetAction(CurrentState);
            return new WhirlTickResult(CurrentState, action, IsSeekAction(action));
        }

        public WhirlTickResult AdvanceTick()
        {
            CurrentState = GetNextState(CurrentState);
            if (CurrentState == WhirlState.Four)
            {
                _numberOfWhirls++;
            }

            string action = GetAction(CurrentState);
            return new WhirlTickResult(CurrentState, action, IsSeekAction(action));
        }

        private string GetAction(WhirlState state)
        {
            switch (state)
            {
                case WhirlState.Zero:
                    if (_numberOfWhirls == 0 && !ConnectedHaden)
                    {
                        return "Beginning my whirl. Not connected to haden.";
                    }
                    return "Resting in my zeroth state. Connected.";
                case WhirlState.One:
                    return "Discover the peak value detected at a light sensor.";
                case WhirlState.Two:
                    if (!ConnectedHaden)
                    {
                        return "Not connected to haden.";
                    }
                    return "I am now in state two on the whirl. Connected.";
                case WhirlState.Three:
                    return "Is a peak value detected at a light sensor?";
                case WhirlState.Four:
                    if (!ConnectedHaden)
                    {
                        return "Not connected to haden.";
                    }
                    return "I am now in state four on the whirl. Connected.";
                default:
                    return "Resting in my zeroth state. Connected.";
            }
        }

        private static WhirlState GetNextState(WhirlState state)
        {
            switch (state)
            {
                case WhirlState.Zero:
                    return WhirlState.One;
                case WhirlState.One:
                    return WhirlState.Two;
                case WhirlState.Two:
                    return WhirlState.Three;
                case WhirlState.Three:
                    return WhirlState.Four;
                case WhirlState.Four:
                default:
                    return WhirlState.Zero;
            }
        }

        private static bool IsSeekAction(string action)
        {
            return action.Contains("Discover", StringComparison.Ordinal);
        }
    }
}
