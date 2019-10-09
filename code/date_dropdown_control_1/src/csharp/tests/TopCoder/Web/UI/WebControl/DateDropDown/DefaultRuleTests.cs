// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Globalization;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the Rule class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class RuleTests
    {
        /// <summary>
        /// The Rule instance to use throughout the tests.
        /// </summary>
        private Rule rule;

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
            rule = new Rule();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            rule = null;
        }

        #region "Accuracy Tests"

        /// <summary>
        /// Tests the constructor.
        /// Rule()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(rule, "Constructor returns null.");
        }

        /// <summary>
        /// Tests the DateType property for accuracy.
        /// </summary>
        [Test]
        public void TestDateType()
        {
            //Set value
            rule.DateType = "Day";
            Assert.AreEqual(rule.DateType, "Day", "Wrong DateType property implementation.");
        }

        /// <summary>
        /// Tests the DateValue property for accuracy.
        /// </summary>
        [Test]
        public void TestDateValue()
        {
            //Set value
            rule.DateValue = "Any value";
            Assert.AreEqual(rule.DateValue, "Any value", "Wrong DateValue property implementation.");
        }

        /// <summary>
        /// Tests the TimeValue property for accuracy.
        /// </summary>
        [Test]
        public void TestTimeValue()
        {
            //Set value
            rule.TimeValue = "Some value";
            Assert.AreEqual(rule.TimeValue, "Some value", "Wrong TimeValue property implementation.");
        }

        /// <summary>
        /// Tests the YearDivisor property for accuracy.
        /// </summary>
        [Test]
        public void TestYearDivisor()
        {
            //Set value
            rule.YearDivisor = 1;
            Assert.AreEqual(rule.YearDivisor, 1, "Wrong YearDivisor property implementation.");

            //Set value
            rule.YearDivisor = 5;
            Assert.AreEqual(rule.YearDivisor, 5, "Wrong YearDivisor property implementation.");
        }

        /// <summary>
        /// Tests the ReverseOrder property for accuracy.
        /// </summary>
        [Test]
        public void TestReverseOrder()
        {
            //Set value
            rule.ReverseOrder = true;
            Assert.IsTrue(rule.ReverseOrder, "Wrong ReverseOrder property implementation.");

            //Set value
            rule.ReverseOrder = false;
            Assert.IsFalse(rule.ReverseOrder, "Wrong ReverseOrder property implementation.");
        }

        /// <summary>
        /// Tests the Interleave property for accuracy.
        /// </summary>
        [Test]
        public void TestInterleave()
        {
            //Set value
            rule.Interleave = 0;
            Assert.AreEqual(rule.Interleave, 0, "Wrong Interleave property implementation.");

            //Set value
            rule.Interleave = 2;
            Assert.AreEqual(rule.Interleave, 2, "Wrong Interleave property implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets every 2nd Thursday of the August 2007. Dates 2, 16, 30th of August are expected
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy1()
        {
            rule.DateType = "DayOfWeek";
            rule.DateValue = "Thursday";
            rule.Interleave = 1;

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);
            string[] expected = new string[] { "08/02/2007 00:00", "08/16/2007 00:00", "08/30/2007 00:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets 9 AM and 9 PM of every 2nd Thursday of the August 2007.
        /// 2 Dates each for 2, 16, 30th of August are expected with time 9 Am and 9 Pm
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy2()
        {
            rule.DateType = "DayOfWeek";
            rule.DateValue = "Thursday";
            rule.TimeValue = "9:00 AM;9:00 PM";
            rule.Interleave = 1;

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);
            string[] expected = new string[] { "08/02/2007 09:00", "08/02/2007 21:00",
                "08/16/2007 09:00", "08/16/2007 21:00", "08/30/2007 09:00", "08/30/2007 21:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets 9 AM and 9 PM of every 2nd Thursday from 1 Aug 2007 to 30 Aug 2007 (10 AM)
        /// 2 Dates each for 2, 16 with time 9 Am and 9 Pm and 1 date for 30th of August with 9 am are expected
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy3()
        {
            rule.DateType = "DayOfWeek";
            rule.DateValue = "Thursday";
            rule.TimeValue = "9:00 AM;9:00 PM";
            rule.Interleave = 1;

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, "08/30/2007 10:00");
            string[] expected = new string[] { "08/02/2007 09:00", "08/02/2007 21:00",
                "08/16/2007 09:00", "08/16/2007 21:00", "08/30/2007 09:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets every 2nd Thursday from 1 Dec 2008 to 30 Jan 2010 where YEarDivisor is 2.
        /// The whole of 2009 must be skipped and the interleaving must be applied to the remaining dates
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy4()
        {
            rule.DateType = "DayOfWeek";
            rule.DateValue = "Thursday";
            rule.Interleave = 1;
            rule.YearDivisor = 2;

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1,
                "12/01/2008 00:00", "01/30/2010 00:00");
            string[] expected = new string[] { "12/04/2008 00:00", "12/18/2008 00:00",
                "01/14/2010 00:00", "01/28/2010 00:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets Monday to Wednesday and Saturday from 01 Aug 2007 to 30 Aug 2007
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy5()
        {
            rule.DateType = "DayOfWeek";
            rule.DateValue = "Monday-Wednesday;Saturday;Sunday";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);

            for (int i = 0; i < ret.Length; i++)
            {
                DateTime date = DateTime.ParseExact(ret[i], DisplayDateFormat1, CultureInfo.InvariantCulture);
                Assert.IsFalse(date.DayOfWeek == DayOfWeek.Friday, "Wrong GenerateDates implementation.");
                Assert.IsFalse(date.DayOfWeek == DayOfWeek.Thursday, "Wrong GenerateDates implementation.");
            }
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the "5;10;15;20;25" days of the August 2007
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy6()
        {
            rule.DateType = "DayOfMonth";
            rule.DateValue = "5;10;15;20;25";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);

            for (int i = 0; i < ret.Length; i++)
            {
                DateTime date = DateTime.ParseExact(ret[i], DisplayDateFormat1, CultureInfo.InvariantCulture);
                Assert.IsTrue(date.Day % 5 == 0, "Wrong GenerateDates implementation.");
            }
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Tries to get the 32nd day of the August 2007. Must return empty array
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy7()
        {
            rule.DateType = "DayOfMonth";
            rule.DateValue = "32";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);
            Assert.IsTrue(ret.Length == 0, "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the days of the August 2007 when days are given in composite format.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy8()
        {
            rule.DateType = "DayOfMonth";
            rule.DateValue = "1-5;7;10-15;17";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);
            Assert.IsTrue(ret.Length == 14, "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the last 3 days of the August 2007.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy9()
        {
            rule.DateType = "DayOfMonth";
            rule.DateValue = "1-3";
            rule.ReverseOrder = true;

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, StopDate1);
            string[] expected = new string[] { "08/31/2007 00:00", "08/30/2007 00:00", "08/29/2007 00:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the 29th of Feb for all years between 2000 and 2010. Note invalid dates are skipped.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy10()
        {
            rule.DateType = "DayMonth";
            rule.DateValue = "02-29";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
            string[] expected = new string[] { "02/29/2000 00:00", "02/29/2004 00:00", "02/29/2008 00:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the 29th of Feb for all years between 2000 and 2010. Note invalid dates are skipped.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy11()
        {
            rule.DateType = "DayMonth";
            rule.DateValue = "02-29";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1,
                "01/01/2000 00:00", "01/01/2010 00:00");
            string[] expected = new string[] { "02/29/2000 00:00", "02/29/2004 00:00", "02/29/2008 00:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the date 08/07/2007.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy12()
        {
            rule.DateType = "Day";
            rule.DateValue = "08/07/2007";
            rule.TimeValue = "21:00";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
            string[] expected = new string[] { "08/07/2007 21:00" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the date 08/07/2007 but no date is returned as year divisor is specified as 4.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy13()
        {
            rule.DateType = "Day";
            rule.DateValue = "08/07/2007";
            rule.TimeValue = "21:00";
            rule.YearDivisor = 4;

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
            string[] expected = new string[0];

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        /// <summary>
        /// Tests the GenerateDates method for accuracy.
        /// Gets the date 08/07/2007 with a time specified.
        /// </summary>
        [Test]
        public void TestGenerateDatesAccuracy14()
        {
            rule.DateType = "Day";
            rule.DateValue = "08/07/2007";
            rule.TimeValue = "09:45";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
            string[] expected = new string[] { "08/07/2007 09:45" };

            Assert.IsTrue(UnitTestHelper.CompareStringArrays(ret, expected), "Wrong GenerateDates implementation.");
        }

        #endregion

        #region "Failure Tests"

        /// <summary>
        /// Tests the DateType property when value is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestDateTypeFail1()
        {
            rule.DateType = null;
        }

        /// <summary>
        /// Tests the DateType property when value is not valid.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestDateTypeFail2()
        {
            rule.DateType = "Invalid";
        }

        /// <summary>
        /// Tests the DateValue property when value is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestDateValueFail()
        {
            rule.DateValue = null;
        }

        /// <summary>
        /// Tests the TimeValue property when value is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestTimeValueFail()
        {
            rule.TimeValue = null;
        }

        /// <summary>
        /// Tests the YearDivisor when value is negative
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestYearDivisorFail1()
        {
            rule.YearDivisor = -1;
        }

        /// <summary>
        /// Tests the YearDivisor when value is 0
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestYearDivisorFail2()
        {
            rule.YearDivisor = 0;
        }

        /// <summary>
        /// Tests the Interleave property when value is negative.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestInterleaveFail()
        {
            rule.Interleave = -1;
        }

        /// <summary>
        /// Tests the GenerateDates method input date format is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGenerateDatesFail1()
        {
            rule.GenerateDates(null, DisplayDateFormat1, StartDate1, StopDate1);
        }

        /// <summary>
        /// Tests the GenerateDates method display date format is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGenerateDatesFail2()
        {
            rule.GenerateDates(InputDateFormat1, null, StartDate1, StopDate1);
        }

        /// <summary>
        /// Tests the GenerateDates method start date is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGenerateDatesFail3()
        {
            rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, null, StopDate1);
        }

        /// <summary>
        /// Tests the GenerateDates method stop date is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGenerateDatesFail4()
        {
            rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, null);
        }

        /// <summary>
        /// Tests the GenerateDates method input date format is empty
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGenerateDatesFail5()
        {
            rule.GenerateDates("     ", DisplayDateFormat1, StartDate1, StopDate1);
        }

        /// <summary>
        /// Tests the GenerateDates method display date format is empty
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGenerateDatesFail6()
        {
            rule.GenerateDates(InputDateFormat1, "      ", StartDate1, StopDate1);
        }

        /// <summary>
        /// Tests the GenerateDates method start date is empty
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGenerateDatesFail7()
        {
            rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "     ", StopDate1);
        }

        /// <summary>
        /// Tests the GenerateDates method stop date is empty
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGenerateDatesFail8()
        {
            rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, StartDate1, "     ");
        }

        /// <summary>
        /// Tests the GenerateDates method when DateValue is in wrong format for Day DateType
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail9()
        {
            rule.DateType = "Day";
            rule.DateValue = "08 - 07 - 2007";
            rule.TimeValue = "09:45";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
        }

        /// <summary>
        /// Tests the GenerateDates method when DateValue is in wrong format for Day DayMonth
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail10()
        {
            rule.DateType = "DayMonth";
            rule.DateValue = "aa-bb";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
        }

        /// <summary>
        /// Tests the GenerateDates method when DateValue is in wrong format for Day DateType
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail11()
        {
            rule.DateType = "DayOfMonth";
            rule.DateValue = "1;5-10";

            //Must be in HH:mm tt format
            rule.TimeValue = "aa:bb";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
        }

        /// <summary>
        /// Tests the GenerateDates method when DateValue is in wrong format for DateType DayMonth
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail12()
        {
            rule.DateType = "DayOfMonth";
            //Must be numbers not alphabets
            rule.DateValue = "aa;bb";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
        }

        /// <summary>
        /// Tests the GenerateDates method when DateValue is in wrong format for DateType DayOfWeek
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail13()
        {
            rule.DateType = "DayOfWeek";
            //Must be numbers not alphabets
            rule.DateValue = "NoSuchDay";

            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010 00:00");
        }

        /// <summary>
        /// Tests the GenerateDates method when start or end date is not in correct format.
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail14()
        {
            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2000 00:00",
                "01/01/2010");
        }

        /// <summary>
        /// Tests the GenerateDates method when start date is greater than end date.
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail15()
        {
            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2010 00:00",
                "01/01/2000 00:00");
        }

        /// <summary>
        /// Tests the GenerateDates method when date value for DayOfWeek type is invalid.
        /// RuleInvalidDataException is expected
        /// </summary>
        [Test, ExpectedException(typeof(RuleInvalidDataException))]
        public void TestGenerateDatesFail16()
        {
            rule.DateType = "DayOfWeek";
            rule.DateValue = "Monday-9";
            string[] ret = rule.GenerateDates(InputDateFormat1, DisplayDateFormat1, "01/01/2010 00:00",
                "01/01/2000 00:00");
        }
        #endregion
    }
}
