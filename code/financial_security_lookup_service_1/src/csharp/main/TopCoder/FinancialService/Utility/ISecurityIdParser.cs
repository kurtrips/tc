// ISecurityIdParser.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para> This interface defines the contract to parse the security id to get its details. The security id details
    /// should at least contain its identification type, and it may contain some other security id specific information
    /// too.</para>
    /// </summary>
    ///
    /// <threadsafety>Implementations should be thread-safe.</threadsafety>
    ///
    /// <author>Standlove</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ISecurityIdParser
    {
        /// <summary>
        /// <para>
        /// Parse the security id to get its details.
        /// The result must at least contain the security identifier type.
        /// </para>
        /// </summary>
        ///
        /// <param name="securityId">the security id to parse.</param>
        /// <returns>the parsed SecurityIdDetails object.</returns>
        ///
        /// <exception cref="ArgumentNullException">if the given argument is null.</exception>
        /// <exception cref="ArgumentException">if the given argument is empty string.</exception>
        /// <exception cref="UnknownSecurityIdTypeException">if type of the given security id is unknown.</exception>
        /// <exception cref="InvalidSecurityIdFormatException">if the format of security id is invalid.</exception>
        /// <exception cref="SecurityIdParsingException">if any other error occurs in parsing the id.</exception>
        SecurityIdDetails Parse(string securityId);
    }
}
