// BindDataInvalidException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>This exception will describe bad data in the data source object to be binded to the list control.  The data
    /// from external data source (for example, XML file) can be binded to this control. If any entry of binded data
    /// becomes invalid (type is not DateTime or String, or the parsing of text string produced an exception, etc.),
    /// then this  exception will be thrown.</para>
    /// <threadsafety>
    /// Since this exception derives from a non thread-safe exception, this exception is not itself thread safe.
    /// </threadsafety>
    /// </summary>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class BindDataInvalidException : DateDropDownException
    {

        /// <summary>
        /// <para>The default constructor. Creates a new exception without any additional information.</para>
        /// </summary>
        public BindDataInvalidException() : base()
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        public BindDataInvalidException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance with the given error message and the given cause.</para>
        /// </summary>
        /// <param name="message">Explanation of the error. Can be empty string or null (useless, but allowed).</param>
        /// <param name="cause">
        /// Underlying cause of the error. Can be null, which means that initial exception is nonexistent or unknown.
        /// </param>
        public BindDataInvalidException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>
        /// <para>Create a new exception instance (used by serialization).</para>
        /// </summary>
        /// <param name="info">the object that holds the serialized object data. Can not be null.</param>
        /// <param name="context">the contextual information about the source or destination. Can not be null.</param>
        protected BindDataInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
