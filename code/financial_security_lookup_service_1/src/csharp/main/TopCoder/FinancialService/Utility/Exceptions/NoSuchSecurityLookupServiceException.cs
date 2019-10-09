// NoSuchSecurityLookupServiceException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para>This exception extends the FinancialSecurityException, and it is thrown from FinancialSecurityManager if
    /// there is no corresponding ISecurityLookupService defined for the specific security identifier type.  </para>
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
    public class NoSuchSecurityLookupServiceException : FinancialSecurityException
    {
        /// <summary><para>Empty constructor.</para></summary>
        public NoSuchSecurityLookupServiceException()
            : base()
        {
        }

        /// <summary><para>Constructor with error message. </para></summary>
        /// <param name="message">the error message.</param>
        public NoSuchSecurityLookupServiceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <para>Constructor with error message and inner exception. </para>
        /// </summary>
        /// <param name="message">the error message.</param>
        /// <param name="innerException">the inner exception.</param>
        public NoSuchSecurityLookupServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary><para>Constructor for serialization support.</para></summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected NoSuchSecurityLookupServiceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
