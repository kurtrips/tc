// CssXmlFormatter.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Text;
using System.Xml;
using TopCoder.Web.Controls.XmlViewer;

namespace TopCoder.Web.Controls.XmlViewer.Formatters
{
    /// <summary>
    /// <para>This class is an IXmlFormatter that formats the XML in a read-able way, including indentation, and it
    /// also adds CSS classes to specific elements in the XML, allowing the user to include specific class names in
    /// a CSS file that allow for customization of the display.</para>
    /// <para>The CSS class names are customizable. These can customized using the following properties.
    /// TagCSSClass: The beginning and end parts of the tag
    /// NodeNameCSSClass: The name of a node
    /// AttributeNameCSSClass: The name of an attribute.
    /// AttributeValueCSSClass: The value of an attribute
    /// InnerTextCSSClass: The inner text of a node
    /// CommentCSSClass: The comment nodes in the raw XML
    /// </para>
    /// <para>This class is no thread safe, as it is mutable.</para>
    /// <para>This class is also serializable.</para>
    /// </summary>
    ///
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    ///
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class CssXmlFormatter : IXmlFormatter
    {
        /// <summary>
        /// Represents a span element with a pluggable css class name and element value.
        /// </summary>
        private const string SpanGeneral = "<span class='{0}'>{1}</span>";

        /// <summary>
        /// Represents html markup for a line break.
        /// </summary>
        private const string HtmlLineBreak = "<br>";

        /// <summary>
        /// Represents an equal sign surrounded by a space on either side.
        /// </summary>
        private const string EqualSign = " = ";

        /// <summary>
        /// Represents Html markup for an opening tag.
        /// </summary>
        private const string HtmlTagOpen = "&lt;";

        /// <summary>
        /// Represents Html markup for an closing tag.
        /// </summary>
        private const string HtmlTagClose = "&gt;";

        /// <summary>
        /// Represents html markup for
        /// </summary>
        private const string HtmlSpace = "&nbsp;";

        /// <summary>
        /// Represents a forward slash.
        /// </summary>
        private const string ForwardSlash = "/";

        /// <summary>
        /// The CSS class used to modify the look of tags in the formatted XML. This value's default is "tag". It
        /// can't be set to null or an empty string, is modified through the "TagCSSClass" property.
        /// </summary>
        private string tagCSSClass = "tag";

        /// <summary>
        /// The CSS class used to modify the look of node names in the formatted XML. This value's default is
        /// "node_name". It can't be set to null or an empty string, is modified through the "NodeNameCSSClass"
        /// property.
        /// </summary>
        private string nodeNameCSSClass = "node_name";

        /// <summary>
        /// The CSS class used to modify the look of attribute names in the formatted XML. This value's default is
        /// "attribute_name". It can't be set to null or an empty string, is modified through the "AttributeName"
        /// property.
        /// </summary>
        private string attributeNameCSSClass = "attribute_name";

        /// <summary>
        /// The CSS class used to modify the look of attribute values in the formatted XML.  This value's default is
        /// "attribute_value". It can't be set to null or an empty string, is modified through the
        /// "AttributeValueCSSClass" property.
        /// </summary>
        private string attributeValueCSSClass = "attribute_value";

        /// <summary>
        /// The CSS class used to modify the look of inner text in the formatted XML. This value's default is
        /// "inner_text". It can't be set to null or an empty string, is modified through the "InnerTextCSSClass"
        /// property.
        /// </summary>
        private string innerTextCSSClass = "inner_text";

        /// <summary>
        /// The CSS class used to modify the look of comments in the formatted XML. This value's default is "comment".
        /// It can't be set to null or an empty string, is modified through the "CommentCSSClass" property.
        /// </summary>
        private string commentCSSClass = "comment";

        /// <summary>
        /// This value indicates how much space is added for indentation when formatting the XML. The default value is
        /// two, so sub-nodes of a given node will be indented by two spaces more than their parent. This value can't
        /// be less than 0 and is modified through the Indentation property.
        /// </summary>
        private int indentation = 2;

        /// <summary>The CSS class used to modify the look of tags in the formatted XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <value>The CSS class used to modify the look of tags in the formatted XML.</value>
        public string TagCSSClass
        {
            get
            {
                return tagCSSClass;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "TagCSSClass");
                tagCSSClass = value;
            }
        }

        /// <summary>The CSS class used to modify the look of node names in the formatted XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <value>The CSS class used to modify the look of node names in the formatted XML.</value>
        public string NodeNameCSSClass
        {
            get
            {
                return nodeNameCSSClass;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "NodeNameCSSClass");
                nodeNameCSSClass = value;
            }
        }

        /// <summary>The CSS class used to modify the look of attribute names in the formatted XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <value>The CSS class used to modify the look of attribute names in the formatted XML.</value>
        public string AttributeNameCSSClass
        {
            get
            {
                return attributeNameCSSClass;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "AttributeNameCSSClass");
                attributeNameCSSClass = value;
            }
        }

        /// <summary>The CSS class used to modify the look of attribute values in the formatted XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <value>The CSS class used to modify the look of attribute values in the formatted XML.</value>
        public string AttributeValueCSSClass
        {
            get
            {
                return attributeValueCSSClass;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "AttributeValueCSSClass");
                attributeValueCSSClass = value;
            }
        }

        /// <summary>The CSS class used to modify the look of inner text in the formatted XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <value>The CSS class used to modify the look of inner text in the formatted XML.</value>
        public string InnerTextCSSClass
        {
            get
            {
                return innerTextCSSClass;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "InnerTextCSSClass");
                innerTextCSSClass = value;
            }
        }

        /// <summary>The CSS class used to modify the look of comments in the formatted XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is an empty string</exception>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given value is null</exception>
        /// <value>The CSS class used to modify the look of comments in the formatted XML.</value>
        public string CommentCSSClass
        {
            get
            {
                return commentCSSClass;
            }
            set
            {
                HelperClass.ValidateNotNullNotEmpty(value, "CommentCSSClass");
                commentCSSClass = value;
            }
        }

        /// <summary>This value indicates how much space is added for indentation when formatting the XML.</summary>
        /// <exception cref="ArgumentException">ArgumentException if the given value is less than 0</exception>
        /// <value>This value indicates how much space is added for indentation when formatting the XML.</value>
        public int Indentation
        {
            get
            {
                return indentation;
            }
            set
            {
                HelperClass.ValidateNonNegative(value, "Indentation");
                indentation = value;
            }
        }

        /// <summary>
        /// Default no operation constructor.
        /// </summary>
        public CssXmlFormatter()
        {
        }

        /// <summary>
        /// This method formats the given XML string with proper indentation and CSS.
        /// </summary>
        /// <exception cref="ArgumentNullException">ArgumentNullException if the given parameter is null</exception>
        /// <exception cref="ArgumentException">ArgumentException if the given parameter is an empty string</exception>
        /// <exception cref="InvalidXmlException">
        /// InvalidXmlException if the XML string given isn't well formed XML
        /// </exception>
        /// <param name="xml">The XML to format</param>
        /// <returns>The formatted XML string</returns>
        public string Format(string xml)
        {
            HelperClass.ValidateNotNullNotEmpty(xml, "xml");

            //Load the document
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch (Exception e)
            {
                throw new InvalidXmlException("Xml string is malformed.", e);
            }

            string formatted = String.Empty;
            foreach (XmlNode node in doc.ChildNodes)
            {
                formatted += FormatNode(node, 0);
            }
            return formatted;
            //return FormatNode(doc.DocumentElement, 0);
        }

        /// <summary>
        /// <para>This method formats the given XmlNode with CSS and proper indentation. This is achieved by
        /// putting "span" tags with CSS class names around the various XML Node types
        /// The indentation value given indicates how far indented the tag is in comparison to the root node.
        /// </para>
        /// <para>
        /// The following node types are supported:
        /// XmlDeclaration
        /// ProcessingInstruction
        /// Entity
        /// DocumentType
        /// Comment
        /// CDATA
        /// Element
        /// Text (Inside Element types)
        /// EntityReference (Inside Element types)
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">If node is null</exception>
        /// <exception cref="ArgumentException">If indent is less than 0</exception>
        /// <param name="node">The node to format</param>
        /// <param name="indent">The indent level of the node</param>
        /// <returns>An HTML string representing the node and its children, after formatting</returns>
        protected virtual string FormatNode(XmlNode node, int indent)
        {
            HelperClass.ValidateNotNull(node, "node");
            HelperClass.ValidateNonNegative(indent, "indent");

            //Usage of StringBuilder is more efficient than string
            //when variable number of concatenations are performed.
            StringBuilder result = new StringBuilder();

            switch (node.NodeType)
            {
                case XmlNodeType.XmlDeclaration:
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.Entity:
                case XmlNodeType.DocumentType:
                case XmlNodeType.Comment:
                case XmlNodeType.CDATA:
                {
                    string nonElementNode = node.OuterXml.Replace("<", HtmlTagOpen).Replace(">", HtmlTagClose);
                    result.Append(String.Format(SpanGeneral, CommentCSSClass, nonElementNode));
                    result.Append(AddLineBreakAndIndent(indent, true));
                    break;
                }
                case XmlNodeType.Element:
                {
                    result.Append(FormatElementNode(node, indent));
                    break;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Returns a string containing an optional line break and requisite number of html space characters.
        /// </summary>
        /// <param name="indent">A value indicative of the number of spaces to be added.</param>
        /// <param name="lineBreak">Whether to add a line break to the return string.</param>
        /// <returns>Returns a string containing requisite number of html space characters.</returns>
        private string AddLineBreakAndIndent(int indent, bool lineBreak)
        {
            string res = String.Empty;

            //Add a line break if needed
            if (lineBreak)
            {
                res += HtmlLineBreak;
            }

            string indentBlock = String.Empty;
            //Form the indentBlock
            for (int i = 0; i < indentation; i++)
            {
                indentBlock += HtmlSpace;
            }

            //Add the indentation spaces
            for (int i = 0; i < indent; i++)
            {
                res += indentBlock;
            }

            //Add one more space, so that the xml is a little to the right of the
            //left border of the control.
            res += HtmlSpace;

            return res;
        }

        /// <summary>
        /// This method formats the given XmlNode of type Element with CSS and proper indentation. This is achieved by
        /// putting "span" tags with CSS class names around the XML tags,
        /// the node names, the attribute names and values, the inner text and comments.
        /// The indentation value given indicates how far indented the tag is in comparison to the root node.
        /// This method recursively calls itself to format child nodes of the node given.
        /// </summary>
        /// <exception cref="ArgumentNullException">If node is null</exception>
        /// <exception cref="ArgumentException">If indent is less than 0</exception>
        /// <param name="node">The node to format</param>
        /// <param name="indent">The indent level of the node</param>
        /// <returns>An HTML string representing the node and its children, after formatting</returns>
        private string FormatElementNode(XmlNode node, int indent)
        {
            HelperClass.ValidateNotNull(node, "node");
            HelperClass.ValidateNonNegative(indent, "indent");

            //Usage of StringBuilder is more efficient than string
            //when variable number of concatenations are performed.
            StringBuilder result = new StringBuilder();

            //Add the indentation spaces
            result.Append(AddLineBreakAndIndent(indent, false));

            //Add the CSS to the open tag and the node name
            result.Append(String.Format(SpanGeneral, TagCSSClass, HtmlTagOpen) +
                          String.Format(SpanGeneral, NodeNameCSSClass, node.Name));

            //Add the attributes
            foreach (XmlAttribute attr in node.Attributes)
            {
                //Each attribute is on a different line
                //Indent the attributes at the same level as the parent node.
                result.Append(AddLineBreakAndIndent(indent, true));

                //CSS for the attribute name and value
                result.Append(String.Format(SpanGeneral, AttributeNameCSSClass, attr.Name) + EqualSign +
                              String.Format(SpanGeneral, AttributeValueCSSClass, attr.Value));
            }

            //Add the close of the initial tag
            result.Append(String.Format(SpanGeneral, TagCSSClass, HtmlTagClose));

            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.NodeType)
                {
                    //Handle child nodes of type Element
                    //Recursively add the child nodes at an indentation level more than the current node
                    case XmlNodeType.Element:
                    {
                        result.Append(HtmlLineBreak);
                        result.Append(FormatElementNode(child, indent + 1));
                        break;
                    }
                    //Handle child nodes of type Text
                    case XmlNodeType.Text:
                    case XmlNodeType.EntityReference:
                    {
                        result.Append(AddLineBreakAndIndent(indent + 1, true));
                        result.Append(String.Format(SpanGeneral, InnerTextCSSClass, child.InnerText));
                        break;
                    }
                    //Handle child nodes of other types like Comment, CDATA.
                    case XmlNodeType.Comment:
                    case XmlNodeType.CDATA:
                    {
                        result.Append(AddLineBreakAndIndent(indent + 1, true));
                        string htmlComment = child.OuterXml.Replace("<", HtmlTagOpen).Replace(">", HtmlTagClose);
                        result.Append(String.Format(SpanGeneral, CommentCSSClass, htmlComment));
                        break;
                    }
                }
            }

            //Add the node closing tag
            result.Append(AddLineBreakAndIndent(indent, true));
            result.Append(String.Format(SpanGeneral, TagCSSClass, HtmlTagOpen + ForwardSlash) +
                          String.Format(SpanGeneral, NodeNameCSSClass, node.Name) +
                          String.Format(SpanGeneral, TagCSSClass, HtmlTagClose));

            return result.ToString();
        }
    }
}
