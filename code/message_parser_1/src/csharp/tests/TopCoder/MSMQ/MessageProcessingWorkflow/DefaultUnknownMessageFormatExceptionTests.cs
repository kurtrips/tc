// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// Unit tests for the UnknownMessageFormatException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class UnknownMessageFormatExceptionTests
    {
        /// <summary>
        /// Tests the constructor.
        /// UnknownMessageFormatException()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            UnknownMessageFormatException ose = new UnknownMessageFormatException();
            Assert.IsTrue(ose is ApplicationException,
                "UnknownMessageFormatException does not derive from ApplicationException");
        }

        /// <summary>
        /// Tests the constructor.
        /// UnknownMessageFormatException(string message)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            UnknownMessageFormatException ose = new UnknownMessageFormatException("abc");
            Assert.IsTrue(ose is ApplicationException,
                "UnknownMessageFormatException does not derive from ApplicationException");
            Assert.AreEqual(ose.Message, "abc", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor.
        /// UnknownMessageFormatException(string message, Exception innerException)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            Exception e = new Exception("def");
            UnknownMessageFormatException ose = new UnknownMessageFormatException("abc", e);
            Assert.IsTrue(ose is ApplicationException,
                "UnknownMessageFormatException does not derive from ApplicationException");
            Assert.AreEqual(ose.Message, "abc", "Wrong constructor implementation");
            Assert.AreEqual(ose.InnerException, e, "Wrong constructor implementation");
            Assert.AreEqual(ose.InnerException.Message, "def", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor ApplicationException(SerializationInfo, StreamingContext).
        /// </summary>
        [Test]
        public void ConstructorSerializationTest()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            UnknownMessageFormatException ex1 = new UnknownMessageFormatException("Failed");
            bf.Serialize(stream, ex1);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            UnknownMessageFormatException ex2 = (UnknownMessageFormatException)bf.Deserialize(stream);
            Assert.AreEqual(ex1.Message, ex2.Message, "Error message should be correct.");
        }
    }
}
