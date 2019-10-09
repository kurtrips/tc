// SoapMessageReferenceLoader.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Web.Services.Protocols;
using System.IO;
using System.Text;
using System.Xml;
using TopCoder.Security.Cryptography.Mobile;

namespace TopCoder.Security.Cryptography.Mobile.ReferenceLoaders
{
    /// <summary>
    /// <strong>Purpose:</strong> <p>This is a reference loader which will load data from a Soap Envelope (i.e. xml) The
    /// expectation of the URI is that it will be of the #reference format.</p>
    /// <p><b>Thread Safety: </b>This implementation is not thread-safe. Calling function must lock
    /// the LoadReferenceData method to ensure thread safety.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SoapMessageReferenceLoader : IReferenceLoader
    {
        /// <summary>
        /// Reference Loading exception message
        /// </summary>
        private string REF_LOAD_EX_MSG =
            "Specified reference could not be loaded. Please see inner exception for details.";

        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This represents the soap message where we will be looking for the
        /// actual reference data. This is initialized in the constructor, cannot be null.</p> <p>This represents the
        /// document where the URI in the LoadReferenceData method will be localizing its search.</p>
        /// </summary>
        private SoapMessage soapMessage;

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>This is a simple constructor which will assign the parameter to the
        /// corresponding member variable.</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">if the input parameter is null</exception>
        /// <param name="soapmessage">soap message which will be our source document</param>
        public SoapMessageReferenceLoader(SoapMessage soapmessage)
        {
            ExceptionHelper.ValidateNotNull(soapmessage, "soapmessage");
            this.soapMessage = soapmessage;
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>Loads the specified xml element with id as uriString.
        /// and returns the byte array of that element.</p>
        /// </summary>
        /// <param name="uriString">local soap message uri string</param>
        /// <returns>the resource as an array of bytes</returns>
        /// <exception cref="ReferenceLoadingException">
        /// If there are issues encountered during the loading. This could be due to IO for example.
        /// </exception>
        /// <exception cref="ArgumentNullException">If input parameter is null</exception>
        public byte[] LoadReferenceData(string uriString)
        {
            ExceptionHelper.ValidateNotNull(uriString, "uriString");

            try
            {
                XmlDocument doc = new XmlDocument();
                //Get stream from SoapMessage
                Stream stream = soapMessage.Stream;
                stream.Position = 0;
                //Create XmlTextReader from stream
                XmlTextReader reader = new XmlTextReader(stream);
                //Load stream into xml
                doc.Load(reader);

                //Remove # from uriString and find element with that id
                uriString = uriString.Remove(0, 1);

                //Find Node with given id
                XmlNode nodeFound = null;
                FindNodeWithId((XmlNode)(doc.DocumentElement), uriString, ref nodeFound);

                if (nodeFound == null)
                {
                    throw new ReferenceLoadingException("No element with id " +
                                uriString + " was found in soap message");
                }

                return Encoding.UTF8.GetBytes(nodeFound.OuterXml);
            }
            catch (ReferenceLoadingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ReferenceLoadingException(REF_LOAD_EX_MSG, ex);
            }
        }

        /// <summary>
        /// This function is a replacement for GetElementById function which is not
        /// present in the XmlDocument class of the Mobile System.Xml namespace.
        /// This is a recursive function which keeps going deeper into the xml tree
        /// and returns immediately if any node with the specified id is found.
        /// </summary>
        /// <param name="element">The element in which to find the attribute.</param>
        /// <param name="id">The id to search for</param>
        /// <param name="foundNode">If found, this parameter holds the value of XmlNode after return</param>
        private void FindNodeWithId(XmlNode element, string id, ref XmlNode foundNode)
        {
            //Return if we have already found the node
            if (foundNode != null)
            {
                return;
            }

            //For the current element, check all attributes for the Id attribute
            foreach (XmlNode node in element.Attributes)
            {
                if (node.LocalName == "Id" && node.Value == id)
                {
                    foundNode = element;
                }
            }

            //Check all child nodes of the current element
            foreach (XmlNode child in element.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                {
                    FindNodeWithId(child, id, ref foundNode);
                }
            }
        }
    }
}
