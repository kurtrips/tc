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
    /// Unit tests for the LayoutPostProcessingException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class LayoutPostProcessingExceptionTests
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
        /// <para>Test LayoutPostProcessingException().</para>
        ///
        /// <para>Type should be correct.</para>
        /// </summary>
        [Test]
        public void TestCtor()
        {
            Assert.IsTrue(new LayoutPostProcessingException() is LayoutException,
                "Wrong type of LayoutPostProcessingException");
            Assert.IsTrue(new LayoutPostProcessingException() is SelfDocumentingException,
                "Wrong type of LayoutPostProcessingException");
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(string), by passing a null reference.</para>
        ///
        /// <para>Should work with message as null.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            new LayoutPostProcessingException(null);
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(string),
        /// by passing an error message.</para>
        ///
        /// <para>Message should be correct.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception e = new LayoutPostProcessingException(message);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(string, Exception),
        /// by passing null references.</para>
        ///
        /// <para>Should work with message as null and innerException as null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            new LayoutPostProcessingException(null, null);
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(string, Exception),
        /// by passing an error message and a null reference.</para>
        ///
        /// <para>Should work with innerException as null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception e = new LayoutPostProcessingException(message, null);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(string, Exception),
        /// by passing a null reference and an inner exception.</para>
        /// <para>Should work with message as null and innerException as non-null</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception e = new LayoutPostProcessingException(null, cause);
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(string, Exception),
        /// by passing an error message and an inner exception.</para>
        ///
        /// <para>Should have correct message and correct innerException</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception e = new LayoutPostProcessingException(message, cause);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test LayoutPostProcessingException(SerializationInfo, StreamingContext).</para>
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
                LayoutPostProcessingException serial =
                    new LayoutPostProcessingException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                LayoutPostProcessingException deserial =
                    formatter.Deserialize(stream) as LayoutPostProcessingException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial, "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message, "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message, deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }
    }
}
