// Helper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.ServiceModel;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Services.WCF.ScheduleItem;
using TopCoder.Util.ExceptionManager.SDE;
using Oracle.DataAccess.Client;
using HermesNS.Entity.Common;
using HermesNS.SystemServices.Data.ProxyConnection;
using Hermes.Services.Security.Authorization.Client.Common;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// <para>
    /// This class exposes static helper functions which helps improve code readability and reduces code redundancy.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>This class is stateless and thread-safe.</threadsafety>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class Helper
    {
        /// <summary>
        /// Checks whether an object is null or not
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="name">The name of the object</param>
        /// <exception cref="InvalidArgumentException">If object is null</exception>
        internal static void ValidateNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new InvalidArgumentException(name + " must not be null.");
            }
        }

        /// <summary>
        /// Checks whether an string is empty or not (after trimming).
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="InvalidArgumentException">If string is empty</exception>
        internal static void ValidateNotEmpty(string str, string name)
        {
            if (str != null && str.Trim().Equals(String.Empty))
            {
                throw new InvalidArgumentException(name + " must not be empty.");
            }
        }

        /// <summary>
        /// Checks whether an string is neither empty nor null.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="InvalidArgumentException">If string is empty or null</exception>
        internal static void ValidateNotNullNotEmpty(string str, string name)
        {
            ValidateNotNull(str, name);
            ValidateNotEmpty(str, name);
        }

        /// <summary>
        /// Gets the value of a config manager property inside the given namespace.
        /// </summary>
        /// <param name="nameSpace">The config manager namespace from which to read the property.</param>
        /// <param name="key">The property name to read</param>
        /// <param name="isRequired">Whether the property value should be non null</param>
        /// <returns>The value of a config manager property inside the given namespace.</returns>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If the property is required and is not found.
        /// If the property value is empty after trimming.
        /// </exception>
        internal static string GetConfigValue(string nameSpace, string key, bool isRequired)
        {
            string value = ConfigManager.GetInstance().GetValue(nameSpace, key);

            //If required and null, throw error. If empty then error is thrown irrespective of isRequired
            if ((value == null || isRequired) && value.Trim().Equals(string.Empty))
            {
                throw new ScheduleItemConfigurationException("Required configuration parameter: " + key +
                    " could not be found in configuration namespace: " + nameSpace);
            }

            return value;
        }

        /// <summary>
        /// Wraps the given exception into a SelfDocumentingException instance after adding the instance variables,
        /// method parameters and local variables to it
        /// </summary>
        /// <param name="e">The exception to wrap.</param>
        /// <param name="message">The message for the SelfDocumentingException</param>
        /// <param name="methodName">The fully qualified method name from where the exception is thrown.</param>
        /// <param name="instanceVarsNames">The names of the instance variables.</param>
        /// <param name="instanceVars">The values of the instance variables at time of exception.</param>
        /// <param name="parameterVarsNames">The names of the method parameters.</param>
        /// <param name="parameterVars">The values of the method parameters at time of exception.</param>
        /// <param name="localVarsNames">The names of the local variables.</param>
        /// <param name="localVars">The values of the local variables at time of exception.</param>
        /// <param name="exceptionToThrow">The type of exception (deriving from SDE) to throw.</param>
        /// <returns>The formed SelfDocumentingException instance.</returns>
        internal static SelfDocumentingException GetSDE(Exception e, string message, string methodName,
            string[] instanceVarsNames, object[] instanceVars,
            string[] parameterVarsNames, object[] parameterVars,
            string[] localVarsNames, object[] localVars,
            Type exceptionToThrow)
        {
            //Wrap only if it is not already of type SelfDocumentingException
            SelfDocumentingException sde = null;
            if (e is SelfDocumentingException)
            {
                sde = (SelfDocumentingException)e;
            }
            else
            {
                //Create instance of the actual exception type to throw
                sde = (SelfDocumentingException)exceptionToThrow.GetConstructor(
                    new Type[] { typeof(string), typeof(Exception) }).Invoke(new object[] { message, e });
            }

            MethodState ms = sde.PinMethod(methodName, e.StackTrace);

            //Add instance variables, method parameters and local variables
            for (int i = 0; i < instanceVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. A class with setter only property cannot be added.
                try
                {
                    ms.AddInstanceVariable(instanceVarsNames[i], instanceVars[i]);
                }
                catch { }
            }
            for (int i = 0; i < parameterVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. A class with setter only property cannot be added.
                try
                {
                    ms.AddMethodParameter(parameterVarsNames[i], parameterVars[i]);
                }
                catch { }
            }
            for (int i = 0; i < localVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. A class with setter only property cannot be added.
                try
                {
                    ms.AddLocalVariable(localVarsNames[i], localVars[i]);
                }
                catch { }
            }

            ms.Lock();
            return sde;
        }

        /// <summary>
        /// Gets an OracleConnection instance given the connection string using the WcfHelper component.
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns>An OracleConnection instance</returns>
        internal static OracleConnection GetConnection(string connectionString)
        {
            Profile profile = WcfHelper.GetProfileFromContext(OperationContext.Current);
            return OracleConnectionHelper.GetPooledConnection(profile.UserID, connectionString);
        }

        /// <summary>
        /// Checks whether a given string id is a valid Guid.
        /// </summary>
        /// <param name="id">The string to check</param>
        /// <exception cref="InvalidArgumentException">
        /// If the given id is not a valid guid.
        /// </exception>
        internal static void ValidateGuid(string id)
        {
            try
            {
                new Guid(id);
            }
            catch (Exception e)
            {
                throw new InvalidArgumentException("Entity's id is not a valid guid.", e);
            }
        }
    }
}

//namespace HermesNS.SystemServices.Data.ProxyConnection
//{
//    /// <summary>
//    /// A mock implementation of OracleConnectionHelper class.
//    /// </summary>
//    public class OracleConnectionHelper
//    {
//        /// <summary>
//        /// Mock implementation. Gets a new OracleConnection for the given connection name.
//        /// </summary>
//        /// <param name="userID">This param is ignored</param>
//        /// <param name="connectionName">The connection string with which to form OracleConnection instance</param>
//        /// <returns>Created OracleConnection instance.</returns>
//        public static OracleConnection GetPooledConnection(string userID, string connectionName)
//        {
//            return new OracleConnection(connectionName);
//        }
//    }
//}
