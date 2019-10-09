// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author kurtrips

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the ReferenceLoadingException class.
    /// </summary>
    [TestFixture]
    public class ReferenceLoadingExceptionTests
    {
        /// <summary>
        /// Tests the definition.
        /// </summary>
        [Test]
        public void DefinitionTest()
        {
            Assert.IsTrue((object)new ReferenceLoadingException() is ApplicationException,
                "ReferenceLoadingException is not of type ApplicationException");
        }

        /// <summary>
        /// <p>Tests empty constructor of ReferenceLoadingException class</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyReferenceLoadingExceptionConstructor1()
        {
            ReferenceLoadingException excp = new ReferenceLoadingException();
        }

        /// <summary>
        /// <p>Tests constructor of ReferenceLoadingException class
        /// with string parameter</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyReferenceLoadingExceptionConstructor2()
        {
            ReferenceLoadingException excp = new ReferenceLoadingException("Error Message");
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
        }

        /// <summary>
        /// <p>Tests constructor of ReferenceLoadingException class
        /// with string parameter and inner exception parameters</p>
        /// <p>No Exception is expected.</p>
        /// </summary>
        [Test]
        public void TestAccuracyReferenceLoadingExceptionConstructor3()
        {
            Exception ex = new Exception("Inner Exception Message");
            ReferenceLoadingException excp =
                new ReferenceLoadingException("Error Message", ex);
            Assert.AreEqual(excp.Message, "Error Message", "Exception message has wrong value");
            Assert.AreEqual(excp.InnerException, ex, "Inner exception has wrong value");
            Assert.AreEqual(excp.InnerException.Message, "Inner Exception Message",
                "Inner exception message has wrong value");
        }
    }
}
