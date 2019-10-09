/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using TopCoder.Configuration;
using TopCoder.Util.ObjectFactory;
namespace Hermes.Services.Security.Authorization.Client.Common
{
    /// <summary>
    /// <para>
    /// This class is used to help mediate method.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <p>
    /// <strong>Thread Safety:</strong>
    /// static class is thread safe, and a synchronize object is used for
    /// configuration object.
    /// </p>
    /// </remarks>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    public static class HermesAuthorizationMediator
    {
        /// <summary>
        /// <para>
        /// Represents the key in object factory configuration.
        /// </para>
        /// </summary>
        private const string AuthorizationMappingProvider =
            "AuthorizationMappingProvider";

        /// <summary>
        /// <para>
        /// The configuration used to create
        /// <see cref="IAuthorizationMappingProvider"/> instance.
        /// </para>
        /// </summary>
        private static IConfiguration _Configuration;

        /// <summary>
        /// <para>
        /// Syhchrnoization object for configuration.
        /// </para>
        /// </summary>
        private static object _SyncObj = new object();

        /// <summary>
        /// <para>
        /// Sets the configuration object which is used for creating
        /// <see cref="IAuthorizationMappingProvider"/> instance.
        /// </para>
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If the value is null.
        /// </exception>
        public static IConfiguration Configuration
        {
            set
            {
                lock (_SyncObj)
                {
                    Helper.CheckNotNull(value, "Configuration");
                    _Configuration = value;
                }
            }
        }

        /// <summary>
        /// <para>
        /// Check function's attributes.
        /// </para>
        /// </summary>
        /// <param name="applicationId">
        /// Application id.
        /// </param>
        /// <param name="sessionID">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="methodBase">the method to be mediate.</param>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="methodBase"/> is null.
        /// </exception>
        public static void MediateMethod(string applicationId, string sessionID,
            string sessionToken, MethodBase methodBase)
        {
            Helper.CheckNotNull(methodBase, "methodBase");
            FunctionalAbilitiesAttribute[] attributes =
                methodBase.GetCustomAttributes(
                    typeof(FunctionalAbilitiesAttribute), true)
                        as FunctionalAbilitiesAttribute[];

            if (!CheckFunctionalAttributes(applicationId, sessionID,
                sessionToken, attributes))
            {
                throw new NoFunctionalAttributeException(
                    methodBase, "Can not find function attributes.");
            }
        }

        /// <summary>
        /// <para>
        /// Check type's attributes.
        /// </para>
        /// </summary>
        /// <param name="applicationId">
        /// Application id.
        /// </param>
        /// <param name="sessionID">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="type">the type to be mediate.</param>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="type"/> is null.
        /// </exception>
        public static void MediateObject(string applicationId, string sessionID,
            string sessionToken, Type type)
        {
            Helper.CheckNotNull(type, "type");
            FunctionalAbilitiesAttribute[] attributes =
                type.GetCustomAttributes(typeof(FunctionalAbilitiesAttribute),
                    true) as FunctionalAbilitiesAttribute[];

            if (!CheckFunctionalAttributes(applicationId, sessionID,
                sessionToken, attributes))
            {
                throw new NoFunctionalAttributeException(
                    string.Format("Can not find {0}", type.FullName));
            }
        }

        /// <summary>
        /// <para>
        /// Check functions.
        /// </para>
        /// </summary>
        /// <param name="applicationId">
        /// application id.
        /// </param>
        /// <param name="sessionID">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="attributes">the attributes to be checked.</param>
        /// <returns>
        /// true if all attributes pass check, otherwise false.
        /// </returns>
        private static bool CheckFunctionalAttributes(string applicationId,
            string sessionID, string sessionToken,
                FunctionalAbilitiesAttribute[] attributes)
        {
            using (HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient())
            {
                IAuthorizationMappingProvider provider = CreateMapper();
                foreach (FunctionalAbilitiesAttribute attribute in attributes)
                {
                    foreach (string functionalAbility in attribute.FunctionalAbilities)
                    {
                        if (client.CheckFunction(sessionID, sessionToken,
                            provider.GetFunctionName(functionalAbility)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// <para>
        /// Get the function name using the <paramref name="referenceFunctionName"/>.
        /// </para>
        /// </summary>
        /// <param name="applicationId">
        /// application id.
        /// </param>
        /// <param name="sessionId">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="referenceFunctionName">
        /// Reference function name.
        /// </param>
        /// <returns></returns>
        public static IList<KeyValuePair<string, string>> GetFunctionAttributes
            (string applicationId, string sessionId,
            string sessionToken, string referenceFunctionName)
        {
            using (HermesAuthorizationServiceClient client =
                HermesAuthorizationServiceClient.GetClient())
            {
                IAuthorizationMappingProvider provider = CreateMapper();
                List<KeyValuePair<string, string>> attributes =
                    client.GetFunctionAttributes(sessionId, sessionToken,
                        provider.GetFunctionName(referenceFunctionName));
                List<KeyValuePair<string, string>> ret =
                    new List<KeyValuePair<string, string>>();
                foreach (KeyValuePair<string, string> value in attributes)
                {
                    ret.Add(new KeyValuePair<string,string>(
                        provider.GetFunctionAttributeName(
                        referenceFunctionName, value.Key), value.Value));
                }
                return ret;
            }
        }

        /// <summary>
        /// <para>
        /// Create <see cref="IAuthorizationMappingProvider"/> with
        /// <see cref="ConfigurationAPIObjectFactory"/>.
        /// </para>
        /// </summary>
        /// <returns>
        /// The <see cref="IAuthorizationMappingProvider"/> instance.
        /// </returns>
        private static IAuthorizationMappingProvider CreateMapper()
        {
            ConfigurationAPIObjectFactory objectFactory =
                new ConfigurationAPIObjectFactory(_Configuration);
            return objectFactory.CreateDefinedObject(AuthorizationMappingProvider) as
                IAuthorizationMappingProvider;
        }
    }
}
