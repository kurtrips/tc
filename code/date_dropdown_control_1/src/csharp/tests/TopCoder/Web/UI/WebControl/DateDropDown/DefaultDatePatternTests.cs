// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Web.UI;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the DatePattern class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DatePatternTests
    {
        /// <summary>
        /// The DatePattern instance to use throughout the tests
        /// </summary>
        DatePattern dp;

        /// <summary>
        /// The default display format to use for the GenerateDates call
        /// </summary>
        private const string DisplayDateFormat1 = "MM/dd/yyyy HH:mm";

        /// <summary>
        /// The default input date format to use for the GenerateDates call
        /// </summary>
        private const string InputDateFormat1 = "MM/dd/yyyy HH:mm";

        /// <summary>
        /// The default start date to use for the GenerateDates call
        /// </summary>
        private const string StartDate1 = "08/01/2007 00:00";

        /// <summary>
        /// The default end date to use for the GenerateDates call
        /// </summary>
        private const string StopDate1 = "09/01/2007 00:00";

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dp = new DatePattern();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dp = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// DatePattern()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(dp, "Constructor returns null!");
        }

        /// <summary>
        /// Tests the InputDateFormat property.
        /// </summary>
        [Test]
        public void TestInputDateFormat()
        {
            dp.InputDateFormat = InputDateFormat1;
            Assert.AreEqual(dp.InputDateFormat, InputDateFormat1, "Wrong InputDateFormat implementation.");
        }

        /// <summary>
        /// Tests the InputDateFormat property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestInputDateFormatFail()
        {
            dp.InputDateFormat = null;
        }

        /// <summary>
        /// Tests the DisplayDateFormat property.
        /// </summary>
        [Test]
        public void TestDisplayDateFormat()
        {
            dp.DisplayDateFormat = DisplayDateFormat1;
            Assert.AreEqual(dp.DisplayDateFormat, DisplayDateFormat1, "Wrong DisplayDateFormat implementation.");
        }

        /// <summary>
        /// Tests the DisplayDateFormat property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestDisplayDateFormatFail()
        {
            dp.DisplayDateFormat = null;
        }

        /// <summary>
        /// Tests the StartDate property.
        /// </summary>
        [Test]
        public void TestStartDate()
        {
            dp.StartDate = StartDate1;
            Assert.AreEqual(dp.StartDate, StartDate1, "Wrong StartDate implementation.");
        }

        /// <summary>
        /// Tests the StartDate property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestStartDateFail()
        {
            dp.StartDate = null;
        }

        /// <summary>
        /// Tests the StopDate property.
        /// </summary>
        [Test]
        public void TestStopDate()
        {
            dp.StopDate = StopDate1;
            Assert.AreEqual(dp.StopDate, StopDate1, "Wrong StopDate implementation.");
        }

        /// <summary>
        /// Tests the StopDate property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSStopDateFail()
        {
            dp.StopDate = null;
        }

        /// <summary>
        /// Tests the Rules property.
        /// </summary>
        [Test]
        public void TestRules()
        {
            Assert.IsNotNull(dp.Rules, "Wrong Rules getter implementation.");
        }

        /// <summary>
        /// Tests the ControlId property.
        /// </summary>
        [Test]
        public void TestControlId()
        {
            dp.ControlID = "abs";
            Assert.AreEqual(dp.ControlID, "abs", "Wrong ControlId implementation.");
        }

        /// <summary>
        /// Tests the ControlId property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestControlIdFail()
        {
            dp.ControlID = null;
        }

        /// <summary>
        /// Tests the ControlInstance property.
        /// void ControlInstance(Control value)
        /// </summary>
        [Test]
        public void TestControlInstance()
        {
            Control c = new Control();
            dp.ControlInstance = c;
            Assert.AreEqual(dp.ControlInstance, c, "Wrong ControlInstance implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method when no Rule is present.
        /// String[] GenerateDates()
        /// </summary>
        [Test]
        public void TestGenerateDates()
        {
            string[] res = dp.GenerateDates();
            Assert.AreEqual(res.Length, 0, "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method when 2 rules are present.
        /// String[] GenerateDates()
        /// </summary>
        [Test]
        public void TestGenerateDates1()
        {
            //Create the rules
            Rule rule1 = new Rule();
            rule1.DateType = "DayOfWeek";
            rule1.DateValue = "Friday;Monday-Wednesday";
            rule1.Interleave = 2;

            Rule rule2 = new Rule();
            rule2.DateType = "DayOfMonth";
            rule2.DateValue = "1;5-10";
            rule2.ReverseOrder = true;

            //Add the rules to the pattern
            dp.Rules.Add(rule1);
            dp.Rules.Add(rule2);

            //Set the pattern properties
            dp.InputDateFormat = "MM:dd:yyyy";
            dp.DisplayDateFormat = "MM:dd:yyyy";
            dp.StartDate = "01:01:2007";
            dp.StopDate = "12:31:2007";

            //Generate
            string[] res = dp.GenerateDates();
            Assert.AreEqual(res.Length, 139, "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the toString() method
        /// </summary>
        [Test]
        public void TestToString()
        {
            string s = dp.ToString();
            Assert.AreEqual(s, "<DatePattern />", "Wrong ToString implementation.");
        }
    }
}
