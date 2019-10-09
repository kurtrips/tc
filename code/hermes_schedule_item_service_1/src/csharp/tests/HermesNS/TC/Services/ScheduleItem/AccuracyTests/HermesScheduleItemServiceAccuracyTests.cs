/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;
using System.ServiceModel;
using TopCoder.Services.WCF;
using System.Collections.Generic;
using TopCoder.Util.ObjectFactory;
using HermesNS.TC.Entity.Validation;
using TopCoder.Services.WCF.ScheduleItem;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.ScheduleItem.Persistence;
using HermesNS.TC.LoggingWrapperPublisher;

namespace HermesNS.TC.Services.ScheduleItem
{
    /// <summary>
    ///  Test the accuracy of the <c>HermesScheduleItemService</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesScheduleItemServiceAccuracyTests
    {
        /// <summary>
        /// Host for the HermesAuditTrailSaveService service
        /// </summary>
        ServiceHost hostAudit = null;

        /// <summary>
        /// The host for the HermesGenericNoteService service.
        /// </summary>
        ServiceHost hostNote = null;

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuditTrailSaveService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAudit =
            new Uri("http://localhost:10101/HermesAuditTrailSaveService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesGenericNoteService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressNote =
            new Uri("http://localhost:20202/HermesGenericNoteService");

        /// <summary>
        /// <see cref="HermesScheduleItemService"/> instance to test
        /// </summary>
        private HermesScheduleItemService service;

        /// <summary>
        /// Set up environment once before all tests are run
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            HostHermesAuditTrailSaveService();
            HostGenericNoteService();
        }

        /// <summary>
        /// Cleans up environment once after all tests are run.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            if (hostAudit != null)
            {
                hostAudit.Close();
            }
            if (hostNote != null)
            {
                hostNote.Close();
            }
        }

        /// <summary>
        /// <para>
        /// Set up the environment
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            AccuracyTestHelper.LoadConfig();
            service = new HermesScheduleItemService();
            MockHermesScheduleItemPersistenceProvider.ClearDB();
        }

        /// <summary>
        /// <para>
        /// Clear the environment
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            AccuracyTestHelper.ClearConfig();
        }

        /// <summary>
        /// Creates a new host for the HermesAuditTrailSaveService and starts the service.
        /// </summary>
        private void HostHermesAuditTrailSaveService()
        {
            //Create host
            hostAudit = new ServiceHost(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailSaveService),
                endPointAddressAudit);

            //Create end point for service
            hostAudit.AddServiceEndpoint(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailSaveService),
                new BasicHttpBinding(), endPointAddressAudit);
            hostAudit.Open();
        }

        /// <summary>
        /// Host the generic note service.
        /// </summary>
        private void HostGenericNoteService()
        {
            //Create host
            hostNote = new ServiceHost(
                typeof(HermesNS.TC.Services.GenericNotes.HermesGenericNoteService), endPointAddressNote);

            //Create end point for the service
            hostNote.AddServiceEndpoint(typeof(HermesNS.TC.Services.GenericNotes.HermesGenericNoteService),
                new BasicHttpBinding(), endPointAddressNote);

            hostNote.Open();
        }

        /// <summary>
        /// <para>
        /// Test default namespace.
        /// </para>
        /// </summary>
        [Test]
        public void TestNameSpace()
        {
            Assert.AreEqual("HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService",
                HermesScheduleItemService.DefaultNamespace, "DefautNamespace should be equal.");
        }

        #region Test Constructor for accuracy
        /// <summary>
        /// <para>
        /// Test Constructor for accuracy
        /// All fields should be set.
        /// </para>
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            CheckService();
        }

        /// <summary>
        /// <para>
        /// Test Constructor for accuracy
        /// All fields should be set.
        /// </para>
        /// </summary>
        [Test]
        public void TestConstructor_Params()
        {
            service = new HermesScheduleItemService(HermesScheduleItemService.DefaultNamespace);
            CheckService();
        }

        /// <summary>
        /// <para>
        /// Test Constructor for accuracy
        /// All fields should be set.
        /// </para>
        /// </summary>
        [Test]
        public void TestConstructor_Without_ObjectFactory()
        {
            service = new HermesScheduleItemService("NoObjectFactory");
            CheckService();
        }

        /// <summary>
        /// <para>
        /// Test Constructor for accuracy
        /// All fields should be set.
        /// </para>
        /// </summary>
        public void TestConstructor_Parameters()
        {
            service = new HermesScheduleItemService(new MockHermesScheduleItemPersistenceProvider(),
                ObjectFactory.GetDefaultObjectFactory(), "test", "test", new HermesLogger("logger"));
            CheckService();
        }
        #endregion

        #region Test Method CreateActivity for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateActivity for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateActivity()
        {
            HermesActivity entity = service.CreateActivity("test", new HermesActivityType());
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Name, "Name should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method CreateActivityGroup for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateActivityGroup for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateActivityGroup()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("test");
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Abbreviation, "Abbreviation should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method CreateActivityType for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateActivityType for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateActivityType()
        {
            HermesActivityType entity = service.CreateActivityType("test", new HermesActivityGroup());
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Abbreviation, "Name should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method CreateScheduleItem for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateScheduleItem for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateScheduleItem()
        {
            DateTime t = DateTime.Now;
            HermesScheduleItem entity = service.CreateScheduleItem(t, new HermesActivity());
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual(t, entity.WorkDate, "Abbreviation should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method CreateScheduleItemStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateScheduleItemStatus for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateScheduleItemStatus()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("test", "desc");
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Abbreviation, "Abbreviation should be equal.");
            Assert.AreEqual("desc", entity.Description, "Description should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method CreateScheduleItemRequestStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateScheduleItemRequestStatus for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("test", "desc");
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Abbreviation, "Abbreviation should be equal.");
            Assert.AreEqual("desc", entity.Description, "Description should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method DeleteActivity for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteActivity for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteActivity()
        {
            HermesActivity entity = service.CreateActivity("test",
                new HermesActivityType());
            service.DeleteActivity(entity.Id);
            Assert.IsNull(service.GetActivity(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteActivities for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteActivities for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteActivities()
        {
            HermesActivity entity = service.CreateActivity("test",
                new HermesActivityType());
            service.DeleteActivities(new string[] { entity.Id });
            Assert.IsNull(service.GetActivity(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteActivityGroup for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteActivityGroup for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteActivityGroup()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("test");
            service.DeleteActivityGroup(entity.Id);
            Assert.IsNull(service.GetActivityGroup(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteActivityGroups for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteActivityGroups for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteActivityGroups()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("test");
            service.DeleteActivityGroups(new string[] { entity.Id });
            Assert.IsNull(service.GetActivityGroup(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteActivityType for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteActivityType for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteActivityType()
        {
            HermesActivityType entity = service.CreateActivityType("test",
                new HermesActivityGroup());
            service.DeleteActivityType(entity.Id);
            Assert.IsNull(service.GetActivityType(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteActivityTypes for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteActivityTypes for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteActivityTypes()
        {
            HermesActivityType entity = service.CreateActivityType("test",
                new HermesActivityGroup());
            service.DeleteActivityTypes(new string[] { entity.Id });
            Assert.IsNull(service.GetActivityType(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteScheduleItem for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItem for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItem()
        {
            HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now,
                new HermesActivity());
            service.DeleteScheduleItem(entity.Id);
            Assert.IsNull(service.GetScheduleItem(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteScheduleItems for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItems for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItems()
        {
            HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now,
                new HermesActivity());
            service.DeleteScheduleItems(new string[] { entity.Id });
            Assert.IsNull(service.GetScheduleItem(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteScheduleItemStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItemStatus for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItemStatus()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("test",
                "test2");
            service.DeleteScheduleItemStatus(entity.Id);
            Assert.IsNull(service.GetScheduleItemStatus(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteScheduleItemStatuses for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItemStatuses for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItemStatuses()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("test",
                "test2");
            service.DeleteScheduleItemStatuses(new string[] { entity.Id });
            Assert.IsNull(service.GetScheduleItemStatus(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteScheduleItemRequestStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItemRequestStatus for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("test",
                "test2");
            service.DeleteScheduleItemRequestStatus(entity.Id);
            Assert.IsNull(service.GetScheduleItemRequestStatus(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method DeleteScheduleItemRequestStatuses for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItemRequestStatuses for accruacy
        /// entity should be deleted.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItemRequestStatuses()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("test",
                "test2");
            service.DeleteScheduleItemRequestStatuses(new string[] { entity.Id });
            Assert.IsNull(service.GetScheduleItemRequestStatus(entity.Id), "entity should not be got.");
        }
        #endregion

        #region Test Method GetActivity for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetActivity for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetActivity()
        {
            HermesActivity entity = service.CreateActivity("test", new HermesActivityType());
            entity = service.GetActivity(entity.Id);
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Name, "Name should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method GetActivityType for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetActivityType for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetActivityType()
        {
            HermesActivityType entity = service.CreateActivityType("test", new HermesActivityGroup());
            entity = service.GetActivityType(entity.Id);
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Abbreviation, "Name should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method GetActivityGroup for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetActivityGroup for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetActivityGroup()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("test");
            entity = service.GetActivityGroup(entity.Id);
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.AreEqual("test", entity.Abbreviation, "Abbreviation should be equal.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method GetScheduleItem for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetScheduleItem for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetScheduleItem()
        {
            HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, new HermesActivity());
            entity = service.GetScheduleItem(entity.Id);
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method GetScheduleItemStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetScheduleItemStatus for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetScheduleItemStatus()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("test", "test1");
            entity = service.GetScheduleItemStatus(entity.Id);
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method GetScheduleItemRequestStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetScheduleItemRequestStatus for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("test", "test1");
            entity = service.GetScheduleItemRequestStatus(entity.Id);
            Assert.IsNotNull(entity, "entity should be created.");
            Assert.IsNotNull(entity.Id, "id is set.");
            Assert.IsNotNull(entity.LastModifiedBy, "Modified is set.");
            Assert.AreNotEqual(DateTime.MinValue, entity.LastModifiedDate,
                "modified Date have been updated.");
        }
        #endregion

        #region Test Method GetAllActivites for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetAllActivites for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetAllActivites()
        {
            HermesActivity entity = service.CreateActivity("test", new HermesActivityType());
            IList<HermesActivity> entities = service.GetAllActivities(true);
            Assert.IsNotNull(entities, "entities should be created.");
            Assert.AreEqual(1, entities.Count, "1 item expected.");
        }
        #endregion

        #region Test Method GetAllActivityGroups for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetAllActivityGroups for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetAllActivityGroups()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("test");
            IList<HermesActivityGroup> entities = service.GetAllActivityGroups();
            Assert.IsNotNull(entities, "entities should be created.");
            Assert.AreEqual(1, entities.Count, "1 item expected.");
        }
        #endregion

        #region Test Method GetAllActivityTypes for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetAllActivityTypes for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetAllActivityTypes()
        {
            HermesActivityType entity = service.CreateActivityType("test", new HermesActivityGroup());
            IList<HermesActivityType> entities = service.GetAllActivityTypes();
            Assert.IsNotNull(entities, "entities should be created.");
            Assert.AreEqual(1, entities.Count, "1 item expected.");
        }
        #endregion

        #region Test Method GetAllScheduleItemStatuses for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetAllScheduleItemStatuses for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetAllScheduleItemStatuses()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("test", "desc");
            IList<HermesScheduleItemStatus> entities = service.GetAllScheduleItemStatuses();
            Assert.IsNotNull(entities, "entities should be created.");
            Assert.AreEqual(1, entities.Count, "1 item expected.");
        }
        #endregion

        #region Test Method GetAllScheduleItemRequestStatuses for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetAllScheduleItemRequestStatuses for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetAllScheduleItemRequestStatuses()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("test", "desc");
            IList<HermesScheduleItemRequestStatus> entities = service.GetAllScheduleItemRequestStatuses();
            Assert.IsNotNull(entities, "entities should be created.");
            Assert.AreEqual(1, entities.Count, "1 item expected.");
        }
        #endregion

        #region Test Method SaveActivity for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveActivity for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveActivity()
        {
            HermesActivityGroup group = service.CreateActivityGroup("group");
            HermesActivityType type = service.CreateActivityType("test", group);
            HermesActivity entity = service.CreateActivity("test", type);
            HermesActivity newEntity = new HermesActivity();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Name = "New";
            newEntity.ActivityType = type;
            newEntity = service.SaveActivity(newEntity);
            Assert.IsNotNull(newEntity, "entity should be created.");
            Assert.AreEqual("New", newEntity.Name, "Name should be equal.");
        }
        #endregion

        #region Test Method SaveActivityGroup for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveActivityGroup for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveActivityGroup()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("group");
            HermesActivityGroup newEntity = new HermesActivityGroup();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Name = "New";
            newEntity = service.SaveActivityGroup(newEntity);
            Assert.IsNotNull(newEntity, "entity should be created.");
            Assert.AreEqual("New", newEntity.Name, "Name should be equal.");
        }
        #endregion

        #region Test Method SaveActivityType for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveActivityType for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveActivityType()
        {
            HermesActivityGroup group = service.CreateActivityGroup("group");
            HermesActivityType entity = service.CreateActivityType("type", group);
            HermesActivityType newEntity = new HermesActivityType();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Name = "New";
            newEntity.ActivityGroup = group;
            newEntity = service.SaveActivityType(newEntity);
            Assert.IsNotNull(newEntity, "entity should be created.");
            Assert.AreEqual("New", newEntity.Name, "Name should be equal.");
        }
        #endregion

        #region Test Method SaveScheduleItem for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveScheduleItem for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveScheduleItem()
        {
            HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("1", "2");
            HermesScheduleItem newEntity = new HermesScheduleItem();
            newEntity.Id = entity.Id;
            //newEntity.Note = new HermesGenericNote();
            newEntity.ScheduleItemRequestStatus = s1;
            newEntity.ScheduleItemStatus = s2;
            newEntity.Duration = 10;
            newEntity.Activity = activiy;
            newEntity = service.SaveScheduleItem(newEntity);
            Assert.IsNotNull(newEntity, "entity should be created.");
        }
        #endregion

        #region Test Method SaveScheduleItemStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveScheduleItemStatus for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveScheduleItemStatus()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("1", "2");
            HermesScheduleItemStatus newEntity = new HermesScheduleItemStatus();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Description = "New";
            newEntity = service.SaveScheduleItemStatus(newEntity);
            Assert.IsNotNull(newEntity, "entity should be created.");
            Assert.AreEqual("New", newEntity.Abbreviation, "Name should be equal.");
        }
        #endregion

        #region Test Method SaveScheduleItemRequestStatus for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveScheduleItemRequestStatus for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("1", "2");
            HermesScheduleItemRequestStatus newEntity = new HermesScheduleItemRequestStatus();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Description = "New";
            newEntity = service.SaveScheduleItemRequestStatus(newEntity);
            Assert.IsNotNull(newEntity, "entity should be created.");
            Assert.AreEqual("New", newEntity.Abbreviation, "Name should be equal.");
        }
        #endregion

        #region Test Method SaveActivities for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveActivities for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveActivities()
        {
            HermesActivityGroup group = service.CreateActivityGroup("group");
            HermesActivityType type = service.CreateActivityType("test", group);
            HermesActivity entity = service.CreateActivity("test", type);
            HermesActivity newEntity = new HermesActivity();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Name = "New";
            newEntity.ActivityType = type;
            IList<HermesActivity> newEntities = service.SaveActivities(new HermesActivity[] { newEntity });
            Assert.IsNotNull(newEntities, "entity should be created.");
            Assert.AreEqual(1, newEntities.Count, "entity should be created.");
        }
        #endregion

        #region Test Method SaveActivityGroups for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveActivityGroups for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveActivityGroups()
        {
            HermesActivityGroup entity = service.CreateActivityGroup("group");
            HermesActivityGroup newEntity = new HermesActivityGroup();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Name = "New";
            IList<HermesActivityGroup> newEntities = service.SaveActivityGroups(
                new HermesActivityGroup[] { newEntity });
            Assert.IsNotNull(newEntities, "entity should be created.");
            Assert.AreEqual(1, newEntities.Count, "entity should be created.");
        }
        #endregion

        #region Test Method SaveActivityTypes for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveActivityTypes for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveActivityTypes()
        {
            HermesActivityGroup group = service.CreateActivityGroup("group");
            HermesActivityType entity = service.CreateActivityType("group", group);
            HermesActivityType newEntity = new HermesActivityType();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Name = "New";
            newEntity.ActivityGroup = group;
            IList<HermesActivityType> newEntities = service.SaveActivityTypes(
                new HermesActivityType[] { newEntity });
            Assert.IsNotNull(newEntities, "entity should be created.");
            Assert.AreEqual(1, newEntities.Count, "entity should be created.");
        }
        #endregion

        #region Test Method SaveScheduleItems for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveScheduleItems for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveScheduleItems()
        {
            HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("1", "2");
            HermesScheduleItem newEntity = new HermesScheduleItem();
            newEntity.Id = entity.Id;
            //newEntity.Note = new HermesGenericNote();
            newEntity.ScheduleItemRequestStatus = s1;
            newEntity.ScheduleItemStatus = s2;
            newEntity.Duration = 10;
            newEntity.Activity = activiy;
            IList<HermesScheduleItem> newEntities = service.SaveScheduleItems(
                new HermesScheduleItem[] { newEntity });
            Assert.IsNotNull(newEntities, "entity should be created.");
            Assert.AreEqual(1, newEntities.Count, "entity should be created.");
        }
        #endregion

        #region Test Method SaveScheduleItemStatuses for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveScheduleItemStatuses for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveScheduleItemStatuses()
        {
            HermesScheduleItemStatus entity = service.CreateScheduleItemStatus("group", "test");
            HermesScheduleItemStatus newEntity = new HermesScheduleItemStatus();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Description = "New";
            IList<HermesScheduleItemStatus> newEntities = service.SaveScheduleItemStatuses(
                new HermesScheduleItemStatus[] { newEntity });
            Assert.IsNotNull(newEntities, "entity should be created.");
            Assert.AreEqual(1, newEntities.Count, "entity should be created.");
        }
        #endregion

        #region Test Method SaveScheduleItemRequestStatuses for accuracy
        /// <summary>
        /// <para>
        /// Test Method SaveScheduleItemRequestStatuses for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodSaveScheduleItemRequestStatuses()
        {
            HermesScheduleItemRequestStatus entity = service.CreateScheduleItemRequestStatus("group", "test");
            HermesScheduleItemRequestStatus newEntity = new HermesScheduleItemRequestStatus();
            newEntity.Id = entity.Id;
            newEntity.Abbreviation = "New";
            newEntity.Description = "New";
            IList<HermesScheduleItemRequestStatus> newEntities = service.SaveScheduleItemRequestStatuses(
                new HermesScheduleItemRequestStatus[] { newEntity });
            Assert.IsNotNull(newEntities, "entity should be created.");
            Assert.AreEqual(1, newEntities.Count, "entity should be created.");
        }
        #endregion

        #region Test Method CreateScheduleItemEditCopy for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateScheduleItemEditCopy for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateScheduleItemEditCopy()
        {
            HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("published", "published");
            HermesScheduleItemStatus s3 = service.CreateScheduleItemStatus("edit copy", "edit copy");
            entity.ScheduleItemStatus = s2;
            entity.ScheduleItemRequestStatus = s1;
            MockHermesScheduleItemPersistenceProvider.items[entity.Id] = entity;
            HermesScheduleItem edit = service.CreateScheduleItemEditCopy(entity);
            Assert.IsNotNull(edit, "instance should be created.");
            Assert.AreEqual("edit copy", edit.ScheduleItemStatus.Description,
                "description should be copy edit.");
        }
        #endregion

        #region Test Method CreateScheduleItemPublishEditCopyRelationship for accuracy
        /// <summary>
        /// <para>
        /// Test Method CreateScheduleItemPublishEditCopyRelationship for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodCreateScheduleItemPublishEditCopyRelationship()
        {
            //HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            //HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            //HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            //HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("published", "published");
            //HermesScheduleItemStatus s3 = service.CreateScheduleItemStatus("edit copy", "edit copy");
            //entity.ScheduleItemStatus = s2;
            //entity.ScheduleItemRequestStatus = s1;
            //MockHermesScheduleItemPersistenceProvider.items[entity.Id] = entity;
            //HermesScheduleItem edit = service.CreateScheduleItemEditCopy(entity);

            //Assert.IsNotNull(edit, "instance should be created.");
            //Assert.AreEqual("edit copy", edit.ScheduleItemStatus.Description,
            //    "description should be copy edit.");

            //service.CreateScheduleItemPublishEditCopyRelationship(entity, edit);

            //Assert.AreEqual(entity, service.GetScheduleItemParentCopy(edit), "relation should be built.");
        }
        #endregion

        #region Test Method DeleteScheduleItemPublishEditCopyRelationship for accuracy
        /// <summary>
        /// <para>
        /// Test Method DeleteScheduleItemPublishEditCopyRelationship for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodDeleteScheduleItemPublishEditCopyRelationship()
        {
            //HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            //HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            //HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            //HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("published", "published");
            //HermesScheduleItemStatus s3 = service.CreateScheduleItemStatus("edit copy", "edit copy");
            //entity.ScheduleItemStatus = s2;
            //entity.ScheduleItemRequestStatus = s1;
            //MockHermesScheduleItemPersistenceProvider.items[entity.Id] = entity;
            //HermesScheduleItem edit = service.CreateScheduleItemEditCopy(entity);

            //Assert.IsNotNull(edit, "instance should be created.");
            //Assert.AreEqual("edit copy", edit.ScheduleItemStatus.Description,
            //    "description should be copy edit.");

            //service.CreateScheduleItemPublishEditCopyRelationship(entity, edit);

            //Assert.AreEqual(entity, service.GetScheduleItemParentCopy(edit), "relation should be built.");

            //service.DeleteScheduleItemPublishEditCopyRelationship(edit);
            //Assert.IsNull(service.GetScheduleItemParentCopy(edit), "relation should be deleted.");
        }

        #endregion

        #region Test Method GetScheduleItemEditCopy for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetScheduleItemEditCopy for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetScheduleItemEditCopy()
        {
            //HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            //HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            //HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            //HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("published", "published");
            //HermesScheduleItemStatus s3 = service.CreateScheduleItemStatus("edit copy", "edit copy");
            //entity.ScheduleItemStatus = s2;
            //entity.ScheduleItemRequestStatus = s1;
            //MockHermesScheduleItemPersistenceProvider.items[entity.Id] = entity;
            //HermesScheduleItem edit = service.CreateScheduleItemEditCopy(entity);

            //Assert.IsNotNull(edit, "instance should be created.");
            //Assert.AreEqual("edit copy", edit.ScheduleItemStatus.Description,
            //    "description should be copy edit.");

            //service.CreateScheduleItemPublishEditCopyRelationship(entity, edit);

            //Assert.AreEqual(edit, service.GetScheduleItemEditCopy(entity), "relation should be built.");
        }

        #endregion

        #region Test Method GetScheduleItemParentCopy for accuracy
        /// <summary>
        /// <para>
        /// Test Method GetScheduleItemParentCopy for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodGetScheduleItemParentCopy()
        {
            //HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            //HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            //HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            //HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("published", "published");
            //HermesScheduleItemStatus s3 = service.CreateScheduleItemStatus("edit copy", "edit copy");
            //entity.ScheduleItemStatus = s2;
            //entity.ScheduleItemRequestStatus = s1;
            //MockHermesScheduleItemPersistenceProvider.items[entity.Id] = entity;
            //HermesScheduleItem edit = service.CreateScheduleItemEditCopy(entity);

            //Assert.IsNotNull(edit, "instance should be created.");
            //Assert.AreEqual("edit copy", edit.ScheduleItemStatus.Description,
            //    "description should be copy edit.");

            //service.CreateScheduleItemPublishEditCopyRelationship(entity, edit);

            //Assert.AreEqual(entity, service.GetScheduleItemParentCopy(edit), "relation should be built.");
        }

        #endregion

        #region Test Method PublishScheduleItem for accuracy
        /// <summary>
        /// <para>
        /// Test Method PublishScheduleItem for accruacy
        /// entity should be created.
        /// </para>
        /// </summary>
        [Test]
        public void TestMethodPublishScheduleItem()
        {
            //HermesActivity activiy = service.CreateActivity("group", new HermesActivityType());
            //HermesScheduleItem entity = service.CreateScheduleItem(DateTime.Now, activiy);
            //HermesScheduleItemRequestStatus s1 = service.CreateScheduleItemRequestStatus("1", "2");
            //HermesScheduleItemStatus s2 = service.CreateScheduleItemStatus("published", "published");
            //HermesScheduleItemStatus s3 = service.CreateScheduleItemStatus("edit copy", "edit copy");
            //HermesScheduleItemStatus s4 = service.CreateScheduleItemStatus("retried", "retried");
            //entity.ScheduleItemStatus = s2;
            //entity.ScheduleItemRequestStatus = s1;
            //MockHermesScheduleItemPersistenceProvider.items[entity.Id] = entity;
            //HermesScheduleItem edit = service.CreateScheduleItemEditCopy(entity);

            //Assert.IsNotNull(edit, "instance should be created.");
            //Assert.AreEqual("edit copy", edit.ScheduleItemStatus.Description,
            //    "description should be copy edit.");

            //service.CreateScheduleItemPublishEditCopyRelationship(entity, edit);

            //edit = service.PublishScheduleItem(edit);
        }

        #endregion

        /// <summary>
        /// Test HostUpdated for coverage.
        /// </summary>
        [Test]
        public void TestHostUpated()
        {
            AccuracyTestHelper.InvokeMethod(service, "HostUpdated");
        }

        #region Tool
        /// <summary>
        /// <para>
        /// Check the service.
        /// </para>
        /// </summary>
        private void CheckService()
        {
            Assert.IsNotNull(service, "instance should be intialized.");
            Assert.IsTrue(service is TCWcfServiceBase, "intance should be TCWcfServiceBase");
            Assert.IsNotNull(AccuracyTestHelper.GetField<string>(service, "_auditClientKey"),
                "_auditClientKey should be set.");
            Assert.IsNotNull(AccuracyTestHelper.GetField<ObjectFactory>(service, "_objectFactory"),
                "_objectFactory should be created.");
            Assert.IsNotNull(AccuracyTestHelper.GetField<string>(service, "_genericNotesClientKey"),
                "_genericNotesClientKey should be created.");
        }
        #endregion
    }
}
