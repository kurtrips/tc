// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the KeyInfoException class.
    /// </summary>
    [TestFixture]
    public class KeyInfoExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new KeyInfoException() is ApplicationException,
                "KeyInfoException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of KeyInfoException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyKeyInfoExceptionConstructor1()
        {
            KeyInfoException excp = new KeyInfoException();
        }

        /// <summary>
        /// <p>Tests constructor of KeyInfoException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyKeyInfoExceptionConstructor2()
        {
            KeyInfoException excp = new KeyInfoException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of KeyInfoException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyKeyInfoExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            KeyInfoException excp =
                new KeyInfoException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
