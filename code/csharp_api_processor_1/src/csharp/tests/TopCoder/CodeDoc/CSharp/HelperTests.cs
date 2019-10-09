// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp
{
    /// <summary>
    /// Unit tests for the Helper class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class HelperTests
    {
        /// <summary>
        /// Tests the ValidateNotNull method when obj is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNull1()
        {
            Helper.ValidateNotNull(null, "a");
        }

        /// <summary>
        /// Tests the ValidateNotNull method when obj is not null.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateNotNull2()
        {
            Helper.ValidateNotNull(new object(), "a");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method when str is empty.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotEmpty1()
        {
            Helper.ValidateNotEmpty("      ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method when str is not empty.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateNotEmpty2()
        {
            Helper.ValidateNotEmpty("   d    ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when str is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNullNotEmpty1()
        {
            Helper.ValidateNotNullNotEmpty(null, "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when str is empty.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotNullNotEmpty2()
        {
            Helper.ValidateNotNullNotEmpty("      ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when str is not empty and not null.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateNotNullNotEmpty3()
        {
            Helper.ValidateNotNullNotEmpty("   s   ", "a");
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is null and checkNull is true.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateArray1()
        {
            Helper.ValidateArray(null, "a", true, true, true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is null and checkNull is false.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateArray2()
        {
            Helper.ValidateArray(null, "a", false, true, true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is empty and checkEmpty is false.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateArray3()
        {
            Helper.ValidateArray(new string[0], "a", true, false, true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is empty and checkEmpty is true.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateArray4()
        {
            Helper.ValidateArray(new string[0], "a", true, true, true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr has null element and checkNullElement is true
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateArray5()
        {
            Helper.ValidateArray(new string[] { null }, "a", true, true, true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr has null element and checkNullElement is false
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateArray6()
        {
            Helper.ValidateArray(new string[] { null }, "a", true, true, false, false);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr has empty string element and checkEmptyElement is true
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateArray7()
        {
            Helper.ValidateArray(new string[] { "     " }, "a", true, true, true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr has empty string element and checkEmptyElement is false
        /// No Exception is expected
        /// </summary>
        [Test]
        public void TestValidateArray8()
        {
            Helper.ValidateArray(new string[] { "     " }, "a", true, true, true, false);
        }

        /// <summary>
        /// Tests the GetXpathPredicate method.
        /// </summary>
        [Test]
        public void TestGetXpathPredicate()
        {
            Assert.AreEqual(Helper.GetXpathPredicate("abc", "def"), "[abc='def']",
                "Wrong GetXpathPredicate implementation.");
        }

        /// <summary>
        /// Tests the GetXpathAttributePredicate method.
        /// </summary>
        [Test]
        public void TestGetXpathAttributePredicate()
        {
            Assert.AreEqual(Helper.GetXpathAttributePredicate("abc", "def"), "[@abc='def']",
                "Wrong GetXpathAttributePredicate implementation.");
        }
    }
}
