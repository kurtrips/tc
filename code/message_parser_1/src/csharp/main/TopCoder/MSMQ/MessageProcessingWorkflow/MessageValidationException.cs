// MessageValidationException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is a specialized exception, which are used to signal to the callers that a parsed in
    /// message is not valid.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class MessageValidationException : ApplicationException
    {
        /// <summary><p>Create a new exception instance.</p></summary>
        public MessageValidationException() : base()
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        public MessageValidationException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message and the given cause.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        /// <param name="innerException">the cause of the exception</param>
        public MessageValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance (used by serialization)</p>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data</param>
        /// <param name="context">the contextual information about the source or destination</param>
        protected MessageValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
