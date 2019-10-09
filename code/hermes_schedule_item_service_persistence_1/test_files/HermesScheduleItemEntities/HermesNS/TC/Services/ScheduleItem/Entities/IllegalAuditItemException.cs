// IllegalAuditItemException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;
using TopCoder.Util.ExceptionManager.SDE;

namespace HermesNS.TC.Services.ScheduleItem.Entities
{
    /// <summary>
    /// This exception is thrown by the Audit method in all entities if the passed item is the same object as this
    /// object or is of the wrong type. It extends SelfDocumentingException.
    /// </summary>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2006, TopCoder, Inc. All rights reserved.</copyright>
    public class IllegalAuditItemException : SelfDocumentingException
    {
        /// <summary>
        /// <p>Initializes a new instance of this class with its message string set to a system-supplied message.</p>
        /// </summary>
        public IllegalAuditItemException() : base()
        {
        }

        /// <remarks>22</remarks>
        /// <summary><p>Initializes a new instance of this class with a specified error message.</p></summary>
        /// <param name="message">A string message that describes the error.</param>
        public IllegalAuditItemException(string message) : base(message)
        {
        }

        /// <remarks>30</remarks>
        /// <summary>
        /// <p>Initializes a new instance of this class with a specified error message and a reference to the inner
        /// exception that is the cause of this exception.</p>
        /// </summary>
        /// <param name="message">A string message that describes the error.</param>
        /// <param name="cause">The exception that is the cause of the current exception.</param>
        public IllegalAuditItemException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <remarks>38</remarks>
        /// <summary>
        /// <p>Initializes a new instance of this class with the specified serialization and context information.</p>
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected IllegalAuditItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
