/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>SecurityData</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SecurityDataFailureTests
    {
        /// <summary>
        /// An array of reference ids.
        /// </summary>
        string[] referenceIds = new string[] { "value1", "value2" };

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName)</c> constructor with null id.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityData_1_Null_id()
        {
            new SecurityData(null, "companyName");
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName)</c> constructor with empty id.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityData_1_empty_id()
        {
            new SecurityData("          ", "companyName");
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName)</c> constructor with null
        /// companyName. An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityData_1_Null_companyName()
        {
            new SecurityData("id", null);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName)</c> constructor with empty
        /// companyName. An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityData_1_empty_companyName()
        {
            new SecurityData("id", "     ");
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with null id.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityData_2_Null_id()
        {
            new SecurityData(null, "companyName", referenceIds);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with empty id.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityData_2_empty_id()
        {
            new SecurityData("          ", "companyName", referenceIds);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with null companyName.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityData_2_Null_companyName()
        {
            new SecurityData("id", null, referenceIds);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with empty companyName.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityData_2_empty_companyName()
        {
            new SecurityData("id", "      ", referenceIds);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with null referenceIds.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityData_2_Null_referenceIds()
        {
            new SecurityData("id", "companyName", null);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with empty item referenceIds.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityData_2_empty_item_referenceIds()
        {
            referenceIds[1] = "     ";
            new SecurityData("id", "companyName", referenceIds);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityData(string id, string companyName, string[] referenceIds)
        /// </c> constructor with null item referenceIds.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityData_2_null_item_referenceIds()
        {
            referenceIds[1] = null;
            new SecurityData("id", "companyName", referenceIds);
        }
    }
}