// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the ServiceNotAvailableException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class ServiceNotAvaliableExceptionTests
    {
        /// <summary>
        /// <p>Message string for test.</p>
        /// </summary>
        private string message = "message";

        /// <summary>
        /// <p>Exception instance for test.</p>
        /// </summary>
        private Exception cause = new Exception("innerException");

        /// <summary>
        /// <p>Test ServiceNotAvailableException().</p>
        ///
        /// <p>Type should be correct.</p>
        /// </summary>
        [Test]
        public void TestCtor()
        {
            Assert.IsTrue(new ServiceNotAvailableException() is SecurityLookupException,
                "Wrong type of ServiceNotAvailableException");
            Assert.IsTrue(new ServiceNotAvailableException() is FinancialSecurityException,
                "Wrong type of ServiceNotAvailableException");
            Assert.IsTrue(new ServiceNotAvailableException() is SelfDocumentingException,
                "Wrong type of ServiceNotAvailableException");
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(string), by passing a null reference.</p>
        ///
        /// <p>Should work with message as null.</p>
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            new ServiceNotAvailableException(null);
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(string),
        /// by passing an error message.</p>
        ///
        /// <p>Message should be correct.</p>
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception e = new ServiceNotAvailableException(message);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(string, Exception),
        /// by passing null references.</p>
        ///
        /// <p>Should work with message as null and innerException as null</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            new ServiceNotAvailableException(null, null);
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(string, Exception),
        /// by passing an error message and a null reference.</p>
        ///
        /// <p>Should work with innerException as null</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception e = new ServiceNotAvailableException(message, null);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(string, Exception),
        /// by passing a null reference and an inner exception.</p>
        /// <p>Should work with message as null and innerException as non-null</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception e = new ServiceNotAvailableException(null, cause);
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(string, Exception),
        /// by passing an error message and an inner exception.</p>
        ///
        /// <p>Should have correct message and correct innerException</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception e = new ServiceNotAvailableException(message, cause);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <p>Test ServiceNotAvailableException(SerializationInfo, StreamingContext).</p>
        ///
        /// <p>Desterilized instance should have same property as it before serialization.</p>
        /// </summary>
        [Test]
        public void TestCtorInfoContext()
        {
            // Stream for serialization.
            using (Stream stream = new MemoryStream())
            {
                // Serialize the instance.
                ServiceNotAvailableException serial =
                    new ServiceNotAvailableException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                ServiceNotAvailableException deserial =
                    formatter.Deserialize(stream) as ServiceNotAvailableException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial, "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message, "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message, deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }

    }
}
