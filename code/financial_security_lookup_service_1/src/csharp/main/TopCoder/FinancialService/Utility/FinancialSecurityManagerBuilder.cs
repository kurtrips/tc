// FinancialSecurityManagerBuilder.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.Configuration;
using TopCoder.Configuration;
using TopCoder.Util.ObjectFactory;
using TopCoder.Cache;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> This class creates the FinancialSecurityManager using the passed-in IConfiguration object. It uses the
    /// Configuration API and Object Factory TCS components to create the FinancialSecurityManager object properly.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>This class is stateless and thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class FinancialSecurityManagerBuilder
    {
        /// <summary>
        /// <para>Creates a new FinancialSecurityManagerBuilder instance.</para>
        /// </summary>
        private FinancialSecurityManagerBuilder()
        {
        }

        /// <summary>
        /// <para>Build the FinancialSecurityManager object using the given IConfiguration object.</para>
        /// </summary>
        ///
        /// <param name="configuration">the IConfiguration to load configuration.</param>
        /// <returns>the created FinancialSecurityManager object.</returns>
        ///
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if the given argument is null.
        /// Wraps ConfigurationErrorsException if the configured value is invalid, or any required
        /// property is missing.
        /// </exception>
        public static FinancialSecurityManager BuildFinancialSecurityManager(IConfiguration configuration)
        {
            //Declare local variables
            ISecurityIdParser securityIdParser = null;
            ConfigurationAPIObjectFactory of = null;
            string ofDefinitionsKey = null;
            string securityIdParserKey = null;
            string[] securityIdTypes = null;
            string[] securityLookupServiceKeys = null;
            IDictionary<string, ISecurityLookupService> securityLookupServices = null;
            bool recursiveLookup = false, referenceLookup = false;
            ICache cache = null;
            string securityDataCacheKey = null;
            ISecurityDataCombiner combiner = null;
            string securityDataCombinerKey = null;

            try
            {
                Helper.ValidateNotNull(configuration, "configuration");

                //Get the key of the nested object definition to use
                ofDefinitionsKey = GetConfigValue(configuration, "objectfactory_key", true, false);

                try
                {
                    //Create the Obejct Factory instance using the nested definition to use.
                    of = new ConfigurationAPIObjectFactory(configuration[ofDefinitionsKey]);
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(
                        "Unable to create ConfigurationAPIObjectFactory instance from object factory", e);
                }

                //Create ISecurityIdParser instance if needed.
                securityIdParserKey = GetConfigValue(configuration, "security_id_parser_key", false, false);
                if (securityIdParserKey != null)
                {
                    try
                    {
                        securityIdParser = (ISecurityIdParser)of.CreateDefinedObject(securityIdParserKey);
                    }
                    catch (Exception e)
                    {
                        throw new ConfigurationErrorsException(
                            "Unable to create ISecurityIdParser instance from object factory", e);
                    }
                }

                //Get the security types and the keys for creating their respective ISecurityLookupService instances
                securityIdTypes = GetConfigArrayValue(
                    configuration, "security_id_types", true, false);
                securityLookupServiceKeys = GetConfigArrayValue(
                    configuration, "security_lookup_service_keys", true, false);

                //Length of both must be same
                if (securityIdTypes.Length != securityLookupServiceKeys.Length)
                {
                    throw new ConfigurationErrorsException(
                        "Arrays for security_id_types and security_lookup_service_keys must be of same length.");
                }

                //Generate dictionary of security types to lookup service mappings.
                securityLookupServices = new Dictionary<string, ISecurityLookupService>();
                for (int i = 0; i < securityIdTypes.Length; i++)
                {
                    //If any keys are empty, throw exception
                    if (securityIdTypes[i].Trim().Equals(String.Empty)
                        || securityLookupServiceKeys[i].Trim().Equals(String.Empty))
                    {
                        throw new ConfigurationErrorsException("Arrays for security_id_types and " +
                            "security_lookup_service_keys must not contain empty elements");
                    }
                    else
                    {
                        try
                        {
                            //Create the lookup service instance from ObjectFactory
                            ISecurityLookupService lookupService = (ISecurityLookupService)
                                of.CreateDefinedObject(securityLookupServiceKeys[i]);

                            //Add type to lookup mapping to dictionary
                            securityLookupServices[securityIdTypes[i]] = lookupService;
                        }
                        catch (Exception e)
                        {
                            throw new ConfigurationErrorsException(
                                "Unable to create ISecurityLookupService instance from object factory", e);
                        }
                    }
                }

                //Set recursive_lookup and referenceLookup
                try
                {
                    recursiveLookup = bool.Parse(GetConfigValue(configuration, "recursive_lookup", true, false));
                    referenceLookup = bool.Parse(GetConfigValue(configuration, "reference_lookup", true, false));
                }
                catch (FormatException fe)
                {
                    throw new ConfigurationErrorsException("Unable to parse value to boolean.", fe);
                }

                //Load cache
                securityDataCacheKey = GetConfigValue(configuration, "security_data_cache_key", true, false);
                try
                {
                    cache = (ICache)of.CreateDefinedObject(securityDataCacheKey);
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(
                        "Unable to create ICache instance from object factory", e);
                }

                //Load data combiner
                securityDataCombinerKey = GetConfigValue(configuration, "security_data_combiner_key", true, false);
                try
                {
                    combiner = (ISecurityDataCombiner)of.CreateDefinedObject(securityDataCombinerKey);
                }
                catch (Exception e)
                {
                    throw new ConfigurationErrorsException(
                        "Unable to create ISecurityDataCombiner instance from object factory", e);
                }

                //Create and return FinancialSecurityManager instance
                if (securityIdParser == null)
                {
                    return new FinancialSecurityManager(
                        securityLookupServices, combiner, recursiveLookup, referenceLookup, cache);
                }
                else
                {
                    return new FinancialSecurityManager(
                        securityIdParser, securityLookupServices, combiner, recursiveLookup, referenceLookup, cache);
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e,
                    "Unable to build FinancialSecurityManager instance from configuration.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManagerBuilder.BuildFinancialSecurityManager",
                    new string[0], new object[0],
                    new string[] { "configuration" }, new object[] { configuration },
                    new string[] { "securityIdParser", "of", "of_definitions_key", "security_id_parser_key",
                        "security_id_types", "security_lookup_service_keys", "securityLookupServices",
                        "recursiveLookup", "referenceLookup", "cache", "securityDataCacheKey",
                        "combiner", "securityDataCombinerKey" },
                    new object[] { securityIdParser, of, ofDefinitionsKey, securityIdParserKey, securityIdTypes,
                        securityLookupServiceKeys, securityLookupServices, recursiveLookup, referenceLookup, cache,
                        securityDataCacheKey, combiner, securityDataCombinerKey });

            }
        }

        /// <summary>
        /// Get the value of a simple attribute of type string from an IConfiguration instance.
        /// </summary>
        ///
        /// <param name="config">The IConfiguration instance</param>
        /// <param name="childName">The name of simple attribute</param>
        /// <param name="isRequired">Is the value required or not</param>
        /// <param name="isEmptyAllowed">Can the value be empty or not.</param>
        /// <returns>the value of a simple attribute of type string from an IConfiguration instance.</returns>
        ///
        /// <exception cref="ConfigurationErrorsException">
        /// If the value is required and not present or if the value cannot be empty but is empty.
        /// </exception>
        private static string GetConfigValue(IConfiguration config, string childName, bool isRequired,
            bool isEmptyAllowed)
        {
            //Get value from config
            string ret = config.GetSimpleAttribute(childName) as string;

            //If required and not present, throw exception
            if (isRequired && ret == null)
            {
                throw new ConfigurationErrorsException("Required property is missing from configuration: " + childName);
            }

            //If empty is not allowed and empty, throw exception
            if (ret != null && !isEmptyAllowed && ret.Trim().Equals(String.Empty))
            {
                throw new ConfigurationErrorsException("Empty value found in configuration for property: " + childName);
            }

            return ret;
        }

        /// <summary>
        /// Get the value of a attribute of type string array from an IConfiguration instance.
        /// </summary>
        ///
        /// <param name="config">The IConfiguration instance</param>
        /// <param name="childName">The name of attribute</param>
        /// <param name="isRequired">Is the value required or not</param>
        /// <param name="isEmptyAllowed">Can the value be empty array or not.</param>
        /// <returns>the value of a attribute of type string array from an IConfiguration instance.</returns>
        ///
        /// <exception cref="ConfigurationErrorsException">
        /// If the value is required and not present or if the value cannot be empty array but is empty.
        /// </exception>
        private static string[] GetConfigArrayValue(IConfiguration config, string childName, bool isRequired,
            bool isEmptyAllowed)
        {
            //Get value from config
            string[] ret = config.GetAttribute(childName) as string[];

            //If required and not present, throw exception
            if (isRequired && ret == null)
            {
                throw new ConfigurationErrorsException("Required property is missing from configuration: " + childName);
            }

            //If empty is not allowed and empty, throw exception
            if (ret != null && !isEmptyAllowed && ret.Length == 0)
            {
                throw new ConfigurationErrorsException("Empty array found in configuration for property: " + childName);
            }

            return ret;
        }

    }
}
