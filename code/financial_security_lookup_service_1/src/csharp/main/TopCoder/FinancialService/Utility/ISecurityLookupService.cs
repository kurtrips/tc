// ISecurityLookupService.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> This interface defines the contract to lookup the security data by the given security id details. The
    /// returned SecurityData contains the company name for now and an array of cross referenced ids.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>Implementations should be thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ISecurityLookupService
    {
        /// <summary>
        /// <para>Lookup the security data by the given security id details.
        /// The returned SecurityData contains the company name for now and an array of cross referenced ids.
        /// </para>
        /// </summary>
        ///
        /// <param name="securityIdDetails">the security id details used to lookup security data.</param>
        /// <returns>the security data to return.</returns>
        /// <exception cref="ArgumentNullException">if the argument is null.</exception>
        /// <exception cref="ServiceNotAvailableException">if the lookup service is not available.</exception>
        /// <exception cref="SecurityLookupException">if any error occurs when looking up the security data.</exception>
        SecurityData Lookup(SecurityIdDetails securityIdDetails);
    }
}
