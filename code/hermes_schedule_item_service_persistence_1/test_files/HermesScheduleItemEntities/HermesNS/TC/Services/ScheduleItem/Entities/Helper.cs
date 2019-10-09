// Helper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Entity.Validation;

namespace HermesNS.TC.Services.ScheduleItem.Entities
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
        /// <exception cref="ArgumentNullException">If object is null</exception>
        public static bool ValidateNotNull(object obj, string name, bool throwError)
        {
            if (obj == null && throwError)
            {
                throw new ArgumentNullException(name, name + " must not be null.");
            }
            return obj != null;
        }

        /// <summary>
        /// Checks whether an string is empty or not (after trimming).
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentException">If string is empty</exception>
        public static bool ValidateNotEmpty(string str, string name, bool throwError)
        {
            if (str!= null && str.Trim().Equals(String.Empty) && throwError)
            {
                throw new ArgumentException(name + " must not be empty.", name);
            }
            return !str.Trim().Equals(string.Empty);
        }

        /// <summary>
        /// Checks whether an string is neither empty nor null.
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <param name="name">The name of the string</param>
        /// <exception cref="ArgumentException">If string is empty</exception>
        /// <exception cref="ArgumentNullException">If string is null</exception>
        public static bool ValidateNotNullNotEmpty(string str, string name, bool throwError)
        {
            return ValidateNotNull(str, name, throwError) && ValidateNotEmpty(str, name, throwError);
        }

        /// <summary>
        /// Validates a string array. Checks if it is not null, not empty and does not contain null or empty elements.
        /// </summary>
        /// <param name="arr">The string array.</param>
        /// <param name="name">The name of the array</param>
        /// <param name="checkNull">Whether to check if array is null.</param>
        /// <param name="checkEmpty">Whether to check if array is empty.</param>
        /// <exception cref="ArgumentException">If array is empty and empty is not allowed or
        /// if it contains empty or null elements</exception>
        /// <exception cref="ArgumentNullException">If array is null and null is not allowed</exception>
        public static void ValidateArray(string[] arr, string name, bool checkNull, bool checkEmpty)
        {
            //Check for null.
            if (checkNull)
            {
                ValidateNotNull(arr, name, true);
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
                if (arr.GetValue(i) == null)
                {
                    throw new ArgumentException("Element of " + name + " at index " + i,
                        "Element of " + name + " at index " + i + " must not be null.");
                }
                ValidateNotEmpty(arr.GetValue(i) as string, "Element of " + name + " at index " + i, true);
            }
        }

        /// <summary>
        /// Validates a Dictionary of key as string and value as ISecurityLookupService.
        /// Checks if it is not null, not empty, does not contain empty keys or null elements.
        /// </summary>
        /// <param name="dic">The Dictionary</param>
        /// <param name="name">The name of the Dictionary</param>
        /// <exception cref="ArgumentException">If Dictionary is empty or
        /// if it contains empty keys or null elements</exception>
        /// <exception cref="ArgumentNullException">If Dictionary is null</exception>
        public static void ValidateDictionary(IDictionary<string, string> dic, string name)
        {
            ValidateNotNull(dic, name, true);

            if (dic.Count == 0)
            {
                throw new ArgumentException("Dictionary must not be empty.", name);
            }

            IEnumerator<KeyValuePair<string, string>> en = dic.GetEnumerator();
            while (en.MoveNext())
            {
                //NOTE: Key can never be null.
                //Check empty key
                ValidateNotEmpty(en.Current.Key, "Dictionary key of " + name, true);

                //Check null value
                if (en.Current.Value == null)
                {
                    throw new ArgumentException("Dictionary value must not be null.", name);
                }
            }
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
        public static SelfDocumentingException GetSelfDocumentingException(
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
                //This is a bug with SDE component. For ex. an XmlDocument instance cant be added.
                try
                {
                    ms.AddInstanceVariable(instanceVarsNames[i], instanceVars[i]);
                }
                catch { }
            }
            for (int i = 0; i < parameterVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. For ex. an XmlDocument instance cant be added.
                try
                {
                    ms.AddMethodParameter(parameterVarsNames[i], parameterVars[i]);
                }
                catch { }
            }
            for (int i = 0; i < localVarsNames.Length; i++)
            {
                //Ignore if unable to add object to MethodState.
                //This is a bug with SDE component. For ex. an XmlDocument instance cant be added.
                try
                {
                    ms.AddLocalVariable(localVarsNames[i], localVars[i]);
                }
                catch { }
            }

            ms.Lock();
            return sde;
        }

        public static DataValidationRecord CreateDataValidationRecord(
            string objectId, string entityFullName, string validatedPropertyName,
            string validationFormatStringResourceName, object[] validationFormatStringData)
        {
            DataValidationRecord dvr = new DataValidationRecord();

            dvr.ObjectId = objectId;
            dvr.EntityFullName = entityFullName;
            dvr.ValidatedPropertyName = validatedPropertyName;
            dvr.ValidationFormatStringResourceName = validationFormatStringResourceName;
            if (validationFormatStringData != null)
            {
                dvr.ValidationFormatStringData = validationFormatStringData;
            }

            return dvr;
        }

        public static bool IsNumeric(Type type)
        {
            if (type.Equals(typeof(int)) || type.Equals(typeof(double)) || type.Equals(typeof(decimal))
                || type.Equals(typeof(short)) || type.Equals(typeof(long)) || type.Equals(typeof(float))
                || type.Equals(typeof(Int16)) || type.Equals(typeof(Int64)) || type.Equals(typeof(UInt16))
                || type.Equals(typeof(UInt32)) || type.Equals(typeof(UInt64)) || type.Equals(typeof(ulong))
                || type.Equals(typeof(ushort)))
            {
                return true;
            }

            return false;
        }
    }
}
