// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Configuration;
using System.Collections.Generic;
using TopCoder.Configuration;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the FinancialSecurityManagerBuilder class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class FinancialSecurityManagerBuilderTests
    {
        /// <summary>
        /// The IConfiguration to use fro building FinancialSecurityManager instance.
        /// </summary>
        IConfiguration config;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Prepare the config.
            config = UnitTestHelper.GetConfigObject();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            config = null;
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method.
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManager()
        {
            FinancialSecurityManager fsm =
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);

            //Check if objet has been created correctly.
            Assert.IsNotNull(UnitTestHelper.GetPrivateFieldValue(fsm, "securityIdParser"),
                "Incorrect BuildFinancialSecurityManager implementation.");
            Assert.AreEqual((UnitTestHelper.GetPrivateFieldValue(fsm, "securityLookupServices")
                as Dictionary<string, ISecurityLookupService>).Count, 2,
                "Incorrect BuildFinancialSecurityManager implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "recursiveLookup"), true,
                "Incorrect BuildFinancialSecurityManager implementation.");
            Assert.AreEqual(UnitTestHelper.GetPrivateFieldValue(fsm, "referenceLookup"), false,
                "Incorrect BuildFinancialSecurityManager implementation.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCache"),
                "Incorrect BuildFinancialSecurityManager implementation.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateFieldValue(fsm, "securityDataCombiner"),
                "Incorrect BuildFinancialSecurityManager implementation.");
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method when a required property is missing.
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// SelfDocumentingException is expected with inner exception as ConfigurationErrorsException
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManagerFail1()
        {
            try
            {
                config.RemoveAttribute("security_data_cache_key");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "ConfigurationErrorsException is expected.");
            }
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method when a required property has empty value.
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// SelfDocumentingException is expected with inner exception as ConfigurationErrorsException
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManagerFail2()
        {
            try
            {
                config.SetSimpleAttribute("objectfactory_key", "       ");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "ConfigurationErrorsException is expected.");
            }
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method when a required property has invalid value.
        /// for recursive_lookup lookup key has invalid boolean value
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// SelfDocumentingException is expected with inner exception as ConfigurationErrorsException
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManagerFail3()
        {
            try
            {
                config.SetSimpleAttribute("recursive_lookup", "ColdTurkey");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "ConfigurationErrorsException is expected.");
            }
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method when security_lookup_service_keys has empty keys.
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// SelfDocumentingException is expected with inner exception as ConfigurationErrorsException
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManagerFail4()
        {
            try
            {
                config.SetAttribute("security_lookup_service_keys", new string[] { "  ", "ColdTurkey" });
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "ConfigurationErrorsException is expected.");
            }
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method when security_lookup_service_keys
        /// and security_lookup_service_keys arrays have different lengths
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// SelfDocumentingException is expected with inner exception as ConfigurationErrorsException
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManagerFail5()
        {
            try
            {
                config.SetAttribute("security_id_types", new string[] { SecurityIdType.CUSIP, SecurityIdType.SEDOL });
                config.SetAttribute("security_lookup_service_keys",
                    new string[] { "object_customLookupService1",
                        "object_customLookupService1", "object_customLookupService1" });
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(config);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "ConfigurationErrorsException is expected.");
            }
        }

        /// <summary>
        /// Tests the BuildFinancialSecurityManager method when config parameter is null.
        /// FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        /// SelfDocumentingException is expected with inner exception as ArgumentNullException
        /// </summary>
        [Test]
        public void TestBuildFinancialSecurityManagerFail6()
        {
            try
            {
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException, "SelfDocumentingException is expected.");
                Assert.IsTrue(e.InnerException is ArgumentNullException,
                    "ArgumentNullException is expected.");
            }
        }
    }
}
