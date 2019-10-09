// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System.IO;
using TopCoder.MSMQ.ConversationManager.Entities;
using TopCoder.Util.ConfigurationManager;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// Provides a demonstration of the intended usage of this component.
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class Demo
    {
        /// <summary>
        /// The ConfigManager instance to use for the demo.
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
        /// Provides a demonstration of the intended usage of this component.
        /// </summary>
        [Test]
        public void Demonstration()
        {
            //The amount of code involved in using this component is minimal.
            //More effort is required to set up the configurations correctly.
            //Refer to CS or sample config files in test_files folder for the same

            //Usage:

            //Use static method of MessageParserManager to get a parser instance.
            //Get default instance of parser
            IMessageParser parser = MessageParserManager.GetParser();

            //Or get a custom parser instance as per defined in configuration file, using the overloaded function.
            parser = MessageParserManager.GetParser("XmlMessageParser");

            //Get the message string to parse
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");

            //Get the Message instance
            Message message = parser.ParseMessage(messageText);
        }
    }
}
