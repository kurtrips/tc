/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * AccuracyTestsTestHelper.cs
 */
using System;
using System.Reflection;
using TopCoder.Configuration;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// Helper class for unit tests.
    /// </summary>
    ///
    /// <author>
    /// icyriver
    /// </author>
    ///
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    ///
    /// <version>
    /// 1.0
    /// </version>
    internal sealed class AccuracyTestsTestHelper
    {
        /// <summary>
        /// Private constructor to prevent instantiation.
        /// </summary>
        private AccuracyTestsTestHelper()
        {
        }

        /// <summary>
        /// Helper method used to get the private field value from the given obj with
        /// the given name.
        /// </summary>
        ///
        /// <param name="obj"> the object to get the private field value.</param>
        /// <param name="fieldName"> the private field name to get.</param>
        ///
        /// <returns>The private field value we wanted</returns>
        internal static Object getPrivateField(Object obj, string fieldName)
        {
            Type t = obj.GetType();
            object field = t.InvokeMember(fieldName, BindingFlags.GetField | BindingFlags.Instance |
                BindingFlags.NonPublic, null, obj, null);
            return field;
        }

        /// <summary>
        /// Helper method used to get the property value from the given obj with
        /// the given name.
        /// </summary>
        ///
        /// <param name="obj"> the object to get the private field value.</param>
        /// <param name="fieldName"> the property name to get.</param>
        ///
        /// <returns>The property value we wanted</returns>
        internal static Object getPropertyField(Object obj, string fieldName)
        {
            Type t = obj.GetType();
            object field = t.InvokeMember(fieldName, BindingFlags.GetProperty | 
                BindingFlags.Public | BindingFlags.Instance, null, obj, null);
            return field;
        }

        /// <summary>
        /// Helper method used to create an instance of <c>IConfiguration</c>c>. It is a sample configuration 
        /// for creating FinancialSecurityManager by <c>FinancialSecurityManagerBuilder</c>.
        /// </summary>
        /// <returns>Sample configuration for creating FinancialSecurityManager instance using IConfiguration</returns>
        internal static IConfiguration GetConfigObject()
        {
            //Prepare the config.
            IConfiguration config = new DefaultConfiguration("accTest");

            //Add the config parameters
            config.SetSimpleAttribute("objectfactory_key", "object_key");
            config.SetSimpleAttribute("security_id_parser_key", "object_idParser");
            config.SetAttribute("security_id_types", new string[] { SecurityIdType.CUSIP, SecurityIdType.ISIN });
            config.SetAttribute("security_lookup_service_keys",
                new string[] { "object_lookupService", "object_lookupService" });
            config.SetSimpleAttribute("recursive_lookup", "false");
            config.SetSimpleAttribute("reference_lookup", "true");
            config.SetSimpleAttribute("security_data_cache_key", "object_cache");
            config.SetSimpleAttribute("security_data_combiner_key", "object_combiner");

            IConfiguration ofConfig = new DefaultConfiguration("object_key");

            ofConfig.AddChild(GetIdParserConfigObject());

            ofConfig.AddChild(GetLookupServiceConfigObject());

            ofConfig.AddChild(GetCacheConfigObject());

            ofConfig.AddChild(GetCombinerConfigObject());

            config.AddChild(ofConfig);

            return config;
        }

        /// <summary>
        /// Helper method used to create an instance of <c>IConfiguration</c>c>. It is a sample configuration 
        /// for creating an instance of <c>ISecurityDataCombiner</c> by ObjectFactory.
        /// </summary>
        ///
        /// <returns>an instance of IConfiguration used to create an <c>ISecurityDataCombiner</c></returns>
        internal static IConfiguration GetCombinerConfigObject()
        {
            IConfiguration comiberConfig = new DefaultConfiguration("object_combiner");
            comiberConfig.SetSimpleAttribute("name", "object_combiner");
            IConfiguration comiberConfigTypeName = new DefaultConfiguration("type_name");
            comiberConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityDataCombiners.DefaultSecurityDataCombiner, " +
                "TopCoder.FinancialService.Utility.Test");
            comiberConfig.AddChild(comiberConfigTypeName);
            return comiberConfig;
        }

        /// <summary>
        /// Helper method used to create an instance of <c>IConfiguration</c>c>. It is a sample configuration 
        /// for creating an instance of <c>ICache</c> by ObjectFactory.
        /// </summary>
        /// <returns>an instance of IConfiguration used to create an <c>ICache</c></returns>
        internal static IConfiguration GetCacheConfigObject()
        {
            IConfiguration cacheConfig = new DefaultConfiguration("object_cache");
            cacheConfig.SetSimpleAttribute("name", "object_cache");
            IConfiguration cacheTypeName = new DefaultConfiguration("type_name");
            cacheTypeName.SetSimpleAttribute("value", "TopCoder.Cache.SimpleCache, TopCoder.Cache");
            cacheConfig.AddChild(cacheTypeName);

            return cacheConfig;
        }

        /// <summary>
        /// Helper method used to create an instance of <c>IConfiguration</c>c>. It is a sample configuration 
        /// for creating an instance of <c>ISecurityLookupService</c> by ObjectFactory.
        /// </summary>
        /// <returns>an instance of IConfiguration used to create an <c>ISecurityLookupService</c></returns>
        internal static IConfiguration GetLookupServiceConfigObject()
        {
            IConfiguration lookupServiceConfig = new DefaultConfiguration("object_lookupService");
            lookupServiceConfig.SetSimpleAttribute("name", "object_lookupService");
            IConfiguration lookupServiceConfigTypeName = new DefaultConfiguration("type_name");
            lookupServiceConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.AccuracyTests.SimpleSecurityLookupService, " +
                "TopCoder.FinancialService.Utility.Test");
            lookupServiceConfig.AddChild(lookupServiceConfigTypeName);

            return lookupServiceConfig;
        }

        /// <summary>
        /// Helper method used to create an instance of <c>IConfiguration</c>c>. It is a sample configuration 
        /// for creating an instance of <c>ISecurityIdParser</c> by ObjectFactory.
        /// </summary>
        /// <returns>an instance of IConfiguration used to create an <c>ISecurityIdParser</c></returns>
        internal static IConfiguration GetIdParserConfigObject()
        {
            IConfiguration idParserConfig = new DefaultConfiguration("object_idParser");
            idParserConfig.SetSimpleAttribute("name", "object_idParser");
            IConfiguration idParserConfigTypeName = new DefaultConfiguration("type_name");
            idParserConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityIdParsers.DefaultSecurityIdParser, " +
                "TopCoder.FinancialService.Utility.Test");
            idParserConfig.AddChild(idParserConfigTypeName);

            return idParserConfig;
        }
    }
}
