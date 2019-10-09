// XmlTreeViewType.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// <para>This enum holds the different supported view types for the tree view control: Tree view and Raw view.
    /// Use the values in this enum to set the XmlTreeViewControl.ViewType property to change the
    /// displayed view of the control.</para>
    /// <para>Since this is an enumeration, it is thread safe.</para>
    /// </summary>
    ///
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public enum XmlTreeViewType
    {
        /// <summary>This enum value represents the raw XML view of the XmlTreeViewControl</summary>
        RawXml,

        /// <summary>This enum value represents the tree view of the XmlTreeViewControl</summary>
        TreeView
    }
}
