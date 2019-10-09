/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System;
using System.Reflection;
using System.ServiceModel;
using System.Security.Principal;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Transactions;
using System.Collections.Generic;
using TopCoder.Configuration;
using TopCoder.Services.WCF;
using TopCoder.Services.WCF.ScheduleItem;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ObjectFactory;
using HermesNS.TC.Services.AuditTrail;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.ScheduleItem.Persistence;
using HermesNS.TC.Services.GenericNotes;
using HermesNS.TC.Entity.Validation;
using Hermes.Services.Security.Authorization.TopCoder;
using Hermes.Services.Security.Authorization;
using Hermes.Services.Security.Authorization.Client.Common;
using HermesNS.TC.Services.ScheduleItem.Clients;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem.StressTests
{
    /// <summary>
    /// <p>
    /// This test suite benchmarks the performance of the <c>HermesScheduleItemService</c>.
    /// </p>
    /// </summary>
    /// <author>hotblue</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesScheduleItemServiceStressTests
    {

        /// <summary>
        /// Host for the HermesAuditTrailSaveService service
        /// </summary>
        ServiceHost hostAudit = null;

        /// <summary>
        /// Host for the HermesScheduleItemService service
        /// </summary>
        ServiceHost host = null;

        /// <summary>
        /// The host for the HermesAuditTrailRetrieveService service.
        /// </summary>
        ServiceHost hostAuditGet = null;

        /// <summary>
        /// The host for the HermesGenericNoteService service.
        /// </summary>
        ServiceHost hostNote = null;

        /// <summary>
        /// The host for the HermesAuthorizationService service.
        /// </summary>
        ServiceHost hostAuth = null;

        /// <summary>
        /// The client for making calls to the service.
        /// </summary>
        HermesScheduleItemServiceClient client = null;

        /// <summary>
        /// The client of the HermesScheduleItemService to be used for testing.
        /// </summary>
        HermesScheduleItemService service = null;

        /// <summary>
        /// The audit retreiver client for testing the audit functionality
        /// of the HermesScheduleItemService methods.
        /// </summary>
        HermesAuditTrailRetrieveServiceClient auditGetClient = null;

        /// <summary>
        /// The note client for testing the notes functionality
        /// of the HermesScheduleItemService methods.
        /// </summary>
        GenericNotesServiceClient noteClient = null;

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesScheduleItemService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddress =
            new Uri("net.tcp://localhost:12121/HermesScheduleItemService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuditTrailSaveService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAudit =
            new Uri("http://localhost:23232/HermesAuditTrailSaveService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuditTrailRetrieveService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAuditGet =
            new Uri("http://localhost:34343/HermesAuditTrailRetreiveService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesGenericNoteService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressNote =
            new Uri("http://localhost:45454/HermesGenericNoteService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuthorizationService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAuth =
            new Uri("http://localhost:56565/HermesAuthorizationService");

        /// <summary>
        /// The Transaction scope to use.
        /// </summary>
        TransactionScope scope = null;

        /// <summary>
        /// A HermesActivityType client to be used for other entities.
        /// </summary>
        HermesActivityType refHermesActivityType = null;

        /// <summary>
        /// A HermesActivityGroup client to be used for other entities.
        /// </summary>
        HermesActivityGroup refHermesActivityGroup = null;

        /// <summary>
        /// A HermesActivity client to be used for other entities.
        /// </summary>
        HermesActivity refHermesActivity = null;

        /// <summary>
        /// A HermesScheduleItemStatus client to be used for other entities.
        /// </summary>
        HermesScheduleItemStatus refHermesScheduleItemStatus = null;

        /// <summary>
        /// A HermesScheduleItemRequestStatus client to be used for other entities.
        /// </summary>
        HermesScheduleItemRequestStatus refHermesScheduleItemRequestStatus = null;

        /// <summary>
        /// A HermesScheduleItem client to be used for other entities.
        /// </summary>
        HermesScheduleItem refHermesScheduleItem = null;

        /// <summary>
        /// Set up environment once before all tests are run
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            StressTestsHelper.LoadConfiguration();

            // Create service for direct testing
            service = new HermesScheduleItemService();

            // Host the HermesScheduleItemService
            HostHermesScheduleItemServiceAndOpenClient();

            // Host the HermesAuditTrailSaveService
            HostHermesAuditTrailSaveService();

            // Host the HermesAuditTrailRetrieveService
            HostHermesAuditTrailRetrieveServiceAndOpenClient();

            // Host the GenericNoteService
            HostGenericNoteServiceAndOpenClient();

            // Host the HermesAuthorizationService
            HostAuthService();
        }

        /// <summary>
        /// Cleans up environment once after all tests are run.
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            ConfigManager.GetInstance().Clear(false);

            if (host != null)
            {
                host.Close();
            }

            if (hostAudit != null)
            {
                hostAudit.Close();
            }

            if (hostAuditGet != null)
            {
                hostAuditGet.Close();
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

            if (auditGetClient != null)
            {
                try
                {
                    auditGetClient.Close();
                }
                catch { }
            }
        }

        /// <summary>
        /// <para>
        /// Set up the environment for each test.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            // Setup some reference data in the DB
            StressTestsHelper.ClearTestDatabase();
            StressTestsHelper.SetupTestDatabase();

            // Create Message Headers
            CreateMessageHeaders();

            HermesAuthorizationMediator.Configuration = StressTestsHelper.CreateMediatorConfig();

            // Open transaction for calls from service
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = IsolationLevel.RepeatableRead;
            scope = new TransactionScope(TransactionScopeOption.RequiresNew, options);

            // Get some entities already present in database. These are used by the tests.
            CreateReferenceEntities();
        }

        /// <summary>
        /// <para>
        /// Clean the environment for each test.
        /// </para>
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
                // This is required for failure tests
            }

            StressTestsHelper.ClearTestDatabase();
        }

        /// <summary>
        /// Creates a new host for the HermesAuditTrailSaveService and starts the service.
        /// </summary>
        private void HostHermesAuditTrailSaveService()
        {
            // Create host
            hostAudit = new ServiceHost(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailSaveService),
                endPointAddressAudit);

            // Create end point for service
            hostAudit.AddServiceEndpoint(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailSaveService),
                new BasicHttpBinding(), endPointAddressAudit);
            hostAudit.Open();
        }

        /// <summary>
        /// Creates a new host for the HermesScheduleItemService and starts the service.
        /// Also creates and opens a client for accessing the service.
        /// </summary>
        private void HostHermesScheduleItemServiceAndOpenClient()
        {
            // Create host
            host = new ServiceHost(typeof(HermesScheduleItemService), endPointAddress);

            // Create a custom binding for enabling transaction flow from client to service
            NetTcpBinding tcpBinding = new NetTcpBinding();
            tcpBinding.TransactionFlow = true;
            tcpBinding.TransactionProtocol = TransactionProtocol.WSAtomicTransactionOctober2004;

            // Create end point for service
            host.AddServiceEndpoint(typeof(HermesScheduleItemService), tcpBinding, endPointAddress);
            host.Open();

            // Create client client for making calls
            client = new HermesScheduleItemServiceClient(
                tcpBinding, new EndpointAddress(endPointAddress.OriginalString));
            client.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            client.Open();

            // Increase timeout incase tests run more than normal timeout period
            ((IContextChannel)client.InnerChannel).OperationTimeout = new TimeSpan(0, 5, 0);
        }

        /// <summary>
        /// Creates a new host for the HermesAuditTrailRetrieveService and starts the service.
        /// Also creates and opens a client for accessing the service.
        /// </summary>
        private void HostHermesAuditTrailRetrieveServiceAndOpenClient()
        {
            // Create host
            hostAuditGet = new ServiceHost(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailRetrieveService),
                endPointAddressAuditGet);

            // Create end point for service
            hostAuditGet.AddServiceEndpoint(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailRetrieveService),
                new BasicHttpBinding(), endPointAddressAuditGet);
            hostAuditGet.Open();

            // Create client client for making calls
            auditGetClient = new HermesAuditTrailRetrieveServiceClient(new BasicHttpBinding(),
                new EndpointAddress(endPointAddressAuditGet.OriginalString));
            auditGetClient.Open();
        }

        /// <summary>
        /// Host the generic note service.
        /// </summary>
        private void HostGenericNoteServiceAndOpenClient()
        {
            // Create host
            hostNote = new ServiceHost(
                typeof(HermesNS.TC.Services.GenericNotes.HermesGenericNoteService), endPointAddressNote);

            // Create end point for the service
            hostNote.AddServiceEndpoint(typeof(HermesNS.TC.Services.GenericNotes.HermesGenericNoteService),
                new BasicHttpBinding(), endPointAddressNote);

            hostNote.Open();

            noteClient = new GenericNotesServiceClient(new BasicHttpBinding(),
                new EndpointAddress(endPointAddressNote));
            noteClient.Open();
        }

        /// <summary>
        /// Host the HermesAuthorizationService service.
        /// </summary>
        private void HostAuthService()
        {
            // Create host
            hostAuth = new ServiceHost(typeof(HermesAuthorizationService), endPointAddressAuth);

            // Create end point for service
            hostAuth.AddServiceEndpoint(typeof(IAuthorization), new BasicHttpBinding(),
                endPointAddressAuth);
            hostAuth.Open();
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
        /// Creates entity instances to be used for the tests.
        /// </summary>
        private void CreateReferenceEntities()
        {
            // Get an activity type to be used in tests
            refHermesActivityType = client.GetActivityType("22222222222222222222222222222222");

            // Get an activity group to be used in tests
            refHermesActivityGroup = client.GetActivityGroup("11111111111111111111111111111111");

            // Get an activity to be used in tests
            refHermesActivity = client.GetActivity("33333333333333333333333333333333");

            // Get an activity to be used in tests
            refHermesScheduleItemStatus = client.GetScheduleItemStatus("44444444444444444444444444444444");

            // Get an activity to be used in tests
            refHermesScheduleItemRequestStatus =
                client.GetScheduleItemRequestStatus("55555555555555555555555555555555");

            // Get an activity to be used in tests
            refHermesScheduleItem = client.GetScheduleItem("66666666666666666666666666666666");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>CreateActivityGroup(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkCreateActivityGroup()
        {
            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivityGroup activityGroup = client.CreateActivityGroup("AG" + i);
                Assert.IsNotNull(activityGroup, "activityGroup should have been created.");
            }
            StressTestsHelper.Stop("CreateActivityGroup");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetActivityGroup(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetActivityGroup()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivityGroup result = client.GetActivityGroup(activityGroup.Id);
                Assert.IsNotNull(result, "result should have been retrieved.");
            }
            StressTestsHelper.Stop("GetActivityGroup");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetActivityGroup(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteActivityGroup()
        {
            HermesActivityGroup[] activityGroups = new HermesActivityGroup[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                activityGroups[i] = client.CreateActivityGroup("AG" + i);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                client.DeleteActivityGroup(activityGroups[i].Id);
            }
            StressTestsHelper.Stop("DeleteActivityGroup");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetAllActivityGroups(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetAllActivityGroups()
        {
            for (int i = 0; i < 10; i++)
            {
                client.CreateActivityGroup("AG" + i);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                IList<HermesActivityGroup> result = client.GetAllActivityGroups();
                Assert.IsNotNull(result, "result should have been retrieved.");
            }
            StressTestsHelper.Stop("GetAllActivityGroups");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>CreateActivityType(string, ActivityGroup)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkCreateActivityType()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivityType activityType = client.CreateActivityType("AT" + i, activityGroup);
                Assert.IsNotNull(activityType, "activityType should have been created.");
            }
            StressTestsHelper.Stop("CreateActivityType");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetActivityType(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetActivityType()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivityType result = client.GetActivityType(activityType.Id);
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetActivityType");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetAllActivityTypes()</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetAllActivityTypes()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            for (int i = 0; i < 10; i++)
            {
                client.CreateActivityType("AT" + i, activityGroup);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                IList<HermesActivityType> result = client.GetAllActivityTypes();
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetAllActivityTypes");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>DeleteActivityType(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteActivityType()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");

            HermesActivityType[] activityTypes = new HermesActivityType[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                activityTypes[i] = client.CreateActivityType("AT" + i, activityGroup);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                client.DeleteActivityType(activityTypes[i].Id);
            }
            StressTestsHelper.Stop("DeleteActivityType");
        }
        

        /// <summary>
        /// Benchmarks the performance of the <c>CreateScheduleItemStatus(string, string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkCreateScheduleItemStatus()
        {
            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesScheduleItemStatus status = client.CreateScheduleItemStatus("S" + i, "Status" + i);
                Assert.IsNotNull(status, "status should have been created.");
            }
            StressTestsHelper.Stop("CreateScheduleItemStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetScheduleItemStatus(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetScheduleItemStatus()
        {
            HermesScheduleItemStatus status = client.CreateScheduleItemStatus("S1", "Status1");

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesScheduleItemStatus result = client.GetScheduleItemStatus(status.Id);
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetScheduleItemStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetAllScheduleItemStatuses()</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetAllScheduleItemStatuses()
        {
            for (int i = 0; i < 10; i++)
            {
                client.CreateScheduleItemStatus("S" + i, "Status" + i);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                IList<HermesScheduleItemStatus> result = client.GetAllScheduleItemStatuses();
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetAllScheduleItemStatuses");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>DeleteScheduleItemStatus(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteScheduleItemStatus()
        {
            HermesScheduleItemStatus[] statuses = new HermesScheduleItemStatus[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                statuses[i] = client.CreateScheduleItemStatus("S" + i, "Status" + i);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                client.DeleteScheduleItemStatus(statuses[i].Id);
            }
            StressTestsHelper.Stop("DeleteScheduleItemStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>SaveScheduleItemStatus(HermesScheduleItemStatus)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkSaveScheduleItemStatus()
        {
            HermesScheduleItemStatus[] statuses = new HermesScheduleItemStatus[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                statuses[i] = client.CreateScheduleItemStatus("S" + i, "Status" + i);
                statuses[i].Description = "NewStatus" + i;
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesScheduleItemStatus result = client.SaveScheduleItemStatus(statuses[i]);
            }
            StressTestsHelper.Stop("SaveScheduleItemStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>CreateScheduleItemRequestStatus(string, string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkCreateScheduleItemRequestStatus()
        {
            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesScheduleItemRequestStatus status = client.CreateScheduleItemRequestStatus("S" + i, "Status" + i);
                Assert.IsNotNull(status, "status should have been created.");
            }
            StressTestsHelper.Stop("CreateScheduleItemRequestStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetScheduleItemRequestStatus(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus status = client.CreateScheduleItemRequestStatus("S1", "Status1");

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesScheduleItemRequestStatus result = client.GetScheduleItemRequestStatus(status.Id);
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetScheduleItemRequestStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>DeleteScheduleItemRequestStatus(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus[] statuses = 
                new HermesScheduleItemRequestStatus[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                statuses[i] = client.CreateScheduleItemRequestStatus("S" + i, "Status" + i);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                client.DeleteScheduleItemRequestStatus(statuses[i].Id);
            }
            StressTestsHelper.Stop("DeleteScheduleItemRequestStatus");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>SaveScheduleItemRequestStatus(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkSaveScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus[] statuses =
                new HermesScheduleItemRequestStatus[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                statuses[i] = client.CreateScheduleItemRequestStatus("S" + i, "Status" + i);
                statuses[i].Description = "NewDesc" + i;
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesScheduleItemRequestStatus result = client.SaveScheduleItemRequestStatus(statuses[i]);
                Assert.IsNotNull(result, "Should not be null.");
            }
            StressTestsHelper.Stop("SaveScheduleItemRequestStatus");
        }

        /// <summary>
        /// Benchmarks the performance of the <c>GetAllScheduleItemRequestStatuses()</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetAllScheduleItemRequestStatuses()
        {
            for (int i = 0; i < 10; i++)
            {
                client.CreateScheduleItemRequestStatus("S" + i, "Status" + i);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                IList<HermesScheduleItemRequestStatus> result = client.GetAllScheduleItemRequestStatuses();
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetAllScheduleItemRequestStatuses");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>CreateActivity(string, HermesActivityGroup)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkCreateActivity()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivity activity = client.CreateActivity("Test", activityType);
                Assert.IsNotNull(activity, "activity should have been created.");
            }
            StressTestsHelper.Stop("CreateActivity");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetActivity(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetActivity()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivity result = client.GetActivity(activity.Id);
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetActivity");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>DeleteActivity(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteActivity()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity[] activities = new HermesActivity[StressTestsHelper.Iteration];
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                activities[i] = client.CreateActivity("Test", activityType);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                client.DeleteActivity(activities[i].Id);
            }
            StressTestsHelper.Stop("DeleteActivity");
        }

        /// <summary>
        /// Benchmarks the performance of the <c>SaveActivity(HermesActivity)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkSaveActivity()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity[] activities = new HermesActivity[StressTestsHelper.Iteration];
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                activities[i] = client.CreateActivity("Test" + i, activityType);
                activities[i].Abbreviation = "abbr" + i;
                activities[i].ActivityType = activityType;
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                HermesActivity activity = client.SaveActivity(activities[i]);
                Assert.IsNotNull(activity, "Should not be null.");
            }
            StressTestsHelper.Stop("SaveActivity");
        }



        /// <summary>
        /// Benchmarks the performance of the <c>GetAllActivities(bool)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetAllActivities()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);

            for (int i = 0; i < 10; i++)
            {
                client.CreateActivity("Test" + i, activityType);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                IList<HermesActivity> result = client.GetAllActivities(true);
                Assert.IsNotNull(result, "result should have been created.");
            }
            StressTestsHelper.Stop("GetAllActivities");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>CreateScheduleItem(DateTime, HermesActivity)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkCreateScheduleItem()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                // Create the item
                HermesScheduleItem item = client.CreateScheduleItem(DateTime.Today, activity);

                // Verify the result.
                HermesScheduleItem result = client.GetScheduleItem(item.Id);

                Assert.IsNotNull(result, "HermesScheduleItem gotten should not be null.");
            }
            StressTestsHelper.Stop("CreateScheduleItem");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>GetScheduleItem(string)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetScheduleItem()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);

            // Create the item
            HermesScheduleItem item = client.CreateScheduleItem(DateTime.Today, activity);

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                // Retrieve the item.
                HermesScheduleItem result = client.GetScheduleItem(item.Id);

                // Verify the result.
                Assert.IsNotNull(result, "Should not be null.");
            }
            StressTestsHelper.Stop("GetScheduleItem");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>SaveScheduleItem(HermesScheduleItem)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkSaveScheduleItem()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);

            // Create the item
            HermesScheduleItem item = client.CreateScheduleItem(DateTime.Today, activity);

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                item.Duration = i;

                // Save the item.
                HermesScheduleItem result = client.SaveScheduleItem(item);

                // Verify the result.
                Assert.IsNotNull(result, "HermesScheduleItem gotten should not be null.");
            }
            StressTestsHelper.Stop("SaveScheduleItem");
        }


        /// <summary>
        /// Benchmarks the performance of the <c>DeleteScheduleItem(HermesScheduleItem)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteScheduleItem()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);
            HermesScheduleItem[] items = new HermesScheduleItem[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                // Create the item
                items[i] = client.CreateScheduleItem(DateTime.Today, activity);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                // Delete the item.
                client.DeleteScheduleItem(items[i].Id);
            }
            StressTestsHelper.Stop("DeleteScheduleItem");
        }



        /// <summary>
        /// Benchmarks the performance of the <c>SaveScheduleItems(IList&lt;HermesScheduleItem&gt;)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkSaveScheduleItems()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);

            // Create the item
            HermesScheduleItem item = client.CreateScheduleItem(DateTime.Today, activity);

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                item.Duration = i;

                IList<HermesScheduleItem> items = new List<HermesScheduleItem>();
                items.Add(item);

                // Save the item.
                IList<HermesScheduleItem> result = client.SaveScheduleItems(items);

                // Verify the result.
                Assert.IsNotNull(result, "Should not be null.");
            }
            StressTestsHelper.Stop("SaveScheduleItems");
        }



        /// <summary>
        /// Benchmarks the performance of the <c>DeleteScheduleItems(IList&lt;string&gt;)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkDeleteScheduleItems()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);
            HermesScheduleItem[] items = new HermesScheduleItem[StressTestsHelper.Iteration];

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                // Create the item
                items[i] = client.CreateScheduleItem(DateTime.Today, activity);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                IList<string> ids = new List<string>();
                ids.Add(items[i].Id);

                // Delete the items.
                client.DeleteScheduleItems(ids);
            }
            StressTestsHelper.Stop("DeleteScheduleItems");
        }

        /// <summary>
        /// Benchmarks the performance of the <c>GetScheduleItemEditCopy(HermesScheduleItem)</c> method.
        /// </summary>
        [Test]
        public void BenchmarkGetScheduleItemEditCopy()
        {
            HermesActivityGroup activityGroup = client.CreateActivityGroup("AG1");
            HermesActivityType activityType = client.CreateActivityType("AT1", activityGroup);
            HermesActivity activity = client.CreateActivity("Test", activityType);
            HermesScheduleItem[] items = new HermesScheduleItem[StressTestsHelper.Iteration];
            HermesScheduleItem parent = client.CreateScheduleItem(DateTime.Today, activity);

            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                // Create the item
                items[i] = client.CreateScheduleItemEditCopy(parent);
            }

            StressTestsHelper.Start();
            for (int i = 0; i < StressTestsHelper.Iteration; i++)
            {
                client.GetScheduleItemEditCopy(parent);
            }
            StressTestsHelper.Stop("GetScheduleItemEditCopy");
        }
    }
}