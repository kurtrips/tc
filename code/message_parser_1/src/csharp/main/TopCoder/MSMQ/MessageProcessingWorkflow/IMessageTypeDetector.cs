// IMessageTypeDetector.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.Util.DataValidator;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is a simple contract interface for detecting message types from a
    /// specific input message. Each detector will specialize in a specific format (such as Xml, Csv, etc.) This
    /// contract also provides for provisioning of the actual recipes for the message type that the parser will use when
    /// it needs to parse out the input message.</p>
    /// <p><strong>Thread-Safety:</strong></p> <p>Implementations should be thread-safe.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IMessageTypeDetector
    {
        /// <summary>
        /// <p>This method specifies a contract for matching a message type (string) to
        /// a specific input message (string). The basic idea is that a detector specializes in figuring out the type of
        /// the message.</p>
        /// </summary>
        /// <param name="messageText">the actual message for which we want to get the type</param>
        /// <returns>the type of message</returns>
        /// <exception cref="ArgumentNullException">If messageText is null.</exception>
        /// <exception cref="ArgumentException">If messageText is empty.</exception>
        /// <exception cref="UnknownMessageTypeException">
        /// If the actual type cannot be obtained from the input or the type cannot be matched
        /// </exception>
        /// <exception cref="UnknownMessageFormatException">If the format of messageText is unknown/wrong.</exception>
        string GetMessageType(string messageText);

        /// <summary>
        /// <p>This is a contract method which will return an object representing the
        /// specific instance of parsing recipe for the specific message type. For example, for Xml format the return
        /// might be simply a Node element or a whole Document.</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">If messageType is null.</exception>
        /// <exception cref="ArgumentException">If messageType is empty.</exception>
        /// <exception cref="UnknownMessageTypeException">If messageType is not recognized.</exception>
        /// <param name="messageType">message type</param>
        /// <returns>An object representing the parsing information needed by the parser to parse messages of this type
        /// </returns>
        object GetMessageTypeParseRecipe(string messageType);

        /// <summary>
        /// <p>This will return a validator available for the specific message type.
        /// Callers would then use such a validator to validate the specific message.</p>
        /// </summary>
        /// <exception cref="ArgumentNullException">If messageType is null.</exception>
        /// <exception cref="ArgumentException">If messageType is empty.</exception>
        /// <param name="messageType">the actual type of the message.</param>
        /// <returns>a validator for this message type if one is set; null otherwise</returns>
        IValidator GetValidator(string messageType);

        /// <summary>
        /// <p>This method returns whether the current detector
        /// can handle the message passed in i.e. it can parse out the message type from the message.</p>
        /// </summary>
        /// <param name="message">The message string.</param>
        /// <exception cref="ArgumentNullException">If the input is null.</exception>
        /// <exception cref="ArgumentException">If the input is empty string.</exception>
        /// <exception cref="UnknownMessageFormatException">If the input is not valid xml.</exception>
        /// <returns>whether the current detector can handle the message passed </returns>
        bool CanHandleMessage(string message);

    }
}
