// UnknownMessageFormatException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This exception is thrown when the format of the message is unrecognized.
    /// This could be happening during parsing or during message detection operation.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class UnknownMessageFormatException : ApplicationException
    {
        /// <summary><p>Create a new exception instance.</p></summary>
        public UnknownMessageFormatException() : base()
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        public UnknownMessageFormatException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message and the given cause.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        /// <param name="innerException">the cause of the exception</param>
        public UnknownMessageFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance (used by serialization).</p>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data</param>
        /// <param name="context">the contextual information about the source or destination</param>
        protected UnknownMessageFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
