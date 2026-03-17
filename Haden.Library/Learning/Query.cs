using System;

namespace Haden.Library
{
    /// <summary>
    /// A combination of feature and related label. A list of queries is usually used for tracking possible transitions from state to state.
    /// </summary>
    public class Query
    {
        /// <summary>
        /// The feature that will be queried during training.
        /// </summary>
        public FeatureValuePair Feature { get; set; }
        /// <summary>
        /// The label provides context for comparing this query, for getting the expected reward. 
        /// </summary>
        public FeatureValuePair Label { get; set; }                  
        public Query(FeatureValuePair datavectorFeature, FeatureValuePair label)
        {
            //Check for nulls
            if (datavectorFeature == null || label == null)
                throw new ArgumentException("Parameters cannot be null.");
            if (datavectorFeature.Name == null || datavectorFeature.Value == null)
                throw new ArgumentException("DatavectorFeature's Name and Value parameters cannot be null.");
            if (label.Name == null || label.Value == null)
                throw new ArgumentException("Labels's Name and Value parameters cannot be null.");

            //Save parameters
            this.Feature = new FeatureValuePair(datavectorFeature.Name, datavectorFeature.Value); //To prevent additional details being stored by a derived object.
            this.Label = new FeatureValuePair(label.Name, label.Value);
        }
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = "";
            s += "" + this.Feature.ToString().PadRight(20, ' ');
            s += ", " + this.Label.Value.ToString().PadRight(8, ' ');
            //s += ", " + this.GetHashCode();
            return s;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Tuple.Create(Feature, Label.Value).GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            Query that = obj as Query;
            if (that == null)
            {
                return false;
            }

            return Equals(this.Feature, that.Feature)
                && Equals(this.Label, that.Label);
        }
    }
}
