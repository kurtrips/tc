// StandardFormCanonicalizer.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Text;
using System.Xml;

namespace TopCoder.Security.Cryptography.Mobile.Canonicalizers
{
    /// <summary>
    /// <p>This is a simple, UTF-8 based, implementation of the ICanonicalizer interface contract. XML documents are
    /// usually lexically loose meaning that the same document in terms of data can actually be written with many
    /// different sets of bytes even when using the same encoding. For example extra white spaces could be present in
    /// one and not the other and of course the order of attributes or even tags could be different. This implementation
    /// will provide a normalized lexical form for XML where all of the allowed variations have been removed, and strict
    /// rules are imposed to allow consistent byte-by-byte comparison.</p>
    /// <p><b>Thread Safety: </b>This is a thread-safe implementation, as it has no mutable state.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class StandardFormCanonicalizer : ICanonicalizer
    {
        /// <summary>
        /// Represents the canonicalization exception message
        /// </summary>
        private const string CANON_EX_MSG = "Xml Canonicalization was unsuccessful. See inner exception for details";
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
        public  StandardFormCanonicalizer()
        {
        }

        /// <summary>
        /// <p>This method will accept a text string and will modify (if necessary) the string data to bring this string
        /// into a canonical  form. Here we will implement the specific cacnonical form for xml descrobed  here:
        /// http://www.w3.org/TR/2001/REC-xml-c14n-20010315 </p>
        /// <para>The steps involved in canonicalization are:</para>
        /// 1.The document is encoded in UTF-8
        /// 2.Line breaks normalized to #xA on input, before parsing
        /// 3.Attribute values are normalized, as if by a validating processor
        /// 4.Character and parsed entity references are replaced
        /// 5.CDATA sections are replaced with their character content
        /// 6.The XML declaration and document type declaration (DTD) are removed
        /// 7.Empty elements are converted to start-end tag pairs
        /// 8.Whitespace outside of the document element and within start and end tags is normalized
        /// 9.All whitespace in character content is retained (excluding characters removed during line
        /// feed normalization)
        /// 10.Attribute value delimiters are set to quotation marks (double quotes)
        /// 11.Special characters in attribute values and character content are replaced by character references
        /// 12.Superfluous namespace declarations are removed from each element
        /// 13.Default attributes are added to each element
        /// 14.Lexicographic order is imposed on the namespace declarations and attributes of each element
        /// </summary>
        /// <param name="text">text that will be canonicalized</param>
        /// <returns>canonicalized text</returns>
        /// <exception cref="CanonicalizationException">
        /// If text is not valid xml.
        /// If text does not represent valid SignedInfo node.
        /// If any problem was encountered during canonicalization.
        /// </exception>
        /// <exception cref="ArgumentNullException">If parameter is null</exception>
        ///
        /// <remarks>As we need to canonicalize only the SignedInfo node created by our component
        /// itself, most of the points mentioned above are already taken care of.
        /// Thus only points 1,2,3,7 are implemented.</remarks>
        public string BringToCanonicalForm(string text)
        {
            ExceptionHelper.ValidateNotNull(text, "text");
            try
            {
                //Line breaks are normalized to "#xA" on input
                text = text.Replace("\r\n", "\n");
                text = text.Replace("\r", "\n");

                //Encoding to UTF8 is automatically done by loading the string into xml document
                XmlDocument input = new XmlDocument();
                input.LoadXml(text);

                //Canonicalize the input and save result in output
                string output = DoCanonicalize(input);

                return output;
            }
            catch (Exception ex)
            {
                throw new CanonicalizationException(CANON_EX_MSG, ex);
            }
        }

        /// <summary>
        /// Canonicalizes the element
        /// </summary>
        /// <param name="doc">The xml document containing the xml to canonicalize</param>
        /// <returns>The canonicalized xml</returns>
        private string DoCanonicalize(XmlDocument doc)
        {
            //Empty elements are converted to start-end tag pairs
            XmlElement signMethod = (XmlElement) doc.SelectSingleNode("SignedInfo/SignatureMethod");
            signMethod.IsEmpty = false;
            doc.DocumentElement.ReplaceChild((XmlNode) signMethod ,
                doc.SelectSingleNode("SignedInfo/SignatureMethod"));

            //Empty elements are converted to start-end tag pairs
            XmlElement canonMethod = (XmlElement)doc.SelectSingleNode("SignedInfo/CanonicalizationMethod");
            canonMethod.IsEmpty = false;
            doc.DocumentElement.ReplaceChild((XmlNode) canonMethod,
                doc.SelectSingleNode("SignedInfo/CanonicalizationMethod"));

            //Rest of conditions for canonicalized xml are already met.
            return doc.DocumentElement.OuterXml;
        }

    }
}
