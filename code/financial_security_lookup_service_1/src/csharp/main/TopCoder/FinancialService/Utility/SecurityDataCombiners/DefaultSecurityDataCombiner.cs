// DefaultSecurityDataCombiner.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.FinancialService.Utility.SecurityDataCombiners
{
    /// <summary>
    /// <para> This class implements the ISecurityDataCombiner interface, and it is used to combine the two passed-in
    /// SecurityData objects into a new SecurityData object.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is stateless and thread-safe.
    /// </threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class DefaultSecurityDataCombiner : ISecurityDataCombiner
    {
        /// <summary>
        /// <para>Empty constructor.</para>
        /// </summary>
        public DefaultSecurityDataCombiner()
        {
        }

        /// <summary>
        /// <para>
        /// Combines the id and referenceIds of firstSecurityData and secondSecurityData
        /// arguments into a new refIds array with unique values. This happens even if the securities are not related.
        /// The return data has id and company name equal to the respective fields of the firstSecurityData.
        /// The passed-in security data objects are not changed.
        /// </para>
        /// </summary>
        ///
        /// <param name="firstSecurityData">the first security data.</param>
        /// <param name="secondSecurityData">the second security data.</param>
        ///
        /// <returns>the combined SecurityData object.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// if any argument is null.
        /// </exception>
        public SecurityData Combine(SecurityData firstSecurityData, SecurityData secondSecurityData)
        {
            //List to hold the combined reference ids.
            List<string> combinedRefIds = new List<string>();


            Helper.ValidateNotNull(firstSecurityData, "firstSecurityData");
            Helper.ValidateNotNull(secondSecurityData, "secondSecurityData");

            //Add the reference ids of the firstSecurityData
            for (int i = 0; i < firstSecurityData.ReferenceIds.Length; i++)
            {
                if (!combinedRefIds.Contains(firstSecurityData.ReferenceIds[i]))
                {
                    combinedRefIds.Add(firstSecurityData.ReferenceIds[i]);
                }
            }

            //Add the reference ids of the secondSecurityData
            for (int i = 0; i < secondSecurityData.ReferenceIds.Length; i++)
            {
                if (!combinedRefIds.Contains(secondSecurityData.ReferenceIds[i]))
                {
                    combinedRefIds.Add(secondSecurityData.ReferenceIds[i]);
                }
            }

            //Add the id of the firstSecurityData
            if (!combinedRefIds.Contains(firstSecurityData.Id))
            {
                combinedRefIds.Add(firstSecurityData.Id);
            }

            //Add the id of the secondSecurityData
            if (!combinedRefIds.Contains(secondSecurityData.Id))
            {
                combinedRefIds.Add(secondSecurityData.Id);
            }

            return new SecurityData(firstSecurityData.Id, firstSecurityData.CompanyName, combinedRefIds.ToArray());
        }
    }
}
