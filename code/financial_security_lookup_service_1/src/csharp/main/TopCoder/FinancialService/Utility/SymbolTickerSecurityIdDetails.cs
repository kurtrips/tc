// SymbolTickerSecurityIdDetails.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para>This class extends the SecurityIdDetails class and it contains extra attributes like financial markets and
    /// special code for the security id of symbol ticker type.  We can determine the market the security id is in (e.g.
    /// NYSE, NASDAQ or AMEX),  if it cannot be determined, the component tries to narrow down the potential markets
    /// (e.g. [NASDAQ, AMEX])</para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is immutable and thread-safe.
    /// </threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SymbolTickerSecurityIdDetails : SecurityIdDetails
    {
        /// <summary>
        /// <para>
        /// Represents the financial markets the security id is in.It is initialized in the constructor, and never
        /// changed afterwards.  It has property-getter to access it. It must be non-null, non-empty string array. Each
        /// element in the array must be non-null, non-empty string.</para>
        /// </summary>
        private readonly string[] financialMarkets;

        /// <summary>
        /// <para>
        /// Represents the special code for the security id. It is initialized in the constructor, and never changed
        /// afterwards. It has property-getter to access it. It can be null, but it cannot be empty string.</para>
        /// </summary>
        private readonly string specialCode;

        /// <summary>
        /// <para>Represents the property getter for the financialMarkets.</para>
        /// </summary>
        /// <value>Represents the financial markets the security id is in.</value>
        public string[] FinancialMarkets
        {
            get
            {
                //Make shallow copy and return
                string[] ret = new string[financialMarkets.Length];
                financialMarkets.CopyTo(ret, 0);
                return ret;
            }
        }

        /// <summary>
        /// <para>Represents the property getter for the specialCode</para>
        /// </summary>
        /// <value>Represents the special code for the security id.</value>
        public string SpecialCode
        {
            get
            {
                return specialCode;
            }
        }

        /// <summary>
        /// <para>Constructor with security id, security id type, and financial markets.</para>
        /// </summary>
        ///
        /// <param name="id">the security id.</param>
        /// <param name="type">the security id type.</param>
        /// <param name="financialMarkets">the financial markets the security id is in.</param>
        ///
        /// <exception cref="ArgumentNullException">if any given argument is null</exception>
        /// <exception cref="ArgumentException">if id or type argument is empty string, or financialMarkets argument
        /// is empty array or financialMarkets argument contains null or empty string element.</exception>
        public SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets)
            : this(id, type, financialMarkets, null)
        {
        }

        /// <summary>
        /// <para>Constructor with security id, security id type, financial markets and special code.</para>
        /// </summary>
        ///
        /// <param name="id">the security id.</param>
        /// <param name="type">the security id type.</param>
        /// <param name="financialMarkets">the financial markets the security id is in.</param>
        /// <param name="specialCode">the special code of the security id.</param>
        ///
        /// <exception cref="ArgumentNullException">if any argument except specialCode is null.</exception>
        /// <exception cref="ArgumentException">if id or type argument is empty string, or financialMarkets argument
        /// is empty array or financialMarkets argument contains null or empty string element or specialCode is empty.
        /// </exception>
        public SymbolTickerSecurityIdDetails(string id, string type, string[] financialMarkets, string specialCode)
            : base(id, type)
        {
            //Validate
            if (specialCode != null)
            {
                Helper.ValidateNotEmpty(specialCode, "specialCode");
            }
            Helper.ValidateArray(financialMarkets, "financialMarkets", true, true);

            //Assign
            this.specialCode = specialCode;

            //Create shallow copy of array and assign
            this.financialMarkets = new string[financialMarkets.Length];
            financialMarkets.CopyTo(this.financialMarkets, 0);
        }

    }
}
