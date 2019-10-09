// DateDropDownException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>A common parent for the all custom exceptions of the component. It can help if a new exception added later.
    /// The code that catches the common parent exception will still be good.  Also it wraps some external exceptions
    /// not related directly to this component.</para>
    /// <threadsafety>
    /// Since this exception derives from a non thread-safe exception, this exception is not itself thread safe.
    /// </threadsafety>
    /// </summary>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class DateDropDownException : ApplicationException
    {

        /// <summary>
        /// <para>The default constructor. Creates a new exception without any additional information.</para>
        /// </summary>
        public DateDropDownException() : base()
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        public DateDropDownException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message and the given cause.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        /// <param name="cause">
        /// Underlying cause of the error. Can be null, which means that initial exception is nonexistent or unknown.
        /// </param>
        public DateDropDownException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance (used by serialization).</para>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data. Can not be null.</param>
        /// <param name="context">the contextual information about the source or destination. Can not be null.</param>
        protected DateDropDownException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
