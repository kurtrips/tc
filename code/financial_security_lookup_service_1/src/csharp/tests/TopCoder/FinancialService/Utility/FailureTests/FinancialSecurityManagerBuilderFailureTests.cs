/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using NUnit.Framework;
using System.Configuration;
using TopCoder.Configuration;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>FinancialSecurityManagerBuilder</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class FinancialSecurityManagerBuilderFailureTests
    {
        /// <summary>
        /// Gets a sample configuration for creating FinancialSecurityManager instance using IConfiguration.
        /// </summary>
        /// <returns>
        /// Sample configuration for creating FinancialSecurityManager instance using IConfiguration.
        /// </returns>
        private static IConfiguration GetConfigObject()
        {
            // Prepare the config.
            IConfiguration config = new DefaultConfiguration("Root");

            // Add the config parameters
            config.SetSimpleAttribute("objectfactory_key", "object_of1");
            config.SetSimpleAttribute("security_id_parser_key", "object_parser1");
            config.SetAttribute("security_id_types", new string[] { SecurityIdType.CUSIP, SecurityIdType.SEDOL });
            config.SetAttribute("security_lookup_service_keys",
                new string[] { "object_customLookupService1", "object_customLookupService1" });
            config.SetSimpleAttribute("recursive_lookup", "True");
            config.SetSimpleAttribute("reference_lookup", "False");
            config.SetSimpleAttribute("security_data_cache_key", "object_SimpleCache1");
            config.SetSimpleAttribute("security_data_combiner_key", "object_defaultCombiner1");

            // IConfiguration to be passed to the ConfigurationAPIObjectFactory constructor
            IConfiguration ofConfig = new DefaultConfiguration("object_of1");

            // Definition for the DefaultSecurityIdParser
            IConfiguration parserConfig = new DefaultConfiguration("object_parser1");
            parserConfig.SetSimpleAttribute("name", "object_parser1");
            IConfiguration parserConfigTypeName = new DefaultConfiguration("type_name");
            parserConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityIdParsers.DefaultSecurityIdParser, " +
                "TopCoder.FinancialService.Utility.Test");
            parserConfig.AddChild(parserConfigTypeName);
            ofConfig.AddChild(parserConfig);

            // Definition for the SecurityLookupServiceMock
            IConfiguration lookupConfig = new DefaultConfiguration("object_customLookupService1");
            lookupConfig.SetSimpleAttribute("name", "object_customLookupService1");
            IConfiguration lookupConfigTypeName = new DefaultConfiguration("type_name");
            lookupConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.FailureTests.SecurityLookupServiceMock, " +
                "TopCoder.FinancialService.Utility.Test");
            lookupConfig.AddChild(lookupConfigTypeName);
            ofConfig.AddChild(lookupConfig);

            //Definition for the SimpleCache
            IConfiguration simpleCacheConfig = new DefaultConfiguration("object_SimpleCache1");
            simpleCacheConfig.SetSimpleAttribute("name", "object_SimpleCache1");
            IConfiguration simpleCacheTypeName = new DefaultConfiguration("type_name");
            simpleCacheTypeName.SetSimpleAttribute("value", "TopCoder.Cache.SimpleCache, TopCoder.Cache");
            simpleCacheConfig.AddChild(simpleCacheTypeName);
            ofConfig.AddChild(simpleCacheConfig);

            // Definition for the DefaultSecurityDataCombiner
            IConfiguration comiberConfig = new DefaultConfiguration("object_defaultCombiner1");
            comiberConfig.SetSimpleAttribute("name", "object_defaultCombiner1");
            IConfiguration comiberConfigTypeName = new DefaultConfiguration("type_name");
            comiberConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityDataCombiners.DefaultSecurityDataCombiner, " +
                "TopCoder.FinancialService.Utility.Test");
            comiberConfig.AddChild(comiberConfigTypeName);
            ofConfig.AddChild(comiberConfig);

            config.AddChild(ofConfig);

            return config;
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with null configuration.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_Null_configuration()
        {
            try
            {
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(null);
            }
            catch(Exception e)
            {
                Assert.IsTrue(e.InnerException is ArgumentNullException,
                    "Inner exception should be ArgumentNullException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with null 'objectfactory_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_Null_objectfactory_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.RemoveAttribute("objectfactory_key");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with empty 'objectfactory_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_empty_objectfactory_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("objectfactory_key", "       ");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with invalid 'objectfactory_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_invalid_objectfactory_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("objectfactory_key", "invalid");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with empty 'security_id_parser_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_empty_security_id_parser_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("security_id_parser_key", "       ");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with invalid 'security_id_parser_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_invalid_security_id_parser_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("security_id_parser_key", "invalid");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with null 'security_id_types'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_null_security_id_types()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.RemoveAttribute("security_id_types");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with null 'security_lookup_service_keys'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_null_security_lookup_service_keys()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.RemoveAttribute("security_lookup_service_keys");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// when the number of 'security_lookup_service_keys' and 'security_id_types' is 0.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_zero_security_lookup_service_keys()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetAttribute("security_id_types", new string[] { });
                configuration.SetAttribute("security_lookup_service_keys", new string[] { });
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with invalid 'recursive_lookup'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_invalid_recursive_lookup()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("recursive_lookup", "invalid");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with invalid 'reference_lookup'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_invalid_reference_lookup()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("reference_lookup", "invalid");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with null 'security_data_cache_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_Null_security_data_cache_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.RemoveAttribute("security_data_cache_key");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with empty 'security_data_cache_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_empty_security_data_cache_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("security_data_cache_key", "       ");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with invalid 'security_data_cache_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_invalid_security_data_cache_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("security_data_cache_key", "invalid");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with null 'security_data_combiner_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_Null_security_data_combiner_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.RemoveAttribute("security_data_combiner_key");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with empty 'security_data_combiner_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_empty_security_data_combiner_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("security_data_combiner_key", "       ");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }

        /// <summary>
        /// Tests the failure of the <c>BuildFinancialSecurityManager(IConfiguration configuration)</c> method
        /// with invalid 'security_data_combiner_key'.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestBuildFinancialSecurityManager_invalid_security_data_combiner_key()
        {
            try
            {
                IConfiguration configuration = GetConfigObject();
                configuration.SetSimpleAttribute("security_data_combiner_key", "invalid");
                FinancialSecurityManagerBuilder.BuildFinancialSecurityManager(configuration);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException is ConfigurationErrorsException,
                    "Inner exception should be ConfigurationErrorsException");
                throw e;
            }
        }
    }
}