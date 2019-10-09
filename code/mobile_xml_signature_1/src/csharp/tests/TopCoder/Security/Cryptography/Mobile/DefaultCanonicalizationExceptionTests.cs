// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the CanonicalizationException class.
    /// </summary>
    [TestFixture]
    public class CanonicalizationExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new CanonicalizationException() is ApplicationException,
                "CanonicalizationException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of CanonicalizationException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyCanonicalizationExceptionConstructor1()
        {
            CanonicalizationException excp = new CanonicalizationException();
        }

        /// <summary>
        /// <p>Tests constructor of CanonicalizationException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyCanonicalizationExceptionConstructor2()
        {
            CanonicalizationException excp = new CanonicalizationException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of CanonicalizationException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyCanonicalizationExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            CanonicalizationException excp =
                new CanonicalizationException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
