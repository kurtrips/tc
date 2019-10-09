// DateDropDownList.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// <para>This is a custom web-control that allows the user to select a single date/time item from a drop-down list.
    /// The class is inherited from the standard DropDownList web-control and provides a special support for list items,
    /// which can only  contain date and/or time. The control supports adding dates to its items list by four different
    /// ways: 1) Pattern generation. The user can set period of generation, given date/times, and special rules for
    /// automatic generation of date/times. 2) Add method. A special methods for programmatical adding/removing dates
    /// are implemented. 3) Data binding. The user can bind external data source to this control items list. The data
    /// from data source will be parsed, validated and set to this control. 4) Standard ListItem ASP tags. The standard
    /// ASP ListItem tags can be used for manually set the items to this control list. All set data will be validated to
    /// have proper display format. The control supports properties for input date/times parsing and display formatting.
    /// The initial (single) selection of the control item can be configured through the implemented properties, or set
    /// explicitly.  The selected item can be easily retrieved (as DateTime object) at any time by using a provided
    /// function. Note, this control fully maintains all formatting and rendering capabilities of the underlying
    /// DropDownList control. </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// <para>This class is not thread-safe. It inherits from the not thread-safe parent class (DropDownList),
    /// and there are no needs to make web-controls thread-safe. </para>
    /// </threadsafety>
    ///
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DefaultProperty("InitialSelection")]
    [ToolboxData("<{0}:DateDropDownList runat=server></{0}:DateDropDownList>")]
    [System.Drawing.ToolboxBitmap("DateDropDownList.ico")]
    public class DateDropDownList : DropDownList
    {
        /// <summary>
        /// <para>Represents the reference to the custom DatePattern property object of this class. The field is directly
        /// initialized to null, an next will be set to a DataPattern instance in the related getter property. After
        /// that, the field value is  changed in the LoadViewState(...) method, but it will never set to null. The inner
        /// state of the field can be changed at any time. Use the related getter property to retrieve the value of this
        /// field.</para>
        /// </summary>
        private DatePattern datePattern;

        /// <summary>
        /// <para>Represents the state (serialized to a string) of the DatePattern custom property of this class. This
        /// field will be used in SaveViewState() and TrackViewState() methods to properly storing DatePattern
        /// serialized data to the ViewState.  The field is directly initialized and will be changed during life-time of
        /// the class. It can not be null, but can be empty string.</para>
        /// </summary>
        private string datePatternInitialString;

        /// <summary>
        /// <para>Represents the getter/setter property for the InitialSelection data of the ViewState.</para>
        /// </summary>
        /// <value>The Initial Selection mode for the control.</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string InitialSelection
        {
            get
            {
                string ret = ViewState["InitialSelection" + ID] as string;
                return ret == null ? String.Empty : ret;
            }
            set
            {
                ViewState["InitialSelection" + ID] = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the InitialSelectionTimeStamp data of the ViewState.</para>
        /// </summary>
        /// <value>The timeStamp with reference to which initial selection is performed.</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string InitialSelectionTimeStamp
        {
            get
            {
                string ret = ViewState["InitialSelectionTimeStamp" + ID] as string;
                return (ret == null) ? String.Empty : ret;
            }
            set
            {
                ViewState["InitialSelectionTimeStamp" + ID] = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the InitialSelectionDateFormat data of the ViewState.</para>
        /// </summary>
        /// <value>The date format to use for initial selection.</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string InitialSelectionDateFormat
        {
            get
            {
                string ret = ViewState["InitialSelectionDateFormat" + ID] as string;
                return (ret == null) ? String.Empty : ret;
            }

            set
            {
                ViewState["InitialSelectionDateFormat" + ID] = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter property for the first selected item of this control internal list.</para>
        /// </summary>
        /// <value>The first selected date in the listBox.</value>
        /// <exception cref="DateDropDownException">If no dates are selected in the DateListBox.</exception>
        public virtual DateTime SelectedDate
        {
            get
            {
                if (SelectedItem == null)
                {
                    throw new DateDropDownException("No date is selected in the DateDropDownList.");
                }
                return DateTime.ParseExact(SelectedItem.Value, DisplayDateFormat, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the InputDateFormat data of the ViewState. The standard .NET
        /// date/time formatting string format is used for this property value.</para>
        /// </summary>
        /// <value>The input date format for the control</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string InputDateFormat
        {
            get
            {
                string ret = ViewState["InputDateFormat" + ID] as string;
                return ret == null ? String.Empty : ret;
            }
            set
            {
                ViewState["InputDateFormat" + ID] = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter/setter property for the DisplayDateFormat data of the ViewState. The standard .NET
        /// date/time formatting string format is used for this property value.</para>
        /// </summary>
        /// <value>The display date format for the control</value>
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public virtual string DisplayDateFormat
        {
            get
            {
                string ret = ViewState["DisplayDateFormat" + ID] as string;
                return ret == null ? String.Empty : ret;
            }
            set
            {
                ViewState["DisplayDateFormat" + ID] = value;
            }
        }

        /// <summary>
        /// <para>Represents the getter property for the DatePattern used for the control.</para>
        /// </summary>
        /// <value>The DatePattern used for the control.</value>
        [Browsable(true)]
        [NotifyParentProperty(true)]
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual DatePattern DatePattern
        {
            get
            {
                if (datePattern == null)
                {
                    datePattern = new DatePattern();
                }
                return datePattern;
            }
        }

        /// <summary>
        /// <para>The default constructor.</para>
        /// </summary>
        public DateDropDownList() : base()
        {
        }

        /// <summary>
        /// <para>Add the date/time item to the internal list of this control. The new item will be shown on the display
        /// list represented by this control.</para>
        /// </summary>
        /// <param name="dateItem">
        /// an instance of the DateTime structure. This dateTime element will be added to the internal list of items to
        /// this control. Can be any value.
        /// </param>
        /// <exception cref="DateDropDownException">If date could not be added to the control</exception>
        public void AddDateItem(DateTime dateItem)
        {
            string dateString;
            try
            {
                dateString = dateItem.ToString(DisplayDateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new DateDropDownException("Unable to add date to the DateDropDownList.", e);
            }
            Items.Add(dateString);
        }

        /// <summary>
        /// <para>Add the array of multiple given data/time items to the internal list of this control. The items will be
        /// added to the display list represented by this control.</para>
        /// </summary>
        /// <param name="dateItems">
        /// an array of instances of the DateTime structure. These dateTime elements of the array will be added to the
        /// internal list of items of this control. The array can not be null or empty. The array elements can be any
        /// value.
        /// </param>
        /// <exception cref="ArgumentNullException">if dateItems is null.</exception>
        /// <exception cref="ArgumentException">if the dateItems is empty.</exception>
        /// <exception cref="DateDropDownException">if problem during formatting dateItem to a string occurs</exception>
        public void AddDateItems(DateTime[] dateItems)
        {
            Helper.ValidateNotNull(dateItems, "dateItems");
            if (dateItems.Length == 0)
            {
                throw new ArgumentException("dateItems must not be empty.", "dateItems");
            }

            foreach (DateTime date in dateItems)
            {
                AddDateItem(date);
            }
        }

        /// <summary>
        /// <para>Add the interval of multiple given data/time items to the internal list of this control. The items will
        /// be added to the display list represented by this control.</para>
        /// </summary>
        /// <param name="startDate">
        /// the DateTime structure instance, which defines the beginning of the time interval for adding items to the
        /// internal items list of this control. It should be not greater than stopDate argument.
        /// </param>
        /// <param name="stopDate">
        /// the DateTime structure instance, which defines the ending of the time interval for adding items of the
        /// internal items list of this control. It should be not lesser than startDate argument.
        /// </param>
        /// <exception cref="ArgumentException">if the startDate is greater than stopDate.</exception>
        /// <exception cref="DateDropDownException">if any problem during formatting dateItem to a string occured
        /// </exception>
        public void AddDateItems(DateTime startDate, DateTime stopDate)
        {
            if (startDate > stopDate)
            {
                throw new ArgumentException("startDate must be less or equal to stopDate.", "startDate");
            }

            for (DateTime date = startDate; date <= stopDate; date = date.AddDays(1))
            {
                AddDateItem(date);
            }
        }

        /// <summary>
        /// <para>Remove the single given data/time item from the internal list of this control. The item will be removed
        /// from the display list represented by this control.</para>
        /// </summary>
        /// <param name="dateItem">
        /// an instance of the DateTime structure. This dateTime element will be removed from the internal list of items
        /// to this control. Can be any value.
        /// </param>
        /// <returns>
        /// the number of the removed elements from the internal items list of this control. Can be positive number
        /// (including 0).
        /// </returns>
        /// <exception cref="DateDropDownException">If the date cannot be converted to display format</exception>
        public int RemoveDateItem(DateTime dateItem)
        {
            //Convert to string
            string dateString;
            try
            {
                dateString = dateItem.ToString(DisplayDateFormat, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new DateDropDownException("Unable to remove date from the DateDropDownList.", e);
            }

            //Remove
            int removed = 0;
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].Value == dateString)
                {
                    Items.RemoveAt(i);
                    removed++;
                }
            }

            return removed;
        }

        /// <summary>
        /// <para>Remove the array of multiple given data/time items from the internal list of this control. The items will
        /// be removed from the display list represented by this control.</para>
        /// </summary>
        /// <param name="dateItems">
        /// an array of instances of the DateTime structure. These dateTime elements of the array will be removed from
        /// the internal list of items of this control. The array can not be null or empty. The array elements can be
        /// any value.
        /// </param>
        /// <returns>
        /// the number of the removed elements from the internal items list of this control. Can be positive number
        /// (including 0).
        /// </returns>
        /// <exception cref="DateDropDownException">If the date cannot be converted to display format</exception>
        /// <exception cref="ArgumentNullException">if the dateItems is null</exception>
        /// <exception cref="ArgumentException">if the dateItems is empty.</exception>
        public int RemoveDateItems(DateTime[] dateItems)
        {
            Helper.ValidateNotNull(dateItems, "dateItems");
            if (dateItems.Length == 0)
            {
                throw new ArgumentException("dateItems must not be empty.", "dateItems");
            }

            //Delegate to other overload
            int removed = 0;
            foreach (DateTime dateItem in dateItems)
            {
                removed += RemoveDateItem(dateItem);
            }
            return removed;
        }

        /// <summary>
        /// <para>Remove the interval of multiple given data/time items from the internal list of this control. The items
        /// will be removed from the display list represented by this control.</para>
        /// </summary>
        /// <param name="startDate">
        /// the DateTime structure instance, which defines the beginning of the time interval for removing items of the
        /// internal items list of this control. It should be not greater than stopDate argument.
        /// </param>
        /// <param name="stopDate">
        /// the DateTime structure instance, which defines the ending of the time interval for removing items of the
        /// internal items list of this control. It should be not lesser than startDate argument.
        /// </param>
        /// <returns>
        /// the number of the removed elements from the internal items list of this control. Can be positive number
        /// (including 0).
        /// </returns>
        /// <exception cref="ArgumentException">if the startDate is greater than stopDate</exception>
        public int RemoveDateItems(DateTime startDate, DateTime stopDate)
        {
            if (startDate > stopDate)
            {
                throw new ArgumentException("startDate must be less or equal to stopDate.", "startDate");
            }

            //Delegate to other overload
            int removed = 0;
            for (DateTime date = startDate; date <= stopDate; date = date.AddDays(1))
            {
                removed += RemoveDateItem(date);
            }
            return removed;
        }

        /// <summary>
        /// <para>The overridden method called just before saving view state and rendering the content. It is responsible
        /// for setting the generated date/time items and set the initial selection.</para>
        /// </summary>
        /// <param name="e">
        /// an instance of the EventArgs. This argument is not directly used in this method - it is simply sent to the
        /// super method. Can be any value.
        /// </param>
        /// <exception cref="InitialSelectionInvalidDataException">
        /// if any problem with setting initial selection occurs.
        /// </exception>
        /// <exception cref="RuleInvalidDataException">
        /// if any problem with generating date/time items by Rule class occurs
        /// </exception>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                //Get dates and add to items
                string[] dates = DatePattern.GenerateDates();
                foreach (string date in dates)
                {
                    Items.Add(date);
                }

                //Initial selection
                InitialSelectionManager initSelectionMgr = new InitialSelectionManager(
                    InitialSelection, InitialSelectionTimeStamp, InitialSelectionDateFormat);
                initSelectionMgr.SelectItems(Items, false, DisplayDateFormat);
            }
        }

        /// <summary>
        /// <para>Overrides the method, which binds the specified data source to this control. In this method the
        /// validation of the data to be binded is implemented</para>
        /// </summary>
        /// <param name="dataSource">
        /// an instance of the IEnumerable. It contains the data elements to be binded to this control. Can not be null.
        /// Should not contain null elements.
        /// </param>
        /// <exception cref="ArgumentNullException">if datasource is null</exception>
        /// <exception cref="ArgumentException">if dataSource contains null elements</exception>
        /// <exception cref="BindDataInvalidException">if the binded data is invalid (not DateTime or string data type,
        /// has incorrect formatting for text string, and so on)</exception>
        protected override void PerformDataBinding(IEnumerable dataSource)
        {
            //Validate
            Helper.ValidateNotNull(dataSource, "dataSource");

            //Bind data
            ItemValidator itemValidator = new ItemValidator(DisplayDateFormat);
            if (!itemValidator.Validate(dataSource))
            {
                throw new BindDataInvalidException("Data could not be bound to the control.");
            }

            base.PerformDataBinding(dataSource);
        }

        /// <summary>
        /// <para>Overrides the method, which loads the previously saved state in the ViewState. The DatePattern object is
        /// loaded here from the ViewState.</para>
        /// </summary>
        /// <param name="savedState">
        /// any object instance. This argument is not directly used in this method - it is simply sent to the super
        /// method. Can be any value.
        /// </param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            //Retrieve the DatePattern from Viewstate
            DatePattern pattern = ViewState["DatePattern_" + ID] as DatePattern;
            if (pattern != null)
            {
                datePattern = pattern;
            }
        }

        /// <summary>
        /// <para>Overrides the method, which saves the state to the ViewState. The DatePattern object is saved here to the
        /// ViewState.</para>
        /// </summary>
        /// <returns>an object containing the saved state of this control. Can not be null.</returns>
        protected override object SaveViewState()
        {
            //Save the DatePattern to Viewstate
            if (datePattern != null)
            {
                string datePatternStr = datePattern.ToString();
                if (datePatternStr != datePatternInitialString)
                {
                    ViewState["DatePattern_" + ID] = datePatternStr;
                }
            }

            return base.SaveViewState();
        }

        /// <summary>
        /// <para>Overrides the method, which marks the starting point to begin tracking and saving view-state changes. The
        /// string representation of this.datePattern is stored here to this.datePatternInitialString.</para>
        /// </summary>
        protected override void TrackViewState()
        {
            //Save string representation of datePattern to datePatternInitialString instance variable
            if (datePattern != null)
            {
                datePatternInitialString = datePattern.ToString();
            }

            base.TrackViewState();
        }

        /// <summary>
        /// <para>Overrides the method, which is called on the control initialization. This method allows to set the
        /// current control ID to the sub-property control.</para>
        /// </summary>
        /// <param name="e">
        /// an instance of the EventArgs. This argument is not directly used in this method - it is simply sent to the
        /// super method. Can be any value.
        /// </param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Associate DatePattern sub property to control.
            DatePattern.ControlID = ID;
            DatePattern.ControlInstance = this;
        }
    }
}
