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
    /// Unit tests for the UnknownSecurityIdTypeException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class UnknownSecurityIdTypeExceptionTests
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
        /// <p>Test UnknownSecurityIdTypeException().</p>
        ///
        /// <p>Type should be correct.</p>
        /// </summary>
        [Test]
        public void TestCtor()
        {
            Assert.IsTrue(new UnknownSecurityIdTypeException() is SecurityIdParsingException,
                "Wrong type of UnknownSecurityIdTypeException");
            Assert.IsTrue(new UnknownSecurityIdTypeException() is FinancialSecurityException,
                "Wrong type of UnknownSecurityIdTypeException");
            Assert.IsTrue(new UnknownSecurityIdTypeException() is SelfDocumentingException,
                "Wrong type of UnknownSecurityIdTypeException");
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(string), by passing a null reference.</p>
        ///
        /// <p>Should work with message as null.</p>
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            new UnknownSecurityIdTypeException(null);
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(string),
        /// by passing an error message.</p>
        ///
        /// <p>Message should be correct.</p>
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception e = new UnknownSecurityIdTypeException(message);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(string, Exception),
        /// by passing null references.</p>
        ///
        /// <p>Should work with message as null and innerException as null</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            new UnknownSecurityIdTypeException(null, null);
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(string, Exception),
        /// by passing an error message and a null reference.</p>
        ///
        /// <p>Should work with innerException as null</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception e = new UnknownSecurityIdTypeException(message, null);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(string, Exception),
        /// by passing a null reference and an inner exception.</p>
        /// <p>Should work with message as null and innerException as non-null</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception e = new UnknownSecurityIdTypeException(null, cause);
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(string, Exception),
        /// by passing an error message and an inner exception.</p>
        ///
        /// <p>Should have correct message and correct innerException</p>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception e = new UnknownSecurityIdTypeException(message, cause);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <p>Test UnknownSecurityIdTypeException(SerializationInfo, StreamingContext).</p>
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
                UnknownSecurityIdTypeException serial =
                    new UnknownSecurityIdTypeException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                UnknownSecurityIdTypeException deserial =
                    formatter.Deserialize(stream) as UnknownSecurityIdTypeException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial, "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message, "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message, deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }

    }
}
