using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Hermes.Services.Security.Authorization.Client;
using Hermes.Services.Security.Authorization.TopCoder;
using NUnit.Framework;
using TopCoder.LoggingWrapper;
using TopCoder.Services.WCF;
using TopCoder.Util.ConfigurationManager;

namespace Hermes.Services.Security.Authorization
{
    [TestFixture]
    public class MyTest
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
        private HermesAuthorizationServiceClient client;
        
        /// <summary>
        /// <para>
        /// Set up the environment.
        /// </para>
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().Clear();
            ConfigManager.GetInstance().LoadFile("../../test_files/Logger.xml");
        }

        /// <summary>
        /// <para>
        /// Clean the environment.
        /// </para>
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            if (client != null)
            {
                client.Close();
            }
            if (serviceHost != null)
            {
                serviceHost.Close();
            }
            ConfigManager.GetInstance().Clear();
        }
        
        [Test]
        public void Test1()
        {
            Logger logger = LogManager.CreateLogger("TopCoder.LoggingWrapper");
            Assert.IsNotNull(logger);
        }
        
        [Test]
        public void Test2()
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



            client = HermesAuthorizationServiceClient.GetClient();
            
            Assert.IsTrue(client.CheckEntity("1", "101", "SomeEntity1",
                Rights.Read), "entity1 should have read rights.");
            Assert.IsFalse(client.CheckEntity("1", "101", "SomeEntity2",
                Rights.Read), "entity2 should not have read rights.");
        }
        
        public static void Main(params string[] args)
        {
            Console.WriteLine("hello");
        }
    }
}
