// SignatureManagerException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This exception class wraps up any exception that is thrown inside the sign method of the SignatureManager class</p>
    /// <p><b>Thread Safety: </b>This class is thread-safe</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SignatureManagerException : ApplicationException
    {
        /// <summary>
        /// <p>Purpose: Constructs this exception without a message or inner exception.</p>
        /// </summary>
        public SignatureManagerException() : base()
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message but without an inner exception.</p>
        /// </summary>
        /// <param name="message">The exception message</param>
        public SignatureManagerException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message and inner exception.</p>
        /// </summary>
        /// <param name="inner">The inner exception instance</param>
        /// <param name="message">The exception message</param>
        public SignatureManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
