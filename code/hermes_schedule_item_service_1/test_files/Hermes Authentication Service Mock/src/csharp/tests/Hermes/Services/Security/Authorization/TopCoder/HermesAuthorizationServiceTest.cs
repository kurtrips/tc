/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using System.ServiceModel;
using NUnit.Framework;
using TopCoder.Services.WCF;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ExceptionManager.SDE;

namespace Hermes.Services.Security.Authorization.TopCoder
{
    /// <summary>
    /// <para>The unit test for <see cref="HermesAuthorizationService"/> class.</para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HermesAuthorizationServiceTest
    {
        /// <summary>
        /// <para>The session id</para>
        /// </summary>
        private const string SessionId = "1";

        /// <summary>
        /// <para>The allowed session token.</para>
        /// </summary>
        private const string SessionTokenAllowed = "101";

        /// <summary>
        /// <para>The denied session token.</para>
        /// </summary>
        private const string SessionTokenDenied = "104";

        /// <summary>
        /// The not exist name used for testing.
        /// </summary>
        private const string NotExistName = "NOTEXISTNAME";
        
        /// <summary>
        /// The allowed role.
        /// </summary>
        private const string RoleAllowed = "Admin";

        /// <summary>
        /// The denied role.
        /// </summary>
        private const string RoleDenied = "Employee";

        /// <summary>
        /// The allowed function name.
        /// </summary>
        private const string FunctionAllowed = "Enter Competition";

        /// <summary>
        /// The denied function name.
        /// </summary>
        private const string FunctionDenied = "Post Components";

        /// <summary>
        /// The allowed entity name.
        /// </summary>
        private const string EntityAllowed = "SomeEntity1";

        /// <summary>
        /// The denied entity name.
        /// </summary>
        private const string EntityDenied = "SomeEntity3";
        
        /// <summary>
        /// <para>The HermesAuthorizationService instance used for testing.</para>
        /// </summary>
        private HermesAuthorizationService service;
        
        /// <summary>
        /// <para>Set up testing environment.</para>
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            LoadConfig();

            service =
                new HermesAuthorizationService(
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService");
        }
        
        /// <summary>
        /// <para>Clear up testing environment.</para>
        /// </summary>
        [TearDown]
        protected void TearDown()
        {
            ClearConfig();
        }
        
        /// <summary>
        /// Load configuration.
        /// </summary>
        private void LoadConfig()
        {
            ConfigManager cm = ConfigManager.GetInstance();
            cm.Clear();
            cm.LoadFile("../../test_files/TopCoderAuthConfig.xml");
        }
        
        /// <summary>
        /// Clear configuration.
        /// </summary>
        private void ClearConfig()
        {
            ConfigManager.GetInstance().Clear();
        }
        
        /// <summary>
        /// Test constructor with default namespace.
        /// </summary>
        [Test]
        public void TestConstructor_Accuracy1()
        {
            service = new HermesAuthorizationService();

            AssertInitializedFields(service);
        }

        /// <summary>
        /// Test constructor with given namespace.
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestConstructor_Null()
        {
            new HermesAuthorizationService(null);
        }
        
        /// <summary>
        /// Test constructor with given namespace.
        /// </summary>
        [Test, ExpectedException(typeof(SelfDocumentingException))]
        public void TestConstructor_Empty()
        {
            new HermesAuthorizationService("    ");
        }
        
        /// <summary>
        /// Test constructor with given namespace.
        /// </summary>
        [Test]
        public void TestConstructor_Accuracy2()
        {
            service =
                new HermesAuthorizationService(
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService");

            AssertInitializedFields(service);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Null1()
        {
            service.CheckRole(null, SessionTokenAllowed, RoleAllowed);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Null2()
        {
            service.CheckRole(SessionId, null, RoleAllowed);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Null3()
        {
            service.CheckRole(SessionId, SessionTokenAllowed, null);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Empty1()
        {
            service.CheckRole("  ", SessionTokenAllowed, RoleAllowed);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_Empty2()
        {
            service.CheckRole(SessionId, SessionTokenAllowed, "  ");
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_InvalidSessionToken()
        {
            service.CheckRole(SessionId, SessionTokenDenied, RoleAllowed);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckRole_NotExist()
        {
            service.CheckRole(SessionId, SessionTokenDenied, NotExistName);
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test]
        public void TestCheckRole_Accuracy1()
        {
            Assert.IsTrue(service.CheckRole(SessionId, SessionTokenAllowed, RoleAllowed), "The value is not expected.");
        }

        /// <summary>
        /// Test CheckRole method.
        /// </summary>
        [Test]
        public void TestCheckRole_Accuracy2()
        {
            Assert.IsFalse(service.CheckRole(SessionId, SessionTokenAllowed, RoleDenied), "The value is not expected.");
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Null1()
        {
            service.CheckFunction(null, SessionTokenAllowed, FunctionAllowed);
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Null2()
        {
            service.CheckFunction(SessionId, null, FunctionAllowed);
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Null3()
        {
            service.CheckFunction(SessionId, SessionTokenAllowed, null);
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Empty1()
        {
            service.CheckFunction("  ", SessionTokenAllowed, FunctionAllowed);
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_Empty2()
        {
            service.CheckFunction(SessionId, SessionTokenAllowed, "  ");
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_InvalidSessionToken()
        {
            service.CheckFunction(SessionId, SessionTokenDenied, FunctionAllowed);
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckFunction_NotExist()
        {
            service.CheckFunction(SessionId, SessionTokenDenied, NotExistName);
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test]
        public void TestCheckFunction_Accuracy1()
        {
            Assert.IsTrue(service.CheckFunction(SessionId, SessionTokenAllowed, FunctionAllowed), 
                "The value is not expected.");
        }

        /// <summary>
        /// Test CheckFunction method.
        /// </summary>
        [Test]
        public void TestCheckFunction_Accuracy2()
        {
            Assert.IsFalse(service.CheckFunction(SessionId, SessionTokenAllowed, FunctionDenied), 
                "The value is not expected.");
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Null1()
        {
            service.CheckEntity(null, SessionTokenAllowed, EntityAllowed, Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Null2()
        {
            service.CheckEntity(SessionId, null, EntityAllowed, Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Null3()
        {
            service.CheckEntity(SessionId, SessionTokenAllowed, null, Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Empty1()
        {
            service.CheckEntity("  ", SessionTokenAllowed, EntityAllowed, Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_Empty2()
        {
            service.CheckEntity(SessionId, SessionTokenAllowed, "  ", Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_InvalidSessionToken()
        {
            service.CheckEntity(SessionId, SessionTokenDenied, EntityAllowed, Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestCheckEntity_NotExist()
        {
            service.CheckEntity(SessionId, SessionTokenDenied, NotExistName, Rights.Delete);
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test]
        public void TestCheckEntity_Accuracy1()
        {
            Assert.IsTrue(service.CheckEntity(SessionId, SessionTokenAllowed, EntityAllowed, Rights.Read), 
                "The value is not expected.");
        }

        /// <summary>
        /// Test CheckEntity method.
        /// </summary>
        [Test]
        public void TestCheckEntity_Accuracy2()
        {
            Assert.IsFalse(service.CheckEntity(SessionId, SessionTokenAllowed, EntityDenied, Rights.Delete), 
                "The value is not expected.");
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Null1()
        {
            service.GetEntityRights(null, SessionTokenAllowed, EntityAllowed);
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Null2()
        {
            service.GetEntityRights(SessionId, null, EntityAllowed);
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Null3()
        {
            service.GetEntityRights(SessionId, SessionTokenAllowed, null);
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Empty1()
        {
            service.GetEntityRights("  ", SessionTokenAllowed, EntityAllowed);
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_Empty2()
        {
            service.GetEntityRights(SessionId, SessionTokenAllowed, "  ");
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_InvalidSessionToken()
        {
            service.GetEntityRights(SessionId, SessionTokenDenied, EntityAllowed);
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetEntityRights_NotExist()
        {
            service.GetEntityRights(SessionId, SessionTokenDenied, NotExistName);
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test]
        public void TestGetEntityRights_Accuracy1()
        {
            Assert.AreEqual(Rights.Read | Rights.Insert, 
                            service.GetEntityRights(SessionId, SessionTokenAllowed, EntityAllowed), 
                            "The value is not expected.");
        }

        /// <summary>
        /// Test GetEntityRights method.
        /// </summary>
        [Test]
        public void TestGetEntityRights_Accuracy2()
        {
            Assert.AreEqual((Rights) 0,
                            service.GetEntityRights(SessionId, SessionTokenAllowed, EntityDenied),
                            "The value is not expected.");
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_Null1()
        {
            service.GetFunctionAttributes(null, SessionTokenAllowed, FunctionAllowed);
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_Null2()
        {
            service.GetFunctionAttributes(SessionId, null, FunctionAllowed);
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_Null3()
        {
            service.GetFunctionAttributes(SessionId, SessionTokenAllowed, null);
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_Empty1()
        {
            service.GetFunctionAttributes("  ", SessionTokenAllowed, FunctionAllowed);
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_Empty2()
        {
            service.GetFunctionAttributes(SessionId, SessionTokenAllowed, "  ");
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_InvalidSessionToken()
        {
            service.GetFunctionAttributes(SessionId, SessionTokenDenied, FunctionAllowed);
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestGetFunctionAttributes_NotExist()
        {
            service.GetFunctionAttributes(SessionId, SessionTokenDenied, NotExistName);
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test]
        public void TestGetFunctionAttributes_Accuracy1()
        {
            List<KeyValuePair<string, string>> expectedAttributes = new List<KeyValuePair<string, string>>();
            expectedAttributes.Add(new KeyValuePair<string, string>("Allowed Competitiones",
                "Algorithm Competition, Component Competition"));
            expectedAttributes.Add(new KeyValuePair<string, string>("Registered Number", "10/15"));
            
            CompareList(expectedAttributes,
                        service.GetFunctionAttributes(SessionId, SessionTokenAllowed, FunctionAllowed),
                        "function attributes");
        }

        /// <summary>
        /// Test GetFunctionAttributes method.
        /// </summary>
        [Test]
        public void TestGetFunctionAttributes_Accuracy2()
        {
            List<KeyValuePair<string, string>> expectedAttributes = new List<KeyValuePair<string, string>>();
            expectedAttributes.Add(new KeyValuePair<string, string>("Components Ready",
                "ConfigurationManager, ObjectFactory"));
            expectedAttributes.Add(new KeyValuePair<string, string>("Components Developing", "Hermes Project"));

            CompareList(expectedAttributes,
                        service.GetFunctionAttributes(SessionId, SessionTokenAllowed, FunctionDenied),
                        "function attributes");
        }

        /// <summary>
        /// Test SetApplication method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSetApplication_Null1()
        {
            service.SetApplication(null);
        }

        /// <summary>
        /// Test SetApplication method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestSetApplication_Empty1()
        {
            service.SetApplication("   ");
        }
        
        /// <summary>
        /// Test SetApplication method.
        /// </summary>
        [Test]
        public void TestSetApplication()
        {
            service.SetApplication("application id");

            Assert.AreEqual("application id", GetField(service, "_AppId"), "The application id is not property set.");
        }

        /// <summary>
        /// Test Dispose method.
        /// </summary>
        [Test]
        public void TestDispose()
        {
            service.Dispose();

            Assert.AreEqual(true, GetField(service, "_IsDisposed"), "The disponsed status is not expected.");
        }

        /// <summary>
        /// Test Dispose method.
        /// </summary>
        [Test, ExpectedException(typeof(FaultException<TCFaultException>))]
        public void TestDispose2()
        {
            service.Dispose();

            service.CheckRole(SessionId, SessionTokenAllowed, RoleAllowed);
        }
        
        /// <summary>
        /// Assert the service has been initialized correctly.
        /// </summary>
        /// <param name="service">the HermesAuthorizationService instance to check</param>
        private void AssertInitializedFields(HermesAuthorizationService service)
        {
            IDictionary<KeyValuePair<string, string>, Boolean> logins =
                (IDictionary<KeyValuePair<string, string>, Boolean>)GetField(service, "_Logins");
            CompareDictionies(ConstructExpectedLogins(), logins, "_Logins");

            IDictionary<string, Boolean> roles =
                (IDictionary<string, Boolean>)GetField(service, "_Roles");
            CompareDictionies(ConstructExpectedRoles(), roles, "_Roles");
            
            IDictionary<string, Boolean> functions =
                (IDictionary<string, Boolean>)GetField(service, "_Functions");
            CompareDictionies(ConstructExpectedFunctions(), functions, "_Functions");
            
            IDictionary<string, List<KeyValuePair<string, string>>> functionsAttributes =
                (IDictionary<string, List<KeyValuePair<string, string>>>)GetField(service, "_FunctionsAttributes");
            CompareDictionies2(ConstructExpectedFunctionsAtributes(), functionsAttributes, "_FunctionsAttributes");

            IDictionary<string, Rights> entities =
                (IDictionary<string, Rights>)GetField(service, "_Entities");
            CompareDictionies(ConstructExpectedEntities(), entities, "_Entities");
        }
        
        /// <summary>
        /// Constructs the expected logins for configuration.
        /// </summary>
        /// <returns>the expected logins.</returns>
        private static IDictionary<KeyValuePair<string, string>, Boolean> ConstructExpectedLogins()
        {
            IDictionary<KeyValuePair<string, string>, Boolean> expectedLogins =
                new Dictionary<KeyValuePair<string, string>, Boolean>();
            expectedLogins.Add(new KeyValuePair<string, string>("1", "101"), true);
            expectedLogins.Add(new KeyValuePair<string, string>("1", "102"), true);
            expectedLogins.Add(new KeyValuePair<string, string>("1", "104"), false);
            expectedLogins.Add(new KeyValuePair<string, string>("1", ""), false);

            expectedLogins.Add(new KeyValuePair<string, string>("2", ""), true);
            expectedLogins.Add(new KeyValuePair<string, string>("2", "104"), true);
            expectedLogins.Add(new KeyValuePair<string, string>("2", "107"), true);

            expectedLogins.Add(new KeyValuePair<string, string>("3", "109"), false);
            expectedLogins.Add(new KeyValuePair<string, string>("3", "105"), false);
            return expectedLogins;
        }

        /// <summary>
        /// Constructs the expected roles for configuration.
        /// </summary>
        /// <returns>the expected roles.</returns>
        private static IDictionary<string, Boolean> ConstructExpectedRoles()
        {
            IDictionary<string, Boolean> expectedRoles =
                new Dictionary<string, Boolean>();
            expectedRoles.Add("Admin", true);
            expectedRoles.Add("HR", false);
            expectedRoles.Add("Employee", false);
            return expectedRoles;
        }

        /// <summary>
        /// Constructs the expected functions for configuration.
        /// </summary>
        /// <returns>the expected functions.</returns>
        private static IDictionary<string, Boolean> ConstructExpectedFunctions()
        {
            IDictionary<string, Boolean> expectedFunctions =
                new Dictionary<string, Boolean>();
            expectedFunctions.Add("Enter Competition", true);
            expectedFunctions.Add("Post Components", false);
            return expectedFunctions;
        }

        /// <summary>
        /// Constructs the expected functions attributes for configuration.
        /// </summary>
        /// <returns>the expected functions.</returns>
        private static IDictionary<string, List<KeyValuePair<string, string>>> ConstructExpectedFunctionsAtributes()
        {
            IDictionary<string, List<KeyValuePair<string, string>>> expectedFunctionsAttributes =
                new Dictionary<string, List<KeyValuePair<string, string>>>();
            
            // for Enter Competition
            List<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>();
            attributes.Add(new KeyValuePair<string, string>("Allowed Competitiones", 
                "Algorithm Competition, Component Competition"));
            attributes.Add(new KeyValuePair<string, string>("Registered Number", "10/15"));
            
            expectedFunctionsAttributes.Add("Enter Competition", attributes);

            // For Post Components
            attributes = new List<KeyValuePair<string, string>>();
            attributes.Add(new KeyValuePair<string, string>("Components Ready",
                "ConfigurationManager, ObjectFactory"));
            attributes.Add(new KeyValuePair<string, string>("Components Developing", "Hermes Project"));            
            expectedFunctionsAttributes.Add("Post Components", attributes);
            
            return expectedFunctionsAttributes;
        }

        /// <summary>
        /// Constructs the expected entities for configuration.
        /// </summary>
        /// <returns>the expected entities.</returns>
        private static IDictionary<string, Rights> ConstructExpectedEntities()
        {
            IDictionary<string, Rights> expectedEntities =
                new Dictionary<string, Rights>();
            expectedEntities.Add("SomeEntity1", Rights.Read | Rights.Insert);
            expectedEntities.Add("SomeEntity2", Rights.Update | Rights.Delete | Rights.Execute);
            expectedEntities.Add("SomeEntity3", (Rights) 0 );
            
            return expectedEntities;
        }

        /// <summary>
        /// Compare two dictionaries.
        /// </summary>
        /// <typeparam name="K">the type of key</typeparam>
        /// <typeparam name="V">the type of value</typeparam>
        /// <param name="d1">the first dictionary</param>
        /// <param name="d2">the second dictionary</param>
        /// <param name="name">the name of dictionary</param>
        private static void CompareDictionies<K, V>(IDictionary<K, V> d1, IDictionary<K, V> d2, string name)
        {
            Assert.AreEqual(d1.Count, d2.Count, String.Format("The length of {0} is not same.", name));
            foreach (KeyValuePair<K, V> pair in d1)
            {
                Assert.IsTrue(d2.ContainsKey(pair.Key),
                              String.Format("The second {0} does not contains key {1}", name, pair.Key));

                Assert.AreEqual(d2[pair.Key], pair.Value,
                              String.Format("The {0} does not equal for key {1}", name, pair.Key));
            }
        }

        /// <summary>
        /// Compare two dictionaries.
        /// </summary>
        /// <typeparam name="K">the type of key</typeparam>
        /// <typeparam name="V">the type of value</typeparam>
        /// <param name="d1">the first dictionary</param>
        /// <param name="d2">the second dictionary</param>
        /// <param name="name">the name of dictionary</param>
        private static void CompareDictionies2<K, V>(IDictionary<K, List<V>> d1, IDictionary<K, List<V>> d2, 
                                                     string name)
        {
            Assert.AreEqual(d1.Count, d2.Count, String.Format("The length of {0} is not same.", name));
            foreach (KeyValuePair<K, List<V>> pair in d1)
            {
                Assert.IsTrue(d2.ContainsKey(pair.Key),
                              String.Format("The second {0} does not contains key {1}", name, pair.Key));

                List<V> value1 = d1[pair.Key];
                List<V> value2 = d2[pair.Key];

                CompareList(value1, value2, String.Format("{0} for key {1}", name, pair.Key));
            }
        }
        
        /// <summary>
        /// Compare given 2 lists.
        /// </summary>
        /// <typeparam name="V">the type of value</typeparam>
        /// <param name="l1">the first list</param>
        /// <param name="l2">the second list</param>
        /// <param name="name">the name of value</param>
        private static void CompareList<V>(List<V> l1, List<V> l2, string name)
        {
            Assert.AreEqual(l1.Count, l2.Count,
                 String.Format("The length of list in {0} is not same.", name));
            for (int i = 0; i < l1.Count; i++)
            {
                Assert.AreEqual(l1[i], l2[i],
                                String.Format("The {0} index of list in {1} is not same.", i, name));
            } 
        }
        
        /// <summary>
        /// <para>Gets the field in the instance. The field can be private or protected.</para>
        /// </summary>
        ///
        /// <param name="instance">
        /// The instance whose field is retrieved.
        /// </param>
        /// <param name="field">
        /// The field to be retrieved.
        /// </param>
        ///
        /// <returns>
        /// The value of the field in the instance.
        /// </returns>
        [ReflectionPermission(SecurityAction.PermitOnly, Unrestricted = true)]
        internal static object GetField(object instance, string field)
        {
            return instance.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance).
                GetValue(instance);
        }
    }
}
