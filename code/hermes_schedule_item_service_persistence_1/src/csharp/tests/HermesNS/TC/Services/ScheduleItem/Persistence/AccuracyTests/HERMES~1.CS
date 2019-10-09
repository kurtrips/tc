/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
using System;
using NUnit.Framework;
using System.Collections.Generic;
using HermesNS.TC.Services.GenericNotes;
using HermesNS.TC.Services.ScheduleItem.Entities;
using TopCoder.Services.WCF.ScheduleItem.Persistence;

namespace HermesNS.TC.Services.ScheduleItem.Persistence.AccuracyTests
{
    /// <summary>
    /// <p>Accuracy tests for the <c>HermesScheduleItemPersistenceProvider</c> class.</p>
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesScheduleItemPersistenceProviderAccuracyTests
    {
        /// <summary>
        /// <p>Instance of <c>HermesScheduleItemPersistenceProvider</c> used to test</p>
        /// </summary>
        private HermesScheduleItemPersistenceProvider instance = null;

        /// <summary>
        /// <p>Sets up test environment.</p>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            AccuracyTestsHelper.LoadConfiguration();
            AccuracyTestsHelper.ClearTables();
            instance = new HermesScheduleItemPersistenceProvider();
        }

        /// <summary>
        /// <p>Tears down test environment.</p>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            AccuracyTestsHelper.ClearTables();
            AccuracyTestsHelper.ClearConfiguration();
            instance = null;
        }

        /// <summary>
        /// <p>Accuracy test <c>HermesScheduleItemPersistenceProvider()</c> constructor.</p>
        /// </summary>
        [Test]
        public void TestConstructor_1()
        {
            Assert.IsNotNull(instance, "Initialize of HermesScheduleItemPersistenceProvider is wrong.");
            Assert.IsTrue(instance is IScheduleItemPersistenceProvider<string, HermesScheduleItem,
                HermesActivity, HermesScheduleItemStatus, HermesScheduleItemRequestStatus,
                HermesActivityGroup, HermesActivityType, HermesGenericNote, HermesGenericNoteItem,
                HermesGenericNoteItemHistory>, "HermesScheduleItemPersistenceProvider should inherit" +
                " from IScheduleItemPersistenceProvider");
        }

        /// <summary>
        /// <p>Accuracy test <c>HermesScheduleItemPersistenceProvider(string nameSpace)</c> constructor.</p>
        /// </summary>
        [Test]
        public void TestConstructor_2()
        {
            instance = new HermesScheduleItemPersistenceProvider(typeof(
                HermesScheduleItemPersistenceProvider).FullName);
            Assert.IsNotNull(instance, "Initialize of HermesScheduleItemPersistenceProvider is wrong.");
            Assert.IsTrue(instance is IScheduleItemPersistenceProvider<string, HermesScheduleItem,
                HermesActivity, HermesScheduleItemStatus, HermesScheduleItemRequestStatus,
                HermesActivityGroup, HermesActivityType, HermesGenericNote, HermesGenericNoteItem,
                HermesGenericNoteItemHistory>, "HermesScheduleItemPersistenceProvider should inherit" +
                " from IScheduleItemPersistenceProvider");
        }

        /// <summary>
        /// <p>Accuracy test <c>HermesScheduleItemPersistenceProvider(string, IScheduleItemHelperBase)</c>
        /// constructor.</p>
        /// </summary>
        [Test]
        public void TestConstructor_3()
        {
            instance = new HermesScheduleItemPersistenceProvider("conn",
                new HermesScheduleItemPersistenceHelper());
            Assert.IsNotNull(instance, "Initialize of HermesScheduleItemPersistenceProvider is wrong.");
            Assert.IsTrue(instance is IScheduleItemPersistenceProvider<string, HermesScheduleItem,
                HermesActivity, HermesScheduleItemStatus, HermesScheduleItemRequestStatus,
                HermesActivityGroup, HermesActivityType, HermesGenericNote, HermesGenericNoteItem,
                HermesGenericNoteItemHistory>, "HermesScheduleItemPersistenceProvider should inherit" +
                " from IScheduleItemPersistenceProvider");
        }

        /// <summary>
        /// <p>Accuracy test the <c>SaveActivity(HermesActivity activity)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestSaveActivity_Accuracy()
        {
            HermesActivity first = AccuracyTestsHelper.CreateHermesActivity();
            instance.SaveActivityGroup(first.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.ActivityType);
            instance.SaveActivity(first);

            // Verify
            HermesActivity second = instance.GetActivity(first.Id);
            AccuracyTestsHelper.VerifyHermesActivity(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>SaveActivityGroup(HermesActivityGroup activityGroup)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestSaveActivityGroup_Accuracy()
        {
            HermesActivityGroup first = AccuracyTestsHelper.CreateHermesActivityGroup();
            instance.SaveActivityGroup(first);

            // Verify
            HermesActivityGroup second = instance.GetActivityGroup(first.Id);
            AccuracyTestsHelper.VerifyHermesActivityGroup(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>SaveActivityType(HermesActivityType activityType)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestSaveActivityType_Accuracy()
        {
            HermesActivityType first = AccuracyTestsHelper.CreateHermesActivityType();
            instance.SaveActivityGroup(first.ActivityGroup);
            instance.SaveActivityType(first);

            // Verify
            HermesActivityType second = instance.GetActivityType(first.Id);
            AccuracyTestsHelper.VerifyHermesActivityType(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>SaveScheduleItem(HermesScheduleItem scheduleItem)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestSaveScheduleItem_Accuracy()
        {
            HermesScheduleItem first = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(first.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.Activity.ActivityType);
            instance.SaveActivity(first.Activity);
            instance.SaveScheduleItemRequestStatus(first.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(first.ScheduleItemStatus);
            instance.SaveScheduleItem(first);

            // Verify
            HermesScheduleItem second = instance.GetScheduleItem(first.Id);
            AccuracyTestsHelper.VerifyHermesScheduleItem(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>SaveScheduleItemStatus(HermesScheduleItemStatus)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestSaveScheduleItemStatus_Accuracy()
        {
            HermesScheduleItemStatus first = AccuracyTestsHelper.CreateHermesScheduleItemStatus();
            instance.SaveScheduleItemStatus(first);

            // Verify
            HermesScheduleItemStatus second = instance.GetScheduleItemStatus(first.Id);
            AccuracyTestsHelper.VerifyHermesScheduleItemStatus(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>SaveScheduleItemRequestStatus(HermesScheduleItemRequestStatus)</c>
        /// method.</p>.
        /// </summary>
        [Test]
        public void TestSaveScheduleItemRequestStatus_Accuracy()
        {
            HermesScheduleItemRequestStatus first = AccuracyTestsHelper.CreateHermesScheduleItemRequestStatus();
            instance.SaveScheduleItemRequestStatus(first);

            // Verify
            HermesScheduleItemRequestStatus second = instance.GetScheduleItemRequestStatus(first.Id);
            AccuracyTestsHelper.VerifyHermesScheduleItemRequestStatus(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteActivity(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteActivity_Accuracy()
        {
            HermesActivity first = AccuracyTestsHelper.CreateHermesActivity();
            instance.SaveActivityGroup(first.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.ActivityType);
            instance.SaveActivity(first);

            // Verify
            HermesActivity second = instance.GetActivity(first.Id);
            Assert.IsNotNull(second, "SaveActivity is wrong.");

            instance.DeleteActivity(first.Id);

            // Verify
            second = instance.GetActivity(first.Id);
            Assert.IsNull(second, "DeleteActivity is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteActivityGroup(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteActivityGroup_Accuracy()
        {
            HermesActivityGroup first = AccuracyTestsHelper.CreateHermesActivityGroup();
            instance.SaveActivityGroup(first);

            // Verify
            HermesActivityGroup second = instance.GetActivityGroup(first.Id);
            Assert.IsNotNull(second, "SaveActivityGroup is wrong.");

            instance.DeleteActivityGroup(first.Id);

            // Verify
            second = instance.GetActivityGroup(first.Id);
            Assert.IsNull(second, "DeleteActivityGroup is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteActivityType(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteActivityType_Accuracy()
        {
            HermesActivityType first = AccuracyTestsHelper.CreateHermesActivityType();
            instance.SaveActivityGroup(first.ActivityGroup);
            instance.SaveActivityType(first);

            // Verify
            HermesActivityType second = instance.GetActivityType(first.Id);
            Assert.IsNotNull(second, "SaveActivityType is wrong.");

            instance.DeleteActivityType(first.Id);

            // Verify
            second = instance.GetActivityType(first.Id);
            Assert.IsNull(second, "DeleteActivityType is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteScheduleItem(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItem_Accuracy()
        {
            HermesScheduleItem first = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(first.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.Activity.ActivityType);
            instance.SaveActivity(first.Activity);
            instance.SaveScheduleItemRequestStatus(first.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(first.ScheduleItemStatus);
            instance.SaveScheduleItem(first);

            // Verify
            HermesScheduleItem second = instance.GetScheduleItem(first.Id);
            Assert.IsNotNull(second, "SaveScheduleItem is wrong.");

            instance.DeleteScheduleItem(first.Id);

            // Verify
            second = instance.GetScheduleItem(first.Id);
            Assert.IsNull(second, "DeleteScheduleItem is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteScheduleItemStatus(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemStatus_Accuracy()
        {
            HermesScheduleItemStatus first = AccuracyTestsHelper.CreateHermesScheduleItemStatus();
            instance.SaveScheduleItemStatus(first);

            // Verify
            HermesScheduleItemStatus second = instance.GetScheduleItemStatus(first.Id);
            Assert.IsNotNull(second, "SaveScheduleItemStatus is wrong.");

            instance.DeleteScheduleItemStatus(first.Id);

            // Verify
            second = instance.GetScheduleItemStatus(first.Id);
            Assert.IsNull(second, "DeleteScheduleItemStatus is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteScheduleItemRequestStatus(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemRequestStatus_Accuracy()
        {
            HermesScheduleItemRequestStatus first = AccuracyTestsHelper.CreateHermesScheduleItemRequestStatus();
            instance.SaveScheduleItemRequestStatus(first);

            // Verify
            HermesScheduleItemRequestStatus second = instance.GetScheduleItemRequestStatus(first.Id);
            Assert.IsNotNull(second, "SaveScheduleItemRequestStatus is wrong.");

            instance.DeleteScheduleItemRequestStatus(first.Id);

            // Verify
            second = instance.GetScheduleItemRequestStatus(first.Id);
            Assert.IsNull(second, "DeleteScheduleItemRequestStatus is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetActivity(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetActivity_Accuracy1()
        {
            HermesActivity first = AccuracyTestsHelper.CreateHermesActivity();
            instance.SaveActivityGroup(first.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.ActivityType);
            instance.SaveActivity(first);

            // Verify
            HermesActivity second = instance.GetActivity(first.Id);
            AccuracyTestsHelper.VerifyHermesActivity(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetActivity(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetActivity_Accuracy2()
        {
            HermesActivity first = AccuracyTestsHelper.CreateHermesActivity();
            HermesActivity second = instance.GetActivity(first.Id);
            Assert.IsNull(second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetActivityGroup(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetActivityGroup_Accuracy1()
        {
            HermesActivityGroup first = AccuracyTestsHelper.CreateHermesActivityGroup();
            instance.SaveActivityGroup(first);

            // Verify
            HermesActivityGroup second = instance.GetActivityGroup(first.Id);
            AccuracyTestsHelper.VerifyHermesActivityGroup(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetActivityGroup(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetActivityGroup_Accuracy2()
        {
            HermesActivityGroup first = AccuracyTestsHelper.CreateHermesActivityGroup();
            HermesActivityGroup second = instance.GetActivityGroup(first.Id);
            Assert.IsNull(second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetActivityType(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetActivityType_Accuracy1()
        {
            HermesActivityType first = AccuracyTestsHelper.CreateHermesActivityType();
            instance.SaveActivityGroup(first.ActivityGroup);
            instance.SaveActivityType(first);

            // Verify
            HermesActivityType second = instance.GetActivityType(first.Id);
            AccuracyTestsHelper.VerifyHermesActivityType(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetActivityType(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetActivityType_Accuracy2()
        {
            HermesActivityType first = AccuracyTestsHelper.CreateHermesActivityType();

            // Verify
            HermesActivityType second = instance.GetActivityType(first.Id);
            Assert.IsNull(second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetScheduleItem(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleItem_Accuracy1()
        {
            HermesScheduleItem first = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(first.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.Activity.ActivityType);
            instance.SaveActivity(first.Activity);
            instance.SaveScheduleItemRequestStatus(first.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(first.ScheduleItemStatus);
            instance.SaveScheduleItem(first);

            // Verify
            HermesScheduleItem second = instance.GetScheduleItem(first.Id);
            AccuracyTestsHelper.VerifyHermesScheduleItem(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetScheduleItem(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleItem_Accuracy2()
        {
            HermesScheduleItem first = AccuracyTestsHelper.CreateHermesScheduleItem();

            // Verify
            HermesScheduleItem second = instance.GetScheduleItem(first.Id);
            Assert.IsNull(second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetScheduleItemStatus(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleItemStatus_Accuracy1()
        {
            HermesScheduleItemStatus first = AccuracyTestsHelper.CreateHermesScheduleItemStatus();
            instance.SaveScheduleItemStatus(first);

            // Verify
            HermesScheduleItemStatus second = instance.GetScheduleItemStatus(first.Id);
            AccuracyTestsHelper.VerifyHermesScheduleItemStatus(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetScheduleItemStatus(string id)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleItemStatus_Accuracy2()
        {
            HermesScheduleItemStatus first = AccuracyTestsHelper.CreateHermesScheduleItemStatus();

            // Verify
            HermesScheduleItemStatus second = instance.GetScheduleItemStatus(first.Id);
            Assert.IsNull(second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetScheduleRequestStatus(string id)</c>
        /// method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleRequestStatus_Accuracy1()
        {
            HermesScheduleItemRequestStatus first = AccuracyTestsHelper.CreateHermesScheduleItemRequestStatus();
            instance.SaveScheduleItemRequestStatus(first);

            // Verify
            HermesScheduleItemRequestStatus second = instance.GetScheduleItemRequestStatus(first.Id);
            AccuracyTestsHelper.VerifyHermesScheduleItemRequestStatus(first, second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetScheduleRequestStatus(string id)</c>
        /// method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleRequestStatus_Accuracy2()
        {
            HermesScheduleItemRequestStatus first = AccuracyTestsHelper.CreateHermesScheduleItemRequestStatus();

            // Verify
            HermesScheduleItemRequestStatus second = instance.GetScheduleItemRequestStatus(first.Id);
            Assert.IsNull(second);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivities(bool showDisabled)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivities_Accuracy1()
        {
            HermesActivity first = AccuracyTestsHelper.CreateHermesActivity();
            instance.SaveActivityGroup(first.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.ActivityType);
            instance.SaveActivity(first);

            // Verify
            IList<HermesActivity> result = instance.GetAllActivities(true);
            Assert.AreEqual(1, result.Count, "GetAllActivities is wrong.");
            AccuracyTestsHelper.VerifyHermesActivity(first, result[0]);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivities(bool showDisabled)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivities_Accuracy2()
        {
            HermesActivity first = AccuracyTestsHelper.CreateHermesActivity();
            instance.SaveActivityGroup(first.ActivityType.ActivityGroup);
            instance.SaveActivityType(first.ActivityType);
            instance.SaveActivity(first);

            // Verify
            IList<HermesActivity> result = instance.GetAllActivities(false);
            Assert.AreEqual(0, result.Count, "GetAllActivities is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivities(bool showDisabled)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivities_Accuracy3()
        {
            IList<HermesActivity> result = instance.GetAllActivities(true);
            Assert.AreEqual(0, result.Count, "GetAllActivities is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivityGroups()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivityGroups_Accuracy1()
        {
            HermesActivityGroup first = AccuracyTestsHelper.CreateHermesActivityGroup();
            instance.SaveActivityGroup(first);

            // Verify
            IList<HermesActivityGroup> second = instance.GetAllActivityGroups();
            Assert.AreEqual(1, second.Count, "GetAllActivityGroups is wrong.");
            AccuracyTestsHelper.VerifyHermesActivityGroup(first, second[0]);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivityGroups()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivityGroups_Accuracy2()
        {
            HermesActivityGroup first = AccuracyTestsHelper.CreateHermesActivityGroup();
            IList<HermesActivityGroup> second = instance.GetAllActivityGroups();
            Assert.AreEqual(0, second.Count, "GetAllActivityGroups is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivityTypes()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivityTypes_Accuracy1()
        {
            HermesActivityType first = AccuracyTestsHelper.CreateHermesActivityType();
            instance.SaveActivityGroup(first.ActivityGroup);
            instance.SaveActivityType(first);

            // Verify
            IList<HermesActivityType> second = instance.GetAllActivityTypes();
            Assert.AreEqual(1, second.Count, "GetAllActivityGroups is wrong.");
            AccuracyTestsHelper.VerifyHermesActivityType(first, second[0]);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllActivityTypes()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllActivityTypes_Accuracy2()
        {
            HermesActivityType first = AccuracyTestsHelper.CreateHermesActivityType();

            // Verify
            IList<HermesActivityType> second = instance.GetAllActivityTypes();
            Assert.AreEqual(0, second.Count, "GetAllActivityTypes is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllScheduleItemStatuses()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllScheduleItemStatuses_Accuracy1()
        {
            HermesScheduleItemStatus first = AccuracyTestsHelper.CreateHermesScheduleItemStatus();
            instance.SaveScheduleItemStatus(first);

            // Verify
            IList<HermesScheduleItemStatus> second = instance.GetAllScheduleItemStatuses();
            Assert.AreEqual(1, second.Count, "GetAllScheduleItemStatuses is wrong.");
            AccuracyTestsHelper.VerifyHermesScheduleItemStatus(first, second[0]);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllScheduleItemStatuses()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllScheduleItemStatuses_Accuracy2()
        {
            HermesScheduleItemStatus first = AccuracyTestsHelper.CreateHermesScheduleItemStatus();

            // Verify
            IList<HermesScheduleItemStatus> second = instance.GetAllScheduleItemStatuses();
            Assert.AreEqual(0, second.Count, "GetAllScheduleItemStatuses is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllScheduleRequestStatuses()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllScheduleRequestStatuses_Accuracy1()
        {
            HermesScheduleItemRequestStatus first = AccuracyTestsHelper.CreateHermesScheduleItemRequestStatus();
            instance.SaveScheduleItemRequestStatus(first);

            // Verify
            IList<HermesScheduleItemRequestStatus> second = instance.GetAllScheduleItemRequestStatuses();
            Assert.AreEqual(1, second.Count, "GetAllScheduleRequestStatuses is wrong.");
            AccuracyTestsHelper.VerifyHermesScheduleItemRequestStatus(first, second[0]);
        }

        /// <summary>
        /// <p>Accuracy test the <c>GetAllScheduleRequestStatuses()</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetAllScheduleRequestStatuses_Accuracy2()
        {
            HermesScheduleItemRequestStatus first = AccuracyTestsHelper.CreateHermesScheduleItemRequestStatus();

            // Verify
            IList<HermesScheduleItemRequestStatus> second = instance.GetAllScheduleItemRequestStatuses();
            Assert.AreEqual(0, second.Count, "GetAllScheduleRequestStatuses is wrong.");
        }

        /// <summary>
        /// <p>Accuracy test the <c> CreateScheduleItemPublishEditCopyRelationship(HermesScheduleItem parent,
        /// HermesScheduleItem editCopy)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestCreateScheduleItemPublishEditCopyRelationship_Accuracy()
        {
            HermesScheduleItem parent = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(parent.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(parent.Activity.ActivityType);
            instance.SaveActivity(parent.Activity);
            instance.SaveScheduleItemRequestStatus(parent.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(parent.ScheduleItemStatus);
            instance.SaveScheduleItem(parent);

            HermesScheduleItem editCopy = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(editCopy.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(editCopy.Activity.ActivityType);
            instance.SaveActivity(editCopy.Activity);
            instance.SaveScheduleItemRequestStatus(editCopy.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(editCopy.ScheduleItemStatus);
            instance.SaveScheduleItem(editCopy);

            instance.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);

            // Verify
            HermesScheduleItem dbEditCopy = instance.GetScheduleItemEditCopy(parent);
            AccuracyTestsHelper.VerifyHermesScheduleItem(editCopy, dbEditCopy);
            HermesScheduleItem dbParent = instance.GetScheduleItemParentCopy(editCopy);
            AccuracyTestsHelper.VerifyHermesScheduleItem(parent, dbParent);
        }

        /// <summary>
        /// <p>Accuracy test the <c>DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem editCopy)</c>
        /// method.</p>.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemPublishEditCopyRelationship_Accuracy()
        {
            HermesScheduleItem parent = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(parent.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(parent.Activity.ActivityType);
            instance.SaveActivity(parent.Activity);
            instance.SaveScheduleItemRequestStatus(parent.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(parent.ScheduleItemStatus);
            instance.SaveScheduleItem(parent);

            HermesScheduleItem editCopy = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(editCopy.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(editCopy.Activity.ActivityType);
            instance.SaveActivity(editCopy.Activity);
            instance.SaveScheduleItemRequestStatus(editCopy.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(editCopy.ScheduleItemStatus);
            instance.SaveScheduleItem(editCopy);

            instance.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);

            // Verify
            HermesScheduleItem dbEditCopy = instance.GetScheduleItemEditCopy(parent);
            AccuracyTestsHelper.VerifyHermesScheduleItem(editCopy, dbEditCopy);
            HermesScheduleItem dbParent = instance.GetScheduleItemParentCopy(editCopy);
            AccuracyTestsHelper.VerifyHermesScheduleItem(parent, dbParent);

            instance.DeleteScheduleItemPublishEditCopyRelationship(editCopy);

            // Verify
            dbEditCopy = instance.GetScheduleItemEditCopy(parent);
            Assert.IsNull(dbEditCopy);
            dbParent = instance.GetScheduleItemParentCopy(editCopy);
            Assert.IsNull(dbParent);
        }

        /// <summary>
        /// <p>Accuracy test the <c> GetScheduleItemEditCopy(HermesScheduleItem parent)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleItemEditCopy_Accuracy()
        {
            HermesScheduleItem parent = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(parent.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(parent.Activity.ActivityType);
            instance.SaveActivity(parent.Activity);
            instance.SaveScheduleItemRequestStatus(parent.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(parent.ScheduleItemStatus);
            instance.SaveScheduleItem(parent);

            HermesScheduleItem editCopy = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(editCopy.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(editCopy.Activity.ActivityType);
            instance.SaveActivity(editCopy.Activity);
            instance.SaveScheduleItemRequestStatus(editCopy.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(editCopy.ScheduleItemStatus);
            instance.SaveScheduleItem(editCopy);

            instance.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);

            // Verify
            HermesScheduleItem dbEditCopy = instance.GetScheduleItemEditCopy(parent);
            AccuracyTestsHelper.VerifyHermesScheduleItem(editCopy, dbEditCopy);
        }

        /// <summary>
        /// <p>Accuracy test the <c> GetScheduleItemParentCopy(HermesScheduleItem editCopy)</c> method.</p>.
        /// </summary>
        [Test]
        public void TestGetScheduleItemParentCopy_Accuracy()
        {
            HermesScheduleItem parent = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(parent.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(parent.Activity.ActivityType);
            instance.SaveActivity(parent.Activity);
            instance.SaveScheduleItemRequestStatus(parent.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(parent.ScheduleItemStatus);
            instance.SaveScheduleItem(parent);

            HermesScheduleItem editCopy = AccuracyTestsHelper.CreateHermesScheduleItem();
            instance.SaveActivityGroup(editCopy.Activity.ActivityType.ActivityGroup);
            instance.SaveActivityType(editCopy.Activity.ActivityType);
            instance.SaveActivity(editCopy.Activity);
            instance.SaveScheduleItemRequestStatus(editCopy.ScheduleItemRequestStatus);
            instance.SaveScheduleItemStatus(editCopy.ScheduleItemStatus);
            instance.SaveScheduleItem(editCopy);
            instance.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);

            // Verify
            HermesScheduleItem dbParent = instance.GetScheduleItemParentCopy(editCopy);
            AccuracyTestsHelper.VerifyHermesScheduleItem(parent, dbParent);
        }
    }
}