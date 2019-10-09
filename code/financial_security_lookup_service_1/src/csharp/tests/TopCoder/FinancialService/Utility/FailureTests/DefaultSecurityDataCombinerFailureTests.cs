/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>DefaultSecurityDataCombiner</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DefaultSecurityDataCombinerFailureTests
    {
        /// <summary>
        /// The first security data.
        /// </summary>
        SecurityData firstSecurityData = new SecurityData("id1", "company1", new string[] {"A", "B" } );

        /// <summary>
        /// The second security data.
        /// </summary>
        SecurityData secondSecurityData = new SecurityData("id2", "company2", new string[] { "C", "D" });

        /// <summary>
        /// Private variable that represents the <c>DefaultSecurityDataCombiner</c> for the tests.
        /// </summary>
        private ISecurityDataCombiner instance;

        /// <summary>
        /// <para>Sets up test environment.</para>
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            instance = new DefaultSecurityDataCombiner();
        }

        /// <summary>
        /// Tests the failure of the <c>Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// </c> method with null firstSecurityData.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCombine_Null_firstSecurityData()
        {
            instance.Combine(null, secondSecurityData);
        }

        /// <summary>
        /// Tests the failure of the <c>Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        /// </c> method with null secondSecurityData.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestCombine_Null_secondSecurityData()
        {
            instance.Combine(firstSecurityData, null);
        }
    }
}