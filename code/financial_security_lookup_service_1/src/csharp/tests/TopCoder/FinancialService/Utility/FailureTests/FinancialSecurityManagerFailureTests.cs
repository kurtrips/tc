/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using TopCoder.Cache;
using NUnit.Framework;
using System.Reflection;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.FinancialService.Utility.SecurityIdParsers;
using TopCoder.FinancialService.Utility.SecurityDataCombiners;

namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Failure tests for <c>FinancialSecurityManager</c> class.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class FinancialSecurityManagerFailureTests
    {
        /// <summary>
        /// Private variable that represents the <c>DefaultSecurityDataCombiner</c> for the tests.
        /// </summary>
        private DefaultSecurityDataCombiner combiner;

        /// <summary>
        /// Private variable that represents the <c>DefaultSecurityIdParser</c> for the tests.
        /// </summary>
        private DefaultSecurityIdParser parser;

        /// <summary>
        /// Private variable that represents the <c>SecurityLookupServiceMock</c> for the tests.
        /// </summary>
        private SecurityLookupServiceMock lookupService;

        /// <summary>
        /// Private variable that represents the <c>ICache</c> for the tests.
        /// </summary>
        private ICache dataCache;

        /// <summary>
        /// Private variable that represents the <c>ISecurityLookupService dictionary</c> for the tests.
        /// </summary>
        private IDictionary<string, ISecurityLookupService> securityLookupServices;

        /// <summary>
        /// Private variable that represents the <c>FinancialSecurityManager</c> for the tests.
        /// </summary>
        private FinancialSecurityManager instance;

        /// <summary>
        /// <para>Sets up test environment.</para>
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            parser = new DefaultSecurityIdParser();
            combiner = new DefaultSecurityDataCombiner();
            lookupService = new SecurityLookupServiceMock();
            dataCache = new SimpleCache(3, new FIFOCacheEvictionStrategy(), TimeSpan.FromSeconds(10));
            securityLookupServices = new Dictionary<string, ISecurityLookupService>();
            securityLookupServices[SecurityIdType.CUSIP] = lookupService;
            securityLookupServices[SecurityIdType.ISIN] = lookupService;
            securityLookupServices[SecurityIdType.SEDOL] = lookupService;
            securityLookupServices[SecurityIdType.SymbolTicker] = lookupService;
            instance = new FinancialSecurityManager(securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityLookupServices. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_1_Null_securityLookupServices()
        {
            new FinancialSecurityManager(null, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with empty
        /// securityLookupServices. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_1_empty_securityLookupServices()
        {
            securityLookupServices = new Dictionary<string, ISecurityLookupService>();
            new FinancialSecurityManager(securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with empty
        /// securityLookupServices key. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_1_empty_key_securityLookupServices()
        {
            securityLookupServices["         "] = lookupService;
            new FinancialSecurityManager(securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityLookupServices value. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_1_null_value_securityLookupServices()
        {
            securityLookupServices[SecurityIdType.ISIN] = null;
            new FinancialSecurityManager(securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityDataCombiner. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_1_Null_securityDataCombiner()
        {
            new FinancialSecurityManager(securityLookupServices, null, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityDataCache. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_1_Null_securityDataCache()
        {
            new FinancialSecurityManager(securityLookupServices, combiner, true, true, null);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityIdParser. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_Null_ssecurityIdParser()
        {
            new FinancialSecurityManager(null, securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with empty
        /// securityLookupServices. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_empty_securityLookupServices()
        {
            securityLookupServices = new Dictionary<string, ISecurityLookupService>();
            new FinancialSecurityManager(parser, securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityLookupServices. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_Null_securityLookupServices()
        {
            new FinancialSecurityManager(parser, null, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with empty
        /// securityLookupServices key. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_empty_key_securityLookupServices()
        {
            securityLookupServices["         "] = lookupService;
            new FinancialSecurityManager(parser, securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityLookupServices value. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_null_value_securityLookupServices()
        {
            securityLookupServices[SecurityIdType.ISIN] = null;
            new FinancialSecurityManager(parser, securityLookupServices, combiner, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityDataCombiner. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_Null_securityDataCombiner()
        {
            new FinancialSecurityManager(parser, securityLookupServices, null, true, true, dataCache);
        }

        /// <summary>
        /// Tests the failure of the <c>FinancialSecurityManager(...)</c> constructor with null
        /// securityDataCache. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TesFinancialSecurityManager_2_Null_securityDataCache()
        {
            new FinancialSecurityManager(parser, securityLookupServices, combiner, true, true, null);
        }

        /// <summary>
        /// Tests the failure of the <c>Parse(string securityId)</c> method with null securityId.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        public void TestParse_Null_securityId()
        {
            try
            {
                instance.Parse(null);
                Assert.Fail("It should throw exception.");
            }
            catch(Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException);
            }
        }

        /// <summary>
        /// Tests the failure of the <c>Parse(string securityId)</c> method with empty secondSecurityData.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        public void TestParse_empty_securityId()
        {
            try
            {
                instance.Parse("   ");
                Assert.Fail("It should throw exception.");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is SelfDocumentingException);
            }
        }

        /// <summary>
        /// Tests the failure of the <c>Lookup(string securityId)</c> method with null securityId.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLookup_Null_securityId()
        {
            instance.Lookup((string)null);
        }

        /// <summary>
        /// Tests the failure of the <c>Lookup(string securityId)</c> method with empty secondSecurityData.
        /// An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLookup_empty_securityId()
        {
            instance.Lookup("   ");
        }

        /// <summary>
        /// Tests the failure of the <c>Lookup(SecurityIdDetails securityIdDetails)</c> method with null
        /// securityIdDetails. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLookup_Null_securityIdDetails()
        {
            instance.Lookup((SecurityIdDetails)null);
        }

        /// <summary>
        /// Tests the failure of the <c>Lookup(SecurityIdDetails securityIdDetails)</c> method If there is no
        /// corresponding ISecurityLookupService defined for the specific security identifier type. An
        /// <c>NoSuchSecurityLookupServiceException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchSecurityLookupServiceException))]
        public void TestLookup_invalid_securityIdDetails()
        {
            SecurityIdDetails detail = new SecurityIdDetails("id", "type");
            SecurityData result = instance.Lookup(detail);
        }

        /// <summary>
        /// Tests the failure of the <c>ConvertFromCUSIPToISIN(string cusipSecurityId)</c> method with null
        /// cusipSecurityId. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestConvertFromCUSIPToISIN_Null_securityId()
        {
            instance.ConvertFromCUSIPToISIN((string)null);
        }

        /// <summary>
        /// Tests the failure of the <c>ConvertFromCUSIPToISIN(string cusipSecurityId)</c> method with empty
        /// cusipSecurityId. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestConvertFromCUSIPToISIN_empty_securityId()
        {
            instance.ConvertFromCUSIPToISIN("   ");
        }

        /// <summary>
        /// Tests the failure of the <c>ConvertFromISINToCUSIP(string isinSecurityId)</c> method with null
        /// isinSecurityId. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestConvertFromISINToCUSIP_Null_isinSecurityId()
        {
            instance.ConvertFromISINToCUSIP((string)null);
        }

        /// <summary>
        /// Tests the failure of the <c>ConvertFromISINToCUSIP(string isinSecurityId)</c> method with empty
        /// isinSecurityId. An <c>SelfDocumentingException</c> is expected to be thrown.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestConvertFromISINToCUSIP_empty_isinSecurityId()
        {
            instance.ConvertFromISINToCUSIP("   ");
        }
    }
}