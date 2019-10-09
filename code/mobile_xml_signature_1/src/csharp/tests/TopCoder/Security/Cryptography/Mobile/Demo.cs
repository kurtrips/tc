// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using System.Diagnostics;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// The demonstration of the expected usage of this component.
    /// </summary>
    [TestFixture]
    public class Demo
    {
        /// <summary>
        /// The demonstration of the expected usage of this component.
        /// No exception expected
        /// </summary>
        [Test]
        public void TheDemo()
        {
            //Setup References
            IList<InstantiationVO> transformersList1 = new List<InstantiationVO>();
            InstantiationVO digester1 = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha1",
                new Dictionary<string, object>());
            IReference reference1 = new Reference("http://www.google.com", transformersList1, digester1, "http");
            IList<IReference> references = new List<IReference>();
            references.Add(reference1);

            //Setup Canonicalizer
            IDictionary<string, object> c14nParams = new Dictionary<string, object>();
            InstantiationVO c14nIVO = new InstantiationVO("http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                c14nParams);

            //Setup Signers
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            DSAParameters dsaParams = dsa.ExportParameters(true);
            IDictionary<string, object> signerParams = new Dictionary<string, object>();
            signerParams.Add("DSAKeyInfo", dsaParams);
            InstantiationVO signerIVO = new InstantiationVO("xml:dig:signer:rsa-dsa", 
                signerParams);

            //Setup main digester
            InstantiationVO digesterMain = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha1",
                new Dictionary<string, object>());

            SignatureManager sm = new SignatureManager();

            //Sign the references
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");


            //Now verify the XmlSignature using the VerifySignature method exposed by the SignatureManager class
            IDictionary<string, object> keyInfoProviderParams = new Dictionary<string, object>();

            //Note here the use of false only exports the public key to the DSA parameters
            DSAParameters verificationDSAParams = dsa.ExportParameters(false);

            //Set the Public Key of the KeyInfoProvider class
            keyInfoProviderParams.Add("PublicKey", verificationDSAParams);

            //Create Instantiation VO for KeyInfoProvider
            InstantiationVO keyInfoProvider = new InstantiationVO("testKeyInfoProvider", keyInfoProviderParams);

            //Verify
            sm.VerifySignature(signed, keyInfoProvider);
        }

        /// <summary>
        /// This test starts the Web Service using Process class of .Net framework.
        /// 
        /// Please refer to the Call_Xml_Signature function in the code of Web Service for 
        /// learning how to use this component for SOAP messages.
        /// The code is available at test_files/TestWebServiceClient/Code folder.
        /// 
        /// Test is successful if no exception is encountered.
        /// </summary>
        [Test]
        public void SoapDemo()
        {
            Process webServiceClient = new Process();
            string path = "../../test_files/TestWebServiceClient/TestWSClient.exe";

            webServiceClient.StartInfo.FileName = path;
            webServiceClient.Start();
        }
    }
}
