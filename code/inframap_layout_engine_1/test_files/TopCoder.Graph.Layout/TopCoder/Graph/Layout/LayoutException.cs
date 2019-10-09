// LayoutException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// <p>The mock LayoutException class</p>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class LayoutException : SelfDocumentingException
    {
        /// <summary>
        /// <p>This is a simple default exception.</p>
        /// </summary>
        public LayoutException()
            : base()
        {
        }

        /// <summary>
        /// <p>Initializes a new instance of this class with a specified error message.</p>
        /// </summary>
        /// <param name="message">exception/error message.</param>
        public LayoutException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// <p>Constructor initialized with an error message and the inner cause exception</p>
        /// </summary>
        /// <param name="message">exception/error message.</param>
        /// <param name="cause">the cause exception to be chained</param>
        public LayoutException(String message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        /// <p>Serialization constructor. Initializes a new instance of this class with
        /// the specified serialization and context information.</p>
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected LayoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
