/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Hermes.Services.Security.Authorization.TopCoder;
using NUnit.Framework;
using TopCoder.Services.WCF;
using TopCoder.Util.ExceptionManager.SDE;
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
    public class HermesAuthorizationServiceClientTest
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
        }

        /// <summary>
        /// <para>
        /// Clean the environment.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            TestHelper.ClearConfig();
        }

        #region Constructor Accuracy Test
        /// <summary>
        /// <para>
        /// Test <c>GetClient()</c> for Accuracy
        /// </para>
        /// </summary>
        [Test]
        public void TestGetClient_Accuracy()
        {
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient();
            ValidateFields(client);
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient()</c> for Accuracy
        /// </para>
        /// </summary>
        [Test]
        public void TestGetClient_Without_ObjectFactory()
        {
            TestHelper.LoadConfig(
                "HermesAuthentiationServiceClientNoObjectFactory.xml");
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient();
            ValidateFields(client);
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient(string)</c> for Accuracy
        /// </para>
        /// </summary>
        [Test]
        public void TestGetClientString_Accuracy()
        {
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient(
                "Hermes.Services.Security.Authorization.Client." +
                "HermesAuthorizationServiceClient");
            ValidateFields(client);
        }

        #endregion

        #region Constructor Failure Test
        /// <summary>
        /// <para>
        /// Test <c>GetClient(string)</c> for Failure.
        ///
        /// In this case, the param is null.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestGetClientString_Null_Namespace()
        {
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient(null);
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient(string)</c> for Failure.
        ///
        /// In this case, the param is empty.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestGetClientString_Empty_Namespace()
        {
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient("  ");
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient()</c> for Failure.
        ///
        /// In this case, the binding is not set int configuration.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestGetClientString_WithoutBinding()
        {
            TestHelper.LoadConfig(
                "HermesAuthentiationServiceClientNoBinding.xml");
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient();
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient()</c> for Failure.
        ///
        /// In this case, the binding is invalid.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestGetClientString_WrongBinding()
        {
            TestHelper.LoadConfig(
                "HermesAuthentiationServiceClientInvalidBinding.xml");
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient();
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient()</c> for Failure.
        ///
        /// In this case, the remote address is not set int configuration.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestGetClientString_WithoutAddress()
        {
            TestHelper.LoadConfig(
                "HermesAuthentiationServiceClientNoAddress.xml");
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient();
        }

        /// <summary>
        /// <para>
        /// Test <c>GetClient()</c> for Failure.
        ///
        /// In this case, the binding is invalid.
        /// </para>
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestGetClientString_WrongAddress()
        {
            TestHelper.LoadConfig(
                "HermesAuthentiationServiceClientInvalidAddress.xml");
            HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient();
        }
        #endregion
        #region Tool
        /// <summary>
        /// <para>
        /// Validate fields in <see cref="HermesAuthorizationServiceClient"/>.
        /// </para>
        /// </summary>
        /// <param name="c">
        /// <see cref="HermesAuthorizationServiceClient"/> instance.
        /// </param>
        private static void ValidateFields(HermesAuthorizationServiceClient c)
        {
            Assert.IsNotNull(c, "instance should be created.");
            Assert.IsTrue(c.Endpoint.Binding is BasicHttpBinding,
                "client binding should be http binding.");
            Assert.AreEqual(c.Endpoint.Address,
                new EndpointAddress(DemoEndPointAddress),
                "client binding should be right Uri.");
        }
        #endregion
    }
}
