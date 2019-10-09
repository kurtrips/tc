// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the DigesterException class.
    /// </summary>
    [TestFixture]
    public class DigesterExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new DigesterException() is ApplicationException,
                "DigesterException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of DigesterException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyDigesterExceptionConstructor1()
        {
            DigesterException excp = new DigesterException();
        }

        /// <summary>
        /// <p>Tests constructor of DigesterException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyDigesterExceptionConstructor2()
        {
            DigesterException excp = new DigesterException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of DigesterException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyDigesterExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            DigesterException excp =
                new DigesterException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
