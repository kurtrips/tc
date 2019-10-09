/*
 * Copyright (c)2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Security.Principal;
using System.Transactions;
using TopCoder.Util.ConfigurationManager;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.GenericNotes.Entities;
using Hermes.Services.Security.Authorization.Client.Common;
using Hermes.Services.Security.Authorization.TopCoder;
using Hermes.Services.Security.Authorization;
using HermesNS.TC.Services.ScheduleItem.Clients;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem
{
    /// <summary>
    /// Demonstration of component usage.
    /// </summary>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [HermesNS.TC.CoverageExcludeAttribute]
    public class DemoTests
    {
        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesScheduleItemService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddress =
            new Uri("net.tcp://localhost:11111/HermesScheduleItemService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuthorizationService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAuth =
            new Uri("http://localhost:55555/HermesAuthorizationService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesGenericNoteService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressNote =
            new Uri("http://localhost:44444/HermesGenericNoteService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuditTrailSaveService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAudit =
            new Uri("http://localhost:22222/HermesAuditTrailSaveService");

        /// <summary>
        /// The Transaction scope to use.
        /// </summary>
        TransactionScope scope = null;

        /// <summary>
        /// Host for the HermesScheduleItemService service
        /// </summary>
        ServiceHost host = null;

        /// <summary>
        /// The host for the HermesAuthorizationService service.
        /// </summary>
        ServiceHost hostAuth = null;

        /// <summary>
        /// Host for the HermesAuditTrailSaveService service
        /// </summary>
        ServiceHost hostAudit = null;

        /// <summary>
        /// The host for the HermesGenericNoteService service.
        /// </summary>
        ServiceHost hostNote = null;

        /// <summary>
        /// Sets up the environment for the demo tests.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            UnitTestHelper.LoadConfigMgrFiles();

            //Setup some reference data in the DB
            UnitTestHelper.ClearTestDatabase();
            UnitTestHelper.SetupTestDatabase();

            //Host the HermesScheduleItemService and open its client
            HostHermesScheduleItemServiceAndOpenClient();

            //Host authorization service
            HostAuthService();

            //Aduit service
            HostHermesAuditTrailSaveService();

            //Note service
            HostGenericNoteService();

            //Create Message Headers
            CreateMessageHeaders();

            HermesAuthorizationMediator.Configuration = UnitTestHelper.CreateMediatorConfig();

            //Open transaction for calls from service
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.RepeatableRead;
            scope = new TransactionScope(TransactionScopeOption.RequiresNew, options);
        }

        /// <summary>
        /// Cleans up the environment for the demo tests.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            try
            {
                scope.Complete();
                scope.Dispose();
            }
            catch
            {
                //This is required for failure tests
            }

            UnitTestHelper.ClearTestDatabase();

            ConfigManager.GetInstance().Clear(false);

            if (host != null)
            {
                host.Close();
            }

            if (hostAudit != null)
            {
                hostAudit.Close();
            }

            if (hostNote != null)
            {
                hostNote.Close();
            }

            if (hostAuth != null)
            {
                hostAuth.Close();
            }

            if (client != null)
            {
                try
                {
                    client.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// The demo for the component which shows creating, saving, deleting and getting entities.
        /// This method just demonstrates the above functionality for HermesScheduleItem.
        /// Similar code can be used for other entities like HermesActivity, HermesActivityGroup etc.
        /// </summary>
        [Test]
        public void Demo1()
        {
            //Get existing HermesActivity
            HermesActivity refHermesActivity =
                client.GetActivity(new Guid("33333333333333333333333333333333").ToString());

            //Create 2 new schedule items
            HermesScheduleItem schedItem1 = client.CreateScheduleItem(DateTime.Today, refHermesActivity);
            HermesScheduleItem schedItem2 = client.CreateScheduleItem(DateTime.Today, refHermesActivity);

            //Make some changes to the above 2
            schedItem1.Duration = new decimal(34.67);
            schedItem1.ExceptionFlag = 'Y';
            schedItem1.ExpirationDate = DateTime.Today.AddDays(24);
            HermesGenericNote note = new HermesGenericNote();
            note.Description = "NewNoteDesc";
            schedItem1.Note = note;
            schedItem1.ScheduleItemRequestStatus =
                client.GetScheduleItemRequestStatus(new Guid("55555555555555555555555555555555").ToString());
            schedItem1.ScheduleItemStatus =
                client.GetScheduleItemStatus(new Guid("44444444444444444444444444444444").ToString());
            schedItem1.Version = 2;
            schedItem1.WorkDate = DateTime.Today.AddDays(1);
            schedItem1.WorkDayAmount = new decimal(10.45);

            schedItem2.Duration = new decimal(34.67);
            schedItem2.ExceptionFlag = 'Y';
            schedItem2.ExpirationDate = DateTime.Today.AddDays(24);
            note = new HermesGenericNote();
            note.Description = "NewNoteDesc";
            schedItem2.Note = note;
            schedItem2.ScheduleItemRequestStatus =
                client.GetScheduleItemRequestStatus(new Guid("55555555555555555555555555555555").ToString());
            schedItem2.ScheduleItemStatus =
                client.GetScheduleItemStatus(new Guid("44444444444444444444444444444444").ToString());
            schedItem2.Version = 2;
            schedItem2.WorkDate = DateTime.Today.AddDays(1);
            schedItem2.WorkDayAmount = new decimal(10.45);

            //We can save them individually
            client.SaveScheduleItem(schedItem1);
            client.SaveScheduleItem(schedItem2);

            //Or we can save them in one go
            List<HermesScheduleItem> schedItems = new List<HermesScheduleItem>();
            schedItems.Add(schedItem1);
            schedItems.Add(schedItem2);
            client.SaveScheduleItems(schedItems);

            //Get the schedule items
            schedItem1 = client.GetScheduleItem(schedItem1.Id);
            schedItem2 = client.GetScheduleItem(schedItem2.Id);

            //Although getting many schedule items in one go is not supported but it is supported for other types
            //Get all activities
            IList<HermesActivity> activities1 = client.GetAllActivities(true);
            //Get all activities which are enabled
            IList<HermesActivity> activities2 = client.GetAllActivities(false);
            //Get all activity groups
            IList<HermesActivityGroup> activityGroups = client.GetAllActivityGroups();

            //Delete them in one go
            List<string> ids = new List<string>();
            ids.Add(schedItem1.Id);
            ids.Add(schedItem2.Id);
            client.DeleteScheduleItems(ids);

            //Delete a single schedule item
            client.DeleteScheduleItem(new Guid("66666666666666666666666666666666").ToString());
        }

        /// <summary>
        /// The demo for the component which shows maintaining the parent - edit copy relations between
        /// HermesScheduleItem instances
        /// </summary>
        [Test]
        public void Demo2()
        {
            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());

            //Create relation
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);

            //Create edit copy
            HermesScheduleItem newEditCopy = client.CreateScheduleItemEditCopy(parent);

            //Get parent
            parent = client.GetScheduleItemParentCopy(editCopy);

            //Get edit copy
            editCopy = client.GetScheduleItemEditCopy(parent);

            //Publish item
            client.PublishScheduleItem(editCopy);

            //Delete relation
            client.DeleteScheduleItemPublishEditCopyRelationship(editCopy);
        }

        /// <summary>
        /// The client for making calls to the service.
        /// </summary>
        HermesScheduleItemServiceClient client = null;

        /// <summary>
        /// Creates a new host for the HermesScheduleItemService and starts the service.
        /// Also creates and opens a client for accessing the service.
        /// </summary>
        private void HostHermesScheduleItemServiceAndOpenClient()
        {
            //Create host
            host = new ServiceHost(typeof(HermesScheduleItemService), endPointAddress);

            //Create a custom binding for enabling transaction flow from client to service
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.TransactionFlow = true;
            tcpBinding.TransactionProtocol = TransactionProtocol.WSAtomicTransactionOctober2004;

            //Create end point for service
            host.AddServiceEndpoint(typeof(HermesScheduleItemService), tcpBinding, endPointAddress);
            host.Open();

            //Create client instance for making calls
            client = new HermesScheduleItemServiceClient(
                tcpBinding, new EndpointAddress(endPointAddress.OriginalString));
            client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            client.Open();
        }

        /// <summary>
        /// Creates MessageHeader instances, and adds them to the current operation context.
        /// </summary>
        private void CreateMessageHeaders()
        {
            OperationContextScope ocScope = new OperationContextScope(client.InnerChannel);

            MessageHeader msgHeader = MessageHeader.CreateHeader("session_application_id", "session_ns", "test");
            OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);

            msgHeader = MessageHeader.CreateHeader("session_id", "session_ns", "session_id_name");
            OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);

            msgHeader = MessageHeader.CreateHeader("session_token", "session_ns", "session_token_name");
            OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);

            msgHeader = MessageHeader.CreateHeader("session_user_id", "session_ns", "session_user_id_name");
            OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);

            msgHeader = MessageHeader.CreateHeader("session_username", "session_ns", "session_username_name");
            OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);

            msgHeader = MessageHeader.CreateHeader("session_culture", "session_ns", "session_culture_name");
            OperationContext.Current.OutgoingMessageHeaders.Add(msgHeader);
        }

        /// <summary>
        /// Host the HermesAuthorizationService service.
        /// </summary>
        private void HostAuthService()
        {
            //Create host
            hostAuth = new ServiceHost(typeof(HermesAuthorizationService), endPointAddressAuth);

            //Create end point for service
            hostAuth.AddServiceEndpoint(typeof(IAuthorization), new BasicHttpBinding(),
                endPointAddressAuth);
            hostAuth.Open();
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
    }
}
