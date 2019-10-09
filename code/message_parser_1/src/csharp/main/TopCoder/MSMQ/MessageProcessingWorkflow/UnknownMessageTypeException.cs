// UnknownMessageTypeException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is a very specific type of exception, which signals that a message type cannot
    /// be established. This could be due to the fact that the component simply do not recognize the specific type
    /// (the component has no detector for it), the component has no configured recipe for it,
    /// that the information needed to detect the type is missing.</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class UnknownMessageTypeException : ApplicationException
    {
        /// <summary><p>Create a new exception instance</p></summary>
        public UnknownMessageTypeException() : base()
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        public UnknownMessageTypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message and the given cause.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        /// <param name="innerException">the cause of the exception</param>
        public UnknownMessageTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance (used by serialization).</p>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data</param>
        /// <param name="context">the contextual information about the source or destination</param>
        protected UnknownMessageTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
