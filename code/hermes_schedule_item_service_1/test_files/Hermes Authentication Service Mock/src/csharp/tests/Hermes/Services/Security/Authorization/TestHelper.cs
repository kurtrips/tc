/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */

using System;
using System.Reflection;
using Hermes.Services.Security.Authorization.TopCoder;
using NUnit.Framework;
using TopCoder.Util.ConfigurationManager;

namespace Hermes.Services.Security.Authorization
{
    /// <summary>
    /// Defines helper methods used for tests.
    /// </summary>
    ///
    /// <threadsafety>
    /// All static methods are thread safe.
    /// </threadsafety>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [CoverageExclude]
    internal static class TestHelper
    {
        /// <summary>
        /// <para>
        /// Loads configuration.
        /// </para>
        /// </summary>
        public static void LoadConfig()
        {
            ClearConfig();

            ConfigManager cm = ConfigManager.GetInstance();
            cm.LoadFile("../../test_files/ObjectFactory.xml");
            cm.LoadFile("../../test_files/ExceptionManager.xml");
            cm.LoadFile("../../test_files/WCFBase.xml");
            cm.LoadFile("../../test_files/HermesAuthentiationServiceClient.xml");
            cm.LoadFile("../../test_files/Logger.xml");
            cm.LoadFile("../../test_files/TopCoderAuthConfig.xml");
        }

        /// <summary>
        /// <para>
        /// Loads configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="serviceConfig">
        /// config file name.
        /// </param>
        public static void LoadConfig(string serviceConfig)
        {
            ClearConfig();

            ConfigManager cm = ConfigManager.GetInstance();
            cm.LoadFile("../../test_files/ObjectFactory.xml");
            cm.LoadFile("../../test_files/ExceptionManager.xml");
            cm.LoadFile("../../test_files/WCFBase.xml");
            cm.LoadFile("../../test_files/" + serviceConfig);
            cm.LoadFile("../../test_files/Logger.xml");
        }

        /// <summary>
        /// <para>
        /// Add configuration.
        /// </para>
        /// </summary>
        ///
        /// <param name="serviceConfig">
        /// Config File name.
        /// </param>
        public static void AddConfig(string serviceConfig)
        {
            ConfigManager cm = ConfigManager.GetInstance();
            cm.LoadFile("../../test_files/" + serviceConfig);
        }

        /// <summary>
        /// <para>
        /// Clears configuration.
        /// </para>
        /// </summary>
        public static void ClearConfig()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// <para>
        /// Use reflect to get the value of service.
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// Field type.
        /// </typeparam>
        /// <param name="service">
        /// <see cref="HermesAuthorizationService"/> object.
        /// </param>
        /// <param name="fieldName">
        /// field name.
        /// </param>
        /// <returns>The field value in service.</returns>
        public static T GetField<T>(HermesAuthorizationService service,
            string fieldName)
        {
            FieldInfo info =
                service.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)info.GetValue(service);
        }

        /// <summary>
        /// <para>
        /// Use reflect to invoke method.
        /// </para>
        /// </summary>
        /// <param name="service">
        /// <see cref="HermesAuthorizationService"/> object.
        /// </param>
        /// <param name="methodName">
        /// method name.
        /// </param>
        public static void Invoke(HermesAuthorizationService service,
            string methodName)
        {
            MethodInfo info =
                service.GetType().GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.NonPublic);
            info.Invoke(service, new object[] { });
        }
    }
}
