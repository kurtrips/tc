/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System;
using System.Data;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using HermesNS.TC.Services.ScheduleItem.Persistence;
using Hermes.Services.Security.Authorization.Client.Common;
using TopCoder.Data.ConnectionFactory;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Configuration;
using Oracle.DataAccess.Client;

namespace HermesNS.TC.Services.ScheduleItem.StressTests
{
    /// <summary>
    /// <p>
    /// A helper class for the stress tests.
    /// </p>
    /// </summary>
    /// <author>hotblue</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class StressTestsHelper
    {
        /// <summary>
        /// <p>
        /// The number to repeat a single task.
        /// </p>
        /// </summary>
        public static int Iteration = 100;

        /// <summary>
        /// <p>
        /// The tick count for the current watch.
        /// </p>
        /// </summary>
        private static long start = 0;

        /// <summary>
        /// Start the watch.
        /// </summary>
        public static void Start()
        {
            start = Environment.TickCount;
        }

        /// <summary>
        /// Stop the watch and output information to console.
        /// </summary>
        /// <param name="action">The action performed.</param>
        public static void Stop(string action)
        {
            Console.WriteLine(string.Format("Test {0} method {1} times took {2}ms.",
                action, Iteration, Environment.TickCount - start));
        }

        /// <summary>
        /// <para>
        /// Clears configuration.
        /// </para>
        /// </summary>
        public static void ClearConfiguration()
        {
            ConfigManager.GetInstance().Clear(true);
        }

        /// <summary>
        /// <para>
        /// Loads configuration.
        /// </para>
        /// </summary>
        public static void LoadConfiguration()
        {
            ClearConfiguration();

            ConfigManager cm = ConfigManager.GetInstance();
            cm.LoadFile("../../test_files/StressTests/Config.xml");
        }



        /// <summary>
        /// <para>
        /// Execute the query.
        /// </para>
        /// </summary>
        /// <param name="connection">
        /// Db connection.
        /// </param>
        /// <param name="commandString">
        /// Command string.
        /// </param>
        private static void ExecuteNonQuery(IDbConnection connection, string commandString)
        {
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = commandString;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// <para>
        /// Execute query.
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// Type information.
        /// </typeparam>
        /// <param name="query">
        /// query string.
        /// </param>
        /// <param name="index">
        /// The query field index.
        /// </param>
        /// <returns>
        /// Result record.
        /// </returns>
        public static IList<T> ExecuteReader<T>(string query, int index)
        {
            IList<T> ret = new List<T>();
            using (IDbConnection connection = ConnectionManager.Instance.CreateDefaultPredefinedDbConnection())
            {
                connection.Open();
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (typeof(T).Equals(typeof(long)))
                            {
                                ret.Add((T)(object)reader.GetInt64(index));
                            }
                            else if (typeof(T).Equals(typeof(string)))
                            {
                                ret.Add((T)(object)reader.GetString(index));
                            }
                            else
                            {
                                ret.Add((T)reader.GetValue(index));
                            }
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Sets up the database for each test by inserting an entry into each of the database tables.
        /// </summary>
        internal static void SetupTestDatabase()
        {
            ExecuteStatementsOfSqlFile("../../test_files/StressTests/CreateTestData.sql");
        }

        /// <summary>
        /// Clears the database for each test by deleting all rows of all the tables.
        /// </summary>
        internal static void ClearTestDatabase()
        {
            ExecuteStatementsOfSqlFile("../../test_files/StressTests/ClearTestData.sql");
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