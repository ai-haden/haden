namespace Haden.Library
{
    public class TrainingStats
    {
        /// <summary>
        /// A tracking variable. No default value. It must be assigned.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The total number of states in the policy's state space.
        /// </summary>
        public int StatesTotal { get; set; }

        /// <summary>
        /// The number of different states that were visited during training with this datavector.
        /// </summary>
        public int QueriesTotal { get; set; }

        /// <summary>
        /// The number of new states created using this datavector. Typically '1' for zero or low exploration rates.
        /// </summary>
        public int StatesCreated { get; set; }
        /// <summary>
        /// The number of correct classifications, if testing sample points is enabled. This is not assigned during training. It must be set externally, usually after running the classification method on test points.
        /// </summary>
        public int CorrectClassifications { get; set; }
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = "";
            s += "Id=" + Id;
            s += "  Total=" + StatesTotal;
            s += "  Visited=" + QueriesTotal;
            s += "  Created=" + StatesCreated;
            s += "  Correct=" + CorrectClassifications;

            return s;
        }
    }
}
