/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>Test <see cref="DateDropDownException"/> class, unit test.</para>
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DateDropDownExceptionUnitTest
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
        /// <para>Test DateDropDownException().</para>
        /// <para>The type of the exception must be correct.</para>
        /// </summary>
        [Test]
        public void TestCtor()
        {
            Exception ex = new DateDropDownException();
            Assert.IsTrue(ex is ApplicationException, "Wrong definiton of exception class.");
        }

        /// <summary>
        /// <para>Test DateDropDownException(string), by passing a null reference.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Null()
        {
            new DateDropDownException(null);
        }

        /// <summary>
        /// <para>Test DateDropDownException(string),
        /// by passing an error message.</para>
        /// </summary>
        [Test]
        public void TestCtorMessage_Valid()
        {
            Exception e = new DateDropDownException(message);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test DateDropDownException(string, Exception),
        /// by passing null references.</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null3()
        {
            new DateDropDownException(null, null);
        }

        /// <summary>
        /// <para>Test DateDropDownException(string, Exception),
        /// by passing an error message and a null reference.</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null1()
        {
            Exception e = new DateDropDownException(message, null);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
        }

        /// <summary>
        /// <para>Test DateDropDownException(string, Exception),
        /// by passing a null reference and an inner exception.</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Null2()
        {
            Exception e = new DateDropDownException(null, cause);
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test DateDropDownException(string, Exception),
        /// by passing an error message and an inner exception.</para>
        /// </summary>
        [Test]
        public void TestCtorMessageInner_Valid()
        {
            Exception e = new DateDropDownException(message, cause);
            Assert.AreEqual(message, e.Message, "e.Message should be equal to message.");
            Assert.AreEqual(cause, e.InnerException, "e.InnerException should be equal to cause.");
        }

        /// <summary>
        /// <para>Test DateDropDownException(SerializationInfo, StreamingContext).</para>
        /// <para>Desterilized instance should have same property as it before serialization.</para>
        /// </summary>
        [Test]
        public void TestCtorInfoContext()
        {
            // Stream for serialization.
            using (Stream stream = new MemoryStream())
            {
                // Serialize the instance.
                DateDropDownException serial =
                    new DateDropDownException(message, cause);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, serial);

                // Deserialize the instance.
                stream.Seek(0, SeekOrigin.Begin);
                DateDropDownException deserial =
                    formatter.Deserialize(stream) as DateDropDownException;

                // Verify the instance.
                Assert.IsFalse(serial == deserial, "Instance not deserialized.");
                Assert.AreEqual(serial.Message, deserial.Message, "Message mismatches.");
                Assert.AreEqual(serial.InnerException.Message, deserial.InnerException.Message,
                    "InnerException mismatches.");
            }
        }
    }
}

