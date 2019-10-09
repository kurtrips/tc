/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.ServiceModel;
using TopCoder.Services.WCF;
using TopCoder.Services.WCF.ScheduleItem;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ObjectFactory;
using HermesNS.TC.Services.ScheduleItem.Entities;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem.FailureureTests
{
    /// <summary>
    /// Failureure tests of the <see cref="HermesScheduleItemService"/> class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesScheduleItemServiceFailureureTests
    {
        /// <summary>
        /// The instance of the HermesScheduleItemService to be used for testing.
        /// </summary>
        HermesScheduleItemService service = null;


        /// <summary>
        /// <para>
        /// Set up.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().Clear();
            ConfigManager.GetInstance().LoadFile("../../test_files/failure/failure.xml");
            service = new HermesScheduleItemService();
        }

        /// <summary>
        /// <para>
        /// Tear down.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Failureure test of the HermesScheduleItemService
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestHermesScheduleItemServiceNull()
        {
            new HermesScheduleItemService(null);
        }

        /// <summary>
        /// Failureure test of the HermesScheduleItemService
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestHermesScheduleItemServiceEmpty()
        {
            new HermesScheduleItemService("       ");
        }

        /// <summary>
        /// Failureure test of the HermesScheduleItemService
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestHermesScheduleItemServiceInvalid()
        {
            new HermesScheduleItemService("Invalid");
        }


        /// <summary>
        /// Failureure test of the CreateActivity
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityFailure()
        {
            service.CreateActivity(null, new HermesActivityType());
        }

        /// <summary>
        /// Failureure test of the CreateActivity
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityFailure2()
        {
            service.CreateActivity("   ", new HermesActivityType());
        }


        /// <summary>
        /// Failureure test of the CreateActivityGroup
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityGroupFailure()
        {
            service.CreateActivityGroup(null);
        }

        /// <summary>
        /// Failureure test of the CreateActivityGroup
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityGroupFailure2()
        {
            service.CreateActivityGroup("              ");
        }



        /// <summary>
        /// Failureure test of the CreateActivityType
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityTypeFailure()
        {
            service.CreateActivityType(null, new HermesActivityGroup());
        }

        /// <summary>
        /// Failureure test of the CreateActivityType
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityTypeFailure2()
        {
            service.CreateActivityType("           ", new HermesActivityGroup());
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFailure()
        {
            service.CreateScheduleItemStatus("string", null);
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFailure2()
        {
            service.CreateScheduleItemStatus("string", "          ");
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFailure3()
        {
            service.CreateScheduleItemStatus("     ", "string");
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFailure4()
        {
            service.CreateScheduleItemStatus(null, "string");
        }


        /// <summary>
        /// Failureure test of the CreateScheduleItemRequestStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFailure()
        {
            service.CreateScheduleItemRequestStatus("string", null);
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemRequestStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFailure2()
        {
            service.CreateScheduleItemRequestStatus("string", "         ");
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemRequestStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFailure3()
        {
            service.CreateScheduleItemRequestStatus(null, "string");
        }

        /// <summary>
        /// Failureure test of the CreateScheduleItemRequestStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFailure4()
        {
            service.CreateScheduleItemRequestStatus("        ", "string");
        }

        /// <summary>
        /// Failureure test of the SaveActivity
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityFailure()
        {
            service.SaveActivity(null);
        }


        /// <summary>
        /// Failureure test of the SaveActivityGroup
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupFailure()
        {
            service.SaveActivityGroup(null);
        }

        /// <summary>
        /// Failureure test of the SaveActivityType
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypeFailure()
        {
            service.SaveActivityType(null);
        }



        /// <summary>
        /// Failureure test of the SaveScheduleItemStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusFailure()
        {
            service.SaveScheduleItemStatus(null);
        }


        /// <summary>
        /// Failureure test of the SaveScheduleItemRequestStatus
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusFailure()
        {
            service.SaveScheduleItemRequestStatus(null);
        }


        /// <summary>
        /// Failureure test of the SaveScheduleItem
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemFailure()
        {
            service.SaveScheduleItem(null);
        }


        /// <summary>
        /// Failureure test of the SaveActivities
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivitiesFailure()
        {
            service.SaveActivities(null);
        }

        /// <summary>
        /// Failureure test of the SaveActivityGroups
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupsFailure()
        {
            service.SaveActivityGroups(null);
        }

        /// <summary>
        /// Failureure test of the SaveActivityTypes
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypesFailure()
        {
            service.SaveActivityTypes(null);
        }


        /// <summary>
        /// Failureure test of the SaveScheduleItemStatuses
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusesFailure()
        {
            service.SaveScheduleItemStatuses(null);
        }


        /// <summary>
        /// Failureure test of the SaveScheduleItemRequestStatuses
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusesFailure()
        {
            service.SaveScheduleItemRequestStatuses(null);
        }


        /// <summary>
        /// Failureure test of the SaveScheduleItems
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemsFailure()
        {
            service.SaveScheduleItems(null);
        }


        /// <summary>
        /// Failureure test of the CreateScheduleItemEditCopy
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemEditCopyFailure()
        {
            service.CreateScheduleItemEditCopy(null);
        }


        /// <summary>
        /// Failureure test of the GetScheduleItem
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemEditCopyFailure()
        {
            service.GetScheduleItem(null);
        }

        /// <summary>
        /// Failureure test of the GetScheduleItem
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemParentCopyFailure()
        {
            service.GetScheduleItem(null);
        }


        /// <summary>
        /// Failureure test of the PublishScheduleItem
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestPublishScheduleItemFailure()
        {
            service.PublishScheduleItem(null);
        }
    }
}
