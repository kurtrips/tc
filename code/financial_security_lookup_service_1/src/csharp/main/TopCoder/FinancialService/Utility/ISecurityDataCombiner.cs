// ISecurityDataCombiner.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> This interface defines contract to combine two given security data into one new security data object.
    /// It is used in the FinancialSecurityManager to combine the cross-referenced security data objects. User needs to
    /// provide custom implementations to this interface if the custom SecurityData class is provided.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>Implementations should be thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ISecurityDataCombiner
    {

        /// <summary>
        /// <para>Combine two given security data into one new SecurityData object to return.
        /// The passed-in security data objects shouldn't be changed.</para>
        /// </summary>
        ///
        /// <param name="firstSecurityData">the first security data.</param>
        /// <param name="secondSeurityData">the second security data.</param>
        /// <returns>the combined SecurityData object.</returns>
        ///
        /// <exception cref="ArgumentNullException">If any argument is null.</exception>
        /// <exception cref="SecurityDataCombiningException">if fail to combine the security data.</exception>
        SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSeurityData);
    }
}
