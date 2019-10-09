// ConfigurationException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <p>This is a simple exception, which are used when there is an issue with configuration.
    /// This exception would be used to wrap any exception that resulted from using Configuration Manager
    /// or Object factory or reading a filed (I/O exception)</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class ConfigurationException : ApplicationException
    {
        /// <summary><p>Create a new exception instance.</p></summary>
        public ConfigurationException() : base()
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        public ConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance with the given error message and the given cause.</p>
        /// </summary>
        /// <param name="message">the message describing the exception</param>
        /// <param name="innerException">the cause of the exception</param>
        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// <p>Create a new exception instance (used by serialization).</p>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data</param>
        /// <param name="context">the contextual information about the source or destination</param>
        protected  ConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
