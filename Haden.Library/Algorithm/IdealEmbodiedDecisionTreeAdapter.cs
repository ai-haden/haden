using System;
using System.Collections.Generic;
using Haden.Library.DecisionTree;

namespace Haden.Library.Algorithm
{
    /// <summary>
    /// Exports branch-01/branch-02 embodied interactions to a TreeNode representation.
    /// This is intended for explainability and offline analysis, not as the primary control loop.
    /// </summary>
    public static class IdealEmbodiedDecisionTreeAdapter
    {
        public static TreeNode Export(IdealEmbodiedLightSeeker seeker)
        {
            if (seeker == null)
            {
                throw new ArgumentNullException(nameof(seeker));
            }

            string[] experiments = seeker.GetExperiments();
            Dictionary<string, string> interactions = seeker.GetLearnedInteractions();
            Dictionary<string, double> valenceMap = seeker.GetResultValenceMap();

            TreeNode root = new TreeNode("root", TreeNodeType.Root);
            TreeNode experimentFeature = new TreeNode("experiment", TreeNodeType.Feature)
            {
                Parent = root
            };
            root.SubNodes.Add(experimentFeature);

            foreach (string experiment in experiments)
            {
                TreeNode valueNode = new TreeNode(experiment, TreeNodeType.Value)
                {
                    Parent = experimentFeature
                };

                string resultLabel;
                if (!interactions.TryGetValue(experiment, out resultLabel))
                {
                    resultLabel = "unknown";
                }

                double valence = 0.0;
                if (resultLabel != "unknown")
                {
                    valenceMap.TryGetValue(resultLabel, out valence);
                }

                valueNode.Leaves.Add(new TreeLeaf(resultLabel, ToNonNegativeWeight(valence)));
                experimentFeature.SubNodes.Add(valueNode);
            }

            return root;
        }

        private static double ToNonNegativeWeight(double valence)
        {
            // Normalize expected valence (-1..1) into probability-safe reward weights.
            double weight = (valence + 1.0) / 2.0;
            return weight <= 0.0 ? 0.001 : weight;
        }
    }
}
