// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.GenericNotes;
using NUnit.Framework;
using TopCoder.Util.ConfigurationManager;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// Demonstrates the usage of the component.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DemoClass
    {
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
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            UnitTestHelper.ClearTestDatabase();
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Demo for the component.
        /// </summary>
        [Test]
        public void Demo()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();

            ///////////////////INSERT UPDATE GET DELETE/////////////////////////////
            // Create an activity group
            HermesActivityGroup hag = new HermesActivityGroup();
            hag.Id = new Guid("1234567890ABCDEF1234567890ABCDEF").ToString();
            hag.Abbreviation = "HAG1";
            hag.Name = "HermesActivityGroup1";
            hag.LastModifiedBy = "hagUser";
            hag.LastModifiedDate = new DateTime(2007, 7, 7);
            //Save it into database
            hsipp.SaveActivityGroup(hag);
            //Get it from database
            HermesActivityGroup getFromDb = hsipp.GetActivityGroup("1234567890ABCDEF1234567890ABCDEF");
            //Delete it from database
            hsipp.DeleteActivityGroup("1234567890ABCDEF1234567890ABCDEF");

            //Create an activity type
            HermesActivityType hat = new HermesActivityType();
            hat.Id = new Guid("34563456345634563456345634563456").ToString();
            hat.Abbreviation = "HAT1";
            hat.Name = "HermesActivityType1";
            hat.LastModifiedBy = "hatUser";
            hat.LastModifiedDate = new DateTime(2007, 11, 7);
            hat.ActivityGroup = hsipp.GetActivityGroup("11111111111111111111111111111111");
            //Save it into database
            hsipp.SaveActivityType(hat);
            //Get it from database
            HermesActivityType getFromDb2 = hsipp.GetActivityType("34563456345634563456345634563456");
            //Delete it from database
            hsipp.DeleteActivityType("34563456345634563456345634563456");

            //Create an activity
            HermesActivity activity = new HermesActivity();
            activity.Id = new Guid("99887766998877669988776699887766").ToString();
            activity.Abbreviation = "ACT";
            activity.ActivityType = hsipp.GetActivityType("22222222222222222222222222222222");
            activity.DefaultDuration = new decimal(33.45);
            activity.DefaultExpireDays = 666;
            activity.DefaultStartTime = 432;
            activity.Enabled = true;
            activity.ExclusiveFlag = true;
            activity.LastModifiedBy = "ACTuser";
            activity.LastModifiedDate = new DateTime(2007, 8, 8);
            activity.Name = "CoolAndReallyNiceActivity";
            activity.WorkDayAmount = 9966;
            //Save it into database
            hsipp.SaveActivity(activity);
            //Get it from database
            HermesActivity getFromDb3 = hsipp.GetActivity("99887766998877669988776699887766");
            //Delete it from database
            hsipp.DeleteActivity("99887766998877669988776699887766");

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

            //....and so on for other properties



            /////////////////GET ALL ENTITIES//////////
            IList<HermesScheduleItemRequestStatus> reqStats = hsipp.GetAllScheduleItemRequestStatuses();
            IList<HermesScheduleItemStatus> statuses = hsipp.GetAllScheduleItemStatuses();
            IList<HermesActivity> items = hsipp.GetAllActivities(false);

            //....and so on for other properties



            //////////////MANAGE SCHEDULE ITEMS RELATIONS////////////////
            hsipp.CreateScheduleItemPublishEditCopyRelationship(hsiParent, hsiEdit);
            hsipp.GetScheduleItemEditCopy(hsiParent);
            hsipp.GetScheduleItemParentCopy(hsiEdit);
            hsipp.DeleteScheduleItemPublishEditCopyRelationship(hsiEdit);

        }
    }
}
