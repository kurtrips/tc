// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the HelperClass class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HelperTests
    {
        /// <summary>
        /// Tests the ValidateNonNegative method for accuracy.
        /// </summary>
        [Test]
        public void ValidateNonNegativeAccuracy()
        {
            HelperClass.ValidateNonNegative(0, "abcd");
            HelperClass.ValidateNonNegative(5, "abcd");
        }

        /// <summary>
        /// Tests the ValidateNonNegative method for failure when value is negative.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateNonNegativeFail()
        {
            HelperClass.ValidateNonNegative(-1, "abcd");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method for accuracy.
        /// </summary>
        [Test]
        public void ValidateNotEmptyAccuracy()
        {
            HelperClass.ValidateNotEmpty("abcd", "name");
            HelperClass.ValidateNotEmpty("         s           ", "name");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method for failure when trimmed length of string is 0.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateNotEmptyFail()
        {
            HelperClass.ValidateNotEmpty("         ", "name");
        }

        /// <summary>
        /// Tests the ValidateNotNull method for accuracy.
        /// </summary>
        [Test]
        public void ValidateNotNullAccuracy()
        {
            HelperClass.ValidateNotNull("abcd", "name");
            HelperClass.ValidateNotNull(new object(), "name");
        }

        /// <summary>
        /// Tests the ValidateNotNull method for failure when null is passed.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ValidateNotNullFail()
        {
            object a = null;
            HelperClass.ValidateNotNull(a, "name");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method for accuracy.
        /// </summary>
        [Test]
        public void ValidateNotNullNotEmptyAccuracy()
        {
            HelperClass.ValidateNotNullNotEmpty("   b      ", "name");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method for failure when null is passed.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ValidateNotNullNotEmptyFail1()
        {
            string a = null;
            HelperClass.ValidateNotNullNotEmpty(a, "name");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method for failure when empty string is passed.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ValidateNotNullNotEmptyFail2()
        {
            HelperClass.ValidateNotNullNotEmpty("   ", "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for accuracy when xml is valid.
        /// </summary>
        [Test]
        public void ValidateWellFormedXml_Accuracy()
        {
            string xml = File.ReadAllText("../../test_files/valid.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml is invalid because of an invalid comment.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail1()
        {
            string xml = File.ReadAllText("../../test_files/invalid_comment.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml is invalid because of an invalid attribute.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail2()
        {
            string xml = File.ReadAllText("../../test_files/invalid_invalid_attribute.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml is invalid because tag is not closed.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail3()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_end_tag.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml has no root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail4()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_root.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml has a node has invalid name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail5()
        {
            string xml = File.ReadAllText("../../test_files/invalid_node_name.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml has a node with 2 attributes with same name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail6()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_attributes_same_name.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml has more than one root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail7()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_roots.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }

        /// <summary>
        /// Tests the ValidateWellFormedXml function for failure when xml has overlapping nodes.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void ValidateWellFormedXml_Fail8()
        {
            string xml = File.ReadAllText("../../test_files/invalid_wrong_tag_order.xml");
            HelperClass.ValidateWellFormedXml(xml, "name");
        }
    }
}
