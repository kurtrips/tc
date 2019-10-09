// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using System.Collections.Generic;
using TopCoder.Util.ConfigurationManager;
using TopCoder.MSMQ.MessageProcessingWorkflow.Detectors;
using TopCoder.MSMQ.ConversationManager.Entities;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow.Parsers
{
    /// <summary>
    /// Unit tests for the XmlMessageParser class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class XmlMessageParserTests
    {
        /// <summary>
        /// The XmlMessageParser instance to use throughout the tests.
        /// </summary>
        XmlMessageParser parser;

        /// <summary>
        /// The ConfigManager instance to use throughout the tests.
        /// </summary>
        ConfigManager configMan;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            configMan = ConfigManager.GetInstance();
            configMan.Clear();
            configMan.LoadFile("../../test_files/MessageParserMainConfig.xml");
            configMan.LoadFile("../../test_files/MessageParserObjectDefinitionsConfig.xml");

            parser = new XmlMessageParser("message.parser.XmlMessageParser");
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            configMan.Clear();
            configMan = null;

            parser = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlMessageParser(string namespace)
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsNotNull(parser, "Parser instance is null.");
            Assert.IsTrue(parser is IMessageParser, "Parser instance has wrong type.");
        }

        /// <summary>
        /// Tests the constructor when detector for parser is not defined.
        /// XmlMessageParser(string namespace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail1()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.BadDetector");
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlMessageParser(Generic IList configParameters)
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            XmlMessageTypeDetector detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector");
            IList<IMessageTypeDetector> configParameters = new List<IMessageTypeDetector>();
            configParameters.Add(detector);

            parser = new XmlMessageParser(configParameters);
            Assert.IsNotNull(parser, "parser must not be null.");
        }

        /// <summary>
        /// Tests the constructor when configParameters is null.
        /// XmlMessageParser(Generic IList configParameters)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor2_Fail1()
        {
            parser = new XmlMessageParser((IList<IMessageTypeDetector>)null);
        }

        /// <summary>
        /// Tests the constructor when configParameters contains a null element.
        /// XmlMessageParser(Generic IList configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail2()
        {
            IList<IMessageTypeDetector> configParameters = new List<IMessageTypeDetector>();
            configParameters.Add(null);
            parser = new XmlMessageParser(configParameters);
        }

        /// <summary>
        /// Tests the ParseMessage method.
        /// Message ParseMessage(string messageText)
        /// </summary>
        [Test]
        public void TestParseMessage()
        {
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            Message message = parser.ParseMessage(messageText);
            Assert.AreEqual(message.DisplayMessage, messageText, "DisplayMessage of returned message is incorrect");
            Assert.AreEqual(message.Received.Date, DateTime.Now.Date, "Received of returned message is incorrect");
            Assert.AreEqual(message.SourceQueue.Name, "Source Queue", "SourceQueue of returned message is incorrect");
            Assert.AreEqual(message.SourceQueue.Description, "MyDescription",
                "SourceQueue of returned message is incorrect");
            Assert.AreEqual(message.SourceQueue.Path, "Mypath", "SourceQueue of returned message is incorrect");
            Assert.AreEqual(message.DestinationQueue, null, "DestinationQueue is incorrect.");
            Assert.AreEqual(message.Type.Name, "SRequest", "DestinationQueue is incorrect.");
        }

        /// <summary>
        /// Tests the ParseMessage method destinationQueue is not specified in recipe.
        /// Message ParseMessage(string messageText)
        /// </summary>
        [Test]
        public void TestParseMessage2()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParserValid2");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            Message message = parser.ParseMessage(messageText);
            Assert.AreEqual(message.DestinationQueue, null, "DestinationQueue is incorrect.");
        }

        /// <summary>
        /// Tests the ParseMessage method when messageText is null.
        /// Message ParseMessage(string messageText)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestParseMessageFail1()
        {
            parser.ParseMessage(null);
        }

        /// <summary>
        /// Tests the ParseMessage method when messageText is empty string.
        /// Message ParseMessage(string messageText)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestParseMessageFail2()
        {
            parser.ParseMessage("   ");
        }

        /// <summary>
        /// Tests the ParseMessage method when messageText is not valid xml.
        /// Message ParseMessage(string messageText)
        /// UnknownMessageFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownMessageFormatException))]
        public void TestParseMessageFail3()
        {
            parser.ParseMessage("<root>");
        }

        /// <summary>
        /// Tests the ParseMessage method when MessageConfig/MessageType node is not found.
        /// Message ParseMessage(string messageText)
        /// MessageParsingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageParsingException))]
        public void TestParseMessageFail4()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid1");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when XPath attribute for
        /// MessageConfig/MessageType node is not found in recipe.
        /// Message ParseMessage(string messageText)
        /// MessageParsingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageParsingException))]
        public void TestParseMessageFail5()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid2");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when XPath attribute for MessageConfig/MessageType node
        /// is empty in recipe.
        /// Message ParseMessage(string messageText)
        /// MessageParsingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageParsingException))]
        public void TestParseMessageFail6()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid3");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when a parsed attribute value cannot be converted into
        /// the type specified for it.
        /// Message ParseMessage(string messageText)
        /// MessageValidationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageValidationException))]
        public void TestParseMessageFail7()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid4");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when a mandatory attribute is not found.
        /// Message ParseMessage(string messageText)
        /// MessageParsingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageParsingException))]
        public void TestParseMessageFail8()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid5");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when a parsed attribute value cannot be converted into
        /// the type specified for it as the type itself does not exist.
        /// Message ParseMessage(string messageText)
        /// MessageValidationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageValidationException))]
        public void TestParseMessageFail9()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid6");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when a parsed attribute value is empty. This value when passed
        /// to MessageAttribute constructor causes exception and that exception is rethrown.
        /// Message ParseMessage(string messageText)
        /// MessageValidationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageValidationException))]
        public void TestParseMessageFail10()
        {
            string messageText = File.ReadAllText("../../test_files/testMessageInvalid1.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when parsed Message fails validation.
        /// Message ParseMessage(string messageText)
        /// MessageValidationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageValidationException))]
        public void TestParseMessageFail11()
        {
            XmlMessageTypeDetector detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetectorWithValidator");
            IList<IMessageTypeDetector> configParameters = new List<IMessageTypeDetector>();
            configParameters.Add(detector);
            parser = new XmlMessageParser(configParameters);

            string messageText = File.ReadAllText("../../test_files/testMessage2.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when value of InnerCount node is invalid.
        /// Message ParseMessage(string messageText)
        /// MessageParsingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageParsingException))]
        public void TestParseMessageFail12()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Invalid7");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when a parsed attribute value is empty. This value when passed
        /// to MessageAttribute constructor causes exception and that exception is rethrown.
        /// Message ParseMessage(string messageText)
        /// MessageValidationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageValidationException))]
        public void TestParseMessageFail13()
        {
            string messageText = File.ReadAllText("../../test_files/testMessageInvalid2.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when the FindOrCreateMessageType method of persistence
        /// throws exception.
        /// Message ParseMessage(string messageText)
        /// MessageValidationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageValidationException))]
        public void TestParseMessageFail14()
        {
            parser = new XmlMessageParser("message.parser.XmlMessageParser.Unidentified");
            string messageText = File.ReadAllText("../../test_files/testMessageInvalid3.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the ParseMessage method when the SourceMessageQueue name parsed from message
        /// is empty.
        /// Message ParseMessage(string messageText)
        /// MessageParsingException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(MessageParsingException))]
        public void TestParseMessageFail15()
        {
            string messageText = File.ReadAllText("../../test_files/testMessageInvalid4.xml");
            parser.ParseMessage(messageText);
        }

        /// <summary>
        /// Tests the Clone method.
        /// Object Clone()
        /// </summary>
        [Test]
        public void TestClone()
        {
            object clone = parser.Clone();
            Assert.IsTrue(clone is XmlMessageParser, "Clone returns wrong instance.");
            Assert.IsFalse(object.ReferenceEquals(clone, parser), "Clone returns the same reference.");
        }
    }
}
