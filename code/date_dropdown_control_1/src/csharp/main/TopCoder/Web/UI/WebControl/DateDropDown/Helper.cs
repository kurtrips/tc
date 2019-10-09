// Helper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>This is a utility class exposing static utility functions which helps in improving
    /// code readability and decreases code redundancy.</para>
    /// </summary>
    /// <threadsafety>
    /// This class is thread safe as it has no state.
    /// </threadsafety>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class Helper
    {
        /// <summary>
        /// Validates an object for null.
        /// </summary>
        /// <param name="x">The object to check</param>
        /// <param name="name">The name of the parameter</param>
        /// <exception cref="ArgumentNullException">If the object is null</exception>
        public static void ValidateNotNull(object x, string name)
        {
            if (x == null)
            {
                throw new ArgumentNullException(name, name + " must not be null.");
            }
        }

        /// <summary>
        /// Validates a string for empty.
        /// </summary>
        /// <param name="x">The string to check</param>
        /// <param name="name">The name of the string parameter</param>
        /// <exception cref="ArgumentException">If the string is empty</exception>
        public static void ValidateNotEmpty(string x, string name)
        {
            if (x.Trim().Equals(String.Empty))
            {
                throw new ArgumentException(name + " must not be empty.", name);
            }
        }

        /// <summary>
        /// Validates a string for empty for empty and null
        /// </summary>
        /// <param name="x">The string to check</param>
        /// <param name="name">The name of the string parameter</param>
        /// <exception cref="ArgumentNullException">If the string is null</exception>
        /// <exception cref="ArgumentException">If the string is empty</exception>
        public static void ValidateNotNullNotEmpty(string x, string name)
        {
            ValidateNotNull(x, name);
            ValidateNotEmpty(x, name);
        }

        /// <summary>
        /// Check whther an integer is positive or non-negative.
        /// </summary>
        /// <param name="x">The integer to check</param>
        /// <param name="name">The name of the integer parameter</param>
        /// <param name="zeroAllowed">Whether 0 is a valid value for the integer</param>
        /// <exception cref="ArgumentException">
        /// If the integer is negative or is 0 when zeroAllowed is false.
        /// </exception>
        public static void ValidatePositive(int x, string name, bool zeroAllowed)
        {
            if (x < 0)
            {
                throw new ArgumentException(name + " must not be a negative integer.", name);
            }
            if (x == 0 && !zeroAllowed)
            {
                throw new ArgumentException(name + " must be a positive integer.", name);
            }
        }
    }
}
