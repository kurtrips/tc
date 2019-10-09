using NUnit.Framework;
using TopCoder.Services.WCF.ScheduleItem;
using HermesNS.TC.Services.ScheduleItem.Entities;
using System;
using HermesNS.TC.Services.GenericNotes;

namespace HermesNS.TC.Services.ScheduleItem.Persistence.FailureTests
{
    /// <summary>
    /// Failure test for <see cref="HermesScheduleItemPersistenceProvider"/> class.
    /// </summary>
    [TestFixture]
    public class HermesScheduleItemPersistenceProviderFailureTest
    {
        /// <summary>
        /// The connection name.
        /// </summary>
        private string ConnectionName = "Valid";

        /// <summary>
        /// An instance of <see cref="HermesScheduleItemPersistenceProvider"/>.
        /// </summary>
        private HermesScheduleItemPersistenceProvider provider;

        /// <summary>
        /// An instance of <see cref="HermesScheduleItemPersistenceProvider"/> of
        /// invalid connection.
        /// </summary>
        private HermesScheduleItemPersistenceProvider invalidProvider;

        #region Setup/TearDown
        /// <summary>
        /// Set up for each test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            FailureTestHelper.LoadConfigFiles();

            provider = new HermesScheduleItemPersistenceProvider();
            invalidProvider = new HermesScheduleItemPersistenceProvider("Invalid",
                new HermesScheduleItemPersistenceHelper());
        }

        /// <summary>
        /// Tear down for each test.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            FailureTestHelper.ClearConfigFiles();
            provider = null;
            invalidProvider = null;

        }

        #endregion

        #region Test ctors
        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider()</code>.
        /// When the default namespace unexisted, throw <see cref="ScheduleItemConfigurationException"/>.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor1_DefaultNamespaceUnexisted()
        {
            FailureTestHelper.ClearConfigFiles();

            new HermesScheduleItemPersistenceProvider();
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When nameSpace is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor2_NamespaceIsNull()
        {
            new HermesScheduleItemPersistenceProvider(null);
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When nameSpace is empty, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor2_NamespaceIsEmpty()
        {
            new HermesScheduleItemPersistenceProvider("  \r\n ");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When nameSpace is unexisted, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_NamespaceIsUnexisted()
        {
            new HermesScheduleItemPersistenceProvider("FailureTest.Unexisted");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When objectFactoryNamespace is empty, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_ObjectFactoryNamespaceIsEmpty()
        {
            new HermesScheduleItemPersistenceProvider(
                "FailureTest.Invalid.EmptyObjectFactoryNamespace");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When connectionName missed, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_ConnectionNameMissed()
        {
            new HermesScheduleItemPersistenceProvider(
                "FailureTest.Invalid.ConnectionNameMissed");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When connectionName is empty, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_ConnectionNameIsEmpty()
        {
            new HermesScheduleItemPersistenceProvider(
                "FailureTest.Invalid.EmptyConnectionName");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When PersistenceHelper missed, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_PersistenceHelperMissed()
        {
            new HermesScheduleItemPersistenceProvider(
                "FailureTest.Invalid.PersistenceHelperMissed");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When PersistenceHelper is empty, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_PersistenceHelperIsEmpty()
        {
            new HermesScheduleItemPersistenceProvider(
                "FailureTest.Invalid.EmptyPersistenceHelper");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string nameSpace)</code>.
        /// When PersistenceHelper is invalid, throw ScheduleItemConfigurationException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2_PersistenceHelperIsInvalid()
        {
            new HermesScheduleItemPersistenceProvider(
                "FailureTest.Invalid.InvalidPersistenceHelper");
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string connectionName,
        /// IScheduleItemHelperBase helper)</code>.
        /// When connectionName is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3_ConnectionNameIsNull()
        {
            new HermesScheduleItemPersistenceProvider(null,
                new HermesScheduleItemPersistenceHelper());
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string connectionName,
        /// IScheduleItemHelperBase helper)</code>.
        /// When connectionName is empty, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3_ConnectionNameIsEmpty()
        {
            new HermesScheduleItemPersistenceProvider("  ",
                new HermesScheduleItemPersistenceHelper());
        }

        /// <summary>
        /// Test ctor <code>HermesScheduleItemPersistenceProvider(string connectionName,
        /// IScheduleItemHelperBase helper)</code>.
        /// When helper is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3_HelperIsNull()
        {
            new HermesScheduleItemPersistenceProvider(ConnectionName,
                null);
        }
        #endregion

        #region Test SaveActivity(HermesActivity activity)
        /// <summary>
        /// Test method <code>SaveActivity(HermesActivity activity)</code>.
        /// When activity is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveActivity_ActivityIsNull()
        {
            provider.SaveActivity(null);
        }

        #endregion

        #region Test SaveActivityGroup(HermesActivityGroup activityGroup)
        /// <summary>
        /// Test method <code>SaveActivityGroup(HermesActivityGroup activityGroup)</code>.
        /// When activityGroup is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveActivityGroup_ActivityGroupIsNull()
        {
            provider.SaveActivityGroup(null);
        }

        /// <summary>
        /// Test method <code>SaveActivityGroup(HermesActivityGroup activityGroup)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveActivityGroup_InvalidConnection()
        {
            invalidProvider.SaveActivityGroup(CreateGroup());
        }
        #endregion

        #region Test SaveActivityType(HermesActivityType activityType)
        /// <summary>
        /// Test method <code>SaveActivityType(HermesActivityType activityType)</code>.
        /// When activityType is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveActivityType_ActivityTypeIsNull()
        {
            provider.SaveActivityType(null);
        }

        /// <summary>
        /// Test method <code>SaveActivityType(HermesActivityType activityType)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveActivityType_InvalidConnection()
        {
            invalidProvider.SaveActivityType(CreateType());
        }
        #endregion

        #region Test SaveScheduleItem(HermesScheduleItem scheduleItem)
        /// <summary>
        /// Test method <code>SaveScheduleItem(HermesScheduleItem scheduleItem)</code>.
        /// When scheduleItem is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveScheduleItem_ScheduleItemIsNull()
        {
            provider.SaveScheduleItem(null);
        }
        #endregion

        #region Test SaveScheduleItemStatus(HermesScheduleItemStatus)
        /// <summary>
        /// Test method <code>SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)</code>.
        /// When scheduleItemStatus is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveScheduleItemStatus_ScheduleItemStatusIsNull()
        {
            provider.SaveScheduleItemStatus(null);
        }

        /// <summary>
        /// Test method <code>SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveScheduleItemStatus_InvalidConnection()
        {
            invalidProvider.SaveScheduleItemStatus(CreateItemStatus());
        }
        #endregion

        #region Test SaveScheduleItemRequestStatus(HermesScheduleItemRequestStatus)
        /// <summary>
        /// Test method <code>SaveScheduleItemRequestStatus(HermesScheduleItemRequestStatus)</code>.
        /// When scheduleRequestStatus is null, throw InvalidArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestSaveScheduleItemRequestStatus_ScheduleRequestStatusIsNull()
        {
            provider.SaveScheduleItemRequestStatus(null);
        }

        /// <summary>
        /// Test method <code>SaveScheduleItemRequestStatus(HermesScheduleItemRequestStatus)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestSaveScheduleItemRequestStatus_InvalidConnection()
        {
            invalidProvider.SaveScheduleItemRequestStatus(CreateRequestStatus());
        }
        #endregion

        #region Test DeleteActivity(string id)
        /// <summary>
        /// Test method <code>DeleteActivity(string id)</code>.
        /// When id not found, throw EntityNotFoundException.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteActivity_IdNotfound()
        {
            provider.DeleteActivity(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Test method <code>DeleteActivity(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteActivity_InvalidConnection()
        {
            invalidProvider.DeleteActivity(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test DeleteActivityGroup(string id)
        /// <summary>
        /// Test method <code>DeleteActivityGroup(string id)</code>.
        /// When id not found, throw EntityNotFoundException.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteActivityGroup_IdNotfound()
        {
            provider.DeleteActivityGroup(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Test method <code>DeleteActivityGroup(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteActivityGroup_InvalidConnection()
        {
            invalidProvider.DeleteActivityGroup(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test DeleteActivityType(string id)
        /// <summary>
        /// Test method <code>DeleteActivityType(string id)</code>.
        /// When id not found, throw EntityNotFoundException.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteActivityType_IdNotfound()
        {
            provider.DeleteActivityType(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Test method <code>DeleteActivityType(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteActivityType_InvalidConnection()
        {
            invalidProvider.DeleteActivityType(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test DeleteScheduleItem(string id)
        /// <summary>
        /// Test method <code>DeleteScheduleItem(string id)</code>.
        /// When id not found, throw EntityNotFoundException.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteScheduleItem_IdNotfound()
        {
            provider.DeleteScheduleItem(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Test method <code>DeleteScheduleItem(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteScheduleItem_InvalidConnection()
        {
            invalidProvider.DeleteScheduleItem(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test DeleteScheduleItemStatus(string id)
        /// <summary>
        /// Test method <code>DeleteScheduleItemStatus(string id)</code>.
        /// When id not found, throw EntityNotFoundException.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteScheduleItemStatus_IdNotfound()
        {
            provider.DeleteScheduleItemStatus(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Test method <code>DeleteScheduleItemStatus(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteScheduleItemStatus_InvalidConnection()
        {
            invalidProvider.DeleteScheduleItemStatus(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test DeleteScheduleItemRequestStatus(string id)
        /// <summary>
        /// Test method <code>DeleteScheduleItemRequestStatus(string id)</code>.
        /// When id not found, throw EntityNotFoundException.
        /// </summary>
        [Test, ExpectedException(typeof(EntityNotFoundException))]
        public void TestDeleteScheduleItemRequestStatus_IdNotfound()
        {
            provider.DeleteScheduleItemRequestStatus(Guid.NewGuid().ToString("N"));
        }

        /// <summary>
        /// Test method <code>DeleteScheduleItemRequestStatus(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteScheduleItemRequestStatus_InvalidConnection()
        {
            invalidProvider.DeleteScheduleItemRequestStatus(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetActivity(string id)
        /// <summary>
        /// Test method <code>GetActivity(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetActivity_InvalidConnection()
        {
            invalidProvider.GetActivity(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetActivityGroup(string id)
        /// <summary>
        /// Test method <code>GetActivityGroup(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetActivityGroup_InvalidConnection()
        {
            invalidProvider.GetActivityGroup(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetActivityType(string id)
        /// <summary>
        /// Test method <code>GetActivityType(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetActivityType_InvalidConnection()
        {
            invalidProvider.GetActivityType(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetScheduleItem(string id)
        /// <summary>
        /// Test method <code>GetScheduleItem(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItem_InvalidConnection()
        {
            invalidProvider.GetScheduleItem(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetScheduleItemStatus(string id)
        /// <summary>
        /// Test method <code>GetScheduleItemStatus(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemStatus_InvalidConnection()
        {
            invalidProvider.GetScheduleItemStatus(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetScheduleItemRequestStatus(string id)
        /// <summary>
        /// Test method <code>GetScheduleItemRequestStatus(string id)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemRequestStatus_InvalidConnection()
        {
            invalidProvider.GetScheduleItemRequestStatus(Guid.NewGuid().ToString("N"));
        }
        #endregion

        #region Test GetAllActivities(bool showDisabled)
        /// <summary>
        /// Test method <code>GetAllActivities(bool showDisabled)</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetAllActivities_InvalidConnection()
        {
            invalidProvider.GetAllActivities(true);
        }
        #endregion

        #region Test GetAllActivityGroups()
        /// <summary>
        /// Test method <code>GetAllActivityGroups()</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetAllActivityGroups_InvalidConnection()
        {
            invalidProvider.GetAllActivityGroups();
        }
        #endregion

        #region Test GetAllActivityTypes()
        /// <summary>
        /// Test method <code>GetAllActivityTypes()</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetAllActivityTypes_InvalidConnection()
        {
            invalidProvider.GetAllActivityTypes();
        }
        #endregion

        #region Test GetAllScheduleItemStatuses()
        /// <summary>
        /// Test method <code>GetAllScheduleItemStatuses()</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetAllScheduleItemStatuses_InvalidConnection()
        {
            invalidProvider.GetAllScheduleItemStatuses();
        }
        #endregion

        #region Test GetAllScheduleRequestStatuses()
        /// <summary>
        /// Test method <code>GetAllScheduleRequestStatuses()</code>.
        /// When connnection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetAllScheduleRequestStatuses_InvalidConnection()
        {
            invalidProvider.GetAllScheduleItemRequestStatuses();
        }
        #endregion

        #region Test CreateScheduleItemPublishEditCopyRelationship(HermesScheduleItem, HermesScheduleItem)
        /// <summary>
        /// Test method <code>CreateScheduleItemPublishEditCopyRelationship(HermesScheduleItem,
        /// HermesScheduleItem)</code>.
        /// When parent is null, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestCreateScheduleItemPublishEditCopyRelationship_ParentIsNull()
        {
            provider.CreateScheduleItemPublishEditCopyRelationship(null, CreateScheduleItem());
        }
        /// <summary>
        /// Test method <code>CreateScheduleItemPublishEditCopyRelationship(HermesScheduleItem,
        /// HermesScheduleItem)</code>.
        /// When editCopy is null, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestCreateScheduleItemPublishEditCopyRelationship_EditCopyIsNull()
        {
            provider.CreateScheduleItemPublishEditCopyRelationship(CreateScheduleItem(), null);
        }

        /// <summary>
        /// Test method <code>CreateScheduleItemPublishEditCopyRelationship(HermesScheduleItem,
        /// HermesScheduleItem)</code>.
        /// When connection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestCreateScheduleItemPublishEditCopyRelationship_ConnectionNameInvalidl()
        {
            invalidProvider.CreateScheduleItemPublishEditCopyRelationship(
                CreateScheduleItem(), CreateScheduleItem());
        }
        #endregion

        #region Test DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem editCopy)
        /// <summary>
        /// Test method <code>DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem)</code>.
        /// When editCopy is null, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteScheduleItemPublishEditCopyRelationship_EditCopyIsNull()
        {
            provider.DeleteScheduleItemPublishEditCopyRelationship(null);
        }

        /// <summary>
        /// Test method <code>DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem)</code>.
        /// When connection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestDeleteScheduleItemPublishEditCopyRelationship_ConnectionNameInvalidl()
        {
            invalidProvider.DeleteScheduleItemPublishEditCopyRelationship(CreateScheduleItem());
        }
        #endregion

        #region Test GetScheduleItemEditCopy(HermesScheduleItem parent)
        /// <summary>
        /// Test method <code>GetScheduleItemEditCopy(HermesScheduleItem parent)</code>.
        /// When parent is null, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemEditCopy_EditCopyIsNull()
        {
            provider.GetScheduleItemEditCopy(null);
        }

        /// <summary>
        /// Test method <code>GetScheduleItemEditCopy(HermesScheduleItem parent)</code>.
        /// When connection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemEditCopy_ConnectionNameInvalidl()
        {
            invalidProvider.GetScheduleItemEditCopy(CreateScheduleItem());
        }
        #endregion

        #region Test GetScheduleItemParentCopy(HermesScheduleItem editCopy)
        /// <summary>
        /// Test method <code>GetScheduleItemParentCopy(HermesScheduleItem editCopy)</code>.
        /// When editCopy is null, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemParentCopy_EditCopyIsNull()
        {
            provider.GetScheduleItemParentCopy(null);
        }

        /// <summary>
        /// Test method <code>GetScheduleItemParentCopy(HermesScheduleItem editCopy)</code>.
        /// When connection name is invalid, throw ScheduleItemPersistenceException.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemPersistenceException))]
        public void TestGetScheduleItemParentCopy_ConnectionNameInvalidl()
        {
            invalidProvider.GetScheduleItemParentCopy(CreateScheduleItem());
        }
        #endregion

        #region Help Methods
        /// <summary>
        /// Create an instance of <see cref="HermesActivity"/>.
        /// </summary>
        /// <returns>The created HermesActivity instance.</returns>
        public static HermesActivity CreateActivity()
        {
            HermesActivity activit = new HermesActivity();
            activit.Abbreviation = "DR";
            activit.Name = "Digital Run";
            activit.ExclusiveFlag = true;
            activit.LastModifiedBy = "failureTester";
            activit.LastModifiedDate = DateTime.UtcNow;
            activit.ActivityType = CreateType();

            activit.DefaultDuration = 11.2M;
            activit.DefaultExpireDays = 100;
            activit.DefaultStartTime = 100;
            activit.Enabled = true;

            activit.WorkDayAmount = 100;

            return activit;
        }

        /// <summary>
        /// Create an instance of <see cref="HermesActivityType"/>.
        /// </summary>
        /// <returns>The created HermesActivityType instance.</returns>
        private static HermesActivityType CreateType()
        {
            HermesActivityType type = new HermesActivityType();
            type.Abbreviation = "C#";
            type.Name = "CShape";
            type.LastModifiedBy = "failureTester";
            type.LastModifiedDate = DateTime.UtcNow;
            type.Id = Guid.NewGuid().ToString("N");
            type.ActivityGroup = CreateGroup();

            return type;
        }

        /// <summary>
        /// Create an instance of <see cref="HermesActivityGroup"/>.
        /// </summary>
        /// <returns>The created HermesActivityGroup instance.</returns>
        private static HermesActivityGroup CreateGroup()
        {
            HermesActivityGroup group = new HermesActivityGroup();
            group.Abbreviation = "dev";
            group.Name = "Developer";
            group.LastModifiedBy = "failureTester";
            group.LastModifiedDate = DateTime.UtcNow;
            group.Id = Guid.NewGuid().ToString("N");

            return group;
        }

        /// <summary>
        /// Create an instance of <see cref="HermesScheduleItem"/>.
        /// </summary>
        /// <returns>The created HermesScheduleItem instance.</returns>
        public static HermesScheduleItem CreateScheduleItem()
        {
            HermesScheduleItem item = new HermesScheduleItem();
            item.Activity = CreateActivity();
            item.Duration = 11.1M;
            item.LastModifiedBy = "failureTester";
            item.LastModifiedDate = DateTime.UtcNow;

            item.Note = new HermesGenericNote();
            item.Note.Id = Guid.NewGuid().ToString("N");

            item.ExceptionFlag = 'E';
            item.ExpirationDate = DateTime.UtcNow;
            item.WorkDate = DateTime.UtcNow;
            item.ScheduleItemRequestStatus = CreateRequestStatus();
            item.ScheduleItemStatus = CreateItemStatus();
            item.Version = 1;
            item.WorkDayAmount = 6.8M;

            return item;
        }

        /// <summary>
        /// Create an instance of <see cref="HermesScheduleItemStatus"/>.
        /// </summary>
        /// <returns>The created HermesScheduleItemStatus instance.</returns>
        public static HermesScheduleItemStatus CreateItemStatus()
        {
            HermesScheduleItemStatus itemStatus = new HermesScheduleItemStatus();
            itemStatus.Abbreviation = "Act";
            itemStatus.Description = "Active";
            itemStatus.LastModifiedBy = "failureTester";
            itemStatus.LastModifiedDate = DateTime.UtcNow;
            itemStatus.Id = Guid.NewGuid().ToString("N");

            return itemStatus;
        }

        /// <summary>
        /// Create an instance of <see cref="HermesScheduleItemRequestStatus"/>.
        /// </summary>
        /// <returns>The created HermesScheduleItemRequestStatus instance.</returns>
        public static HermesScheduleItemRequestStatus CreateRequestStatus()
        {
            HermesScheduleItemRequestStatus reqStatus = new HermesScheduleItemRequestStatus();
            reqStatus.Abbreviation = "Req";
            reqStatus.Description = "Request";
            reqStatus.LastModifiedBy = "failureTester";
            reqStatus.LastModifiedDate = DateTime.UtcNow;
            reqStatus.Id = Guid.NewGuid().ToString("N");

            return reqStatus;
        }

        #endregion
    }
}