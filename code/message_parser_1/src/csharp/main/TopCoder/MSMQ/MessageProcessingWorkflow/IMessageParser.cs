// IMessageParser.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.MSMQ.ConversationManager.Entities;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is a general parsing contract interface. It is quite simple: we accept a string message and we parse it
    /// and we attempt to create a Massage (entity) instance based on this input.
    /// We normally create instances of this interface through the MessageParserManager, which acts as a factory
    /// for named parsers.</p>
    /// <para>Each parser is responsible
    /// for parsing a specific medium (such as Xml or CSV) and is highly configurable through the use of message type
    /// recipes. Basically each input message is a single message of a specific message type. Each type will have a
    /// configured parsing recipe, which are a configuration file that outlines (dynamically) how to parse
    /// out/extract data.</para>
    /// <p><strong>Thread-Safety:</strong></p> <p>Implementation must be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IMessageParser : ICloneable
    {
        /// <summary>
        /// <p>This is the contract for parsing a message based solely on the provided text.</p>
        /// </summary>
        /// <param name="messageText">text message to be parsed</param>
        /// <returns>the actual Message instance that was parsed out</returns>
        /// <exception cref="UnknownMessageFormatException">messageText is not valid format.</exception>
        /// <exception cref="ArgumentNullException">If message is null</exception>
        /// <exception cref="ArgumentException">If message is empty</exception>
        /// <exception cref="MessageValidationException">
        /// If one of the classes of DataEntities throws exception at their constructors.
        /// If the type specified in Attribute node either does not exist or the value parsed cannot be converted.\
        /// Validation based on a validator fails.
        /// </exception>
        /// <exception cref="MessageParsingException">If messageText could not be parsed.</exception>
        Message ParseMessage(string messageText);
    }
}
