// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Xml;
using System.Collections.Generic;
using System.Security.Cryptography;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the SignatureManager class when using SHA256Digester.
    /// These tests are not comprehensive SignatureManager tests but instead check the overall
    /// functionality of the SignatureManager class when SHA256Digester is used
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SignatureManagerUsingSHA256Tests
    {
        /// <summary>
        /// The SignatureManager instance to use
        /// </summary>
        SignatureManager sm;

        /// <summary>
        /// The list of treansformers to use
        /// </summary>
        IList<InstantiationVO> transformersList1;

        /// <summary>
        /// The digester to use for reference1
        /// </summary>
        InstantiationVO digester1;

        /// <summary>
        /// The reference to use
        /// </summary>
        IReference reference1;

        /// <summary>
        /// The list of references to sign
        /// </summary>
        IList<IReference> references;

        /// <summary>
        /// The parameters to use for canonicalizers
        /// </summary>
        IDictionary<string, object> c14nParams;

        /// <summary>
        /// InstantiationVO for canonicalizer
        /// </summary>
        InstantiationVO c14nIVO;

        /// <summary>
        /// The DSA class to setup the signer
        /// </summary>
        DSACryptoServiceProvider dsa;

        /// <summary>
        /// The parametrs of DSA class to use for signing
        /// </summary>
        DSAParameters dsaParams;

        /// <summary>
        /// Parameters for signer class
        /// </summary>
        IDictionary<string, object> signerParams;

        /// <summary>
        /// The InstantiationVO for the signer
        /// </summary>
        InstantiationVO signerIVO;

        /// <summary>
        /// InstantiationVO for the digester
        /// </summary>
        InstantiationVO digesterMain;

        /// <summary>
        /// <p><strong>Represents:</strong></p> <p>This rrepresents the default configuration namespace to be used when
        /// instantiating and initializing an instance of registry.</p>
        /// </summary>
        private static string SHA256Namespace = "TopCoder.Security.Cryptography.Mobile.SHA256Test";

        /// <summary>
        /// Sets up the various classes needed for signing
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Setup References
            transformersList1 = new List<InstantiationVO>();
            digester1 = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha256",
                new Dictionary<string, object>());
            reference1 = new Reference("http://www.topcoder.com", transformersList1, digester1, "http");
            references = new List<IReference>();
            references.Add(reference1);

            //Setup Canonicalizer
            c14nParams = new Dictionary<string, object>();
            c14nIVO = new InstantiationVO("http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                c14nParams);

            //Setup Signers
            dsa = new DSACryptoServiceProvider();
            dsaParams = dsa.ExportParameters(true);
            signerParams = new Dictionary<string, object>();
            signerParams.Add("DSAKeyInfo", dsaParams);
            signerIVO = new InstantiationVO(dsa.SignatureAlgorithm, signerParams);

            //Setup main digester
            digesterMain = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha256",
                new Dictionary<string, object>());
        }

        /// <summary>
        /// Tear down the test environment.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sm = null;
            transformersList1 = null;
            digester1 = null;
            reference1 = null;
            references = null;
            c14nParams = null;
            c14nIVO = null;
            dsa = null;
            signerParams = null;
            signerIVO = null;
            digesterMain = null;
        }

        /// <summary>
        /// Tests the constructor when using SHA256Digester
        /// SignatureManager()
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            sm = new SignatureManager(SHA256Namespace);
            Assert.IsNotNull(sm, "SignatureManager instance is null");
            Assert.IsTrue(sm is SignatureManager, "SignatureManager instance has wrong type");
        }

        /// <summary>
        /// Tests the constructor for accuracy when using ProcessRegistry overload.
        /// SignatureManager(ProcessRegistry registry)
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            ProcessRegistry pr = new ProcessRegistry(SHA256Namespace);
            sm = new SignatureManager(pr);
            Assert.IsNotNull(sm, "SignatureManager instance is null");
            Assert.IsTrue(sm is SignatureManager, "SignatureManager instance has wrong type");
        }

        /// <summary>
        /// Tests the Sign method for accuracy when SHA256 digester is used
        /// XmlNode Sign(IList of IReference references, InstantiationVO canonicalizer, InstantiationVO signer,
        /// InstantiationVO digester, string signID)
        /// </summary>
        [Test]
        public void TestSignAccuracy()
        {
            sm = new SignatureManager(SHA256Namespace);
            XmlNode res = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //Verify if the Signature node is well formed
            Assert.AreEqual(res.Name, "Signature", "Root Node is malformed");
            Assert.AreEqual(res.NamespaceURI, "http://www.w3.org/2000/09/xmldsig#",
                "Root attribute collection has no default namespace");
            Assert.AreEqual(res.Attributes.GetNamedItem("Id").Value, "myFirstSign", "Id attribute is incorrect");

            //Other checks for well formed output are done by VerifySignature method
        }

        /// <summary>
        /// Tests the VerifySignature method for accuracy when SHA256 digester is used
        /// void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider)
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestVerifySignature()
        {
            //First sign
            sm = new SignatureManager(SHA256Namespace);
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //Now verify the XmlSignature using the VerifySignature method exposed by the SignatureManager class
            IDictionary<string, object> keyInfoProviderParams = new Dictionary<string, object>();

            //Note here the use of false only exports the public key to the DSA parameters
            DSAParameters verificationDSAParams = dsa.ExportParameters(false);

            //Set the Public Key of the KeyInfoProvider class
            keyInfoProviderParams.Add("PublicKey", verificationDSAParams);

            //Create Instantiation VO for KeyInfoProvider
            InstantiationVO keyInfoProvider = new InstantiationVO("testKeyInfoProvider", keyInfoProviderParams);

            //Verify. Note that if no exception is thrown here then verification was successful.
            //No assert would be required.
            sm.VerifySignature(signed, keyInfoProvider);
        }
    }
}
