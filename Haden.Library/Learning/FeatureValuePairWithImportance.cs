namespace Haden.Library
{
    /// <summary>
    /// Extends the "FeatureValuePair" class to have an importance attribute.
    /// </summary>
    public class FeatureValuePairWithImportance : FeatureValuePair
    {
        private double _Importance = 0;
        /// <summary>
        /// A relative score (-1 to 1) of the importance of including this feature. -1=Don't include, 0=neutral, 1=very important
        /// </summary>
        public double Importance
        {
            get
            {
                return _Importance;
            }
            set
            {
                if (value > 1.0)
                    _Importance = 1.0;
                else if (value < -1.0)
                    _Importance = -1.0;
                else
                    _Importance = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureValuePairWithImportance"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="importance">The importance.</param>
        public FeatureValuePairWithImportance(string name, object value, double importance) : base(name, value)
        {
            this.Importance = importance;
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
            s += base.ToString();
            s += " Importance=" + this.Importance.ToString().PadRight(5, ' ');
            s += ", " + this.GetHashCode();
            return s;
        }
    }
}
