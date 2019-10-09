/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>SecurityIdDetails</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SecurityIdDetailsFailureTests
    {
        /// <summary>
        /// Tests the failure of the <c>SecurityIdDetails(string id, string type)</c> constructor with null id.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityIdDetails_Null_id()
        {
            new SecurityIdDetails(null, "type");
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityIdDetails(string id, string type)</c> constructor with empty id.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityIdDetails_empty_id()
        {
            new SecurityIdDetails("          ", "type");
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityIdDetails(string id, string type)</c> constructor with null type.
        /// An <c>ArgumentNullException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSecurityIdDetails_Null_type()
        {
            new SecurityIdDetails("id", null);
        }

        /// <summary>
        /// Tests the failure of the <c>SecurityIdDetails(string id, string type)</c> constructor with empty type.
        /// An <c>ArgumentException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSecurityIdDetails_empty_type()
        {
            new SecurityIdDetails("id", "      ");
        }
    }
}