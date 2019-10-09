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
    /// Test <see cref="HermesAuthorizationService"/> class, unit test.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class HermesAuthorizationServiceClientImplTest
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

        #region Accuracy Test for CheckEntity
        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for accuracy.
        ///
        /// You can configure the sessionId, sessionToken in configuration.
        /// </para>
        /// </summary>
        [Test]
        public void TestCheckEntity_Accuracy()
        {
            Assert.IsTrue(service.CheckEntity("1", "101", "SomeEntity1",
                Rights.Read), "entity1 should have read rights.");
            Assert.IsFalse(service.CheckEntity("1", "101", "SomeEntity2",
                Rights.Read), "entity2 should not have read rights.");
        }
        #endregion
        #region Failure Test for CheckEntity
        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Null_SessionId()
        {
            service.CheckEntity(null, "101", "SomeEntity1", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Empty_SessionId()
        {
            service.CheckEntity(" ", "101", "SomeEntity1", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the sessionToken is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Null_SessionToken()
        {
            service.CheckEntity("1", null, "SomeEntity1", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the entity name is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Null_EntityName()
        {
            service.CheckEntity("1", "101", null, Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the entity name is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Empty_EntityName()
        {
            service.CheckEntity("1", "101", " ", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, login failed.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_LoginFailed()
        {
            service.CheckEntity("1", "wrong", "SomeEntity1", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, exception happen during login.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_LoginException()
        {
            service.CheckEntity("Exception", "101", "SomeEntity1", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the entity can not be found.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_None_Entity()
        {
            service.CheckEntity("1", "101", "entity_none", Rights.Delete);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the rights value is invalid.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Invalid_Rights1()
        {
            service.CheckEntity("1", "101", "SomeEntity1", (Rights)0);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckEntity(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, exception happen in underlying communication.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Auth_Exception()
        {
            service.CheckEntity("1", "101", "Exception", Rights.Delete);
        }
        #endregion

        #region Accuracy Test for CheckFunction
        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for accuracy.
        ///
        /// You can configure the sessionId, sessionToken in configuration.
        /// </para>
        /// </summary>
        [Test]
        public void TestCheckFunction_Accuracy()
        {
            Assert.IsTrue(service.CheckFunction("1", "101", "Enter Competition"),
                "User can do test action.");
            Assert.IsFalse(service.CheckFunction("1", "101", "Post Components"),
                "User can not do none action.");
        }
        #endregion
        #region Failure Test for CheckFunction
        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Null_SessionId()
        {
            service.CheckFunction(null, "101", "Post Components");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionId is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Empty_SessionId()
        {
            service.CheckFunction(" ", "101", "Post Components");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the sessionToken is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Null_SessionToken()
        {
            service.CheckFunction("1", null, "Post Components");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, the function name is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Null_FunctionName()
        {
            service.CheckFunction("1", "101", null);
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string, Rights)</c>
        /// for failure.
        ///
        /// In this case, the function name is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Empty_FunctionName()
        {
            service.CheckFunction("1", "101", " ");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, login in failed.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_LoginFailed()
        {
            service.CheckFunction("1", "wrong", "Post Components");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, exception happen during login.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_LoginException()
        {
            service.CheckFunction("Exception", "101", "Post Components");
        }

        /// <summary>
        /// <para>
        /// Test Method <c>CheckFunction(string, string, string)</c>
        /// for failure.
        ///
        /// In this case, exception happen in underlying communication.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Auth_Exception()
        {
            service.CheckFunction("1", "101", "Exception");
        }
        #endregion
    }
}
