// TranformerException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This is an exception specific to Transformers and would normally be caught by the manager and then chained in
    /// a  SignatureManagerException or its descendant. It is thrown if there is an issue with processing a
    /// transformation of a reference. </p>
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class TransformerException : ApplicationException
    {
        /// <summary>
        /// <p>Purpose: Constructs this exception without a message or inner exception.</p>
        /// </summary>
        public TransformerException() : base()
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message but without an inner exception.</p>
        /// </summary>
        /// <param name="message">The exception message</param>
        public TransformerException(string message) : base(message)
        {
        }

        /// <summary>
        /// <p>Purpose: Constructs this exception with the given message and inner exception.</p>
        /// </summary>
        /// <param name="inner">The inner exception instance</param>
        /// <param name="message">The exception message</param>
        public TransformerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
