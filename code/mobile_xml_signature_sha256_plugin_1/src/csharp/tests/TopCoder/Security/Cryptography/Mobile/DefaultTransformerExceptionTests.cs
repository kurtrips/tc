// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author kurtrips

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the TransformerException class.
    /// </summary>
    [TestFixture]
    public class TransformerExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new TransformerException() is ApplicationException,
                "TransformerException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of TransformerException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyTransformerExceptionConstructor1()
        {
            TransformerException excp = new TransformerException();
        }

        /// <summary>
        /// <p>Tests constructor of TransformerException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyTransformerExceptionConstructor2()
        {
            TransformerException excp = new TransformerException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of TransformerException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyTransformerExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            TransformerException excp =
                new TransformerException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
