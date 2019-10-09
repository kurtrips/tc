using System;
using System.Collections.Generic;
using System.Text;

using HermesNS.Entity.Common;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    public static class WcfHelper
    {
        // TODO: Define these values in the Component Spec
        // This is a static class and unless we want to use 
        // Configuration Manager in the static constructor then
        // we need to simply hard code and document well.
        private static readonly string SESSION_NS = "session_ns";
        private static readonly string SESSION_ID_NAME = "session_id";
        private static readonly string SESSION_TOKEN_NAME = "session_token";
        private static readonly string SESSION_USER_ID_NAME = "session_user_id";
        private static readonly string SESSION_USERNAME_NAME = "session_username";
        private static readonly string SESSION_CULTURE_NAME = "session_culture";
        private static readonly string SESSION_APPLICATION_ID_NAME = "session_application_id";

        public static Profile GetProfileFromContext(System.ServiceModel.OperationContext context)
        {
            Profile profile = new Profile();
            if (context != null)
            {
                profile.SessionID = context.IncomingMessageHeaders.GetHeader<string>(SESSION_ID_NAME, SESSION_NS);
                profile.SessionToken = context.IncomingMessageHeaders.GetHeader<string>(SESSION_TOKEN_NAME, SESSION_NS);
                profile.UserID = context.IncomingMessageHeaders.GetHeader<string>(SESSION_USER_ID_NAME, SESSION_NS);
                profile.UserName = context.IncomingMessageHeaders.GetHeader<string>(SESSION_USERNAME_NAME, SESSION_NS);
                profile.Culture = context.IncomingMessageHeaders.GetHeader<string>(SESSION_CULTURE_NAME, SESSION_NS);
            }
            else
            {
                profile.SessionID = SESSION_ID_NAME;
                profile.SessionToken = SESSION_TOKEN_NAME;
                profile.UserID = SESSION_USER_ID_NAME;
                profile.UserName = SESSION_USERNAME_NAME;
                profile.Culture = SESSION_CULTURE_NAME;
            }

            return profile;
        }

        public static string GetApplicationID(System.ServiceModel.OperationContext context)
        {
            if (context != null)
            {
                return context.IncomingMessageHeaders.GetHeader<string>(SESSION_APPLICATION_ID_NAME, SESSION_NS);
            }
            else
            {
                return SESSION_APPLICATION_ID_NAME;
            }
        }

    }
}
