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
        private static readonly string SESSION_NS = "ns";
        private static readonly string SESSION_ID_NAME = "";
        private static readonly string SESSION_TOKEN_NAME = "";
        private static readonly string SESSION_USER_ID_NAME = "";
        private static readonly string SESSION_USERNAME_NAME = "";
        private static readonly string SESSION_CULTURE_NAME = "";
        private static readonly string SESSION_APPLICATION_ID_NAME = "";

        public static Profile GetProfileFromContext(System.ServiceModel.OperationContext context)
        {
            Profile profile = new Profile();

            profile.SessionID = context.IncomingMessageHeaders.GetHeader<string>(SESSION_ID_NAME, SESSION_NS);
            profile.SessionToken = context.IncomingMessageHeaders.GetHeader<string>(SESSION_TOKEN_NAME, SESSION_NS);
            profile.UserID = context.IncomingMessageHeaders.GetHeader<string>(SESSION_USER_ID_NAME, SESSION_NS);
            profile.UserName = context.IncomingMessageHeaders.GetHeader<string>(SESSION_USERNAME_NAME, SESSION_NS);
            profile.Culture = context.IncomingMessageHeaders.GetHeader<string>(SESSION_CULTURE_NAME, SESSION_NS);

            return profile;
        }

        public static string GetApplicationID(System.ServiceModel.OperationContext context)
        {
            return context.IncomingMessageHeaders.GetHeader<string>(SESSION_APPLICATION_ID_NAME, SESSION_NS);
        }

    }
}
