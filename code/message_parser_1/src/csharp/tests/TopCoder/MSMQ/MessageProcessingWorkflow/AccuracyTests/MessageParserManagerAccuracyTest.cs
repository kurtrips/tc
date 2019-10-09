using System;
using TopCoder.Util.ConfigurationManager;
using TopCoder.MSMQ.MessageProcessingWorkflow.Parsers;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// Accuracy tests for the MessageParserManager class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class MessageParserManagerAccuracyTest
    {
        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager configMan = ConfigManager.GetInstance();
            configMan.Clear(false);
            configMan.LoadFile("../../test_files/accuracyTests/MessageParserMainConfig.xml");
            configMan.LoadFile("../../test_files/accuracyTests/MessageParserObjectDefinitionsConfig.xml");
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Tests the public variable for accuracy.
        /// </summary>
        [Test]
        public void TestClass()
        {
            Assert.AreEqual(MessageParserManager.DefaultMessageParserName, "XmlMessageParser");
        }

        /// <summary>
        /// Tests the GetParser method.
        /// IMessageParser GetParser()
        /// </summary>
        [Test]
        public void TestGetParser1()
        {
            IMessageParser mp = MessageParserManager.GetParser();
            Assert.IsTrue(mp is XmlMessageParser, "Parser returned has wrong type.");
        }

        /// <summary>
        /// Tests the GetParser method. Note as there is no class CsvMessageParser, this just returns the instance
        /// of XmlMessageParser.
        /// IMessageParser GetParser(string name)
        /// </summary>
        [Test]
        public void TestGetParser2()
        {
            IMessageParser mp = MessageParserManager.GetParser("CsvMessageParser");
            Assert.IsTrue(mp is XmlMessageParser, "Parser returned has wrong type.");
        }

        /// <summary>
        /// Tests the RefreshConfiguration method.
        /// void RefreshConfiguration(string name)
        /// </summary>
        [Test]
        public void TestRefreshConfiguration()
        {
            MessageParserManager.GetParser("XmlMessageParser");
            MessageParserManager.RefreshConfiguration("XmlMessageParser");
        }
    }
}