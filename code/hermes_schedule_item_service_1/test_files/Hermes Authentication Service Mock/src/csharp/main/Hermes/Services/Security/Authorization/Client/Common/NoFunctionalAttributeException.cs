/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Reflection;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// <para>
    /// The excepton is thrown by <see cref="HermesAuthorizationMediator"/>,
    /// if it can not find functinal attribute.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is not thread safe.
    /// </threadsafety>
    ///
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    public class NoFunctionalAttributeException : Exception
    {
        /// <summary>
        /// <para>
        /// Represents the method info.
        /// </para>
        /// </summary>
        private MethodBase _MethodBase = null;

        /// <summary>
        /// <para>
        /// Gets the method info.
        /// </para>
        /// </summary>
        /// <value>
        /// The method info.
        /// </value>
        public MethodBase Method
        {
            get
            {
                return _MethodBase;
            }
        }

        /// <summary>
        /// <para>
        /// Create new <see cref="NoFunctionalAttributeException"/> with
        /// <paramref name="info"/>.
        /// </para>
        /// </summary>
        /// <param name="info"></param>
        public NoFunctionalAttributeException(MethodBase info)
        {
            _MethodBase = info;
        }

        /// <summary>
        /// <para>
        /// Create new <see cref="NoFunctionalAttributeException"/> with
        /// <paramref name="info"/> and <paramref name="message"/>.
        /// </para>
        /// </summary>
        /// <param name="info">
        /// The method info.
        /// </param>
        /// <param name="message">
        /// error message.
        /// </param>
        public NoFunctionalAttributeException(MethodBase info, string message)
            : base(message)
        {
            _MethodBase = info;
        }

        /// <summary>
        /// <para>
        /// Create new <see cref="NoFunctionalAttributeException"/> with
        /// <paramref name="message"/>.
        /// </para>
        /// </summary>
        /// <param name="message">
        /// error message.
        /// </param>
        public NoFunctionalAttributeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// <para>
        /// Create new <see cref="NoFunctionalAttributeException"/>.
        /// </para>
        /// </summary>
        public NoFunctionalAttributeException()
        {
        }
    }
}
