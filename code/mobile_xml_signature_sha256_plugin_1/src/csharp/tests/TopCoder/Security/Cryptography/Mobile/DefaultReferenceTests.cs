// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author kurtrips

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the Reference class.
    /// </summary>
    [TestFixture]
    public class ReferenceTests
    {
        /// <summary>
        /// The class instance to use for all tests
        /// </summary>
        Reference refer;

        /// <summary>
        /// The transformers list to use
        /// </summary>
        IList<InstantiationVO> transformers;

        /// <summary>
        /// The uri string to use for all tests
        /// </summary>
        string uri;

        /// <summary>
        /// The digester to use for the reference
        /// </summary>
        InstantiationVO digester;

        /// <summary>
        /// The protocol to use for all tests
        /// </summary>
        string protocol;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            uri = "http://www.topcoder.com";
            transformers = new List<InstantiationVO>();
            digester = new InstantiationVO("http://www.w3.org/2000/09/xmldsig#sha1" ,
                new Dictionary<string,object>());
            protocol = "http";
            refer = new Reference(uri, transformers, digester, protocol);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            uri = null;
            transformers = null;
            digester = null;
            protocol = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// Reference(string referenceURI, IList transformerInstanceDefinitions,
        /// InstantiationVO digesterInstanceDefinition, string protocol)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(refer, "Reference instance is null");
            Assert.IsTrue(refer is Reference, "Reference instance has wrong type");
        }

        /// <summary>
        /// Tests the Protocol method.
        /// string Protocol()
        /// </summary>
        [Test]
        public void Test_Protocol()
        {
            Assert.AreEqual(refer.Protocol, "http", "Protocol property is incorrect");
        }

        /// <summary>
        /// Tests the ReferenceURI method.
        /// string ReferenceURI()
        /// </summary>
        [Test]
        public void TestReferenceURI()
        {
            Assert.AreEqual(refer.ReferenceURI, "http://www.topcoder.com", "ReferenceURI property is incorrect");
        }

        /// <summary>
        /// Tests the TransformerInstanceDefinitions method.
        /// IList TransformerInstanceDefinitions()
        /// </summary>
        [Test]
        public void TestTransformerInstanceDefinitions()
        {
            Assert.AreEqual(refer.TransformerInstanceDefinitions, transformers,
                "TransformerInstanceDefinitions property is incorrect");
            Assert.AreEqual(refer.TransformerInstanceDefinitions.Count, 0);
        }

        /// <summary>
        /// Tests the DigesterInstanceDefinition method.
        /// InstantiationVO DigesterInstanceDefinition()
        /// </summary>
        [Test]
        public void TestDigesterInstanceDefinition()
        {
            Assert.AreEqual(refer.DigesterInstanceDefinition, digester,
                "DigesterInstanceDefinition property is incorrect");

            Assert.AreEqual(refer.DigesterInstanceDefinition.Key, "http://www.w3.org/2000/09/xmldsig#sha1",
                "DigesterInstanceDefinition property is incorrect");
        }

        /// <summary>
        /// Tests failure of Reference constructor if uri is null
        /// </summary>
        [Test , ExpectedException(typeof(ArgumentNullException))]
        public void TestRefernceFailure()
        {
            refer = new Reference((string)null, transformers, digester, protocol);
        }

        /// <summary>
        /// Tests failure of Reference constructor if transformers is null
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestRefernceFailure1()
        {
            refer = new Reference(uri,(IList<InstantiationVO>)(null), digester, protocol);
        }

        /// <summary>
        /// Tests failure of Reference constructor if digester is null
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestRefernceFailure2()
        {
            refer = new Reference(uri, transformers, (InstantiationVO)(null), protocol);
        }

        /// <summary>
        /// Tests failure of Reference constructor if digester is null
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestRefernceFailure3()
        {
            refer = new Reference(uri, transformers, digester, (string)(null));
        }

        /// <summary>
        /// Tests failure of Reference constructor if any string parameter is empty string
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRefernceFailure4()
        {
            refer = new Reference(uri , transformers, digester, "          ");
        }
    }
}
