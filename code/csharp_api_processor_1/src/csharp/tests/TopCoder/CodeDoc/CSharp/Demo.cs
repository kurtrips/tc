// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER
using System;
using System.Xml;
using TopCoder.Configuration;
using TopCoder.Util.ConfigurationManager;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.CodeDoc.CSharp.Reflection;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp
{
    /// <summary>
    /// Provides a demonstration of this component.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class Demo
    {
        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");
        }

        /// <summary>
        /// Clears the test environment.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Using this component from the Command Line.
        /// </summary>
        [Test]
        public void DemoTest_CommandLine()
        {
            //Create CommandLineProcessor
            IConfiguration config = UnitTestHelper.GetConfig();
            CommandLineProcessor processor = new CommandLineProcessor(config);

            // to run the processor we need an argument array.
            // this one will analyze MockLibrary.dll with the inline documentation
            // provided by MockLibrary.xml. Only the types whose namespace
            // starts with ‘MockLibrary’ are listed. And we also generate the API for
            // private elements.
            string[] args = new string[5];
            args[0] = "/assemblies:" + UnitTestHelper.MOCKLIBPATH;
            args[1] = "/docFiles:" + UnitTestHelper.MOCKXMLPATH;
            args[2] = "/modules:MockLibrary.dll";
            args[3] = "/typePrefixes:MockLibrary";
            args[4] = "/documentPrivates";

            // create a new XmlDocument with root <apispec>
            XmlDocument apiSpec = new XmlDocument();
            apiSpec.LoadXml("<apispec></apispec>");

            // generate the API spec to apiSpec XmlDocument
            processor.ProcessDocument(args, apiSpec);
        }

        /// <summary>
        /// Using this component in a non-command line way.
        /// Here only MockLibrary is documented.
        /// </summary>
        [Test]
        public void DemoTest_NonCommandLine1()
        {
            //Setup options
            ReflectionEngineParameters rep = new ReflectionEngineParameters();
            rep.AssemblyFileNames = new string[] { UnitTestHelper.MOCKLIBPATH };
            rep.ReferencePaths = new string[] { UnitTestHelper.REFPATH };
            rep.SlashDocFileNames = new string[] { UnitTestHelper.MOCKXMLPATH };
            rep.DocumentPrivates = true;

            ReflectionEngine re = new ReflectionEngine();

            //Write the API specification
            string xml = re.WriteAPISpec(rep, "<apispec></apispec>");
            XmlDocument xmlOutput = new XmlDocument();
            xmlOutput.LoadXml(xml);
        }

        /// <summary>
        /// Using this component in a non-command line way.
        /// Here MockLibrary and BaseLibrary are both documented even though BaseLibrary's doc file is not yet present.
        /// This makes sure that the documentation of MockLibrary makes correct xpaths to classes in BaseLibrary.
        /// The doc file for BaseLibrary can be added at any later time.
        /// </summary>
        [Test]
        public void DemoTest_NonCommandLine2()
        {
            //Setup options
            ReflectionEngineParameters rep = new ReflectionEngineParameters();
            rep.AssemblyFileNames =
                new string[] { UnitTestHelper.MOCKLIBPATH, UnitTestHelper.BASELIBPATH };
            rep.ReferencePaths = new string[] { UnitTestHelper.REFPATH };
            rep.SlashDocFileNames = new string[] { UnitTestHelper.MOCKXMLPATH };
            rep.DocumentPrivates = true;

            ReflectionEngine re = new ReflectionEngine();

            //Write the API specification
            string xml = re.WriteAPISpec(rep, "<apispec></apispec>");
            XmlDocument xmlOutput = new XmlDocument();
            xmlOutput.LoadXml(xml);
        }
    }
}
