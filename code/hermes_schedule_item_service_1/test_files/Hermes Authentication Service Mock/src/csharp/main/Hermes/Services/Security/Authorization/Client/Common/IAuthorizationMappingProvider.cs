/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// <para>
    /// This is class is used to map the functon name and attributes.
    /// </para>
    /// </summary>
    ///
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
   public interface IAuthorizationMappingProvider
   {
       /// <summary>
       /// <para>
       /// Get function name mapped.
       /// </para>
       /// </summary>
       /// <param name="referenceFunctionName">
       /// Reference name.
       /// </param>
       /// <returns>
       /// The mapped name.
       /// </returns>
       /// <exception cref="ArgumentNullException">
       /// if the <paramref name="referenceFunctionName"/> is null.
       /// </exception>
       /// <exception cref="ArgumentException">
       /// if the <paramref name="referenceFunctionName"/> is empty.
       /// </exception>
      string GetFunctionName(string referenceFunctionName);

      /// <summary>
      /// <para>
      /// Get function attribute name mapped.
      /// </para>
      /// </summary>
      /// <param name="referenceFunctionName">
      /// Reference name.
      /// </param>
      /// <param name="attribute">
      /// Attribute name.
      /// </param>
      /// <returns>
      /// The mapped name.
      /// </returns>
      /// <exception cref="ArgumentNullException">
      /// if the <paramref name="referenceFunctionName"/> or
      /// <paramref name="attribute"/>is null.
      /// </exception>
      /// <exception cref="ArgumentException">
      /// if the <paramref name="referenceFunctionName"/> or
      /// <paramref name="attribute"/> is empty.
      /// </exception>
      string GetFunctionAttributeName(string referenceFunctionName,
          string attribute);
   }
}
