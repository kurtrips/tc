/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using NUnit.Framework;

using System.Data;

using TopCoder.Services.WCF.ScheduleItem;

using HermesNS.TC.Services.ScheduleItem.Persistence;
using TopCoder.Util.ConfigurationManager;

namespace HermesNS.TC.Services.ScheduleItem.Persistence.FailureTests
{
    /// <summary>
    /// Failure test for <see cref="HermesScheduleItemPersistenceHelper"/> class.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesScheduleItemPersistenceHelperFailureTest
    {
        /// <summary>
        /// An instance of <see cref="HermesScheduleItemPersistenceHelper"/> to perform test on.
        /// </summary>
        private HermesScheduleItemPersistenceHelper helper;

        /// <summary>
        /// The connection to database.
        /// </summary>
        private IDbConnection conn;

        /// <summary>
        /// Set up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().Clear(false);
            ConfigManager.GetInstance().LoadFile("../../test_files/failure/ConnectionFactory.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/failure/HermesScheduleItemPersistenceProvider.xml");

            conn = FailureTestHelper.CreateConnection();
            helper = new HermesScheduleItemPersistenceHelper();
        }

        /// <summary>
        /// Tear down for each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ConfigManager.GetInstance().Clear(false);

            conn.Close();
            conn.Dispose();
        }

        #region Test BuildActivities(IDataReader reader)
        /// <summary>
        /// Test method <code>BuildActivities(IDataReader reader)</code>.
        /// When reader is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildActivities_ReaderIsNull()
        {
            helper.BuildActivities(null);
        }

        /// <summary>
        /// Test method <code>BuildActivities(IDataReader reader)</code>.
        /// When reader is closed, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildActivities_ReaderIsClosed()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM view_activity";
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    reader.Close();
                    helper.BuildActivities(reader);
                }
            }
        }

        ///// <summary>
        ///// Test method <code>BuildActivities(IDataReader reader)</code>.
        ///// When reader is invalid for activity, throw InvalidArgumentException.
        ///// </summary>
        //[Test, ExpectedException(typeof(InvalidArgumentException))]
        //public void TestBuildActivities_ReaderIsInvalid()
        //{
        //    using (IDbCommand cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "SELECT * FROM view_activity_group";
        //        conn.Open();
        //        using (IDataReader reader = cmd.ExecuteReader())
        //        {
        //            helper.BuildActivities(reader);
        //        }
        //    }
        //}
        #endregion

        #region Test BuildActivityGroups(IDataReader reader)
        /// <summary>
        /// Test method <code>BuildActivityGroups(IDataReader reader)</code>.
        /// When reader is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildActivityGroups_ReaderIsNull()
        {
            helper.BuildActivityGroups(null);
        }

        /// <summary>
        /// Test method <code>BuildActivityGroups(IDataReader reader)</code>.
        /// When reader is closed, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildActivityGroups_ReaderIsClosed()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM view_activity_group";
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    reader.Close();
                    helper.BuildActivityGroups(reader);
                }
            }
        }

        ///// <summary>
        ///// Test method <code>BuildActivityGroups(IDataReader reader)</code>.
        ///// When reader is invalid for activity group, throw InvalidArgumentException.
        ///// </summary>
        //[Test, ExpectedException(typeof(InvalidArgumentException))]
        //public void TestBuildActivityGroups_ReaderIsInvalid()
        //{
        //    using (IDbCommand cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "SELECT * FROM view_activity";
        //        conn.Open();
        //        using (IDataReader reader = cmd.ExecuteReader())
        //        {
        //            helper.BuildActivityGroups(reader);
        //        }
        //    }
        //}
        #endregion

        #region Test BuildActivityTypes(IDataReader reader)
        /// <summary>
        /// Test method <code>BuildActivityTypes(IDataReader reader)</code>.
        /// When reader is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildActivityTypes_ReaderIsNull()
        {
            helper.BuildActivityTypes(null);
        }

        /// <summary>
        /// Test method <code>BuildActivityTypes(IDataReader reader)</code>.
        /// When reader is closed, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildActivityTypes_ReaderIsClosed()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM view_activity_type";
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    reader.Close();
                    helper.BuildActivityTypes(reader);
                }
            }
        }

        ///// <summary>
        ///// Test method <code>BuildActivityTypes(IDataReader reader)</code>.
        ///// When reader is invalid for activity group, throw InvalidArgumentException.
        ///// </summary>
        //[Test, ExpectedException(typeof(InvalidArgumentException))]
        //public void TestBuildActivityTypes_ReaderIsInvalid()
        //{
        //    using (IDbCommand cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "SELECT * FROM view_activity";
        //        conn.Open();
        //        using (IDataReader reader = cmd.ExecuteReader())
        //        {
        //            helper.BuildActivityTypes(reader);
        //        }
        //    }
        //}
        #endregion

        #region Test BuildScheduleItems(IDataReader reader)
        /// <summary>
        /// Test method <code>BuildScheduleItems(IDataReader reader)</code>.
        /// When reader is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildScheduleItems_ReaderIsNull()
        {
            helper.BuildScheduleItems(null);
        }

        /// <summary>
        /// Test method <code>BuildScheduleItems(IDataReader reader)</code>.
        /// When reader is closed, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildScheduleItems_ReaderIsClosed()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM view_schedule_item";
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    reader.Close();
                    helper.BuildScheduleItems(reader);
                }
            }
        }

        ///// <summary>
        ///// Test method <code>BuildScheduleItems(IDataReader reader)</code>.
        ///// When reader is invalid for schedule item, throw InvalidArgumentException.
        ///// </summary>
        //[Test, ExpectedException(typeof(InvalidArgumentException))]
        //public void TestBuildScheduleItems_ReaderIsInvalid()
        //{
        //    using (IDbCommand cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "SELECT * FROM view_activity";
        //        conn.Open();
        //        using (IDataReader reader = cmd.ExecuteReader())
        //        {
        //            helper.BuildScheduleItems(reader);
        //        }
        //    }
        //}
        #endregion

        #region Test BuildScheduleItemStatuses(IDataReader reader)
        /// <summary>
        /// Test method <code>BuildScheduleItemStatuses(IDataReader reader)</code>.
        /// When reader is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildScheduleItemStatuses_ReaderIsNull()
        {
            helper.BuildScheduleItemStatuses(null);
        }

        /// <summary>
        /// Test method <code>BuildScheduleItemStatuses(IDataReader reader)</code>.
        /// When reader is closed, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildScheduleItemStatuses_ReaderIsClosed()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM view_schedule_item_status";
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    reader.Close();
                    helper.BuildScheduleItemStatuses(reader);
                }
            }
        }

        ///// <summary>
        ///// Test method <code>BuildScheduleItemStatuses(IDataReader reader)</code>.
        ///// When reader is invalid for schedule item statuses, throw InvalidArgumentException.
        ///// </summary>
        //[Test, ExpectedException(typeof(InvalidArgumentException))]
        //public void TestBuildScheduleItemStatuses_ReaderIsInvalid()
        //{
        //    using (IDbCommand cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "SELECT * FROM view_activity";
        //        conn.Open();
        //        using (IDataReader reader = cmd.ExecuteReader())
        //        {
        //            helper.BuildScheduleItemStatuses(reader);
        //        }
        //    }
        //}
        #endregion

        #region Test BuildScheduleItemRequestStatuses(IDataReader reader)
        /// <summary>
        /// Test method <code>BuildScheduleItemRequestStatuses(IDataReader reader)</code>.
        /// When reader is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildScheduleItemRequestStatuses_ReaderIsNull()
        {
            helper.BuildScheduleItemRequestStatuses(null);
        }

        /// <summary>
        /// Test method <code>BuildScheduleItemRequestStatuses(IDataReader reader)</code>.
        /// When reader is closed, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestBuildScheduleItemRequestStatuses_ReaderIsClosed()
        {
            using (IDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM view_sched_item_req_status";
                conn.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    reader.Close();
                    helper.BuildScheduleItemRequestStatuses(reader);
                }
            }
        }

        ///// <summary>
        ///// Test method <code>BuildScheduleItemRequestStatuses(IDataReader reader)</code>.
        ///// When reader is invalid for schedule item request statuses, throw InvalidArgumentException.
        ///// </summary>
        //[Test, ExpectedException(typeof(InvalidArgumentException))]
        //public void TestBuildScheduleItemRequestStatuses_ReaderIsInvalid()
        //{
        //    using (IDbCommand cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = "SELECT * FROM view_activity";
        //        conn.Open();
        //        using (IDataReader reader = cmd.ExecuteReader())
        //        {
        //            helper.BuildScheduleItemRequestStatuses(reader);
        //        }
        //    }
        //}
        #endregion
    }
}