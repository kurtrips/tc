using System;
using System.ServiceModel;
using Oracle.DataAccess.Client;
using HermesNS.Entity.Common;
using TopCoder.Util.ConfigurationManager;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// A mock implementation of WcfHelper class.
    /// </summary>
    public class WcfHelper
    {
        /// <summary>
        /// Returns the user profile.
        /// </summary>
        /// <param name="context">the OperationContext.</param>
        /// <returns>the profile.</returns>
        public static Profile GetProfileFromContext(OperationContext context)
        {
            Profile profile = new Profile();
            profile.UserID = "TCSDEVELOPER";
            profile.UserName = "BASE_OBJECTS";

            return profile;
        }

        /// <summary>
        /// Returns the id of the application.
        /// </summary>
        /// <param name="context">the context from which the id is retrieved.</param>
        /// <returns>application id.</returns>
        public static string GetApplicationId(OperationContext operationContext)
        {
            return "hermes";;
        }
    }

    public class HermesAuthorizationServiceMediator
    {
        public static void MediateMethod()
        {
        }

        public static void MediateMethod(string applicationId, object p, object p_3, System.Reflection.MethodBase methodBase)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

namespace HermesNS.SystemServices.Data.ProxyConnection
{
    /// <summary>
    /// A mock implementation of OracleConnectionHelper class.
    /// </summary>
    public class OracleConnectionHelper
    {
        /// <summary>
        /// Mock implementation. Gets a new OracleConnection for the given connection name.
        /// </summary>
        /// <param name="userID">This param is ignored</param>
        /// <param name="connectionName">The connection string with which to form OracleConnection instance</param>
        /// <returns>Created OracleConnection instance.</returns>
        public static OracleConnection GetPooledConnection(string userID, string connectionName)
        {
            string connectionString = ConfigManager.GetInstance().
                GetValue("TopCoder.Data.ConnectionFactory", connectionName + "_Default_String");
            return new OracleConnection(connectionString);
        }
    }
}

namespace HermesNS.Entity.Common
{
    /// <summary>
    /// A mock implementation of Profile class.
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// Creates instance of Profile class
        /// </summary>
        public Profile()
        {
        }

        /// <summary>
        /// Mock variable for holding the id.
        /// </summary>
        string id;

        /// <summary>
        /// Mock implementation of the id of user profile
        /// </summary>
        public string UserID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Mock variable for holding the user name.
        /// </summary>
        private string userName;

        /// <summary>
        /// Mock implementation of UserName property.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// Mock variable for holding session id.
        /// </summary>
        private string sessionId;

        /// <summary>
        /// Mock implementation of SessionID property.
        /// </summary>
        public string SessionID
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        /// <summary>
        /// Mock variable for holding session token.
        /// </summary>
        private string sessionToken;

        /// <summary>
        /// Mock implementation of SessionToken property
        /// </summary>
        public string SessionToken
        {
            get { return sessionToken; }
            set { sessionToken = value; }
        }
    }
}
