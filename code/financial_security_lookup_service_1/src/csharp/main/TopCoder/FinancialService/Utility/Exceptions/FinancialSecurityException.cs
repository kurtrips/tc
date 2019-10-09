// FinancialSecurityException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para>This exception extends the SelfDocumentingException, and it is used as the parent exception class for all
    /// the custom exceptions in this component.</para>
    /// </summary>
    ///
    /// <threadsafety>This class is not thread safe as it indirectly derives from
    /// ApplicationException which is not thread safe</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class FinancialSecurityException : SelfDocumentingException
    {
        /// <summary><para>Empty constructor.</para></summary>
        public FinancialSecurityException() : base()
        {
        }

        /// <summary><para>Constructor with error message</para></summary>
        /// <param name="message">the error message.</param>
        public FinancialSecurityException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Constructor with error message and inner exception</para>
        /// </summary>
        /// <param name="message">the error message.</param>
        /// <param name="innerException">the inner exception.</param>
        public FinancialSecurityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary><para>Constructor for serialization support.</para></summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected FinancialSecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
