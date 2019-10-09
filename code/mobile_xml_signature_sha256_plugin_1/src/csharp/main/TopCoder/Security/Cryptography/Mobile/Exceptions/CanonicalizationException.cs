// CanonicalizationException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This exception class wraps up any exception that is thrown during the Canonicalization process.</p>
    /// <p><b>Thread Safety: </b>This class is thread-safe</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class CanonicalizationException : ApplicationException
    {
        /// <summary>
        /// <p>Purpose: Constructs this exception without a message or inner exception.</p>
        /// </summary>
        public CanonicalizationException() : base()
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message but without an inner exception.</p>
        /// </summary>
        /// <param name="message">The exception message</param>
        public CanonicalizationException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message and inner exception.</p>
        /// </summary>
        /// <param name="inner">The inner exception instance</param>
        /// <param name="message">The exception message</param>
        public CanonicalizationException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
