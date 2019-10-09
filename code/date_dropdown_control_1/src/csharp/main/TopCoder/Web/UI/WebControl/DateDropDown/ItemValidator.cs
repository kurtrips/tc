// ItemValidator.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Globalization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>This is a simple validator for date items. It checks whether the given items in the correct user provided
    /// format. The format is defined by a standard .NET formatting date/time string. And the overloaded "validate"
    /// method works with  several input data types (like string[], IList{string}, etc.). </para>
    /// </summary>
    /// <threadsafety>
    /// <para>This class is thread-safe, because it is immutable and no referenced data is exposed. </para>
    /// </threadsafety>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class ItemValidator
    {
        /// <summary>
        /// <para>Represents the date/time formatting string for all the user provided date and times in the list items to
        /// be validated. So, this is some kind of validating string for the given list items. The standard .NET
        /// date/time formatting string format is  used for this field value. Initialized in the constructor and never
        /// changed later. It can not be null or empty string.</para>
        /// </summary>
        private readonly string dateFormat;

        /// <summary>
        /// <para>A simple initializing constructor.</para>
        /// </summary>
        /// <param name="dateFormat">
        /// the date/time formatting string (in the standard .NET format for date/times). Can not be null or empty
        /// string.
        /// </param>
        /// <exception cref="ArgumentNullException">If dateFormat is null.</exception>
        /// <exception cref="ArgumentException">If dateFormat is empty.</exception>
        public ItemValidator(string dateFormat)
        {
            Helper.ValidateNotNullNotEmpty(dateFormat, "dateFormat");
            this.dateFormat = dateFormat;
        }

        /// <summary>
        /// <para>Validate the date/time items from the input IEnumeration to have a correct type (DateTime or string), and
        /// a correct format (for string data type only). The function returns boolean validation result (true -
        /// success, false - fail).</para>
        /// </summary>
        /// <param name="dataSource">
        /// an instance of the IEnumerable. It contains the date/time elements to be validated. Can not be null. Should
        /// not contain null elements. Can be empty collection. An empty string value is allowed for collection
        /// elements.
        /// </param>
        /// <returns>
        /// the result of validation. true value means the successful validation, false - validation failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">If dataSource is null.</exception>
        /// <exception cref="ArgumentException">If dataSource has null elements</exception>
        public bool Validate(IEnumerable dataSource)
        {
            //Check for null
            Helper.ValidateNotNull(dataSource, "dataSource");

            //Validate
            return ValidateUsingEnumerator(dataSource.GetEnumerator(), "dataSource");
        }

        /// <summary>
        /// <para>Validate the date/time items from the input ListItemCollection to have a correct format. The function
        /// returns boolean validation result (true - success, false - fail).</para>
        /// </summary>
        /// <param name="items">
        /// an instance of the ListItemCollection. It contains the date/time elements to be validated. Can not be null.
        /// Should not contain null elements. Can be empty collection. An empty string value is allowed for collection
        /// elements.
        /// </param>
        /// <returns>
        /// the result of validation. true value means the successful validation, false - validation failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">If items is null.</exception>
        /// <exception cref="ArgumentException">If items has null elements</exception>
        public bool Validate(ListItemCollection items)
        {
            //Check for null
            Helper.ValidateNotNull(items, "items");

            //Validate
            return ValidateUsingEnumerator(items.GetEnumerator(), "items");
        }

        /// <summary>
        /// <para>Validate the date/time items from the input string[] to have a correct format. The function returns
        /// boolean validation result (true - success, false - fail).</para>
        /// </summary>
        /// <param name="items">
        /// an array of strings. It contains the date/time elements to be validated. Can not be null. Should not contain
        /// null elements. Can be empty array. An empty string value is allowed for array elements.
        /// </param>
        /// <returns>
        /// the result of validation. true value means the successful validation, false - validation failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">If items is null.</exception>
        /// <exception cref="ArgumentException">If items has null elements</exception>
        public bool Validate(string[] items)
        {
            //Check for null
            Helper.ValidateNotNull(items, "items");

            //Validate
            return ValidateUsingEnumerator(items.GetEnumerator(), "items");
        }

        /// <summary>
        /// <para>Validate the date/time items from the input IList&lt;string&gt; to have a correct format.
        /// The function returns boolean validation result (true - success, false - fail).</para>
        /// </summary>
        /// <param name="items">
        /// an instance of the IList&lt;string&gt;. It contains the date/time elements to be validated. Can not be null.
        /// Should not contain null elements. An empty string value is allowed for collection elements.
        /// </param>
        /// <returns>
        /// the result of validation. true value means the successful validation, false - validation failed.
        /// </returns>
        /// <exception cref="ArgumentNullException">If items is null.</exception>
        /// <exception cref="ArgumentException">If items has null elements</exception>
        public bool Validate(IList<string> items)
        {
            //Check for null
            Helper.ValidateNotNull(items, "items");

            //Validate
            return ValidateUsingEnumerator(items.GetEnumerator(), "items");
        }

        /// <summary>
        /// Validates using the enumerator of a collection or array whether the elements are valid dates.
        /// </summary>
        /// <param name="en">The collection or array enumerator</param>
        /// <param name="collectionName">The name of the collection or array</param>
        /// <returns>True if all elements of the collection are valid dates, false otherwise</returns>
        /// <exception cref="ArgumentException">
        /// If the collection contains null elements.
        /// </exception>
        private bool ValidateUsingEnumerator(IEnumerator en, string collectionName)
        {
            //Check null elements first.
            while (en.MoveNext())
            {
                //Null elements are not allowed
                if (en.Current == null)
                {
                    throw new ArgumentException(collectionName + " must not contain null elements.", collectionName);
                }
            }
            en.Reset();

            //Validate
            while (en.MoveNext())
            {
                //string are ok as long as they can be parsed to a DateTime
                if (en.Current is string)
                {
                    try
                    {
                        DateTime.ParseExact(en.Current as string, dateFormat, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return false;
                    }
                }
                //DateTime elements are always ok
                else if (en.Current is DateTime)
                {
                    continue;
                }
                //An item in a ListItemCollection
                else if (en.Current is ListItem)
                {
                    try
                    {
                        DateTime.ParseExact((en.Current as ListItem).Value, dateFormat, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        return false;
                    }
                }
                //No other format is allowed
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}