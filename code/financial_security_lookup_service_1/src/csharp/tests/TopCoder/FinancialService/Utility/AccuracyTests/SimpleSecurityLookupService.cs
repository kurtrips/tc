/*
 * TCS Financial Security Lookup Service Version 1.0 Accuracy Tests
 * 
 * SimpleSecurityLookupService.cs
 */
using System;

namespace TopCoder.FinancialService.Utility.AccuracyTests
{
    /// <summary>
    /// The <c>SimpleSecurityLookupService</c>' is a mock class which extends from
    /// <c>ISecurityLookupService</c> interface.
    /// It is used for accuracy tests only.
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
    public class SimpleSecurityLookupService : ISecurityLookupService
    {
        /// <summary>
        /// Creates an instance of SimpleSecurityLookupService
        /// It is an empty ctor.
        /// </summary>
        public SimpleSecurityLookupService()
        {
        }

        /// <summary>
        /// <p>Looks up the security data for the specified security ID details. The returned 
        /// <c>SecurityData</c> contains the company name and an array of cross referenced IDs.  </p>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException">if the argument is null</exception>
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data</exception>
        /// <param name='securityIdDetails'>the security id details used to lookup security data.</param>
        ///
        /// <returns>the security data</returns>
        public SecurityData Lookup(SecurityIdDetails securityIdDetails)
        {

            if (securityIdDetails.Id == "A")
            {
                // security id A references B & C
                return new SecurityData("A", "company1", new string[] { "B", "C" });
            }
            else if (securityIdDetails.Id == "B")
            {
                // security id B references D
                return new SecurityData("B", "company1", new string[] { "D" });
            }
            else if (securityIdDetails.Id == "C")
            {
                // security id C has no direct references.
                return new SecurityData("C", "company1");
            }
            else if (securityIdDetails.Id == "D")
            {
                // security id D references B
                return new SecurityData("D", "company1", new string[] { "B" });
            }
            else
            {
                // never go here.
                return null;
            }
        }
    }
}