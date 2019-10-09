// Helper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.IO;
using System.Xml.Schema;
using System.Reflection;

namespace TopCoder.CodeDoc.CSharp
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
        /// The namespace to use for writing global classes.
        /// </summary>
        public const string GLOBALNS = "__global__";

        /// <summary>
        /// Checks whether an object is null or not
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <param name="name">The name of the object</param>
        /// <exception cref="ArgumentNullException">If object is null</exception>
        public static void ValidateNotNull(object obj, string name)
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
        public static void ValidateNotEmpty(string str, string name)
        {
            if (str != null && str.Trim().Equals(String.Empty))
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
        public static void ValidateNotNullNotEmpty(string str, string name)
        {
            ValidateNotNull(str, name);
            ValidateNotEmpty(str, name);
        }

        /// <summary>
        /// Validates an array. Checks if it is not null, not empty and does not contain null or empty elements.
        /// </summary>
        /// <param name="arr">The array.</param>
        /// <param name="name">The name of the array</param>
        /// <param name="checkNull">Whether to check if array is null.</param>
        /// <param name="checkEmpty">Whether to check if array is empty.</param>
        /// <param name="checkEmptyElement">If it is string array then whether to check for empty elements</param>
        /// <param name="checkNullElement">Whether to check for null elements.</param>
        /// <exception cref="ArgumentException">If array is empty and empty is not allowed OR
        /// If it contains empty (only for string arrays) or null elements if they are not allowed. OR
        /// If array is null and null is not allowed</exception>
        public static void ValidateArray(Array arr, string name, bool checkNull, bool checkEmpty,
            bool checkNullElement, bool checkEmptyElement)
        {
            //Check for null.
            if (checkNull)
            {
                ValidateNotNull(arr, name);
            }
            else
            {
                //Return if it is null
                if (arr == null)
                {
                    return;
                }
            }

            //Check array length
            if (checkEmpty)
            {
                if (arr.Length == 0)
                {
                    throw new ArgumentException(name + " must not be empty array.", name);
                }
            }

            //Check array elements for null and empty.
            for (int i = 0; i < arr.Length; i++)
            {
                if (checkNullElement)
                {
                    if (arr.GetValue(i) == null)
                    {
                        throw new ArgumentException("Element of " + name + " at index " + i + " must not be null.",
                            "Element of " + name + " at index " + i);
                    }
                }

                if (arr.GetType() == typeof(string[]) && checkEmptyElement)
                {
                    ValidateNotEmpty((string)arr.GetValue(i), "Element of " + name + " at index " + i);
                }
            }
        }

        /// <summary>
        /// Returns an xpath predicate string for selecting a child (of the context node )
        /// with name 'nodeName' and value 'value'
        /// </summary>
        /// <param name="nodeName">The child node name to find</param>
        /// <param name="value">The child node value to find</param>
        /// <returns>The xpath predicate string so formed</returns>
        public static string GetXpathPredicate(string nodeName, string value)
        {
            return "[" + nodeName + "='" + value + "']";
        }

        /// <summary>
        /// Returns an xpath predicate string for selecting an attribute (of the context node )
        /// with name 'attrName' and value 'attrValue'
        /// </summary>
        /// <param name="attrName">The attribute name to find.</param>
        /// <param name="attrValue">The attribute value to find.</param>
        /// <returns>The xpath predicate string so formed</returns>
        public static string GetXpathAttributePredicate(string attrName, string attrValue)
        {
            return "[@" + attrName + "='" + attrValue + "']";
        }

        /// <summary>
        /// Checks whether a given type is a delegate.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns>True if delegate, false otherwise</returns>
        public static bool IsDelegate(Type type)
        {
            return (type.IsClass && !type.IsValueType &&
                (type.BaseType == typeof(Delegate) || type.BaseType == typeof(MulticastDelegate)));
        }

        /// <summary>
        /// Checks whether the given method is an indexer
        /// </summary>
        /// <param name="methodInfo">The method info to check</param>
        /// <returns>true if indexer, false otherwise</returns>
        public static bool IsIndexer(MethodBase methodInfo)
        {
            return (methodInfo.IsSpecialName &&
                (methodInfo.Name.StartsWith("get_Item") || methodInfo.Name.StartsWith("set_Item")));
        }

        /// <summary>
        /// Gets the binding flags for the getting properties, methods etc.
        /// </summary>
        /// <param name="privateAllowed">
        /// Whether private properties, methods etc. should be included in the search.
        /// </param>
        /// <returns>The binding flags for the getting properties, methods etc.</returns>
        public static BindingFlags GetBindingFlags(bool privateAllowed)
        {
            BindingFlags bindingFlags =
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            if (privateAllowed)
            {
                bindingFlags |= BindingFlags.NonPublic;
            }
            return bindingFlags;
        }

        /// <summary>
        /// Variable for holding the apispec schema information.
        /// </summary>
        private static XmlSchema apiSpecXsd = null;

        /// <summary>
        /// Gets the apispec schema information.
        /// </summary>
        /// <returns>The XmlSchema instance containing the apispec schema information.</returns>
        public static XmlSchema GetApispecSchema()
        {
            if (apiSpecXsd == null)
            {
                //Load schema from resource file
                string[] ress = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                if (ress == null || ress.Length != 1)
                {
                    throw new ApplicationException("Assembly must be built with the apispec.xsd as embedded resource.");
                }
                Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ress[0]);
                apiSpecXsd = XmlSchema.Read(schemaStream, null);
                apiSpecXsd.Compile(null);
            }
            return apiSpecXsd;
        }
    }
}
