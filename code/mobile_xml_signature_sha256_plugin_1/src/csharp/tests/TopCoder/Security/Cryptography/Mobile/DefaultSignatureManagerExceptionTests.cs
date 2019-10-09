// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author kurtrips

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the SignatureManagerException class.
    /// </summary>
    [TestFixture]
    public class SignatureManagerExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new SignatureManagerException() is ApplicationException,
                "SignatureManagerException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of SignatureManagerException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracySignatureManagerExceptionConstructor1()
        {
            SignatureManagerException excp = new SignatureManagerException();
        }

        /// <summary>
        /// <p>Tests constructor of SignatureManagerException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracySignatureManagerExceptionConstructor2()
        {
            SignatureManagerException excp = new SignatureManagerException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of SignatureManagerException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracySignatureManagerExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            SignatureManagerException excp =
                new SignatureManagerException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
