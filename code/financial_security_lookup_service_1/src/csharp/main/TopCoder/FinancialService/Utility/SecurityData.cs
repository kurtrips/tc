// SecurityData.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para>This class represents the security data corresponding to the security id. It is returned from the
    /// ISecurityLookupService after performing the lookup to the security id. It contains the security id, company
    /// name, and an array of reference ids.</para>
    /// </summary>
    ///
    /// <threadsafety>This class is immutable and thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SecurityData
    {
        /// <summary>
        /// <para>Represents the security id. It is initialized in the constructor, and never changed afterwards. It has
        /// property-getter to access it. It must be non-null, non-empty string.</para>
        /// </summary>
        private readonly string id;

        /// <summary>
        /// <para>Represents the company name of the security data. It is initialized in the constructor, and never
        /// changed afterwards. It has property-getter to access it. It must be non-null, non-empty string.</para>
        /// </summary>
        private readonly string companyName;

        /// <summary>
        /// <para>Represents the reference ids of the security data. It is initialized in the constructor, and never
        /// changed afterwards.  It has property-getter to access it. It can be empty array. It must be non-null. Each
        /// element in the array must be non-null, non-empty string.</para>
        /// </summary>
        private readonly string[] referenceIds;

        /// <summary>
        /// <para>Represents the property getter for the security id</para>
        /// </summary>
        /// <value>Represents the security id</value>
        public string Id
        {
            get
            {
                return id;
            }
        }

        /// <summary>
        /// <para>Represents the property getter for the company name.</para>
        /// </summary>
        /// <value>Represents the company name</value>
        public string CompanyName
        {
            get
            {
                return companyName;
            }
        }

        /// <summary>
        /// <para>Represents the reference ids of the security data.</para>
        /// </summary>
        /// <value>The reference ids of the security data.</value>
        public string[] ReferenceIds
        {
            get
            {
                //Make shallow copy and return
                string[] ret = new string[referenceIds.Length];
                referenceIds.CopyTo(ret, 0);
                return ret;
            }
        }


        /// <summary>
        /// <para>Constructor with the security id and company name.</para>
        /// </summary>
        ///
        /// <param name="id">the security id.</param>
        /// <param name="companyName">the company name of the security data.</param>
        ///
        /// <exception cref="ArgumentNullException">if the given arguments are null.</exception>
        /// <exception cref="ArgumentException">if the given arguments are empty string.</exception>
        public SecurityData(string id, string companyName) : this(id, companyName, new string[0])
        {
        }

        /// <summary>
        /// <para>Constructor with the security id, company name and an array of reference ids.</para>
        /// </summary>
        /// <param name="id">the security id.</param>
        /// <param name="companyName">the company name of the security data.</param>
        /// <param name="referenceIds">an array of reference ids.</param>
        ///
        /// <exception cref="ArgumentNullException">if  any argument is null.</exception>
        /// <exception cref="ArgumentException">id or companyName argument is empty string,
        /// or referenceIds argument contains null or empty string element if it's non-empty array.</exception>
        public SecurityData(string id, string companyName, string[] referenceIds)
        {
            Helper.ValidateNotNullNotEmpty(id, "id");
            Helper.ValidateNotNullNotEmpty(companyName, "companyName");
            Helper.ValidateArray(referenceIds, "referenceIds", true, false);

            this.id = id;
            this.companyName = companyName;

            //Shallow copy the referenceIds array and then store
            this.referenceIds = new string[referenceIds.Length];
            referenceIds.CopyTo(this.referenceIds, 0);
        }
    }
}
