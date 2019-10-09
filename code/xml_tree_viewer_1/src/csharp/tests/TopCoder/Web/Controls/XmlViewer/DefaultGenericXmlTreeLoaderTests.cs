// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using TopCoder.Web.UI.WebControl.TreeView;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the public methods of GenericXmlTreeLoader class.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class GenericXmlTreeLoaderTestsPublic
    {
        /// <summary>
        /// The GenericXmlTreeLoader instance to use throughout this test case.
        /// </summary>
        private GenericXmlTreeLoader gxtl;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            gxtl = new GenericXmlTreeLoader();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            gxtl = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// GenericXmlTreeLoader()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsNotNull(gxtl, "GenericXmlTreeLoader constructor returns null.");
            Assert.IsTrue(gxtl is ITreeLoader, "GenericXmlTreeLoader instance has incorrect type.");
        }

        /// <summary>
        /// Tests the constructor for accuracy.
        /// GenericXmlTreeLoader(string xml)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");

            //No exception should be thrown here
            gxtl = new GenericXmlTreeLoader(validXml);

            Assert.IsNotNull(gxtl, "GenericXmlTreeLoader constructor returns null.");
            Assert.IsTrue(gxtl is ITreeLoader, "GenericXmlTreeLoader instance has incorrect type.");
            Assert.AreEqual(gxtl.Xml, validXml, "Constructor implementation is incorrect.");
        }

        /// <summary>
        /// Tests the constructor for failure when xml is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor2_Fail1()
        {
            gxtl = new GenericXmlTreeLoader((string)null);
        }

        /// <summary>
        /// Tests the constructor for failure when xml is empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail2()
        {
            gxtl = new GenericXmlTreeLoader("           ");
        }

        /// <summary>
        /// Tests the constructor for failure when xml is invalid because of an invalid comment.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail3()
        {
            string xml = File.ReadAllText("../../test_files/invalid_comment.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml is invalid because of an invalid attribute.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail4()
        {
            string xml = File.ReadAllText("../../test_files/invalid_invalid_attribute.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml is invalid because tag is not closed.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail5()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_end_tag.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml has no root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail6()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_root.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml has a node has invalid name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail7()
        {
            string xml = File.ReadAllText("../../test_files/invalid_node_name.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml has a node with 2 attributes with same name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail8()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_attributes_same_name.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml has more than one root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail9()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_roots.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the constructor for failure when xml has overlapping nodes.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestConstructor2_Fail10()
        {
            string xml = File.ReadAllText("../../test_files/invalid_wrong_tag_order.xml");
            gxtl = new GenericXmlTreeLoader(xml);
        }

        /// <summary>
        /// Tests the Xml getter method for accuracy
        /// </summary>
        [Test]
        public void TestGetXml()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl.Xml = validXml;
            Assert.AreEqual(validXml, gxtl.Xml, "Xml getter property incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Xml setter for accuracy.
        /// </summary>
        [Test]
        public void TestSetXml()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl.Xml = validXml;
            Assert.AreEqual(validXml, gxtl.Xml, "Xml setter property incorrectly implemented.");
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSetXml_Fail1()
        {
            gxtl.Xml = (string)null;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is empty string.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSetXml_Fail2()
        {
            gxtl.Xml = "           ";
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is invalid because of an invalid comment.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail3()
        {
            string xml = File.ReadAllText("../../test_files/invalid_comment.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is invalid because of an invalid attribute.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail4()
        {
            string xml = File.ReadAllText("../../test_files/invalid_invalid_attribute.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml is invalid because tag is not closed.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail5()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_end_tag.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has no root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail6()
        {
            string xml = File.ReadAllText("../../test_files/invalid_no_root.xml");
            gxtl.Xml =xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has a node has invalid name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail7()
        {
            string xml = File.ReadAllText("../../test_files/invalid_node_name.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has a node with 2 attributes with same name.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail8()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_attributes_same_name.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has more than one root node.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail9()
        {
            string xml = File.ReadAllText("../../test_files/invalid_two_roots.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the Xml setter for failure when xml has overlapping nodes.
        /// InvalidXmlException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidXmlException))]
        public void TestSetXml_Fail10()
        {
            string xml = File.ReadAllText("../../test_files/invalid_wrong_tag_order.xml");
            gxtl.Xml = xml;
        }

        /// <summary>
        /// Tests the LoadTree method for accuracy
        /// </summary>
        [Test]
        public void TestLoadTree()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl = new GenericXmlTreeLoader(validXml);

            TreeNode treeNode = gxtl.LoadTree();

            //Check if LoadTree has expected results
            Assert.IsNotNull(treeNode, "LoadTree returns null.");
            Assert.AreEqual(treeNode.ChildCount, 1, "LoadTree implementation is incorrect.");

            TreeNode treeNode1 = (TreeNode)(treeNode.Children[0]);
            Assert.AreEqual(treeNode1.ChildCount, 4, "LoadTree implementation is incorrect.");
            Assert.AreEqual(treeNode1.Text, "Message", "LoadTree implementation is incorrect.");

            TreeNode treeNode11 = (TreeNode)(treeNode1.Children[0]);
            TreeNode treeNode12 = (TreeNode)(treeNode1.Children[1]);
            TreeNode treeNode13 = (TreeNode)(treeNode1.Children[2]);
            TreeNode treeNode14 = (TreeNode)(treeNode1.Children[3]);

            Assert.AreEqual(treeNode11.ChildCount, 0, "LoadTree implementation is incorrect.");
            Assert.AreEqual(treeNode11.Text, "Status (Pending)", "LoadTree implementation is incorrect.");

            Assert.AreEqual(treeNode12.ChildCount, 0, "LoadTree implementation is incorrect.");
            Assert.AreEqual(treeNode12.Text, "User (John)", "LoadTree implementation is incorrect.");

            Assert.AreEqual(treeNode13.ChildCount, 0, "LoadTree implementation is incorrect.");
            Assert.AreEqual(treeNode13.Text, "Security (None)", "LoadTree implementation is incorrect.");

            Assert.AreEqual(treeNode14.ChildCount, 4, "LoadTree implementation is incorrect.");
            Assert.AreEqual(treeNode14.Text, "Entries", "LoadTree implementation is incorrect.");
        }

        /// <summary>
        /// Tests the LoadTree method for failure when xml is null.
        /// InvalidOperationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadTreeFail1()
        {
            gxtl = new GenericXmlTreeLoader();
            gxtl.LoadTree();
        }

        /// <summary>
        /// Tests the LoadChildren method for accuracy.
        /// TreeNode[] LoadChildren(Int32[] path)
        /// </summary>
        [Test]
        public void TestLoadChildren()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl = new GenericXmlTreeLoader(validXml);

            //Try some paths and check for correct return values.
            int[] path1 = new int[] { 0, 3, 1 };
            TreeNode[] treeNodeArr = gxtl.LoadChildren(path1);
            Assert.IsNotNull(treeNodeArr, "LoadChildren returns null");
            Assert.AreEqual(treeNodeArr.Length, 2, "LoadChildren returns wrong node");
            Assert.AreEqual(treeNodeArr[0].Text, "ID (2)", "LoadChildren returns wrong node");

            int[] path2 = new int[] { 0, 3, 1 };
            treeNodeArr = gxtl.LoadChildren(path2);
            Assert.IsNotNull(treeNodeArr, "LoadChildren returns null");
            Assert.AreEqual(treeNodeArr.Length, 2, "LoadChildren returns wrong node");
            Assert.AreEqual(treeNodeArr[1].Text, "Date (\r\n        03/08/2007)", "LoadChildren returns wrong node");

            int[] path3 = new int[] { 0, 2 };
            treeNodeArr = gxtl.LoadChildren(path3);
            Assert.IsNotNull(treeNodeArr, "LoadChildren returns null");
            Assert.AreEqual(treeNodeArr.Length, 0, "LoadChildren returns wrong node");

            int[] path4 = new int[] { 0, 3 };
            treeNodeArr = gxtl.LoadChildren(path4);
            Assert.IsNotNull(treeNodeArr, "LoadChildren returns null");
            Assert.AreEqual(treeNodeArr.Length, 4, "LoadChildren returns wrong node");
            Assert.AreEqual(treeNodeArr[0].Text, "Entry", "LoadChildren returns wrong node");
            Assert.AreEqual(treeNodeArr[1].Text, "Entry", "LoadChildren returns wrong node");
            Assert.AreEqual(treeNodeArr[2].Text, "Entry (\r\n      Test with a mixed element  content\r\n    )",
                "LoadChildren returns wrong node");
        }

        /// <summary>
        /// Tests the LoadChildren method for failure when path is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLoadChildrenFail1()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl = new GenericXmlTreeLoader(validXml);

            gxtl.LoadChildren((int[])null);
        }

        /// <summary>
        /// Tests the LoadChildren method for failure when path is empty array.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoadChildrenFail2()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl = new GenericXmlTreeLoader(validXml);

            gxtl.LoadChildren(new int[0]);
        }

        /// <summary>
        /// Tests the LoadChildren method for failure when path has a negative element.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoadChildrenFail3()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl = new GenericXmlTreeLoader(validXml);

            gxtl.LoadChildren(new int[] { 0, -1 });
        }

        /// <summary>
        /// Tests the LoadChildren method for failure when path is invalid.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoadChildrenFail4()
        {
            string validXml = File.ReadAllText("../../test_files/valid.xml");
            gxtl = new GenericXmlTreeLoader(validXml);

            gxtl.LoadChildren(new int[] { 0, 5 });
        }
    }

    /// <summary>
    /// Unit tests for the protected methods of GenericXmlTreeLoader class.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class GenericXmlTreeLoaderTestsProtected : GenericXmlTreeLoader
    {
        /// <summary>
        /// Tests the LoadNodeChildren method for accuracy.
        /// LoadNodeChildren(XmlNodeList children)
        /// </summary>
        [Test]
        public void TestLoadNodeChildren()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("../../test_files/valid.xml");

            IList<TreeNode> nodes = LoadNodeChildren(doc.ChildNodes);

            //Check if LoadNodeChildren has expected results
            Assert.IsNotNull(nodes, "LoadNodeChildren returns null.");
            Assert.AreEqual(nodes.Count, 1, "LoadNodeChildren implementation is incorrect.");

            TreeNode treeNode1 = nodes[0];
            Assert.AreEqual(treeNode1.ChildCount, 4, "LoadNodeChildren implementation is incorrect.");
            Assert.AreEqual(treeNode1.Text, "Message", "LoadNodeChildren implementation is incorrect.");

            TreeNode treeNode11 = treeNode1[0];
            TreeNode treeNode12 = treeNode1[1];
            TreeNode treeNode13 = treeNode1[2];
            TreeNode treeNode14 = treeNode1[3];

            Assert.AreEqual(treeNode11.ChildCount, 0, "LoadNodeChildren implementation is incorrect.");
            Assert.AreEqual(treeNode11.Text, "Status (Pending)", "LoadNodeChildren implementation is incorrect.");

            Assert.AreEqual(treeNode12.ChildCount, 0, "LoadNodeChildren implementation is incorrect.");
            Assert.AreEqual(treeNode12.Text, "User (John)", "LoadNodeChildren implementation is incorrect.");

            Assert.AreEqual(treeNode13.ChildCount, 0, "LoadNodeChildren implementation is incorrect.");
            Assert.AreEqual(treeNode13.Text, "Security (None)", "LoadNodeChildren implementation is incorrect.");

            Assert.AreEqual(treeNode14.ChildCount, 4, "LoadNodeChildren implementation is incorrect.");
            Assert.AreEqual(treeNode14.Text, "Entries", "LoadNodeChildren implementation is incorrect.");
        }

        /// <summary>
        /// Tests the LoadNodeChildren method for failure when children is null.
        /// LoadNodeChildren(XmlNodeList children)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLoadNodeChildrenFail()
        {
            IList<TreeNode> nodes = LoadNodeChildren((XmlNodeList)null);
        }
    }
}
