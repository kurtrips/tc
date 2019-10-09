/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 
using System;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Configuration;
using TopCoder.LoggingWrapper;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// <para>
    /// This class exposes static helper functions which help improve code readability and reduces code redundancy.
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
        /// <exception cref="ArgumentNullException">If object is null</exception>
        internal static void ValidateNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name, name + " must not be null.");
            }
        }

        /// <summary>
        /// Checks whether an string is empty or not (after trimming).
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentException">If string is empty</exception>
        internal static void ValidateNotEmpty(string str, string name)
        {
            if (str.Trim().Equals(String.Empty))
            {
                throw new ArgumentException(name + " must not be empty.", name);
            }
        }

        /// <summary>
        /// Checks whether an string is neither empty nor null.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentException">If string is empty</exception>
        /// <exception cref="ArgumentNullException">If string is null</exception>
        internal static void ValidateNotNullNotEmpty(string str, string name)
        {
            ValidateNotNull(str, name);
            ValidateNotEmpty(str, name);
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
        /// <returns>The formed SelfDocumentingException instance.</returns>
        internal static SelfDocumentingException GetSelfDocumentingException(
            Exception e, string message, string methodName, string[] instanceVarsNames, object[] instanceVars,
            string[] parameterVarsNames, object[] parameterVars, string[] localVarsNames, object[] localVars)
        {
            //Wrap only if it is not already of type SelfDocumentingException
            SelfDocumentingException sde = null;
            if (e is SelfDocumentingException)
            {
                sde = (SelfDocumentingException)e; 
            }
            else
            {
                sde = new SelfDocumentingException(message, e);
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
                catch
                {
                }
            }
            for (int i = 0; i < parameterVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. A class with setter only property cannot be added.
                try
                {
                    ms.AddMethodParameter(parameterVarsNames[i], parameterVars[i]);
                }
                catch
                {
                }
            }
            for (int i = 0; i < localVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. A class with setter only property cannot be added.
                try
                {
                    ms.AddLocalVariable(localVarsNames[i], localVars[i]);
                }
                catch
                {
                }
            }

            ms.Lock();
            return sde;
        }

        /// <summary>
        /// Reads a configration element from the configuration object.
        /// </summary>
        /// <param name="config">The configuration object.</param>
        /// <param name="key">The key of the element</param>
        /// <param name="isRequired">Whether the element is required.</param>
        /// <returns>The value of the configuration element.</returns>
        internal static string ReadConfig(IConfiguration config, string key, bool isRequired)
        {
            object value = null;
            try
            {
                //Get simple value
                value = config.GetSimpleAttribute(key);

                //Error if key not found and it is required element.
                if (value == null && isRequired)
                {
                    throw new ConfigurationAPIException(
                        key + " is a required configuration parameter but is missing.");
                }

                //Empty is not allowed
                string retValue = (string)value;
                if (retValue != null && retValue.Trim().Equals(string.Empty))
                {
                    throw new ConfigurationAPIException(
                        key + " configuration parameter must not contain empty string.");
                }

                return retValue;
            }
            catch (Exception e)
            {
                throw GetSelfDocumentingException(e, "Unable to read element from config.",
                    "Astraea.Inframap.Layout.Helper.ReadConfig", new string[0], new object[0],
                    new string[] { "config", "key", "isRequired" }, new object[] { config, key, isRequired },
                    new string[] { "value" }, new object[] { value });
            }
        }

        /// <summary>
        /// Reads an integer value from an IConfiguration instance for a given key
        /// </summary>
        /// <param name="config">The IConfiguration instance from which to read.</param>
        /// <param name="key">The key in the IConfiguration object for which to get value.</param>
        /// <returns>The integer value of the key.</returns>
        /// <exception cref="SelfDocumentingException">
        /// If the value at the key attribute is not a valid integer or if it is a non positive integer.
        /// (Wraps ConfigurationAPIException)
        /// </exception>
        internal static int ReadConfigInt(IConfiguration config, string key)
        {
            try
            {
                int ret = int.Parse(ReadConfig(config, key, true));
                if (ret <= 0)
                {
                    throw new ConfigurationAPIException("Parsed integer value is a non-positive number.");
                }
                return ret;
            }
            catch (FormatException fe)
            {
                throw GetSelfDocumentingException(
                    new ConfigurationAPIException(
                        key + " configuration parameter value is not a valid integer.", fe),
                    "Unable to read element from config.",
                    "Astraea.Inframap.Layout.Helper.ReadConfigInt", new string[0], new object[0],
                    new string[] { "config", "key" }, new object[] { config, key },
                    new string[0], new object[0]);
            }
            catch (Exception e)
            {
                throw GetSelfDocumentingException(e, e.Message,
                    "Astraea.Inframap.Layout.Helper.ReadConfigInt", new string[0], new object[0],
                    new string[] { "config", "key" }, new object[] { config, key },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Reads a double value from an IConfiguration instance for a given key
        /// </summary>
        /// <param name="config">The IConfiguration instance from which to read.</param>
        /// <param name="key">The key in the IConfiguration object for which to get value.</param>
        /// <returns>The double value of the key.</returns>
        /// <exception cref="SelfDocumentingException">
        /// If the value at the key attribute is not a valid double or if it is a non positive double.
        /// (Wraps ConfigurationAPIException)
        /// </exception>
        internal static double ReadConfigDouble(IConfiguration config, string key)
        {
            try
            {
                double ret = double.Parse(ReadConfig(config, key, true));
                if (ret <= 0)
                {
                    throw new ConfigurationAPIException("Parsed double value is a non-positive number.");
                }
                return ret;
            }
            catch (FormatException fe)
            {
                throw GetSelfDocumentingException(
                    new ConfigurationAPIException(
                        key + " configuration parameter value is not a valid double.", fe),
                    "Unable to read element from config.",
                    "Astraea.Inframap.Layout.Helper.ReadConfigDouble", new string[0], new object[0],
                    new string[] { "config", "key" }, new object[] { config, key },
                    new string[0], new object[0]);
            }
            catch (Exception e)
            {
                throw GetSelfDocumentingException(e, e.Message,
                    "Astraea.Inframap.Layout.Helper.ReadConfigDouble", new string[0], new object[0],
                    new string[] { "config", "key" }, new object[] { config, key },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// A helper method for logging purpose.
        /// </summary>
        /// <param name="logger">The logger instance. Can be null. If null then no logging is performed.</param>
        /// <param name="level">The level at which to log.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="param">The objects with which to format the message string.</param>
        internal static void Log(Logger logger, Level level, string message, object[] param)
        {
            if (logger != null)
            {
                logger.Log(level, message, param);
            }
        }
    }
}
