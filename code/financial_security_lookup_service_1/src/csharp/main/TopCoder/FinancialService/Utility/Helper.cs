// Helper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Util.ExceptionManager.SDE;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// <para>
    /// This class exposes static helper functions which helps improve code readability and reduces code redundancy
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
                if (arr.GetValue(i) == null)
                {
                    throw new ArgumentException("Element of " + name + " at index " + i + " must not be null.",
                        "Element of " + name + " at index " + i);
                }
                ValidateNotEmpty(arr.GetValue(i) as string, "Element of " + name + " at index " + i);
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
        public static void ValidateDictionary(IDictionary<string, ISecurityLookupService> dic, string name)
        {
            ValidateNotNull(dic, name);

            if (dic.Count == 0)
            {
                throw new ArgumentException("Dictionary must not be empty.", name);
            }

            IEnumerator<KeyValuePair<string, ISecurityLookupService>> en = dic.GetEnumerator();
            while (en.MoveNext())
            {
                //NOTE: Key can never be null.
                //Check empty key
                ValidateNotEmpty(en.Current.Key, "Dictionary key of " + name);

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

        /// <summary>
        /// Calculates the check digit for an 'isin id without its check digit'
        /// </summary>
        /// <param name="isinWithoutCheckDigit">isin id without its check digit</param>
        /// <returns>The calculated check digit.</returns>
        public static char GetCheckDigitForIsin(string isinWithoutCheckDigit)
        {
            //Stores the individual digits.
            IList<int> indDigits = new List<int>();

            //Create list of digits for calcualating check digit
            for (int i = 0; i < isinWithoutCheckDigit.Length; i++)
            {
                //Get the numeric representation for the character
                int numVal;
                if (char.IsDigit(isinWithoutCheckDigit[i]))
                {
                    numVal = isinWithoutCheckDigit[i] - '0';
                }
                else
                {
                    numVal = isinWithoutCheckDigit[i] - 'A' + 10;
                }

                //Add individual digits of the numeric representation.
                //For ex. Q = 26 gets added as 2 and 6
                if (numVal / 10 > 0)
                {
                    indDigits.Add(numVal / 10);
                }
                indDigits.Add(numVal % 10);
            }

            //Multiply alternate digits in the list starting from right with 2
            bool multiply = false;
            int sum = 0;
            for (int i = indDigits.Count - 1; i >= 0; i--)
            {
                multiply = !multiply;
                indDigits[i] *= (multiply == true) ? 2 : 1;
                sum += (indDigits[i] / 10) + (indDigits[i] % 10);
            }

            //Calculate check digit
            sum = sum % 10;
            sum = 10 - sum;
            sum = sum % 10;

            return Convert.ToChar('0' + sum);
        }
    }
}
