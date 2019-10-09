// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the SigningException class.
    /// </summary>
    [TestFixture]
    public class SigningExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new SigningException() is ApplicationException,
                "SigningException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of SigningException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracySigningExceptionConstructor1()
        {
            SigningException excp = new SigningException();
        }

        /// <summary>
        /// <p>Tests constructor of SigningException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracySigningExceptionConstructor2()
        {
            SigningException excp = new SigningException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of SigningException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracySigningExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            SigningException excp =
                new SigningException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
