// RuleInvalidDataException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>This exception will describe bad data in the properties used for setting rule of the date generation pattern.
    /// The rule of date generation pattern can be set by using Rule property. If any data in sub-properties of this
    /// property becomes invalid (absent required property, or exception during  parsing, or incorrect numeric value,
    /// etc.), then this exception will be thrown.</para>
    /// <threadsafety>
    /// Since this exception derives from a non thread-safe exception, this exception is not itself thread safe.
    /// </threadsafety>
    /// </summary>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class RuleInvalidDataException : DateDropDownException
    {
        /// <summary>
        /// <para>The default constructor. Creates a new exception without any additional information.</para>
        /// </summary>
        public RuleInvalidDataException() : base()
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        public RuleInvalidDataException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message and the given cause.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        /// <param name="cause">
        /// Underlying cause of the error. Can be null, which means that initial exception is nonexistent or unknown.
        /// </param>
        public RuleInvalidDataException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance (used by serialization).</para>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data. Can not be null.</param>
        /// <param name="context">the contextual information about the source or destination. Can not be null.</param>
        protected RuleInvalidDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
