/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using Hermes.Services.Security.Authorization.TopCoder;
using NUnit.Framework;
using Hermes.Services.Security.Authorization.Client;
using System.ServiceModel;
using System;
using TopCoder.Services.WCF;
using System.ServiceModel.Description;
namespace Hermes.Services.Security.Authorization
{
    /// <summary>
    /// <para>
    /// A demo class shows how to use the component.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// This class will use generated client class.
    /// </para>
    /// </remarks>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class Demo2
    {
        /// <summary>
        /// <para>
        /// Represents the endpoint address used for demo.
        /// </para>
        /// </summary>
        private static readonly Uri DemoEndPointAddress =
            new Uri("http://localhost:11111/demo");

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

        /// <summary>
        /// <para>
        /// A Demo shows the usage of running the component
        /// as a hosted WCF service.
        /// </para>
        /// </summary>
        [Test]
        public void testDemo()
        {
            AuthorizationClient client = new AuthorizationClient(
                new BasicHttpBinding(), new EndpointAddress(
                "http://localhost:11111/demo"));
            CheckEntity(client);
            CheckRole(client);
            CheckFunction(client);
            GetEntityRights(client);
        }

        /// <summary>
        /// <para>
        /// Show <c>CheckEntity</c> function usage.
        /// </para>
        /// </summary>
        /// <param name="client">
        /// <see cref="HermesAuthorizationServiceClient"/> instance.
        /// </param>
        private static void CheckEntity(
            AuthorizationClient client)
        {
            Assert.IsTrue(
                client.CheckEntity("1", "101", "SomeEntity1", Rights.Read),
                "entity1 should have read rights.");
            Assert.IsFalse(client.CheckEntity(
                "1", "101", "SomeEntity2", Rights.Read),
                "entity2 should not have read rights.");
            Assert.IsTrue(
                client.CheckEntity("1", "101", "SomeEntity2", Rights.Update),
                "entity2 should have update rights.");
        }

        /// <summary>
        /// <para>
        /// Show <c>CheckRole</c> function usage.
        /// </para>
        /// </summary>
        /// <param name="client">
        /// <see cref="HermesAuthorizationServiceClient"/> instance.
        /// </param>
        private static void CheckRole(AuthorizationClient client)
        {
            Assert.IsTrue(
                client.CheckRole("1", "101", "Admin"),
                "Admin Role should been granted");
            Assert.IsFalse(
                client.CheckRole("1", "101", "Employee"),
                "None Role should not been granted");
        }

        /// <summary>
        /// <para>
        /// Show <c>CheckFunction</c> function usage.
        /// </para>
        /// </summary>
        /// <param name="client">
        /// <see cref="HermesAuthorizationServiceClient"/> instance.
        /// </param>
        private static void CheckFunction(
            AuthorizationClient client)
        {
            //Check Function
            Assert.IsTrue(
                client.CheckFunction("1", "101", "Enter Competition"),
                "User can do test action.");
            Assert.IsTrue(
                client.CheckFunction("1", "101", "Enter Competition"),
                "User can do do action.");
            Assert.IsFalse(
                client.CheckFunction("1", "101", "Post Components"),
                "User can not do none action.");
        }

        /// <summary>
        /// <para>
        /// Show <c>GetEntityRights</c> function usage.
        /// </para>
        /// </summary>
        /// <param name="client">
        /// <see cref="HermesAuthorizationServiceClient"/> instance.
        /// </param>
        private static void GetEntityRights(
            AuthorizationClient client)
        {
            Rights rights = client.GetEntityRights("1", "101", "SomeEntity1");
            Assert.AreEqual(
                rights, Rights.Read | Rights.Insert, "entiry3 should have Execute rights.");
        }
    }
}
