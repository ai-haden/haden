using System.Collections.Generic;

namespace Haden.Library.Algorithm
{
    public class StateActionSpace
    {
        /// <summary>
        /// Gets the listof actions.
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetListofActions()
        { 
            return new List<string>(); 
        }
        /// <summary>
        /// Gets the listof states.
        /// </summary>
        /// <returns></returns>
        public virtual List<int> GetListofStates()
        {
            return new List<int>();
        }
        /// <summary>
        /// Gets the eligible actions.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public virtual List<string> GetEligibleActions(int state)
        {
            return new List<string>();
        }
    }
}
