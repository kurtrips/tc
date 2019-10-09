// GenericXmlTreeLoader.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;
using TopCoder.Web.UI.WebControl.TreeView;
using System.Collections.Generic;
using System.Collections;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// <para>This class implements the ITreeLoader interface from the Tree View Control component to load TreeNodes
    /// from a generic XML string.
    /// This class provides an implementation to create Tree Nodes based on attributes and names of Xml nodes read from
    /// an XML string given.</para>
    /// <para>This class isn't thread safe, as it is mutable through the Xml property, but it will be used in a
    /// thread safe manner through the XmlTreeViewControl and the ASP.NET environment.</para>
    /// </summary>
    ///
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class GenericXmlTreeLoader : ITreeLoader
    {
        /// <summary>
        /// Represents the standard format for nodes of TreeViewControl with pluggable node name and node text.
        /// </summary>
        private const string StandardNodeFormat = "{0} ({1})";

        /// <summary>
        /// This member variable holds the XML that will be loaded into a tree in the LoadTree call.
        /// This value can't be set to null or an empty string.
        /// </summary>
        private string xml;

        /// <summary>
        /// This property sets and gets the XML that this class will transform into TreeNode instances.
        /// It cannot be a null or empty, malformed XML string.
        /// </summary>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="InvalidXmlException">
        /// InvalidXmlException if the given parameter isn't well formed XML
        /// </exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <value>The XML that this class will transform into TreeNode instances.</value>
        public string Xml
        {
            get
            {
                return xml;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "Xml property of GenericXmlTreeLoader");
                HelperClass.ValidateWellFormedXml(value, "Xml property of GenericXmlTreeLoader");
                xml = value;
            }
        }

        /// <summary>
        /// Default constructor, does nothing
        /// </summary>
        public GenericXmlTreeLoader()
        {
        }

        /// <summary>
        /// This constructor overload can be used to set the xml to be used by the returning instance.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the given parameter is null</exception>
        /// <exception cref="ArgumentException">If the given parameter is an empty string</exception>
        /// <exception cref="InvalidXmlException">
        /// InvalidXmlException if the given string isn't well formed XML
        /// </exception>
        /// <param name="xml">The XML string that will be transformed by this class</param>
        public GenericXmlTreeLoader(string xml)
        {
            Xml = xml;
        }

        /// <summary>
        /// This method loads the TreeNode structure that will be displayed in the TreeViewControl, based on the XML in
        /// the xml member variable.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the Xml property is null</exception>
        /// <returns>The root TreeNode, with all children underneath it</returns>
        public TreeNode LoadTree()
        {
            if (Xml == null)
            {
                throw new InvalidOperationException("Xml property must be set before loading tree.");
            }

            //Create the XmlDocument
            XmlDocument doc=new XmlDocument();
            doc.LoadXml(Xml);

            //Create the root node
            TreeNode root = new TreeNode("root");

            //Get the root node's children
            IList<TreeNode> children = LoadNodeChildren(doc.ChildNodes);

            //Add the nodes as children of the root node
            foreach (TreeNode child in children)
            {
                child.IsExpanded = true;
                root.AddChild(child);
            }

            return root;
        }

        /// <summary>
        /// This method returns the child nodes of a specified "path", where the path is just indices of nodes that
        /// point to the node whose children are returned.
        /// </summary>
        ///
        /// <remarks>This method is not used by this component and is implemented only to meet the
        /// interface requirements set forth by the ITreeViewLoader interface.</remarks>
        ///
        /// <exception cref="ArgumentNullException">If the given parameter is null</exception>
        /// <exception cref="ArgumentException">
        /// ArgumentException if the parameter given is an empty array, if the array contains a negative value, or if no
        /// node matches the given path
        /// </exception>
        ///
        /// <param name="path">The path to the node whose children we return</param>
        ///
        /// <returns>The children of the node pointed to by the path</returns>
        public TreeNode[] LoadChildren(int[] path)
        {
            HelperClass.ValidateNotNull(path, "path");

            // Check the array length.
            if (path.Length == 0)
            {
                throw new ArgumentException("Empty array is not allowed.", "path");
            }

            //Get parent tree
            TreeNode parentNode = LoadTree();

            // Check the array element.
            foreach (int pos in path)
            {
                if (pos < 0)
                {
                    throw new ArgumentException("Negative path value is not allowed.", "path");
                }
                if (pos >= parentNode.ChildCount)
                {
                    throw new ArgumentException("Path value is not less than parentNode.ChildCount.", "path");
                }
                parentNode = parentNode[pos];
            }

            return (TreeNode[]) ((ArrayList) parentNode.Children).ToArray(typeof(TreeNode));
        }

        /// <summary>
        /// This method recursively creates TreeNodes based on the XmlNodes in the given parameter. This method
        /// looks at all Element type nodes, creating nodes for
        /// each attribute of the element, as well as for each node as a child of the Element that is only a text node.
        /// The leaf nodes are created with the following text format: "node.Name (node.Value)".
        /// Similar processing occurs for loading any children of the nodes.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <param name="children">The children Xml nodes to create TreeNodes from</param>
        /// <returns>All tree nodes created from the node list given</returns>
        protected virtual IList<TreeNode> LoadNodeChildren(XmlNodeList children)
        {
            HelperClass.ValidateNotNull(children, "children");

            //Create the result list
            IList<TreeNode> result = new List<TreeNode>();

            //Loop through all the given nodes
            foreach (XmlNode node in children)
            {
                //Only get Element nodes
                if (node.NodeType == XmlNodeType.Element)
                {
                    //Handle child text nodes for showing in [Node Name] [(Node Text)] format
                    string nodeInnerText = ExtractText(node);
                    string nodeText = node.Name;
                    if (nodeInnerText != null)
                    {
                        nodeText = String.Format(StandardNodeFormat, node.Name, nodeInnerText);
                    }

                    //Create the current TreeNode
                    TreeNode thisTreeNode = new TreeNode(nodeText);

                    //Create child TreeNode for each attribute
                    foreach (XmlAttribute attr in node.Attributes)
                    {
                        TreeNode attrNode = new TreeNode(String.Format(StandardNodeFormat, attr.Name, attr.Value));
                        attrNode.IsExpanded = true;
                        thisTreeNode.AddChild(attrNode);
                    }

                    //Recursively create hierarchy for the current node's child nodes
                    //Add this whole hierarchy as the child of the current TreeNode
                    IList<TreeNode> childTreeNodes = LoadNodeChildren(node.ChildNodes);
                    foreach (TreeNode childTreeNode in childTreeNodes)
                    {
                        childTreeNode.IsExpanded = true;
                        thisTreeNode.AddChild(childTreeNode);
                    }

                    //Add current TreeNode to the result collection
                    thisTreeNode.IsExpanded = true;
                    result.Add(thisTreeNode);
                }
            }

            return result;
        }

        /// <summary>
        /// Extracts all the inner text of a node leaving out child nodes, comments and CData section
        /// </summary>
        /// <param name="node">The node for which to extract the inner text</param>
        /// <returns>The inner text of a node leaving out child nodes, comments and CData section.
        /// If node has no inner text or entities, null is returned</returns>
        private string ExtractText(XmlNode node)
        {
            bool found = false;
            string allText = String.Empty;

            foreach (XmlNode child in node)
            {
                //Add inner text and entity references to the text to be returned
                if (child.NodeType == XmlNodeType.Text || child.NodeType == XmlNodeType.EntityReference)
                {
                    found = true;
                    allText += child.InnerText;
                }
            }
            return (found) ? allText : null;
        }

    }
}
