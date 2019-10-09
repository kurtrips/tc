/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Runtime.Serialization;
using Hermes.Services.Security.Authorization.TopCoder;
using TopCoder.Util.ExceptionManager.SDE;
namespace Hermes.Services.Security.Authorization
{

    /// <summary>
    /// <para>
    /// This exception is thrown by <see cref="IAuthorization"/> service
    /// operations if the given session id and session token don't correspond
    /// to a valid and active session.
    /// In <see cref="HermesAuthorizationService"/>, the given session
    /// information is used in an attempt to log into the underlying wrapped
    /// service; if the login attempt fails, an
    /// <see cref="InvalidSessionException"/> is thrown.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// <strong>Thread Safety:</strong>
    /// This class derives from <see cref="SelfDocumentingException"/>,
    /// which is not thread safe, so it is not thread safe.
    /// </para>
    /// </remarks>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [Serializable]
    public class InvalidSessionException : SelfDocumentingException
    {
        /// <summary>
        /// <para>This is the default constructor for this exception.</para>
        /// </summary>
        public InvalidSessionException()
        {
        }

        /// <summary>
        /// <para>This is a single-argument constructor for this exception that
        /// provides a message.</para>
        /// </summary>
        /// <param name="message">
        /// A string representing the message for this exception.
        /// This argument is not checked - it may be null or empty.
        /// </param>
        public InvalidSessionException(String message)
            : base(message)
        {
        }

        /// <summary>
        /// <para>
        /// This is a two-argument constructor for this exception that
        /// provides a message and a cause.
        /// </para>
        /// </summary>
        /// <param name="message">
        /// A string representing the message for this exception. This argument
        /// is not checked - it may be null or empty.
        /// </param>
        /// <param name="cause">
        /// An exception representing the cause of the exception. This argument
        /// is not checked - it may be null.
        /// </param>
        public InvalidSessionException(String message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>
        /// <para>
        /// This is the standard constructor for controlling the serialization
        /// process.
        /// </para>
        /// </summary>
        /// <param name="info">
        /// Stores all the data needed to serialize or deserialize an object.
        /// This argument is not checked for null - it is passed to the
        /// constructor of the base class.
        /// </param>
        /// <param name="context">
        /// Describes the source and destination of a given serialized stream,
        /// and provides an additional caller-defined context.
        /// This argument is a struct, and cannot be null- it is passed to the
        /// constructor of the base class.
        /// </param>
        protected InvalidSessionException(SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
