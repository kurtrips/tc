/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Xml;

namespace TopCoder.Web.Controls.XmlViewer
{
    /// <summary>
    /// <para>Defines helper methods used by this component. These methods help improve readability
    /// of the code and reduce code redundancy.</para>
    /// <para>Thread Safety: This class conatins only static methods and is thus thread safe.</para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class HelperClass
    {
        /// <summary>
        /// <para>Private constructor so that instance of this class cannot be created.</para>
        /// </summary>
        private HelperClass()
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
        /// Checks if xml string is well formed or not.
        /// </summary>
        /// <param name="xml">The xml string</param>
        /// <param name="name">The name of the parameter which is the xml string</param>
        /// <exception cref="InvalidXmlException">If xml is not well formed.</exception>
        public static void ValidateWellFormedXml(string xml, string name)
        {
            XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Document, null);
            try
            {
                while (reader.Read()) ;
            }
            catch (Exception e)
            {
                throw new InvalidXmlException(name + " is not well-formed xml. See inner exception for details.", e);
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Validates an integer. The integer must be non negative.
        /// </summary>
        /// <param name="value">The ineteger to be validated</param>
        /// <param name="name">The parameter name of the integer to be validated</param>
        /// <exception cref="ArgumentException">If value is negative</exception>
        public static void ValidateNonNegative(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentException(name + " must be a non-negative number.");
            }
        }
    }
}
