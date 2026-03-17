using System.Collections.Generic;

namespace Haden.Library
{
    /// <summary>
    /// A set of extensions for haden.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines whether [is greater than] [the specified comparison value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="comparisonValue">The comparison value.</param>
        /// <returns>
        ///   <c>true</c> if [is greater than] [the specified comparison value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterThan(this double value, double comparisonValue)
        {
            if (value > comparisonValue)
                return true;
            return false;
        }
        /// <summary>
        /// Determines whether [is less than] [the specified comparison value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="comparisonValue">The comparison value.</param>
        /// <returns>
        ///   <c>true</c> if [is less than] [the specified comparison value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThan(this double value, double comparisonValue)
        {
            if (value < comparisonValue)
                return true;
            return false;
        }
        /// <summary>
        /// Determines whether [is greater than previous].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [is greater than previous] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterThanPrevious(this double[] input)
        {
            var previousValue = 0.0;
            var value = 0.0;
            for (var i = 0; i < 1; i++)
            {
                value = input[0];
                previousValue = input[1];
            }
            if (value > previousValue)
                return true;
            return false;
        }
        /// <summary>
        /// Determines whether [is less than previous].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [is less than previous] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThanPrevious(this double[] input)
        {
            var previousValue = 0.0;
            var value = 0.0;
            for (var i = 0; i < 1; i++)
            {
                value = input[0];
                previousValue = input[1];
            }
            if (value < previousValue)
                return true;
            return false;
        }
        /// <summary>
        /// Determines whether the specified value is between a minimum-maximum range (inclusive).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        public static bool IsBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
        /// <summary>
        /// Determines whether [is less than previous].
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>
        ///   <c>true</c> if [is less than previous] [the specified input]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThanPrevious(this List<double> input)
        {
            var previousValue = 0.0;
            var value = 0.0;
            for (var i = 0; i < 1; i++)
            {
                value = input[0];
                previousValue = input[1];
            }
            if (value < previousValue)
                return true;
            return false;
        }
    }
}
