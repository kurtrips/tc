// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.Util.ConfigurationManager;
using System.Xml;

namespace TopCoder.CodeDoc.CSharp
{
    /// <summary>
    /// Unit tests for the CSharpAPIProcessor class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class CSharpAPIProcessorTests
    {
        /// <summary>
        /// The CSharpAPIProcessor instance to use for the tests.
        /// </summary>
        CSharpAPIProcessor csap;

        /// <summary>
        /// The ReflectionEngineParameters instance to use for the tests.
        /// </summary>
        ReflectionEngineParameters rep;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");

            rep = new ReflectionEngineParameters();
            rep.AssemblyFileNames = new string[] { UnitTestHelper.MOCKLIBPATH  };
            rep.ReferencePaths = new string[] { UnitTestHelper.REFPATH };
            rep.SlashDocFileNames = new string[] { UnitTestHelper.MOCKXMLPATH };
            rep.LoggerNamespace = "MyLoggerNamespace";
            rep.DocumentPrivates = true;

            csap = new CSharpAPIProcessor(rep);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ConfigManager.GetInstance().Clear(false);
            csap = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// CSharpAPIProcessor(ReflectionEngineParameters rep)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(csap is IXmlProcessor, "Wrong type of class.");
        }


        /// <summary>
        /// Tests the constructor when rep is null.
        /// CSharpAPIProcessor(ReflectionEngineParameters rep)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            csap = new CSharpAPIProcessor(null);
        }

        /// <summary>
        /// Tests the ProcessDocument method.
        /// void ProcessDocument(XmlDocument apiSpec)
        /// </summary>
        [Test]
        public void TestProcessDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<apispec></apispec>");
            csap.ProcessDocument(doc);
            Assert.AreNotEqual("<apispec></apispec>", doc.DocumentElement.OuterXml,
                "ProcessDocument should write the api specification.");
        }

        /// <summary>
        /// Tests the ProcessDocument method when no namespace or type matches the typePrefix.
        /// void ProcessDocument(XmlDocument apiSpec)
        /// </summary>
        [Test]
        public void TestProcessDocument1()
        {
            rep.TypePrefixes = new string[] { "NoSuchPrefix" };
            csap = new CSharpAPIProcessor(rep);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<apispec></apispec>");

            csap.ProcessDocument(doc);
            Assert.AreEqual("<apispec></apispec>", doc.DocumentElement.OuterXml,
                "No action must be taken.");
        }

        /// <summary>
        /// Tests the ProcessDocument method when apiSpec is null.
        /// void ProcessDocument(XmlDocument apiSpec)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestProcessDocumentFail1()
        {
            csap.ProcessDocument(null);
        }

        /// <summary>
        /// Tests the ProcessDocument method when apiSpec does not have root name as apispec.
        /// void ProcessDocument(XmlDocument apiSpec)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestProcessDocumentFail2()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<root></root>");
            csap.ProcessDocument(doc);
        }

        /// <summary>
        /// Tests the ProcessDocument method when it fails due to bad ReflectionEngineParameters.
        /// void ProcessDocument(XmlDocument apiSpec)
        /// XmlProcessorException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(XmlProcessorException))]
        public void TestProcessDocumentFail3()
        {
            rep.LoggerNamespace = "NoSuchLogger";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<apispec></apispec>");
            csap.ProcessDocument(doc);
        }


    }
}
