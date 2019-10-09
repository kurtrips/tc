/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System.ServiceModel;
using HermesNS.Entity.Common;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// <para>
    /// Mock implementation.
    /// </para>
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    public static class WcfHelper
    {
        /// <summary>
        /// Mock implementation.
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>profile instance.</returns>
        public static Profile GetProfileFromContext(OperationContext context)
        {
            Profile profile = new Profile();
            profile.SessionID = "test_seesion_id";
            profile.SessionToken = "test_seesion_token";
            profile.UserID = "user_id";
            profile.UserName = "test_user_name";
            profile.Culture = "test_culture";
            return profile;
        }

        /// <summary>
        /// Mock implementation.
        /// </summary>
        /// <param name="context">context.</param>
        /// <returns>application id.</returns>
        public static string GetApplicationID(OperationContext context)
        {
            return "ApplicationID";
        }
    }
}
