// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the VerificationException class.
    /// </summary>
    [TestFixture]
    public class VerificationExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new VerificationException() is ApplicationException,
                "VerificationException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of VerificationException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyVerificationExceptionConstructor1()
        {
            VerificationException excp = new VerificationException();
        }

        /// <summary>
        /// <p>Tests constructor of VerificationException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyVerificationExceptionConstructor2()
        {
            VerificationException excp = new VerificationException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of VerificationException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyVerificationExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            VerificationException excp =
                new VerificationException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
