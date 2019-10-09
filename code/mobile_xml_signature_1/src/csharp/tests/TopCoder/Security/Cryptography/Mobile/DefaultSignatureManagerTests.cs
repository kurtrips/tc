// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using TopCoder.Util.CompactConfigurationManager;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the SignatureManager class.
    /// </summary>
    [TestFixture]
    public class SignatureManagerTests
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
        private static string DefaultNamespace = "TopCoder.Security.Cryptography.Mobile";

        /// <summary>
        /// Sets up the various classes needed for signing
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Setup References
            transformersList1 = new List<InstantiationVO>();
            digester1 = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha1",
                new Dictionary<string, object>());
            reference1 = new Reference("http://www.google.com", transformersList1, digester1, "http");
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
            signerIVO = new InstantiationVO("xml:dig:signer:rsa-dsa", signerParams);

            //Setup main digester
            digesterMain = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha1",
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
        /// Tests the constructor.
        /// SignatureManager()
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            sm = new SignatureManager();
            Assert.IsNotNull(sm, "SignatureManager instance is null");
            Assert.IsTrue(sm is SignatureManager, "SignatureManager instance has wrong type");
        }

        /// <summary>
        /// Tests the constructor for failure when namespace does not exist in config file.
        /// SignatureManager(string namespace)
        /// ConfigurationException expected
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor2()
        {
            sm = new SignatureManager("MyNamespace");
        }

        /// <summary>
        /// Tests the constructor for failure when parameter passed is null
        /// SignatureManager(string namespace)
        /// </summary>
        [Test , ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor3()
        {
            string s = null;
            sm = new SignatureManager(s);

        }

        /// <summary>
        /// Tests the constructor for failure when parameter passed is empty string
        /// SignatureManager(string namespace)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor4()
        {
            string s = String.Empty;
            sm = new SignatureManager(s);
        }

        /// <summary>
        /// Tests the constructor for accuracy
        /// SignatureManager(ProcessRegistry registry)
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor5()
        {
            ProcessRegistry pr = new ProcessRegistry();
            sm = new SignatureManager(pr);
            Assert.IsNotNull(sm, "SignatureManager instance is null");
            Assert.IsTrue(sm is SignatureManager, "SignatureManager instance has wrong type");
        }

        /// <summary>
        /// Tests the constructor for failure when passed parameter is null
        /// SignatureManager(ProcessRegistry registry)
        /// </summary>
        [Test , ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor6()
        {
            ProcessRegistry pr = null;
            sm = new SignatureManager(pr);
        }

        /// <summary>
        /// Tests the constructor for accuracy
        /// SignatureManager(string aNamespace)
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor7()
        {
            ProcessRegistry pr = new ProcessRegistry();
            sm = new SignatureManager(DefaultNamespace);
            Assert.IsNotNull(sm, "SignatureManager instance is null");
            Assert.IsTrue(sm is SignatureManager, "SignatureManager instance has wrong type");
        }

        /// <summary>
        /// Tests the constructor for failure
        /// SignatureManager(string aNamespace)
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor8()
        {
            ProcessRegistry pr = new ProcessRegistry();
            sm = new SignatureManager("aNonExistantNamespace");
        }

        /// <summary>
        /// Tests the Sign method for accuracy
        /// XmlNode Sign(IList of IReference references, InstantiationVO canonicalizer, InstantiationVO signer,
        /// InstantiationVO digester, string signID)
        /// </summary>
        [Test]
        public void TestSign()
        {
            sm = new SignatureManager();
            XmlNode res = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //Verify if the Signature node is well formed
            Assert.AreEqual(res.Name, "Signature", "Root Node is malformed");
            Assert.AreEqual(res.Attributes.Count, 2, "Root attribute collection has incorrect count");
            Assert.AreEqual(res.Attributes[0].Name, "xmlns", "Root attribute collection has no xmlns attribute");
            Assert.AreEqual(res.Attributes.GetNamedItem("Id").Value, "myFirstSign", "Id attribute is incorrect");
            //Other checks for well formed output are done by VerifySignature method
        }

        /// <summary>
        /// Tests the Sign method for failure when any of the first 4 parameters are null
        /// XmlNode Sign(IList of IReference references, InstantiationVO canonicalizer, InstantiationVO signer,
        /// InstantiationVO digester, string signID)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSignFail1()
        {
            sm = new SignatureManager();
            sm.Sign(references, c14nIVO, null, "myFirstSign");
        }

        /// <summary>
        /// Tests the Sign method for failure when sign Id is null
        /// XmlNode Sign(IList of IReference references, InstantiationVO canonicalizer, InstantiationVO signer,
        /// InstantiationVO digester, string signID)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSignFail2()
        {
            sm = new SignatureManager();
            sm.Sign(references, c14nIVO, signerIVO, null);
        }

        /// <summary>
        /// Tests the Sign method for failure when key of one of classes (here tested for Signer)
        /// does not exist in registry
        /// XmlNode Sign(IList of IReference references, InstantiationVO canonicalizer, InstantiationVO signer,
        /// InstantiationVO digester, string signID)
        /// </summary>
        [Test, ExpectedException(typeof(SignatureManagerException))]
        public void TestSignFail3()
        {
            sm = new SignatureManager();
            signerIVO = new InstantiationVO("nonExistantSignerKey", signerParams);
            sm.Sign(references, c14nIVO, signerIVO, "myId");
        }

        /// <summary>
        /// Tests the VerifySignature method for accuracy
        /// void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider)
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestVerifySignature()
        {
            //First sign
            sm = new SignatureManager();
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //Now Verify
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
        /// Tests the VerifySignature method for failure when SignatureValue is tampered
        /// void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider)
        /// VerificationFailedException is expected
        /// </summary>
        [Test, ExpectedException(typeof(VerificationFailedException))]
        public void TestVerifySignatureFailure1()
        {
            //First sign
            sm = new SignatureManager();
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //get the SignatureValue
            XmlNode sigValueNode = signed.SelectSingleNode("SignatureValue");
            string signature = sigValueNode.InnerXml;

            //Tamper it.
            signature = signature.ToUpper();
            signed.SelectSingleNode("SignatureValue").InnerXml = signature;

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
        /// Tests the VerifySignature method for failure when Reference digest value is tampered
        /// void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider)
        /// VerificationFailedException is expected
        /// </summary>
        [Test, ExpectedException(typeof(VerificationFailedException))]
        public void TestVerifySignatureFailure2()
        {
            //First sign
            sm = new SignatureManager();
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //get the Digest value
            XmlNode digValueNode = signed.SelectSingleNode("SignedInfo/Reference/DigestValue");
            string digest = digValueNode.InnerXml;

            //Tamper it.
            digest = digest.ToUpper();
            signed.SelectSingleNode("SignedInfo/Reference/DigestValue").InnerXml = digest;

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
        /// Tests the VerifySignature method for failure when passed public key is incorrect
        /// void VerifySignature(XmlNode node, InstantiationVO keyInfoProvider)
        /// VerificationFailedException is expected
        /// </summary>
        [Test, ExpectedException(typeof(VerificationFailedException))]
        public void TestVerifySignatureFailure3()
        {
            //First sign
            sm = new SignatureManager();
            XmlNode signed = sm.Sign(references, c14nIVO, signerIVO, "myFirstSign");

            //Now verify the XmlSignature using the VerifySignature method exposed by the SignatureManager class
            IDictionary<string, object> keyInfoProviderParams = new Dictionary<string, object>();

            //Note here the use of false only exports the public key to the DSA parameters
            //Note that new instance of DSACryptoServiceProvider creates a different public key than
            //the one that was used for signing
            DSACryptoServiceProvider dsa1 = new DSACryptoServiceProvider();
            DSAParameters verificationDSAParams = dsa1.ExportParameters(false);

            //Set the Public Key of the KeyInfoProvider class
            keyInfoProviderParams.Add("PublicKey", verificationDSAParams);

            //Create Instantiation VO for KeyInfoProvider
            InstantiationVO keyInfoProvider = new InstantiationVO("testKeyInfoProvider", keyInfoProviderParams);

            //Verify
            sm.VerifySignature(signed, keyInfoProvider);
        }
    }
}
