/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Runtime.Serialization;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// This exception is thrown by IPrioritizer interface implementations to indicate any problem while job comparison.
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is derived from non thread safe class and it is not thread safe also.
    /// </threadsafety>
    ///
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class PrioritizerException : ApplicationException
    {
        /// <summary>
        /// <para>Initializes a new instance of exception</para>
        /// </summary>
        public PrioritizerException() : base()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the exception with a specified error message.</para>
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public PrioritizerException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the exception with a specified error message and a reference
        /// to the inner exception that is the cause of this exception.</para>
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="inner">
        /// The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.
        /// </param>
        public PrioritizerException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the exception with serialized data.</para>
        /// </summary>
        /// <param name="info">
        /// The SerializationInfo that holds the serialized object data about the exception being thrown
        /// </param>
        /// <param name="context">
        /// The StreamingContext that contains contextual information about the source or destination
        /// </param>
        protected PrioritizerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
