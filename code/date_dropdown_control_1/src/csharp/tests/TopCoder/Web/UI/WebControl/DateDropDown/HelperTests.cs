// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the Helper class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HelperTests
    {
        /// <summary>
        /// Tests the ValidateNotNull when object is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNull1()
        {
            Helper.ValidateNotNull(null, "a");
        }

        /// <summary>
        /// Tests the ValidateNotNull when object is not null.
        /// no exception is expected.
        /// </summary>
        [Test]
        public void TestValidateNotNull2()
        {
            Helper.ValidateNotNull(new object(), "a");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty when string is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotEmpty1()
        {
            Helper.ValidateNotEmpty("     ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty when string is empty.
        /// no exception is expected.
        /// </summary>
        [Test]
        public void TestValidateNotEmpty2()
        {
            Helper.ValidateNotEmpty("   s  ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty when string is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ValidateNotNullNotEmpty1()
        {
            Helper.ValidateNotNullNotEmpty(null, "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty when string is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateNotNullNotEmpty2()
        {
            Helper.ValidateNotNullNotEmpty("   ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty when string is not null and not empty.
        /// No exception is expected.
        /// </summary>
        [Test]
        public void ValidateNotNullNotEmpty3()
        {
            Helper.ValidateNotNullNotEmpty(" d  ", "a");
        }

        /// <summary>
        /// Tests the ValidatePositive when int is 0 and zero is allowed.
        /// No exception is expected.
        /// </summary>
        [Test]
        public void ValidatePositive1()
        {
            Helper.ValidatePositive(0, "a", true);
        }

        /// <summary>
        /// Tests the ValidatePositive when int is 0 and zero is not allowed.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ValidatePositive2()
        {
            Helper.ValidatePositive(0, "a", false);
        }

        /// <summary>
        /// Tests the ValidatePositive when int is negative.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ValidatePositive3()
        {
            Helper.ValidatePositive(-1, "a", false);
        }
    }
}
