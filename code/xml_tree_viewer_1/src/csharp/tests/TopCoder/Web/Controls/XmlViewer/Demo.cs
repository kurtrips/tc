// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using TopCoder.Web.Controls.XmlViewer.Formatters;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Provides a demonstration of the intended usage of this component.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class Demo
    {
        /// <summary>
        /// Demo.
        /// Since this is a web tool, this demo only shows how to configure the Control properly.
        /// </summary>
        [Test]
        public void Demonstartion()
        {
            //The following lines demonstrates creating the control dynamically in code.
            //For information on how to embed this control in a page at design time see CS Section 4.

            //Create control
            XmlTreeViewControl firstXTVC = new XmlTreeViewControl();

            //Set some xml
            string xml = File.ReadAllText("../../test_files/valid.xml");
            firstXTVC.Xml = xml;

            //Set the properties of the XmlTreeViewControl
            firstXTVC.FolderIconUrl = "folder.png";
            firstXTVC.ExpandingCssClass = "expanding";
            firstXTVC.ExpandingFailureText = "Failed";
            firstXTVC.ExpandingIconUrl = "folder_go.png";
            firstXTVC.ExpandingText = "Expanding...";
            firstXTVC.FolderIconUrl = "folder.png";
            firstXTVC.LeafIconUrl = "bullet_green.png";
            firstXTVC.SaveFileName = "XmlTree.xml";
            firstXTVC.SaveButtonIconUrl = "disk.png";
            firstXTVC.Prefixes = new string[] { "&nbsp;&nbsp;",
                                                "&nbsp;&nbsp;&nbsp;&nbsp;",
                                                "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" };

            //Specify formatter to use and set it's properties
            CssXmlFormatter formatter = new CssXmlFormatter();
            formatter.AttributeNameCSSClass = "myANClass";
            formatter.AttributeValueCSSClass = "myAVClass";
            formatter.CommentCSSClass = "myCMClass";
            formatter.Indentation = 4;
            formatter.InnerTextCSSClass = "myITClass";
            formatter.NodeNameCSSClass = "myNNClass";
            formatter.TagCSSClass = "myTGClass";
            firstXTVC.Formatter = formatter;
        }
    }
}
