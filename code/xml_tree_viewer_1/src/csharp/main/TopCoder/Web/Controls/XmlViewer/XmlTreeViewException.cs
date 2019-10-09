// XmlTreeViewException.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Runtime.Serialization;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// This exception is the base class for all custom exceptions in this component. This exception isn't thread safe,
    /// as it inherits the mutability of its parent class, but it will be used in a thread safe manner. The
    /// constructors in this exception wrap the constructors of the base class.
    /// </summary>
    /// <author>Ghostar</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class XmlTreeViewException : ApplicationException
    {
        /// <summary>Creates a new, blank exception</summary>
        public XmlTreeViewException()
        {
        }

        /// <summary>Creates a new exception with the given message</summary>
        /// <param name="message">Message this exception will contain</param>
        public XmlTreeViewException(string message) : base(message)
        {
        }

        /// <summary>Creates a new exception with the given message and cause</summary>
        /// <param name="message">Message this exception will contain</param>
        /// <param name="cause">Cause of this exception</param>
        public XmlTreeViewException(string message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>Serialization constructor for this exception:</summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected XmlTreeViewException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
