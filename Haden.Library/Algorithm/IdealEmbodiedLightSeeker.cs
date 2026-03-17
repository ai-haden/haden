using System;
using System.Collections.Generic;

namespace Haden.Library.Algorithm
{
    public enum EmbodiedMood
    {
        SelfSatisfied,
        Frustrated,
        Bored,
        Pleased,
        Pained
    }

    public sealed class EmbodiedStepTrace
    {
        public EmbodiedStepTrace(string experiment, string anticipatedResult, string result, EmbodiedMood mood)
        {
            Experiment = experiment;
            AnticipatedResult = anticipatedResult;
            Result = result;
            Mood = mood;
        }

        public string Experiment { get; }
        public string AnticipatedResult { get; }
        public string Result { get; }
        public EmbodiedMood Mood { get; }
    }

    // Branch 01: embodied loop where cognition is grounded in experiment->result interactions.
    // Branch 02: interactional motivation through result valence.
    public sealed class IdealEmbodiedLightSeeker
    {
        private readonly Dictionary<string, string> _memory = new Dictionary<string, string>();
        private readonly Dictionary<string, double> _resultValence = new Dictionary<string, double>();
        private readonly string[] _experiments;
        private readonly int _boredomThreshold;
        private string _previousExperiment;
        private int _selfSatisfactionStreak;
        private bool _boredOnNextStep;

        public IdealEmbodiedLightSeeker(string[] experiments, string initialExperiment, int boredomThreshold = 3)
        {
            if (experiments == null || experiments.Length < 2)
            {
                throw new ArgumentException("At least two experiments are required.", nameof(experiments));
            }

            _experiments = experiments;
            _previousExperiment = initialExperiment;
            _boredomThreshold = boredomThreshold;
        }

        public void SetResultValence(string resultLabel, double valence)
        {
            _resultValence[resultLabel] = valence;
        }

        public EmbodiedStepTrace Step(Func<string, string> performExperiment)
        {
            string experiment = SelectExperiment();
            string anticipatedResult = Predict(experiment);
            string result = performExperiment(experiment);
            _memory[experiment] = result;

            bool predicted = anticipatedResult != null && string.Equals(anticipatedResult, result, StringComparison.Ordinal);
            if (predicted)
            {
                _selfSatisfactionStreak++;
            }
            else
            {
                _selfSatisfactionStreak = 0;
                _boredOnNextStep = false;
            }

            EmbodiedMood mood = ComputeMood(predicted, result);
            _previousExperiment = experiment;
            if (mood == EmbodiedMood.SelfSatisfied && _selfSatisfactionStreak >= _boredomThreshold)
            {
                _boredOnNextStep = true;
            }

            return new EmbodiedStepTrace(experiment, anticipatedResult, result, mood);
        }

        private string SelectExperiment()
        {
            if (_boredOnNextStep)
            {
                _boredOnNextStep = false;
                return GetOtherExperiment(_previousExperiment);
            }

            string motivated = GetMotivatedExperiment();
            return motivated ?? _previousExperiment;
        }

        private string GetMotivatedExperiment()
        {
            string bestExperiment = null;
            double bestValence = double.MinValue;
            foreach (string experiment in _experiments)
            {
                string predicted = Predict(experiment);
                if (predicted == null)
                {
                    continue;
                }

                double valence = GetValence(predicted);
                if (valence > bestValence)
                {
                    bestValence = valence;
                    bestExperiment = experiment;
                }
            }

            return bestExperiment;
        }

        private string Predict(string experiment)
        {
            return _memory.ContainsKey(experiment) ? _memory[experiment] : null;
        }

        private EmbodiedMood ComputeMood(bool predicted, string result)
        {
            double valence = GetValence(result);
            if (valence > 0)
            {
                return EmbodiedMood.Pleased;
            }
            if (valence < 0)
            {
                return EmbodiedMood.Pained;
            }

            if (_boredOnNextStep)
            {
                return EmbodiedMood.Bored;
            }

            return predicted ? EmbodiedMood.SelfSatisfied : EmbodiedMood.Frustrated;
        }

        private double GetValence(string result)
        {
            return _resultValence.ContainsKey(result) ? _resultValence[result] : 0.0;
        }

        private string GetOtherExperiment(string experiment)
        {
            foreach (string candidate in _experiments)
            {
                if (!string.Equals(candidate, experiment, StringComparison.Ordinal))
                {
                    return candidate;
                }
            }

            return _experiments[0];
        }
    }
}
