/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System.Data;
using HermesNS.SystemServices.Data.ProxyConnection;
using TopCoder.Util.ConfigurationManager;

namespace HermesNS.TC.Services.ScheduleItem.Persistence.FailureTests
{
    /// <summary>
    /// A helper class used by failure test.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class FailureTestHelper
    {
        /// <summary>
        /// The base dir for accuray tests.
        /// </summary>
        internal const string FailureBaseDir = "../../test_files/failure/";

        /// <summary>
        /// The user name.
        /// </summary>
        private const string DbUserName = "";

        /// <summary>
        /// The data source.
        /// </summary>
        private const string DataSource = "failure";

        /// <summary>
        /// Creats connection.
        /// </summary>
        ///
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            return OracleConnectionHelper.GetPooledConnection(DbUserName, DataSource);
        }

        /// <summary>
        /// Adds config files for test.
        /// </summary>
        internal static void LoadConfigFiles()
        {
            ClearConfigFiles();
            ConfigManager cm = ConfigManager.GetInstance();

            cm.LoadFile(FailureBaseDir + "HermesScheduleItemPersistenceProvider.xml");
            cm.LoadFile(FailureBaseDir + "ConnectionFactory.xml");
        }

        /// <summary>
        /// Clears config files after test.
        /// </summary>
        internal static void ClearConfigFiles()
        {
            ConfigManager.GetInstance().Clear(false);
        }
    }
}