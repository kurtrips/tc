// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Data;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Services.WCF.ScheduleItem.Persistence;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.GenericNotes;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// Unit tests for the HermesScheduleItemPersistenceHelper class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class HermesScheduleItemPersistenceHelperTests
    {
        /// <summary>
        /// The HermesScheduleItemPersistenceHelper instance to use for the tests.
        /// </summary>
        HermesScheduleItemPersistenceHelper helper;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/mainTestConfig.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/otherTestConfig.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/ConnectionFactory.xml");

            UnitTestHelper.ClearTestDatabase();
            UnitTestHelper.SetupTestDatabase();

            helper = new HermesScheduleItemPersistenceHelper();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            UnitTestHelper.ClearTestDatabase();
            ConfigManager.GetInstance().Clear(false);
            helper = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// HermesScheduleItemPersistenceHelper()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsTrue(helper is IScheduleItemHelperBase<string, HermesScheduleItem,
                HermesActivity, HermesScheduleItemStatus, HermesScheduleItemRequestStatus,
                HermesActivityGroup, HermesActivityType, HermesGenericNote, HermesGenericNoteItem,
                HermesGenericNoteItemHistory>, "Wrong type of class.");
        }

        /// <summary>
        /// Tests the BuildActivities method.
        /// public IList&lt;HermesActivity&gt; BuildActivities(IDataReader reader)
        /// </summary>
        [Test]
        public void TestBuildActivities()
        {
            using (OracleConnection connection = Helper.GetConnection("myConnectionName"))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_all_activities", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_hideDisabled", OracleDbType.Int32).Value = 0;
                    command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                        .Direction = ParameterDirection.Output;

                    //Call the procedure
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        IList<HermesActivity> activities = helper.BuildActivities(reader);
                        Assert.IsNotNull(activities, "Must not return null.");
                        Assert.AreEqual(1, activities.Count, "Must return 1 record.");
                        Assert.AreEqual("33333333-3333-3333-3333-333333333333",
                            activities[0].Id, "Wrong data returned.");
                    }
                }
            }
        }

        /// <summary>
        /// Tests the BuildActivityGroups method.
        /// public IList&lt;HermesActivityGroup&gt; BuildActivityGroups(IDataReader reader)
        /// </summary>
        [Test]
        public void TestBuildActivityGroups()
        {
            using (OracleConnection connection = Helper.GetConnection("myConnectionName"))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_activity_groups", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    //Set the parameters
                    command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                        .Direction = ParameterDirection.Output;

                    //Call the procedure
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        IList<HermesActivityGroup> actGrps = helper.BuildActivityGroups(reader);
                        Assert.IsNotNull(actGrps, "Must not return null.");
                        Assert.AreEqual(1, actGrps.Count, "Must return 1 record.");
                        Assert.AreEqual("11111111-1111-1111-1111-111111111111",
                            actGrps[0].Id, "Wrong data returned.");
                    }
                }
            }
        }

        /// <summary>
        /// Tests the BuildActivityTypes method.
        /// public IList&lt;HermesActivityType&gt; BuildActivityTypes(IDataReader reader)
        /// </summary>
        [Test]
        public void TestBuildActivityTypes()
        {
            using (OracleConnection connection = Helper.GetConnection("myConnectionName"))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_activity_types", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    //Set the parameters
                    command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                        .Direction = ParameterDirection.Output;

                    //Call the procedure
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        IList<HermesActivityType> actTypes = helper.BuildActivityTypes(reader);
                        Assert.IsNotNull(actTypes, "Must not return null.");
                        Assert.AreEqual(1, actTypes.Count, "Must return 1 record.");
                        Assert.AreEqual("22222222-2222-2222-2222-222222222222",
                            actTypes[0].Id, "Wrong data returned.");
                    }
                }
            }
        }

        /// <summary>
        /// Tests the BuildScheduleItemRequestStatuses method.
        /// public IList&lt;HermesScheduleItemRequestStatus&gt; BuildScheduleItemRequestStatuses(IDataReader reader)
        /// </summary>
        [Test]
        public void TestBuildScheduleItemRequestStatuses()
        {
            using (OracleConnection connection = Helper.GetConnection("myConnectionName"))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_sched_item_req_stat", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    //Set the parameters
                    command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                        .Direction = ParameterDirection.Output;

                    //Call the procedure
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        IList<HermesScheduleItemRequestStatus> schedReqStatuses =
                            helper.BuildScheduleItemRequestStatuses(reader);
                        Assert.IsNotNull(schedReqStatuses, "Must not return null.");
                        Assert.AreEqual(1, schedReqStatuses.Count, "Must return 1 record.");
                        Assert.AreEqual("55555555-5555-5555-5555-555555555555",
                            schedReqStatuses[0].Id, "Wrong data returned.");
                    }
                }
            }
        }

        /// <summary>
        /// Tests the BuildScheduleItemStatuses method.
        /// public IList&lt;HermesScheduleItemStatus&gt; BuildScheduleItemStatuses(IDataReader reader)
        /// </summary>
        [Test]
        public void TestBuildScheduleItemStatuses()
        {
            using (OracleConnection connection = Helper.GetConnection("myConnectionName"))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_sched_item_stats", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    //Set the parameters
                    command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                        .Direction = ParameterDirection.Output;

                    //Call the procedure
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        IList<HermesScheduleItemStatus> schedStatuses =
                            helper.BuildScheduleItemStatuses(reader);
                        Assert.IsNotNull(schedStatuses, "Must not return null.");
                        Assert.AreEqual(1, schedStatuses.Count, "Must return 1 record.");
                        Assert.AreEqual("44444444-4444-4444-4444-444444444444",
                            schedStatuses[0].Id, "Wrong data returned.");
                    }
                }
            }
        }

        /// <summary>
        /// Tests the BuildScheduleItems method.
        /// public IList&lt;HermesScheduleItem&gt; BuildScheduleItems(IDataReader reader)
        /// </summary>
        [Test]
        public void TestBuildScheduleItems()
        {
            using (OracleConnection connection = Helper.GetConnection("myConnectionName"))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_sched_item", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    //Set the parameters
                    command.Parameters.Add("p_schedule_item_id", OracleDbType.Raw).Value =
                        new Guid("66666666666666666666666666666666").ToByteArray();
                    command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                        .Direction = ParameterDirection.Output;

                    //Call the procedure
                    connection.Open();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        IList<HermesScheduleItem> schedItems =
                            helper.BuildScheduleItems(reader);
                        Assert.IsNotNull(schedItems, "Must not return null.");
                        Assert.AreEqual(1, schedItems.Count, "Must return 1 record.");
                    }
                }
            }
        }

    }
}
