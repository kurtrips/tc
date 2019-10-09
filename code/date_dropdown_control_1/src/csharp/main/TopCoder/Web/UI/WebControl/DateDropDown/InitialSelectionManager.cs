// InitialSelectionManager.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Web.UI.WebControls;
using System.Globalization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>The class for managing initial selection of the list-based web-control. The user should provide the values of
    /// the initial selection properties and the collection of the list items. The class provides the SelectItems(...)
    /// method, which will examine the data of initial selection properties and the list of items, and next will
    /// produce a correct initial selection. </para>
    /// </summary>
    /// <threadsafety>
    /// <para>This class is not thread-safe. It is mutable and
    /// will be used by web-controls, which do not execute additional threads. </para>
    /// </threadsafety>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class InitialSelectionManager
    {
        /// <summary>
        /// <para>Represents the "Select:ClosestToday" initial selection rules string. It will be used for parsing
        /// InitialSelection property. This field is directly initialized and never changes later.
        /// It can not be null or empty string.
        /// </para>
        /// </summary>
        public const string SelectClosestToday = "Select:ClosestToday";

        /// <summary>
        /// <para>Represents the "Select:ClosestNotBeforeToday" initial selection rules string. It will be used for parsing
        /// InitialSelection property. This field is directly initialized and never changes later.
        /// It can not be null or empty string.
        /// </para>
        /// </summary>
        public const string SelectClosestNotBeforeToday = "Select:ClosestNotBeforeToday";

        /// <summary>
        /// <para>Represents the "Select:ClosestNotBeforeTimeStamp" initial selection rules string. It will be used for
        /// parsing InitialSelection property. This field is directly initialized and never changes later.
        /// It can not be null or empty string.  </para>
        /// </summary>
        public const string SelectClosestNotBeforeTimeStamp = "Select:ClosestNotBeforeTimeStamp";

        /// <summary>
        /// <para>Represents the "Select:ClosestNotAfterToday" initial selection rules string. It will be used for parsing
        /// InitialSelection property. This field is directly initialized and never changes later.
        /// It can not be null or empty string.
        /// </para>
        /// </summary>
        public const string SelectClosestNotAfterToday = "Select:ClosestNotAfterToday";

        /// <summary>
        /// <para>Represents the "Select:ClosestNotAfterTimeStamp" initial selection rules string. It will be used for
        /// parsing InitialSelection property.
        /// This field is directly initialized and never changes later. It can not be null or empty string.  </para>
        /// </summary>
        public const string SelectClosestNotAfterTimeStamp = "Select:ClosestNotAfterTimeStamp";

        /// <summary>
        /// <para>Represents the "Select:FirstListItem" initial selection rules string. It will be used for parsing
        /// InitialSelection property.
        /// This field is directly initialized and never changes later. It can not be null or empty string.
        /// </para>
        /// </summary>
        public const string SelectFirstListItem = "Select:FirstListItem";

        /// <summary>
        /// <para>Represents the "Select:LastListItem" initial selection rules string. It will be used for parsing
        /// InitialSelection property.
        /// This field is directly initialized and never changes later. It can not be null or empty string.
        /// </para>
        /// </summary>
        public const string SelectLastListItem = "Select:LastListItem";

        /// <summary>
        /// This is a possible value of the isBeforeOrAfter parameter of PerformInitialSelectionForRule.
        /// It indicates the selection type of 'NotBefore'
        /// </summary>
        private const int FilterNotBefore = 0;

        /// <summary>
        /// This is a possible value of the isBeforeOrAfter parameter of PerformInitialSelectionForRule.
        /// It indicates the selection type of 'NotAfter'
        /// </summary>
        private const int FilterNotAfter = 1;

        /// <summary>
        /// This is a possible value of the isBeforeOrAfter parameter of PerformInitialSelectionForRule.
        /// It indicates the 'any' selection type.
        /// </summary>
        private const int FilterNone = 2;

        /// <summary>
        /// <para>Represents the initial selection string, which can be a simple date/time or any rule from the SelectXXX
        /// static fields of this class.
        /// Use the related property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string initialSelection = String.Empty;

        /// <summary>
        /// <para>Represents the time stamp (date/time) for several initial selection strings
        /// (SelectClosestNotBeforeTimeStamp, SelectClosestNotAfterTimeStamp).
        /// This field is directly initialized. Use the related
        /// property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string timeStamp = String.Empty;

        /// <summary>
        /// <para>Represents the formatting string for dates and times placed in this.InitialSeLection and this.TimeStamp
        /// properties. The standard .NET date/time formatting string format is used for this field value.
        /// Use the related property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string dateFormat = String.Empty;

        /// <summary>
        /// <para>Represents the getter/setter property for the Initial Selection.
        /// This can be either a one of SelectXXX constant fields specified in this class or can
        /// be a direct date string or can also be a combination of the above 2 types separated by semicolon.
        /// </para>
        /// </summary>
        /// <value>This can be either a one of SelectXXX constant fields specified in this class or can
        /// be a direct date string or can also be a combination of the above 2 types separated by semicolon.</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        public string InitialSelection
        {
            get
            {
                return initialSelection;
            }
            set
            {
                Helper.ValidateNotNull(value, "InitialSelection");
                initialSelection = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the TimeStamp. This must be a date string with time info.</para>
        /// </summary>
        /// <value>This must be a date string with time info.</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        public string TimeStamp
        {
            get
            {
                return timeStamp;
            }
            set
            {
                Helper.ValidateNotNull(value, "TimeStamp");
                timeStamp = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the Date Format.
        /// This can be any .NET standard date format string.</para>
        /// </summary>
        /// <value>This can be any .NET standard date format string.</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        public string DateFormat
        {
            get
            {
                return dateFormat;
            }
            set
            {
                Helper.ValidateNotNull(value, "DateFormat");
                dateFormat = value;
            }
        }

        /// <summary>
        /// <para>The default constructor</para>
        /// </summary>
        public InitialSelectionManager()
        {
        }

        /// <summary>
        /// <para>A simple initializing constructor with one argument (initialSelection).</para>
        /// </summary>
        /// <param name="initialSelection">
        /// the data value for InitialSelection property of this class. Represents the initial selection string, which
        /// can be a simple date/time or any rule from the SelectXXX static fields of this class. Can not be null, but
        /// can be empty string.
        /// </param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        public InitialSelectionManager(string initialSelection)
            : this(initialSelection, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// <para>A simple initializing constructor with two arguments (initialSelection, dateFormat).</para>
        /// </summary>
        /// <param name="initialSelection">
        /// the data value for InitialSelection property of this class. Represents the initial selection string, which
        /// can be a simple date/time or any rule from the SelectXXX static fields of this class. Can not be null, but
        /// can be empty string.
        /// </param>
        /// <param name="dateFormat">
        /// the data value for DateFormat property of this class. Represents the formatting string for dates and times
        /// placed in this.InitialSeLection and this.TimeStamp properties. The standard .NET date/time formatting string
        /// format is used for this  argument value. Can not be null, but can be empty string.
        /// </param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        public InitialSelectionManager(string initialSelection, string dateFormat)
            : this(initialSelection, String.Empty, dateFormat)
        {
        }

        /// <summary>
        /// <para>A simple initializing constructor with three arguments (initialSelection, timeStamp, dateFormat)</para>
        /// </summary>
        /// <param name="initialSelection">
        /// the data value for InitialSelection property of this class. Represents the initial selection string, which
        /// can be a simple date/time or any rule from the SelectXXX static fields of this class. Can not be null, but
        /// can be empty string.
        /// </param>
        /// <param name="timeStamp">
        /// the data value for TimeStamp property of this class. Represents the time stamp (date/time) for several
        /// initial selection strings (SelectClosestNotBeforeTimeStamp, SelectClosestNotAfterTimeStamp). Can not be
        /// null, but can be empty string.
        /// </param>
        /// <param name="dateFormat">
        /// the data value for DateFormat property of this class. Represents the formatting string for dates and times
        /// placed in this.InitialSeLection and this.TimeStamp properties. The standard .NET date/time formatting string
        /// format is used for this  argument value. Can not be null, but can be empty string.
        /// </param>
        /// <exception cref="ArgumentNullException">if any argument is null.</exception>
        public InitialSelectionManager(string initialSelection, string timeStamp, string dateFormat)
        {
            InitialSelection = initialSelection;
            DateFormat = dateFormat;
            TimeStamp = timeStamp;
        }

        /// <summary>
        /// <para>Selects the item(s) of the provided ListItemCollection in accordance with the currently set initial
        /// selection properties (selection rule).</para>
        /// </summary>
        /// <param name="listItems">
        /// an instance of the ListItemCollection, which contains the items of the internal list of this control. One
        /// (or several) item will be selected in accordance with the selection rule set to this class. The argument can
        /// not be null. No collection  element can be null. The collection can be empty.
        /// </param>
        /// <param name="allowMultipleSelection">
        /// the control flag, which allows multiple/single selection modes. true value means that the multiple selection
        /// is allowed for the listItems, false - only single selection is allowed.
        /// </param>
        /// <param name="displayDateFormat">The display format of the list item dates.</param>
        /// <returns>
        /// the number of the selected elements from the internal items list of this control. Can be positive number
        /// (including 0).
        /// </returns>
        /// <exception cref="ArgumentNullException">if listItems is null</exception>
        /// <exception cref="ArgumentException">if listItems contains empty elements.</exception>
        /// <exception cref="InitialSelectionInvalidDataException">
        /// if the current set of initial selection properties is invalid
        /// (absent data for requried properties, invalid rule, etc.).
        /// </exception>
        public int SelectItems(ListItemCollection listItems, bool allowMultipleSelection, string displayDateFormat)
        {
            Helper.ValidateNotNull(listItems, "listItems");

            //Return if some item is already selected
            foreach (ListItem item in listItems)
            {
                if (item == null)
                {
                    throw new ArgumentException("ListCollection must not contain null elements.", "listItems");
                }

                if (item.Selected)
                {
                    return 0;
                }
            }

            int selectionsMade = 0;
            string[] initialSelections = InitialSelection.Split(';');

            //For each ';' delimited part of the InitialSelection string
            foreach (string initialSelection in initialSelections)
            {
                switch (initialSelection)
                {
                    case SelectFirstListItem:
                    {
                        //atleast 1 items in the list must be present
                        if (listItems.Count != 0)
                        {
                            listItems[0].Selected = true;
                            selectionsMade++;
                        }
                        break;
                    }
                    case SelectLastListItem:
                    {
                        //atleast 1 items in the list must be present
                        if (listItems.Count != 0)
                        {
                            listItems[listItems.Count - 1].Selected = true;
                            selectionsMade++;
                        }
                        break;
                    }
                    case SelectClosestToday:
                    {
                        selectionsMade += PerformInitialSelectionForRule(listItems, FilterNone, false,
                            displayDateFormat);
                        break;
                    }
                    case SelectClosestNotAfterToday:
                    {
                        selectionsMade += PerformInitialSelectionForRule(listItems, FilterNotAfter, false,
                            displayDateFormat);
                        break;
                    }
                    case SelectClosestNotBeforeToday:
                    {
                        selectionsMade += PerformInitialSelectionForRule(listItems, FilterNotBefore, false,
                            displayDateFormat);
                        break;
                    }
                    case SelectClosestNotAfterTimeStamp:
                    {
                        selectionsMade += PerformInitialSelectionForRule(listItems, FilterNotAfter, true,
                            displayDateFormat);
                        break;
                    }
                    case SelectClosestNotBeforeTimeStamp:
                    {
                        selectionsMade += PerformInitialSelectionForRule(listItems, FilterNotBefore, true,
                            displayDateFormat);
                        break;
                    }
                    default:
                    {
                        selectionsMade += PerformInitialSelectionForDate(listItems, initialSelection,
                            displayDateFormat);
                        break;
                    }
                }

                //Break if multiple selections are not allowed.
                if (!allowMultipleSelection && selectionsMade == 1)
                {
                    return 1;
                }
            }

            return selectionsMade;
        }

        /// <summary>
        /// Selects an item in the ListItemCollection where the date specified is in the date format.
        /// </summary>
        /// <param name="listItems">the ListItemCollection in which to perform selection</param>
        /// <param name="dateString">The date string</param>
        /// <param name="displayDateFormat">The display format of the list item dates.</param>
        /// <returns>
        /// The number of items selected. This is either 0 or 1.
        /// </returns>
        private int PerformInitialSelectionForDate(ListItemCollection listItems, string dateString,
            string displayDateFormat)
        {
            //Throw error if InitialSelection is empty string and list contains some items.
            if (InitialSelection.Trim().Equals(String.Empty) && listItems.Count > 0)
            {
                throw new InitialSelectionInvalidDataException("InitialSelection can not be an empty string.");
            }

            //Try each list item for each date
            foreach (ListItem item in listItems)
            {
                DateTime itemDate, selectDate;
                try
                {
                    itemDate = DateTime.ParseExact(item.Value, displayDateFormat, CultureInfo.InvariantCulture);
                    selectDate = DateTime.ParseExact(dateString.Trim(), DateFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    throw new InitialSelectionInvalidDataException("Could not convert date string to date.", e);
                }

                //Item must not be selected from before
                if (!item.Selected && itemDate == selectDate)
                {
                    item.Selected = true;
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Selects an item in the ListItemCollection where the date specified is in the SelectXXX format.
        /// </summary>
        /// <param name="listItems">the ListItemCollection in which to perform selection</param>
        /// <param name="isBeforeOrAfter">
        /// 0 indicates the selection type of 'NotBefore'
        /// 1 indicates the selection type of 'NotAfter'
        /// 2 indicates any type
        /// </param>
        /// <param name="isTimestampOrToday">
        /// Whether to compare the list dates to the TimeStamp date or to compare them to today's date.
        /// </param>
        /// <param name="displayDateFormat">The display format of the list item dates.</param>
        /// <returns>The number of selections made. This is either 0 or 1.</returns>
        private int PerformInitialSelectionForRule(ListItemCollection listItems, int isBeforeOrAfter,
            bool isTimestampOrToday, string displayDateFormat)
        {
            Helper.ValidateNotNull(listItems, "listItems");

            //The date to compare to (now or today)
            DateTime comparee;
            ListItem selection = null;
            long min = long.MaxValue;

            //Use timestamp for slections with timestamp
            if (isTimestampOrToday)
            {
                try
                {
                    comparee = DateTime.ParseExact(TimeStamp, DateFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    throw new InitialSelectionInvalidDataException("Could not convert " + TimeStamp + " to date.", e);
                }
            }
            //Use today without timestamp
            else
            {
                comparee = DateTime.Today;
            }

            //Search for item according to rule
            foreach (ListItem item in listItems)
            {
                //The date of the list item
                DateTime itemDate;
                try
                {
                    itemDate = DateTime.ParseExact(item.Value, displayDateFormat, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    throw new InitialSelectionInvalidDataException("Could not convert " + item.Value + " to date.", e);
                }

                //Get the distance between the 2 dates
                long deviation = itemDate.Ticks - comparee.Ticks;

                //NotBefore
                if (isBeforeOrAfter == FilterNotBefore)
                {
                    //The item date is before the comparee date, so continue.
                    if (deviation < 0)
                    {
                        continue;
                    }
                }
                //NotAfter
                else if (isBeforeOrAfter == FilterNotAfter)
                {
                    //The item date is after the comparee date, so continue.
                    if (deviation > 0)
                    {
                        continue;
                    }
                }
                deviation = Math.Abs(deviation);

                //If closest then store the minimum found and the corresponding item
                if (deviation < min)
                {
                    min = deviation;
                    selection = item;
                }
            }

            //No item was found
            if (selection == null)
            {
                return 0;
            }

            //Select the found value only if not already selected.
            if (!selection.Selected)
            {
                selection.Selected = true;
                return 1;
            }
            return 0;
        }
    }
}
