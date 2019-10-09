/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.FinancialService.Utility.SecurityIdParsers;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>DefaultSecurityIdParser</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DefaultSecurityIdParserFailureTests
    {
        /// <summary>
        /// Private variable that represents the <c>DefaultSecurityIdParser</c> for the tests.
        /// </summary>
        private DefaultSecurityIdParser instance;

        /// <summary>
        /// <para>Sets up test environment.</para>
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            instance = new DefaultSecurityIdParser();
        }

        /// <summary>
        /// Tests the failure of the <c>Parse(string securityId)</c> method with null securityId.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestParse_Null_securityId()
        {
            instance.Parse(null);
        }

        /// <summary>
        /// Tests the failure of the <c>Parse(string securityId)</c> method with empty secondSecurityData.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestParse_empty_securityId()
        {
            instance.Parse("   ");
        }
    }
}