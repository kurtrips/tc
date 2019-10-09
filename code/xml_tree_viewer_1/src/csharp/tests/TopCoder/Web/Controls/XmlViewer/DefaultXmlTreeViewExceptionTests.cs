// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the XmlTreeViewException class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class XmlTreeViewExceptionTests
    {
        /// <summary>
        /// Tests the constructor.
        /// XmlTreeViewException()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            XmlTreeViewException oce = new XmlTreeViewException();
            Assert.IsTrue(oce is XmlTreeViewException, "Exception instance is not of type XmlTreeViewException");
            Assert.IsTrue(oce is ApplicationException, "Exception instance is not of type ApplicationException");
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlTreeViewException(string message)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            XmlTreeViewException oce = new XmlTreeViewException("abc");
            Assert.IsTrue(oce is XmlTreeViewException,
                "Exception instance is not of type XmlTreeViewException");
            Assert.AreEqual(oce.Message, "abc", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlTreeViewException(string message, Exception innerException)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            Exception e = new Exception("def");
            XmlTreeViewException oce = new XmlTreeViewException("abc", e);
            Assert.IsTrue(oce is XmlTreeViewException,
                "Exception instance is not of type XmlTreeViewException");
            Assert.AreEqual(oce.Message, "abc", "Wrong constructor implementation");
            Assert.AreEqual(oce.InnerException, e, "Wrong constructor implementation");
            Assert.AreEqual(oce.InnerException.Message, "def", "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor XmlTreeViewException(SerializationInfo, StreamingContext).
        /// </summary>
        [Test]
        public void ConstructorSerializationTest()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            XmlTreeViewException ex1 = new XmlTreeViewException("Failed");
            bf.Serialize(stream, ex1);

            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            XmlTreeViewException ex2 = (XmlTreeViewException)bf.Deserialize(stream);
            Assert.AreEqual(ex1.Message, ex2.Message, "Error message should be correct.");
        }
    }
}
