/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Configuration;
using TopCoder.Util.ConfigurationManager;
namespace Hermes.Services.Security.Authorization
{
    /// <summary>
    /// <para>
    /// Helper class that defines shared utility methods to do the argument
    /// checks and some operations on <see cref="ConfigManager"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Thread-safety:</strong>
    /// This class is immutable and thread-safe.
    /// </para>
    /// </remarks>
    ///
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    internal sealed class Helper
    {
        /// <summary>
        /// <para>
        /// Prevents initializing instance.
        /// </para>
        /// </summary>
        private Helper()
        {
        }

        /// <summary>
        /// <para>
        /// Checks whether the given object is null.
        /// </para>
        /// </summary>
        ///
        /// <param name="value">
        /// The object to check.
        /// </param>
        /// <param name="paramName">
        /// The actual parameter name of the argument being checked.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If object is null.
        /// </exception>
        public static void CheckNotNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException
                    (paramName, "Argument " + paramName + " can not be null");
            }
        }

        /// <summary>
        /// <para>
        /// Checks whether the given string is null or empty.
        /// </para>
        /// </summary>
        ///
        /// <param name="value">
        /// The string to check.
        /// </param>
        /// <param name="paramName">
        /// The actual parameter name of the argument being checked.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// If string is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If string is empty after trimming.
        /// </exception>
        public static void CheckString(string value, string paramName)
        {
            CheckNotNull(value, paramName);
            if (value.Trim().Length == 0)
            {
                throw new ArgumentException
                    ("Argument " + paramName + " must be non-empty string",
                    paramName);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a single string from specified configuration.
        /// </para>
        /// </summary>
        /// <param name="namespace">
        /// The namespace to be read.
        /// </param>
        /// <param name="propertyName">
        /// The property's name
        /// </param>
        /// <param name="required">
        /// Whether the attribute is mandatory.
        /// </param>
        /// <returns>
        /// The string value retrieved.
        /// </returns>
        /// <exception cref="ConfigurationException">
        /// If the configuration attribute is absent while it is mandatory
        /// or it is not a single non-empty string.
        /// </exception>
        public static string LoadPropertyString(string @namespace,
            string propertyName, bool required)
        {
            try
            {
                ConfigManager cm = ConfigManager.GetInstance();
                string ret = cm.GetValue(@namespace, propertyName);
                if (ret == null && !required)
                {
                    return null;
                }
                CheckString(ret, propertyName);
                return ret;
            }
            catch (Exception e)
            {
                throw new ConfigurationException(
                    "Load " + propertyName + " failed.", e);
            }
        }

        /// <summary>
        /// Gets a string array from the configuration using the given namespace and property name. The string
        /// element in array cannot be empty string. If the property does not exist, return an empty array.
        /// </summary>
        ///
        /// <param name="nameSpace">The namespace where the property value is read.</param>
        /// <param name="propertyName">The property name where the property value is read.</param>
        /// <param name="allowEmptyString">The flag indicating whether it is allowed that the element in string array 
        /// is empty string.</param>
        /// <returns>The string array configured, or empty array if the property does not exist.</returns>
        /// <exception cref="ConfigurationException">If any string element in array is empty string,
        /// or any other error occurs.</exception>
        public static string[] GetValues(string nameSpace, string propertyName, bool allowEmptyString)
        {
            ConfigManager cm;
            try
            {
                // Try to get instance of ConfigManager, it may fail when it can not find 'preload.xml' file.
                cm = ConfigManager.GetInstance();
            }
            catch (Exception e)
            {
                throw new ConfigurationException(
                    "Failed to get instance of ConfigManager.", e);
            }

            string[] values = cm.GetValues(nameSpace, propertyName);

            // if property does not exist, return empty array.
            if (values == null)
            {
                return new string[0];
            }

            if (!allowEmptyString)
            {
                foreach (string value in values)
                {
                    if (value.Trim().Length == 0)
                    {
                        throw new ConfigurationErrorsException(
                            "Property '" + propertyName + "' cannot contain empty string element.");
                    }
                }   
            }

            return values;
        }
    }
}
