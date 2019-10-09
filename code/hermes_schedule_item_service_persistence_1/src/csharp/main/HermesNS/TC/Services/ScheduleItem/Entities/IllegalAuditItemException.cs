// IllegalAuditItemException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;
using TopCoder.Util.ExceptionManager.SDE;

namespace HermesNS.TC.Services.ScheduleItem.Entities
{
    /// <summary>
    /// This exception is thrown by the Audit method in all entities if the passed item is the same object as this
    /// object. It extends SelfDocumentingException.
    /// </summary>
    /// <threadsafety>
    /// Since this class indirectly derives from a non thread-safe(ApplicationException), it is not thread safe.
    /// </threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class IllegalAuditItemException : SelfDocumentingException
    {
        /// <summary>
        /// <para>
        /// Initializes a new instance of this class with its message string set to a system-supplied message.
        /// </para>
        /// </summary>
        public IllegalAuditItemException() : base()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of this class with a specified error message.</para>
        /// </summary>
        /// <param name="message">A string message that describes the error.</param>
        public IllegalAuditItemException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of this class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.</para>
        /// </summary>
        /// <param name="message">A string message that describes the error.</param>
        /// <param name="cause">The exception that is the cause of the current exception.</param>
        public IllegalAuditItemException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>
        /// <para>
        /// Initializes a new instance of this class with the specified serialization and context information.
        /// </para>
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected IllegalAuditItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
