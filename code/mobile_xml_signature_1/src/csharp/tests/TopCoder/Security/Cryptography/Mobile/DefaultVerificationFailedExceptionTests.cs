// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the VerificationFailedException class.
    /// </summary>
    [TestFixture]
    public class VerificationFailedExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new VerificationFailedException() is ApplicationException,
                "VerificationFailedException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of VerificationFailedException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyVerificationFailedExceptionConstructor1()
        {
            VerificationFailedException excp = new VerificationFailedException();
        }

        /// <summary>
        /// <p>Tests constructor of VerificationFailedException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyVerificationFailedExceptionConstructor2()
        {
            VerificationFailedException excp = new VerificationFailedException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of VerificationFailedException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyVerificationFailedExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            VerificationFailedException excp =
                new VerificationFailedException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex , "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
