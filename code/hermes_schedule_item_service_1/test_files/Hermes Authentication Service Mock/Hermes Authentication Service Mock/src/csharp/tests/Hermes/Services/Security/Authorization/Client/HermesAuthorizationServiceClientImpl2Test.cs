/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Hermes.Services.Security.Authorization.TopCoder;
using NUnit.Framework;
using TopCoder.Services.WCF;
namespace Hermes.Services.Security.Authorization.Client
{
    /// <summary>
  /// Test <see cref="HermesAuthorizationService"/> class, unit test.t.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class HermesAuthorizationServiceClientImpl2Test
    {
        /// <summary>
        /// <para>
        /// Represents the endpoint address used for server.
        /// </para>
        /// </summary>
        private static readonly Uri DemoEndPointAddress
            = new Uri("http://localhost:11111/demo");

        /// <summary>
        /// <para>
        /// Represents the host.
        /// </para>
        /// </summary>
        private TCWcfServiceHost serviceHost = null;

        /// <summary>
        /// <para>
        /// <see cref="HermesAuthorizationServiceClient"/> instance for test.
        /// </para>
        /// </summary>
        private HermesAuthorizationServiceClient service;

        /// <summary>
        /// <para>
        /// Set up the environment.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            TestHelper.LoadConfig();
            serviceHost = new TCWcfServiceHost(
                typeof(HermesAuthorizationService), DemoEndPointAddress);
            BasicHttpBinding binding = new BasicHttpBinding();
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            serviceHost.Description.Behaviors.Add(behavior);
            serviceHost.AddServiceEndpoint(
                typeof(IAuthorization), binding, "");
            serviceHost.Open();
            service = HermesAuthorizationServiceClient.GetClient();
        }

        /// <summary>
        /// <para>
        /// Clean the environment.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (service != null)
            {
                service.Close();
            }
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            TestHelper.ClearConfig();
        }

        #region Accuracy Test for CheckRole
        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for accuracy.
        ///
        /// You can configure the sessionId, sessionToken in configuration.
        /// </para>
        /// </summary>
        [Test]
        public void TestCheckRole_Accuracy()
        {
            Assert.IsTrue(service.CheckRole("1", "101", "Admin"),
                "Admin Role should been granted");
            Assert.IsFalse(service.CheckRole("1", "101", "Employee"),
                "None Role should not been granted");
        }
        #endregion
        #region Failure Test for CheckRole
        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Null_SessionId()
        {
            service.CheckRole(null, "101", "Admin");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Empty_SessionId()
        {
            service.CheckRole(" ", "101", "Admin");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionToken is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Null_SessionToken()
        {
            service.CheckRole("1", null, "Admin");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the role name is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Null_RoleName()
        {
            service.CheckRole("1", "101", null);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the role name is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Empty_RoleName()
        {
            service.CheckRole("1", "101", " ");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, login in failed.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_LoginFailed()
        {
            service.CheckRole("1", "wrong", "Admin");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, exception happen during login.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_LoginException()
        {
            service.CheckRole("Exception", "101", "Admin");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckRole(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, exception happen in underlying communication.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Auth_Exception()
        {
            service.CheckRole("1", "101", "Exception");
        }
        #endregion

        #region Accuracy Test for GetEntityRights
        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string)</c>
        /// for accuracy.
        ///
        /// You can configure the sessionId, sessionToken in configuration.
        /// </para>
        /// </summary>
        [Test]
        public void TestGetEntityRights_Accuracy()
        {
            Rights rights = service.GetEntityRights("1", "101", "SomeEntity1");
            Assert.AreEqual(rights, Rights.Read | Rights.Insert,
                "entity3 should have Execute rights.");
        }
        #endregion
        #region Failure Test for GetEntityRights
        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Null_SessionId()
        {
            service.GetEntityRights(null, "101", "SomeEntity1");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Empty_SessionId()
        {
            service.GetEntityRights(" ", "101", "SomeEntity1");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionToken is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Null_SessionToken()
        {
            service.GetEntityRights("1", null, "SomeEntity1");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the entity name is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Null_RoleName()
        {
            service.GetEntityRights("1", "101", null);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the entity name is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Empty_RoleName()
        {
            service.GetEntityRights("1", "101", " ");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, login in failed.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_LoginFailed()
        {
            service.GetEntityRights("1", "wrong", "SomeEntity1");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, exception happen during login.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_LoginException()
        {
            service.GetEntityRights("Exception", "101", "SomeEntity1");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>GetEntityRights(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, exception happen in underlying communication.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Auth_Exception()
        {
            service.GetEntityRights("1", "101", "Exception");
        }
        #endregion
    }
}
