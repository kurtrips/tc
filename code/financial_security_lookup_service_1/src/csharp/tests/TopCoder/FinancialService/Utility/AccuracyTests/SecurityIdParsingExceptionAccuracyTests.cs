/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SecurityIdParsingExceptionAccuracyTests.cs
 */
using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SecurityIdParsingException</c>' Accuracy Tests.
    /// This accuracy tests addresses the functionality provided
    /// by the <c>SecurityIdParsingException</c> class.
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
    public class SecurityIdParsingExceptionAccuracyTests
    {
        /// <summary>
        /// Accuracy Test of the <c>SecurityIdParsingException</c>'s inheritance.
        /// </summary>
        [Test]
        public void InheritanceTest()
        {
            // check the inheritance here.
            Assert.IsTrue(typeof(FinancialSecurityException).IsAssignableFrom(typeof(
                SecurityIdParsingException)),
                "The SecurityIdParsingException should extend from FinancialSecurityException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityIdParsingException()</c> constructor.
        /// The exception instance should be created.
        /// </summary>
        [Test]
        public void Constructor_Default_Test()
        {
            // test with SecurityIdParsingException.
            Assert.IsNotNull(new SecurityIdParsingException(),
                "Failed to create the instance of SecurityIdParsingException.");
        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityIdParsingException(string)</c> constructor.
        /// The exception instance should be created and should have correct type and message.
        /// </summary>
        [Test]
        public void Constructor_String_Test()
        {
            string errorMessage = "Exception Message";

            // test with SecurityIdParsingException.
            Exception exception = new SecurityIdParsingException(errorMessage);
            Assert.IsNotNull(exception,
                "Failed to create the instance of SecurityIdParsingException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");

        }

        /// <summary>
        /// Accuracy Test of the <c>SecurityIdParsingException(string, Exception)</c> constructor.
        /// The exception instance should be created and should have correct message and inner exception.
        /// </summary>
        [Test]
        public void Constructor_StringAndException_Test()
        {
            string errorMessage = "Exception Message";

            Exception innerException = new Exception("Exception Message");

            // test with SecurityIdParsingException.
            Exception exception = new SecurityIdParsingException(errorMessage, innerException);
            Assert.IsNotNull(exception,
                "Failed to create the instance of SecurityIdParsingException.");

            // check the error message here.
            Assert.AreEqual(errorMessage, exception.Message,
                "The error message should be: " + errorMessage + ".");
            // check the inner exception.
            Assert.AreEqual(innerException, exception.InnerException,
                "The Inner Exception should be equal.");
        }
    }
}