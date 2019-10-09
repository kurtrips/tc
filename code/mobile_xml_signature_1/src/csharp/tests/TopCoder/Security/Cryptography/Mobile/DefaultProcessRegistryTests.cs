// Copyright (c)2003, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the ProcessRegistry class.
    /// </summary>
    [TestFixture]
    public class ProcessRegistryTests
    {
        /// <summary>
        /// The ProcessRegistry instance to use for this test suite
        /// </summary>
        ProcessRegistry pr;

        /// <summary>
        /// An empty dictionary conatining no parameters.
        /// Used for calls to GetXXXInstance
        /// </summary>
        IDictionary<string, object> emptyDic = new Dictionary<string, object>();

        /// <summary>
        /// The default namespace to use for this test suite
        /// </summary>
        private const string DefaultNamespace = "TopCoder.Security.Cryptography.Mobile";
        
        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            pr = new ProcessRegistry();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            pr = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// ProcessRegistry()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsNotNull(pr, "ProcessRegistry instance is null");
            Assert.IsTrue(pr is ProcessRegistry, "ProcessRegistry instance has wrong type");
        }

        /// <summary>
        /// Tests the constructor.
        /// ProcessRegistry(string namespace)
        /// </summary>
        [Test]
        public void TestConstructorAcc()
        {
            pr = new ProcessRegistry(DefaultNamespace);
        }

        /// <summary>
        /// Tests the constructor for failure
        /// ProcessRegistry(string namespace)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            pr = new ProcessRegistry( (string)null );
        }

        /// <summary>
        /// Tests the constructor for failure
        /// ProcessRegistry(string namespace)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail2()
        {
            string s = "                 ";
            pr = new ProcessRegistry(s);
        }

        /// <summary>
        /// Tests the RegisteredDefinitionsMap property.
        /// </summary>
        [Test]
        public void Testget_RegisteredDefinitionsMap()
        {
            Assert.IsNotNull(pr.RegisteredDefinitionsMap , "RegisteredDefinitionsMap is null");
            Assert.IsTrue(pr.RegisteredDefinitionsMap is IDictionary<string, string>, 
                "RegisteredDefinitionsMap is not of type IDictionary<string,string>");
        }

        /// <summary>
        /// Tests the GetDigesterInstance method.
        /// </summary>
        [Test]
        public void TestGetDigesterInstance()
        {
            IDigester dig = pr.GetDigesterInstance("http://www.w3.org/2000/09/xmldsig#sha1", emptyDic);
            Assert.IsNotNull(dig, "Digester instance is null");
            Assert.IsTrue(dig is Digesters.SHA1Digester, "Digester has incorect type");
        }

        /// <summary>
        /// Tests the GetSignerInstance method.
        /// </summary>
        [Test]
        public void TestGetSignerInstance()
        {
            ISigner signer = pr.GetSignerInstance("xml:dig:signer:rsa-dsa", emptyDic);
            Assert.IsNotNull(signer, "Signer instance is null");
            Assert.IsTrue(signer is Signers.DSASigner,
                "Signer has incorect type");
        }

        /// <summary>
        /// Tests the GetCanonicalizerInstance method.
        /// </summary>
        [Test]
        public void TestGetCanonicalizerInstance()
        {
            ICanonicalizer canonc = pr.GetCanonicalizerInstance("http://www.w3.org/TR/2001/REC-xml-c14n-20010315",
                emptyDic);
            Assert.IsNotNull(canonc, "Canonicalizer instance is null");
            Assert.IsTrue(canonc is Canonicalizers.StandardFormCanonicalizer,
                "Canonicalizer has incorect type");
        }

        /// <summary>
        /// Tests the GetReferenceLoaderInstance method.
        /// </summary>
        [Test]
        public void TestGetReferenceLoaderInstance()
        {
            IReferenceLoader refLoader = pr.GetReferenceLoaderInstance("http", emptyDic);
            Assert.IsNotNull(refLoader, "ReferenceLoader instance is null");
            Assert.IsTrue(refLoader is ReferenceLoaders.WebBasedReferenceLoader,
                "ReferenceLoader has incorect type");
        }

        /// <summary>
        /// Tests the GetKeyInfoProviderInstance method.
        /// </summary>
        [Test]
        public void TestGetKeyInfoProviderInstance()
        {
            IKeyInfoProvider kf = pr.GetKeyInfoProviderInstance("testKeyInfoProvider", emptyDic);
            Assert.IsNotNull(kf, "KeyInfoProvider instance is null");
            Assert.IsTrue(kf is DummyKeyInfoProvider, "KeyInfoProvider has incorect type");
        }

        /// <summary>
        /// Tests the GetTransformerInstance method.
        /// </summary>
        [Test]
        public void TestGetTransformerInstance()
        {
            ITransformer kf = pr.GetTransformerInstance("testTransformer", emptyDic);
            Assert.IsNotNull(kf, "Transformer instance is null");
            Assert.IsTrue(kf is DummyTransformer, "Transformer has incorect type");
        }

        /// <summary>
        /// Tests the GetTransformerInstance method.
        /// Null return value is expected
        /// </summary>
        [Test]
        public void TestGetTransformerInstanceError()
        {
            Assert.IsNull(pr.GetTransformerInstance("noSuchKey", emptyDic));
        }

        /// <summary>
        /// Tests the GetKeyInfoProviderInstance method.
        /// Null return value is expected
        /// </summary>
        [Test]
        public void TestGetKeyInfoProviderInstanceError()
        {
            Assert.IsNull(pr.GetKeyInfoProviderInstance("noSuchKey", emptyDic));
        }

        /// <summary>
        /// Tests the GetReferenceLoaderInstance method.
        /// Null return value is expected
        /// </summary>
        [Test]
        public void TestGetReferenceLoaderInstanceError()
        {
            Assert.IsNull(pr.GetReferenceLoaderInstance("noSuchKey", emptyDic));
        }

        /// <summary>
        /// Tests the GetSignerInstance method.
        /// Null return value is expected
        /// </summary>
        [Test]
        public void TestGetSignerInstanceError()
        {
            Assert.IsNull(pr.GetSignerInstance("noSuchKey", emptyDic));
        }

        /// <summary>
        /// Tests the GetCanonicalizerInstance method.
        /// Null return value is expected
        /// </summary>
        [Test]
        public void TestGetCanonicalizerInstanceError()
        {
            Assert.IsNull(pr.GetCanonicalizerInstance("noSuchKey", emptyDic));
        }

        /// <summary>
        /// Tests the GetCanonicalizerInstance method.
        /// Null return value is expected
        /// </summary>
        [Test]
        public void TestGetDigesterInstanceError()
        {
            Assert.IsNull(pr.GetDigesterInstance("noSuchKey", emptyDic));
        }
    }
}
