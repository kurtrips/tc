/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */
namespace TopCoder.FinancialService.Utility.StressTests
{
    /// <summary>
    /// <para>
    /// A custom implementation of <see cref="ISecurityLookupService"/> used in stress tests.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is immutable and is thread safe.
    /// </threadsafety>
    ///
    /// <author>crazypigs</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All Rights Reserved.</copyright>
    public class CustomSecurityLookupService : ISecurityLookupService
    {
        /// <summary>
        /// <para>
        /// Default constructor.
        /// </para>
        /// </summary>
        public CustomSecurityLookupService()
        {
        }

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
        public SecurityData Lookup(SecurityIdDetails securityIdDetails)
        {
            if (securityIdDetails.Id == "A")
            {
                // security id A references B & C
                return new SecurityData("A", "company1", new string[] {"B", "C"});
            }
            else if (securityIdDetails.Id == "B")
            {
                // security id B references D
                return new SecurityData("B", "company1", new string[] {"D"});
            }
            else if (securityIdDetails.Id == "C")
            {
                // security id C has no direct references.
                return new SecurityData("C", "company1");
            }
            else if (securityIdDetails.Id == "D")
            {
                // security id D references B
                return new SecurityData("D", "company1", new string[] {"B"});
            }
            throw new SecurityLookupException("Error occurs");
        }
    }
}