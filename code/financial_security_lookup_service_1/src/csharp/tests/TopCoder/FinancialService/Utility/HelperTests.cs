// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
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
        /// Tests the ValidateDictionary method when dic is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateDictionary1()
        {
            Helper.ValidateDictionary(null, "a");
        }

        /// <summary>
        /// Tests the ValidateDictionary method when dic is empty.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateDictionary2()
        {
            Helper.ValidateDictionary(new Dictionary<string, ISecurityLookupService>(), "a");
        }

        /// <summary>
        /// Tests the ValidateDictionary method when dic has empty key.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateDictionary3()
        {
            Dictionary<string, ISecurityLookupService> dic = new Dictionary<string, ISecurityLookupService>();
            dic.Add("     ", new CustomSecurityLookupService());

            Helper.ValidateDictionary(dic, "a");
        }

        /// <summary>
        /// Tests the ValidateDictionary method when dic has null value.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateDictionary4()
        {
            Dictionary<string, ISecurityLookupService> dic = new Dictionary<string, ISecurityLookupService>();
            dic.Add(" s    ", null);

            Helper.ValidateDictionary(dic, "a");
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is null and checkNull is true.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateArray1()
        {
            Helper.ValidateArray(null, "a", true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is null and checkNull is false.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateArray2()
        {
            Helper.ValidateArray(null, "a", false, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is empty and checkEmpty is false.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateArray3()
        {
            Helper.ValidateArray(new string[0], "a", true, false);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr is empty and checkEmpty is true.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateArray4()
        {
            Helper.ValidateArray(new string[0], "a", true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr has null element.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateArray5()
        {
            Helper.ValidateArray(new string[] { null }, "a", true, true);
        }

        /// <summary>
        /// Tests the ValidateArray method when arr has empty string element.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateArray6()
        {
            Helper.ValidateArray(new string[] { "     " }, "a", true, true);
        }

        /// <summary>
        /// Tests the GetSelfDocumentingException method.
        /// </summary>
        [Test]
        public void TestGetSelfDocumentingException()
        {
            Exception e = new Exception("Message");

            SelfDocumentingException sde = Helper.GetSelfDocumentingException(
                e, "My Mesasge", "Cold.Turkey", new string[0], new object[0],
                new string[0], new object[0], new string[0], new object[0]);

            Assert.AreEqual(sde.InnerException, e, "Inner exception is incorrect.");
        }

        /// <summary>
        /// Tests the GetCheckDigitForIsin method.
        /// </summary>
        [Test]
        public void TestGetCheckDigitForIsin()
        {
            char c = Helper.GetCheckDigitForIsin("US383883105");
            Assert.AreEqual(c, '1', "Check digit is incorrect.");
        }
    }
}
