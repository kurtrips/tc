/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;

using System;
using TopCoder.MSMQ.ConversationManager.DataAccess;
using TopCoder.MSMQ.MessageProcessingWorkflow.Detectors;
using TopCoder.Util.ConfigurationManager;
using System.IO;
using TopCoder.MSMQ.ConversationManager.Entities;

namespace TopCoder.MSMQ.MessageProcessingWorkflow.Parsers
{
    /// <summary>
    /// <p>Accuracy tests for the <c>XmlMessageParser</c> class.</p>
    /// </summary>
    ///
    /// <author>tianniu</author>
    ///
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved
    /// </copyright>
    ///
    /// <version>
    /// 1.0
    /// </version>
    [TestFixture]
    public class XmlMessageParserAccuracyTest
    {
        /// <summary>
        /// Represent the instance of <c> ConfigManager</c> used to test.
        /// </summary>
        private readonly ConfigManager cm = ConfigManager.GetInstance();

        /// <summary>
        /// Keep the instance of <c>XmlMessageParser</c> for test.
        /// </summary>
        private XmlMessageParser parser;

        /// <summary>
        /// <p>
        ///  Represent the config file used to test.
        /// </p>
        /// </summary>
        private const string CONF = "../../test_files/accuracyTests/Config.xml";

        /// <summary>
        /// Represent the configParameters  used to test.
        /// </summary>
        private IList<IMessageTypeDetector> configParameters;


        /// <summary>
        /// Represent a message used for test.
        /// </summary>
        private XmlDocument xmlMessage;

        /// <summary>
        /// Represent the persistence  used to test.
        /// </summary>
        private ConversationManagerPersistence persistence;

        /// <summary>
        /// <p> Set up test environment.</p>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            cm.Clear(false);
            cm.LoadFile(CONF);
            cm.LoadFile("../../test_files/accuracyTests/ObjectFactory.xml");
            parser = new XmlMessageParser("TopCoder.MSMQ.MessageProcessingWorkflow.Parsers.XmlMessageParser");
            persistence = new ConversationManagerPersistence();
            configParameters = new List<IMessageTypeDetector>();
            configParameters.Add(new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector"));
            xmlMessage = new XmlDocument();
        }

        /// <summary>
        /// Clears the Config Manager of any namespaces.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            cm.Clear(false);
        }

        /// <summary>
        /// Accuracy test for the constructor <c>XmlMessageParser(string)</c>.
        /// The instance of <c>XmlMessageParser</c> should be created successfully.
        /// </summary>
        [Test]
        public void TestConstructorAccuracy()
        {
            Assert.IsNotNull(new XmlMessageParser("TopCoder.MSMQ.MessageProcessingWorkflow.Parsers.XmlMessageParser"),
                "create fails");
        }

        /// <summary>
        /// Accuracy test for the constructor <c>XmlMessageParser(IConversationManagerPersistence,IList)</c>.
        /// The instance of <c>XmlMessageParser</c> should be created successfully.
        /// </summary>
        [Test]
        public void TestConstructorAccuracy2()
        {
            //Assert.IsNotNull(new XmlMessageParser(persistence, configParameters), "create fails");
            Assert.IsNotNull(new XmlMessageParser(configParameters), "create fails");
        }

        /// <summary>
        /// Tests the ParseMessage method.
        /// Message ParseMessage(string messageText)
        /// </summary>
        [Test]
        public void TestParseMessage()
        {
            string messageText = File.ReadAllText("../../test_files/accuracyTests/MessageParse.xml");
            Message message = parser.ParseMessage(messageText);
            Assert.AreEqual(messageText, message.DisplayMessage, "DisplayMessage of returned message is incorrect");
            Assert.AreEqual(DateTime.Now.Date, message.Received.Date, "Received of returned message is incorrect");
            Assert.AreEqual("Source Queue", message.SourceQueue.Name, "SourceQueue of returned message is incorrect");
            Assert.AreEqual("MyDescription", message.SourceQueue.Description,
                "SourceQueue of returned message is incorrect");
            Assert.AreEqual("Mypath", message.SourceQueue.Path, "SourceQueue of returned message is incorrect");
            Assert.IsNull(message.DestinationQueue, "DestinationQueue is incorrect.");
            Assert.AreEqual("SRequest", message.Type.Name, "DestinationQueue is incorrect.");
        }

        /// <summary>
        /// Accuracy test the method <c>Clone()</c> .
        /// the <c>parser</c> should be returned.
        /// </summary>
        [Test]
        public void TestCloneAccuracy()
        {
            Assert.AreNotEqual(parser, parser.Clone(), "the parser should be returned.");
        }
    }
}