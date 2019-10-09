// InvalidXmlException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// <para>This exception is thrown by properties and methods in this component that expect well-formed XML
    /// strings and don't receive that as a parameter.</para>
    /// <para>This exception isn't thread safe, as it inherits the mutability of its parent class,
    /// but it will be used in a thread safe manner.</para>
    /// </summary>
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class InvalidXmlException : XmlTreeViewException
    {
        /// <summary>Creates a new, blank exception</summary>
        public InvalidXmlException()
            : base()
        {
        }

        /// <summary>Creates a new exception with the given message</summary>
        /// <param name="message">Message this exception will contain</param>
        public InvalidXmlException(string message)
            : base(message)
        {
        }

        /// <summary>Creates a new exception with the given message and cause</summary>
        /// <param name="message">Message this exception will contain</param>
        /// <param name="cause">Cause of this exception</param>
        public InvalidXmlException(string message, Exception cause)
            : base(message, cause)
        {
        }

        /// <summary>Serialization constructor for this exception:</summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidXmlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
