namespace Haden.Library.DecisionTree
{
public partial class TreeNode
    {
        public class TreeDisplaySettings
        {
            /// <summary>
            /// Gets or sets a value indicating whether [include default tree styling].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [include default tree styling]; otherwise, <c>false</c>.
            /// </value>
            public bool IncludeDefaultTreeStyling { get; set; } = true;
            /// <summary>
            /// Gets or sets the value display property.
            /// </summary>
            /// <value>
            /// The value display property.
            /// </value>
            public string ValueDisplayProperty { get; set; } = "ToString";
            /// <summary>
            /// Gets or sets the label display property.
            /// </summary>
            /// <value>
            /// The label display property.
            /// </value>
            public string LabelDisplayProperty { get; set; } = "ToString";
        }
    }
}
