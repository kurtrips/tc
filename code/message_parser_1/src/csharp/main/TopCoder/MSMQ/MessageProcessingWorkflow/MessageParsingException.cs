// MessageParsingException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is a more general exception, which will signal issue with actual parsing of a
    /// message. The parsing exception deals with the actual specifics of parsing such as for example
    /// using Xpath to read data from an Xml Document.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class MessageParsingException : ApplicationException
    {
        /// <summary><p>Create a new exception instance.</p></summary>
        public MessageParsingException() : base()
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        public MessageParsingException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message and the given cause.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        /// <param name="innerException">the cause of the exception</param>
        public MessageParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance (used by serialization)</p>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data</param>
        /// <param name="context">the contextual information about the source or destination</param>
        protected MessageParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
