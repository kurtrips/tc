/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SecurityLookupExceptionAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SecurityLookupException</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SecurityLookupException</c> class.
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
    public class SecurityLookupExceptionAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>SecurityLookupException</c>'s inheritance.
        /// </summary>
        [Test]
        public void InheritanceTest()
        {
            // check the inheritance here.
            Assert.IsTrue(typeof(FinancialSecurityException).IsAssignableFrom(typeof(
                SecurityIdParsingException)),
                "The SecurityLookupException should extend from FinancialSecurityException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityLookupException()</c> constructor.
        /// The exception instance should be created.
        /// </summary>
        [Test]
        public void Constructor_Default_Test()
        {
            // test with SecurityLookupException.
            Assert.IsNotNull(new SecurityLookupException(),
                "Failed to create the instance of SecurityLookupException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityLookupException(string)</c> constructor.
        /// The exception instance should be created and should have correct type and message.
        /// </summary>
        [Test]
        public void Constructor_String_Test()
        {
            string errorMessage = "Exception Message";

            // test with SecurityLookupException.
            Exception exception = new SecurityLookupException(errorMessage);
            Assert.IsNotNull(exception,
                "Failed to create the instance of SecurityLookupException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");

        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityLookupException(string, Exception)</c> constructor.
        /// The exception instance should be created and should have correct message and inner exception.
        /// </summary>
        [Test]
        public void Constructor_StringAndException_Test()
        {
            string errorMessage = "Exception Message";

            Exception innerException = new Exception("Exception Message");

            // test with SecurityLookupException.
            Exception exception = new SecurityLookupException(errorMessage, innerException);
            Assert.IsNotNull(exception,
                "Failed to create the instance of SecurityLookupException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");
            // check the inner exception.
            Assert.AreEqual(innerException, exception.InnerException,
                "The Inner Exception should be equal.");
        }
    }
}