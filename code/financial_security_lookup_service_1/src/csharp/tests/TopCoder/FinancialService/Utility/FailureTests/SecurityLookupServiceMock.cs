/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
namespace TopCoder.FinancialService.Utility.FailureTests
{
    /// <summary>
    /// Mock of the ISecurityLookupService interface.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class SecurityLookupServiceMock : ISecurityLookupService
    {
        /// <summary>
        /// Creates a new <c>SecurityLookupServiceTester</c> instance.
        /// </summary>
        public SecurityLookupServiceMock()
        {
        }

        /// <summary>
        /// Implements a basic lookup function.
        /// </summary>
        /// <param name="securityIdDetails">The security id details.</param>
        /// <returns>Security data that matches the details.</returns>
        public SecurityData Lookup(SecurityIdDetails securityIdDetails)
        {
            if (securityIdDetails.Id.Equals("A"))
            {
                // security id A references B & C
                return new SecurityData("A", "companyA", new string[] { "B", "C" });
            }
            else if (securityIdDetails.Id.Equals("B"))
            {
                // security id B references D
                return new SecurityData("B", "companyB", new string[] { "D" });
            }
            else if (securityIdDetails.Id.Equals("C"))
            {
                // security id C has no direct references.
                return new SecurityData("C", "companyC");
            }
            else if (securityIdDetails.Id.Equals("D"))
            {
                // security id D references B
                return new SecurityData("D", "companyD", new string[] { "B" });
            }
            return null;
        }
    }
}