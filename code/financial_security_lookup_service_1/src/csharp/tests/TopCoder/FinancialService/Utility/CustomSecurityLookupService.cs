// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Custom ISecurityLookupService implementation for testing.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class CustomSecurityLookupService : ISecurityLookupService
    {
        /// <summary>
        /// Default empty constructor.
        /// </summary>
        public CustomSecurityLookupService() { }

        /// <summary>
        /// Mocks the lookup behavior. Returns data according to the Id of securityIdDetails.
        /// </summary>
        /// <param name="securityIdDetails">The securityId details</param>
        /// <returns>Returns data according to the Id of securityIdDetails.</returns>
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
            else if (securityIdDetails.Id == "X")
            {
                // security id X references Y and X
                return new SecurityData("X", "company1", new string[] { "Y", "X" });
            }
            else if (securityIdDetails.Id == "Y")
            {
                // security id Y references X
                return new SecurityData("Y", "company1", new string[] { "X" });
            }
            else if (securityIdDetails.Id == "ZZZ")
            {
                throw new ServiceNotAvailableException("Service is down.");
            }
            throw new SecurityLookupException("No such company found");
        }
    }
}
