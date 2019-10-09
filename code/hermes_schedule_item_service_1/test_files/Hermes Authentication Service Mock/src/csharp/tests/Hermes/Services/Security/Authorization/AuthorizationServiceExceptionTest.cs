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
    /// Test <see cref="AuthorizationServiceException"/> class, unit test.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class AuthorizationServiceExceptionTest
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
        /// Test AuthorizationServiceException().
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtor()
        {
            AuthorizationServiceException ex =
                new AuthorizationServiceException();
            Assert.IsNotNull(ex, "A new instance should be created.");

            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test AuthorizationServiceException(string),
        /// by passing a null reference.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            AuthorizationServiceException ex =
                new AuthorizationServiceException(null);
            Assert.IsNotNull(ex, "A new instance should be created.");

            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test AuthorizationServiceException(string),
        /// by passing an error message.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception ex = new AuthorizationServiceException(message);
            Assert.IsNotNull(ex, "A new instance should be created.");
            Assert.AreEqual(message, ex.Message,
                "ex.Message should be equal to message.");
            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test AuthorizationServiceException(string, Exception),
        /// by passing null references.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            AuthorizationServiceException ex =
                new AuthorizationServiceException(null, null);
            Assert.IsNotNull(ex, "A new instance should be created.");

            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test AuthorizationServiceException(string, Exception),
        /// by passing an error message and a null reference.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception ex = new AuthorizationServiceException(message, null);
            Assert.IsNotNull(ex, "A new instance should be created.");
            Assert.AreEqual(message, ex.Message,
                "ex.Message should be equal to message.");
            Assert.IsNull(ex.InnerException, "Invalid inner message.");
        }

        /// <summary>
        /// Test AuthorizationServiceException(string, Exception),
        /// by passing a null reference and an inner exception.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception ex = new AuthorizationServiceException(null, cause);
            Assert.IsNotNull(ex,
                "A new instance should be created.");
            Assert.AreEqual(cause, ex.InnerException,
                "ex.InnerException should be equal to cause.");

        }

        /// <summary>
        /// Test AuthorizationServiceException(string, Exception),
        /// by passing an error message and an inner exception.
        ///
        /// Should be correct.
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception ex = new AuthorizationServiceException(message, cause);
            Assert.IsNotNull(ex, "A new instance should be created.");
            Assert.AreEqual(message, ex.Message,
                "ex.Message should be equal to message.");
            Assert.AreEqual(cause, ex.InnerException,
                "ex.InnerException should be equal to cause.");
        }

        /// <summary>
        /// Test
        /// AuthorizationServiceException(SerializationInfo, StreamingContext).
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
                AuthorizationServiceException serial =
                    new AuthorizationServiceException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                AuthorizationServiceException deserial =
                    formatter.Deserialize(stream)
                    as AuthorizationServiceException;

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
