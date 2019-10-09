/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 
using System;
using System.Runtime.Serialization;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// <para>This exception signals issues with post-processing of a layouting request.</para>
    /// </summary>
    /// <threadsafety>
    /// Since this class ultimately derives from a non thread safe class (ApplicationException), it is not thread safe.
    /// </threadsafety>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public class LayoutPostProcessingException : LayoutException
    {

        /// <summary>
        /// <para>Creates a new LayoutPostProcessingException instance</para>
        /// </summary>
        public LayoutPostProcessingException() : base()
        {
        }

        /// <summary>
        /// <para>
        /// Initializes a new instance of this class with a specified error message.</para>
        /// </summary>
        /// <param name="message">exception/error message.</param>
        public LayoutPostProcessingException(String message) : base(message)
        {
        }

        /// <summary>
        /// <para>Constructor initialized with an error message and the inner cause exception</para>
        /// </summary>
        /// <param name="message">exception/error message.</param>
        /// <param name="cause">the cause exception to be chained</param>
        public LayoutPostProcessingException(String message, Exception cause) : base(message, cause)
        {
        }

        /// <summary>
        /// <para>Serialization constructor. Initializes a new instance of this class with
        /// the specified serialization and context information.</para>
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected LayoutPostProcessingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }
}
