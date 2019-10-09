// DigesterException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This exception class wraps up any exception that is thrown during the Digesting process.</p>
    /// <p><b>Thread Safety: </b>This class is thread-safe</p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DigesterException : ApplicationException
    {
        /// <summary>
        /// <p>Purpose: Constructs this exception without a message or inner exception.</p>
        /// </summary>
        public DigesterException() : base()
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message but without an inner exception.</p>
        /// </summary>
        /// <param name="message">The exception message</param>
        public DigesterException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message and inner exception.</p>
        /// </summary>
        /// <param name="inner">The inner exception instance</param>
        /// <param name="message">The exception message</param>
        public DigesterException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
