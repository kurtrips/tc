/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;

namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// Mock implementation of IAuthorizationMappingProvider interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [HermesNS.TC.CoverageExcludeAttribute]
    public class MockAuthorizationMappingProvider : IAuthorizationMappingProvider
    {
        /// <summary>Get function name mapped.</summary>
        /// <param name="referenceFunctionName">Reference name.</param>
        /// <returns>The mapped name.</returns>
        /// <exception cref="ArgumentNullException">if the referenceFunctionName is null.</exception>
        /// <exception cref="ArgumentException">if the referenceFunctionName is empty.</exception>
        public string GetFunctionName(string referenceFunctionName)
        {
            return referenceFunctionName;
        }

        /// <summary>Get function attribute name mapped.</summary>
        /// <param name="referenceFunctionName">Reference name.</param>
        /// <param name="attribute">Attribute name.</param>
        /// <returns>The mapped name.</returns>
        /// <exception cref="ArgumentNullException">referenceFunctionName or attribute is null.</exception>
        /// <exception cref="ArgumentException">if the ArgumentException or attribute is empty.</exception>
        public string GetFunctionAttributeName(string referenceFunctionName, string attribute)
        {
            return referenceFunctionName;
        }
    }
}
