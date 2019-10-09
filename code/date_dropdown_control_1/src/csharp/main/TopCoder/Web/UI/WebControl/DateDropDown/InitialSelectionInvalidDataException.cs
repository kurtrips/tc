// InitialSelectionInvalidDataException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>This exception will describe bad data in the properties used for setting initial selection of the list.  The
    /// initial selection of the control can be set by using InitialSelection, InitialSelectionTimeStamp,
    /// InitialSelectionDateFormat properties. If any data in such properties becomes invalid (absent required property,
    /// or exception during  parsing, or incorrect numeric value, etc.), then this exception will be thrown.</para>
    /// </summary>
    /// <threadsafety>
    /// Since this exception derives from a non thread-safe exception, this exception is not itself thread safe.
    /// </threadsafety>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class InitialSelectionInvalidDataException : DateDropDownException
    {
        /// <summary>
        /// <para>The default constructor. Creates a new exception without any additional information.</para>
        /// </summary>
        public InitialSelectionInvalidDataException() : base()
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        public InitialSelectionInvalidDataException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message and the given cause.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        /// <param name="cause">
        /// Underlying cause of the error. Can be null, which means that initial exception is nonexistent or unknown.
        /// </param>
        public InitialSelectionInvalidDataException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance (used by serialization).</para>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data. Can not be null.</param>
        /// <param name="context">the contextual information about the source or destination. Can not be null.</param>
        protected InitialSelectionInvalidDataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}
