using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Haden.Library.Algorithm
{
    public class BaseLearner : LearningAlgorithm
    {
        public StateActionSpace ActionSpace { get; set; }
        public List<int> States { get; set; }
        public List<string> Actions { get; set; }
        public double Gamma { get; set; } 
        public double LearningRate { get; set; }
        public Dictionary<int, int> Values { get; set; }
        public Dictionary<int, string> Policy { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLearner"/> class.
        /// </summary>
        /// <param name="stateActionSpace">The state action space.</param>
        public BaseLearner(StateActionSpace stateActionSpace)
        {
            Debug.Assert(stateActionSpace is StateActionSpace);// Check type.
            ActionSpace = stateActionSpace;
            States = ActionSpace.GetListofStates();
            Actions = ActionSpace.GetListofActions();
            Gamma = 0.5;
            LearningRate = 0.1;
            // Random initialization.
            Values = new Dictionary<int, int>();
            Policy = new Dictionary<int, string>();

            foreach (var _tup_1 in this.States.Select((_p_1, _p_2) => Tuple.Create(_p_2, _p_1)))
            {
                var i = _tup_1.Item1;
                var state = _tup_1.Item2;
                Values[state] = 0;
                var eligibleActions = ActionSpace.GetEligibleActions(state);
                Policy[state] = eligibleActions[i % eligibleActions.Count];
            }
        }
        /// <summary>
        /// Gets the next action.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        /// <returns></returns>
        public override int GetNextAction(int currentState)
        {
            if (!Policy.ContainsKey(currentState))
            {
                throw new KeyNotFoundException("No policy entry exists for state " + currentState + ".");
            }

            string actionLabel = Policy[currentState];
            int actionIndex = Actions.IndexOf(actionLabel);
            if (actionIndex < 0)
            {
                throw new InvalidOperationException("Policy action '" + actionLabel + "' is not in the action space.");
            }

            return actionIndex;
        }
        /// <summary>
        /// Receives the reward.
        /// </summary>
        /// <param name="oldState">The old state.</param>
        /// <param name="action">The action.</param>
        /// <param name="nextState">State of the next.</param>
        /// <param name="reward">The reward.</param>
        /// <remarks>Do TD return.</remarks>
        public override void ReceiveReward(int oldState, string action, int nextState, double reward)
        {
            var td_error = reward + Gamma * Values[nextState] - Values[oldState];
            Values[oldState] += Convert.ToInt32(LearningRate * td_error);
        }
        /// <summary>
        /// Finalizes the episode.
        /// </summary>
        /// <param name="currentStates">The current states.</param>
        /// <param name="nextActions">The next actions.</param>
        /// <remarks>Update policy greedily, in the basic example assuming equal transition probabilities.</remarks>
        public override void FinalizeEpisode(int[] currentStates, int[] nextActions)
        {
            if (currentStates == null || currentStates.Length < 2)
            {
                throw new ArgumentException("currentStates must contain at least two entries.", nameof(currentStates));
            }
            if (nextActions == null || nextActions.Length < 2)
            {
                throw new ArgumentException("nextActions must contain at least two entries.", nameof(nextActions));
            }

            var currentState = currentStates[0] + nextActions[0];
            var nextAction = currentStates[1] + nextActions[1];
            var nextStateDeterministic = currentState + nextAction;
            foreach (var state in Policy.Keys)
            {
                if (!Values.ContainsKey(nextStateDeterministic))
                {
                    throw new KeyNotFoundException("Expected deterministic next state '" + nextStateDeterministic + "' in Values.");
                }

                var current_next_value = Values[nextStateDeterministic];
                foreach (var action in ActionSpace.GetEligibleActions(state))
                {
                    var value_of_next = Values[nextStateDeterministic];
                    if (value_of_next >= current_next_value)
                    {
                        Policy[state] = action;
                        current_next_value = value_of_next;
                    }
                }
            }
        }
    }
}
