/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * FinancialSecurityExceptionAccuracyTests.cs
 */
using System;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>FinancialSecurityException</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>FinancialSecurityException</c> class.
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
    public class FinancialSecurityExceptionAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>FinancialSecurityException</c>'s inheritance.
        /// </summary>
        [Test]
        public void InheritanceTest()
        {
            // check the inheritance here.
            Assert.IsTrue(typeof(SelfDocumentingException).IsAssignableFrom(typeof(FinancialSecurityException)),
                "The FinancialSecurityException should extend from SelfDocumentingException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>FinancialSecurityException()</c> constructor.
        /// The exception instance should be created.
        /// </summary>
        [Test]
        public void Constructor_Default_Test()
        {
            // test with FinancialSecurityException.
            Assert.IsNotNull(new FinancialSecurityException(),
                "Failed to create the instance of FinancialSecurityException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>FinancialSecurityException(string)</c> constructor.
        /// The exception instance should be created and should have correct type and message.
        /// </summary>
        [Test]
        public void Constructor_String_Test()
        {
            string errorMessage = "Exception Message";

            // test with FinancialSecurityException.
            Exception exception = new FinancialSecurityException(errorMessage);
            Assert.IsNotNull(exception,
                "Failed to create the instance of FinancialSecurityException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");

        }

        /// <summary>
        /// Accuracy Test of the <c>FinancialSecurityException(string, Exception)</c> constructor.
        /// The exception instance should be created and should have correct message and inner exception.
        /// </summary>
        [Test]
        public void Constructor_StringAndException_Test()
        {
            string errorMessage = "Exception Message";

            Exception innerException = new Exception("Exception Message");

            // test with FinancialSecurityException.
            Exception exception = new FinancialSecurityException(errorMessage, innerException);
            Assert.IsNotNull(exception,
                "Failed to create the instance of FinancialSecurityException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");
            // check the inner exception.
            Assert.AreEqual(innerException, exception.InnerException,
                "The Inner Exception should be equal.");
        }
    }
}