using System;
using System.Collections.Generic;
using System.Linq;

namespace Haden.Library
{
    /// <summary>
    /// An extension of the DataVector class. It includes a classification label for this datavector. Each feature also contains its relative reward (or cost).
    /// </summary>
    public class DataVectorTraining : DataVector
    {
        /// <summary>
        /// The classification label of this datavector.
        /// </summary>
        public FeatureValuePair Label { get; set; }
        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; internal set; }
        /// <summary>
        /// Retrieves a feature within this datavector, by name.
        /// </summary>
        /// <param name="featureName"></param>
        /// <returns></returns>
        new public FeatureValuePairWithImportance this[string featureName]
        {
            get
            {
                return (FeatureValuePairWithImportance) Features.Find(f => f.Name == featureName);
            }
        }
        public DataVectorTraining()
        {
            Features = new List<FeatureValuePairWithImportance>().Cast<FeatureValuePair>().ToList();
        }
        /// <summary>
        /// Creates a Datavector, and adds additional information about feature rewards (or costs) and the classification label.
        /// </summary>
        /// <param name="headers">The names of the features.</param>
        /// <param name="dataobjects">The actual values of each feature.</param>
        /// <param name="importance">The relative importance of each feature (-1 to 1).</param>
        /// <param name="labelFeatureName">The feature to use as the label. It will be shifted out of the headers and dataobjects and stored as "Label".</param>
        public DataVectorTraining(string[] headers, object[] dataobjects, double[] importance, string labelFeatureName)
        {
            //Check number of headers matches number of data
            if ((headers.Length != dataobjects.Length) || (headers.Length != importance.Length))
                throw new FormatException("Number of headers, importance, and data per line do not match. Ensure there is a header and importance for each value.");

            //Check label feature is valid
            if (!headers.Contains(labelFeatureName))
                throw new ArgumentException("'labelFeatureName' must exist in the list of headers.");

            //Build list of features.
            Features = new List<FeatureValuePairWithImportance>().Cast<FeatureValuePair>().ToList();
            for (int i = 0; i < headers.Length; i++)
                AddFeature(headers[i], dataobjects[i], importance[i]);

            //Set label
            FeatureValuePair labelFeature = Features.Find(f => f.Name == labelFeatureName);
            SetLabel(labelFeature.Name, labelFeature.Value);

            //Remove label from the list of features
            Features.RemoveAll(p => p.Name == Label.Name);
        }
        /// <summary>
        /// Adds the feature.
        /// </summary>
        /// <param name="featureName">Name of the feature.</param>
        /// <param name="value">The value.</param>
        /// <param name="importance">The importance.</param>
        public void AddFeature(string featureName, object value, double importance)
        {
            FeatureValuePairWithImportance fvp = new FeatureValuePairWithImportance(featureName, value, importance);
            Features.Add(fvp);
        }
        /// <summary>
        /// Sets the label.
        /// </summary>
        /// <param name="featureName">Name of the feature.</param>
        /// <param name="value">The value.</param>
        public void SetLabel(string featureName, object value)
        {
            this.Label = new FeatureValuePair(featureName, value);
            this.Features.RemoveAll(p => p.Name == featureName);
        }
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = "";
            s += " Features=" + this.Features.Count;
            s += " LabelFeatureName=" + this.Label.Name;
            s += " LabelValue=" + this.Label.Value;
            return s;
        }
    }
}
