/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System.Reflection;
using TopCoder.Util.ConfigurationManager;

namespace HermesNS.TC.Services.ScheduleItem
{
    /// <summary>
    /// Defines helper methods used for tests.
    /// </summary>
    ///
    /// <threadsafety>
    /// All static methods are thread safe.
    /// </threadsafety>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class AccuracyTestHelper
    {
        /// <summary>
        /// <para>
        /// Loads configuration.
        /// </para>
        /// </summary>
        internal static void LoadConfig()
        {
            ClearConfig();

            ConfigManager cm = ConfigManager.GetInstance();
            cm.LoadFile("../../test_files/accuracy/ObjectFactory.xml");
            cm.LoadFile("../../test_files/accuracy/ExceptionManager.xml");
            cm.LoadFile("../../test_files/accuracy/WCFBase.xml");
            cm.LoadFile("../../test_files/accuracy/Logger.xml");
            cm.LoadFile("../../test_files/accuracy/ScheduleItemService.xml");
        }

        /// <summary>
        /// <para>
        /// Clears configuration.
        /// </para>
        /// </summary>
        internal static void ClearConfig()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// <para>
        /// Use reflect to get the value of handler.
        /// </para>
        /// </summary>
        /// <typeparam name="T">
        /// Field type.
        /// </typeparam>
        /// <param name="handler">
        /// field owner object.
        /// </param>
        /// <param name="fieldName">
        /// field name.
        /// </param>
        /// <returns>The field value in service.</returns>
        internal static T GetField<T>(object handler, string fieldName)
        {
            FieldInfo info =
                handler.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (info == null)
            {
                info = handler.GetType().BaseType.GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
            }
            return (T)info.GetValue(handler);
        }

        /// <summary>
        /// <para>
        /// Use relect to to invoke non-public method.
        /// </para>
        /// </summary>
        /// <param name="handler">
        /// method owner object.
        /// </param>
        /// <param name="methodName">
        /// the method name
        /// </param>
        /// <param name="param">
        /// parameters
        /// </param>
        internal static void InvokeMethod(object handler, string methodName, params object[] param)
        {
            MethodInfo info =
                handler.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            info.Invoke(handler, param);
        }
    }
}
