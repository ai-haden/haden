using System;
using System.Collections.Generic;

namespace Haden.Library.Algorithm
{
    public sealed class IdealReinforcementModel
    {
        private readonly Dictionary<string, Dictionary<string, double>> _qValues = new Dictionary<string, Dictionary<string, double>>();
        private readonly Random _random;

        public IdealReinforcementModel(double learningRate, double discountFactor, int? seed = null)
        {
            LearningRate = learningRate;
            DiscountFactor = discountFactor;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public double LearningRate { get; }
        public double DiscountFactor { get; }

        public double GetQValue(string state, string action)
        {
            EnsureStateAction(state, action);
            return _qValues[state][action];
        }

        public string SelectAction(string state, string[] actions, double epsilon)
        {
            if (actions == null || actions.Length == 0)
            {
                throw new ArgumentException("At least one action is required.", nameof(actions));
            }

            foreach (string action in actions)
            {
                EnsureStateAction(state, action);
            }

            if (epsilon > 0.0 && _random.NextDouble() < epsilon)
            {
                return actions[_random.Next(actions.Length)];
            }

            return GetGreedyAction(state, actions);
        }

        public double Update(string state, string action, double reward, string nextState, string[] nextActions)
        {
            EnsureStateAction(state, action);

            double maxNext = 0.0;
            if (nextActions != null && nextActions.Length > 0)
            {
                maxNext = double.MinValue;
                foreach (string nextAction in nextActions)
                {
                    EnsureStateAction(nextState, nextAction);
                    double candidate = _qValues[nextState][nextAction];
                    if (candidate > maxNext)
                    {
                        maxNext = candidate;
                    }
                }
            }

            double current = _qValues[state][action];
            double tdError = reward + (DiscountFactor * maxNext) - current;
            double updated = current + (LearningRate * tdError);
            _qValues[state][action] = updated;
            return tdError;
        }

        public string GetGreedyAction(string state, string[] actions)
        {
            if (actions == null || actions.Length == 0)
            {
                throw new ArgumentException("At least one action is required.", nameof(actions));
            }

            string bestAction = actions[0];
            double bestValue = GetQValue(state, bestAction);

            for (int i = 1; i < actions.Length; i++)
            {
                string action = actions[i];
                double candidate = GetQValue(state, action);
                if (candidate > bestValue)
                {
                    bestAction = action;
                    bestValue = candidate;
                }
            }

            return bestAction;
        }

        private void EnsureStateAction(string state, string action)
        {
            if (!_qValues.ContainsKey(state))
            {
                _qValues[state] = new Dictionary<string, double>();
            }

            if (!_qValues[state].ContainsKey(action))
            {
                _qValues[state][action] = 0.0;
            }
        }
    }
}
