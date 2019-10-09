// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using TopCoder.Configuration;
using TopCoder.Cache;
using System.Reflection;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Static class exposing helper methods for usage by tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class UnitTestHelper
    {
        /// <summary>
        /// Compares 2 string arrays. The 2 arrays are considered equal if there is a one to one mapping from
        /// the elements of array1 to elements of array2. The order of the elements does not matter.
        /// </summary>
        /// <param name="refArray1">Array 1</param>
        /// <param name="refArray2">Array 2</param>
        /// <returns>If the arrays are equal as per criteria above.</returns>
        public static bool AreReferenceIdsEqual(string[] refArray1, string[] refArray2)
        {
            int N = refArray1.Length;

            if (N != refArray2.Length)
            {
                return false;
            }

            //Make array to check if each element in refArray2 had a one to one mapping in refArray1
            bool[] store = new bool[N];

            //Populate the store array.
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (refArray1[i] == refArray2[j] && store[j] == false)
                    {
                        store[j] = true;
                        break;
                    }
                }
            }

            //All entries in store must become true
            for (int i = 0; i < N; i++)
            {
                if (!store[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the value of the private field with given fieldName from the given object using Reflection.
        /// </summary>
        /// <param name="obj">The object in which to look for private field</param>
        /// <param name="fieldName">The name of private variable</param>
        /// <returns>The value of the private field</returns>
        public static object GetPrivateFieldValue(object obj, string fieldName)
        {
            //Get the type
            Type type = obj.GetType();

            //Get the field information
            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            //return the field's value
            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Checks the expected values of the lookedUp, Id and ReferenceIds for an entry in cache.
        /// </summary>
        /// <param name="cache">The cache instance</param>
        /// <param name="id">The expected id.</param>
        /// <param name="refIds">The expected reference ids</param>
        /// <param name="lookedUp">The expected value of lookedUp</param>
        public static void CheckSecurityDataRecord(ICache cache, string id, string[] refIds, bool lookedUp)
        {
            //Get the SecurityDataRecord object from cache for the given id
            object rec = cache[id];

            //Lookedup variable must be correct
            Assert.AreEqual(lookedUp, (bool)GetPrivateFieldValue(rec, "lookedUp"), "lookedUp variable must be correct");

            SecurityData secData = (SecurityData)GetPrivateFieldValue(rec, "securityData");

            Assert.IsTrue(AreReferenceIdsEqual(secData.ReferenceIds, refIds), "Reference ids differ.");
            Assert.AreEqual(secData.Id, id, "Id must be equal.");
        }

        /// <summary>
        /// Gets a sample configuration for creating FinancialSecurityManager instance using IConfiguration
        /// </summary>
        /// <returns>Sample configuration for creating FinancialSecurityManager instance using IConfiguration</returns>
        public static IConfiguration GetConfigObject()
        {
            //Prepare the config.
            IConfiguration config = new DefaultConfiguration("Root");

            //Add the config parameters
            config.SetSimpleAttribute("objectfactory_key", "object_of1");
            config.SetSimpleAttribute("security_id_parser_key", "object_parser1");
            config.SetAttribute("security_id_types", new string[] { SecurityIdType.CUSIP, SecurityIdType.SEDOL });
            config.SetAttribute("security_lookup_service_keys",
                new string[] { "object_customLookupService1", "object_customLookupService1" });
            config.SetSimpleAttribute("recursive_lookup", "True");
            config.SetSimpleAttribute("reference_lookup", "False");
            config.SetSimpleAttribute("security_data_cache_key", "object_SimpleCache1");
            config.SetSimpleAttribute("security_data_combiner_key", "object_defaultCombiner1");

            //IConfiguration to be passed to the ConfigurationAPIObjectFactory constructor
            IConfiguration ofConfig = new DefaultConfiguration("object_of1");

            //Definition for the DefaultSecurityIdParser
            IConfiguration parserConfig = new DefaultConfiguration("object_parser1");
            parserConfig.SetSimpleAttribute("name", "object_parser1");
            IConfiguration parserConfigTypeName = new DefaultConfiguration("type_name");
            parserConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.SecurityIdParsers.DefaultSecurityIdParser, " +
                "TopCoder.FinancialService.Utility.Test");
            parserConfig.AddChild(parserConfigTypeName);
            ofConfig.AddChild(parserConfig);

            //Definition for the CustomSecurityLookupService
            IConfiguration lookupConfig = new DefaultConfiguration("object_customLookupService1");
            lookupConfig.SetSimpleAttribute("name", "object_customLookupService1");
            IConfiguration lookupConfigTypeName = new DefaultConfiguration("type_name");
            lookupConfigTypeName.SetSimpleAttribute("value",
                "TopCoder.FinancialService.Utility.CustomSecurityLookupService, " +
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

            //Definition for the DefaultSecurityDataCombiner
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
    }
}
