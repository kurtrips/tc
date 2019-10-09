// SignatureManager.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Web.Services.Protocols;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is the main manager of this component. It is responsible for creating a Digital (xml based) signature as
    /// well as being able to verify digital signatures. For signature creation it coordinates the work of reference
    /// loaders to load references, transformers to transform each loaded reference, digesters to provide digests for
    /// each loaded reference, canonicalizers to bring the data to a standardized canonical form, digester to provide a
    /// digest of the canonical form, signer which will sign the resulting canonical form. The result of this process 
    /// will be an xml-based signed element, which will be a digital signature of the data provided by the references. 
    /// It will also be responsible for verification of a digital signature (an inverse process to a great extent of 
    /// signing) so that when given an input consisting of an XML node (or an xml string) it can verify the signature.
    /// This component uses the ProcessRegistry to fetch all the necessary processing elements (like a digester or 
    /// a transformer) for added flexibility.</p>
    /// <p><b>Thread Safety:</b>
    /// This implementation is thread safe as it acts mostly as a utility and all state is 
    /// invoked in a thread-safe manner.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SignatureManager
    {
        /// <summary>
        /// Represents the error message for a verification exception
        /// </summary>
        private const string VERIF_EXCP_MSG =
            "Could not complete verification process. Please see inner exception for details";

        /// <summary>
        /// The error message if verification of a reference node fails
        /// </summary>
        private const string REF_VERIF_FAILED =
            "Verification of reference failed. Either the reference itself has changed or its digest is tampered";
        
        /// <summary>
        /// Represents the default XML digital signature namespace
        /// </summary>
        private const string DEF_XMLDSIG_NS = "http://www.w3.org/2000/09/xmldsig#";

        /// <summary>
        /// SignatureManager Exception error message.
        /// </summary>
        private const string SIGN_MAN_EXCP_MSG = "Signing process failed. Please see inner exception for more details";

        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This rrepresents the default configuration namespace to be used when
        /// instantiating and initializing an instance of registry.</p>
        /// </summary>
        private static string DefaultNamespace = "TopCoder.Security.Cryptography.Mobile";

        /// <summary>
        /// Represents an empty dictionary and is used as parameters for registry.GetXXXInstance functions
        /// </summary>
        private Dictionary<string, object> emptyDic = new Dictionary<string, object>();

        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This is a registry of the diferent elements that we could encounter
        /// when dealing with signatures. It will be used as a lookup for the different pieces such as transformers or
        /// verifiers. This registry is set in any of the 3 constructors. Can not be null. Once set it is immutable and
        /// is invisible to the outside world. The registry must at least contain entries for a Digester, Signer,
        /// Canonicalizer, and Reference Loader.</p>
        /// </summary>
        private ProcessRegistry registry;

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>Creates a new instance of the manager.
        /// Initializes registry using Default namespace</p>
        /// </summary>
        public SignatureManager() : this(DefaultNamespace)
        {
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>Creates a new instance of the registry.
        /// Initializes registry using passed namespace</p>
        /// </summary>
        /// <param name="aNamespace">configuration namespace</param>
        /// <exception cref="ArgumentNullException">If aNamespace is null</exception>
        /// <exception cref="ArgumentException">If aNamespace is empty</exception>
        public SignatureManager(string aNamespace)
        {
            registry = new ProcessRegistry(aNamespace);
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p> <p>Creates a new instance initialized with the instance of the
        /// registry.</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">if parameter is null</exception>
        /// <param name="registry">registry of different working pieces</param>
        public SignatureManager(ProcessRegistry registry)
        {
            ExceptionHelper.ValidateNotNull(registry, "registry");
            this.registry = registry;
        }

        /// <summary>
        /// <p><strong>Purpose:</strong></p>  <p>This method processes the references and creates the digital signature
        /// of the contents, and returns the whole XML Node of the signed contents. It produces an XmlNode
        /// of type signature.</p>
        /// <p>Note: Current implementation does not create a KeyInfoNode</p>
        /// </summary>
        /// <param name="references">references that we will be signing</param>
        /// <param name="canonicalizer">the specific canonicalizer to use</param>
        /// <param name="signer">the specific signer to use</param>
        /// <param name="signID">Signature id</param>
        /// <returns>Xml Node with digitally signed representation of the data</returns>
        /// <exception cref="ArgumentNullException">if any element is null</exception>
        /// <exception cref="SignatureManagerException">any internal exception thrown in this function
        /// is wrapped up as SignatureManagerException</exception>
        public XmlNode Sign(IList<IReference> references, InstantiationVO canonicalizer, InstantiationVO signer,
            string signID)
        {
            //Validate not null
            ExceptionHelper.ValidateNotNull(references, "references");
            ExceptionHelper.ValidateNotNull(canonicalizer, "canonicalizer");
            ExceptionHelper.ValidateNotNull(signer, "signer");
            ExceptionHelper.ValidateNotNull(signID, "signID");

            XmlDocument doc = new XmlDocument();

            try
            {
                //Create Signature node
                XmlNode signatureNode = doc.CreateNode(XmlNodeType.Element, "Signature", null);

                //Create SignedInfo Node
                XmlNode signedInfoNode = CreateSignedInfoNode(doc, canonicalizer, signer, references);

                //Get Canonicalizer and canonicalize
                ICanonicalizer canonInst = registry.GetCanonicalizerInstance(canonicalizer.Key,
                    canonicalizer.Params);
                string canonicalized = canonInst.BringToCanonicalForm(signedInfoNode.OuterXml);

                //Add SignedInfo node to Signature node
                signatureNode.InnerXml += canonicalized;

                //Get Signer Instance , sign, assign to signatureValue node
                ISigner signerInst = registry.GetSignerInstance(signer.Key, signer.Params);
                string signed = signerInst.Sign(UnicodeEncoding.Unicode.GetBytes(canonicalized));
                XmlNode signValueNode = CreateSignatureValue(doc, signed);

                //Append signatureValue to Signature node
                signatureNode.InnerXml += signValueNode.OuterXml;

                //Add default namespace attribute to Signature node
                XmlAttribute defNs = doc.CreateAttribute("xmlns");
                defNs.Value = DEF_XMLDSIG_NS;
                signatureNode.Attributes.Append(defNs);
                //Add Id attribute to Signature node
                XmlAttribute idAttr = doc.CreateAttribute("Id");
                idAttr.Value = signID;
                signatureNode.Attributes.Append(idAttr);

                return signatureNode;
            }
            catch (Exception ex)
            {
                throw new SignatureManagerException(SIGN_MAN_EXCP_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Process the Signature node and verify the digital signatire of the contents.</p>
        /// </summary>
        /// <exception cref="VerificationException">if something went wrong during verification</exception>
        /// <exception cref="VerificationFailedException">if the signature is invalid</exception>
        /// <exception cref="ArgumentNullException">If any input parameter is null</exception>
        /// <param name="node">xml node with digital signature to be verified</param>
        /// <param name="keyInfoProvider">The key information for verifying the signature</param>
        /// <param name="soapRefLoader">SoapMessageRefernceLoader instance if the Signature was used on a 
        /// SoapMessage. Should not be null incase of verification of soapMessage. 
        /// Can be null if verification is of a web reference.
        /// </param>
        /// 
        /// <example>
        /// <para>If Signature node to be verified contains all non soap references
        /// <code>
        /// SignatureManager sm = new SignatureManager();
        /// sm.VerifySignature(node, keyInfoProvider, null);
        /// </code>
        /// </para>
        /// <para>If Signature node to be verified contains soap references
        /// <code>
        /// SignatureManager sm = new SignatureManager();
        /// SoapMessageReferenceLoader smrl = new SoapMessageRefernceLoader(theSoapMessage);
        /// sm.VerifySignature(node, keyInfoProvider, smrl);
        /// </code>
        /// Here theSoapMessage is the soap message that was signed.
        /// </para>
        /// For more information see the demo in CS and in test cases.
        /// </example>
        public void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider, IReferenceLoader soapRefLoader)
        {
            //Validate not null
            ExceptionHelper.ValidateNotNull(node, "node");
            ExceptionHelper.ValidateNotNull(keyInfoProvider, "keyInfoProvider");

            try
            {
                //Get SignedInfo node
                XmlNode signedInfoNode = node.SelectSingleNode("SignedInfo");

                //Get CanonicalizationNode
                XmlNode canonNode = signedInfoNode.SelectSingleNode("CanonicalizationMethod");
                string canonMethod = canonNode.Attributes.GetNamedItem("Algorithm").Value;
                //Get Canonicalization instance
                ICanonicalizer canonInst = registry.GetCanonicalizerInstance(canonMethod, emptyDic);
                string canonicalized = canonInst.BringToCanonicalForm(signedInfoNode.OuterXml);

                //Get Signing Algo
                XmlNode signAlgoNode = node.SelectSingleNode("SignedInfo/SignatureMethod");
                string signingAlgo = signAlgoNode.Attributes.GetNamedItem("Algorithm").Value;

                //Get Signer instance using the Signature mEthos Algorithm attribute value
                ISigner signer = registry.GetSignerInstance(signingAlgo, emptyDic);

                //Get keyInfoProvider instance
                IKeyInfoProvider keyInfoProviderInst = registry.GetKeyInfoProviderInstance(keyInfoProvider.Key,
                    keyInfoProvider.Params);

                //Verify the sign
                signer.Verify(canonicalized, keyInfoProviderInst, node);

                //Now proceed to check all references
                XmlNodeList list = node.SelectNodes("SignedInfo/Reference");
                VerifyReferences(list, soapRefLoader);
            }
            catch (VerificationFailedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VerificationException(VERIF_EXCP_MSG, ex);
            }
        }

        /// <summary>
        /// <p>Process the Signature node and verify the digital signatire of the contents.</p>
        /// </summary>
        /// <exception cref="VerificationException">if something went wrong during verification</exception>
        /// <exception cref="VerificationFailedException">if the signature is invalid</exception>
        /// <exception cref="ArgumentNullException">If any input parameter is null</exception>
        /// <param name="node">xml node with digital signature to be verified</param>
        /// <param name="keyInfoProvider">The key information for verifying the signature</param>
        /// 
        /// <example>
        /// <para>If Signature node to be verified contains all non soap references
        /// <code>
        /// SignatureManager sm = new SignatureManager();
        /// sm.VerifySignature(node, keyInfoProvider, null);
        /// </code>
        /// </para>
        /// <para>If Signature node to be verified contains soap references
        /// <code>
        /// SignatureManager sm = new SignatureManager();
        /// SoapMessageReferenceLoader smrl = new SoapMessageRefernceLoader(theSoapMessage);
        /// sm.VerifySignature(node, keyInfoProvider, smrl);
        /// </code>
        /// Here theSoapMessage is the soap message that was signed.
        /// </para>
        /// For more information see the demo in CS and in test cases.
        /// </example>
        public void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider)
        {
            //Validate not null
            ExceptionHelper.ValidateNotNull(node, "node");
            ExceptionHelper.ValidateNotNull(keyInfoProvider, "keyInfoProvider");

            try
            {
                //Get SignedInfo node
                XmlNode signedInfoNode = node.SelectSingleNode("SignedInfo");

                //Get CanonicalizationNode
                XmlNode canonNode = signedInfoNode.SelectSingleNode("CanonicalizationMethod");
                string canonMethod = canonNode.Attributes.GetNamedItem("Algorithm").Value;
                //Get Canonicalization instance
                ICanonicalizer canonInst = registry.GetCanonicalizerInstance(canonMethod, emptyDic);
                string canonicalized = canonInst.BringToCanonicalForm(signedInfoNode.OuterXml);

                //Get Signing Algo
                XmlNode signAlgoNode = node.SelectSingleNode("SignedInfo/SignatureMethod");
                string signingAlgo = signAlgoNode.Attributes.GetNamedItem("Algorithm").Value;

                //Get Signer instance using the Signature mEthos Algorithm attribute value
                ISigner signer = registry.GetSignerInstance(signingAlgo, emptyDic);

                //Get keyInfoProvider instance
                IKeyInfoProvider keyInfoProviderInst = registry.GetKeyInfoProviderInstance(keyInfoProvider.Key,
                    keyInfoProvider.Params);

                //Verify the sign
                signer.Verify(canonicalized, keyInfoProviderInst, node);

                //Now proceed to check all references
                XmlNodeList list = node.SelectNodes("SignedInfo/Reference");
                VerifyReferences(list, null);
            }
            catch (VerificationFailedException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new VerificationException(VERIF_EXCP_MSG, ex);
            }
        }

        #region "Helper Functions"

        /// <summary>
        /// Creates a Reference node according to XML spec http://www.w3.org/TR/xmldsig-core/#sec-Reference
        /// </summary>
        /// <param name="doc">XmlDocument used to create new attribute and nodes</param>
        /// <param name="uriData">Digested value of Uri data</param>
        /// <param name="uriName">Qualified Uri name</param>
        /// <param name="digestMethod">Algorithm used for digesting</param>
        /// <returns>a Reference node according to XML spec http://www.w3.org/TR/xmldsig-core/#sec-Reference
        /// </returns>
        private XmlNode CreateReferenceNode(XmlDocument doc, string uriData, string uriName, string digestMethod)
        {
            //Create Reference node
            XmlNode refNode = doc.CreateNode(XmlNodeType.Element, "Reference", null);

            //Create DigestValue node
            XmlNode digestValNode = doc.CreateNode(XmlNodeType.Element, "DigestValue", null);
            digestValNode.InnerXml = uriData;

            //Create DigestMethod node
            XmlNode digestMethodNode = doc.CreateNode(XmlNodeType.Element, "DigestMethod", null);
            XmlAttribute algoAttr = doc.CreateAttribute("Algorithm");
            algoAttr.Value = digestMethod;
            digestMethodNode.Attributes.Append(algoAttr);

            refNode.InnerXml += digestMethodNode.OuterXml;
            refNode.InnerXml += digestValNode.OuterXml;

            //Create URI attribute and add it to Reference Node
            XmlAttribute uriAttr = doc.CreateAttribute("URI");
            uriAttr.Value = uriName;
            refNode.Attributes.Append(uriAttr);
                return refNode;
        }

        /// <summary>
        /// Creates a CanonicalizationMethod node according to XML spec at
        /// http://www.w3.org/TR/xmldsig-core/#sec-CanonicalizationMethod
        /// </summary>
        /// <param name="doc">XmlDocument used to create new attribute and nodes</param>
        /// <param name="algoValue">Algorithm used for Canonicalization</param>
        /// <returns>a CanonicalizationMethod node according to XML spec</returns>
        private XmlNode CreateCanonicalizationMethodNode(XmlDocument doc, string algoValue)
        {
            //Create CanonicalizationMethod node
            XmlNode canonMethodNode = doc.CreateNode(XmlNodeType.Element, "CanonicalizationMethod", null);

            //Create Algorithm attribute
            XmlAttribute canonAlgoAttr = doc.CreateAttribute("Algorithm");

            //Set Algorithm attribute
            canonAlgoAttr.Value = algoValue;

            //Add the Algo attribute
            canonMethodNode.Attributes.Append(canonAlgoAttr);

            return canonMethodNode;
        }

        /// <summary>
        /// Creates a SignatureMethod node according to XML spec at
        /// http://www.w3.org/TR/xmldsig-core/#sec-SignatureMethod
        /// </summary>
        /// <param name="doc">XmlDocument used to create new attribute and nodes</param>
        /// <param name="algoValue">Algorithm used for Signing</param>
        /// <returns>a SignatureMethod node according to XML spec</returns>
        private XmlNode CreateSignatureMethod(XmlDocument doc, string algoValue)
        {
            //Create SignatureMethod node
            XmlNode signMethodNode = doc.CreateNode(XmlNodeType.Element, "SignatureMethod", null);

            //Create Algorithm attribute
            XmlAttribute signAlgoAttr = doc.CreateAttribute("Algorithm");

            //Set Algorithm attribute
            signAlgoAttr.Value = algoValue;

            //Add the Algo attribute
            signMethodNode.Attributes.Append(signAlgoAttr);

            return signMethodNode;
        }

        /// <summary>
        /// Creates a SignatureValue node according to XML spec at
        /// http://www.w3.org/TR/xmldsig-core/#sec-SignatureValue
        /// </summary>
        /// <param name="doc">XmlDocument used to create new attribute and nodes</param>
        /// <param name="value">The signature value</param>
        /// <returns>a SignatureValue node according to XML spec at
        /// http://www.w3.org/TR/xmldsig-core/#sec-SignatureValue</returns>
        private XmlNode CreateSignatureValue(XmlDocument doc, string value)
        {
            XmlNode signValueNode = doc.CreateNode(XmlNodeType.Element, "SignatureValue", null);
            signValueNode.InnerXml = value;
            return signValueNode;
        }

        /// <summary>
        /// Creates SignedInfo node according to Xml spec at http://www.w3.org/TR/xmldsig-core/#sec-SignedInfo
        /// </summary>
        /// <param name="doc">XmlDocument used to create new attribute and nodes</param>
        /// <param name="canonicalizer">InstantiationVO containing the instance and
        /// property values of the canonicalizer to load</param>
        /// <param name="signer">InstantiationVO containing the instance and property
        /// values of the signer to load</param>
        /// <param name="references">List of references to sign</param>
        /// <returns>SignedInfo node according to Xml spec at http://www.w3.org/TR/xmldsig-core/#sec-SignedInfo
        /// </returns>
        private XmlNode CreateSignedInfoNode(XmlDocument doc, InstantiationVO canonicalizer, InstantiationVO signer,
            IList<IReference> references)
        {
            //Create SignedInfo node
            XmlNode signedInfoNode = doc.CreateNode(XmlNodeType.Element, "SignedInfo", null);

            //Create CanonicalizationMethod node and add it to SignedInfo node
            XmlNode canonMethodNode = CreateCanonicalizationMethodNode(doc, canonicalizer.Key);
            signedInfoNode.InnerXml += canonMethodNode.OuterXml;

            //Create SignatureMethod node and add it to SignedInfo node
            XmlNode signMethodNode = CreateSignatureMethod(doc, signer.Key);
            signedInfoNode.InnerXml += signMethodNode.OuterXml;

            //Process all the references
            foreach (IReference reference in references)
            {
                //Get instance of reference loader.
                IReferenceLoader referenceLoader = null;

                switch (reference.Protocol)
                {
                    case "http":
                        referenceLoader = registry.GetReferenceLoaderInstance("http", emptyDic);
                        break;
                    case "soap":
                        referenceLoader = reference.SoapRefLoader;
                        break;
                }

                //Load reference data using the reference loader.
                byte[] uriData = referenceLoader.LoadReferenceData(reference.ReferenceURI);

                //Apply Transforms if necessary
                if (reference.TransformerInstanceDefinitions.Count > 0)
                {
                    //Create Transforms node
                    XmlNode transformsNode = doc.CreateNode(XmlNodeType.Element, "Transforms", null);
                    foreach (InstantiationVO transfInstVO in reference.TransformerInstanceDefinitions)
                    {
                        //Get Transformer instance
                        ITransformer transfInst =
                            registry.GetTransformerInstance(transfInstVO.Key, transfInstVO.Params);

                        //Create Tansform node and add Algorithm attribute
                        XmlNode transformNode = doc.CreateNode(XmlNodeType.Element, "Transform", null);
                        XmlAttribute attr = doc.CreateAttribute("Algorithm");
                        attr.Value = transfInstVO.Key;
                        transformNode.Attributes.Append(attr);

                        //Add Tranform node to Transforms node
                        transformsNode.InnerXml += transformNode.OuterXml;

                        //Do the transform
                        uriData = transfInst.Transform(uriData);
                    }
                    //Add Transforms node to the SignedInfo node
                    signedInfoNode.InnerXml += transformsNode.OuterXml;
                }

                //Get instance of digester
                IDigester digester = registry.GetDigesterInstance(reference.DigesterInstanceDefinition.Key,
                    reference.DigesterInstanceDefinition.Params);

                //Produce a digest for the reference data bytes
                byte[] digestedData = digester.Digest(uriData);

                //Create Reference node with appropriate data set
                XmlNode refNode = CreateReferenceNode(doc, Convert.ToBase64String(digestedData),
                    reference.ReferenceURI, reference.DigesterInstanceDefinition.Key);

                //Add reference node to SignedInfo node
                signedInfoNode.InnerXml += refNode.OuterXml;
            }

            return signedInfoNode;
        }

        /// <summary>
        /// Verifies the references in all the Reference nodes passed in the list
        /// </summary>
        /// <param name="list">The list of XmlNodes containing the Reference nodes</param>
        /// <param name="soapRefLoader">The SoapReferenceLoader incase we are verifying SOAP messages</param>
        private void VerifyReferences(XmlNodeList list, IReferenceLoader soapRefLoader)
        {
            try
            {
                foreach (XmlNode referenceNode in list)
                {
                    //Get URI
                    string uri = referenceNode.Attributes.GetNamedItem("URI").Value;

                    //Get protocol and load reference loader
                    string protocol = ExtractProtocol(uri);

                    //Make array for holding the refernce data after loading
                    byte[] refData = null;
                    switch (protocol)
                    {
                        case "http":
                            {
                                IReferenceLoader httpRefLoader = 
                                    registry.GetReferenceLoaderInstance(protocol, emptyDic);

                                //Load reference
                                refData = httpRefLoader.LoadReferenceData(uri);
                                break;
                            }
                        case "soap":
                            {
                                ExceptionHelper.ValidateNotNull(soapRefLoader, "soapRefLoader");

                                //Load reference
                                refData = soapRefLoader.LoadReferenceData(uri);
                                break;
                            }
                        default:
                            break;
                    }

                    //Get Digest method and load digester
                    XmlNode digestMethodNode = referenceNode.SelectSingleNode("DigestMethod");
                    string digestAlgo = digestMethodNode.Attributes.GetNamedItem("Algorithm").Value;
                    IDigester digester = registry.GetDigesterInstance(digestAlgo, emptyDic);

                    //Digest reference data
                    byte[] calculatedDigest = digester.Digest(refData);

                    //Get digest value in xml
                    XmlNode digestInXmlNode = referenceNode.SelectSingleNode("DigestValue");
                    byte[] digestInXml = Convert.FromBase64String(digestInXmlNode.InnerXml);

                    //If calculated digest and digest in xml are not same,
                    //either the reference itself has changed or we have a tampered reference node.
                    if (CompareByteArrays(calculatedDigest, digestInXml) == false)
                    {
                        throw new VerificationFailedException(REF_VERIF_FAILED);
                    }
                }
            }
            catch (VerificationFailedException vfEx)
            {
                throw vfEx;
            }
            catch (Exception ex)
            {
                throw new VerificationException(VERIF_EXCP_MSG, ex);
            }
        }

        /// <summary>
        /// Extracts the protocol to use from a given uri string
        /// </summary>
        /// <param name="uri">The uri string</param>
        /// <returns>The protocol to use for loading the uri</returns>
        /// <exception cref="ApplicationException">If protocol is not http or soap</exception>
        private string ExtractProtocol(string uri)
        {
            if (uri.StartsWith("http"))
            {
                return "http";
            }
            else if (uri.StartsWith("#"))
            {
                return "soap";
            }

            throw new ApplicationException("Unidentified reference protocol for the uri " + uri);
        }

        /// <summary>
        /// Compares two byte arrays.
        /// </summary>
        /// <param name="calculatedDigest">The digest calculated from the reference</param>
        /// <param name="digestInXml">The digest present in the signature reference nodes</param>
        /// <returns>true if byte arrays are equal else false</returns>
        private bool CompareByteArrays(byte[] calculatedDigest , byte[] digestInXml)
        {
            //The following code compares the two byte arrays
            if (calculatedDigest.Length != digestInXml.Length)
            {
                return false;
            }
            for (int i = 0; i < digestInXml.Length; i++)
            {
                if (digestInXml[i] != calculatedDigest[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
