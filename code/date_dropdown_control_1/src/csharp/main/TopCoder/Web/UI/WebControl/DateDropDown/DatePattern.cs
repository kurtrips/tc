// DatePattern.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>The class representing a custom DatePattern property for the web-control. The abstraction of date generation
    /// pattern is implemented by this class. It contains several properties (InputDateFormat, DisplayDateFormat,
    /// StartDate,  StopDate), which serves as sub-propertis of this custom property class. An additional Rules property
    /// of this class is a collection of inner Rule-based items. So, the DatePattern custom property class contains
    /// several simple sub-properties  and a collection sub-property. A special method (generateDates()) will produce an
    /// array of generated date items (for each rule), which can be directly reused by any list-based web-control. </para>
    /// </summary>
    /// <threadsafety>
    /// This class is not thread-safe. It is mutable and will be used by
    /// web-controls, which do not execute additional threads.
    /// </threadsafety>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TypeConverter(typeof(DatePatternConverter))]
    [DefaultProperty("Rules")]
    [ParseChildren(true, "Rules")]
    public class DatePattern : Control
    {
        /// <summary>
        /// <para>Represents the date/time formatting string for all the user provided date and times for the date pattern
        /// (including rules). The standard .NET date/time formatting string format is used for this field value.
        /// Directly initialized. Use the related property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string inputDateFormat = String.Empty;

        /// <summary>
        /// <para>Represents the date/time formatting string for the output date/time items generated in this class. The
        /// standard .NET date/time formatting string format is used for this field value.  Directly initialized. Use
        /// the related property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string displayDateFormat = String.Empty;

        /// <summary>
        /// <para>Represents the starting date/time value for generating date/time items in this class. The format of this
        /// field value is determined by the this.InputDateFormat property.  Directly initialized. Use the related
        /// property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string startDate = String.Empty;

        /// <summary>
        /// <para>Represents the ending date/time value for generating date/time items in this class. The format of this
        /// field value is determined by the this.InputDateFormat property.  Directly initialized. Use the related
        /// property to mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string stopDate = String.Empty;

        /// <summary>
        /// <para>Represents the colleciton of the Rule inner properties for this class. It is a collection of date
        /// generation rules for this DatePattern. The field is directly initialized to null and then initialized to an
        /// empty List in the getter property, and  next will never change. The elements of the list can be changed at
        /// any time. The list elements should not be null.</para>
        /// </summary>
        private List<Rule> rules;

        /// <summary>
        /// <para>Represents the control ID to bind to the control. Directly initialized. Use the related property to
        /// mutate. It can not be null, but can be empty string.</para>
        /// </summary>
        private string controlID = String.Empty;

        /// <summary>
        /// <para>Represents the control instance of the parent. It allows a quicker access to the underlying control when
        /// needed. Directly initialized. Use the related property to mutate. It can be any value.</para>
        /// </summary>
        private Control controlInstance;

        /// <summary>
        /// <para>Represents the getter/setter property for the input date format</para>
        /// </summary>
        /// <value>The input date format in which values are specified.</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        [Category("Behavior")]
        [NotifyParentProperty(true)]
        public virtual string InputDateFormat
        {
            get
            {
                return inputDateFormat;
            }
            set
            {
                Helper.ValidateNotNull(value, "InputDateFormat");
                inputDateFormat = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the display date format</para>
        /// </summary>
        /// <value>The date format in which values are displayed.</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        [Category("Behavior")]
        [NotifyParentProperty(true)]
        public virtual string DisplayDateFormat
        {
            get
            {
                return displayDateFormat;
            }
            set
            {
                Helper.ValidateNotNull(value, "DisplayDateFormat");
                displayDateFormat = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the start date.</para>
        /// </summary>
        /// <value>The date from which to start generating dates for pattern</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        [Category("Behavior")]
        [NotifyParentProperty(true)]
        public virtual string StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                Helper.ValidateNotNull(value, "StartDate");
                startDate = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the stop date.</para>
        /// </summary>
        /// <value>The date from which to stop generating dates for pattern</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        [Category("Behavior")]
        [NotifyParentProperty(true)]
        public virtual string StopDate
        {
            get
            {
                return stopDate;
            }
            set
            {
                Helper.ValidateNotNull(value, "StopDate");
                stopDate = value;
            }
        }

        /// <summary>
        /// A collection of rules to apply for the given date pattern.
        /// </summary>
        /// <value>A collection of rules to apply for the given date pattern.</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        public virtual List<Rule> Rules
        {
            get
            {
                if (rules == null)
                {
                    rules = new List<Rule>();
                }
                return rules;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the ID of the DatePattern instance.</para>
        /// </summary>
        /// <value>The ID of the DatePattern instance.</value>
        /// <exception cref="ArgumentNullException">if the value to be set is null.</exception>
        [NotifyParentProperty(true)]
        [Description("The ID of the control to that is bound."),DefaultValue("")]
        [TypeConverter(typeof(ControlIDConverter))]
        public string ControlID
        {
            get
            {
                return controlID;
            }
            set
            {
                Helper.ValidateNotNull(value, "ControlID");
                controlID = value;
            }
        }

        /// <summary>
        /// <para>The control instance of which this instance is the child.</para>
        /// </summary>
        /// <value>The control instance of which this instance is the child.</value>
        [NotifyParentProperty(false)]
        [Description("An instance value for the controls")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Control ControlInstance
        {
            get
            {
                return controlInstance;
            }
            set
            {
                controlInstance = value;
            }
        }

        /// <summary>
        /// <para>The default constructor</para>
        /// </summary>
        public DatePattern()
        {
        }

        /// <summary>
        /// <para>Generates and returns the date/time items as defined by rules of this DatePattern.</para>
        /// </summary>
        /// <returns>
        /// an array of generated date/time items. It can not be null, but can be empty. No array element can be null or
        /// empty string.
        /// </returns>
        /// <exception cref="RuleInvalidDataException">
        /// If there is any problem in generating dates for the given rule like invalid Rule properties etc.
        /// </exception>
        public string[] GenerateDates()
        {
            string[] ret = new string[0];

            //If no rules are present then simply return
            if (Rules.Count == 0)
            {
                return ret;
            }

            //Parse to dates
            DateTime start, end;
            try
            {
                start = DateTime.ParseExact(startDate, inputDateFormat, CultureInfo.InvariantCulture);
                end = DateTime.ParseExact(stopDate, inputDateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new DatePatternInvalidDataException("Could not convert date string to DateTime object.", e);
            }

            //Check startDate is smaller than endDate
            if (start > end)
            {
                throw new DatePatternInvalidDataException("Start date must not be greater than end date.");
            }

            //Generate dates for each rule and store them in an array
            foreach (Rule rule in Rules)
            {
                //Get the particular rule's generated dates.
                string[] ruleDates = rule.GenerateDates(inputDateFormat, displayDateFormat, startDate, stopDate);

                //Resize the array to return.
                Array.Resize<string>(ref ret, ret.Length + ruleDates.Length);

                //Copy the particular rule's generated dates to the return array
                Array.Copy(ruleDates, 0, ret, ret.Length - ruleDates.Length, ruleDates.Length);
            }

            //Get a List<DateTime> containing only unique dates
            List<DateTime> uniqueDates = RemoveDuplicates(ret, displayDateFormat) as List<DateTime>;

            //Sort the list
            uniqueDates.Sort();

            //Resize the return array
            Array.Resize<string>(ref ret, uniqueDates.Count);

            //Copy unique and sorted dates to return array
            for (int i = 0; i < uniqueDates.Count; i++)
            {
                ret[i] = uniqueDates[i].ToString(displayDateFormat, CultureInfo.InvariantCulture);
            }
            return ret;
        }

        /// <summary>
        /// Removes duplicates from an array of date strings and then returns a list of the unique dates.
        /// </summary>
        /// <param name="dateStrings">array of date strings</param>
        /// <param name="displayFormat">the format used for converting the date string to DateTime instance.</param>
        /// <returns>a list of the unique dates</returns>
        private static IList<DateTime> RemoveDuplicates(string[] dateStrings, string displayFormat)
        {
            //Create a unique store
            IDictionary<DateTime, int> uniqueStore = new Dictionary<DateTime, int>();

            for (int i = 0; i < dateStrings.Length; i++)
            {
                DateTime date = DateTime.ParseExact(dateStrings[i], displayFormat, CultureInfo.InvariantCulture);

                //Overwrite duplicates
                uniqueStore[date] = 0;
            }

            return new List<DateTime>(uniqueStore.Keys);
        }

        /// <summary>
        /// Converts the DatePattern instance to a serialized xml format string with the help
        /// of the dedicated DatePatternConverter.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TypeDescriptor.GetConverter(GetType()).
                ConvertTo(null, CultureInfo.InvariantCulture, this, typeof(string)) as string;
        }
    }
}
