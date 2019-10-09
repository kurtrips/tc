// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using TopCoder.Web.Controls.XmlViewer.Formatters;
using TopCoder.Web.UI.WebControl.TreeView;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the XmlTreeViewControl class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class XmlTreeViewControlTests
    {
        /// <summary>
        /// The XmlTreeViewControl instance to use throughout the tests.
        /// </summary>
        XmlTreeViewControl xtvc;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            xtvc = new XmlTreeViewControl();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            xtvc = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlTreeViewControl()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            //Check object
            Assert.IsNotNull(xtvc, "Constructor returns null reference");
            Assert.IsTrue(xtvc is System.Web.UI.UserControl, "XmlTreeViewControl has wrong type.");
            Assert.IsTrue(xtvc is INamingContainer, "XmlTreeViewControl has wrong type.");

            //Check if controls are added
            Assert.AreEqual(xtvc.Controls.Count, 5, "Controls are not added correctly to object.");
            Assert.IsTrue(xtvc.Controls[0] is Button, "Controls are not added correctly to object.");
            Assert.IsTrue(xtvc.Controls[1] is ImageButton, "Controls are not added correctly to object.");
            Assert.IsTrue(xtvc.Controls[2] is Literal, "Controls are not added correctly to object.");
            Assert.IsTrue(xtvc.Controls[3] is Literal, "Controls are not added correctly to object.");
            Assert.IsTrue(xtvc.Controls[4] is TreeViewControl, "Controls are not added correctly to object.");
        }

        /// <summary>
        /// Tests the Xml getter method for accuracy
        /// </summary>
        [Test]
        public void TestGetXml()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            xtvc.Xml = validXml;
            Assert.AreEqual(validXml, xtvc.Xml, "Xml getter property incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Xml setter for accuracy.
        /// </summary>
        [Test]
        public void TestSetXml()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            xtvc.Xml = validXml;
            Assert.AreEqual(validXml, xtvc.Xml, "Xml setter property incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSetXml_Fail1()
        {
            xtvc.Xml = (string)null;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetXml_Fail2()
        {
            xtvc.Xml = "           ";
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is invalid because of an invalid comment.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail3()
        {
            string xml = File.ReadAllText("../../test_files/invalid_comment.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is invalid because of an invalid attribute.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail4()
        {
            string xml = File.ReadAllText("../../test_files/invalid_invalid_attribute.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is invalid because tag is not closed.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail5()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_end_tag.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has no root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail6()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_root.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has a node has invalid name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail7()
        {
            string xml = File.ReadAllText("../../test_files/invalid_node_name.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has a node with 2 attributes with same name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail8()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_attributes_same_name.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has more than one root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail9()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_roots.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has overlapping nodes.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail10()
        {
            string xml = File.ReadAllText("../../test_files/invalid_wrong_tag_order.xml");
            xtvc.Xml = xml;
        }

        /// <summary>
        /// Tests the Formatter getter.
        /// </summary>
        [Test]
        public void TestFormatterGet()
        {
            //Test normally
            CssXmlFormatter formatter = new CssXmlFormatter();
            xtvc.Formatter = formatter;
            Assert.AreEqual(formatter, xtvc.Formatter, "Formatter getter property incorrectly implemented.");

            //Test with null (CssXmlFormatter instance shoould be returned)
            xtvc.Formatter = null;
            Assert.IsTrue(xtvc.Formatter is CssXmlFormatter, "Formatter getter property incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Formatter setter.
        /// </summary>
        [Test]
        public void TestFormatterSet()
        {
            CssXmlFormatter formatter = new CssXmlFormatter();
            xtvc.Formatter = formatter;
            Assert.AreEqual(formatter, xtvc.Formatter, "Formatter setter property incorrectly implemented.");
        }

        /// <summary>
        /// Tests the LeafIconUrl getter for accuracy.
        /// </summary>
        [Test]
        public void TestLeafIconUrlGet()
        {
            //Set some value and check getter
            xtvc.LeafIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.LeafIconUrl, "abcd.png", "LeafIconUrl getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the LeafIconUrl setter for accuracy.
        /// </summary>
        [Test]
        public void TestLeafIconUrlSet()
        {
            //Set some value and check through getter
            xtvc.LeafIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.LeafIconUrl, "abcd.png", "LeafIconUrl setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the LeafIconUrl setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLeafIconUrlFail1()
        {
            xtvc.LeafIconUrl = (string)null;
        }

        /// <summary>
        /// Tests the LeafIconUrl setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLeafIconUrlFail2()
        {
            xtvc.LeafIconUrl = "       ";
        }

        /// <summary>
        /// Tests the FolderIconUrl getter for accuracy.
        /// </summary>
        [Test]
        public void TestFolderIconUrlGet()
        {
            //Set some value and check getter
            xtvc.FolderIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.FolderIconUrl, "abcd.png", "FolderIconUrl getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the FolderIconUrl setter for accuracy.
        /// </summary>
        [Test]
        public void TestFolderIconUrlSet()
        {
            //Set some value and check through getter
            xtvc.FolderIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.FolderIconUrl, "abcd.png", "FolderIconUrl setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the FolderIconUrl setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestFolderIconUrlFail1()
        {
            xtvc.FolderIconUrl = (string)null;
        }

        /// <summary>
        /// Tests the FolderIconUrl setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestFolderIconUrlFail2()
        {
            xtvc.FolderIconUrl = "       ";
        }

        /// <summary>
        /// Tests the Prefixes getter.
        /// </summary>
        [Test]
        public void TestPrefixesGet()
        {
            string[] arr = new string[] { "a", "", "cdef" };
            xtvc.Prefixes = arr;
            Assert.AreEqual(xtvc.Prefixes, arr, "Prefixes getter implememented incorrectly.");
        }

        /// <summary>
        /// Tests the Prefixes setter for accuracy.
        /// </summary>
        [Test]
        public void TestPrefixesSet()
        {
            string[] arr = new string[] { "a", "", "cdef" };
            xtvc.Prefixes = arr;
            Assert.AreEqual(xtvc.Prefixes, arr, "Prefixes setter implememented incorrectly.");
        }

        /// <summary>
        /// Tests the Prefixes setter for failure when null is passed.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestPrefixesSetFail1()
        {
            xtvc.Prefixes = (string[])null;
        }

        /// <summary>
        /// Tests the Prefixes setter for failure when empty array is passed.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestPrefixesSetFail2()
        {
            string[] srr = new string[0];
            xtvc.Prefixes = srr;
        }

        /// <summary>
        /// Tests the Prefixes setter for failure when array with null element is passed.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestPrefixesSetFail3()
        {
            string[] srr = new string[] { "a", null };
            xtvc.Prefixes = srr;
        }

        /// <summary>
        /// Tests the CssClasses getter.
        /// </summary>
        [Test]
        public void TestCssClassesGet()
        {
            string[] arr = new string[] { "a", "", "cdef" };
            xtvc.CssClasses = arr;
            Assert.AreEqual(xtvc.CssClasses, arr, "CssClasses getter implememented incorrectly.");
        }

        /// <summary>
        /// Tests the CssClasses setter for accuracy.
        /// </summary>
        [Test]
        public void TestCssClassesSet()
        {
            string[] arr = new string[] { "a", "", "cdef" };
            xtvc.CssClasses = arr;
            Assert.AreEqual(xtvc.CssClasses, arr, "CssClasses setter implememented incorrectly.");
        }

        /// <summary>
        /// Tests the CssClasses setter for failure when null is passed.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCssClassesSetFail1()
        {
            xtvc.CssClasses = (string[])null;
        }

        /// <summary>
        /// Tests the CssClasses setter for failure when empty array is passed.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestCssClassesSetFail2()
        {
            string[] srr = new string[0];
            xtvc.CssClasses = srr;
        }

        /// <summary>
        /// Tests the CssClasses setter for failure when array with null element is passed.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestCssClassesSetFail3()
        {
            string[] srr = new string[] { "a", null };
            xtvc.CssClasses = srr;
        }

        /// <summary>
        /// Tests the ExpandingIconUrl getter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingIconUrlGet()
        {
            //Set some value and check getter
            xtvc.ExpandingIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingIconUrl, "abcd.png", "ExpandingIconUrl getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingIconUrl setter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingIconUrlSet()
        {
            //Set some value and check through getter
            xtvc.ExpandingIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingIconUrl, "abcd.png", "ExpandingIconUrl setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingIconUrl setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestExpandingIconUrlFail1()
        {
            xtvc.ExpandingIconUrl = (string)null;
        }

        /// <summary>
        /// Tests the ExpandingIconUrl setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestExpandingIconUrlFail2()
        {
            xtvc.ExpandingIconUrl = "       ";
        }

        /// <summary>
        /// Tests the ExpandingText getter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingTextGet()
        {
            //Set some value and check getter
            xtvc.ExpandingText = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingText, "abcd.png", "ExpandingText getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingText setter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingTextSet()
        {
            //Set some value and check through getter
            xtvc.ExpandingText = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingText, "abcd.png", "ExpandingText setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingText setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestExpandingTextFail1()
        {
            xtvc.ExpandingText = (string)null;
        }

        /// <summary>
        /// Tests the ExpandingText setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestExpandingTextFail2()
        {
            xtvc.ExpandingText = "       ";
        }

        /// <summary>
        /// Tests the ExpandingCssClass getter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingCssClassGet()
        {
            //Set some value and check getter
            xtvc.ExpandingCssClass = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingCssClass, "abcd.png", "ExpandingCssClass getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingCssClass setter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingCssClassSet()
        {
            //Set some value and check through getter
            xtvc.ExpandingCssClass = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingCssClass, "abcd.png", "ExpandingCssClass setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingCssClass setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestExpandingCssClassFail1()
        {
            xtvc.ExpandingCssClass = (string)null;
        }

        /// <summary>
        /// Tests the ExpandingCssClass setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestExpandingCssClassFail2()
        {
            xtvc.ExpandingCssClass = "       ";
        }

        /// <summary>
        /// Tests the ExpandingFailureText getter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingFailureTextGet()
        {
            //Set some value and check getter
            xtvc.ExpandingFailureText = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingFailureText, "abcd.png",
                "ExpandingFailureText getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingFailureText setter for accuracy.
        /// </summary>
        [Test]
        public void TestExpandingFailureTextSet()
        {
            //Set some value and check through getter
            xtvc.ExpandingFailureText = "abcd.png";
            Assert.AreEqual(xtvc.ExpandingFailureText, "abcd.png",
                "ExpandingFailureText setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the ExpandingFailureText setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestExpandingFailureTextFail1()
        {
            xtvc.ExpandingFailureText = (string)null;
        }

        /// <summary>
        /// Tests the ExpandingFailureText setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestExpandingFailureTextFail2()
        {
            xtvc.ExpandingFailureText = "       ";
        }

        /// <summary>
        /// Tests the SaveButtonIconUrl getter for accuracy.
        /// </summary>
        [Test]
        public void TestSaveButtonIconUrlGet()
        {
            //Set some value and check getter
            xtvc.SaveButtonIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.SaveButtonIconUrl, "abcd.png", "SaveButtonIconUrl getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the SaveButtonIconUrl setter for accuracy.
        /// </summary>
        [Test]
        public void TestSaveButtonIconUrlSet()
        {
            //Set some value and check through getter
            xtvc.SaveButtonIconUrl = "abcd.png";
            Assert.AreEqual(xtvc.SaveButtonIconUrl, "abcd.png", "SaveButtonIconUrl setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the SaveButtonIconUrl setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSaveButtonIconUrlFail1()
        {
            xtvc.SaveButtonIconUrl = (string)null;
        }

        /// <summary>
        /// Tests the SaveButtonIconUrl setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSaveButtonIconUrlFail2()
        {
            xtvc.SaveButtonIconUrl = "       ";
        }

        /// <summary>
        /// Tests the SaveFileName getter for accuracy.
        /// </summary>
        [Test]
        public void TestSaveFileNameGet()
        {
            //Check getter without setting value. xmlfile.xml should be returned.
            Assert.AreEqual(xtvc.SaveFileName, "XmlTree.xml", "SaveFileName getter incorrectly implemented.");

            //Set some value and check getter
            xtvc.SaveFileName = "abc.xml";
            Assert.AreEqual(xtvc.SaveFileName, "abc.xml", "SaveFileName getter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the SaveFileName setter for accuracy.
        /// </summary>
        [Test]
        public void TestSaveFileNameSet()
        {
            //Set some value and check through getter
            xtvc.SaveFileName = "abcd.png";
            Assert.AreEqual(xtvc.SaveFileName, "abcd.png", "SaveFileName setter incorrectly implemented.");
        }

        /// <summary>
        /// Tests the SaveFileName setter for failure when trying to set to null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSaveFileNameFail1()
        {
            xtvc.SaveFileName = (string)null;
        }

        /// <summary>
        /// Tests the SaveFileName setter for failure when trying to set to empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSaveFileNameFail2()
        {
            xtvc.SaveFileName = "       ";
        }

        /// <summary>
        /// Tests the ViewType getter.
        /// </summary>
        [Test]
        public void TestViewTypeGetter()
        {
            //Set xml because the setter calls ReloadControl
            xtvc.Xml = File.ReadAllText("../../test_files/valid.xml");

            //Test without setting any value
            Assert.AreEqual(xtvc.ViewType, XmlTreeViewType.TreeView, "Default value returned by getter is incorrect");

            //Test after setting any value
            xtvc.ViewType = XmlTreeViewType.RawXml;
            Assert.AreEqual(xtvc.ViewType, XmlTreeViewType.RawXml, "Value returned by getter is incorrect");
        }

        /// <summary>
        /// Tests the ViewType setter for failure when invalid enum value is passed.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestViewTypeSetterFail()
        {
            xtvc.ViewType = (XmlTreeViewType)2;
        }

        /// <summary>
        /// Tests the ViewType setter
        /// </summary>
        [Test]
        public void TestViewTypeSetter()
        {
            //Set xml because this setter calls ReloadControl
            xtvc.Xml = File.ReadAllText("../../test_files/valid.xml");

            xtvc.ViewType = XmlTreeViewType.RawXml;
            Assert.AreEqual(xtvc.ViewType, XmlTreeViewType.RawXml, "Value set by setter is incorrect");

            xtvc.ViewType = XmlTreeViewType.TreeView;
            Assert.AreEqual(xtvc.ViewType, XmlTreeViewType.TreeView, "Value set by setter is incorrect");
        }

        /// <summary>
        /// Tests the GenericPrefix Getter method.
        /// </summary>
        [Test]
        public void TestGenericPrefixGetter()
        {
            xtvc.GenericPrefix = "abc";
            Assert.AreEqual(xtvc.GenericPrefix, "abc", "Wrong getter implementation");
        }

        /// <summary>
        /// Tests the set_GenericPrefix method.
        /// void set_GenericPrefix(string value)
        /// </summary>
        [Test]
        public void TestGenericPrefixSetter()
        {
            //Before setting, prefixes must be null
            Assert.IsNull(xtvc.Prefixes, "Wrong setter implementation.");

            xtvc.GenericPrefix = "ab";
            Assert.AreEqual(xtvc.GenericPrefix, "ab", "Wrong setter implementation");
            Assert.AreEqual(xtvc.Prefixes.Length, 10, "Wrong setter implementation");

            string s = "ab";
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(xtvc.Prefixes[i], s, "Wrong setter implementation");
                s += "ab";
            }

            //Can be set to null
            xtvc.GenericPrefix = null;
            Assert.IsNull(xtvc.GenericPrefix, null, "Wrong setter implementation");
        }
    }
}
