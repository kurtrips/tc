// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Reflection;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using NUnit.Framework;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Services.WCF.ScheduleItem;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.GenericNotes;
using HermesNS.SystemServices.Data.ProxyConnection;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// Unit tests for the HermesScheduleItemPersistenceProvider class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesScheduleItemPersistenceProviderTests
    {
        /// <summary>
        /// The HermesScheduleItemPersistenceProvider instance to use for the tests.
        /// </summary>
        HermesScheduleItemPersistenceProvider hsipp;

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

            hsipp = new HermesScheduleItemPersistenceProvider();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            UnitTestHelper.ClearTestDatabase();
            ConfigManager.GetInstance().Clear(false);
            hsipp = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// HermesScheduleItemPersistenceProvider()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.AreEqual(GetPrivateField(hsipp, "connectionName"),
                ConfigManager.GetInstance().GetValue(hsipp.GetType().FullName, "connectionName"),
                "connectionName field must be set to correct value.");

            Assert.AreEqual(GetPrivateField(hsipp, "helper").GetType().Name, "HermesScheduleItemPersistenceHelper",
                "helper field must be set to correct value.");
        }

        /// <summary>
        /// Tests the constructor.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            hsipp = new HermesScheduleItemPersistenceProvider("SomeComponentLevelNamespace");

            Assert.AreEqual(GetPrivateField(hsipp, "connectionName"), "SomeConnectionString",
                "connectionName field must be set to correct value.");

            Assert.AreEqual(GetPrivateField(hsipp, "helper").GetType().Name, "HermesScheduleItemPersistenceHelper",
                "helper field must be set to correct value.");
        }

        /// <summary>
        /// Tests the constructor for failure when nameSpace is null.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestConstructor2Fail1()
        {
            new HermesScheduleItemPersistenceProvider(null);
        }

        /// <summary>
        /// Tests the constructor for failure when nameSpace is empty.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestConstructor2Fail2()
        {
            new HermesScheduleItemPersistenceProvider("           ");
        }

        /// <summary>
        /// Tests the constructor for failure when connectionName property is missing from config.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestConstructor2Fail3()
        {
            new HermesScheduleItemPersistenceProvider("ConnectionNameMissingNamespace");
        }

        /// <summary>
        /// Tests the constructor for failure when connectionName property in config has empty value.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestConstructor2Fail4()
        {
            new HermesScheduleItemPersistenceProvider("ConnectionNameEmptyNamespace");
        }

        /// <summary>
        /// Tests the constructor for failure when helper property is missing from config.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestConstructor2Fail5()
        {
            new HermesScheduleItemPersistenceProvider("HelperMissingNamespace");
        }

        /// <summary>
        /// Tests the constructor for failure when helper property in config has empty value.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestConstructor2Fail6()
        {
            new HermesScheduleItemPersistenceProvider("HelperEmptyNamespace");
        }

        /// <summary>
        /// Tests the constructor for failure when helper property in config has empty value.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestConstructor2Fail7()
        {
            new HermesScheduleItemPersistenceProvider("WrongHelperTypeNamespace");
        }

        /// <summary>
        /// Tests the constructor for failure when given namespace is not present at all.
        /// HermesScheduleItemPersistenceProvider(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestConstructor2Fail8()
        {
            new HermesScheduleItemPersistenceProvider("NoSuchNamespace");
        }

        /// <summary>
        /// Tests the constructor.
        /// HermesScheduleItemPersistenceProvider(string connectionName, IScheduleItemHelperBase`10 helper)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            HermesScheduleItemPersistenceHelper helper = new HermesScheduleItemPersistenceHelper();
            string connectionName = "TestConnection";

            hsipp = new HermesScheduleItemPersistenceProvider(connectionName, helper);

            Assert.AreEqual(GetPrivateField(hsipp, "connectionName"), connectionName,
                "connectionName must be set correctly.");
            Assert.AreEqual(GetPrivateField(hsipp, "helper"), helper,
                "helper must be set correctly.");
        }

        /// <summary>
        /// Tests the constructor when connectionName is null.
        /// HermesScheduleItemPersistenceProvider(string connectionName, IScheduleItemHelperBase`10 helper)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestConstructor3Fail1()
        {
            new HermesScheduleItemPersistenceProvider(null, new HermesScheduleItemPersistenceHelper());
        }

        /// <summary>
        /// Tests the constructor when connectionName is empty.
        /// HermesScheduleItemPersistenceProvider(string connectionName, IScheduleItemHelperBase`10 helper)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestConstructor3Fail2()
        {
            new HermesScheduleItemPersistenceProvider("     ", new HermesScheduleItemPersistenceHelper());
        }

        /// <summary>
        /// Tests the constructor when helper is empty.
        /// HermesScheduleItemPersistenceProvider(string connectionName, IScheduleItemHelperBase`10 helper)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestConstructor3Fail3()
        {
            new HermesScheduleItemPersistenceProvider("sajldfhlkjasdf", null);
        }

        /// <summary>
        /// Tests the SaveActivity method when inserting a new record
        /// HermesActivity SaveActivity(HermesActivity activity)
        /// </summary>
        [Test]
        public void TestSaveActivity1()
        {
            HermesActivity activity = new HermesActivity();
            activity.Id = new Guid("99887766998877669988776699887766").ToString();
            activity.Abbreviation = "ACT";
            activity.ActivityType = hsipp.GetActivityType("22222222222222222222222222222222");
            activity.DefaultDuration = new decimal(33.45);
            activity.DefaultExpireDays = 666;
            activity.DefaultStartTime = 720;
            activity.Enabled = true;
            activity.ExclusiveFlag = true;
            activity.LastModifiedBy = "ACTuser";
            activity.LastModifiedDate = new DateTime(2007, 8, 8);
            activity.Name = "CoolAndReallyNiceActivity";
            activity.WorkDayAmount = 9966;

            HermesActivity returned = hsipp.SaveActivity(activity);

            Assert.AreEqual(returned, activity);
            HermesActivity getFromDb = hsipp.GetActivity("99887766998877669988776699887766");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, activity);

            //Check record in history table
            CheckHistoryRecord("activity_hist",
                new string[] { "activity_id", "default_expire_days", "work_day_amt",
                    "duration", "activity_nm", "enabled", "activity_type_id", "activity_abbr",
                    "action", "exclusive_ind"},
                new string[] { GuidStringToOracleGuidString(activity.Id),
                    activity.DefaultExpireDays.ToString(), activity.WorkDayAmount.ToString(),
                    activity.DefaultDuration.ToString(), activity.Name,
                    Convert.ToInt32(activity.Enabled).ToString(),
                    GuidStringToOracleGuidString(activity.ActivityType.Id), activity.Abbreviation, "I",
                    Convert.ToInt32(activity.ExclusiveFlag).ToString() });
        }

        /// <summary>
        /// Tests the SaveActivity method when updaing an existing record.
        /// HermesActivity SaveActivity(HermesActivity activity)
        /// </summary>
        [Test]
        public void TestSaveActivity2()
        {
            //Save first
            TestSaveActivity1();

            //Update all fields and save again
            HermesActivity activity = new HermesActivity();
            activity.Id = new Guid("99887766998877669988776699887766").ToString();
            activity.Abbreviation = "ACT2";
            activity.ActivityType = hsipp.GetActivityType("22222222222222222222222222222222");
            activity.DefaultDuration = new decimal(37.43);
            activity.DefaultExpireDays = 5467;
            activity.DefaultStartTime = 721;
            activity.Enabled = false;
            activity.ExclusiveFlag = false;
            activity.LastModifiedBy = "ACTuser2";
            activity.LastModifiedDate = new DateTime(2007, 8, 9);
            activity.Name = "CoolAndReallyNiceActivity2";
            activity.WorkDayAmount = 456;

            HermesActivity returned = hsipp.SaveActivity(activity);

            Assert.AreEqual(returned, activity);
            HermesActivity getFromDb = hsipp.GetActivity("99887766998877669988776699887766");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, activity);

            //Check record in history table
            CheckHistoryRecord("activity_hist",
                new string[] { "activity_id", "default_expire_days", "work_day_amt",
                    "duration", "activity_nm", "enabled", "activity_type_id", "activity_abbr",
                    "action", "exclusive_ind"},
                new string[] { GuidStringToOracleGuidString(activity.Id),
                    activity.DefaultExpireDays.ToString(), activity.WorkDayAmount.ToString(),
                    activity.DefaultDuration.ToString(), activity.Name,
                    Convert.ToInt32(activity.Enabled).ToString(),
                    GuidStringToOracleGuidString(activity.ActivityType.Id), activity.Abbreviation, "U",
                    Convert.ToInt32(activity.ExclusiveFlag).ToString() });
        }

        /// <summary>
        /// Tests the SaveActivity method when activity is null.
        /// HermesActivity SaveActivity(HermesActivity activity)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveActivityFail1()
        {
            hsipp.SaveActivity(null);
        }

        /// <summary>
        /// Tests the SaveActivity method when save fails.
        /// In this case, save fails because null is inserted into non-nullable field.
        /// HermesActivity SaveActivity(HermesActivity activity)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveActivityFail2()
        {
            HermesActivity activity = new HermesActivity();
            activity.Id = new Guid("99887766998877669988776699887766").ToString();
            activity.Abbreviation = "ACT2";
            activity.ActivityType = null;
            activity.DefaultDuration = new decimal(37.43);
            activity.DefaultExpireDays = 5467;
            activity.DefaultStartTime = 721;
            activity.Enabled = false;
            activity.ExclusiveFlag = false;
            activity.LastModifiedBy = "ACTuser2";
            activity.LastModifiedDate = new DateTime(2007, 8, 9);
            activity.Name = "CoolAndReallyNiceActivity2";
            activity.WorkDayAmount = 456;

            HermesActivity returned = hsipp.SaveActivity(activity);

        }

        /// <summary>
        /// Tests the SaveActivityGroup method when inserting a new record.
        /// HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        /// </summary>
        [Test]
        public void TestSaveActivityGroup1()
        {
            HermesActivityGroup hag = new HermesActivityGroup();
            hag.Id = new Guid("1234567890ABCDEF1234567890ABCDEF").ToString();
            hag.Abbreviation = "HAG1";
            hag.Name = "HermesActivityGroup1";
            hag.LastModifiedBy = "hagUser";
            hag.LastModifiedDate = new DateTime(2007, 7, 7);

            HermesActivityGroup returned = hsipp.SaveActivityGroup(hag);

            Assert.AreEqual(returned, hag);
            HermesActivityGroup getFromDb = hsipp.GetActivityGroup("1234567890ABCDEF1234567890ABCDEF");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hag);

            //Check record in history table
            CheckHistoryRecord("activity_group_hist",
                new string[] { "activity_group_id", "act_grp_nm", "action", "act_grp_abbr" },
                new string[] { GuidStringToOracleGuidString(hag.Id), hag.Name, "I", hag.Abbreviation });
        }

        /// <summary>
        /// Tests the SaveActivityGroup method when updating an existing record.
        /// HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        /// </summary>
        [Test]
        public void TestSaveActivityGroup2()
        {
            //Save first
            TestSaveActivityGroup1();

            //Change all values and update
            HermesActivityGroup hag = new HermesActivityGroup();
            hag.Id = new Guid("1234567890ABCDEF1234567890ABCDEF").ToString();
            hag.Abbreviation = "HAG2";
            hag.Name = "HermesActivityGroup2";
            hag.LastModifiedBy = "hagUser2";
            hag.LastModifiedDate = new DateTime(2005, 7, 7);

            HermesActivityGroup returned = hsipp.SaveActivityGroup(hag);

            Assert.AreEqual(returned, hag);
            HermesActivityGroup getFromDb = hsipp.GetActivityGroup("1234567890ABCDEF1234567890ABCDEF");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hag);

            //Check record in history table
            CheckHistoryRecord("activity_group_hist",
                new string[] { "activity_group_id", "act_grp_nm", "action", "act_grp_abbr" },
                new string[] { GuidStringToOracleGuidString(hag.Id), hag.Name, "U", hag.Abbreviation });
        }

        /// <summary>
        /// Tests the SaveActivityGroup method when activityGroup is null.
        /// HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveActivityGroupFail1()
        {
            hsipp.SaveActivityGroup(null);
        }

        /// <summary>
        /// Tests the SaveActivityGroup method when save fails.
        /// In this case, save fails because null is inserted into non-nullable field.
        /// HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveActivityGroupFail2()
        {
            HermesActivityGroup hag = new HermesActivityGroup();
            hag.Id = new Guid("1234567890ABCDEF1234567890ABCDEF").ToString();
            hag.Abbreviation = "HAG1";
            hag.Name = "HermesActivityGroup1";
            hag.LastModifiedBy = null;
            hag.LastModifiedDate = new DateTime(2007, 7, 7);

            HermesActivityGroup returned = hsipp.SaveActivityGroup(hag);

        }

        /// <summary>
        /// Tests the SaveActivityType method when inserting a new record.
        /// HermesActivityType SaveActivityType(HermesActivityType activityType)
        /// </summary>
        [Test]
        public void TestSaveActivityType1()
        {
            HermesActivityType hat = new HermesActivityType();
            hat.Id = new Guid("34563456345634563456345634563456").ToString();
            hat.Abbreviation = "HAT1";
            hat.Name = "HermesActivityType1";
            hat.LastModifiedBy = "hatUser";
            hat.LastModifiedDate = new DateTime(2007, 11, 7);
            hat.ActivityGroup = hsipp.GetActivityGroup("11111111111111111111111111111111");

            HermesActivityType returned = hsipp.SaveActivityType(hat);

            Assert.AreEqual(returned, hat);
            HermesActivityType getFromDb = hsipp.GetActivityType("34563456345634563456345634563456");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hat);

            //Check record in history table
            CheckHistoryRecord("activity_type_hist",
                new string[] {
                    "activity_type_id", "activity_group_id", "activity_type_nm", "action", "activity_type_abbr" },
                new string[] {
                    GuidStringToOracleGuidString(hat.Id), GuidStringToOracleGuidString(hat.ActivityGroup.Id),
                    hat.Name, "I", hat.Abbreviation });
        }

        /// <summary>
        /// Tests the SaveActivityType method when inserting a new record.
        /// HermesActivityType SaveActivityType(HermesActivityType activityType)
        /// </summary>
        [Test]
        public void TestSaveActivityType2()
        {
            //Save first
            TestSaveActivityType1();
            TestSaveActivityGroup1();

            //Change all values and update
            HermesActivityType hat = new HermesActivityType();
            hat.Id = new Guid("34563456345634563456345634563456").ToString();
            hat.Abbreviation = "HAT2";
            hat.Name = "HermesActivityType2";
            hat.LastModifiedBy = "hatUser2";
            hat.LastModifiedDate = new DateTime(2017, 12, 7);
            hat.ActivityGroup = hsipp.GetActivityGroup("1234567890ABCDEF1234567890ABCDEF");

            HermesActivityType returned = hsipp.SaveActivityType(hat);

            Assert.AreEqual(returned, hat);
            HermesActivityType getFromDb = hsipp.GetActivityType("34563456345634563456345634563456");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hat);

            //Check record in history table
            CheckHistoryRecord("activity_type_hist",
                new string[] {
                    "activity_type_id", "activity_group_id", "activity_type_nm", "action", "activity_type_abbr" },
                new string[] {
                    GuidStringToOracleGuidString(hat.Id), GuidStringToOracleGuidString(hat.ActivityGroup.Id),
                    hat.Name, "U", hat.Abbreviation });
        }

        /// <summary>
        /// Tests the SaveActivityType method when activityGroup is null.
        /// HermesActivityType SaveActivityType(HermesActivityType activityType)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveActivityTypeFail1()
        {
            hsipp.SaveActivityType(null);
        }

        /// <summary>
        /// Tests the SaveActivityType method when save fails.
        /// In this case it fails because the activity group being saved is not
        /// present in database and throw foreign key violation
        /// HermesActivityType SaveActivityType(HermesActivityType activityType)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveActivityTypeFail2()
        {
            HermesActivityType hat = new HermesActivityType();
            hat.Id = new Guid("34563456345634563456345634563456").ToString();
            hat.Abbreviation = "HAT1";
            hat.Name = "HermesActivityType1";
            hat.LastModifiedBy = "hatUser";
            hat.LastModifiedDate = new DateTime(2007, 11, 7);
            hat.ActivityGroup = new HermesActivityGroup();
            hat.ActivityGroup.Id = "00000000000000000000000000000001";

            HermesActivityType returned = hsipp.SaveActivityType(hat);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method when inserting a new entry.
        /// HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        /// </summary>
        [Test]
        public void TestSaveScheduleItem1()
        {
            HermesScheduleItem hsi = new HermesScheduleItem();
            hsi.Id = new Guid("19081908190819081908190819081908").ToString();
            hsi.Activity = hsipp.GetActivity("33333333333333333333333333333333");
            hsi.Duration = new decimal(546.89);
            hsi.ExceptionFlag = 'Y';
            hsi.ExpirationDate = new DateTime(2005, 12, 23);
            hsi.LastModifiedBy = "ivern";
            hsi.LastModifiedDate = new DateTime(2007, 12, 1);
            HermesGenericNote note = new HermesGenericNote();
            note.Id = new Guid("12340987123409871234098712340987").ToString();
            hsi.Note = note;
            hsi.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsi.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsi.Version = 1;
            hsi.WorkDate = new DateTime(2004, 12, 12);
            hsi.WorkDayAmount = new Decimal(86787.23);

            HermesScheduleItem returned = hsipp.SaveScheduleItem(hsi);

            Assert.AreEqual(returned, hsi);
            HermesScheduleItem getFromDb = hsipp.GetScheduleItem("19081908190819081908190819081908");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hsi);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method when inserting a new entry.
        /// HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        /// </summary>
        [Test]
        public void TestSaveScheduleItem2()
        {
            //Save first
            TestSaveScheduleItem1();

            //Update values and save again
            HermesScheduleItem hsi = new HermesScheduleItem();
            hsi.Id = new Guid("19081908190819081908190819081908").ToString();
            hsi.Activity = hsipp.GetActivity("33333333333333333333333333333333");
            hsi.Duration = new decimal(456.6);
            hsi.ExceptionFlag = 'N';
            hsi.ExpirationDate = new DateTime(2005, 5, 23);
            hsi.LastModifiedBy = "assiatant";
            hsi.LastModifiedDate = new DateTime(2003, 12, 1);
            HermesGenericNote note = new HermesGenericNote();
            note.Id = new Guid("15678234156782341567823415678234").ToString();
            hsi.Note = note;
            hsi.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsi.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsi.Version = 2;
            hsi.WorkDate = new DateTime(2004, 4, 12);
            hsi.WorkDayAmount = new Decimal(867.23);

            HermesScheduleItem returned = hsipp.SaveScheduleItem(hsi);

            Assert.AreEqual(returned, hsi);
            HermesScheduleItem getFromDb = hsipp.GetScheduleItem("19081908190819081908190819081908");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hsi);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method when scheduleItem is null.
        /// HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveScheduleItemFail1()
        {
            hsipp.SaveScheduleItem(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method when save fails.
        /// In this case it fails because the activity group being saved is not
        /// present in database and throw foreign key violation
        /// HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveScheduleItemFail2()
        {
            HermesScheduleItem hsi = new HermesScheduleItem();
            hsi.Id = new Guid("19081908190819081908190819081908").ToString();
            hsi.Activity = null;
            hsi.Duration = new decimal(456.6);
            hsi.ExceptionFlag = 'N';
            hsi.ExpirationDate = new DateTime(2005, 5, 23);
            hsi.LastModifiedBy = "assiatant";
            hsi.LastModifiedDate = new DateTime(2003, 12, 1);
            HermesGenericNote note = new HermesGenericNote();
            note.Id = new Guid("15678234156782341567823415678234").ToString();
            hsi.Note = note;
            hsi.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsi.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsi.Version = 2;
            hsi.WorkDate = new DateTime(2004, 4, 12);
            hsi.WorkDayAmount = new Decimal(867.23);

            hsipp.SaveScheduleItem(hsi);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method when performing an insert.
        /// HermesScheduleItemStatus SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)
        /// </summary>
        [Test]
        public void TestSaveScheduleItemStatus1()
        {
            HermesScheduleItemStatus hsis = new HermesScheduleItemStatus();
            hsis.Id = new Guid("12345678123456781234567812345678").ToString();
            hsis.Abbreviation = "HSIS1";
            hsis.Description = "HermesScheduleItemStatus1";
            hsis.LastModifiedBy = "hsisUser";
            hsis.LastModifiedDate = new DateTime(2007, 11, 12);

            HermesScheduleItemStatus returned = hsipp.SaveScheduleItemStatus(hsis);

            Assert.AreEqual(returned, hsis);
            HermesScheduleItemStatus getFromDb = hsipp.GetScheduleItemStatus("12345678123456781234567812345678");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hsis);

            //Check record in history table
            CheckHistoryRecord("sched_status_hist",
                new string[] { "sched_status_id", "status_abbr", "status_desc", "action" },
                new string[] { GuidStringToOracleGuidString(hsis.Id), hsis.Abbreviation, hsis.Description, "I" });
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method when performing an update.
        /// HermesScheduleItemStatus SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)
        /// </summary>
        [Test]
        public void TestSaveScheduleItemStatus2()
        {
            //First insert
            TestSaveScheduleItemStatus1();

            //Change some values
            HermesScheduleItemStatus hsis = new HermesScheduleItemStatus();
            hsis.Id = new Guid("12345678123456781234567812345678").ToString();
            hsis.Abbreviation = "HSISNew";
            hsis.Description = "HermesScheduleItemStatusNew";
            hsis.LastModifiedBy = "hsisUserNew";
            hsis.LastModifiedDate = new DateTime(2008, 11, 12);

            HermesScheduleItemStatus returned = hsipp.SaveScheduleItemStatus(hsis);

            Assert.AreEqual(returned, hsis);
            HermesScheduleItemStatus getFromDb = hsipp.GetScheduleItemStatus("12345678123456781234567812345678");

            //Check if the record was properly updated in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hsis);

            //Check record in history table
            CheckHistoryRecord("sched_status_hist",
                new string[] { "sched_status_id", "status_abbr", "status_desc", "action" },
                new string[] { GuidStringToOracleGuidString(hsis.Id), hsis.Abbreviation, hsis.Description, "U" });
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method when scheduleItemStatus is null
        /// HermesScheduleItemStatus SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveScheduleItemStatusFail1()
        {
            hsipp.SaveScheduleItemStatus(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method when save fails.
        /// In this case, save fails because null is inserted into non-nullable field.
        /// HermesScheduleItemStatus SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveScheduleItemStatusFail2()
        {
            //Change some values
            HermesScheduleItemStatus hsis = new HermesScheduleItemStatus();
            hsis.Id = new Guid("12345678123456781234567812345678").ToString();
            hsis.Abbreviation = "HSISNew";
            hsis.Description = "HermesScheduleItemStatusNew";
            hsis.LastModifiedBy = null;
            hsis.LastModifiedDate = new DateTime(2008, 11, 12);

            hsipp.SaveScheduleItemStatus(hsis);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method when a new entry is inserted.
        /// HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
        /// HermesScheduleItemRequestStatus scheduleRequestStatus)
        /// </summary>
        [Test]
        public void TestSaveScheduleItemRequestStatus1()
        {
            HermesScheduleItemRequestStatus hsirs = new HermesScheduleItemRequestStatus();
            hsirs.Id = new Guid("55665566556655665566556655665566").ToString();
            hsirs.Abbreviation = "HSIRS1";
            hsirs.Description = "HermesScheduleItemRequestStatus1";
            hsirs.LastModifiedBy = "hsirsUser";
            hsirs.LastModifiedDate = new DateTime(2002, 11, 12);

            HermesScheduleItemRequestStatus returned = hsipp.SaveScheduleItemRequestStatus(hsirs);

            Assert.AreEqual(returned, hsirs);
            HermesScheduleItemRequestStatus getFromDb = hsipp.GetScheduleItemRequestStatus(
                "55665566556655665566556655665566");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hsirs);

            //Check record in history table
            CheckHistoryRecord("sched_request_status_hist",
                new string[] { "sched_request_status_id", "status_abbr", "status_desc", "action" },
                new string[] { GuidStringToOracleGuidString(hsirs.Id), hsirs.Abbreviation, hsirs.Description, "I" });
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method when an existing entry is updated.
        /// HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
        /// HermesScheduleItemRequestStatus scheduleRequestStatus)
        /// </summary>
        [Test]
        public void TestSaveScheduleItemRequestStatus2()
        {
            //Create
            TestSaveScheduleItemRequestStatus1();

            //Update some value and save again
            HermesScheduleItemRequestStatus hsirs = new HermesScheduleItemRequestStatus();
            hsirs.Id = new Guid("55665566556655665566556655665566").ToString();
            hsirs.Abbreviation = "HSIRS2";
            hsirs.Description = "HermesScheduleItemRequestStatus2";
            hsirs.LastModifiedBy = "hsirsUser2";
            hsirs.LastModifiedDate = new DateTime(2002, 8, 12);

            HermesScheduleItemRequestStatus returned = hsipp.SaveScheduleItemRequestStatus(hsirs);

            Assert.AreEqual(returned, hsirs);
            HermesScheduleItemRequestStatus getFromDb = hsipp.GetScheduleItemRequestStatus(
                "55665566556655665566556655665566");

            //Check if the record was properly saved in database by
            //checking for equality, each property of the saved entity and the entity got from database.
            CompareProperties(getFromDb, hsirs);

            //Check record in history table
            CheckHistoryRecord("sched_request_status_hist",
                new string[] { "sched_request_status_id", "status_abbr", "status_desc", "action" },
                new string[] { GuidStringToOracleGuidString(hsirs.Id), hsirs.Abbreviation, hsirs.Description, "U" });
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method when scheduleItemStatus is null
        /// HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
        /// HermesScheduleItemRequestStatus scheduleRequestStatus)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveScheduleItemRequestStatusFail1()
        {
            hsipp.SaveScheduleItemRequestStatus(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method when save fails.
        /// In this case, save fails because abbreviation is too large for the field.
        /// HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
        /// HermesScheduleItemRequestStatus scheduleRequestStatus)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveScheduleItemRequestStatusFail2()
        {
            //Save
            HermesScheduleItemRequestStatus hsirs = new HermesScheduleItemRequestStatus();
            hsirs.Id = new Guid("12345678123456781234567812345678").ToString();
            hsirs.Abbreviation = "ABigAndVeryLongAndInvalidAbbreviation";
            hsirs.Description = "HermesScheduleItemStatusNew";
            hsirs.LastModifiedBy = null;
            hsirs.LastModifiedDate = new DateTime(2008, 11, 12);

            hsipp.SaveScheduleItemRequestStatus(hsirs);
        }

        /// <summary>
        /// Tests the DeleteActivity method.
        /// void DeleteActivity(string id)
        /// </summary>
        [Test]
        public void TestDeleteActivity()
        {
            //First save an entity
            TestSaveActivity1();
            HermesActivity beforeDelete = hsipp.GetActivity("99887766998877669988776699887766");

            //Now delete
            hsipp.DeleteActivity("99887766998877669988776699887766");

            //Try to get the entity with given id
            HermesActivity returned = hsipp.GetActivity("12345678123456781234567812345678");
            Assert.IsNull(returned, "Deletion was not performed.");

            //Check record in history table
            CheckHistoryRecord("activity_hist",
                new string[] { "activity_id", "action" },
                new string[] { GuidStringToOracleGuidString(beforeDelete.Id), "D" });
        }

        /// <summary>
        /// Tests the DeleteActivity method when entity with given id was not found.
        /// void DeleteActivity(string id)
        /// EntityNotFoundException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteActivityFail1()
        {
            hsipp.DeleteActivity("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        }

        /// <summary>
        /// Tests the DeleteActivityGroup method.
        /// void DeleteActivityGroup(string id)
        /// </summary>
        [Test]
        public void TestDeleteActivityGroup()
        {
            //First save an entity
            TestSaveActivityGroup1();
            HermesActivityGroup beforeDelete = hsipp.GetActivityGroup("1234567890ABCDEF1234567890ABCDEF");

            //Now delete
            hsipp.DeleteActivityGroup("1234567890ABCDEF1234567890ABCDEF");

            //Try to get the entity with given id
            HermesActivityGroup returned = hsipp.GetActivityGroup("1234567890ABCDEF1234567890ABCDEF");
            Assert.IsNull(returned, "Deletion was not performed.");

            //Check record in history table
            CheckHistoryRecord("activity_group_hist",
                new string[] { "activity_group_id", "action" },
                new string[] { GuidStringToOracleGuidString(beforeDelete.Id), "D" });
        }

        /// <summary>
        /// Tests the DeleteActivityGroup method when entity with given id was not found.
        /// void DeleteActivityGroup(string id)
        /// EntityNotFoundException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteActivityGroupFail1()
        {
            hsipp.DeleteActivityGroup("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        }

        /// <summary>
        /// Tests the DeleteActivityType method.
        /// void DeleteActivityType(string id)
        /// </summary>
        [Test]
        public void TestDeleteActivityType()
        {
            //First save an entity
            TestSaveActivityType1();
            HermesActivityType beforeDelete = hsipp.GetActivityType("34563456345634563456345634563456");

            //Now delete
            hsipp.DeleteActivityType("34563456345634563456345634563456");

            //Try to get the entity with given id
            HermesActivityType returned = hsipp.GetActivityType("34563456345634563456345634563456");
            Assert.IsNull(returned, "Deletion was not performed.");

            //Check record in history table
            CheckHistoryRecord("activity_type_hist",
                new string[] { "activity_type_id", "action" },
                new string[] { GuidStringToOracleGuidString(beforeDelete.Id), "D" });
        }

        /// <summary>
        /// Tests the DeleteActivityType method when entity with given id was not found.
        /// void DeleteActivityType(string id)
        /// EntityNotFoundException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteActivityTypeFail1()
        {
            hsipp.DeleteActivityType("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        }

        /// <summary>
        /// Tests the DeleteScheduleItem method.
        /// void DeleteScheduleItem(string id)
        /// </summary>
        [Test]
        public void TestDeleteScheduleItem()
        {
            //First save an entity
            TestSaveScheduleItem1();
            HermesScheduleItem beforeDelete = hsipp.GetScheduleItem("19081908190819081908190819081908");

            //Now delete
            hsipp.DeleteScheduleItem("19081908190819081908190819081908");

            //Try to get the entity with given id
            HermesScheduleItem returned = hsipp.GetScheduleItem("19081908190819081908190819081908");
            Assert.IsNull(returned, "Deletion was not performed.");
        }

        /// <summary>
        /// Tests the DeleteScheduleItem method when entity with given id was not found.
        /// void DeleteScheduleItem(string id)
        /// EntityNotFoundException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteScheduleItemFail1()
        {
            hsipp.DeleteScheduleItem("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatus method.
        /// void DeleteScheduleItemStatus(string id)
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemStatus()
        {
            //First save an ScheduleItemStatus
            TestSaveScheduleItemStatus1();
            HermesScheduleItemStatus beforeDelete = hsipp.GetScheduleItemStatus("12345678123456781234567812345678");

            //Now delete
            hsipp.DeleteScheduleItemStatus("12345678123456781234567812345678");

            //Try to get the ScheduleItemStatus with given id
            HermesScheduleItemStatus returned = hsipp.GetScheduleItemStatus("12345678123456781234567812345678");
            Assert.IsNull(returned, "Deletion was not performed.");

            //Check record in history table
            CheckHistoryRecord("sched_status_hist",
                new string[] { "sched_status_id", "action" },
                new string[] { GuidStringToOracleGuidString(beforeDelete.Id), "D" });
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatus method when entity with given id was not found.
        /// void DeleteScheduleItemStatus(string id)
        /// EntityNotFoundException is expected
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteScheduleItemStatusFail1()
        {
            hsipp.DeleteScheduleItemStatus("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatus method.
        /// void DeleteScheduleItemRequestStatus(string id)
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemRequestStatus()
        {
            //First save an ScheduleItemRequestStatus
            TestSaveScheduleItemRequestStatus1();
            HermesScheduleItemRequestStatus beforeDelete =
                hsipp.GetScheduleItemRequestStatus("55665566556655665566556655665566");

            //Now delete
            hsipp.DeleteScheduleItemRequestStatus("55665566556655665566556655665566");

            //Try to get the ScheduleItemRequestStatus with given id
            HermesScheduleItemRequestStatus returned =
                hsipp.GetScheduleItemRequestStatus("55665566556655665566556655665566");
            Assert.IsNull(returned, "Deletion was not performed.");

            //Check record in history table
            CheckHistoryRecord("sched_request_status_hist",
                new string[] { "sched_request_status_id", "action" },
                new string[] { GuidStringToOracleGuidString(beforeDelete.Id), "D" });
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatus method when entity with given id was not found.
        /// void DeleteScheduleItemRequestStatus(string id)
        /// EntityNotFoundException is expected
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteScheduleItemRequestStatusFail1()
        {
            hsipp.DeleteScheduleItemRequestStatus("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
        }

        /// <summary>
        /// Tests the GetActivity method.
        /// HermesActivity GetActivity(string id)
        /// </summary>
        [Test]
        public void TestGetActivity()
        {
            HermesActivity obj = hsipp.GetActivity("33333333333333333333333333333333");
            Assert.IsNotNull(obj, "Method must return non-null entity instance.");

            obj = hsipp.GetActivity("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.IsNull(obj, "Method must return null if entity for given id is not present");
        }

        /// <summary>
        /// Tests the GetActivity method for failure when id is invalid.
        /// HermesActivity GetActivity(string id)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetActivityFail1()
        {
            hsipp.GetActivity("kljzsdfgjkzdbskljgakljsbg");
        }

        /// <summary>
        /// Tests the GetActivityGroup method.
        /// HermesActivityGroup GetActivityGroup(string id)
        /// </summary>
        [Test]
        public void TestGetActivityGroup()
        {
            HermesActivityGroup obj = hsipp.GetActivityGroup("11111111111111111111111111111111");
            Assert.IsNotNull(obj, "Method must return non-null entity instance.");

            obj = hsipp.GetActivityGroup("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.IsNull(obj, "Method must return null if entity for given id is not present");
        }

        /// <summary>
        /// Tests the GetActivityGroup method for failure when id is invalid.
        /// HermesActivityGroup GetActivityGroup(string id)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetActivityGroupFail1()
        {
            hsipp.GetActivityGroup("kljzsdfgjkzdbskljgakljsbg");
        }

        /// <summary>
        /// Tests the GetActivityType method.
        /// HermesActivityType GetActivityType(string id)
        /// </summary>
        [Test]
        public void TestGetActivityType()
        {
            HermesActivityType obj = hsipp.GetActivityType("22222222222222222222222222222222");
            Assert.IsNotNull(obj, "Method must return non-null entity instance.");

            obj = hsipp.GetActivityType("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.IsNull(obj, "Method must return null if entity for given id is not present");
        }

        /// <summary>
        /// Tests the GetActivityType method for failure when id is invalid.
        /// HermesActivityType GetActivityType(string id)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetActivityTypeFail1()
        {
            hsipp.GetActivityType("kljzsdfgjkzdbskljgakljsbg");
        }

        /// <summary>
        /// Tests the GetScheduleItem method.
        /// HermesScheduleItem GetScheduleItem(string id)
        /// </summary>
        [Test]
        public void TestGetScheduleItem()
        {
            HermesScheduleItem obj = hsipp.GetScheduleItem("66666666666666666666666666666666");
            Assert.IsNotNull(obj, "Method must return non-null entity instance.");

            obj = hsipp.GetScheduleItem("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.IsNull(obj, "Method must return null if entity for given id is not present");
        }

        /// <summary>
        /// Tests the GetScheduleItem method for failure when id is invalid.
        /// HermesScheduleItem GetScheduleItem(string id)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemFail1()
        {
            hsipp.GetScheduleItem("kljzsdfgjkzdbskljgakljsbg");
        }

        /// <summary>
        /// Tests the GetScheduleItemStatus method.
        /// HermesScheduleItemStatus GetScheduleItemStatus(string id)
        /// </summary>
        [Test]
        public void TestGetScheduleItemStatus()
        {
            HermesScheduleItemStatus obj = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            Assert.IsNotNull(obj, "Method must return non-null entity instance.");

            obj = hsipp.GetScheduleItemStatus("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.IsNull(obj, "Method must return null if entity for given id is not present");
        }

        /// <summary>
        /// Tests the GetScheduleItemStatus method for failure when id is invalid.
        /// HermesScheduleItemStatus GetScheduleItemStatus(string id)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemStatusFail1()
        {
            hsipp.GetScheduleItemStatus("kljzsdfgjkzdbskljgakljsbg");
        }

        /// <summary>
        /// Tests the GetScheduleItemRequestStatus method.
        /// HermesScheduleItemRequestStatus GetScheduleItemRequestStatus(string id)
        /// </summary>
        [Test]
        public void TestGetScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus obj = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            Assert.IsNotNull(obj, "Method must return non-null entity instance.");

            obj = hsipp.GetScheduleItemRequestStatus("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.IsNull(obj, "Method must return null if entity for given id is not present");
        }

        /// <summary>
        /// Tests the GetScheduleItemRequestStatus method for failure when id is invalid.
        /// HermesScheduleItemRequestStatus GetScheduleItemRequestStatus(string id)
        /// ScheduleItemPersistenceException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemRequestStatusFail1()
        {
            hsipp.GetScheduleItemRequestStatus("kljzsdfgjkzdbskljgakljsbg");
        }

        /// <summary>
        /// Tests the GetAllActivities method.
        /// IList`1 GetAllActivities(bool showDisabled)
        /// </summary>
        [Test]
        public void TestGetAllActivities()
        {
            //Create an activity and save it
            HermesActivity activity = new HermesActivity();
            activity.Id = new Guid("99887766998877669988776699887766").ToString();
            activity.Abbreviation = "ACT";
            activity.ActivityType = hsipp.GetActivityType("22222222222222222222222222222222");
            activity.DefaultDuration = new decimal(33.45);
            activity.DefaultExpireDays = 666;
            activity.DefaultStartTime = 721;
            activity.Enabled = false;
            activity.ExclusiveFlag = true;
            activity.LastModifiedBy = "ACTuser";
            activity.LastModifiedDate = new DateTime(2007, 8, 8);
            activity.Name = "CoolAndReallyNiceActivity";
            activity.WorkDayAmount = 9966;
            hsipp.SaveActivity(activity);

            //Get all with showDisabled as true
            IList<HermesActivity> activities = hsipp.GetAllActivities(true);
            Assert.AreEqual(2, activities.Count,
                "Must return 2 as 1 activity is already present and showDisabled is true.");

            //Get only enabled
            activities = hsipp.GetAllActivities(false);
            Assert.AreEqual(1, activities.Count,
                "Must return 1 as 1 activity is already present and showDisabled is false.");
        }

        /// <summary>
        /// Tests the GetAllActivityGroups method.
        /// IList`1 GetAllActivityGroups()
        /// </summary>
        [Test]
        public void TestGetAllActivityGroups()
        {
            //Create activity group and save it
            HermesActivityGroup hag = new HermesActivityGroup();
            hag.Id = new Guid("1234567890ABCDEF1234567890ABCDEF").ToString();
            hag.Abbreviation = "HAG1";
            hag.Name = "HermesActivityGroup1";
            hag.LastModifiedBy = "hagUser";
            hag.LastModifiedDate = new DateTime(2007, 7, 7);
            hsipp.SaveActivityGroup(hag);

            IList<HermesActivityGroup> entities = hsipp.GetAllActivityGroups();
            Assert.AreEqual(2, entities.Count,
                "Must return 2 as 1 activity group is already present.");
        }

        /// <summary>
        /// Tests the GetAllActivityTypes method.
        /// IList`1 GetAllActivityTypes()
        /// </summary>
        [Test]
        public void TestGetAllActivityTypes()
        {
            HermesActivityType hat = new HermesActivityType();
            hat.Id = new Guid("34563456345634563456345634563456").ToString();
            hat.Abbreviation = "HAT1";
            hat.Name = "HermesActivityType1";
            hat.LastModifiedBy = "hatUser";
            hat.LastModifiedDate = new DateTime(2007, 11, 7);
            hat.ActivityGroup = hsipp.GetActivityGroup("11111111111111111111111111111111");
            hsipp.SaveActivityType(hat);

            IList<HermesActivityType> entities = hsipp.GetAllActivityTypes();
            Assert.AreEqual(2, entities.Count,
                "Must return 2 as 1 activity type is already present.");
        }

        /// <summary>
        /// Tests the GetAllScheduleItemStatuses method.
        /// IList`1 GetAllScheduleItemStatuses()
        /// </summary>
        [Test]
        public void TestGetAllScheduleItemStatuses()
        {
            HermesScheduleItemStatus hsis = new HermesScheduleItemStatus();
            hsis.Id = new Guid("12345678123456781234567812345678").ToString();
            hsis.Abbreviation = "HSIS1";
            hsis.Description = "HermesScheduleItemStatus1";
            hsis.LastModifiedBy = "hsisUser";
            hsis.LastModifiedDate = new DateTime(2007, 11, 12);
            hsipp.SaveScheduleItemStatus(hsis);

            IList<HermesScheduleItemStatus> entities = hsipp.GetAllScheduleItemStatuses();
            Assert.AreEqual(2, entities.Count,
                "Must return 2 as 1 schedule item status is already present.");
        }

        /// <summary>
        /// Tests the GetAllScheduleRequestStatuses method.
        /// IList`1 GetAllScheduleRequestStatuses()
        /// </summary>
        [Test]
        public void TestGetAllScheduleRequestStatuses()
        {
            HermesScheduleItemRequestStatus hsirs = new HermesScheduleItemRequestStatus();
            hsirs.Id = new Guid("55665566556655665566556655665566").ToString();
            hsirs.Abbreviation = "HSIRS1";
            hsirs.Description = "HermesScheduleItemRequestStatus1";
            hsirs.LastModifiedBy = "hsirsUser";
            hsirs.LastModifiedDate = new DateTime(2002, 11, 12);
            hsipp.SaveScheduleItemRequestStatus(hsirs);

            IList<HermesScheduleItemRequestStatus> entities = hsipp.GetAllScheduleItemRequestStatuses();
            Assert.AreEqual(2, entities.Count,
                "Must return 2 as 1 schedule item statuses is already present.");
        }

        /// <summary>
        /// Tests the CreateScheduleItemPublishEditCopyRelationship method.
        /// void CreateScheduleItemPublishEditCopyRelationship(HermesScheduleItem parent, HermesScheduleItem editCopy)
        /// </summary>
        [Test]
        public void TestCreateScheduleItemPublishEditCopyRelationship()
        {
            //Create a ScheduleItem and save
            HermesScheduleItem hsiParent = new HermesScheduleItem();
            hsiParent.Id = new Guid("19081908190819081908190819081908").ToString();
            hsiParent.Activity = hsipp.GetActivity("33333333333333333333333333333333");
            hsiParent.Duration = new decimal(546.89);
            hsiParent.ExceptionFlag = 'Y';
            hsiParent.ExpirationDate = new DateTime(2005, 12, 23);
            hsiParent.LastModifiedBy = "ivern";
            hsiParent.LastModifiedDate = new DateTime(2007, 12, 1);
            HermesGenericNote note = new HermesGenericNote();
            note.Id = new Guid("12340987123409871234098712340987").ToString();
            hsiParent.Note = note;
            hsiParent.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsiParent.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsiParent.Version = 1;
            hsiParent.WorkDate = new DateTime(2004, 12, 12);
            hsiParent.WorkDayAmount = new Decimal(86787.23);
            hsipp.SaveScheduleItem(hsiParent);

            //Create another ScheduleItem and save
            HermesScheduleItem hsiEdit = new HermesScheduleItem();
            hsiEdit.Id = new Guid("23451987234519872345198723451987").ToString();
            hsiEdit.Activity = hsipp.GetActivity("33333333333333333333333333333333");
            hsiEdit.Duration = new decimal(546.89);
            hsiEdit.ExceptionFlag = 'Y';
            hsiEdit.ExpirationDate = new DateTime(2005, 12, 23);
            hsiEdit.LastModifiedBy = "ivern";
            hsiEdit.LastModifiedDate = new DateTime(2007, 12, 1);
            HermesGenericNote note2 = new HermesGenericNote();
            note2.Id = new Guid("12340987123409871234098712340987").ToString();
            hsiEdit.Note = note2;
            hsiEdit.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsiEdit.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsiEdit.Version = 1;
            hsiEdit.WorkDate = new DateTime(2004, 12, 12);
            hsiEdit.WorkDayAmount = new Decimal(86787.23);
            hsipp.SaveScheduleItem(hsiEdit);

            //Create relation
            hsipp.CreateScheduleItemPublishEditCopyRelationship(hsiParent, hsiEdit);

            //Get edit copy from db and compare to original
            HermesScheduleItem editFromDb = hsipp.GetScheduleItemEditCopy(hsiParent);
            CompareProperties(hsiEdit, editFromDb);

            //Get parent from db and compare to original
            HermesScheduleItem parentFromDb = hsipp.GetScheduleItemParentCopy(hsiEdit);
            CompareProperties(hsiParent, parentFromDb);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemPublishEditCopyRelationship method.
        /// void DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem editCopy)
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemPublishEditCopyRelationship()
        {
            //Create a ScheduleItem and save
            HermesScheduleItem hsiParent = new HermesScheduleItem();
            hsiParent.Id = new Guid("19081908190819081908190819081908").ToString();
            hsiParent.Activity = hsipp.GetActivity("33333333333333333333333333333333");
            hsiParent.Duration = new decimal(546.89);
            hsiParent.ExceptionFlag = 'Y';
            hsiParent.ExpirationDate = new DateTime(2005, 12, 23);
            hsiParent.LastModifiedBy = "ivern";
            hsiParent.LastModifiedDate = new DateTime(2007, 12, 1);
            HermesGenericNote note = new HermesGenericNote();
            note.Id = new Guid("12340987123409871234098712340987").ToString();
            hsiParent.Note = note;
            hsiParent.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsiParent.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsiParent.Version = 1;
            hsiParent.WorkDate = new DateTime(2004, 12, 12);
            hsiParent.WorkDayAmount = new Decimal(86787.23);
            hsipp.SaveScheduleItem(hsiParent);

            //Create another ScheduleItem and save
            HermesScheduleItem hsiEdit = new HermesScheduleItem();
            hsiEdit.Id = new Guid("23451987234519872345198723451987").ToString();
            hsiEdit.Activity = hsipp.GetActivity("33333333333333333333333333333333");
            hsiEdit.Duration = new decimal(546.89);
            hsiEdit.ExceptionFlag = 'Y';
            hsiEdit.ExpirationDate = new DateTime(2005, 12, 23);
            hsiEdit.LastModifiedBy = "ivern";
            hsiEdit.LastModifiedDate = new DateTime(2007, 12, 1);
            HermesGenericNote note2 = new HermesGenericNote();
            note2.Id = new Guid("12340987123409871234098712340987").ToString();
            hsiEdit.Note = note2;
            hsiEdit.ScheduleItemRequestStatus = hsipp.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            hsiEdit.ScheduleItemStatus = hsipp.GetScheduleItemStatus("44444444444444444444444444444444");
            hsiEdit.Version = 1;
            hsiEdit.WorkDate = new DateTime(2004, 12, 12);
            hsiEdit.WorkDayAmount = new Decimal(86787.23);
            hsipp.SaveScheduleItem(hsiEdit);

            //Create relation
            hsipp.CreateScheduleItemPublishEditCopyRelationship(hsiParent, hsiEdit);

            //Now delete
            hsipp.DeleteScheduleItemPublishEditCopyRelationship(hsiEdit);

            //Verify delete
            Assert.IsNull(hsipp.GetScheduleItemEditCopy(hsiParent), "Delete was not performed correctly.");
            Assert.IsNull(hsipp.GetScheduleItemParentCopy(hsiEdit), "Delete was not performed correctly.");
        }

        /// <summary>
        /// Tests the GetScheduleItemEditCopy method.
        /// HermesScheduleItem GetScheduleItemEditCopy(HermesScheduleItem parent)
        /// </summary>
        [Test]
        public void TestGetScheduleItemEditCopy()
        {
            //This test is essentially the test for GetScheduleItemEditCopy
            TestCreateScheduleItemPublishEditCopyRelationship();
        }

        /// <summary>
        /// Tests the GetScheduleItemParentCopy method.
        /// HermesScheduleItem GetScheduleItemParentCopy(HermesScheduleItem editCopy)
        /// </summary>
        [Test]
        public void TestGetScheduleItemParentCopy()
        {
            //This test is essentially the test for GetScheduleItemParentCopy
            TestCreateScheduleItemPublishEditCopyRelationship();
        }

        /// <summary>
        /// Gets the value of a private field of an object using reflection API
        /// </summary>
        /// <param name="obj">The object from which to load the value of field</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The value of the field.</returns>
        private static object GetPrivateField(object obj, string fieldName)
        {
            FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return field.GetValue(obj);
        }

        /// <summary>
        /// General method for comparing the property values for any 2 objects of the same type.
        /// It is used for comapring 2 entities in the tests.
        /// This method is recursive in nature. For property types other than ValueType or string, the method
        /// is recursively called to check all the child properties too.
        /// </summary>
        /// <param name="obj1">The first object</param>
        /// <param name="obj2">The second object</param>
        /// <returns>true if all properties of the 2 objects are same; false otherwise.</returns>
        private static bool CompareProperties(object obj1, object obj2)
        {
            //Both are null
            if (object.Equals(obj1, obj2))
            {
                return true;
            }
            if (obj1 == null || obj2 == null)
            {
                return false;
            }

            PropertyInfo[] props = obj1.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "Values" || prop.Name == "Note")
                {
                    continue;
                }

                object propValue1 = prop.GetValue(obj1, null);
                object propValue2 = prop.GetValue(obj2, null);

                if (prop.PropertyType.IsValueType || prop.PropertyType.Equals(typeof(string)))
                {
                    Assert.IsTrue(object.Equals(propValue1, propValue2),
                        prop.Name + " property values are different.");
                }
                else
                {
                    Assert.IsTrue(CompareProperties(propValue1, propValue2),
                        prop.Name + " property values are different.");
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether a history record exists in the given history table name.
        /// It checks by creating a simple 'select' statement with the 'where' clause defined by the
        /// fieldNames and fieldValues arrays denoting the left and right side of each predicate of the 'where' clause.
        /// </summary>
        /// <param name="tableName">The name of the history table in which to search.</param>
        /// <param name="fieldNames">The left hand sides of the predicates of the 'where' clause</param>
        /// <param name="fieldValues">The right hand sides of the predicates of the 'where' clause</param>
        private static void CheckHistoryRecord(string tableName, string[] fieldNames, string[] fieldValues)
        {
            //Get the connection string from config
            string connectionName = ConfigManager.GetInstance().GetValue(
                HermesScheduleItemPersistenceProvider.DefaultNamespace, "connectionName");

            using (OracleConnection conn = OracleConnectionHelper.GetPooledConnection(null, connectionName))
            {
                using (OracleCommand command = new OracleCommand())
                {
                    //Create query for looking for the exact history record
                    string commandText = "SELECT * FROM " + tableName + " WHERE ";
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        if (i == fieldNames.Length - 1)
                        {
                            commandText += fieldNames[i] + "='" + fieldValues[i] + "'";
                        }
                        else
                        {
                            commandText += fieldNames[i] + "='" + fieldValues[i] + "' AND ";
                        }
                    }

                    //Set text and connection
                    command.CommandText = commandText;
                    command.Connection = conn;
                    conn.Open();

                    //Get the row to find in the history table
                    int rowCount = 0;
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            rowCount++;
                        }
                    }

                    Assert.AreEqual(1, rowCount,
                        "One and only one record must be found in history table for given action.");
                }
            }
        }

        /// <summary>
        /// Converts a string denoting a guid to another string denoting the guid
        /// in the form in which Oracle understands.
        /// </summary>
        /// <param name="guidString">The normal guid string</param>
        /// <returns>The Oracle friendly guid string</returns>
        private static string GuidStringToOracleGuidString(string guidString)
        {
            byte[] byteArr = new Guid(guidString).ToByteArray();
            string ret = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                ret += byteArr[i].ToString("X");
            }
            return ret;
        }
    }
}
