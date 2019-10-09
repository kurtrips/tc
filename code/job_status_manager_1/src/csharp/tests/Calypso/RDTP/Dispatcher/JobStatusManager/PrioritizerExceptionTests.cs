/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */


using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Unit tests for the PrioritizerException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class PrioritizerExceptionTests
    {
        /// <summary>
        /// <para>Message string for test.</para>
        /// </summary>
        private string message = "message";

        /// <summary>
        /// <para>Exception instance for test.</para>
        /// </summary>
        private Exception cause = new Exception("innerException");

        /// <summary>
        /// <para>Test PrioritizerException().</para>
        ///
        /// <para>Type should be correct.</para>
        /// </summary>
        [Test]
        public void TestCtor()
        {
            Assert.IsTrue(new PrioritizerException() is ApplicationException,
                "Wrong type of PrioritizerException");
        }

        /// <summary>
        /// <para>Test PrioritizerException(string), by passing a null reference.</para>
        ///
        /// <para>Should work with message as null.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            new PrioritizerException(null);
        }

        /// <summary>
        /// <para>Test PrioritizerException(string),
        /// by passing an error message.</para>
        ///
        /// <para>Message should be correct.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception e = new PrioritizerException(message);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test PrioritizerException(string, Exception),
        /// by passing null references.</para>
        ///
        /// <para>Should work with message as null and innerException as null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            new PrioritizerException(null, null);
        }

        /// <summary>
        /// <para>Test PrioritizerException(string, Exception),
        /// by passing an error message and a null reference.</para>
        ///
        /// <para>Should work with innerException as null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception e = new PrioritizerException(message, null);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test PrioritizerException(string, Exception),
        /// by passing a null reference and an inner exception.</para>
        /// <para>Should work with message as null and innerException as non-null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception e = new PrioritizerException(null, cause);
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test PrioritizerException(string, Exception),
        /// by passing an error message and an inner exception.</para>
        ///
        /// <para>Should have correct message and correct innerException</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception e = new PrioritizerException(message, cause);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test PrioritizerException(SerializationInfo, StreamingContext).</para>
        ///
        /// <para>Desterilized instance should have same property as it before serialization.</para>
        /// </summary>
        [Test]
        public void TestCtorInfoContext()
        {
            // Stream for serialization.
            using (Stream stream = new MemoryStream())
            {
                // Serialize the instance.
                PrioritizerException serial =
                    new PrioritizerException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                PrioritizerException deserial =
                    formatter.Deserialize(stream) as PrioritizerException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial, "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message, "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message, deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }
    }
}
