/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;

namespace TopCoder.Security.Cryptography.Mobile.Canonicalizers
{
    /// <summary>
    /// <p>This class implements the ICanonicalizer and ITransformer interface contract using Exclusive XML
    /// Canonicalization. XML documents are usually lexically loose meaning that the same document in terms
    /// of data can actually be written with many different sets of bytes even when using the same encoding.
    /// For example extra white spaces could be present in one and not the other and of course the order of
    /// attributes or even tags could be different. This implementation will provide a normalized lexical form
    /// for XML where all of the allowed variations have been removed, and strict rules are imposed to allow
    /// consistent byte-by-byte comparison.</p>
    /// <p><b>Thread Safety: </b>This is a thread-safe implementation, as it has no mutable state.</p>
    /// </summary>
    /// <remarks>
    /// <p>Because of design flaw, both the ICanonicalizer and ITransformer interface have to be implemented
    /// in the same class.</p>
    /// <p>Currently, no support for ProcessingInstruction tags and external Entity References.</p>
    /// </remarks>
    /// <author>count</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class XmlDsigExcCanonicalizer : ICanonicalizer, ITransformer
    {
        /// <summary>
        /// Represents the canonicalization exception message
        /// </summary>
        private const string CANON_EX_MSG =
            "Exclusive Xml Canonicalization was unsuccessful. See inner exception for details";

        /// <summary>
        /// <p>Represents the encoding used by this format. This is initialized statically and never changed. It is read
        /// through the Encoding property getter. </p>
        /// </summary>
        private readonly Encoding encoding = new UTF8Encoding();

        /// <summary>
        /// <p>Represents the encoding used for the canonicalized form.</p>
        /// </summary>
        /// <value>The encoding to use for canonicalization process</value>
        public Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }

        /// <summary>
        /// <p>a default no-op constructor</p>
        /// </summary>
        public XmlDsigExcCanonicalizer()
        {
        }

        /// <summary>
        /// <p>This method will accept a text string and will modify (if necessary) the string data to bring this
        /// string into a canonical form using Exclusive XML Canonicalizer algorithm, details of which can be
        /// found here: http://www.w3.org/2001/10/xml-exc-c14n# </p>
        /// </summary>
        /// <param name="text">text that will be canonicalized</param>
        /// <returns>canonicalized text</returns>
        /// <exception cref="CanonicalizationException">
        /// If text is not valid xml.
        /// If any problem was encountered during canonicalization.
        /// </exception>
        /// <exception cref="ArgumentNullException">If parameter is null</exception>
        public string BringToCanonicalForm(string text)
        {
            ExceptionHelper.ValidateStringNotNullNotEmpty(text, "text");
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(text);
                return XmlDsigExcC14NTransform(doc);
            }
            catch (Exception ex)
            {
                throw new CanonicalizationException(CANON_EX_MSG, ex);
            }
        }

        /// <summary>
        /// This method transforms the input data according Exclusive XML Canonicalization specifications.
        /// </summary>
        /// <param name="data">data to be transformed, in UTF8.</param>
        /// <returns>Transformed data, in UTF8.</returns>
        public byte[] Transform(byte[] data)
        {
            ExceptionHelper.ValidateNotNull(data, "data");
            try
            {
                string s = encoding.GetString(data, 0, data.Length);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(s);
                string result = XmlDsigExcC14NTransform(doc);
                return encoding.GetBytes(result);
            }
            catch (Exception ex)
            {
                throw new TransformerException(CANON_EX_MSG, ex);
            }
        }

        /// <summary>
        /// Canonicalizes the document.
        /// </summary>
        /// <param name="doc">The xml document containing the xml to canonicalize</param>
        /// <returns>The canonicalized xml</returns>
        private string XmlDsigExcC14NTransform(XmlDocument doc)
        {
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            using (Stream stream = new MemoryStream())
            {
                TextWriter writer = new StreamWriter(stream);
                XmlDsigExcC14NTransform(doc.DocumentElement, nsMgr, writer);
                writer.Flush();
                stream.Position = 0;
                TextReader reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Do the canonicalization.
        /// </summary>
        /// <param name="node">The xml node containing the xml to canonicalize</param>
        /// <param name="nsMgr">The namespace manager to help process namespaces</param>.
        /// <param name="txtWriter">TextWriter which can be used to write data.</param>
        /// <remarks>
        /// Currently, no support for ProcessingInstruction tags and external Entity References.
        /// </remarks>
        private void XmlDsigExcC14NTransform(XmlNode node, XmlNamespaceManager nsMgr, TextWriter txtWriter)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Element:
                    txtWriter.Write(string.Format("<{0}", node.Name));

                    nsMgr.PushScope();

                    List<AttributeNode> attrList = new List<AttributeNode>();
                    if (nsMgr.LookupNamespace(node.Prefix) != node.NamespaceURI)
                    {
                        nsMgr.AddNamespace(node.Prefix, node.NamespaceURI);
                        // node.Prefix may be "" here in case of default namespace
                        attrList.Add(new AttributeNode("xmlns", node.Prefix, string.Empty,
                            parseAttrValue(node.NamespaceURI)));
                    }

                    if (node.Attributes.Count > 0)
                    {
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            if (attr.Name != "xmlns" && attr.Prefix != "xmlns")
                            {
                                // NamespaceURI is "" if Prefix is ""
                                // attr.NamespaceURI is not to be process by parseAttrValue, it determines the order
                                attrList.Add(new AttributeNode(attr.Prefix, attr.LocalName,
                                    attr.NamespaceURI, parseAttrValue(attr.Value)));
                                if (attr.Prefix != string.Empty
                                    && nsMgr.LookupNamespace(attr.Prefix) != attr.NamespaceURI)
                                {
                                    nsMgr.AddNamespace(attr.Prefix, attr.NamespaceURI);
                                    attrList.Add(new AttributeNode("xmlns", attr.Prefix, string.Empty,
                                        parseAttrValue(attr.NamespaceURI)));
                                }
                            }
                        }
                    }

                    attrList.Sort();

                    foreach (AttributeNode attr in attrList)
                    {
                        txtWriter.Write(attr.ToString());
                    }
                    txtWriter.Write(">");

                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        XmlDsigExcC14NTransform(childNode, nsMgr, txtWriter);
                    }

                    nsMgr.PopScope();

                    txtWriter.Write(string.Format("</{0}>", node.Name));
                    break;

                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.EntityReference:
                    txtWriter.Write(parseTextNode(node.InnerText));
                    break;
            }
        }

        /// <summary>
        /// Parses the Text node and modify it if necessary.
        /// </summary>
        /// <param name="text">Text to parse.</param>
        /// <returns>Parsed text.</returns>
        private string parseTextNode(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in text)
            {
                switch (ch)
                {
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '\r':
                        sb.Append("&#xD;");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parses the value of Attribute node and modify it if necessary.
        /// </summary>
        /// <param name="value">Value to parse.</param>
        /// <returns>Parsed value.</returns>
        private string parseAttrValue(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in value)
            {
                switch (ch)
                {
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '\"':
                        sb.Append("&quot;");
                        break;
                    case '\r':
                        sb.Append("&#xD;");
                        break;
                    case '\n':
                        sb.Append("&#xA;");
                        break;
                    case '\t':
                        sb.Append("&#x9;");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// This is a custom attribute class to hold Attribute and Namespace nodes of a element and
    /// provides comparability which can make much convenience when reordering these nodes.
    /// </summary>
    internal class AttributeNode : IComparable
    {
        /// <summary>
        /// Holds related information of a attribute,
        /// namespaceURI is used only in comparison to determine the order
        /// </summary>
        private string prefix, localName, namespaceURI, value;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Prefix">Prefix of the attribute.</param>
        /// <param name="LocalName">Local name of the attribute.</param>
        /// <param name="NamespaceURI">Namespace URI of the attribute.</param>
        /// <param name="Value">Value of the attribute.</param>
        public AttributeNode(string Prefix, string LocalName, string NamespaceURI, string Value)
        {
            prefix = Prefix; localName = LocalName;
            namespaceURI = NamespaceURI; value = Value;
        }

        /// <summary>
        /// Implements the ICompareTo(object) interface to provide comparability.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>
        /// The result of comparison.
        /// Less than zero if this instance is less than obj.
        /// Zero if this instance is equal to obj.
        /// Greater than zero if this instance is greater than obj.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj is AttributeNode)
            {
                AttributeNode node = (AttributeNode)obj;
                if (prefix == "xmlns" && node.prefix == "xmlns")
                {
                    return localName.CompareTo(node.localName);
                }
                else if (prefix == "xmlns")
                {
                    return -1;
                }
                else if (node.prefix == "xmlns")
                {
                    return 1;
                }
                else
                {
                    int ret = namespaceURI.CompareTo(node.namespaceURI);
                    return ret != 0 ? ret : localName.CompareTo(node.localName);
                }
            }
            else
            {
                throw new ArgumentException("Object is not a XmlNode.");
            }
        }

        /// <summary>
        /// Overrides the ToString() method to get the string representation of the attribute.
        /// </summary>
        /// <returns>The string representation of the attribute.</returns>
        public override string ToString()
        {
            if (localName == string.Empty || prefix == string.Empty)
            {
                // former for default namespace, latter for unqualified attributes
                return string.Format(" {0}=\"{1}\"", prefix + localName, value);
            }
            else
            {
                return string.Format(" {0}:{1}=\"{2}\"", prefix, localName, value);
            }
        }
    }
}
