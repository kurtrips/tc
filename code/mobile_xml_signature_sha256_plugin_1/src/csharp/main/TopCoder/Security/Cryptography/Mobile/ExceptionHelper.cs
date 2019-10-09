/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Xml;
using System.Collections.Generic;
using TopCoder.Util.CompactConfigurationManager;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// <p>This class is internal and is used to throw various business exceptions. This class exposes static
    /// methods for validations. This helps in reducing code redundancy and improves general readability of
    /// the code.</p>
    /// </summary>
    /// <author>kurtrips</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class ExceptionHelper
    {
        /// <summary>
        /// <p>Error message if name of encrypted elemnt is not EncryptedData</p>
        /// </summary>
        private const string INVALID_NAME = "Name of Encrypted element is not EncryptedData";

        /// <summary>
        /// <p>Private constructor of ExceptionHelper class.</p>
        /// </summary>
        ///
        /// <remarks>
        /// <p>This private constructor prevents the creation of a new instance.</p>
        /// </remarks>
        private ExceptionHelper()
        {
        }

        /// <summary>
        /// <p>Validates the value of a variable. The value cannot be null.</p>
        /// </summary>
        /// <param name="value">The value of the variable to be validated.</param>
        /// <param name="name">The name of the variable to be validated.</param>
        /// <exception cref="ArgumentNullException">The value of the variable is null.</exception>
        public static void ValidateNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name, name + " cannot be null.");
            }
        }

        /// <summary>
        /// <p>Validates the value of a variable. The value cannot be null.</p>
        /// </summary>
        /// <param name="value">The value of the variable to be validated.</param>
        /// <param name="name">The name of the variable to be validated.</param>
        /// <exception cref="ArgumentException">The value of the variable is null.</exception>
        public static void ValidateNotNullArg(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentException(name + " cannot be null.", name);
            }
        }

        /// <summary>
        /// <p>Validates the value of a string. The value cannot be empty string.</p>
        /// </summary>
        /// <param name="value">The value of the string to be validated.</param>
        /// <param name="name">The name of the string to be validated.</param>
        /// <exception cref="ArgumentException">The string is empty.</exception>
        public static void ValidateStringNotEmpty(string value, string name)
        {
            if (value.Trim().Equals(String.Empty))
            {
                throw new ArgumentException(name + " cannot be empty.", name);
            }
        }

        /// <summary>
        /// <p>Validates the value of a string. The value cannot be empty string or null.</p>
        /// </summary>
        /// <param name="value">The value of the string to be validated.</param>
        /// <param name="name">The name of the string to be validated.</param>
        /// <exception cref="ArgumentNullException">The value of the string is null.</exception>
        /// <exception cref="ArgumentException">The value of the string is empty.</exception>
        public static void ValidateStringNotNullNotEmpty(string value, string name)
        {
            ValidateNotNull(value, name);
            ValidateStringNotEmpty(value, name);
        }

        /// <summary>
        /// <p>Validates the value of a string. The value cannot be empty string or null.</p>
        /// </summary>
        /// <param name="value">The value of the string to be validated.</param>
        /// <param name="name">The name of the string to be validated.</param>
        /// <exception cref="ArgumentException">The value of the string is empty or null</exception>
        public static void ValidateStringNotNullNotEmptyArg(string value, string name)
        {
            ValidateNotNullArg(value, name);
            ValidateStringNotEmpty(value, name);
        }

        /// <summary>
        /// Validates list of transformer InstantiationVO for empty and null elements
        /// </summary>
        /// <param name="list">List if InstantiationVOs</param>
        /// <param name="name">Name of parameter</param>
        /// <exception cref="ArgumentException">If any key or value is empty or a null reference</exception>
        public static void ValidateTransformerList(IList<InstantiationVO> list, string name)
        {
            foreach (InstantiationVO ivo in list)
            {
                ValidateStringNotNullNotEmptyArg(ivo.Key , "Transformer Key");
            }
        }

        /// <summary>
        /// Checks parameters collection for empty strings and null references
        /// </summary>
        /// <param name="parameters">the collection</param>
        /// <exception cref="ArgumentNullException">if parameters is null</exception>
        /// <exception cref="ArgumentException">If any key or value is empty or a null reference</exception>
        public static void ValidateInstantiationVOParams(IDictionary<string, object> parameters)
        {
            ValidateNotNull(parameters, "parameters");
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                ValidateStringNotNullNotEmptyArg(pair.Key, "Key in parameters collection");
                ValidateNotNullArg(pair.Value, "Value in parameters collection");
            }
        }
    }
}
