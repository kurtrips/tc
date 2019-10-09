// FinancialSecurityManager.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Cache;
using TopCoder.FinancialService.Utility.SecurityIdParsers;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> This class wraps the ISecurityIdParser and ISecurityLookupService interfaces.
    /// It provides methods to parse the security id to determine its type, lookup security data result, and
    /// convert security ids between CUSIP and ISIN types.  It has a ISecurityLookupService instance configured for each
    /// security id type, and the lookup result is cached using the Simple Cache TCS component, and it is also
    /// able to combine the security data of cross-referenced security ids. It also has recursive lookup and reference
    /// lookup bool flags to indicate how the lookup is performed.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// All variables in this class are readonly.
    /// The securityDataCache's content is mutable, and acess to it is locked using the 'this' variable.
    /// The used ISecurityIdParser, ISecurityDataCombiner and ISecurityLookupService implementations are
    /// expected to be thread-safe too. So this class is thread-safe.
    /// </threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class FinancialSecurityManager
    {
        /// <summary>
        /// <para>
        /// Represents the ISecurityIdParser used to parse the security id to determine its type in Parse,
        /// Convert and Lookup methods. It is initialized in the constructor, and never changed afterwards.
        /// It must be non-null.
        /// </para>
        /// </summary>
        private readonly ISecurityIdParser securityIdParser;

        /// <summary>
        /// <para>Represents mapping from security id type to ISecurityLookupService instances.  Used in Lookup methods.
        /// The key must be non-null, non-empty string, and the value must be non-null ISecurityLookupService object.
        /// It is initialized in the constructor, and never changed afterwards.  It must be non-null, non-empty
        /// dictionary.</para>
        /// </summary>
        private readonly IDictionary<string, ISecurityLookupService> securityLookupServices;

        /// <summary>
        /// <para>Represents the bool flag indicating we should perform recursive lookup or not in Lookup methods. If
        /// recursiveLookup is true, the referenceLookup is ignored.  It is initialized in the constructor, and never
        /// changed afterwards.</para>
        /// </summary>
        private readonly bool recursiveLookup;

        /// <summary>
        /// <para>Represents the bool flag indicating we should perform reference lookup or not in Lookup methods. It is
        /// initialized in the constructor, and never changed afterwards.</para>
        /// </summary>
        private readonly bool referenceLookup;

        /// <summary>
        /// <para>Represents the cache mapping from security id to the SecurityDataRecord to cache the lookup results in
        /// Lookup methods.  The cache key must be non-null, non-empty string.  The cache value must be non-null
        /// SecurityDataRecord objects.  It is initialized in the constructor, and never changed afterwards.  It must be
        /// non-null. It can be empty.  </para>
        /// </summary>
        private readonly ICache securityDataCache;

        /// <summary>
        /// <para>Represents the ISecurityDataCombiner used to combine two security data objects into one security data
        /// object. Used in the Lookup methods. It is initialized in the constructor, and never changed afterwards. It
        /// must be non-null.</para>
        /// </summary>
        private readonly ISecurityDataCombiner securityDataCombiner;

        /// <summary>
        /// <para>Constructor with a dictionary of ISecurityLookupService instances, the recursive lookup flag and the
        /// security data cache.</para>
        /// </summary>
        ///
        /// <param name="securityLookupServices">
        /// a dictionary of security id to ISecurityLookupService instance entries.
        /// </param>
        /// <param name="securityDataCombiner">
        /// the ISecurityDataCombiner used to combine two security data objects into one security data object.
        /// </param>
        /// <param name="recursiveLookup">the bool flag indicating we should perform recursive lookup or not.</param>
        /// <param name="referenceLookup">the bool flag indicating we should perform reference lookup or not.</param>
        /// <param name="securityDataCache">
        /// the cache mapping from security id to the SecurityDataRecord to cache the lookup results in Lookup methods.
        /// </param>
        ///
        /// <exception cref="SelfDocumentingException">
        /// <para>
        /// Wraps ArgumentException: if securityLookupServices argument is empty dictionary,
        /// or any of its entry key is null/empty string, or any of its entry value is null.
        /// </para>
        /// <para>
        /// Wraps ArgumentNullException: if the securityLookupServices, securityDataCombiner,
        /// or securityDataCache argument is null.
        /// </para>
        /// </exception>
        public FinancialSecurityManager(IDictionary<string, ISecurityLookupService> securityLookupServices,
            ISecurityDataCombiner securityDataCombiner, bool recursiveLookup,
            bool referenceLookup, ICache securityDataCache):
            this(new DefaultSecurityIdParser(), securityLookupServices,securityDataCombiner,
            recursiveLookup, referenceLookup, securityDataCache)
        {
        }

        /// <summary>
        /// <para>Constructor with the security parser, a dictionary of ISecurityLookupService instances, the recursive
        /// lookup flag and the security data cache.  Stores a shallow copy the securityLookupServices argument.
        /// </para>
        /// </summary>
        ///
        /// <param name="securityIdParser">the security parser to parse the security id.</param>
        /// <param name="securityLookupServices">
        /// a dictionary of security id to ISecurityLookupService instance entries.
        /// </param>
        /// <param name="securityDataCombiner">
        /// the ISecurityDataCombiner used to combine two security data objects into one security data object.
        /// </param>
        /// <param name="recursiveLookup">the bool flag indicating we should perform recursive lookup or not</param>
        /// <param name="referenceLookup">the bool flag indicating we should perform reference lookup or no</param>
        /// <param name="securityDataCache">
        /// the cache mapping from security id to the SecurityDataRecord to cache the lookup results in Lookup methods.
        /// </param>
        ///
        /// <exception cref="SelfDocumentingException">
        /// <para>
        /// Wraps ArgumentException: if securityLookupServices argument is empty
        /// dictionary, or any of its entry key is null/empty string, or any of its entry value is null.
        /// </para>
        /// <para>
        /// Wraps ArgumentNullException: if the securityParser, securityDataCombiner, securityLookupServices,
        /// or securityDataCache argument is null.
        /// </para>
        /// </exception>
        public FinancialSecurityManager(ISecurityIdParser securityIdParser,
            IDictionary<string, ISecurityLookupService> securityLookupServices,
            ISecurityDataCombiner securityDataCombiner, bool recursiveLookup, bool referenceLookup,
            ICache securityDataCache)
        {
            try
            {
                //Validate
                Helper.ValidateNotNull(securityIdParser, "securityIdParser");
                Helper.ValidateNotNull(securityDataCombiner, "securityDataCombiner");
                Helper.ValidateNotNull(securityDataCache, "securityDataCache");
                Helper.ValidateDictionary(securityLookupServices, "securityLookupServices");

                this.securityIdParser = securityIdParser;
                this.securityDataCombiner = securityDataCombiner;
                this.securityDataCache = securityDataCache;
                this.recursiveLookup = recursiveLookup;
                this.referenceLookup = referenceLookup;

                //Shallow copy
                this.securityLookupServices = new Dictionary<string, ISecurityLookupService>(securityLookupServices);
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to create FinancialSecurityManager instance.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManager.FinancialSecurityManager",
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { this.securityIdParser, this.securityLookupServices, this.recursiveLookup,
                        this.referenceLookup, this.securityDataCache, this.securityDataCombiner },
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { securityIdParser, securityLookupServices, recursiveLookup, referenceLookup,
                        securityDataCache, securityDataCombiner },
                    new string[0], new object[0]);

            }
        }

        /// <summary>
        /// <para>Parse the security id to get its details.
        /// The result will at least contain the security identifier type.
        /// </para>
        /// </summary>
        ///
        /// <param name="securityId">the security id to parse.</param>
        /// <returns>the SecurityIdDetails object containing the security id type.</returns>
        /// 
        /// <exception cref="UnknownSecurityIdTypeException">
        /// if the type of the given security id is unknown.
        /// </exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">If security id has problems like non-alphanumeric
        /// characters etc.</exception>
        /// <exception cref="SelfDocumentingException">
        /// <para>Wraps ArgumentException: if the given argument is empty string.</para>
        /// <para>Wraps ArgumentNullException: if the given argument is null.</para>
        /// </exception>
        public SecurityIdDetails Parse(string securityId)
        {
            try
            {
                return securityIdParser.Parse(securityId);
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to parse securityId.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManager.Parse(string)",
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { securityIdParser, securityLookupServices, recursiveLookup, referenceLookup,
                        securityDataCache, securityDataCombiner },
                    new string[] { "securityId" }, new object[] { securityId },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// <para>Lookup the security data by the given security id.</para>
        /// </summary>
        ///
        /// <param name="securityId">the security id used to lookup security data.</param>
        /// <returns>the security data.</returns>
        ///
        /// <exception cref="UnknownSecurityIdTypeException">
        /// if the type of the given security id is unknown.
        /// </exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">if any other error occurs in parsing the id.</exception>
        /// <exception cref="NoSuchSecurityLookupServiceException">if there is no corresponding ISecurityLookupService
        /// defined for the specific security identifier type.</exception>
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available.</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data.</exception>
        /// <exception cref="SelfDocumentingException">
        /// <para>Wraps ArgumentException: if the given argument is empty string.</para>
        /// <para>Wraps ArgumentNullException: if the given argument is null.</para>
        /// </exception>
        public SecurityData Lookup(string securityId)
        {
            try
            {
                return Lookup(securityIdParser.Parse(securityId));
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Lookup of security failed.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManager.Lookup(string)",
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { securityIdParser, securityLookupServices, recursiveLookup, referenceLookup,
                        securityDataCache, securityDataCombiner },
                    new string[] { "securityId" }, new object[] { securityId },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// <para>Lookup the security data by the given security id details.</para>
        /// </summary>
        ///
        /// <param name="securityIdDetails">the security id details used to lookup security data.</param>
        /// <returns>the security data.</returns>
        ///
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException is argument is null.</exception>
        /// <exception cref="UnknownSecurityIdTypeException">if type of the given security id is unknown.</exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">if any other error occurs in parsing the id.</exception>
        /// <exception cref="NoSuchSecurityLookupServiceException">if there is no corresponding ISecurityLookupService
        /// defined for the specific security identifier type.</exception>
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available.</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data.</exception>
        public SecurityData Lookup(SecurityIdDetails securityIdDetails)
        {
            ISecurityLookupService lookupService = null;

            try
            {
                Helper.ValidateNotNull(securityIdDetails, "securityIdDetails");

                // get corresponding lookup service. Use TryGetValue so that KeyNotFoundException is not encountered.
                securityLookupServices.TryGetValue(securityIdDetails.Type, out lookupService);

                //No lookup service found for current type.
                if (lookupService == null)
                {
                    throw new NoSuchSecurityLookupServiceException(
                        "No lookup service exists for the security type: " + securityIdDetails.Type);
                }

                //Since cache is checked and updated many times in the same function,
                //there needs to control against race conditions.
                lock (securityDataCache)
                {
                    if (recursiveLookup)
                    {
                        return PerformRecursiveLookup(securityIdDetails, lookupService);
                    }
                    else if (referenceLookup)
                    {
                        return PerformReferenceLookup(securityIdDetails, lookupService);
                    }
                    else
                    {
                        return PerformSimpleLookup(securityIdDetails, lookupService);
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to perform lookup.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManager.Lookup(SecurityIdDetails)",
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { securityIdParser, securityLookupServices, recursiveLookup, referenceLookup,
                        securityDataCache, securityDataCombiner },
                    new string[] { "securityIdDetails" }, new object[] { securityIdDetails },
                    new string[] { "lookupService" }, new object[] { lookupService });

            }
        }

        /// <summary>
        /// <para>Convert the security id of CUSIP type to a security id of ISIN type.
        /// The country code used is 'US'</para>
        /// </summary>
        ///
        /// <param name="cusipSecurityId">the security id of CUSIP type.</param>
        /// <returns>the security id of ISIN type.</returns>
        ///
        /// <exception cref="SelfDocumentingException">
        /// <para>Wraps ArgumentException: if the given argument is empty string.</para>
        /// <para>Wraps ArgumentNullException: if the given argument is null.</para>
        /// </exception>
        /// <exception cref="UnknownSecurityIdTypeException">if type of the given security id is unknown.</exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">if any other error occurs in parsing the id.</exception>
        /// <exception cref="InvalidSecurityIdTypeException">If the given security id is not of CUSIP type.</exception>
        public string ConvertFromCUSIPToISIN(string cusipSecurityId)
        {
            SecurityIdDetails securityIdDetails = null;
            string isin = null;

            try
            {
                Helper.ValidateNotNullNotEmpty(cusipSecurityId, "cusipSecurityId");

                //Parse the securityId
                securityIdDetails = securityIdParser.Parse(cusipSecurityId);

                if (securityIdDetails.Type != SecurityIdType.CUSIP)
                {
                    throw new InvalidSecurityIdTypeException("Type of securityId is not CUSIP.");
                }

                //Append US to it.
                isin = "US" + cusipSecurityId.ToUpper();

                //Append check digit
                isin += Helper.GetCheckDigitForIsin(isin);

                return isin;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to convert CUSIP to ISIN.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManager.ConvertFromCUSIPToISIN",
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { securityIdParser, securityLookupServices, recursiveLookup, referenceLookup,
                        securityDataCache, securityDataCombiner },
                    new string[] { "cusipSecurityId" }, new object[] { cusipSecurityId },
                    new string[] { "securityIdDetails", "isin" },
                    new object[] { securityIdDetails, isin });
            }
        }

        /// <summary>
        /// <para>Convert the security id of ISIN (US only) type to a security id of CUSIP type.</para>
        /// </summary>
        ///
        /// <param name="isinSecurityId">the security id of CUSIP type.</param>
        /// <returns>the security id of CUSIP type.</returns>
        ///
        /// <exception cref="SelfDocumentingException">
        /// <para>Wraps ArgumentException: if the given argument is empty string.</para>
        /// <para>Wraps ArgumentNullException: if the given argument is null.</para>
        /// </exception>
        /// <exception cref="UnknownSecurityIdTypeException">if type of the given security id is unknown.</exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">if any other error occurs in parsing the id.</exception>
        /// <exception cref="InvalidSecurityIdTypeException">If the given security id is not ISIN (US only) type.
        /// </exception>
        public string ConvertFromISINToCUSIP(string isinSecurityId)
        {
            SecurityIdDetails securityIdDetails = null;
            string ret = null;

            try
            {
                Helper.ValidateNotNullNotEmpty(isinSecurityId, "isinSecurityId");

                //Must be parseable to type ISIN
                securityIdDetails = securityIdParser.Parse(isinSecurityId);
                if (securityIdDetails.Type != SecurityIdType.ISIN)
                {
                    throw new InvalidSecurityIdTypeException("Type of isinSecurityId is not ISIN.");
                }

                //Must start with US
                if (!isinSecurityId.ToUpper().StartsWith("US"))
                {
                    throw new InvalidSecurityIdTypeException("isinSecurityId is not a valid US ISIN.");
                }

                //Remove the country code and the check digit
                ret = isinSecurityId.Remove(isinSecurityId.Length - 1).Remove(0, 2);

                //Check if underlying CUSIP is valid
                try
                {
                    securityIdDetails = securityIdParser.Parse(ret);
                }
                catch
                {
                    throw new InvalidSecurityIdTypeException("isinSecurityId has not been derived from a valid CUSIP.");
                }

                return ret;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to convert ISIN code to CUSIP.",
                    "TopCoder.FinancialService.Utility.FinancialSecurityManager.ConvertFromISINToCUSIP",
                    new string[] { "securityIdParser", "securityLookupServices", "recursiveLookup",
                        "referenceLookup", "securityDataCache", "securityDataCombiner" },
                    new object[] { securityIdParser, securityLookupServices, recursiveLookup, referenceLookup,
                        securityDataCache, securityDataCombiner },
                    new string[] { "isinSecurityId" }, new object[] { isinSecurityId },
                    new string[] { "securityIdDetails", "ret" }, new object[] { securityIdDetails, ret });
            }
        }

        /// <summary>
        /// Performs a lookup for the givern security id if recursiveLookup and referenceLookup are both false.
        /// </summary>
        /// <param name="securityIdDetails">The details of the security id for which to perform lookup.</param>
        /// <param name="lookupService">The lookup service to use</param>
        /// <returns>the security data.</returns>
        ///
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available.</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data.</exception>
        /// <exception cref="SecurityDataCombiningException">if fail to combine the security data.</exception>
        private SecurityData PerformSimpleLookup(SecurityIdDetails securityIdDetails,
            ISecurityLookupService lookupService)
        {
            // check cache to get the securityDataRecord
            SecurityDataRecord securityDataRecord = securityDataCache[securityIdDetails.Id] as SecurityDataRecord;

            if (securityDataRecord != null)
            {
                return securityDataRecord.SecurityData;
            }
            else
            {
                //If not in cache then perform lookup
                SecurityData securityData = lookupService.Lookup(securityIdDetails);

                //Combine with itself. Combining a securityData instance with itself has the effect of
                //adding the securityID to the referenceIds
                SecurityData combinedData = securityDataCombiner.Combine(securityData, securityData);

                //Create all possible cross references
                foreach (string refId in securityData.ReferenceIds)
                {
                    //Check the reference id in cache
                    SecurityDataRecord refSecurityData = securityDataCache[refId] as SecurityDataRecord;

                    if (refSecurityData != null)
                    {
                        combinedData = securityDataCombiner.Combine(securityData, refSecurityData.SecurityData);
                    }
                }

                //Add or update cache with the cross-referenced security data
                AddUpdateCache(combinedData);

                return combinedData;
            }
        }

        /// <summary>
        /// Performs a lookup for the givern security id if recursiveLookup is false and referenceLookup is true.
        /// </summary>
        /// <param name="securityIdDetails">The details of the security id for which to perform lookup.</param>
        /// <param name="lookupService">The lookup service to use</param>
        /// <returns>the security data.</returns>
        ///
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available.</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data.</exception>
        /// <exception cref="SecurityDataCombiningException">if fail to combine the security data.</exception>
        private SecurityData PerformReferenceLookup(SecurityIdDetails securityIdDetails,
            ISecurityLookupService lookupService)
        {
            // check cache to get the securityDataRecord
            SecurityDataRecord securityDataRecord = securityDataCache[securityIdDetails.Id] as SecurityDataRecord;

            if (securityDataRecord != null && securityDataRecord.IsLookedUp)
            {
                return securityDataRecord.SecurityData;
            }
            else if (securityDataRecord == null)
            {
                return PerformSimpleLookup(securityIdDetails, lookupService);
            }
            else
            {
                //Get the security data for lookupService
                SecurityData securityData = lookupService.Lookup(securityIdDetails);

                //Combine the record in cache with the one returned.
                securityData = securityDataCombiner.Combine(securityData, securityDataRecord.SecurityData);

                //Add or update cache with the cross-referenced security data
                AddUpdateCache(securityData);

                return securityData;
            }
        }

        /// <summary>
        /// Performs a lookup for the givern security id if recursiveLookup is true.
        /// </summary>
        /// <param name="securityIdDetails">The details of the security id for which to perform lookup.</param>
        /// <param name="lookupService">The lookup service to use</param>
        /// <returns>the security data.</returns>
        ///
        /// <exception cref="UnknownSecurityIdTypeException">
        /// if the type of the given security id is unknown.
        /// </exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">if any other error occurs in parsing the id.</exception>
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available.</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data.</exception>
        /// <exception cref="SecurityDataCombiningException">if fail to combine the security data.</exception>
        private SecurityData PerformRecursiveLookup(SecurityIdDetails securityIdDetails,
            ISecurityLookupService lookupService)
        {
            // check cache to get the securityDataRecord
            SecurityDataRecord securityDataRecord = securityDataCache[securityIdDetails.Id] as SecurityDataRecord;

            if (securityDataRecord != null)
            {
                return securityDataRecord.SecurityData;
            }
            else
            {
                //Get the security data for lookupService
                SecurityData securityData = lookupService.Lookup(securityIdDetails);

                //Add current id to cache. This prevents from infinite recursion due to cyclic references.
                securityDataCache[securityIdDetails.Id] = new SecurityDataRecord(securityData, true);

                //Create all possible cross-references and store in combinedData
                SecurityData combinedData = securityData;
                foreach (string refId in securityData.ReferenceIds)
                {
                    //Check the reference id in cache
                    SecurityDataRecord refRecord = securityDataCache[refId] as SecurityDataRecord;

                    SecurityData refData;
                    //if not present in cache, the load from lookup
                    if (refRecord == null)
                    {
                        //Get details of the reference id
                        SecurityIdDetails refDetails = securityIdParser.Parse(refId);

                        //get lookup service for the refId
                        ISecurityLookupService refLookupService = null;
                        securityLookupServices.TryGetValue(refDetails.Type, out refLookupService);

                        //No lookup service found for current type.
                        if (refLookupService == null)
                        {
                            throw new NoSuchSecurityLookupServiceException(
                                "No lookup service exists for the security type: " + refDetails.Type);
                        }

                        refData = PerformRecursiveLookup(securityIdParser.Parse(refId), refLookupService);
                    }
                    else
                    {
                        refData = refRecord.SecurityData;
                    }

                    //Combine the current combined data with
                    combinedData = securityDataCombiner.Combine(combinedData, refData);
                }

                //All cross-refernced ids must now point to the same combinedData instance
                AddUpdateCache(combinedData);

                return combinedData;
            }
        }

        /// <summary>
        /// Adds all the reference ids of the combinedSecurityData to the cache.
        /// </summary>
        /// <param name="combinedSecurityData">The securityData to add to cache</param>
        private void AddUpdateCache(SecurityData combinedSecurityData)
        {
            //Add/update all combined references to cache.
            foreach (string refId in combinedSecurityData.ReferenceIds)
            {
                //Check the reference id in cache
                SecurityDataRecord refSecurityData = securityDataCache[refId] as SecurityDataRecord;

                //Update the referenceId in combinedSecurityData
                SecurityData newSecurityData = new SecurityData(refId, combinedSecurityData.CompanyName,
                    combinedSecurityData.ReferenceIds);

                //update cache for current id. Set looked up to true
                if (refId == combinedSecurityData.Id)
                {
                    securityDataCache[refId] = new SecurityDataRecord(newSecurityData, true);
                }
                //If the reference id is already in cache then update the cache
                else if (refSecurityData != null)
                {
                    securityDataCache[refId] = new SecurityDataRecord(newSecurityData, refSecurityData.IsLookedUp);
                }
                //if not in cache and not looked up yet.
                else if (refId != combinedSecurityData.Id)
                {
                    securityDataCache[refId] = new SecurityDataRecord(newSecurityData, false);
                }
            }
        }

        /// <summary>
        /// <para>This is an internal class of FinancialSecurityManager class. It's stored as the cache entry value in the
        /// FinancialSecurityManager class.  It contains a reference to the SecurityData instance, and a lookedUp bool flag
        /// to indicate the security data is looked up directly from ISecurityLookupService or not.</para>
        /// </summary>
        /// <threadsafety>This class is immutable, and the SecurityData class is also thread-safe,
        /// so this class is thread-safe. </threadsafety>
        ///
        /// <author>Standlove</author>
        /// <author>TCSDEVELOPER</author>
        /// <version>1.0</version>
        /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
        private class SecurityDataRecord
        {
            /// <summary>
            /// <para>Represents the bool flag indicating the security data is looked up from the ISecurityLookupService
            /// directly or not. It is initialized in the constructor, and never changed afterwards.  It has
            /// property-getter to access it.</para>
            /// </summary>
            private readonly bool lookedUp;

            /// <summary>
            /// <para>Represents the referenced SecurityData instance. It is initialized in the constructor,
            /// and never changed afterwards. It has property-getter to access it. It must be non-null.  </para>
            /// </summary>
            private readonly SecurityData securityData;

            /// <summary>
            /// <para>Represents the property getter for the lookedUp.</para>
            /// </summary>
            /// <value>Represents the bool flag indicating the security data is looked up from the ISecurityLookupService
            /// directly or not.</value>
            public bool IsLookedUp
            {
                get
                {
                    return lookedUp;
                }
            }

            /// <summary>
            /// <para>Represents the property getter for the securityData</para>
            /// </summary>
            /// <value>Represents the referenced SecurityData instance.</value>
            public SecurityData SecurityData
            {
                get
                {
                    return securityData;
                }
            }

            /// <summary>
            /// <para>Constructor with the SecurityData instance and the looked-up flag.</para>
            /// </summary>
            ///
            /// <param name="securityData">the SecurityData instance to hold.</param>
            /// <param name="lookedUp">
            /// indicating the security data is looked up from the ISecurityLookupService directly or not.
            /// </param>
            /// <exception cref="ArgumentNullException">if securityData is null.</exception>
            public SecurityDataRecord(SecurityData securityData, bool lookedUp)
            {
                //Validate
                Helper.ValidateNotNull(securityData, "securityData");

                //Assign
                this.securityData = securityData;
                this.lookedUp = lookedUp;
            }
        }
    }
}
