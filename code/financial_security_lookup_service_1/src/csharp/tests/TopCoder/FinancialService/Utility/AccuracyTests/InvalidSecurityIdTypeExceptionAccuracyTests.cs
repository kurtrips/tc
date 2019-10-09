/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * InvalidSecurityIdTypeExceptionAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>InvalidSecurityIdTypeException</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>InvalidSecurityIdTypeException</c> class.
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
    public class InvalidSecurityIdTypeExceptionAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>InvalidSecurityIdTypeException</c>'s inheritance.
        /// </summary>
        [Test]
        public void InheritanceTest()
        {
            // check the inheritance here.
            Assert.IsTrue(typeof(FinancialSecurityException).IsAssignableFrom(typeof(
                InvalidSecurityIdTypeException)),
                "The InvalidSecurityIdTypeException should extend from FinancialSecurityException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>InvalidSecurityIdTypeException()</c> constructor.
        /// The exception instance should be created.
        /// </summary>
        [Test]
        public void Constructor_Default_Test()
        {
            // test with InvalidSecurityIdTypeException.
            Assert.IsNotNull(new InvalidSecurityIdTypeException(),
                "Failed to create the instance of InvalidSecurityIdTypeException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>InvalidSecurityIdTypeException(string)</c> constructor.
        /// The exception instance should be created and should have correct type and message.
        /// </summary>
        [Test]
        public void Constructor_String_Test()
        {
            string errorMessage = "Exception Message";

            // test with InvalidSecurityIdTypeException.
            Exception exception = new InvalidSecurityIdTypeException(errorMessage);
            Assert.IsNotNull(exception,
                "Failed to create the instance of InvalidSecurityIdTypeException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");

        }

        /// <summary>
        /// Accuracy Test of the <c>InvalidSecurityIdTypeException(string, Exception)</c> constructor.
        /// The exception instance should be created and should have correct message and inner exception.
        /// </summary>
        [Test]
        public void Constructor_StringAndException_Test()
        {
            string errorMessage = "Exception Message";

            Exception innerException = new Exception("Exception Message");

            // test with InvalidSecurityIdTypeException.
            Exception exception = new InvalidSecurityIdTypeException(errorMessage, innerException);
            Assert.IsNotNull(exception,
                "Failed to create the instance of InvalidSecurityIdTypeException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");
            // check the inner exception.
            Assert.AreEqual(innerException, exception.InnerException,
                "The Inner Exception should be equal.");
        }
    }
}