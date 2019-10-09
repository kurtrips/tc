using System;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Net;
using System.Xml;
using TopCoder.Security.Cryptography.Mobile;
using TopCoder.Security.Cryptography.Mobile.ReferenceLoaders;
using System.Collections.Generic;
using System.Security.Cryptography;

// Define a SOAP Extension that traces the SOAP request and SOAP
// response for the XML Web service method the SOAP extension is
// applied to.
namespace TestWSClient
{
    public class TraceExtension : SoapExtension
    {
        Stream wireStream;
        Stream appStream;

        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            wireStream = stream;
            appStream = new MemoryStream();
            return appStream;
        }

        // When the SOAP extension is accessed for the first time, the XML Web
        // service method it is applied to is accessed to store the file
        // name passed in, using the corresponding SoapExtensionAttribute.    
        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        // The SOAP extension was configured to run using a configuration file
        // instead of an attribute applied to a specific XML Web service
        // method.
        public override object GetInitializer(Type WebServiceType)
        {
            return null;
        }

        // Receive the file name stored by GetInitializer and store it in a
        // member variable for this specific instance.
        public override void Initialize(object initializer)
        {
        }

        //  If the SoapMessageStage is such that the SoapRequest or
        //  SoapResponse is still in the SOAP format to be sent or received,
        //  save it out to a file.
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    AddIdToRootNode(message);
                    Call_Mobile_Signature(message);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    ReceiveResponse(message);
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
                default:
                    throw new Exception("invalid stage");
            }
        }

        /// <summary>
        /// Adds an Id attribute to the root node
        /// </summary>
        /// <param name="message">The soapMessage on which to operate</param>
        private void AddIdToRootNode(SoapMessage message)
        {
            XmlDocument doc = new XmlDocument();
            //Get stream from SoapMessage
            Stream stream = message.Stream;
            stream.Position = 0;

            //Create XmlTextReader from stream
            XmlTextReader reader = new XmlTextReader(stream);
            //Load stream into xml
            doc.Load(reader);

            //Add Id to the root element itself
            XmlAttribute idAttr = doc.CreateAttribute("Id");
            idAttr.Value = "msgBody";
            doc.DocumentElement.Attributes.Append(idAttr);

            //Save the xml with id to the soap message stream
            stream.Position = 0;
            doc.Save(stream);

            appStream.Position = 0;
            Copy(appStream, wireStream);
        }

        /// <summary>
        /// Demonstrates a call to Mobile_XML_Signature Sign method
        /// </summary>
        /// <param name="message">The soapMessage to operate on</param>
        public void Call_Mobile_Signature(SoapMessage message)
        {
            //Create empty parameters dictionary
            Dictionary<string, object> emptyDic = new Dictionary<string, object>();

            //Create SoapMessage Reference Loader for reference
            IReferenceLoader smrl = new SoapMessageReferenceLoader(message);
            //Create Digester for Reference
            InstantiationVO digesterVO = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha1", emptyDic);
            //Create Transformers
            IList<InstantiationVO> transfs = new List<InstantiationVO>();
            //Create Soap Reference and Add it to list
            Reference soapReference = new Reference("#msgBody", transfs, digesterVO, smrl);
            IList<IReference> references = new List<IReference>();
            references.Add(soapReference);

            //Create Canonicalizer
            InstantiationVO c14nIVO = new InstantiationVO("http://www.w3.org/TR/2001/REC-xml-c14n-20010315", emptyDic);

            //Setup Signers
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            //Usage of true here exports private key also
            DSAParameters dsaParams = dsa.ExportParameters(true);
            IDictionary<string, object> signerParams = new Dictionary<string, object>();
            signerParams.Add("DSAKeyInfo", dsaParams);
            InstantiationVO signerIVO = new InstantiationVO("xml:dig:signer:rsa-dsa",
                signerParams);

            //Sign it
            SignatureManager sm = new SignatureManager();
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myId");
        }

        public void ReceiveResponse(SoapMessage message)
        {
            Copy(wireStream, appStream);
            appStream.Position = 0;
        }

        void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }
    }

    // Create a SoapExtensionAttribute for the SOAP Extension that can be
    // applied to an XML Web service method.
    [AttributeUsage(AttributeTargets.Method)]
    public class TraceExtensionAttribute : SoapExtensionAttribute
    {
        private int priority;

        public override Type ExtensionType
        {
            get { return typeof(TraceExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
    }
}