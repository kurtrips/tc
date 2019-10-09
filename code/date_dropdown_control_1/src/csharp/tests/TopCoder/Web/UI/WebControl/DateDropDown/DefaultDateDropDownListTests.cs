// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Globalization;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the DateDropDownList class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DateDropDownListTests
    {
        /// <summary>
        /// The DateDropDownList instance to use for the tests.
        /// </summary>
        DateDropDownList ddl;

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
            ddl = new DateDropDownList();
            ddl.DisplayDateFormat = DisplayDateFormat1;
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            ddl = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// DateDropDownList()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(ddl, "Constructor returns null.");
        }

        /// <summary>
        /// Tests the InputDateFormat property.
        /// </summary>
        [Test]
        public void TestInputDateFormat()
        {
            Assert.AreEqual(ddl.InputDateFormat, String.Empty, "Wrong InputDateFormat implementation.");
            ddl.InputDateFormat = InputDateFormat1;
            Assert.AreEqual(ddl.InputDateFormat, InputDateFormat1, "Wrong InputDateFormat implementation.");
        }

        /// <summary>
        /// Tests the DisplayDateFormat property.
        /// </summary>
        [Test]
        public void TestDisplayDateFormat()
        {
            ddl.DisplayDateFormat = DisplayDateFormat1;
            Assert.AreEqual(ddl.DisplayDateFormat, DisplayDateFormat1, "Wrong DisplayDateFormat implementation.");
        }

        /// <summary>
        /// Tests the InitialSelection property.
        /// </summary>
        [Test]
        public void TestInitialSelection()
        {
            Assert.AreEqual(ddl.InitialSelection, String.Empty, "Wrong InitialSelection implementation.");
            ddl.InitialSelection = "abc";
            Assert.AreEqual(ddl.InitialSelection, "abc", "Wrong InitialSelection implementation.");
        }

        /// <summary>
        /// Tests the InitialSelectionTimeStamp property.
        /// </summary>
        [Test]
        public void TestInitialSelectionTimeStamp()
        {
            Assert.AreEqual(ddl.InitialSelectionTimeStamp, String.Empty,
                "Wrong InitialSelectionTimeStamp implementation.");
            ddl.InitialSelectionTimeStamp = "def";
            Assert.AreEqual(ddl.InitialSelectionTimeStamp, "def",
                "Wrong InitialSelectionTimeStamp implementation.");
        }

        /// <summary>
        /// Tests the InitialSelectionDateFormat method.
        /// string InitialSelectionDateFormat()
        /// </summary>
        [Test]
        public void TestInitialSelectionDateFormat()
        {
            Assert.AreEqual(ddl.InitialSelectionDateFormat, String.Empty,
                "Wrong InitialSelectionDateFormat implementation.");
            ddl.InitialSelectionDateFormat = "ads";
            Assert.AreEqual(ddl.InitialSelectionDateFormat, "ads", "Wrong InitialSelectionDateFormat implementation.");
        }

        /// <summary>
        /// Tests the DatePattern property.
        /// DatePattern DatePattern()
        /// </summary>
        [Test]
        public void TestDatePattern()
        {
            Assert.IsNotNull(ddl.DatePattern, "Wrong DatePattern implementation.");
        }

        /// <summary>
        /// Tests the AddDateItem method.
        /// void AddDateItem(DateTime dateItem)
        /// </summary>
        [Test]
        public void TestAddDateItem()
        {
            Assert.AreEqual(ddl.Items.Count, 0, "Count must initially be 0.");

            ddl.AddDateItem(DateTime.Today);
            Assert.AreEqual(ddl.Items[0].Value, DateTime.Today.ToString(ddl.DisplayDateFormat,
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

            ddl.AddDateItems(arr);
            Assert.AreEqual(ddl.Items[0].Value, DateTime.Today.ToString(ddl.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
            Assert.AreEqual(ddl.Items[1].Value, DateTime.Now.ToString(ddl.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
        }

        /// <summary>
        /// Tests the AddDateItems method.
        /// void AddDateItems(DateTime startDate, DateTime stopDate)
        /// </summary>
        [Test]
        public void TestAddDateItems2()
        {
            ddl.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(ddl.Items[0].Value, DateTime.Today.ToString(ddl.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
            Assert.AreEqual(ddl.Items[1].Value, DateTime.Today.AddDays(1).ToString(ddl.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
            Assert.AreEqual(ddl.Items[2].Value, DateTime.Today.AddDays(2).ToString(ddl.DisplayDateFormat,
                CultureInfo.InvariantCulture), "Wrong AddDateItems implementation.");
        }

        /// <summary>
        /// Tests the RemoveDateItem method.
        /// int RemoveDateItem(DateTime dateItem)
        /// </summary>
        [Test]
        public void TestRemoveDateItem()
        {
            ddl.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(ddl.Items.Count, 3, "Initial count must be 3.");

            Assert.AreEqual(ddl.RemoveDateItem(DateTime.Today.AddDays(1)), 1, "Wrong RemoveDateItem implementation.");
            Assert.AreEqual(ddl.Items.Count, 2, "Count must be 2.");

            Assert.AreEqual(ddl.RemoveDateItem(DateTime.Today.AddDays(2)), 1, "Wrong RemoveDateItem implementation.");
            Assert.AreEqual(ddl.Items.Count, 1, "Count must be 1.");
        }

        /// <summary>
        /// Tests the RemoveDateItems method.
        /// int RemoveDateItems(DateTime[] dateItems)
        /// </summary>
        [Test]
        public void TestRemoveDateItems1()
        {
            ddl.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(ddl.Items.Count, 3, "Initial count must be 3.");

            //Must return 1 as DateTime.Today.AddDays(5) is not found so is not considered
            Assert.AreEqual(ddl.RemoveDateItems(new DateTime[] { DateTime.Today , DateTime.Today.AddDays(5) }), 1,
                "Wrong RemoveDateItems implementation.");
            Assert.AreEqual(ddl.Items.Count, 2, "count must be 2 now.");
        }

        /// <summary>
        /// Tests the RemoveDateItems method.
        /// int RemoveDateItems(DateTime startDate, DateTime stopDate)
        /// </summary>
        [Test]
        public void TestRemoveDateItems2()
        {
            ddl.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            Assert.AreEqual(ddl.Items.Count, 3, "Initial count must be 3.");

            Assert.AreEqual(ddl.RemoveDateItems(DateTime.Today, DateTime.Today.AddDays(1)), 2,
                "Wrong RemoveDateItems implementation.");
            Assert.AreEqual(ddl.Items.Count, 1, "count must be 1 now.");
        }

        /// <summary>
        /// Tests the SelectedDate property.
        /// </summary>
        [Test]
        public void TestSelectedDate()
        {
            ddl.AddDateItems(DateTime.Today, DateTime.Today.AddDays(2));
            ddl.Items[1].Selected = true;

            Assert.AreEqual(DateTime.Today.AddDays(1), ddl.SelectedDate,
                "Wrong SelectedDate implementation.");
        }

        /// <summary>
        /// Tests the AddItem for failure when unable to convert date to display format.
        /// DateDropDownException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(DateDropDownException))]
        public void TestAddItemFail()
        {
            ddl.DisplayDateFormat = "W";
            ddl.AddDateItem(DateTime.Today);
        }

        /// <summary>
        /// Tests the AddItems for failure when input array is empty
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddItemsFail1()
        {
            ddl.AddDateItems(new DateTime[0]);
        }

        /// <summary>
        /// Tests the AddItems for failure when start is greater than stop date
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddItemsFail2()
        {
            ddl.AddDateItems(DateTime.Today.AddDays(1), DateTime.Today);
        }

        /// <summary>
        /// Tests the RemoveItem for failure when unable to convert date to display format.
        /// DateDropDownException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(DateDropDownException))]
        public void TestRemoveItemFail1()
        {
            ddl.DisplayDateFormat = "W";
            ddl.RemoveDateItem(DateTime.Today);
        }

        /// <summary>
        /// Tests the RemoveItems for failure when input array is empty
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRemoveItemFail2()
        {
            ddl.RemoveDateItems(new DateTime[0]);
        }

        /// <summary>
        /// Tests the RemoveItems for failure when start is greater than stop date
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestRemoveItemFail3()
        {
            ddl.RemoveDateItems(DateTime.Today.AddDays(1), DateTime.Today);
        }
    }
}
