/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SecurityDataCombiningExceptionAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SecurityDataCombiningException</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SecurityDataCombiningException</c> class.
    /// </summary>
    ///
    /// <author>
    /// icyriver
    /// </author>
    ///
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    ///
    /// <version>
    /// 1.0
    /// </version>
    [TestFixture]
    public class SecurityDataCombiningExceptionAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>SecurityDataCombiningException</c>'s inheritance.
        /// </summary>
        [Test]
        public void InheritanceTest()
        {
            // check the inheritance here.
            Assert.IsTrue(typeof(FinancialSecurityException).IsAssignableFrom(typeof(
                SecurityDataCombiningException)),
                "The SecurityDataCombiningException should extend from FinancialSecurityException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityDataCombiningException()</c> constructor.
        /// The exception instance should be created.
        /// </summary>
        [Test]
        public void Constructor_Default_Test()
        {
            // test with SecurityDataCombiningException.
            Assert.IsNotNull(new SecurityDataCombiningException(),
                "Failed to create the instance of SecurityDataCombiningException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityDataCombiningException(string)</c> constructor.
        /// The exception instance should be created and should have correct type and message.
        /// </summary>
        [Test]
        public void Constructor_String_Test()
        {
            string errorMessage = "Exception Message";

            // test with SecurityDataCombiningException.
            Exception exception = new SecurityDataCombiningException(errorMessage);
            Assert.IsNotNull(exception,
                "Failed to create the instance of SecurityDataCombiningException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");

        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityDataCombiningException(string, Exception)</c> constructor.
        /// The exception instance should be created and should have correct message and inner exception.
        /// </summary>
        [Test]
        public void Constructor_StringAndException_Test()
        {
            string errorMessage = "Exception Message";

            Exception innerException = new Exception("Exception Message");

            // test with SecurityDataCombiningException.
            Exception exception = new SecurityDataCombiningException(errorMessage, innerException);
            Assert.IsNotNull(exception,
                "Failed to create the instance of SecurityDataCombiningException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");
            // check the inner exception.
            Assert.AreEqual(innerException, exception.InnerException,
                "The Inner Exception should be equal.");
        }
    }
}