// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the ProcessRegistryException class.
    /// </summary>
    [TestFixture]
    public class ProcessRegistryExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new ProcessRegistryException() is ApplicationException,
                "ProcessRegistryException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of ProcessRegistryException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyProcessRegistryExceptionConstructor1()
        {
            ProcessRegistryException excp = new ProcessRegistryException();
        }

        /// <summary>
        /// <p>Tests constructor of ProcessRegistryException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyProcessRegistryExceptionConstructor2()
        {
            ProcessRegistryException excp = new ProcessRegistryException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of ProcessRegistryException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyProcessRegistryExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            ProcessRegistryException excp =
                new ProcessRegistryException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
