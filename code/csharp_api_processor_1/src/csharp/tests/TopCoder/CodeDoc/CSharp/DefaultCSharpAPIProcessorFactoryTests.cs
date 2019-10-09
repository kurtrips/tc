// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections;
using System.Collections.Generic;
using TopCoder.Util.ConfigurationManager;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.CodeDoc.CSharp.Reflection;
using TopCoder.Util.CommandLine;
using TopCoder.Configuration;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp
{
    /// <summary>
    /// Unit tests for the CSharpAPIProcessorFactory class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class CSharpAPIProcessorFactoryTests
    {
        /// <summary>
        /// The CSharpAPIProcessorFactory instance to use for the tests.
        /// </summary>
        CSharpAPIProcessorFactory capf;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().Clear(false);
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");
            capf = new CSharpAPIProcessorFactory();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ConfigManager.GetInstance().Clear(false);
            capf = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// CSharpAPIProcessorFactory()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.AreEqual(typeof(CSharpAPIProcessorFactory).BaseType, typeof(AbstractXmlProcessorFactory),
                "Wrong type of class.");
        }

        /// <summary>
        /// Tests the ConfigureCommandLineParser method.
        /// void ConfigureCommandLineParser(CommandLineParser commandLineParser, IConfiguration configuration)
        /// </summary>
        [Test]
        public void TestConfigureCommandLineParser()
        {
            IConfiguration config = UnitTestHelper.GetConfig();
            CommandLineParser clp = new CommandLineParser();

            capf.ConfigureCommandLineParser(clp, config);

            //The switches must be added correctly
            Assert.AreEqual(clp.AvailableSwitches.Count, 5, "Incorrect number of switches added.");

            string[] switchNames = new string[] {
                "assemblies", "docFiles", "modules", "documentPrivates", "typePrefixes" };

            for (int i = 0; i < 5; i++)
            {
                bool found = false;
                IEnumerator en = clp.AvailableSwitches.GetEnumerator();
                while (en.MoveNext())
                {
                    CommandLineSwitch cSwitch = en.Current as CommandLineSwitch;
                    if (cSwitch.Switch.Equals(switchNames[i]))
                    {
                        found = true;
                        break;
                    }
                }

                Assert.IsTrue(found, "missing switch: " + switchNames[i]);
            }
        }

        /// <summary>
        /// Tests the ConfigureCommandLineParser method for failure when commandLineParser is null
        /// void ConfigureCommandLineParser(CommandLineParser commandLineParser, IConfiguration configuration)
        /// ArgumentNullException is expeted.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConfigureCommandLineParserFail1()
        {
            capf.ConfigureCommandLineParser(null, UnitTestHelper.GetConfig());
        }

        /// <summary>
        /// Tests the ConfigureCommandLineParser method for failure when configuration is null
        /// void ConfigureCommandLineParser(CommandLineParser commandLineParser, IConfiguration configuration)
        /// ArgumentNullException is expeted.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConfigureCommandLineParserFail2()
        {
            capf.ConfigureCommandLineParser(new CommandLineParser(), null);
        }

        /// <summary>
        /// Tests the NewXmlProcessor method.
        /// IXmlProcessor NewXmlProcessor(CommandLineSwitch[] inputtedSwitches, IConfiguration configuration)
        /// </summary>
        [Test]
        public void TestNewXmlProcessor()
        {
            //Get the switches
            IConfiguration config = UnitTestHelper.GetConfig();
            CommandLineParser clp = new CommandLineParser();
            capf.ConfigureCommandLineParser(clp, config);

            List<CommandLineSwitch> lst = new List<CommandLineSwitch>();
            IEnumerator en = clp.AvailableSwitches.GetEnumerator();
            while (en.MoveNext())
            {
                lst.Add(en.Current as CommandLineSwitch);
            }
            IXmlProcessor xmlProc = capf.NewXmlProcessor(lst.ToArray(), config);

            Assert.AreEqual(xmlProc.GetType(), typeof(CSharpAPIProcessor), "Wrong return type.");
            CSharpAPIProcessor csProc = (CSharpAPIProcessor)xmlProc;

            ReflectionEngineParameters rep = (ReflectionEngineParameters)
                UnitTestHelper.GetPrivateField(csProc, "rep");
            Assert.IsNotNull(rep, "ReflectionEngineParameters instance not set properly.");
        }

        /// <summary>
        /// Tests the NewXmlProcessor method for failure when inputtedSwitches is null.
        /// IXmlProcessor NewXmlProcessor(CommandLineSwitch[] inputtedSwitches, IConfiguration configuration)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestNewXmlProcessorFail1()
        {
            capf.NewXmlProcessor((CommandLineSwitch[])null, UnitTestHelper.GetConfig());
        }

        /// <summary>
        /// Tests the NewXmlProcessor method for failure when configuration is null.
        /// IXmlProcessor NewXmlProcessor(CommandLineSwitch[] inputtedSwitches, IConfiguration configuration)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestNewXmlProcessorFail2()
        {
            capf.NewXmlProcessor(new CommandLineSwitch[] { new CommandLineSwitch("a") }, null);
        }

        /// <summary>
        /// Tests the NewXmlProcessor method for failure when inputtedSwitches has null element.
        /// IXmlProcessor NewXmlProcessor(CommandLineSwitch[] inputtedSwitches, IConfiguration configuration)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestNewXmlProcessorFail3()
        {
            capf.NewXmlProcessor(new CommandLineSwitch[] { null }, UnitTestHelper.GetConfig());
        }

        /// <summary>
        /// Tests the NewXmlProcessor method for failure when configuration is invalid.
        /// IXmlProcessor NewXmlProcessor(CommandLineSwitch[] inputtedSwitches, IConfiguration configuration)
        /// XmlProcessorFactoryException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(XmlProcessorFactoryException))]
        public void TestNewXmlProcessorFail4()
        {
            IConfiguration config = UnitTestHelper.GetConfig();
            config["processor_factory_config"].SetAttribute("reference_paths", new string[] { "" });
            capf.NewXmlProcessor(new CommandLineSwitch[] { new CommandLineSwitch("abc") },
                config["processor_factory_config"]);
        }
    }
}
