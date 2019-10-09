/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Xml;
using System.Collections.Generic;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <para>Defines helper methods used by this component. These methods help improve readability
    /// of the code and reduce code redundancy.</para>
    /// <para>Thread Safety: This class contains only static methods and is thus thread safe.</para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class Helper
    {
        /// <summary>
        /// <para>Private constructor so that instance of this class cannot be created.</para>
        /// </summary>
        private Helper()
        {
        }

        /// <summary>
        /// <para>Validates the value of a variable. The value cannot be null.</para>
        /// </summary>
        ///
        /// <param name="value">The value of the variable to be validated.</param>
        /// <param name="name">The name of the variable to be validated.</param>
        ///
        /// <exception cref="ArgumentNullException">The value of the variable is null.</exception>
        public static void ValidateNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, name + " cannot be null.");
            }
        }

        /// <summary>
        /// Validates that string must not be empty. A string with trimmed length equal to zero is considered empty.
        /// </summary>
        /// <param name="value">The string to be validated.</param>
        /// <param name="name">The variable name of the string to be validated.</param>
        /// <exception cref="ArgumentException">If the string is empty.</exception>
        public static void ValidateNotEmpty(string value, string name)
        {
            if (value.Trim().Length == 0)
            {
                throw new ArgumentException(name + " cannot be empty string.", name);
            }
        }

        /// <summary>
        /// <para>Validates the value of a string variable. The value cannot be null or empty string after
        /// trimming.</para>
        /// </summary>
        ///
        /// <param name="value">The value of the variable to be validated.</param>
        /// <param name="name">The name of the variable to be validated.</param>
        ///
        /// <exception cref="ArgumentNullException">The value of the variable is null.</exception>
        /// <exception cref="ArgumentException">The value of the variable is empty string.</exception>
        public static void ValidateNotNullNotEmpty(string value, string name)
        {
            ValidateNotNull(value, name);
            ValidateNotEmpty(value, name);
        }

        /// <summary>
        /// Validates a dictionary that it is not null and does not contain any null/empty keys and null values.
        /// Also, validates that the dictionary is not empty or null.
        /// The Key of the dictionary is of type string.
        /// </summary>
        /// <typeparam name="T">The value type of the dictionary</typeparam>
        /// <param name="dict">The dictionary to validate</param>
        /// <param name="name">The variable name of the dictionary</param>
        /// <exception cref="ArgumentException">
        /// If dictionary is empty or null.
        /// If any key is empty.
        /// If any value is null.
        /// If any value is empty (if value is of type string)
        /// </exception>
        public static void ValidateCollection<T>(IDictionary<string, T> dict, string name)
        {
            //Validate null
            if (dict == null)
            {
                throw new ArgumentException("Dictionary must not be null.", name);
            }

            //Validate empty dictionary
            if (dict.Count == 0)
            {
                throw new ArgumentException("Dictionary must not be empty.", name);
            }

            //Validate data
            foreach (KeyValuePair<string, T> kvp in dict)
            {
                //Validate dictionary key. Must not be empty string.
                ValidateNotEmpty(kvp.Key, "Dictionary key in " + name);

                //Dictionary value must not be null.
                if (kvp.Value == null)
                {
                    throw new ArgumentException("Dictionary value in " + name + "must not be null.", name);
                }

                //If dictionary value is string, then it must not be empty.
                if (kvp.Value is string)
                {
                    ValidateNotEmpty(kvp.Value as string, "Dictionary value in " + name);
                }
            }
        }

        /// <summary>
        /// Loads an xml string into an XmlDocument instance.
        /// </summary>
        /// <param name="xmlString">The xml string</param>
        /// <param name="paramName">The parameter name of the xml string</param>
        /// <returns>The XmlDocument instance created</returns>
        /// <exception cref="UnknownMessageFormatException">
        /// If xmlString is not well formed.
        /// </exception>
        public static XmlDocument LoadXmlDocument(string xmlString, string paramName)
        {
            //Load into XmlDocument
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xmlString);
            }
            catch (Exception e)
            {
                throw new UnknownMessageFormatException("Xml string is not well formed: " + paramName, e);
            }
            return doc;
        }
    }
}
