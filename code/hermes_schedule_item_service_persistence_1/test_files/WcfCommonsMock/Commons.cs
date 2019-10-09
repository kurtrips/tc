using System;
using System.ServiceModel;
using Oracle.DataAccess.Client;
using HermesNS.Entity.Common;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// A mock implementation of WcfHelper class.
    /// </summary>
    public class WcfHelper
    {
        /// <summary>
        /// A mock implementation of GetProfileFromContext method.
        /// Returns a new Profile instance with id set to 'ivern'
        /// </summary>
        /// <param name="opContext">This is not used.</param>
        /// <returns>The created Profile instance.</returns>
        public static Profile GetProfileFromContext(OperationContext opContext)
        {
            Profile prof = new Profile();
            prof.UserID = "ivern";
            return prof;
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
            return new OracleConnection(connectionName);
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
    }
}
