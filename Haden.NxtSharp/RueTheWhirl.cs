using Haden.Library;

namespace Haden.NxtSharp
{
    [System.Obsolete("Use Haden.Library.WhirlEngine directly.")]
    public static class RueTheWhirl
    {
        private static readonly WhirlEngine Engine = new WhirlEngine();

        public static bool ConnectedHaden
        {
            get { return Engine.ConnectedHaden; }
            set { Engine.ConnectedHaden = value; }
        }

        public static string CurrentState
        {
            get { return Engine.CurrentState.ToString(); }
            set
            {
                WhirlState state;
                if (System.Enum.TryParse(value, true, out state))
                {
                    Engine.Reset();
                    while (Engine.CurrentState != state)
                    {
                        Engine.AdvanceTick();
                    }
                }
            }
        }

        public static string ActionController()
        {
            return Engine.GetCurrentTick().Action;
        }
    }
}
