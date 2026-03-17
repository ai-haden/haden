using System;
using System.Reflection;

namespace Haden.Library
{
    /// <summary>
    /// A combination of feature name and its respective value.
    /// </summary>
    public class FeatureValuePair
    {
        // Properties
        /// <summary>
        /// The name of a single feature, usually from a DataVector.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The value associated with this feature name, usually from a Datavactor.
        /// </summary>
        public object Value { get; private set; }
        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; internal set; }
        /// <summary>
        /// Creates a paired object of feature name and its value.
        /// </summary>
        /// <param name="name">The name of the feature.</param>
        /// <param name="value">The value of the feature.</param>
        public FeatureValuePair(string name, object value)
        {
            //Check for nulls
            if (name == null || value == null)
                throw new ArgumentException("Parameters cannot be null.");

            //Save parameters
            this.Name = name;
            this.Value = value;

            //Subscribe to RemoveSelf event, if it exists.
            if (value.GetType().GetInterface("IRemoveSelf") != null)
            {
                //Retrieve event from value
                EventInfo eventInfo = value.GetType().GetEvent("OnRemoveSelf");

                //Convert to delegate
                Type tDelegate = eventInfo.EventHandlerType;
                MethodInfo miHandler = typeof(FeatureValuePair).GetMethod("Value_OnRemoveSelf",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                Delegate d = Delegate.CreateDelegate(tDelegate, this, miHandler);

                //Subscribe to the event
                eventInfo.GetAddMethod().Invoke(value, new object[] { d });
            }
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
            s += " Name=" + this.Name.ToString().PadRight(20, ' ');
            s += " Value=" + this.Value.ToString().PadRight(8, ' ');
            s += ", " + this.GetHashCode();
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
            return Tuple.Create(Name, Value).GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            FeatureValuePair that = obj as FeatureValuePair;
            if (that == null)
            {
                return false;
            }

            return string.Equals(this.Name, that.Name, StringComparison.Ordinal)
                && Equals(this.Value, that.Value);
        }
    }
}
