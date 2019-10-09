// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// Unit tests for the XmlTreeViewType enumeration.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DefaultXmlTreeViewTypeTests
    {
        /// <summary>
        /// Tests the XmlTreeViewType enumeration for accuracy.
        /// </summary>
        [Test]
        public void XmlTreeViewTypeAccuracy()
        {
            Assert.IsTrue(Enum.IsDefined(typeof(XmlTreeViewType), 0));
            Assert.IsTrue(Enum.IsDefined(typeof(XmlTreeViewType), "RawXml"));
            Assert.IsTrue(Enum.IsDefined(typeof(XmlTreeViewType), 1));
            Assert.IsTrue(Enum.IsDefined(typeof(XmlTreeViewType), "TreeView"));

            Assert.IsFalse(Enum.IsDefined(typeof(XmlTreeViewType), "RawTree"));
            Assert.IsFalse(Enum.IsDefined(typeof(XmlTreeViewType), 2));
        }
    }
}
