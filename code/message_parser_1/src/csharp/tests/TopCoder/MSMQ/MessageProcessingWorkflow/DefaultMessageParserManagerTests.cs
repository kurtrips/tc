// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using TopCoder.Util.ConfigurationManager;
using TopCoder.MSMQ.MessageProcessingWorkflow.Parsers;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// Unit tests for the MessageParserManager class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class MessageParserManagerTests
    {
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
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            configMan.Clear();
            configMan = null;
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
        /// Tests the GetParser method for failure when the parser required is not configured.
        /// IMessageParser GetParser(string name)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestGetParserFail1()
        {
            MessageParserManager.GetParser("NoSuchParser");
        }

        /// <summary>
        /// Tests the GetParser method for failure when null is passed.
        /// IMessageParser GetParser(string name)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetParserFail2()
        {
            MessageParserManager.GetParser(null);
        }

        /// <summary>
        /// Tests the GetParser method for failure when empty string is passed.
        /// IMessageParser GetParser(string name)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetParserFail3()
        {
            MessageParserManager.GetParser("  ");
        }

        /// <summary>
        /// Tests the RefreshConfiguration method.
        /// void RefreshConfiguration(string name)
        /// </summary>
        [Test]
        public void TestRefreshConfiguration()
        {
            IMessageParser parser1 = MessageParserManager.GetParser("XmlMessageParser");
            MessageParserManager.RefreshConfiguration("XmlMessageParser");
            IMessageParser parser2 = MessageParserManager.GetParser("XmlMessageParser");
            Assert.IsFalse(object.ReferenceEquals(parser1, parser2), "Incorrect RefreshConfiguration implementation.");
        }

        /// <summary>
        /// Tests the RefreshConfiguration method for failure when null is passed.
        /// void RefreshConfiguration(string name)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestRefreshConfigurationFail1()
        {
            MessageParserManager.RefreshConfiguration(null);
        }

        /// <summary>
        /// Tests the RefreshConfiguration method for failure when empty string is passed.
        /// void RefreshConfiguration(string name)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRefreshConfigurationFail2()
        {
            MessageParserManager.RefreshConfiguration("  ");
        }
    }
}
