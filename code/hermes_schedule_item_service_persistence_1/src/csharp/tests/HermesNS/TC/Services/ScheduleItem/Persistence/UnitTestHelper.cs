// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.IO;
using TopCoder.Util.ConfigurationManager;
using Oracle.DataAccess.Client;
using HermesNS.SystemServices.Data.ProxyConnection;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// Helper class for unit tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    internal class UnitTestHelper
    {
        /// <summary>
        /// Sets up the database for each test by inserting an entry into each of the database tables.
        /// </summary>
        internal static void SetupTestDatabase()
        {
            ExecuteStatementsOfSqlFile("../../test_files/CreateTestData.sql");
        }

        /// <summary>
        /// Clears the database for each test by deleting all rows of all the tables.
        /// </summary>
        internal static void ClearTestDatabase()
        {
            ExecuteStatementsOfSqlFile("../../test_files/ClearTestData.sql");
        }

        /// <summary>
        /// Executes sql statements from a file on the databse.
        /// </summary>
        /// <param name="fileName">The name of the file conatining the statements</param>
        internal static void ExecuteStatementsOfSqlFile(string fileName)
        {
            //Load the insert statements from file
            string fileContent = File.ReadAllText(fileName);
            string[] statements = fileContent.Split(';');

            //Get the connection string from config
            string connectionName = ConfigManager.GetInstance().GetValue(
                HermesScheduleItemPersistenceProvider.DefaultNamespace, "connectionName");

            //Run each insert statement
            using (OracleConnection conn = OracleConnectionHelper.GetPooledConnection(null, connectionName))
            {
                conn.Open();

                using (OracleCommand command = new OracleCommand())
                {
                    foreach (string statement in statements)
                    {
                        command.CommandText = statement;
                        command.Connection = conn;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
