// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Xml;
using TopCoder.Web.Controls.XmlViewer.Formatters;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the CssXmlFormatter class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class CssXmlFormatterTests
    {
        /// <summary>
        /// The CssXmlFormatter class instance to use throughout the tests.
        /// </summary>
        private CssXmlFormatter cxf;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            cxf = new CssXmlFormatter();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            cxf = null;
        }

        /// <summary>
        /// Tests the Constructor.
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(cxf, "Constructor of CssXmlFormatter returns null.");
            Assert.IsTrue(cxf is IXmlFormatter, "CssXmlFormatter instance has wrong type.");
            Assert.IsTrue(cxf is CssXmlFormatter, "CssXmlFormatter instance has wrong type.");
        }

        /// <summary>
        /// Tests the TagCSSClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestTagCSSClassGet()
        {
            //Tests the default value of the tagCSSClass member variable
            Assert.AreEqual(cxf.TagCSSClass, "tag", "Default value of TagCSSClass is incorrect.");

            //Set some value and check getter
            cxf.TagCSSClass = "some_css_class";
            Assert.AreEqual(cxf.TagCSSClass, "some_css_class", "TagCSSClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the TagCSSClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestTagCSSClassSet()
        {
            //Set some value and check through getter
            cxf.TagCSSClass = "some_css_class";
            Assert.AreEqual(cxf.TagCSSClass, "some_css_class", "TagCSSClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the TagCSSClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestTagCSSClassSetFail1()
        {
            cxf.TagCSSClass = (string)null;
        }

        /// <summary>
        /// Tests the TagCSSClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestTagCSSClassSetFail2()
        {
            cxf.TagCSSClass = "       ";
        }

        /// <summary>
        /// Tests the NodeNameCSSClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestNodeNameCSSClassGet()
        {
            //Tests the default value of the nodeNameCSSClass member variable
            Assert.AreEqual(cxf.NodeNameCSSClass, "node_name",
                "Default value of NodeNameCSSClass is incorrect.");

            //Set some value and check getter
            cxf.NodeNameCSSClass = "some_css_class";
            Assert.AreEqual(cxf.NodeNameCSSClass, "some_css_class",
                "NodeNameCSSClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the NodeNameCSSClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestNodeNameCSSClassSet()
        {
            //Set some value and check through getter
            cxf.NodeNameCSSClass = "some_css_class";
            Assert.AreEqual(cxf.NodeNameCSSClass, "some_css_class",
                "NodeNameCSSClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the NodeNameCSSClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestNodeNameCSSClassSetFail1()
        {
            cxf.NodeNameCSSClass = (string)null;
        }

        /// <summary>
        /// Tests the NodeNameCSSClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestNodeNameCSSClassSetFail2()
        {
            cxf.NodeNameCSSClass = "       ";
        }

        /// <summary>
        /// Tests the InnerTextCSSClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestInnerTextCSSClassGet()
        {
            //Tests the default value of the innerTextCSSClass member variable
            Assert.AreEqual(cxf.InnerTextCSSClass, "inner_text",
                "Default value of InnerTextCSSClass is incorrect.");

            //Set some value and check getter
            cxf.InnerTextCSSClass = "some_css_class";
            Assert.AreEqual(cxf.InnerTextCSSClass, "some_css_class",
                "InnerTextCSSClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the InnerTextCSSClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestInnerTextCSSClassSet()
        {
            //Set some value and check through getter
            cxf.InnerTextCSSClass = "some_css_class";
            Assert.AreEqual(cxf.InnerTextCSSClass, "some_css_class",
                "InnerTextCSSClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the InnerTextCSSClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestInnerTextCSSClassSetFail1()
        {
            cxf.InnerTextCSSClass = (string)null;
        }

        /// <summary>
        /// Tests the InnerTextCSSClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestInnerTextCSSClassSetFail2()
        {
            cxf.InnerTextCSSClass = "       ";
        }

        /// <summary>
        /// Tests the CommentCSSClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestCommentCSSClassGet()
        {
            //Tests the default value of the commentCSSClass member variable
            Assert.AreEqual(cxf.CommentCSSClass, "comment", "Default value of CommentCSSClass is incorrect.");

            //Set some value and check getter
            cxf.CommentCSSClass = "some_css_class";
            Assert.AreEqual(cxf.CommentCSSClass, "some_css_class", "CommentCSSClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the CommentCSSClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestCommentCSSClassSet()
        {
            //Set some value and check through getter
            cxf.CommentCSSClass = "some_css_class";
            Assert.AreEqual(cxf.CommentCSSClass, "some_css_class", "CommentCSSClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the CommentCSSClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCommentCSSClassSetFail1()
        {
            cxf.CommentCSSClass = (string)null;
        }

        /// <summary>
        /// Tests the CommentCSSClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestCommentCSSClassSetFail2()
        {
            cxf.CommentCSSClass = "       ";
        }

        /// <summary>
        /// Tests the AttributeValueCSSClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestAttributeValueCSSClassGet()
        {
            //Tests the default value of the attributeValueCSSClass member variable
            Assert.AreEqual(cxf.AttributeValueCSSClass, "attribute_value",
                "Default value of AttributeValueCSSClass is incorrect.");

            //Set some value and check getter
            cxf.AttributeValueCSSClass = "some_css_class";
            Assert.AreEqual(cxf.AttributeValueCSSClass, "some_css_class",
                "AttributeValueCSSClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the AttributeValueCSSClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestAttributeValueCSSClassSet()
        {
            //Set some value and check through getter
            cxf.AttributeValueCSSClass = "some_css_class";
            Assert.AreEqual(cxf.AttributeValueCSSClass, "some_css_class",
                "AttributeValueCSSClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the AttributeValueCSSClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAttributeValueCSSClassSetFail1()
        {
            cxf.AttributeValueCSSClass = (string)null;
        }

        /// <summary>
        /// Tests the AttributeValueCSSClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAttributeValueCSSClassFail2()
        {
            cxf.AttributeValueCSSClass = "       ";
        }

        /// <summary>
        /// Tests the AttributeNameCSSClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestAttributeNameCSSClassGet()
        {
            //Tests the default value of the attributeNameCSSClass member variable
            Assert.AreEqual(cxf.AttributeNameCSSClass, "attribute_name",
                "Default value of AttributeNameCSSClass is incorrect.");

            //Set some value and check getter
            cxf.AttributeNameCSSClass = "some_css_class";
            Assert.AreEqual(cxf.AttributeNameCSSClass, "some_css_class",
                "AttributeNameCSSClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the AttributeNameCSSClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestAttributeNameCSSClassSet()
        {
            //Set some value and check through getter
            cxf.AttributeNameCSSClass = "some_css_class";
            Assert.AreEqual(cxf.AttributeNameCSSClass, "some_css_class",
                "AttributeNameCSSClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the AttributeNameCSSClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAttributeNameCSSClassSetFail1()
        {
            cxf.AttributeNameCSSClass = (string)null;
        }

        /// <summary>
        /// Tests the AttributeNameCSSClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAttributeNameCSSClassFail2()
        {
            cxf.AttributeNameCSSClass = "       ";
        }

        /// <summary>
        /// Tests the Indentation getter for accuracy.
        /// </summary>
        [Test]
        public void TestIndentationGet()
        {
            //Tests the default value of the indentation member variable
            Assert.AreEqual(cxf.Indentation, 2,
                "Default value of Indentation is incorrect.");

            //Set some value and check getter
            cxf.Indentation = 5;
            Assert.AreEqual(cxf.Indentation, 5,
                "Indentation getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Indentation setter for accuracy.
        /// </summary>
        [Test]
        public void TestIndentationSet()
        {
            //Set some value and check through getter
            cxf.Indentation = 5;
            Assert.AreEqual(cxf.Indentation, 5,
                "Indentation setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Indentation setter for failure when trying to set to a negative value.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestIndentationFail2()
        {
            cxf.Indentation = -1;
        }

        /// <summary>
        /// Tests the Format method.
        /// string Format(string xml)
        /// </summary>
        [Test]
        public void TestFormat()
        {
            string formattedActual = cxf.Format(File.ReadAllText("../../test_files/valid.xml"));
            string formattedExpected = File.ReadAllText("../../test_files/expectedFormat.txt");

            Assert.AreEqual(formattedActual, formattedExpected, "CSS Formatting is incorrect.");
        }

        /// <summary>
        /// Tests the Format method when custom css class names and indentation is used.
        /// string Format(string xml)
        /// </summary>
        [Test]
        public void TestFormatCustom()
        {
            cxf.AttributeNameCSSClass = "myANClass";
            cxf.AttributeValueCSSClass = "myAVClass";
            cxf.CommentCSSClass = "myCMClass";
            cxf.Indentation = 4;
            cxf.InnerTextCSSClass = "myITClass";
            cxf.NodeNameCSSClass = "myNNClass";
            cxf.TagCSSClass = "myTGClass";

            string formattedActual = cxf.Format(File.ReadAllText("../../test_files/valid.xml"));
            string formattedExpected = File.ReadAllText("../../test_files/expectedFormatCustom.txt");

            Assert.AreEqual(formattedActual, formattedExpected, "CSS Formatting is incorrect.");
        }

        /// <summary>
        /// Tests the format function for failure when xml is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestFormat_Fail1()
        {
            cxf.Format((string)null);
        }

        /// <summary>
        /// Tests the format function for failure when xml is empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestFormat_Fail2()
        {
            cxf.Format("           ");
        }

        /// <summary>
        /// Tests the format function for failure when xml is invalid because of an invalid comment.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail3()
        {
            string xml = File.ReadAllText("../../test_files/invalid_comment.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml is invalid because of an invalid attribute.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail4()
        {
            string xml = File.ReadAllText("../../test_files/invalid_invalid_attribute.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml is invalid because tag is not closed.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail5()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_end_tag.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml has no root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail6()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_root.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml has a node has invalid name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail7()
        {
            string xml = File.ReadAllText("../../test_files/invalid_node_name.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml has a node with 2 attributes with same name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail8()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_attributes_same_name.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml has more than one root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail9()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_roots.xml");
            cxf.Format(xml);
        }

        /// <summary>
        /// Tests the format function for failure when xml has overlapping nodes.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestFormat_Fail10()
        {
            string xml = File.ReadAllText("../../test_files/invalid_wrong_tag_order.xml");
            cxf.Format(xml);
        }

    }

    /// <summary>
    /// Unit tests for the CssXmlFormatter class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class CssXmlFormatterTestsProtected : CssXmlFormatter
    {
        /// <summary>
        /// Tests the FormatNode method for accuracy.
        /// </summary>
        [Test]
        public void TestFormatNode()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../test_files/valid.xml");

            string formattedActual = FormatNode(doc.DocumentElement, 0);

            string formattedExpected = File.ReadAllText("../../test_files/rootNodeExpectedFormat.txt");

            Assert.AreEqual(formattedActual, formattedExpected, "CSS Formatting is incorrect.");
        }

        /// <summary>
        /// Tests the FormatNode method for failure when XmlNode is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestFormatNodeFail1()
        {
            FormatNode(null, 0);
        }

        /// <summary>
        /// Tests the FormatNode method for failure when indent is negative.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestFormatNodeFail2()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../test_files/valid.xml");

            FormatNode(doc.DocumentElement, -1);
        }
    }
}
