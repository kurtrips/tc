// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Web.UI.WebControls;
using System.Globalization;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the DateListBox class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DateListBoxTests
    {
        /// <summary>
        /// The DateListBox instance to use for the tests.
        /// </summary>
        DateListBox dlb;

        /// <summary>
        /// The default display format to use
        /// </summary>
        private const string DisplayDateFormat1 = "MM/dd/yyyy HH:mm";

        /// <summary>
        /// The default input date format to use
        /// </summary>
        private const string InputDateFormat1 = "MM/dd/yyyy HH:mm";

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dlb = new DateListBox();
            dlb.DisplayDateFormat = DisplayDateFormat1;
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dlb = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// DateListBox()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(dlb, "Constructor returns null.");
        }

        /// <summary>
        /// Tests the InputDateFormat property.
        /// </summary>
        [Test]
        public void TestInputDateFormat()
        {
            Assert.AreEqual(dlb.InputDateFormat, String.Empty, "Wrong InputDateFormat implementation.");
            dlb.InputDateFormat = InputDateFormat1;
            Assert.AreEqual(dlb.InputDateFormat, InputDateFormat1, "Wrong InputDateFormat implementation.");
        }

        /// <summary>
        /// Tests the DisplayDateFormat property.
        /// </summary>
        [Test]
        public void TestDisplayDateFormat()
        {
            dlb.DisplayDateFormat = DisplayDateFormat1;
            Assert.AreEqual(dlb.DisplayDateFormat, DisplayDateFormat1, "Wrong DisplayDateFormat implementation.");
        }

        /// <summary>
        /// Tests the InitialSelection property.
        /// </summary>
        [Test]
        public void TestInitialSelection()
        {
            Assert.AreEqual(dlb.InitialSelection, String.Empty, "Wrong InitialSelection implementation.");
            dlb.InitialSelection = "abc";
            Assert.AreEqual(dlb.InitialSelection, "abc", "Wrong InitialSelection implementation.");
        }

        /// <summary>
        /// Tests the InitialSelectionTimeStamp property.
        /// </summary>
        [Test]
        public void TestInitialSelectionTimeStamp()
        {
            Assert.AreEqual(dlb.InitialSelectionTimeStamp, String.Empty,
                "Wrong InitialSelectionTimeStamp implementation.");
            dlb.InitialSelectionTimeStamp = "def";
            Assert.AreEqual(dlb.InitialSelectionTimeStamp, "def",
                "Wrong InitialSelectionTimeStamp implementation.");
        }

        /// <summary>
        /// Tests the InitialSelectionDateFormat method.
        /// string InitialSelectionDateFormat()
        /// </summary>
        [Test]
        public void TestInitialSelectionDateFormat()
        {
            Assert.AreEqual(dlb.InitialSelectionDateFormat, String.Empty,
                "Wrong InitialSelectionDateFormat implementation.");
            dlb.InitialSelectionDateFormat = "ads";
            Assert.AreEqual(dlb.InitialSelectionDateFormat, "ads", "Wrong InitialSelectionDateFormat implementation.");
        }

        /// <summary>
        /// Tests the DatePattern property.
        /// DatePattern DatePattern()
        /// </summary>
        [Test]
        public void TestDatePattern()
        {
            Assert.IsNotNull(dlb.DatePattern, "Wrong DatePattern implementation.");
        }

        /// <summary>
        /// Tests the AddDateItem method.
        /// void AddDateItem(DateTime dateItem)
        /// </summary>
        [Test]
        public void TestAddDateItem()
        {
            Assert.AreEqual(dlb.Items.Count, 0, "Count must initially be 0.");

            dlb.AddDateItem(DateTime.Today);
            Assert.AreEqual(dlb.Items[0].Value, DateTime.Today.ToString(dlb.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItem implementation.");
        }

        /// <summary>
        /// Tests the AddDateItems method.
        /// void AddDateItems(DateTime[] dateItems)
        /// </summary>
        [Test]
        public void TestAddDateItems1()
        {
            DateTime[] arr = new DateTime[2] { DateTime.Today, DateTime.Now };

            dlb.AddDateItems(arr);
            Assert.AreEqual(dlb.Items[0].Value, DateTime.Today.ToString(dlb.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
            Assert.AreEqual(dlb.Items[1].Value, DateTime.Now.ToString(dlb.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
        }

        /// <summary>
        /// Tests the AddDateItems method.
        /// void AddDateItems(DateTime startDate, DateTime stopDate)
        /// </summary>
        [Test]
        public void TestAddDateItems2()
        {
            dlb.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(dlb.Items[0].Value, DateTime.Today.ToString(dlb.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
            Assert.AreEqual(dlb.Items[1].Value, DateTime.Today.AddDays(1).ToString(dlb.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
            Assert.AreEqual(dlb.Items[2].Value, DateTime.Today.AddDays(2).ToString(dlb.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
        }

        /// <summary>
        /// Tests the RemoveDateItem method.
        /// int RemoveDateItem(DateTime dateItem)
        /// </summary>
        [Test]
        public void TestRemoveDateItem()
        {
            dlb.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(dlb.Items.Count, 3, "Initial count must be 3.");

            Assert.AreEqual(dlb.RemoveDateItem(DateTime.Today.AddDays(1)), 1, "Wrong RemoveDateItem implementation.");
            Assert.AreEqual(dlb.Items.Count, 2, "Count must be 2.");

            Assert.AreEqual(dlb.RemoveDateItem(DateTime.Today.AddDays(2)), 1, "Wrong RemoveDateItem implementation.");
            Assert.AreEqual(dlb.Items.Count, 1, "Count must be 1.");
        }

        /// <summary>
        /// Tests the RemoveDateItems method.
        /// int RemoveDateItems(DateTime[] dateItems)
        /// </summary>
        [Test]
        public void TestRemoveDateItems1()
        {
            dlb.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(dlb.Items.Count, 3, "Initial count must be 3.");

            //Must return 1 as DateTime.Today.AddDays(5) is not found so is not considered
            Assert.AreEqual(dlb.RemoveDateItems(new DateTime[] { DateTime.Today, DateTime.Today.AddDays(5) }), 1,
                "Wrong RemoveDateItems implementation.");
            Assert.AreEqual(dlb.Items.Count, 2, "count must be 2 now.");
        }

        /// <summary>
        /// Tests the RemoveDateItems method.
        /// int RemoveDateItems(DateTime startDate, DateTime stopDate)
        /// </summary>
        [Test]
        public void TestRemoveDateItems2()
        {
            dlb.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(dlb.Items.Count, 3, "Initial count must be 3.");

            Assert.AreEqual(dlb.RemoveDateItems(DateTime.Today, DateTime.Today.AddDays(1)), 2,
                "Wrong RemoveDateItems implementation.");
            Assert.AreEqual(dlb.Items.Count, 1, "count must be 1 now.");
        }

        /// <summary>
        /// Tests the SelectedDate property.
        /// </summary>
        [Test]
        public void TestSelectedDate()
        {
            dlb.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            dlb.Items[1].Selected = true;

            Assert.AreEqual(DateTime.Today.AddDays(1), dlb.SelectedDate,
                "Wrong SelectedDate implementation.");
        }

        /// <summary>
        /// Tests the AddItem for failure when unable to convert date to display format.
        /// DateDropDownException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(DateDropDownException))]
        public void TestAddItemFail()
        {
            dlb.DisplayDateFormat = "W";
            dlb.AddDateItem(DateTime.Today);
        }

        /// <summary>
        /// Tests the AddItems for failure when input array is empty
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddItemsFail1()
        {
            dlb.AddDateItems(new DateTime[0]);
        }

        /// <summary>
        /// Tests the AddItems for failure when start is greater than stop date
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddItemsFail2()
        {
            dlb.AddDateItems(DateTime.Today.AddDays(1), DateTime.Today);
        }

        /// <summary>
        /// Tests the RemoveItem for failure when unable to convert date to display format.
        /// DateDropDownException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(DateDropDownException))]
        public void TestRemoveItemFail1()
        {
            dlb.DisplayDateFormat = "W";
            dlb.RemoveDateItem(DateTime.Today);
        }

        /// <summary>
        /// Tests the RemoveItems for failure when input array is empty
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRemoveItemFail2()
        {
            dlb.RemoveDateItems(new DateTime[0]);
        }

        /// <summary>
        /// Tests the RemoveItems for failure when start is greater than stop date
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRemoveItemFail3()
        {
            dlb.RemoveDateItems(DateTime.Today.AddDays(1), DateTime.Today);
        }

    }
}
