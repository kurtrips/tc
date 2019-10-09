/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Graph.Layout;
using NUnit.Framework;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// Unit tests for the LayoutPreProcessingException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class LayoutPreProcessingExceptionTests
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
        /// <para>Test LayoutPreProcessingException().</para>
        ///
        /// <para>Type should be correct.</para>
        /// </summary>
        [Test]
        public void TestCtor()
        {
            Assert.IsTrue(new LayoutPreProcessingException() is LayoutException,
                "Wrong type of LayoutPreProcessingException");
            Assert.IsTrue(new LayoutPreProcessingException() is SelfDocumentingException,
                "Wrong type of LayoutPreProcessingException");
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(string), by passing a null reference.</para>
        ///
        /// <para>Should work with message as null.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            new LayoutPreProcessingException(null);
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(string),
        /// by passing an error message.</para>
        ///
        /// <para>Message should be correct.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception e = new LayoutPreProcessingException(message);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(string, Exception),
        /// by passing null references.</para>
        ///
        /// <para>Should work with message as null and innerException as null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            new LayoutPreProcessingException(null, null);
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(string, Exception),
        /// by passing an error message and a null reference.</para>
        ///
        /// <para>Should work with innerException as null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception e = new LayoutPreProcessingException(message, null);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(string, Exception),
        /// by passing a null reference and an inner exception.</para>
        /// <para>Should work with message as null and innerException as non-null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception e = new LayoutPreProcessingException(null, cause);
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(string, Exception),
        /// by passing an error message and an inner exception.</para>
        ///
        /// <para>Should have correct message and correct innerException</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception e = new LayoutPreProcessingException(message, cause);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test LayoutPreProcessingException(SerializationInfo, StreamingContext).</para>
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
                LayoutPreProcessingException serial =
                    new LayoutPreProcessingException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                LayoutPreProcessingException deserial =
                    formatter.Deserialize(stream) as LayoutPreProcessingException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial, "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message, "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message, deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }
    }
}
