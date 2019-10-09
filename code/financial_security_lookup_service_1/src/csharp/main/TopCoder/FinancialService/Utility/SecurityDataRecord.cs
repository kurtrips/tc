// SecurityDataRecord.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
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
    internal class SecurityDataRecord
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
