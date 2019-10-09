// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER
using System;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Helper class for the Unit tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class UnitTestHelper
    {
        /// <summary>
        /// Compares 2 string arrays whether they are equal. The 2 arrays are considered
        /// equal if their lengths are equal and each element is same.
        /// </summary>
        /// <param name="x">The first array</param>
        /// <param name="y">The second array</param>
        /// <returns>true if the arrays are equal, false otherwise</returns>
        public static bool CompareStringArrays(string[] x, string[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }

            //Compare each element
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
