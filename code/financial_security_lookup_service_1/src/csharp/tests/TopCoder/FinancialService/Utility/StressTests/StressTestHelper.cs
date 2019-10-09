/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
using TopCoder.Configuration;

namespace TopCoder.FinancialService.Utility.StressTests
{
    /// <summary>
    /// <para>
    /// This helper class defines some utility method used in stress tests.
    /// </para>
    /// </summary>
    ///
    /// <author>crazypigs</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All Rights Reserved.</copyright>
    internal static class StressTestHelper
    {
        /// <summary>
        /// <para>
        /// Builds an <see cref="IConfiguration"/> instance for test.
        /// </para>
        /// </summary>
        /// <returns>An IConfiguration instance containing configuration for test.</returns>
        public static IConfiguration BuildConfiguration()
        {
            // the root node
            IConfiguration config = new DefaultConfiguration("Root");

            //Add the attributes
            config.SetSimpleAttribute("objectfactory_key", "of_config");
            config.SetSimpleAttribute("security_id_parser_key", "parser1");
            config.SetAttribute("security_id_types", new string[]
                                                         {
                                                             SecurityIdType.CUSIP, SecurityIdType.SEDOL,
                                                             SecurityIdType.ISIN, SecurityIdType.SymbolTicker
                                                         });
            config.SetAttribute("security_lookup_service_keys",
                                new string[] {"LookupService1", "LookupService1", "LookupService1", "LookupService1"});
            config.SetSimpleAttribute("recursive_lookup", "True");
            config.SetSimpleAttribute("reference_lookup", "False");
            config.SetSimpleAttribute("security_data_cache_key", "SimpleCache1");
            config.SetSimpleAttribute("security_data_combiner_key", "defaultCombiner1");

            //IConfiguration used to create ConfigurationAPIObjectFactory
            IConfiguration ofConfig = new DefaultConfiguration("of_config");

            //Definition for the DefaultSecurityIdParser
            IConfiguration parserConfig = new DefaultConfiguration("object");
            parserConfig.SetSimpleAttribute("name", "parser1");
            IConfiguration parserConfigTypeName = new DefaultConfiguration("type_name");
            parserConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityIdParsers.DefaultSecurityIdParser");
            parserConfig.AddChild(parserConfigTypeName);

            IConfiguration parserConfigAssembly = new DefaultConfiguration("assembly");
            parserConfigAssembly.SetSimpleAttribute("value", "TopCoder.FinancialService.Utility.Test.dll");
            parserConfig.AddChild(parserConfigAssembly);
            ofConfig.AddChild(parserConfig);

            //Definition for the CustomSecurityLookupService
            IConfiguration lookupConfig = new DefaultConfiguration("object1");
            lookupConfig.SetSimpleAttribute("name", "LookupService1");
            IConfiguration lookupConfigTypeName = new DefaultConfiguration("type_name");
            lookupConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.StressTests.CustomSecurityLookupService");
            lookupConfig.AddChild(lookupConfigTypeName);

            IConfiguration lookupConfigAssembly = new DefaultConfiguration("assembly");
            lookupConfigAssembly.SetSimpleAttribute("value", "TopCoder.FinancialService.Utility.Test.dll");
            lookupConfig.AddChild(lookupConfigAssembly);

            ofConfig.AddChild(lookupConfig);

            //Definition for the SimpleCache
            IConfiguration simpleCacheConfig = new DefaultConfiguration("object2");
            simpleCacheConfig.SetSimpleAttribute("name", "SimpleCache1");
            IConfiguration simpleCacheTypeName = new DefaultConfiguration("type_name");
            simpleCacheTypeName.SetSimpleAttribute("value", "TopCoder.Cache.SimpleCache");
            simpleCacheConfig.AddChild(simpleCacheTypeName);

            IConfiguration simpleCacheConfigAssembly = new DefaultConfiguration("assembly");
            simpleCacheConfigAssembly.SetSimpleAttribute("value", "TopCoder.Cache.dll");
            simpleCacheConfig.AddChild(simpleCacheConfigAssembly);

            ofConfig.AddChild(simpleCacheConfig);

            //Definition for the DefaultSecurityDataCombiner
            IConfiguration comiberConfig = new DefaultConfiguration("object3");
            comiberConfig.SetSimpleAttribute("name", "defaultCombiner1");
            IConfiguration comiberConfigTypeName = new DefaultConfiguration("type_name");
            comiberConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityDataCombiners.DefaultSecurityDataCombiner");
            comiberConfig.AddChild(comiberConfigTypeName);

            IConfiguration comiberConfigAssembly = new DefaultConfiguration("assembly");
            comiberConfigAssembly.SetSimpleAttribute("value", "TopCoder.FinancialService.Utility.Test.dll");
            comiberConfig.AddChild(comiberConfigAssembly);

            ofConfig.AddChild(comiberConfig);

            config.AddChild(ofConfig);

            return config;
        }
    }
}