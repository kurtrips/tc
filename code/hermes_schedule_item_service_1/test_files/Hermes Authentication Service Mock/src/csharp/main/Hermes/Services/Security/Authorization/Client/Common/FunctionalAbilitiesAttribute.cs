/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// <para>
    /// This the class represent the function attributes.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <p>
    /// <strong>Thread Safety:</strong>
    /// This class is not thread safe.
    /// </p>
    /// </remarks>
    ///
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    public class FunctionalAbilitiesAttribute : Attribute
    {
        /// <summary>
        /// <para>
        /// Represents the list of function ability.
        /// </para>
        /// </summary>
        private readonly List<string> _FunctionalAbilities = new List<string>();

        /// <summary>
        /// <para>
        /// Create new <see cref="FunctionalAbilitiesAttribute"/> instance with
        /// <paramref name="functionalAbilities"/>.
        /// </para>
        /// </summary>
        /// <param name="functionalAbilities">
        /// The abilities to be assigned.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="functionalAbilities"/> is null.
        /// </exception>
        public FunctionalAbilitiesAttribute(
            params string[] functionalAbilities)
        {
            Helper.CheckNotNull(functionalAbilities, "functionalAbilities");
            foreach (string functionAbility in functionalAbilities)
            {
                if (functionAbility == null ||
                    functionAbility.Trim().Length == 0)
                {
                    throw new ArgumentException(
                        "Element should not be null or empty.",
                        "functionalAbilities");
                }
            }
            _FunctionalAbilities.AddRange(functionalAbilities);
        }

        /// <summary>
        /// <para>
        /// Getter for function abilities.
        /// </para>
        /// </summary>
        /// <value>
        /// The list of function ability.
        /// </value>
        public List<string> FunctionalAbilities
        {
            get 
            { 
                return _FunctionalAbilities; 
            }
        }
    }
}
