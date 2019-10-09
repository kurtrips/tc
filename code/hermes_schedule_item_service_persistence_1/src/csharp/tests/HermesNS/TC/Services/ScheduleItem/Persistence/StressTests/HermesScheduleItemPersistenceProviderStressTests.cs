/*
 * "HermesScheduleItemPersistenceProviderStressTests.cs"
 * Copyright (c) 2006, TopCoder, Inc. All rights reserved
 */

using System;
using System.Collections.Generic;
using HermesNS.TC.Services.GenericNotes;
using HermesNS.TC.Services.ScheduleItem.Entities;
using NUnit.Framework;
using TopCoder.Util.ConfigurationManager;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// <p>Stress test for the class <code>HermesScheduleItemPersistenceProvider</code>.</p>
    /// </summary>
    ///
    /// <author>
    /// catcher
    /// </author>
    ///
    /// <copyright>
    /// Copyright (c) 2006, TopCoder, Inc. All rights reserved
    /// </copyright>
    ///
    /// <version>
    /// 1.0
    /// </version>
    [TestFixture]
    public class HermesScheduleItemPersistenceProviderStressTests
    {
        /// <summary>
        /// Represent the instance of <c> ConfigManager</c> used to test.
        /// </summary>
        private readonly ConfigManager cm = ConfigManager.GetInstance();

        /// <summary>
        /// <p>
        ///  Represent the config file used to test.
        /// </p>
        /// </summary>
        private const string CONF = "../../test_files/stress/Config.xml";

        /// <summary>
        /// The HermesScheduleItemPersistenceProvider instance to use for the tests.
        /// </summary>
        private HermesScheduleItemPersistenceProvider provider;

        /// <summary>
        /// The iteration for each method test.
        /// </summary>
        private const int ITERATION = 50;

        /// <summary>
        /// The HermesActivityGroup instance to use for the tests.
        /// </summary>
        private HermesActivityGroup group;

        /// <summary>
        /// The HermesActivityGroup instance to use for the tests.
        /// </summary>
        private HermesActivityType activityType;

        /// <summary>
        /// The HermesActivityGroup instance to use for the tests.
        /// </summary>
        private HermesActivity activity;

        /// <summary>
        /// The HermesActivityGroup instance to use for the tests.
        /// </summary>
        private HermesScheduleItemStatus itemStatus;

        /// <summary>
        /// The HermesActivityGroup instance to use for the tests.
        /// </summary>
        private HermesScheduleItemRequestStatus requestStatus;

        /// <summary>
        /// The HermesActivityGroup instance to use for the tests.
        /// </summary>
        private HermesScheduleItem scheduleItem;

        /// <summary>
        /// <p> Set up test environment.</p>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            cm.Clear(false);
            cm.LoadFile(CONF);
            cm.LoadFile("../../test_files/stress/ConnectionFactory.xml");

            string name =
                cm.GetValue("HermesNS.TC.Services.ScheduleItem.Persistence.HermesScheduleItemPersistenceProvider",
                    "connectionName");
            provider = new HermesScheduleItemPersistenceProvider(name, new HermesScheduleItemPersistenceHelper());
            group = CreateActivityGroup();

            provider.SaveActivityGroup(group);

            activityType = CreateActivityType();
            provider.SaveActivityType(activityType);
            activity = CreateActivity();
            provider.SaveActivity(activity);

            itemStatus = CreateScheduleItemStatus();
            provider.SaveScheduleItemStatus(itemStatus);
            requestStatus = CreateScheduleItemRequestStatus();
            provider.SaveScheduleItemRequestStatus(requestStatus);
            scheduleItem = CreateScheduleItem();
            provider.SaveScheduleItem(scheduleItem);
        }

        /// <summary>
        /// Clears the Config Manager of any namespaces.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            cm.Clear(false);
        }

        /// <summary>
        /// StressTest for the method <c>Activity</c>.
        /// </summary>
        [Test]
        public void TestActivity()
        {
            DateTime start = DateTime.Now;
            HermesActivity hermesActivity = activity;

            for (int i = 1; i <= ITERATION; i++)
            {
                hermesActivity.LastModifiedBy = "test" + i;
                provider.SaveActivity(hermesActivity);
                HermesActivity result = provider.GetActivity(hermesActivity.Id);
                Assert.AreEqual(hermesActivity.LastModifiedBy, result.LastModifiedBy);
                IList<HermesActivity> list = provider.GetAllActivities(true);
            }
            Console.WriteLine("Test Activity run {0} times, taking {1}ms.", ITERATION,
                (DateTime.Now - start).TotalMilliseconds.ToString("n"));
        }

        /// <summary>
        /// StressTest for the method <c>ActivityGroup</c>.
        /// </summary>
        [Test]
        public void TestActivityGroup()
        {
            DateTime start = DateTime.Now;
            HermesActivityGroup activityGroup = CreateActivityGroup();
            for (int i = 1; i <= ITERATION; i++)
            {
                activityGroup.LastModifiedBy = "test" + i;
                provider.SaveActivityGroup(activityGroup);
                HermesActivityGroup result = provider.GetActivityGroup(activityGroup.Id);
                Assert.AreEqual(activityGroup.LastModifiedBy, result.LastModifiedBy);
                provider.DeleteActivityGroup(activityGroup.Id);
                Assert.IsNull(provider.GetActivityGroup(activityGroup.Id));
                IList<HermesActivityGroup> list = provider.GetAllActivityGroups();
            }
            Console.WriteLine("test ActivityGroup run {0} times, taking {1}ms.", ITERATION,
                (DateTime.Now - start).TotalMilliseconds.ToString("n"));
        }

        /// <summary>
        /// StressTest for the method <c>ActivityType</c>.
        /// </summary>
        [Test]
        public void TestActivityType()
        {
            DateTime start = DateTime.Now;
            HermesActivityType hermesActivityType = CreateActivityType();
            for (int i = 1; i <= ITERATION; i++)
            {
                hermesActivityType.LastModifiedBy = "test" + i;
                provider.SaveActivityType(hermesActivityType);
                HermesActivityType result = provider.GetActivityType(hermesActivityType.Id);
                Assert.AreEqual(hermesActivityType.LastModifiedBy, result.LastModifiedBy);
                provider.DeleteActivityType(hermesActivityType.Id);
                Assert.IsNull(provider.GetActivityType(hermesActivityType.Id));
                IList<HermesActivityType> list = provider.GetAllActivityTypes();
            }
            Console.WriteLine("ActivityType run {0} times, taking {1}ms.", ITERATION,
                (DateTime.Now - start).TotalMilliseconds.ToString("n"));
        }

        /// <summary>
        /// StressTest for the method <c>ScheduleItem</c>.
        /// </summary>
        [Test]
        public void TestScheduleItem()
        {
            DateTime start = DateTime.Now;
            HermesScheduleItem item = CreateScheduleItem();
            for (int i = 1; i <= ITERATION; i++)
            {
                item.LastModifiedBy = "test" + i;
                provider.SaveScheduleItem(item);
                HermesScheduleItem result = provider.GetScheduleItem(item.Id);
                Assert.AreEqual(item.LastModifiedBy, result.LastModifiedBy);
                provider.DeleteScheduleItem(item.Id);
                Assert.IsNull(provider.GetScheduleItem(item.Id));
            }
            Console.WriteLine("ScheduleItem run {0} times, taking {1}ms.", ITERATION,
                (DateTime.Now - start).TotalMilliseconds.ToString("n"));
        }

        /// <summary>
        /// StressTest for the method <c>ScheduleItemStatus</c>.
        /// </summary>
        [Test]
        public void TestScheduleItemStatus()
        {
            DateTime start = DateTime.Now;
            HermesScheduleItemStatus item = CreateScheduleItemStatus();
            for (int i = 1; i <= ITERATION; i++)
            {
                item.LastModifiedBy = "test" + i;
                provider.SaveScheduleItemStatus(item);
                HermesScheduleItemStatus result = provider.GetScheduleItemStatus(item.Id);
                Assert.AreEqual(item.LastModifiedBy, result.LastModifiedBy);
                provider.DeleteScheduleItemStatus(item.Id);
                Assert.IsNull(provider.GetScheduleItemStatus(item.Id));
            }
            Console.WriteLine("ScheduleItemStatus run {0} times, taking {1}ms.", ITERATION,
                (DateTime.Now - start).TotalMilliseconds.ToString("n"));
        }

        /// <summary>
        /// StressTest for the method <c>ScheduleItemRequestStatus</c>.
        /// </summary>
        [Test]
        public void TestScheduleItemRequestStatus()
        {
            DateTime start = DateTime.Now;
            HermesScheduleItemRequestStatus item = CreateScheduleItemRequestStatus();
            for (int i = 1; i <= ITERATION; i++)
            {
                item.LastModifiedBy = "test" + i;
                provider.SaveScheduleItemRequestStatus(item);
                provider.DeleteScheduleItemRequestStatus(item.Id);
            }
            Console.WriteLine("ScheduleItemRequestStatus run {0} times, taking {1}ms.", ITERATION,
                (DateTime.Now - start).TotalMilliseconds.ToString("n"));
        }

        /// <summary>
        /// Create the HermesActivityGroup for the test
        /// </summary>
        /// <returns>the instance of the HermesActivityGroup</returns>
        private HermesActivityGroup CreateActivityGroup()
        {
            HermesActivityGroup group = new HermesActivityGroup();
            group.Id = Guid.NewGuid().ToString("n");
            group.Abbreviation = "HAG";
            group.Name = "HermesActivityGroup";
            group.LastModifiedBy = "hagUser";
            group.LastModifiedDate = DateTime.Now;
            return group;
        }

        /// <summary>
        /// Create the HermesScheduleItemStatus for the test
        /// </summary>
        /// <returns>the instance of the HermesScheduleItemStatus</returns>
        private HermesScheduleItemStatus CreateScheduleItemStatus()
        {
            HermesScheduleItemStatus status = new HermesScheduleItemStatus();
            status.Id = Guid.NewGuid().ToString("n");
            status.Abbreviation = "HSISNew";
            status.Description = "stress";
            status.LastModifiedBy = "me";
            status.LastModifiedDate = new DateTime(2006, 12, 12);
            return status;
        }

        /// <summary>
        /// Create the HermesScheduleItem for the test
        /// </summary>
        /// <returns>the instance of the HermesScheduleItem</returns>
        private HermesScheduleItem CreateScheduleItem()
        {
            HermesScheduleItem hsi = new HermesScheduleItem();
            hsi.Id = Guid.NewGuid().ToString("N");
            hsi.Activity = activity;
            hsi.Duration = new decimal(456.6);
            hsi.ExceptionFlag = 'N';
            hsi.ExpirationDate = new DateTime(2005, 5, 23);
            hsi.LastModifiedBy = "abc";
            hsi.LastModifiedDate = new DateTime(2003, 12, 1);

            HermesGenericNote note = new HermesGenericNote();
            note.Id = Guid.NewGuid().ToString("n");
            hsi.Note = note;
            hsi.ScheduleItemRequestStatus = requestStatus;
            hsi.ScheduleItemStatus = itemStatus;
            hsi.Version = 2;
            hsi.WorkDate = new DateTime(2004, 4, 12);
            hsi.WorkDayAmount = new Decimal(867.23);
            return hsi;
        }

        /// <summary>
        /// Create the HermesActivityType for the test
        /// </summary>
        /// <returns>the instance of the HermesActivityType</returns>
        private HermesActivityType CreateActivityType()
        {
            HermesActivityType activityType = new HermesActivityType();
            activityType.Id = Guid.NewGuid().ToString("n");
            activityType.Abbreviation = "HAT1";
            activityType.Name = "HermesActivityType1";
            activityType.LastModifiedBy = "my";
            activityType.LastModifiedDate = new DateTime(2007, 11, 7);
            activityType.ActivityGroup = group;

            return activityType;
        }

        /// <summary>
        /// Create the HermesScheduleItemRequestStatus for the test
        /// </summary>
        /// <returns>the instance of the HermesScheduleItemRequestStatus</returns>
        private HermesScheduleItemRequestStatus CreateScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus status = new HermesScheduleItemRequestStatus();
            status.Id = Guid.NewGuid().ToString("n");
            status.Abbreviation = "HSIRS1";
            status.Description = "HermesScheduleItemRequestStatus1";
            status.LastModifiedBy = "hsirsUser";
            status.LastModifiedDate = new DateTime(2002, 11, 12);
            return status;
        }

        /// <summary>
        /// Create the CreateActivity for the test
        /// </summary>
        /// <returns>the instance of the CreateActivity</returns>
        private HermesActivity CreateActivity()
        {
            HermesActivity activity = new HermesActivity();
            activity.Id = new Guid().ToString();

            activity.Abbreviation = "ACT";
            activity.ActivityType = activityType;
            activity.DefaultDuration = new decimal(456.45);
            activity.DefaultExpireDays = 123;

            //Changed by kurtrips
            //Changed from date to int as this property is supposed
            //to be an int denoting the number of seconds from 2001/1/1
            activity.DefaultStartTime = 300;

            activity.Enabled = true;
            activity.ExclusiveFlag = true;
            activity.LastModifiedBy = "ACTuser";
            activity.LastModifiedDate = DateTime.Now;
            activity.Name = "CoolAndReallyNiceActivity";
            activity.WorkDayAmount = 345;
            return activity;
        }
    }
}