// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the InvalidXmlException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class InvalidXmlExceptionTests
    {
        /// <summary>
        /// Tests the constructor.
        /// InvalidXmlException()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            InvalidXmlException oce = new InvalidXmlException();
            Assert.IsTrue(oce is InvalidXmlException, "Exception instance is not of type InvalidXmlException");
            Assert.IsTrue(oce is XmlTreeViewException, "Exception instance is not of type XmlTreeViewException");
        }

        /// <summary>
        /// Tests the constructor.
        /// InvalidXmlException(string message)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            InvalidXmlException oce = new InvalidXmlException("abc");
            Assert.IsTrue(oce is InvalidXmlException,
                "Exception instance is not of type InvalidXmlException");
            Assert.AreEqual(oce.Message, "abc", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor.
        /// InvalidXmlException(string message, Exception innerException)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            Exception e = new Exception("def");
            InvalidXmlException oce = new InvalidXmlException("abc", e);
            Assert.IsTrue(oce is InvalidXmlException,
                "Exception instance is not of type InvalidXmlException");
            Assert.AreEqual(oce.Message, "abc", "Wrong constructor implementation");
            Assert.AreEqual(oce.InnerException, e, "Wrong constructor implementation");
            Assert.AreEqual(oce.InnerException.Message, "def", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor InvalidXmlException(SerializationInfo, StreamingContext).
        /// </summary>
        [Test]
        public void ConstructorSerializationTest()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            InvalidXmlException ex1 = new InvalidXmlException("Failed");
            bf.Serialize(stream, ex1);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            InvalidXmlException ex2 = (InvalidXmlException)bf.Deserialize(stream);
            Assert.AreEqual(ex1.Message, ex2.Message, "Error message should be correct.");
        }
    }
}
