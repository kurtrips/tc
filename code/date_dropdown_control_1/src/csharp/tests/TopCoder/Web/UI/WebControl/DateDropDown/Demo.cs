// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Provides a demonstration of the component functionality in the code behind.
    /// The web demo is alos provided and can be found in the test_files folder.
    /// </summary>
    /// <author>MiG-29</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class Demo
    {
        /// <summary>
        /// Provides a demo of the code behind usage of DateDropDownList
        /// </summary>
        [Test]
        public void DateDropDownListDemo()
        {
            // Manage DateDropDownList custom control from the code behind
            DateDropDownList ddl = new DateDropDownList();
            ddl.DisplayDateFormat = "MM/dd/yyyy";

            // Add date/time items to the list
            ddl.AddDateItem(new DateTime(2007, 7, 5));
            ddl.AddDateItem(new DateTime(2007, 7, 5, 9, 0, 0));
            ddl.AddDateItems(new DateTime[] { DateTime.Today, DateTime.Now, DateTime.Now.AddDays(2) });
            ddl.AddDateItems(new DateTime(2007, 7, 6), new DateTime(2007, 7, 12));

            // Remove date/time items from the list
            // The delCount will be 1 after the next operation
            int delCount = ddl.RemoveDateItem(new DateTime(2007, 7, 5));
            // The delCount will be 0 after the next operation
            delCount = ddl.RemoveDateItem(new DateTime(2007, 7, 5));
            // Remove several items
            delCount = ddl.RemoveDateItems(new DateTime[] { DateTime.Today, DateTime.Now });
            delCount = ddl.RemoveDateItems(new DateTime(2007, 7, 6), new DateTime(2007, 7, 12));

            // Get the selected item
            ddl.Items[0].Selected = true;
            DateTime selItem = ddl.SelectedDate;
        }

        /// <summary>
        /// Provides a demo of the code behind usage of DateListBox
        /// </summary>
        [Test]
        public void DateListBoxDemo()
        {
            // Manage DateListBox custom control from the code behind
            DateListBox dlb = new DateListBox();
            dlb.DisplayDateFormat = "MM/dd/yyyy";

            // Add date/time items to the list
            dlb.AddDateItem(new DateTime(2007, 7, 5));
            dlb.AddDateItem(new DateTime(2007, 7, 5, 9, 0, 0));
            dlb.AddDateItems(new DateTime[] { DateTime.Today, DateTime.Now, DateTime.Now.AddDays(2) });
            dlb.AddDateItems(new DateTime(2007, 7, 6), new DateTime(2007, 7, 12));

            // Remove date/time items from the list
            // The delCount will be 1 after the next operation
            int delCount = dlb.RemoveDateItem(new DateTime(2007, 7, 5));

            // The delCount will be 0 after the next operation
            delCount = dlb.RemoveDateItem(new DateTime(2007, 7, 5));

            // Remove several items
            delCount = dlb.RemoveDateItems(new DateTime[] { DateTime.Today, DateTime.Now });
            delCount = dlb.RemoveDateItems(new DateTime(2007, 7, 6), new DateTime(2007, 7, 12));

            //Perform selection
            dlb.Items[0].Selected = true;

            // Get the first selected item
            DateTime selItem = dlb.SelectedDate;
            // Get the selected items
            DateTime[] selItems = dlb.SelectedDates;
        }
    }
}
