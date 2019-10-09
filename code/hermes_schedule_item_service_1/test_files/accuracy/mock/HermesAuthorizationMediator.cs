/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System.Reflection;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// Mock implementation of <see cref="HermesAuthorizationMediator"/> for testing purpose.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    public static class HermesAuthorizationMediator
    {
        /// <summary>
        /// <para>
        /// Mock implementation.
        /// </para>
        /// </summary>
        /// <param name="applicationId">application id.</param>
        /// <param name="sessionID">session id.</param>
        /// <param name="sessionToken">session token.</param>
        /// <param name="methodBase">method base.</param>
        public static void MediateMethod(string applicationId, string sessionID,
            string sessionToken, MethodBase methodBase)
        {
        }
    }
}
