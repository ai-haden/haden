using System;

namespace Haden.Library
{
    public class PolicyChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public State State { get; set; }
        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        public Query Query { get; set; }
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public FeatureValuePair Label { get; set; }
    }
}
