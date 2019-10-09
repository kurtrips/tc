// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// Unit tests for the UnknownMessageTypeException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class UnknownMessageTypeExceptionTests
    {
        /// <summary>
        /// Tests the constructor.
        /// UnknownMessageTypeException()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            UnknownMessageTypeException ose = new UnknownMessageTypeException();
            Assert.IsTrue(ose is ApplicationException,
                "UnknownMessageTypeException does not derive from ApplicationException");
        }

        /// <summary>
        /// Tests the constructor.
        /// UnknownMessageTypeException(string message)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            UnknownMessageTypeException ose = new UnknownMessageTypeException("abc");
            Assert.IsTrue(ose is ApplicationException,
                "UnknownMessageTypeException does not derive from ApplicationException");
            Assert.AreEqual(ose.Message, "abc", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor.
        /// UnknownMessageTypeException(string message, Exception innerException)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            Exception e = new Exception("def");
            UnknownMessageTypeException ose = new UnknownMessageTypeException("abc", e);
            Assert.IsTrue(ose is ApplicationException,
                "UnknownMessageTypeException does not derive from ApplicationException");
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

            UnknownMessageTypeException ex1 = new UnknownMessageTypeException("Failed");
            bf.Serialize(stream, ex1);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            UnknownMessageTypeException ex2 = (UnknownMessageTypeException)bf.Deserialize(stream);
            Assert.AreEqual(ex1.Message, ex2.Message, "Error message should be correct.");
        }
    }
}
