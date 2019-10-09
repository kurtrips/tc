/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
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
using HermesNS.TC.Services.GenericNotes.Entities;
using HermesNS.TC.Entity.Validation;
using Hermes.Services.Security.Authorization.TopCoder;
using Hermes.Services.Security.Authorization;
using Hermes.Services.Security.Authorization.Client.Common;
using HermesNS.TC.Services.ScheduleItem.Clients;
using HermesNS.TC.LoggingWrapperPublisher;
using NUnit.Framework;

namespace HermesNS.TC.Services.ScheduleItem
{
    /// <summary>
    /// Unit tests for the HermesScheduleItemService class.
    /// </summary>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    [CoverageExcludeAttribute]
    public class HermesScheduleItemServiceTests
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
        /// The instance of the HermesScheduleItemService to be used for testing.
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
            new Uri("net.tcp://localhost:11111/HermesScheduleItemService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuditTrailSaveService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAudit =
            new Uri("http://localhost:22222/HermesAuditTrailSaveService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuditTrailRetrieveService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAuditGet =
            new Uri("http://localhost:33333/HermesAuditTrailRetreiveService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesGenericNoteService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressNote =
            new Uri("http://localhost:44444/HermesGenericNoteService");

        /// <summary>
        /// <para>
        /// Represents the endpoint address used for HermesAuthorizationService.
        /// </para>
        /// </summary>
        private static readonly Uri endPointAddressAuth =
            new Uri("http://localhost:55555/HermesAuthorizationService");

        /// <summary>
        /// The Transaction scope to use.
        /// </summary>
        TransactionScope scope = null;

        /// <summary>
        /// A HermesActivityType instance to be used for other entities.
        /// This entity is already present in the database. See test_files/CreateTestData.sql
        /// </summary>
        HermesActivityType refHermesActivityType = null;

        /// <summary>
        /// A HermesActivityGroup instance to be used for other entities.
        /// This entity is already present in the database. See test_files/CreateTestData.sql
        /// </summary>
        HermesActivityGroup refHermesActivityGroup = null;

        /// <summary>
        /// A HermesActivity instance to be used for other entities.
        /// This entity is already present in the database. See test_files/CreateTestData.sql
        /// </summary>
        HermesActivity refHermesActivity = null;

        /// <summary>
        /// A HermesScheduleItemStatus instance to be used for other entities.
        /// This entity is already present in the database. See test_files/CreateTestData.sql
        /// </summary>
        HermesScheduleItemStatus refHermesScheduleItemStatus = null;

        /// <summary>
        /// A HermesScheduleItemRequestStatus instance to be used for other entities.
        /// This entity is already present in the database. See test_files/CreateTestData.sql
        /// </summary>
        HermesScheduleItemRequestStatus refHermesScheduleItemRequestStatus = null;

        /// <summary>
        /// A HermesScheduleItem instance to be used for other entities.
        /// This entity is already present in the database. See test_files/CreateTestData.sql
        /// </summary>
        HermesScheduleItem refHermesScheduleItem = null;

        /// <summary>
        /// Set up environment once before all tests are run
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            try
            {
                UnitTestHelper.LoadConfigMgrFiles();

                //Create service for direct testing
                service = new HermesScheduleItemService();

                //Host the HermesScheduleItemService
                HostHermesScheduleItemServiceAndOpenClient();

                //Host the HermesAuditTrailSaveService
                HostHermesAuditTrailSaveService();

                //Host the HermesAuditTrailRetrieveService
                HostHermesAuditTrailRetrieveServiceAndOpenClient();

                //Host the GenericNoteService
                HostGenericNoteServiceAndOpenClient();

                //Host the HermesAuthorizationService
                HostAuthService();
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Console.WriteLine(e.Message);
                    e = e.InnerException;
                }
            }
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
            try
            {
                //Setup some reference data in the DB
                UnitTestHelper.ClearTestDatabase();
                UnitTestHelper.SetupTestDatabase();

                //Create Message Headers
                CreateMessageHeaders();

                HermesAuthorizationMediator.Configuration = UnitTestHelper.CreateMediatorConfig();

                //Open transaction for calls from service
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = IsolationLevel.RepeatableRead;
                scope = new TransactionScope(TransactionScopeOption.RequiresNew, options);

                //Get some entities already present in database. These are used by the tests.
                CreateReferenceEntities();
            }
            catch (Exception e)
            {
                while (e != null)
                {
                    Console.WriteLine(e.Message);
                    e = e.InnerException;
                }
            }
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
                //This is required for failure tests
            }

            UnitTestHelper.ClearTestDatabase();
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

            //Increase timeout incase tests run more than normal timeout period
            ((IContextChannel)client.InnerChannel).OperationTimeout = new TimeSpan(0, 5, 0);
        }

        /// <summary>
        /// Creates a new host for the HermesAuditTrailRetrieveService and starts the service.
        /// Also creates and opens a client for accessing the service.
        /// </summary>
        private void HostHermesAuditTrailRetrieveServiceAndOpenClient()
        {
            //Create host
            hostAuditGet = new ServiceHost(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailRetrieveService),
                endPointAddressAuditGet);

            //Create end point for service
            hostAuditGet.AddServiceEndpoint(typeof(HermesNS.TC.Services.AuditTrail.HermesAuditTrailRetrieveService),
                new BasicHttpBinding(), endPointAddressAuditGet);
            hostAuditGet.Open();

            //Create client instance for making calls
            auditGetClient = new HermesAuditTrailRetrieveServiceClient(new BasicHttpBinding(),
                new EndpointAddress(endPointAddressAuditGet.OriginalString));
            auditGetClient.Open();
        }

        /// <summary>
        /// Host the generic note service.
        /// </summary>
        private void HostGenericNoteServiceAndOpenClient()
        {
            //Create host
            hostNote = new ServiceHost(
                typeof(HermesNS.TC.Services.GenericNotes.HermesGenericNoteService), endPointAddressNote);

            //Create end point for the service
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
            //Create host
            hostAuth = new ServiceHost(typeof(HermesAuthorizationService), endPointAddressAuth);

            //Create end point for service
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
            //Get an activity type to be used in tests
            refHermesActivityType = client.GetActivityType("22222222222222222222222222222222");

            //Get an activity group to be used in tests
            refHermesActivityGroup = client.GetActivityGroup("11111111111111111111111111111111");

            //Get an activity to be used in tests
            refHermesActivity = client.GetActivity("33333333333333333333333333333333");

            //Get an activity to be used in tests
            refHermesScheduleItemStatus = client.GetScheduleItemStatus("44444444444444444444444444444444");

            //Get an activity to be used in tests
            refHermesScheduleItemRequestStatus =
                client.GetScheduleItemRequestStatus("55555555555555555555555555555555");

            //Get an activity to be used in tests
            refHermesScheduleItem = client.GetScheduleItem("66666666666666666666666666666666");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for accuracy.
        /// public HermesScheduleItemService()
        /// </summary>
        [Test]
        public void TestCtor1()
        {
            Assert.IsTrue(service is ScheduleItemServiceBase<string, HermesScheduleItem, HermesActivity,
                HermesScheduleItemStatus, HermesScheduleItemRequestStatus, HermesActivityGroup,
                HermesActivityType, HermesGenericNote, HermesGenericNoteItem, HermesGenericNoteItemHistory>,
                "Wrong type of class.");

            Assert.AreEqual(HermesScheduleItemService.DefaultNamespace,
                "HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService",
                "DefaultNamespace declaration is missing.");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService class for DefaultNamespace field.
        /// </summary>
        [Test]
        public void TestDefaultNamespace()
        {
            Assert.AreEqual(HermesScheduleItemService.DefaultNamespace,
                "HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService",
                "DefaultNamespace declaration is incorrect.");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for accuracy.
        /// public HermesScheduleItemService(string nameSpace)
        /// </summary>
        [Test]
        public void TestCtor2()
        {
            service = new HermesScheduleItemService("MyOtherNamespace");

            Assert.IsTrue(service is ScheduleItemServiceBase<string, HermesScheduleItem, HermesActivity,
                HermesScheduleItemStatus, HermesScheduleItemRequestStatus, HermesActivityGroup,
                HermesActivityType, HermesGenericNote, HermesGenericNoteItem, HermesGenericNoteItemHistory>,
                "Wrong type of class.");

            Assert.AreEqual(HermesScheduleItemService.DefaultNamespace,
                "HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService",
                "DefaultNamespace declaration is missing.");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when namespace is null.
        /// public HermesScheduleItemService(string nameSpace)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor2Fail1()
        {
            service = new HermesScheduleItemService(null);
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when namespace is empty.
        /// public HermesScheduleItemService(string nameSpace)
        /// InvalidArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor2Fail2()
        {
            service = new HermesScheduleItemService("       ");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when a required property is missing.
        /// public HermesScheduleItemService(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2Fail3()
        {
            service = new HermesScheduleItemService("MyFailureNamespace1");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when any property has empty value.
        /// public HermesScheduleItemService(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2Fail4()
        {
            service = new HermesScheduleItemService("MyFailureNamespace2");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when persistence object created
        /// using Object factory is not of HermesScheduleItemPersistenceProvider type.
        /// public HermesScheduleItemService(string nameSpace)
        /// ScheduleItemConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ScheduleItemConfigurationException))]
        public void TestCtor2Fail5()
        {
            service = new HermesScheduleItemService("MyFailureNamespace3");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for accuracy.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// </summary>
        [Test]
        public void TestCtor3()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
            ObjectFactory of = new ConfigurationObjectFactory("TestOFNamespace");
            string auditClientKey = "abcd";
            string genericNotesClientKey = "efgh";
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);

            Assert.IsTrue(service is ScheduleItemServiceBase<string, HermesScheduleItem, HermesActivity,
                HermesScheduleItemStatus, HermesScheduleItemRequestStatus, HermesActivityGroup,
                HermesActivityType, HermesGenericNote, HermesGenericNoteItem, HermesGenericNoteItemHistory>,
                "Wrong type of class.");

            Assert.AreEqual(HermesScheduleItemService.DefaultNamespace,
                "HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService",
                "DefaultNamespace declaration is missing.");
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when persistence is null.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// InvalidArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3Fail1()
        {
            HermesScheduleItemPersistenceProvider hsipp = null;
            ObjectFactory of = new ConfigurationObjectFactory("TestOFNamespace");
            string auditClientKey = "abcd";
            string genericNotesClientKey = "efgh";
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when objectFactory is null.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// InvalidArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3Fail2()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
            ObjectFactory of = null;
            string auditClientKey = "abcd";
            string genericNotesClientKey = "efgh";
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when auditClientKey is null.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// InvalidArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3Fail3()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
            ObjectFactory of = new ConfigurationObjectFactory("TestOFNamespace");
            string auditClientKey = null;
            string genericNotesClientKey = "efgh";
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when auditClientKey is empty.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// InvalidArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3Fail4()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
            ObjectFactory of = new ConfigurationObjectFactory("TestOFNamespace");
            string auditClientKey = "      ";
            string genericNotesClientKey = "efgh";
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when genericNotesClientKey is null.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// InvalidArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3Fail5()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
            ObjectFactory of = new ConfigurationObjectFactory("TestOFNamespace");
            string auditClientKey = "    s  ";
            string genericNotesClientKey = null;
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);
        }

        /// <summary>
        /// Tests the HermesScheduleItemService constructor for failure when genericNotesClientKey is empty.
        /// public HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string, string)
        /// InvalidArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(InvalidArgumentException))]
        public void TestCtor3Fail6()
        {
            HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
            ObjectFactory of = new ConfigurationObjectFactory("TestOFNamespace");
            string auditClientKey = "    s  ";
            string genericNotesClientKey = "            ";
            HermesLogger logger = new HermesLogger("SomeLogger");

            service = new HermesScheduleItemService(hsipp, of, auditClientKey, genericNotesClientKey, logger);
        }

        /// <summary>
        /// Tests the CreateActivity method of the service for accuracy.
        /// </summary>
        [Test]
        public void TestCreateActivity()
        {
            TestCreateActivity("ANewActivity");
        }

        /// <summary>
        /// Tests the CreateActivity method of the service for accuracy.
        /// </summary>
        private HermesActivity TestCreateActivity(string name)
        {
            //Create activity
            HermesActivity activity = client.CreateActivity(name, refHermesActivityType);
            Assert.IsNotNull(activity, "Returned instance was null");

            //Get from DB
            HermesActivity fromDb = client.GetActivity(activity.Id);

            //Check all properties for equality
            CompareProperties(activity, fromDb);

            return activity;
        }

        /// <summary>
        /// Tests the CreateActivity method of the service when name is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityFail1()
        {
            client.CreateActivity(null, refHermesActivityType);
        }

        /// <summary>
        /// Tests the CreateActivity method of the service when name is empty.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityFail2()
        {
            client.CreateActivity("            ", refHermesActivityType);
        }

        /// <summary>
        /// Tests the CreateActivity method of the service when type is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityFail3()
        {
            client.CreateActivity("jksdfhk", null);
        }

        /// <summary>
        /// Tests the CreateActivity method of the service when authorization fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");
            client.CreateActivity("ANewActivity", refHermesActivityType);
        }

        /// <summary>
        /// Tests the CreateActivityGroup method of the service for accuracy.
        /// </summary>
        [Test]
        public void TestCreateActivityGroup()
        {
            //Create activity group
            TestCreateActivityGroup("MyHagAbbr");
        }

        /// <summary>
        /// Creates a HermesActivityGroup with the given abbreviation.
        /// </summary>
        /// <param name="abbr">The abbreviation for the new HermesActivityGroup</param>
        /// <returns>The created HermesActivityGroup</returns>
        private HermesActivityGroup TestCreateActivityGroup(string abbr)
        {
            //Create activity group
            HermesActivityGroup activityGrp = client.CreateActivityGroup(abbr);
            Assert.IsNotNull(activityGrp, "Returned instance was null.");

            //Get from DB
            HermesActivityGroup fromDb = client.GetActivityGroup(activityGrp.Id);

            //Check all properties for equality
            CompareProperties(activityGrp, fromDb);

            return activityGrp;
        }

        /// <summary>
        /// Tests the CreateActivityGroup method of the service for failure when authentican fails.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityGroupFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.CreateActivityGroup("MyHagAbbr");
        }

        /// <summary>
        /// Tests the CreateActivityGroup method of the service for failure when abbreviation is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityGroupFail2()
        {
            client.CreateActivityGroup(null);
        }

        /// <summary>
        /// Tests the CreateActivityGroup method of the service for failure when abbreviation is empty.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityGroupFail3()
        {
            client.CreateActivityGroup("              ");
        }

        /// <summary>
        /// Tests the CreateActivityType method of the service for accuracy.
        /// </summary>
        [Test]
        public void TestCreateActivityType()
        {
            //Create activity type
            TestCreateActivityType("myHATAbbr");
        }

        /// <summary>
        /// Creates a ActivityType with given abbreviation.
        /// </summary>
        /// <param name="abbr">The abbreviation.</param>
        /// <returns>The created HermesActivityType instance.</returns>
        private HermesActivityType TestCreateActivityType(string abbr)
        {
            //Create activity type
            HermesActivityType activityTyp = client.CreateActivityType(abbr, refHermesActivityGroup);
            Assert.IsNotNull(activityTyp, "Returned instance was null.");

            //Get from DB
            HermesActivityType fromDb = client.GetActivityType(activityTyp.Id);

            //Check all properties for equality
            CompareProperties(activityTyp, fromDb);

            return activityTyp;
        }

        /// <summary>
        /// Tests the CreateActivityType method of the service for failure when abbreviation is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityTypeFail1()
        {
            client.CreateActivityType(null, refHermesActivityGroup);
        }

        /// <summary>
        /// Tests the CreateActivityType method of the service for failure when abbreviation is empty.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityTypeFail2()
        {
            client.CreateActivityType("           ", refHermesActivityGroup);
        }

        /// <summary>
        /// Tests the CreateActivityType method of the service for failure when activityGroup is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityTypeFail3()
        {
            client.CreateActivityType("sdf", null);
        }


        /// <summary>
        /// Tests the CreateActivityType method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateActivityTypeFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");
            client.CreateActivityType("skjdf", refHermesActivityGroup);
        }

        /// <summary>
        /// Tests the CreateScheduleItemStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestCreateScheduleItemStatus()
        {
            TestCreateScheduleItemStatus("MSIS", "jhadfkjhadfkj");
        }

        /// <summary>
        /// Creates a new HermesScheduleItemStatus instance using the given description and abbreviation.
        /// </summary>
        /// <param name="description">The description for the HermesScheduleItemStatus</param>
        /// <param name="abbreviation">The abbreviation for the HermesScheduleItemStatus</param>
        /// <returns>The created HermesScheduleItemStatus instance</returns>
        private HermesScheduleItemStatus TestCreateScheduleItemStatus(string abbreviation, string description)
        {
            //Create schedule item status
            HermesScheduleItemStatus st = client.CreateScheduleItemStatus(abbreviation, description);
            Assert.IsNotNull(st, "Returned instance was null.");

            //Get from DB
            HermesScheduleItemStatus fromDb = client.GetScheduleItemStatus(st.Id);

            //Check all properties for equality
            CompareProperties(st, fromDb);

            return st;
        }

        /// <summary>
        /// Tests the CreateScheduleItemStatus method of the service for failure when abbreviation is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFail1()
        {
            client.CreateScheduleItemStatus("someDesc", null);
        }

        /// <summary>
        /// Tests the CreateScheduleItemStatus method of the service for failure when abbreviation is empty.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFail2()
        {
            client.CreateScheduleItemStatus("someDesc", "          ");
        }

        /// <summary>
        /// Tests the CreateScheduleItemStatus method of the service for failure when description is empty.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFail3()
        {
            client.CreateScheduleItemStatus("     ", "dsf");
        }

        /// <summary>
        /// Tests the CreateScheduleItemStatus method of the service for failure when description is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFail4()
        {
            client.CreateScheduleItemStatus(null, "dsf");
        }

        /// <summary>
        /// Tests the CreateScheduleItemStatus method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemStatusFail5()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");
            client.CreateScheduleItemStatus("skjdf", "sdhf");
        }

        /// <summary>
        /// Tests the CreateScheduleItemRequestStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestCreateScheduleItemRequestStatus()
        {
            TestCreateScheduleItemRequestStatus("reqSt", "RS");
        }

        /// <summary>
        /// Creates a HermesScheduleItemRequestStatus with the given abbreviation and description and
        /// tests the CreateScheduleItemRequestStatus method for accuracy.
        /// </summary>
        private HermesScheduleItemRequestStatus TestCreateScheduleItemRequestStatus(string abbr, string desc)
        {
            //Create schedule item status
            HermesScheduleItemRequestStatus st = client.CreateScheduleItemRequestStatus(abbr, desc);
            Assert.IsNotNull(st, "Returned instance was null.");

            //Get from DB
            HermesScheduleItemRequestStatus fromDb = client.GetScheduleItemRequestStatus(st.Id);

            //Check all properties for equality
            CompareProperties(st, fromDb);

            return st;
        }

        /// <summary>
        /// Tests the CreateScheduleItemRequestStatus method of the service for failure when abbreviation is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFail1()
        {
            client.CreateScheduleItemRequestStatus("someDesc", null);
        }

        /// <summary>
        /// Tests the CreateScheduleItemRequestStatus method of the service for failure when abbreviation is empty.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFail2()
        {
            client.CreateScheduleItemRequestStatus("someDesc", "         ");
        }

        /// <summary>
        /// Tests the CreateScheduleItemRequestStatus method of the service for failure when description is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFail3()
        {
            client.CreateScheduleItemRequestStatus(null, "sdf");
        }

        /// <summary>
        /// Tests the CreateScheduleItemRequestStatus method of the service for failure when description is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFail4()
        {
            client.CreateScheduleItemRequestStatus("        ", "sdf");
        }

        /// <summary>
        /// Tests the CreateScheduleItemRequestStatus method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemRequestStatusFail5()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");
            client.CreateScheduleItemRequestStatus("skjdf", "sdhf");
        }

        /// <summary>
        /// Tests the CreateScheduleItem method for accuracy.
        /// </summary>
        [Test]
        public void TestCreateScheduleItem()
        {
            //Create schedule item
            HermesScheduleItem st = client.CreateScheduleItem(new DateTime(2007, 12, 13), refHermesActivity);
            Assert.IsNotNull(st, "Returned instance was null.");

            //Get from DB
            HermesScheduleItem fromDb = client.GetScheduleItem(st.Id);

            //Check all properties for equality
            CompareProperties(st, fromDb);
        }

        /// <summary>
        /// Tests the CreateScheduleItem method of the service for failure when Activity is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemFail1()
        {
            client.CreateScheduleItem(new DateTime(2007, 12, 13), null);
        }

        /// <summary>
        /// Tests the CreateScheduleItem method of the service for failure when Activity
        /// is not present in the database.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemFail2()
        {
            HermesActivity activity = new HermesActivity();
            activity.Id = Guid.NewGuid().ToString();
            client.CreateScheduleItem(new DateTime(2007, 12, 13), activity);
        }

        /// <summary>
        /// Tests the CreateScheduleItem method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemFail3()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");
            client.CreateScheduleItem(new DateTime(2007, 12, 13), refHermesActivity);
        }

        /// <summary>
        /// Tests the DeleteActivity method for accuracy.
        /// </summary>
        public void TestDeleteActivity()
        {
            //First create an activity
            HermesActivity activity = TestCreateActivity("SomeActivity");

            //Now delete
            client.DeleteActivity(activity.Id);
            Assert.IsNull(client.GetActivity(activity.Id), "Now activity must be deleted.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], activity.Id, "HermesActivity.DeletedAudit");
        }

        /// <summary>
        /// Tests the DeleteActivity method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an activity. Now delete
            HermesActivity activity = TestCreateActivity("SomeActivity");
            client.DeleteActivity(activity.Id);
        }

        /// <summary>
        /// Tests the DeleteActivity method of the service for failure when there is error
        /// in database like foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityFail2()
        {
            client.DeleteActivity("33333333333333333333333333333333");
        }

        /// <summary>
        /// Tests the DeleteActivities method for accuracy.
        /// </summary>
        public void TestDeleteActivities()
        {
            //First create 5 activities
            IList<HermesActivity> activities = new List<HermesActivity>();
            for (int i = 0; i < 5; i++)
            {
                activities.Add(TestCreateActivity("myActivity" + i));
            }

            //Now delete them
            IList<string> ids = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                ids.Add(activities[i].Id);
            }

            client.DeleteActivities(ids);

            //All activities must be deleted
            for (int i = 0; i < ids.Count; i++)
            {
                Assert.IsNull(client.GetActivity(ids[i]), "Activity at index " + i + " must be deleted.");
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            //5 audit records must be present
            Assert.AreEqual(5, auditRecords.Count, "5 audit records must be present.");
            for (int i = 0; i < ids.Count; i++)
            {
                CheckAuditRecord(auditRecords[i], activities[i].Id, "HermesActivity.DeletedAudit");
            }

        }

        /// <summary>
        /// Tests the DeleteActivities method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivitiesFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an activity. Now delete
            HermesActivity activity = TestCreateActivity("SomeActivity");
            IList<string> ids = new List<string>();
            ids.Add(activity.Id);
            client.DeleteActivities(ids);
        }

        /// <summary>
        /// Tests the DeleteActivities method of the service for failure when there is error in database
        /// like a foreign key constraint.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivitiesFail2()
        {
            IList<string> ids = new List<string>();
            ids.Add("33333333333333333333333333333333");

            client.DeleteActivities(ids);
        }

        /// <summary>
        /// Tests the DeleteActivityGroup method for accuracy.
        /// </summary>
        public void TestDeleteActivityGroup()
        {
            //First create an activity
            HermesActivityGroup activityGrp = TestCreateActivityGroup("SomeAbbr");

            //Now delete
            client.DeleteActivityGroup(activityGrp.Id);
            Assert.IsNull(client.GetActivityGroup(activityGrp.Id), "Now activity group must be deleted.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], activityGrp.Id, "HermesActivityGroup.DeletedAudit");
        }

        /// <summary>
        /// Tests the DeleteActivityGroup method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityGroupFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an activity group. Now delete
            HermesActivityGroup activityGrp = TestCreateActivityGroup("SomeAbbr");
            client.DeleteActivityGroup(activityGrp.Id);
        }

        /// <summary>
        /// Tests the DeleteActivityGroup method of the service for failure when there is error
        /// in database like foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityGroupFail2()
        {
            client.DeleteActivityGroup("33333333333333333333333333333333");
        }

        /// <summary>
        /// Tests the DeleteActivityGroups method for accuracy.
        /// </summary>
        public void TestDeleteActivityGroups()
        {
            //First create 5 activity groups
            IList<HermesActivityGroup> actGrps = new List<HermesActivityGroup>();
            for (int i = 0; i < 5; i++)
            {
                actGrps.Add(TestCreateActivityGroup("MyHagAbbr" + i));
            }

            //Now delete them
            IList<string> ids = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                ids.Add(actGrps[i].Id);
            }

            client.DeleteActivityGroups(ids);

            //All activity groups must be deleted
            for (int i = 0; i < ids.Count; i++)
            {
                Assert.IsNull(client.GetActivityGroup(ids[i]), "ActivityGroup at index " + i + " must be deleted.");
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            //5 audit records must be present
            Assert.AreEqual(5, auditRecords.Count, "5 audit records must be present.");
            for (int i = 0; i < ids.Count; i++)
            {
                CheckAuditRecord(auditRecords[i], actGrps[i].Id, "HermesActivityGroup.DeletedAudit");
            }

        }

        /// <summary>
        /// Tests the DeleteActivityGroups method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityGroupsFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an activity. Now delete
            HermesActivityGroup actGrp = TestCreateActivityGroup("SomeAbbr");
            IList<string> ids = new List<string>();
            ids.Add(actGrp.Id);
            client.DeleteActivityGroups(ids);
        }

        /// <summary>
        /// Tests the DeleteActivityGroups method of the service for failure when there is error in database
        /// like a foreign key constraint.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityGroupsFail2()
        {
            IList<string> ids = new List<string>();
            ids.Add("11111111111111111111111111111111");

            client.DeleteActivityGroups(ids);
        }

        /// <summary>
        /// Tests the DeleteActivityType method for accuracy.
        /// </summary>
        public void TestDeleteActivityType()
        {
            //First create an activity
            HermesActivityType activityTyp = TestCreateActivityType("SomeAbbr");

            //Now delete
            client.DeleteActivityType(activityTyp.Id);
            Assert.IsNull(client.GetActivityType(activityTyp.Id), "Now activity type must be deleted.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], activityTyp.Id, "HermesActivityType.DeletedAudit");
        }

        /// <summary>
        /// Tests the DeleteActivityType method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityTypeFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an activity type. Now delete.
            HermesActivityType activityTyp = TestCreateActivityType("SomeAbbr");
            client.DeleteActivityType(activityTyp.Id);
        }

        /// <summary>
        /// Tests the DeleteActivityType method of the service for failure when there is error
        /// in database like foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityTypeFail2()
        {
            client.DeleteActivityType("11111111111111111111111111111111");
        }

        /// <summary>
        /// Tests the DeleteActivityTypes method for accuracy.
        /// </summary>
        public void TestDeleteActivityTypes()
        {
            //First create 5 activity types
            IList<HermesActivityType> actTypes = new List<HermesActivityType>();
            IList<string> ids = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                actTypes.Add(TestCreateActivityType("MyHatAbbr" + i));
                ids.Add(actTypes[i].Id);
            }

            //Now delete them
            client.DeleteActivityTypes(ids);

            //All activity types must be deleted
            for (int i = 0; i < ids.Count; i++)
            {
                Assert.IsNull(client.GetActivityType(ids[i]), "ActivityType at index " + i + " must be deleted.");
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            //5 audit records must be present
            Assert.AreEqual(5, auditRecords.Count, "5 audit records must be present.");
            for (int i = 0; i < ids.Count; i++)
            {
                CheckAuditRecord(auditRecords[i], actTypes[i].Id, "HermesActivityType.DeletedAudit");
            }

        }

        /// <summary>
        /// Tests the DeleteActivityTypes method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityTypesFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an activity. Now delete
            HermesActivityType actTyp = TestCreateActivityType("SomeAbbr");
            IList<string> ids = new List<string>();
            ids.Add(actTyp.Id);
            client.DeleteActivityTypes(ids);
        }

        /// <summary>
        /// Tests the DeleteActivityTypes method of the service for failure when there is error in database
        /// like a foreign key constraint.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteActivityTypesFail2()
        {
            IList<string> ids = new List<string>();
            ids.Add("22222222222222222222222222222222");

            client.DeleteActivityTypes(ids);
        }

        /// <summary>
        /// Tests the DeleteScheduleItem method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItem()
        {
            //First create a schedule item
            HermesScheduleItem entity = new HermesScheduleItem();
            entity.Id = Guid.NewGuid().ToString();
            entity.ScheduleItemRequestStatus = client.GetScheduleItemRequestStatus("55555555555555555555555555555555");
            entity.ScheduleItemStatus = client.GetScheduleItemStatus("44444444444444444444444444444444");
            entity.Activity = client.GetActivity("33333333333333333333333333333333");
            HermesGenericNote genNote = new HermesGenericNote();
            genNote.Description = "TestGenNote";
            entity.Note = genNote;

            //Save the schedule item
            entity = client.SaveScheduleItem(entity);
            Assert.IsNotNull(client.GetScheduleItem(entity.Id), "Schedule item must be saved.");
            //The note is also saved
            Assert.IsNotNull(noteClient.GetGenericNote(entity.Note.Id, TimeZone.CurrentTimeZone, null, null),
                "The note must be saved with a new id.");

            //Now delete
            client.DeleteScheduleItem(entity.Id);
            Assert.IsNull(client.GetScheduleItem(entity.Id), "Now schedule item must be deleted.");
            Assert.IsNull(noteClient.GetGenericNote(entity.Note.Id, TimeZone.CurrentTimeZone, null, null),
                "The note must also be deleted now.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], entity.Id, "HermesScheduleItem.DeletedAudit");
        }

        /// <summary>
        /// Tests the DeleteScheduleItem method of the service for failure when authentican fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //First create an schedule item. Now delete.
            HermesScheduleItem entity = client.CreateScheduleItem(new DateTime(2007, 12, 13), refHermesActivity);
            client.DeleteScheduleItem(entity.Id);
        }

        /// <summary>
        /// Tests the DeleteScheduleItem method of the service for failure when there is error
        /// because schedule item with given id is not found in database.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemFail2()
        {
            client.DeleteScheduleItem("12121212121212121212121212121212");
        }

        /// <summary>
        /// Tests the DeleteScheduleItems method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItems()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItem> savedSchedItems = new List<HermesScheduleItem>();

            //First create a few schedule items
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItem entity = new HermesScheduleItem();
                entity.Id = Guid.NewGuid().ToString();
                entity.ScheduleItemRequestStatus =
                    client.GetScheduleItemRequestStatus("55555555555555555555555555555555");
                entity.ScheduleItemStatus = client.GetScheduleItemStatus("44444444444444444444444444444444");
                entity.Activity = client.GetActivity("33333333333333333333333333333333");
                HermesGenericNote genNote = new HermesGenericNote();
                genNote.Description = "TestGenNote" + i;
                entity.Note = genNote;

                //Save the schedule item
                entity = client.SaveScheduleItem(entity);

                ids.Add(entity.Id);
                savedSchedItems.Add(entity);
            }

            //Now delete
            client.DeleteScheduleItems(ids);

            for (int i = 0; i < 5; i++)
            {
                Assert.IsNull(client.GetScheduleItem(savedSchedItems[i].Id), "Now schedule item must be deleted.");
                Assert.IsNull(
                    noteClient.GetGenericNote(savedSchedItems[i].Note.Id, TimeZone.CurrentTimeZone, null, null),
                    "The note must also be deleted now.");
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], savedSchedItems[i].Id, "HermesScheduleItem.DeletedAudit");
            }
        }

        /// <summary>
        /// Tests the DeleteScheduleItems method for failure when some id to delete does not exist in database.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemsFail1()
        {
            IList<string> ids = new List<string>();
            ids.Add("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

            client.DeleteScheduleItems(ids);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemStatus()
        {
            //First create
            HermesScheduleItemStatus entity = TestCreateScheduleItemStatus("SomeDesc", "SomeAbbr");

            //Now delete
            client.DeleteScheduleItemStatus(entity.Id);
            Assert.IsNull(client.GetScheduleItem(entity.Id), "Now schedule item status must be deleted.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], entity.Id, "HermesScheduleItemStatus.DeletedAudit");
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatus method of the service for failure when the entity cannot
        /// be removed from database because of foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemStatusFail1()
        {
            client.DeleteScheduleItemStatus("44444444444444444444444444444444");
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatuses method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemStatuses()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItemStatus> entities = new List<HermesScheduleItemStatus>();
            //First create a few entities
            for (int i = 0; i < 5; i++)
            {
                entities.Add(TestCreateScheduleItemStatus("myDesc" + i, "myAbbr" + i));
                ids.Add(entities[i].Id);
            }

            //Now delete them
            client.DeleteScheduleItemStatuses(ids);

            //All schedule item statuses must be deleted
            for (int i = 0; i < ids.Count; i++)
            {
                Assert.IsNull(client.GetScheduleItemStatus(ids[i]),
                    "HermesScheduleItemStatus at index " + i + " must be deleted.");
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            //5 audit records must be present
            Assert.AreEqual(5, auditRecords.Count, "5 audit records must be present.");
            for (int i = 0; i < ids.Count; i++)
            {
                CheckAuditRecord(auditRecords[i], entities[i].Id, "HermesScheduleItemStatus.DeletedAudit");
            }
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatuses method of the service for failure when one of the ids cannot
        /// be removed from database because of foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemStatusesFail1()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItemStatus> entities = new List<HermesScheduleItemStatus>();
            //First create a few entities
            for (int i = 0; i < 5; i++)
            {
                entities.Add(TestCreateScheduleItemStatus("myDesc" + i, "myAbbr" + i));
                ids.Add(entities[i].Id);
            }
            ids.Add("44444444444444444444444444444444");

            client.DeleteScheduleItemStatuses(ids);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemStatus method of the service for failure when one of the ids cannot
        /// be removed from database because it is not present in the database at all.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemStatusesFail2()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItemStatus> entities = new List<HermesScheduleItemStatus>();
            //First create a few entities
            for (int i = 0; i < 5; i++)
            {
                entities.Add(TestCreateScheduleItemStatus("myDesc" + i, "myAbbr" + i));
                ids.Add(entities[i].Id);
            }
            ids.Add("ABCDABCDABCDABCDABCDABCDABCDABCD");

            client.DeleteScheduleItemStatuses(ids);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemRequestStatus()
        {
            //First create
            HermesScheduleItemRequestStatus entity = TestCreateScheduleItemRequestStatus("SomeAbbr", "SomeDesc");

            //Now delete
            client.DeleteScheduleItemRequestStatus(entity.Id);
            Assert.IsNull(client.GetScheduleItem(entity.Id), "Now schedule item request status must be deleted.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], entity.Id, "HermesScheduleItemRequestStatus.DeletedAudit");
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatus method of the service for failure when the entity cannot
        /// be removed from database because of foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemRequestStatusFail1()
        {
            client.DeleteScheduleItemRequestStatus("55555555555555555555555555555555");
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatuses method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemRequestStatuses()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItemRequestStatus> entities = new List<HermesScheduleItemRequestStatus>();
            //First create a few entities
            for (int i = 0; i < 5; i++)
            {
                entities.Add(TestCreateScheduleItemRequestStatus("myDesc" + i, "myAbbr" + i));
                ids.Add(entities[i].Id);
            }

            //Now delete them
            client.DeleteScheduleItemRequestStatuses(ids);

            //All schedule item statuses must be deleted
            for (int i = 0; i < ids.Count; i++)
            {
                Assert.IsNull(client.GetScheduleItemRequestStatus(ids[i]),
                    "HermesScheduleItemRequestStatus at index " + i + " must be deleted.");
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            //5 audit records must be present
            Assert.AreEqual(5, auditRecords.Count, "5 audit records must be present.");
            for (int i = 0; i < ids.Count; i++)
            {
                CheckAuditRecord(auditRecords[i], entities[i].Id, "HermesScheduleItemRequestStatus.DeletedAudit");
            }
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatuses method of the service for failure when one of the ids cannot
        /// be removed from database because of foreign key violation.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemRequestStatusesFail1()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItemRequestStatus> entities = new List<HermesScheduleItemRequestStatus>();
            //First create a few entities
            for (int i = 0; i < 5; i++)
            {
                entities.Add(TestCreateScheduleItemRequestStatus("myDesc" + i, "myAbbr" + i));
                ids.Add(entities[i].Id);
            }
            ids.Add("55555555555555555555555555555555");

            client.DeleteScheduleItemRequestStatuses(ids);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatuses method of the service for failure when one of the ids cannot
        /// be removed from database because it is not present in the database at all.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemRequestStatusesFail2()
        {
            IList<string> ids = new List<string>();
            IList<HermesScheduleItemRequestStatus> entities = new List<HermesScheduleItemRequestStatus>();
            //First create a few entities
            for (int i = 0; i < 5; i++)
            {
                entities.Add(TestCreateScheduleItemRequestStatus("myDesc" + i, "myAbbr" + i));
                ids.Add(entities[i].Id);
            }
            ids.Add("ABCDABCDABCDABCDABCDABCDABCDABCD");

            client.DeleteScheduleItemRequestStatuses(ids);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatuses method of the service for failure when ids is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemRequestStatusesFail3()
        {
            client.DeleteScheduleItemRequestStatuses(null);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemRequestStatuses method of the service for failure when ids contains null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemRequestStatusesFail4()
        {
            IList<string> ids = new List<string>();
            ids.Add(null);
            client.DeleteScheduleItemRequestStatuses(ids);
        }

        /// <summary>
        /// Tests the GetActivity method for accuracy.
        /// </summary>
        [Test]
        public void TestGetActivity1()
        {
            //First create an activity
            HermesActivity savedInDb = TestCreateActivity("TestActivity");
            //Get
            HermesActivity gotFromDb = client.GetActivity(savedInDb.Id);
            //Compare all properties
            CompareProperties(savedInDb, gotFromDb);
        }

        /// <summary>
        /// Tests the GetActivity method for accuracy when given id is not found in database.
        /// Must return null.
        /// </summary>
        [Test]
        public void TestGetActivity2()
        {
            //Get
            Assert.IsNull(client.GetActivity("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
                "Must return null if id is not found.");
        }

        /// <summary>
        /// Tests the GetActivity method for failure when id is not valid guid.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetActivityFail1()
        {
            client.GetActivity("NotAValidGuid");
        }

        /// <summary>
        /// Tests the GetActivityGroup method for accuracy.
        /// </summary>
        [Test]
        public void TestGetActivityGroup1()
        {
            //First create an activity group
            HermesActivityGroup savedInDb = TestCreateActivityGroup("SomeAbbr");
            //Get
            HermesActivityGroup gotFromDb = client.GetActivityGroup(savedInDb.Id);
            //Compare all properties
            CompareProperties(savedInDb, gotFromDb);
        }

        /// <summary>
        /// Tests the GetActivityGroup method for accuracy when given id is not found in database.
        /// Must return null.
        /// </summary>
        [Test]
        public void TestGetActivityGroup2()
        {
            //Get
            Assert.IsNull(client.GetActivityGroup("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
                "Must return null if id is not found.");
        }

        /// <summary>
        /// Tests the GetActivityGroup method for failure when id is not valid guid.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetActivityGroupFail1()
        {
            client.GetActivityGroup("NotAValidGuid");
        }

        /// <summary>
        /// Tests the GetActivityType method for accuracy.
        /// </summary>
        [Test]
        public void TestGetActivityType1()
        {
            //First create an activity type
            HermesActivityType savedInDb = TestCreateActivityType("SomeAbbre");
            //Get
            HermesActivityType gotFromDb = client.GetActivityType(savedInDb.Id);
            //Compare all properties
            CompareProperties(savedInDb, gotFromDb);
        }

        /// <summary>
        /// Tests the GetActivityType method for accuracy when given id is not found in database.
        /// Must return null.
        /// </summary>
        [Test]
        public void TestGetActivityType2()
        {
            //Get
            Assert.IsNull(client.GetActivityType("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
                "Must return null if id is not found.");
        }

        /// <summary>
        /// Tests the GetActivityType method for failure when id is not valid guid.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetActivityTypeFail1()
        {
            client.GetActivityType("NotAValidGuid");
        }

        /// <summary>
        /// Tests the GetScheduleItemStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestGetScheduleItemStatus1()
        {
            //First create an schedule item status
            HermesScheduleItemStatus savedInDb = TestCreateScheduleItemStatus("SomeDesc", "SomeAbbr");
            //Get
            HermesScheduleItemStatus gotFromDb = client.GetScheduleItemStatus(savedInDb.Id);
            //Compare all properties
            CompareProperties(savedInDb, gotFromDb);
        }

        /// <summary>
        /// Tests the GetScheduleItemStatus method for accuracy when given id is not found in database.
        /// Must return null.
        /// </summary>
        [Test]
        public void TestGetScheduleItemStatus2()
        {
            //Get
            Assert.IsNull(client.GetScheduleItemStatus("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
                "Must return null if id is not found.");
        }

        /// <summary>
        /// Tests the GetScheduleItemStatus method for failure when id is not valid guid.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemStatusFail1()
        {
            client.GetScheduleItemStatus("NotAValidGuid");
        }

        /// <summary>
        /// Tests the GetScheduleItemRequestStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestGetScheduleItemRequestStatus1()
        {
            //First create an schedule item status
            HermesScheduleItemRequestStatus savedInDb = TestCreateScheduleItemRequestStatus("SomeAbbr", "SomeDesc");
            //Get
            HermesScheduleItemRequestStatus gotFromDb = client.GetScheduleItemRequestStatus(savedInDb.Id);
            //Compare all properties
            CompareProperties(savedInDb, gotFromDb);
        }

        /// <summary>
        /// Tests the GetScheduleItemRequestStatus method for accuracy when given id is not found in database.
        /// Must return null.
        /// </summary>
        [Test]
        public void TestGetScheduleItemRequestStatus2()
        {
            //Get
            Assert.IsNull(client.GetScheduleItemRequestStatus("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
                "Must return null if id is not found.");
        }

        /// <summary>
        /// Tests the GetScheduleItemRequestStatus method for failure when id is not valid guid.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemRequestStatusFail1()
        {
            client.GetScheduleItemRequestStatus("NotAValidGuid");
        }

        /// <summary>
        /// Tests the GetScheduleItem method for accuracy.
        /// </summary>
        [Test]
        public void TestGetScheduleItem1()
        {
            //First create an schedule item status
            HermesScheduleItem savedInDb = client.CreateScheduleItem(new DateTime(2007, 12, 13), refHermesActivity);
            //Get
            HermesScheduleItem gotFromDb = client.GetScheduleItem(savedInDb.Id);
            //Compare all properties
            CompareProperties(savedInDb, gotFromDb);
        }

        /// <summary>
        /// Tests the GetScheduleItem method for accuracy when given id is not found in database.
        /// Must return null.
        /// </summary>
        [Test]
        public void TestGetScheduleItem2()
        {
            //Get
            Assert.IsNull(client.GetScheduleItem("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"),
                "Must return null if id is not found.");
        }

        /// <summary>
        /// Tests the GetScheduleItemRequestStatus method for failure when id is not valid guid.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemFail1()
        {
            client.GetScheduleItem("NotAValidGuid");
        }

        /// <summary>
        /// Tests the GetAllActivities method for accuracy.
        /// </summary>
        [Test]
        public void TestGetAllActivities()
        {
            //First add some activities
            IDictionary<string, HermesActivity> activities = new Dictionary<string, HermesActivity>();
            for (int i=1; i<=3; i++)
            {
                HermesActivity act = new HermesActivity();
                act.Abbreviation = "abbr" + i;
                act.ActivityType = client.GetActivityType("22222222222222222222222222222222");
                act.DefaultDuration = 10 +i;
                act.DefaultExpireDays = 20 + i;
                act.DefaultStartTime = 30 + i;
                act.Enabled = i % 2 == 0 ? true : false;
                act.ExclusiveFlag = i % 2 == 0 ? true : false;
                act.Name = "name" + i;
                act.WorkDayAmount = 40 + i;
                act.Id = Guid.NewGuid().ToString();

                activities[act.Id] = client.SaveActivity(act);
            }
            //Note that there are a total of 4 activities in the database as there is 1 before hand.

            //Test when getting disabled activities also
            IList<HermesActivity> gotFromDb = client.GetAllActivities(true);
            for (int i = 0; i < gotFromDb.Count; i++)
            {
                if (gotFromDb[i].Id != new Guid("33333333333333333333333333333333").ToString())
                {
                    Assert.IsTrue(activities.ContainsKey(gotFromDb[i].Id), "Wrong GetAllActivities implementation.");
                    CompareProperties(gotFromDb[i], activities[gotFromDb[i].Id]);
                }
            }
            Assert.AreEqual(4, gotFromDb.Count, "total 4 activities must be retreived.");

            //Test when not getting disabled activities.
            gotFromDb = client.GetAllActivities(false);
            for (int i = 0; i < gotFromDb.Count; i++)
            {
                Assert.IsTrue(gotFromDb[i].Enabled, "Only enabled activities must be retreived.");
                if (gotFromDb[i].Id != new Guid("33333333333333333333333333333333").ToString())
                {
                    CompareProperties(gotFromDb[i], activities[gotFromDb[i].Id]);
                }
            }
            Assert.AreEqual(2, gotFromDb.Count, "Only 2 activities must be retreived.");
        }

        /// <summary>
        /// Tests the GetAllActivities method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetAllActivitiesFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.GetAllActivities(true);
        }

        /// <summary>
        /// Tests the GetAllActivityGroups method for accuracy.
        /// </summary>
        [Test]
        public void TestGetAllActivityGroups()
        {
            //First add some activity groups
            IDictionary<string, HermesActivityGroup> entities = new Dictionary<string, HermesActivityGroup>();
            for (int i = 1; i <= 3; i++)
            {
                HermesActivityGroup ent = new HermesActivityGroup();
                ent.Abbreviation = "myAbbr" + i;
                ent.Id = Guid.NewGuid().ToString();
                ent.Name = "myName" + i;

                entities[ent.Id] = client.SaveActivityGroup(ent);
            }
            //Note that there are a total of 4 activity groups in the database as there is 1 before hand.

            IList<HermesActivityGroup> gotFromDb = client.GetAllActivityGroups();
            for (int i = 0; i < gotFromDb.Count; i++)
            {
                if (gotFromDb[i].Id != new Guid("11111111111111111111111111111111").ToString())
                {
                    Assert.IsTrue(entities.ContainsKey(gotFromDb[i].Id), "Wrong GetAllActivityGroups implementation.");
                    CompareProperties(gotFromDb[i], entities[gotFromDb[i].Id]);
                }
            }
            Assert.AreEqual(4, gotFromDb.Count, "total 4 activity groups must be retreived.");
        }

        /// <summary>
        /// Tests the GetAllActivityGroups method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetAllActivityGroupsFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.GetAllActivityGroups();
        }

        /// <summary>
        /// Tests the GetAllActivityTypes method for accuracy.
        /// </summary>
        [Test]
        public void TestGetAllActivityTypes()
        {
            //First add some activity types
            IDictionary<string, HermesActivityType> entities = new Dictionary<string, HermesActivityType>();
            for (int i = 1; i <= 3; i++)
            {
                HermesActivityType ent = new HermesActivityType();
                ent.Abbreviation = "myAbbr" + i;
                ent.Id = Guid.NewGuid().ToString();
                ent.Name = "myName" + i;
                ent.ActivityGroup = refHermesActivityGroup;

                entities[ent.Id] = client.SaveActivityType(ent);
            }
            //Note that there are a total of 4 activity types in the database as there is 1 before hand.

            IList<HermesActivityType> gotFromDb = client.GetAllActivityTypes();
            for (int i = 0; i < gotFromDb.Count; i++)
            {
                if (gotFromDb[i].Id != new Guid("22222222222222222222222222222222").ToString())
                {
                    Assert.IsTrue(entities.ContainsKey(gotFromDb[i].Id), "Wrong GetAllActivityTypes implementation.");
                    CompareProperties(gotFromDb[i], entities[gotFromDb[i].Id]);
                }
            }
            Assert.AreEqual(4, gotFromDb.Count, "total 4 activity types must be retreived.");
        }

        /// <summary>
        /// Tests the GetAllActivityTypes method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetAllActivityTypesFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.GetAllActivityTypes();
        }

        /// <summary>
        /// Tests the GetAllScheduleItemRequestStatuses method for accuracy.
        /// </summary>
        [Test]
        public void TestGetAllScheduleItemRequestStatuses()
        {
            //First add some schedule item request statuses
            IDictionary<string, HermesScheduleItemRequestStatus> entities =
                new Dictionary<string, HermesScheduleItemRequestStatus>();
            for (int i = 1; i <= 3; i++)
            {
                HermesScheduleItemRequestStatus ent = new HermesScheduleItemRequestStatus();
                ent.Abbreviation = "myAbbr" + i;
                ent.Id = Guid.NewGuid().ToString();
                ent.Description = "myDesc" + i;

                entities[ent.Id] = client.SaveScheduleItemRequestStatus(ent);
            }
            //Note that there are a total of 4 schedule item request statuses in the database as there is 1 before hand.

            IList<HermesScheduleItemRequestStatus> gotFromDb = client.GetAllScheduleItemRequestStatuses();
            for (int i = 0; i < gotFromDb.Count; i++)
            {
                if (gotFromDb[i].Id != new Guid("55555555555555555555555555555555").ToString())
                {
                    Assert.IsTrue(entities.ContainsKey(gotFromDb[i].Id),
                        "Wrong GetAllScheduleItemRequestStatuses implementation.");
                    CompareProperties(gotFromDb[i], entities[gotFromDb[i].Id]);
                }
            }
            Assert.AreEqual(4, gotFromDb.Count, "total 4 schedule item request statuses must be retreived.");
        }

        /// <summary>
        /// Tests the GetAllScheduleItemRequestStatuses method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetAllScheduleItemRequestStatusesFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.GetAllScheduleItemRequestStatuses();
        }

        /// <summary>
        /// Tests the GetAllScheduleItemStatuses method for accuracy.
        /// </summary>
        [Test]
        public void TestGetAllScheduleItemStatuses()
        {
            //First add some schedule item statuses
            IDictionary<string, HermesScheduleItemStatus> entities =
                new Dictionary<string, HermesScheduleItemStatus>();
            for (int i = 1; i <= 3; i++)
            {
                HermesScheduleItemStatus ent = new HermesScheduleItemStatus();
                ent.Abbreviation = "myAbbr" + i;
                ent.Id = Guid.NewGuid().ToString();
                ent.Description = "myDesc" + i;

                entities[ent.Id] = client.SaveScheduleItemStatus(ent);
            }
            //Note that there are a total of 6 schedule item statuses in the database as there is 3 before hand.

            IList<HermesScheduleItemStatus> gotFromDb = client.GetAllScheduleItemStatuses();
            for (int i = 0; i < gotFromDb.Count; i++)
            {
                //All entities already in db start with "4444444444444444444444444444444" with only last digit differing
                if (!gotFromDb[i].Id.StartsWith("44444444-4444-4444-4444-44444444444"))
                {
                    Assert.IsTrue(entities.ContainsKey(gotFromDb[i].Id),
                        "Wrong GetAllScheduleItemStatuses implementation.");
                    CompareProperties(gotFromDb[i], entities[gotFromDb[i].Id]);
                }
            }
            Assert.AreEqual(6, gotFromDb.Count, "total 6 schedule item statuses must be retreived.");
        }

        /// <summary>
        /// Tests the GetAllScheduleItemStatuses method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetAllScheduleItemStatusesFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.GetAllScheduleItemStatuses();
        }

        /// <summary>
        /// Tests the SaveActivity method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveActivity()
        {
            //Create an entity
            HermesActivity ent = new HermesActivity();
            ent.Abbreviation = "abbr";
            ent.ActivityType = refHermesActivityType;
            ent.DefaultDuration = new decimal(32.79);
            ent.DefaultExpireDays = 4;
            ent.DefaultStartTime = 5;
            ent.Enabled = true;
            ent.ExclusiveFlag = false;
            ent.Name = "someName";
            ent.WorkDayAmount = 7;
            ent.Id = Guid.NewGuid().ToString();

            //Save
            ent = client.SaveActivity(ent);

            //Retreive
            HermesActivity fromDb = client.GetActivity(ent.Id);

            //Check all properties for equality
            CompareProperties(ent, fromDb);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], ent.Id, "HermesActivity.CreatedAudit");
        }

        /// <summary>
        /// Tests the SaveActivity method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.SaveActivity(refHermesActivity);
        }

        /// <summary>
        /// Tests the SaveActivity method for failure when validation fails because name is null.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityFail2()
        {
            refHermesActivity.Name = null;
            client.SaveActivity(refHermesActivity);
        }

        /// <summary>
        /// Tests the SaveActivity method for failure when validation fails because the activity
        /// has an activity type which is not present in the database.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityFail3()
        {
            refHermesActivity.ActivityType.Id = Guid.NewGuid().ToString();
            client.SaveActivity(refHermesActivity);
        }

        /// <summary>
        /// Tests the SaveActivity method for failure when activity is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityFail4()
        {
            client.SaveActivity(null);
        }

        /// <summary>
        /// Tests the SaveActivityGroup method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveActivityGroup()
        {
            HermesActivityGroup hag = new HermesActivityGroup();
            hag.Abbreviation = "abbr";
            hag.Id = Guid.NewGuid().ToString();
            hag.Name = "name";

            //Save
            hag = client.SaveActivityGroup(hag);

            //Retreive
            HermesActivityGroup fromDb = client.GetActivityGroup(hag.Id);

            //Check all properties for equality
            CompareProperties(hag, fromDb);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], hag.Id, "HermesActivityGroup.CreatedAudit");
        }

        /// <summary>
        /// Tests the SaveActivityGroup method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.SaveActivityGroup(refHermesActivityGroup);
        }

        /// <summary>
        /// Tests the SaveActivityGroup method for failure when validation fails because name is null.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityGroupFail2()
        {
            refHermesActivityGroup.Name = null;
            client.SaveActivityGroup(refHermesActivityGroup);
        }

        /// <summary>
        /// Tests the SaveActivityGroup method for failure when validation fails because
        /// it has abbreviation greater than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityGroupFail3()
        {
            refHermesActivityGroup.Abbreviation = "jfgjsdbfmnsadbfmnasdbfhsajfbasdmnfbghsdfjh";
            client.SaveActivityGroup(refHermesActivityGroup);
        }

        /// <summary>
        /// Tests the SaveActivityGroup method for failure when activityGroup is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupFail4()
        {
            client.SaveActivityGroup(null);
        }

        /// <summary>
        /// Tests the SaveActivityType method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveActivityType()
        {
            HermesActivityType hat = new HermesActivityType();
            hat.Abbreviation = "abbr";
            hat.Id = Guid.NewGuid().ToString();
            hat.Name = "name";
            hat.ActivityGroup = refHermesActivityGroup;

            //Save
            hat = client.SaveActivityType(hat);

            //Retreive
            HermesActivityType fromDb = client.GetActivityType(hat.Id);

            //Check all properties for equality
            CompareProperties(hat, fromDb);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], hat.Id, "HermesActivityType.CreatedAudit");
        }

        /// <summary>
        /// Tests the SaveActivityType method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypeFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.SaveActivityType(refHermesActivityType);
        }

        /// <summary>
        /// Tests the SaveActivityType method for failure when validation fails because name is null.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityTypeFail2()
        {
            refHermesActivityType.Name = null;
            client.SaveActivityType(refHermesActivityType);
        }

        /// <summary>
        /// Tests the SaveActivityType method for failure when validation fails because
        /// it has abbreviation greater than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityTypeFail3()
        {
            refHermesActivityType.Abbreviation = "jfgjsdbfmnsadbfmnasdbfhsajfbasdmnfbghsdfjh";
            client.SaveActivityType(refHermesActivityType);
        }

        /// <summary>
        /// Tests the SaveActivityType method for failure when validation fails because
        /// it has actvity group which does not exist in the database.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityTypeFail4()
        {
            refHermesActivityType.ActivityGroup.Id = Guid.NewGuid().ToString();
            client.SaveActivityType(refHermesActivityType);
        }

        /// <summary>
        /// Tests the SaveActivityType method for failure when activityType is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypeFail5()
        {
            client.SaveActivityType(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveScheduleItemStatus()
        {
            HermesScheduleItemStatus hsis = new HermesScheduleItemStatus();
            hsis.Abbreviation = "abbr";
            hsis.Id = Guid.NewGuid().ToString();
            hsis.Description = "name";

            //Save
            hsis = client.SaveScheduleItemStatus(hsis);

            //Retreive
            HermesScheduleItemStatus fromDb = client.GetScheduleItemStatus(hsis.Id);

            //Check all properties for equality
            CompareProperties(hsis, fromDb);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], hsis.Id, "HermesScheduleItemStatus.CreatedAudit");
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.SaveScheduleItemStatus(refHermesScheduleItemStatus);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method for failure when validation fails because Description is null.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemStatusFail2()
        {
            refHermesScheduleItemStatus.Description = null;
            client.SaveScheduleItemStatus(refHermesScheduleItemStatus);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method for failure when validation fails because
        /// it has abbreviation greater than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemStatusFail3()
        {
            refHermesScheduleItemStatus.Abbreviation = "jfgjsdbfmnsadbfmnasdbfhsajfbasdmnfbghsdfjh";
            client.SaveScheduleItemStatus(refHermesScheduleItemStatus);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatus method for failure when scheduleItemStatus is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusFail4()
        {
            client.SaveScheduleItemStatus(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveScheduleItemRequestStatus()
        {
            HermesScheduleItemRequestStatus hsis = new HermesScheduleItemRequestStatus();
            hsis.Abbreviation = "abbr";
            hsis.Id = Guid.NewGuid().ToString();
            hsis.Description = "name";

            //Save
            hsis = client.SaveScheduleItemRequestStatus(hsis);

            //Retreive
            HermesScheduleItemRequestStatus fromDb = client.GetScheduleItemRequestStatus(hsis.Id);

            //Check all properties for equality
            CompareProperties(hsis, fromDb);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], hsis.Id, "HermesScheduleItemRequestStatus.CreatedAudit");
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.SaveScheduleItemRequestStatus(refHermesScheduleItemRequestStatus);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method for failure when
        /// validation fails because Description is null.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemRequestStatusFail2()
        {
            refHermesScheduleItemRequestStatus.Description = null;
            client.SaveScheduleItemRequestStatus(refHermesScheduleItemRequestStatus);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method for failure when validation fails because
        /// it has abbreviation greater than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemRequestStatusFail3()
        {
            refHermesScheduleItemRequestStatus.Abbreviation = "jfgjsdbfmnsadbfmnasdbfhsajfbasdmnfbghsdfjh";
            client.SaveScheduleItemRequestStatus(refHermesScheduleItemRequestStatus);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatus method for failure when scheduleItemRequestStatus is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusFail4()
        {
            client.SaveScheduleItemRequestStatus(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveScheduleItem()
        {
            HermesScheduleItem hsi = new HermesScheduleItem();
            hsi.Activity = refHermesActivity;
            hsi.Duration = new decimal(56.78);
            hsi.ExceptionFlag = 'Y';
            hsi.ExpirationDate = new DateTime(2008, 6, 7);
            hsi.Id = Guid.NewGuid().ToString();

            HermesGenericNote note = new HermesGenericNote();
            note.Description = "SomeNoteDesc";
            hsi.Note = note;

            hsi.ScheduleItemRequestStatus = refHermesScheduleItemRequestStatus;
            hsi.ScheduleItemStatus = refHermesScheduleItemStatus;
            hsi.Version = 2;
            hsi.WorkDate = new DateTime(2008, 3, 4);
            hsi.WorkDayAmount = new decimal(98.09);

            //Save
            hsi = client.SaveScheduleItem(hsi);

            //Retreive
            HermesScheduleItem fromDb = client.GetScheduleItem(hsi.Id);

            //Check all properties for equality
            CompareProperties(hsi, fromDb);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            CheckAuditRecord(auditRecords[0], hsi.Id, "HermesScheduleItem.CreatedAudit");

            //Check whether a new note was added correctly
            HermesGenericNote noteSaved =
                noteClient.GetGenericNote(hsi.Note.Id, TimeZone.CurrentTimeZone, null, null);
            Assert.IsNotNull(noteSaved, "Note must be saved.");
            Assert.AreEqual("SomeNoteDesc", noteSaved.Description, "Description must be correct.");
            Assert.IsNotNull(noteSaved.Id, "New id must be generated for note.");
        }

        /// <summary>
        /// Tests the SaveScheduleItem method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.SaveScheduleItem(refHermesScheduleItem);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method for failure when
        /// its activity does not exist in the database.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemFail2()
        {
            HermesActivity act = new HermesActivity();
            act.Id = Guid.NewGuid().ToString();
            refHermesScheduleItem.Activity = act;

            client.SaveScheduleItem(refHermesScheduleItem);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method for failure when validation fails because
        /// it has ScheduleItemStatus which does not exist in database
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemFail3()
        {
            HermesScheduleItemStatus st = new HermesScheduleItemStatus();
            st.Id = Guid.NewGuid().ToString();
            refHermesScheduleItem.ScheduleItemStatus = st;
            client.SaveScheduleItem(refHermesScheduleItem);
        }

        /// <summary>
        /// Tests the SaveScheduleItem method for failure when scheduleItem is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemFail4()
        {
            client.SaveScheduleItem(null);
        }

        /// <summary>
        /// Tests the SaveActivities method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveActivities()
        {
            //Create 5 entities
            IList<HermesActivity> activities = new List<HermesActivity>();
            for (int i = 0; i < 5; i++)
            {
                HermesActivity ent = new HermesActivity();
                ent.Abbreviation = "abbr" + i;
                ent.ActivityType = refHermesActivityType;
                ent.DefaultDuration = new decimal(32.79 + i);
                ent.DefaultExpireDays = 4 + i;
                ent.DefaultStartTime = 5 + i;
                ent.Enabled = i % 2 == 0 ? true : false;
                ent.ExclusiveFlag = i % 2 == 0 ? false : true;
                ent.Name = "someName" + i;
                ent.WorkDayAmount = 7 + i;
                ent.Id = Guid.NewGuid().ToString();

                activities.Add(ent);
            }

            //Save all of them
            activities = client.SaveActivities(activities);

            //Check that all are saved correctly
            for (int i = 0; i < 5; i++)
            {
                HermesActivity activity = client.GetActivity(activities[i].Id);
                CompareProperties(activity, activities[i]);
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], activities[i].Id, "HermesActivity.CreatedAudit");
            }
        }

        /// <summary>
        /// Tests the SaveActivities method for failure when activities is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivitiesFail1()
        {
            client.SaveActivities(null);
        }

        /// <summary>
        /// Tests the SaveActivities method for failure when activities has null element null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivitiesFail2()
        {
            IList<HermesActivity> activities = new List<HermesActivity>();
            activities.Add(null);
            client.SaveActivities(activities);
        }

        /// <summary>
        /// Tests the SaveActivities method for failure when one of the activities fails validation
        /// because its abbreviation is more than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivitiesFail3()
        {
            //Create entities
            IList<HermesActivity> activities = new List<HermesActivity>();
            refHermesActivity.Abbreviation = "abbresfdjghsjdfkgkjfsdhjkfasgdfmhbasdjghak";
            activities.Add(refHermesActivity);

            client.SaveActivities(activities);
        }

        /// <summary>
        /// Tests the SaveActivities method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivitiesFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //Create entities
            IList<HermesActivity> activities = new List<HermesActivity>();
            activities.Add(refHermesActivity);

            client.SaveActivities(activities);
        }

        /// <summary>
        /// Tests the SaveActivityGroups method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveActivityGroups()
        {
            //Create 5 entities
            IList<HermesActivityGroup> activityGroups = new List<HermesActivityGroup>();
            for (int i = 0; i < 5; i++)
            {
                HermesActivityGroup ent = new HermesActivityGroup();
                ent.Abbreviation = "abbr" + i;
                ent.Name = "someName" + i;
                ent.Id = Guid.NewGuid().ToString();

                activityGroups.Add(ent);
            }

            //Save all of them
            activityGroups = client.SaveActivityGroups(activityGroups);

            //Check that all are saved correctly
            for (int i = 0; i < 5; i++)
            {
                HermesActivityGroup activityGroup = client.GetActivityGroup(activityGroups[i].Id);
                CompareProperties(activityGroup, activityGroups[i]);
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], activityGroups[i].Id, "HermesActivityGroup.CreatedAudit");
            }
        }

        /// <summary>
        /// Tests the SaveActivityGroups method for failure when activityGroups is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupsFail1()
        {
            client.SaveActivityGroups(null);
        }

        /// <summary>
        /// Tests the SaveActivityGroups method for failure when activityGroups has null element null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupsFail2()
        {
            IList<HermesActivityGroup> activityGroups = new List<HermesActivityGroup>();
            activityGroups.Add(null);
            client.SaveActivityGroups(activityGroups);
        }

        /// <summary>
        /// Tests the SaveActivityGroups method for failure when one of the activityGroups fails validation
        /// because its abbreviation is more than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityGroupsFail3()
        {
            //Create entities
            IList<HermesActivityGroup> activityGroups = new List<HermesActivityGroup>();
            refHermesActivityGroup.Abbreviation = "abbresfdjghsjdfkgkjfsdhjkfasgdfmhbasdjghak";
            activityGroups.Add(refHermesActivityGroup);

            client.SaveActivityGroups(activityGroups);
        }

        /// <summary>
        /// Tests the SaveActivityGroups method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityGroupsFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //Create entities
            IList<HermesActivityGroup> activityGroupGroups = new List<HermesActivityGroup>();
            activityGroupGroups.Add(refHermesActivityGroup);

            client.SaveActivityGroups(activityGroupGroups);
        }

        /// <summary>
        /// Tests the SaveActivityTypes method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveActivityTypes()
        {
            //Create 5 entities
            IList<HermesActivityType> activityTypes = new List<HermesActivityType>();
            for (int i = 0; i < 5; i++)
            {
                HermesActivityType ent = new HermesActivityType();
                ent.Abbreviation = "abbr" + i;
                ent.Name = "someName" + i;
                ent.ActivityGroup = refHermesActivityGroup;
                ent.Id = Guid.NewGuid().ToString();

                activityTypes.Add(ent);
            }

            //Save all of them
            activityTypes = client.SaveActivityTypes(activityTypes);

            //Check that all are saved correctly
            for (int i = 0; i < 5; i++)
            {
                HermesActivityType activityType = client.GetActivityType(activityTypes[i].Id);
                CompareProperties(activityType, activityTypes[i]);
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], activityTypes[i].Id, "HermesActivityType.CreatedAudit");
            }
        }

        /// <summary>
        /// Tests the SaveActivityTypes method for failure when activityTypes is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypesFail1()
        {
            client.SaveActivityTypes(null);
        }

        /// <summary>
        /// Tests the SaveActivityTypes method for failure when activityTypes has null element.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypesFail2()
        {
            IList<HermesActivityType> activityTypes = new List<HermesActivityType>();
            activityTypes.Add(null);
            client.SaveActivityTypes(activityTypes);
        }

        /// <summary>
        /// Tests the SaveActivityTypes method for failure when one of the activityTypes fails validation
        /// because its abbreviation is more than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveActivityTypesFail3()
        {
            //Create entities
            IList<HermesActivityType> activityTypes = new List<HermesActivityType>();
            refHermesActivityType.Abbreviation = "abbresfdjghsjdfkgkjfsdhjkfasgdfmhbasdjghak";
            activityTypes.Add(refHermesActivityType);

            client.SaveActivityTypes(activityTypes);
        }

        /// <summary>
        /// Tests the SaveActivityTypes method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveActivityTypesFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //Create entities
            IList<HermesActivityType> activityTypes = new List<HermesActivityType>();
            activityTypes.Add(refHermesActivityType);

            client.SaveActivityTypes(activityTypes);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatuses method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveScheduleItemStatuses()
        {
            //Create 5 entities
            IList<HermesScheduleItemStatus> itemStatuses = new List<HermesScheduleItemStatus>();
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItemStatus ent = new HermesScheduleItemStatus();
                ent.Abbreviation = "abbr" + i;
                ent.Description = "someName" + i;
                ent.Id = Guid.NewGuid().ToString();

                itemStatuses.Add(ent);
            }

            //Save all of them
            itemStatuses = client.SaveScheduleItemStatuses(itemStatuses);

            //Check that all are saved correctly
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItemStatus itemStatus = client.GetScheduleItemStatus(itemStatuses[i].Id);
                CompareProperties(itemStatus, itemStatuses[i]);
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], itemStatuses[i].Id, "HermesScheduleItemStatus.CreatedAudit");
            }
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatuses method for failure when scheduleItemStatuses is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusesFail1()
        {
            client.SaveScheduleItemStatuses(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatuses method for failure when scheduleItemStatuses has null element null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusesFail2()
        {
            IList<HermesScheduleItemStatus> itemStatuses = new List<HermesScheduleItemStatus>();
            itemStatuses.Add(null);
            client.SaveScheduleItemStatuses(itemStatuses);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatuses method for failure when one of the scheduleItemStatuses fails validation
        /// because its abbreviation is more than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemStatusesFail3()
        {
            //Create entities
            IList<HermesScheduleItemStatus> itemStatuses = new List<HermesScheduleItemStatus>();
            refHermesScheduleItemStatus.Abbreviation = "abbresfdjghsjdfkgkjfsdhjkfasgdfmhbasdjghak";
            itemStatuses.Add(refHermesScheduleItemStatus);

            client.SaveScheduleItemStatuses(itemStatuses);
        }

        /// <summary>
        /// Tests the SaveScheduleItemStatuses method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemStatusesFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //Create entities
            IList<HermesScheduleItemStatus> itemStatuses = new List<HermesScheduleItemStatus>();
            itemStatuses.Add(refHermesScheduleItemStatus);

            client.SaveScheduleItemStatuses(itemStatuses);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatuses method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveScheduleItemRequestStatuses()
        {
            //Create 5 entities
            IList<HermesScheduleItemRequestStatus> itemRequestStatuses = new List<HermesScheduleItemRequestStatus>();
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItemRequestStatus ent = new HermesScheduleItemRequestStatus();
                ent.Abbreviation = "abbr" + i;
                ent.Description = "someName" + i;
                ent.Id = Guid.NewGuid().ToString();

                itemRequestStatuses.Add(ent);
            }

            //Save all of them
            itemRequestStatuses = client.SaveScheduleItemRequestStatuses(itemRequestStatuses);

            //Check that all are saved correctly
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItemRequestStatus itemRequestStatus =
                    client.GetScheduleItemRequestStatus(itemRequestStatuses[i].Id);
                CompareProperties(itemRequestStatus, itemRequestStatuses[i]);
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], itemRequestStatuses[i].Id,
                    "HermesScheduleItemRequestStatus.CreatedAudit");
            }
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatuses method for failure when scheduleItemRequestStatuses is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusesFail1()
        {
            client.SaveScheduleItemRequestStatuses(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatuses method for failure
        /// when scheduleItemRequestStatuses has null element.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusesFail2()
        {
            IList<HermesScheduleItemRequestStatus> itemRequestStatuses = new List<HermesScheduleItemRequestStatus>();
            itemRequestStatuses.Add(null);
            client.SaveScheduleItemRequestStatuses(itemRequestStatuses);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatuses method for failure when one
        /// of the scheduleItemRequestStatuses fails validation
        /// because its abbreviation is more than 20 characters
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemRequestStatusesFail3()
        {
            //Create entities
            IList<HermesScheduleItemRequestStatus> itemRequestStatuses = new List<HermesScheduleItemRequestStatus>();
            refHermesScheduleItemRequestStatus.Abbreviation = "abbresfdjghsjdfkgkjfsdhjkfasgdfmhbasdjghak";
            itemRequestStatuses.Add(refHermesScheduleItemRequestStatus);

            client.SaveScheduleItemRequestStatuses(itemRequestStatuses);
        }

        /// <summary>
        /// Tests the SaveScheduleItemRequestStatuses method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemRequestStatusesFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //Create entities
            IList<HermesScheduleItemRequestStatus> itemRequestStatuses = new List<HermesScheduleItemRequestStatus>();
            itemRequestStatuses.Add(refHermesScheduleItemRequestStatus);

            client.SaveScheduleItemRequestStatuses(itemRequestStatuses);
        }

        /// <summary>
        /// Tests the SaveScheduleItems method for accuracy.
        /// </summary>
        [Test]
        public void TestSaveScheduleItems()
        {
            //Create 5 entities
            IList<HermesScheduleItem> items = new List<HermesScheduleItem>();
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItem ent = new HermesScheduleItem();
                ent.Activity = refHermesActivity;
                ent.Duration = new decimal(45.67 + i);
                ent.ExceptionFlag = i % 2 == 0 ? 'Y' : 'N';
                ent.ExpirationDate = new DateTime(2008, 1 + i, 10 + i);
                ent.Id = Guid.NewGuid().ToString();

                HermesGenericNote note = new HermesGenericNote();
                note.Description = "NoteDesc" + i;
                ent.Note = note;

                ent.ScheduleItemRequestStatus = refHermesScheduleItemRequestStatus;
                ent.ScheduleItemStatus = refHermesScheduleItemStatus;
                ent.Version = 0;
                ent.WorkDate = new DateTime(2008, 3 + i, 5 + i);
                ent.WorkDayAmount = new decimal(56.78 + i);

                items.Add(ent);
            }

            //Save all of them
            items = client.SaveScheduleItems(items);

            //Check that all are saved correctly
            for (int i = 0; i < 5; i++)
            {
                HermesScheduleItem item = client.GetScheduleItem(items[i].Id);
                CompareProperties(item, items[i]);
            }

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(5, auditRecords.Count, "Five audit record must be present.");
            for (int i = 0; i < 5; i++)
            {
                CheckAuditRecord(auditRecords[i], items[i].Id, "HermesScheduleItem.CreatedAudit");
            }

            //Check whether notes were saved correctly
            for (int i = 0; i < 5; i++)
            {
                Assert.IsNotNull(items[i].Note.Id, "Note id must be created and assigned.");
                HermesGenericNote note =
                    noteClient.GetGenericNote(items[i].Note.Id, TimeZone.CurrentTimeZone, null, null);
                Assert.IsNotNull(note, "Note must be created.");
                Assert.AreEqual("NoteDesc" + i, note.Description, "Correct description must be saved.");
            }
        }

        /// <summary>
        /// Tests the SaveScheduleItems method for failure when scheduleItems is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemsFail1()
        {
            client.SaveScheduleItems(null);
        }

        /// <summary>
        /// Tests the SaveScheduleItems method for failure
        /// when scheduleItems has null element.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemsFail2()
        {
            IList<HermesScheduleItem> items = new List<HermesScheduleItem>();
            items.Add(null);
            client.SaveScheduleItems(items);
        }

        /// <summary>
        /// Tests the SaveScheduleItems method for failure when one
        /// of the scheduleItems fails validation
        /// because its activity is not present in the database.
        /// FaultException&lt;HermesValidationFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<HermesValidationFaultException>))]
        public void TestSaveScheduleItemsFail3()
        {
            //Create entities
            IList<HermesScheduleItem> items = new List<HermesScheduleItem>();
            refHermesActivity.Id = Guid.NewGuid().ToString();
            refHermesScheduleItem.Activity = refHermesActivity;
            items.Add(refHermesScheduleItem);

            client.SaveScheduleItems(items);
        }

        /// <summary>
        /// Tests the SaveScheduleItems method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSaveScheduleItemsFail4()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            //Create entities
            IList<HermesScheduleItem> items = new List<HermesScheduleItem>();
            items.Add(refHermesScheduleItem);

            client.SaveScheduleItems(items);
        }

        /// <summary>
        /// Test the CreateScheduleItemEditCopy method for accuracy.
        /// </summary>
        [Test]
        public void TestCreateScheduleItemEditCopy()
        {
            HermesScheduleItem editCopy = client.CreateScheduleItemEditCopy(refHermesScheduleItem);

            //Check if edit copy was made correctly.
            Assert.IsNotNull(editCopy, "CreateScheduleItemEditCopy returned null.");
            Assert.AreEqual(editCopy.ScheduleItemStatus.Description, "edit copy",
                "Wrong CreateScheduleItemEditCopy implementation.");
            Assert.AreNotEqual(editCopy.Id, refHermesScheduleItem.Id,
                "Wrong CreateScheduleItemEditCopy implementation.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            Assert.AreEqual("HermesScheduleItem.CreatedEditCopy", auditRecords[0].Message, "Wrong message.");
        }

        /// <summary>
        /// Test the CreateScheduleItemEditCopy method for failure when parentItem is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemEditCopyFail1()
        {
            client.CreateScheduleItemEditCopy(null);
        }

        /// <summary>
        /// Test the CreateScheduleItemEditCopy method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemEditCopyFail2()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.CreateScheduleItemEditCopy(refHermesScheduleItem);
        }

        /// <summary>
        /// Test the CreateScheduleItemEditCopy method for failure when parentItem does not exist in database.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemEditCopyFail3()
        {
            HermesScheduleItem parent = new HermesScheduleItem();
            parent.Id = Guid.NewGuid().ToString();
            client.CreateScheduleItemEditCopy(parent);
        }

        /// <summary>
        /// Test the CreateScheduleItemEditCopy method for failure when parentItem exists in database but
        /// has the wrong status.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemEditCopyFail4()
        {
            HermesScheduleItem parent = client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.CreateScheduleItemEditCopy(parent);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for accuracy.
        /// </summary>
        [Test]
        public void TestCreateScheduleItemPublishEditCopyRelationship()
        {

            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);

            //Check if the edit copy and parent relation was made correctly.
            HermesScheduleItem editCopyFromDb = client.GetScheduleItemEditCopy(parent);
            CompareProperties(editCopyFromDb, editCopy);
            HermesScheduleItem parentFromDb = client.GetScheduleItemParentCopy(editCopy);
            CompareProperties(parentFromDb, parent);

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            Assert.AreEqual("HermesScheduleItem.CreatedPublishRelationship", auditRecords[0].Message, "Wrong message.");
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when parent is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail1()
        {
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(null, editCopy);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when editCopy is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail2()
        {
            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(parent, null);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail3()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when
        /// parent does not exist in db.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail4()
        {
            HermesScheduleItem parent = new HermesScheduleItem();
            parent.Id = Guid.NewGuid().ToString();
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when
        /// parent does exists but has wrong status.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail5()
        {
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when
        /// edit copy does not exist in db.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail6()
        {
            HermesScheduleItem editCopy = new HermesScheduleItem();
            editCopy.Id = Guid.NewGuid().ToString();
            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when
        /// edit copy does exist but has wrong status.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCreateScheduleItemPublishEditCopyRelationshipFail7()
        {
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            HermesScheduleItem parent =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            editCopy.ScheduleItemStatus = parent.ScheduleItemStatus;
            client.CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);
        }

        /// <summary>
        /// Tests the DeleteScheduleItemPublishEditCopyRelationship method for accuracy.
        /// </summary>
        [Test]
        public void TestDeleteScheduleItemPublishEditCopyRelationship()
        {
            //First create a parent and edit copy relation
            TestCreateScheduleItemPublishEditCopyRelationship();

            //Now delete
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            client.DeleteScheduleItemPublishEditCopyRelationship(editCopy);

            //Both the schedule items must exist
            Assert.IsNotNull(client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString()),
                "Both the schedule items must exist");
            Assert.IsNotNull(client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString()),
                "Both the schedule items must exist");

            //But their relation must not exist
            Assert.IsNull(client.GetScheduleItemEditCopy(
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString())),
                "Their relation must not exist");
            Assert.IsNull(client.GetScheduleItemParentCopy(
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString())),
                "Their relation must not exist");
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when editCopy is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemPublishEditCopyRelationshipFail1()
        {
            client.DeleteScheduleItemPublishEditCopyRelationship(null);
        }

        /// <summary>
        /// Test the CreateScheduleItemPublishEditCopyRelationship method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDeleteScheduleItemPublishEditCopyRelationshipFail2()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            client.DeleteScheduleItemPublishEditCopyRelationship(
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString()));
        }

        /// <summary>
        /// Test the GetScheduleItemEditCopy method for accuracy.
        /// </summary>
        [Test]
        public void TestGetScheduleItemEditCopy()
        {
            //First create relation
            TestCreateScheduleItemPublishEditCopyRelationship();

            HermesScheduleItem editCopyFromDb =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());
            HermesScheduleItem editCopy = client.GetScheduleItemEditCopy(
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString()));
            CompareProperties(editCopy, editCopyFromDb);
        }

        /// <summary>
        /// Test the GetScheduleItemEditCopy method for failure when parent is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemEditCopyFail1()
        {
            client.GetScheduleItem(null);
        }

        /// <summary>
        /// Test the GetScheduleItemEditCopy method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemEditCopyFail2()
        {
            //First create relation
            TestCreateScheduleItemPublishEditCopyRelationship();

            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            HermesScheduleItem editCopy = client.GetScheduleItemEditCopy(
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString()));
        }

        /// <summary>
        /// Test the GetScheduleItemParentCopy method for accuracy.
        /// </summary>
        [Test]
        public void TestGetScheduleItemParentCopy()
        {
            //First create relation
            TestCreateScheduleItemPublishEditCopyRelationship();

            HermesScheduleItem parentFromDb =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            HermesScheduleItem parent = client.GetScheduleItemParentCopy(
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString()));
            CompareProperties(parent, parentFromDb);
        }

        /// <summary>
        /// Test the GetScheduleItemParentCopy method for failure when parent is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemParentCopyFail1()
        {
            client.GetScheduleItem(null);
        }

        /// <summary>
        /// Test the GetScheduleItemParentCopy method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetScheduleItemParentCopyFail2()
        {
            //First create relation
            TestCreateScheduleItemPublishEditCopyRelationship();

            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            HermesScheduleItem editCopy = client.GetScheduleItemParentCopy(
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString()));
        }

        /// <summary>
        /// Tests the PublishScheduleItem method for accuracy when status of editCopy is edit copy
        /// </summary>
        [Test]
        public void TestPublishScheduleItem1()
        {
            //Create relation first
            TestCreateScheduleItemPublishEditCopyRelationship();
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666A").ToString());

            //Check the edit copy
            editCopy = client.PublishScheduleItem(editCopy);
            Assert.IsNotNull(editCopy, "Must not return null.");
            Assert.AreEqual("retired", editCopy.ScheduleItemStatus.Description, "Wrong status of edit copy.");

            //Check the published item
            HermesScheduleItem published =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());
            Assert.IsNotNull(published, "Parent must be updated.");
            Assert.AreEqual("published", published.ScheduleItemStatus.Description, "Wrong status of published item.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            Assert.AreEqual("HermesScheduleItem.Published", auditRecords[0].Message, "Wrong message.");
        }

        /// <summary>
        /// Tests the PublishScheduleItem method for accuracy when status of editCopy is not edit copy
        /// </summary>
        [Test]
        public void TestPublishScheduleItem2()
        {
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666B").ToString());

            //Check the edit copy
            editCopy = client.PublishScheduleItem(editCopy);
            Assert.IsNotNull(editCopy, "Must not return null.");
            Assert.AreEqual("published", editCopy.ScheduleItemStatus.Description, "Wrong status of edit copy.");

            //Check whether audit records were created correctly.
            IList<HermesAuditRecord> auditRecords = auditGetClient.GetAuditRecords(null);
            Assert.AreEqual(1, auditRecords.Count, "One audit record must be present.");
            Assert.AreEqual("HermesScheduleItem.Published", auditRecords[0].Message, "Wrong message.");
        }

        /// <summary>
        /// Test the PublishScheduleItem method for failure when authentication fails.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestPublishScheduleItemFail1()
        {
            // Remove session_id header
            OperationContext.Current.OutgoingMessageHeaders.RemoveAll("session_id", "session_ns");

            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("6666666666666666666666666666666B").ToString());
            editCopy = client.PublishScheduleItem(editCopy);
        }

        /// <summary>
        /// Test the PublishScheduleItem method for failure when editCopy is null.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestPublishScheduleItemFail2()
        {
            client.PublishScheduleItem(null);
        }

        /// <summary>
        /// Test the PublishScheduleItem method for failure when editCopy has status as published.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestPublishScheduleItemFail3()
        {
            HermesScheduleItem editCopy =
                client.GetScheduleItem(new Guid("66666666666666666666666666666666").ToString());

            client.PublishScheduleItem(editCopy);
        }

        /// <summary>
        /// Test the PublishScheduleItem method for failure when editCopy does not exist in database.
        /// FaultException&lt;TCFaultException&gt; is expected
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestPublishScheduleItemFail4()
        {
            HermesScheduleItem editCopy = client.GetScheduleItem(Guid.NewGuid().ToString());
            client.PublishScheduleItem(editCopy);
        }

        /// <summary>
        /// Tests that the HostUpdated method exists and simply calls it.
        /// </summary>
        [Test]
        public void TestHostUpdated()
        {
            MethodInfo methodInfo = service.GetType().GetMethod("HostUpdated",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.IsNotNull(methodInfo, "Method must exist.");
            methodInfo.Invoke(service, new object[0]);
        }

        /// <summary>
        /// General method for comparing the property values for any 2 objects of the same type.
        /// It is used for comparing 2 entities in the tests.
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

                //Strings and all value types except date
                if (!prop.PropertyType.Equals(typeof(DateTime)) && prop.PropertyType.IsValueType ||
                    prop.PropertyType.Equals(typeof(string)))
                {
                    Assert.IsTrue(object.Equals(propValue1, propValue2),
                        prop.Name + " property values are different.");
                }
                //Date
                else if (prop.PropertyType.Equals(typeof(DateTime)))
                {
                    DateTime d1 = (DateTime)propValue1;
                    DateTime d2 = (DateTime)propValue2;
                    Assert.AreEqual(d1.Date, d2.Date, prop.Name + " properties are different.");
                }
                //Reference types
                else
                {
                    Assert.IsTrue(CompareProperties(propValue1, propValue2),
                        prop.Name + " property values are different.");
                }
            }

            return true;
        }

        /// <summary>
        /// Checks an HermesAuditRecord instance for the correct entityId and message.
        /// </summary>
        /// <param name="auditRecord">The audit record to check.</param>
        /// <param name="entityId">The expected entity id.</param>
        /// <param name="message">The expected message.</param>
        private static void CheckAuditRecord(HermesAuditRecord auditRecord, string entityId, string message)
        {
            Assert.AreEqual(auditRecord.EntityId, entityId, "Wrong entityId of audit record.");
            Assert.AreEqual(auditRecord.Message, message, "Wrong message of audit record.");
        }
    }
}
