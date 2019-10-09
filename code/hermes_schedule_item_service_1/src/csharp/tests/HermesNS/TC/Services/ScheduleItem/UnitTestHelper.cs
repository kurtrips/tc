/*
 * Copyright (c)2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.IO;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Configuration;
using Oracle.DataAccess.Client;
using HermesNS.TC.Services.ScheduleItem.Persistence;
using Hermes.Services.Security.Authorization.Client.Common;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem
{
    /// <summary>
    /// Helper class for unit tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExcludeAttribute]
    internal class UnitTestHelper
    {
        /// <summary>
        /// Loads test files for ConfigManager
        /// </summary>
        internal static void LoadConfigMgrFiles()
        {
            //Load config files
            ConfigManager.GetInstance().LoadFile("../../test_files/WCFBase.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/Logger.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/serviceConfig.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/persistenceConfig.xml");
        }

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
            string connectionString = ConfigManager.GetInstance().GetValue(
                HermesScheduleItemPersistenceProvider.DefaultNamespace, "connectionName");

            //Run each insert statement
            using (OracleConnection conn = new OracleConnection(connectionString))
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

        /// <summary>
        /// Create configuration for the mediator.
        /// </summary>
        /// <returns>The configuration.</returns>
        internal static IConfiguration CreateMediatorConfig()
        {
            IConfiguration ret = new DefaultConfiguration("root");
            IConfiguration objectDef = new DefaultConfiguration("object_MediatorConfig");
            objectDef.SetSimpleAttribute("name", "AuthorizationMappingProvider");
            IConfiguration typeNameConfig = new DefaultConfiguration("type_name");
            typeNameConfig.SetSimpleAttribute("value", typeof(MockAuthorizationMappingProvider).AssemblyQualifiedName);
            objectDef.AddChild(typeNameConfig);
            ret.AddChild(objectDef);
            return ret;
        }
    }
}
