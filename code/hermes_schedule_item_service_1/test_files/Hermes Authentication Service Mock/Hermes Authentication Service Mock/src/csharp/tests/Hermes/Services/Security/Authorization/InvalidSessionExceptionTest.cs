/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Hermes.Services.Security.Authorization
{
    /// <summary>
    /// Test <see cref="InvalidSessionException"/> class, unit test.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class InvalidSessionExceptionTest
    {
        /// <summary>
        /// Message string for test.
        /// </summary>
        private string message = "message";

        /// <summary>
        /// Exception instance for test.
        /// </summary>
        private Exception cause = new Exception("innerException");

        /// <summary>
        /// Test InvalidSessionException().
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtor()
        {
            InvalidSessionException ex =
                new InvalidSessionException();
            Assert.IsNotNull(ex, "A new instance should be created.");

            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test InvalidSessionException(string),
        /// by passing a null reference.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            InvalidSessionException ex =
                new InvalidSessionException(null);
            Assert.IsNotNull(ex, "A new instance should be created.");

            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test InvalidSessionException(string),
        /// by passing an error message.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception ex = new InvalidSessionException(message);
            Assert.IsNotNull(ex, "A new instance should be created.");
            Assert.AreEqual(message, ex.Message,
                "ex.Message should be equal to message.");
            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test InvalidSessionException(string, Exception),
        /// by passing null references.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            InvalidSessionException ex =
                new InvalidSessionException(null, null);
            Assert.IsNotNull(ex, "A new instance should be created.");

            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test InvalidSessionException(string, Exception),
        /// by passing an error message and a null reference.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception ex = new InvalidSessionException(message, null);
            Assert.IsNotNull(ex, "A new instance should be created.");
            Assert.AreEqual(message, ex.Message,
                "ex.Message should be equal to message.");
            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test InvalidSessionException(string, Exception),
        /// by passing a null reference and an inner exception.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception ex = new InvalidSessionException(null, cause);
            Assert.IsNotNull(ex,
                "A new instance should be created.");
            Assert.AreEqual(cause, ex.InnerException,
                "ex.InnerException should be equal to cause.");

        }

        /// <summary>
        /// Test InvalidSessionException(string, Exception),
        /// by passing an error message and an inner exception.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception ex = new InvalidSessionException(message, cause);
            Assert.IsNotNull(ex, "A new instance should be created.");
            Assert.AreEqual(message, ex.Message,
                "ex.Message should be equal to message.");
            Assert.AreEqual(cause, ex.InnerException,
                "ex.InnerException should be equal to cause.");
        }

        /// <summary>
        /// Test
        /// InvalidSessionException(SerializationInfo, StreamingContext).
        ///
        /// Desterilized instance should have same property as it before
        /// serialization.
        /// </summary>
        [Test]
        public void TestCtorInfoContext()
        {
            // Stream for serialization.
            using (Stream stream = new MemoryStream())
            {
                // Serialize the instance.
                InvalidSessionException serial =
                    new InvalidSessionException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                InvalidSessionException deserial =
                    formatter.Deserialize(stream)
                    as InvalidSessionException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial,
                    "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message,
                    "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message,
                    deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }
    }
}
