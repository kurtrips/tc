// VerificationFailedException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>This is the exception used to signal to the user that the verification of the provided signature has
    /// failed.</summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class VerificationFailedException : ApplicationException
    {
        /// <summary>
        /// <p>Purpose: Constructs this exception without a message or inner exception.</p>
        /// </summary>
        public VerificationFailedException() : base()
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message but without an inner exception.</p>
        /// </summary>
        /// <param name="message">The exception message</param>
        public VerificationFailedException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message and inner exception.</p>
        /// </summary>
        /// <param name="inner">The inner exception instance</param>
        /// <param name="message">The exception message</param>
        public VerificationFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
