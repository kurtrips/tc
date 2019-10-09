// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Globalization;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the ItemValidator class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class ItemValidatorTests
    {
        /// <summary>
        /// The ItemValidator instance to use for the tests
        /// </summary>
        private ItemValidator iv;

        /// <summary>
        /// A valid data source consisting of dates
        /// </summary>
        private IList<DateTime> valid1;

        /// <summary>
        /// A valid data source consisting of strings
        /// </summary>
        private IList<string> valid2;

        /// <summary>
        /// A valid data source consisting of strings with incorrect format
        /// </summary>
        private IList<string> invalid1;

        /// <summary>
        /// A valid data source consisting of objects
        /// </summary>
        private IList<object> invalid2;

        /// <summary>
        /// The Date format to use
        /// </summary>
        private const string DateFormat = "MM/dd/yyyy";

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            iv = new ItemValidator(DateFormat);
            valid1 = new List<DateTime>();
            valid2 = new List<string>();
            invalid1 = new List<string>();
            invalid2 = new List<object>();

            valid1.Add(DateTime.Now);
            valid1.Add(DateTime.Today);

            valid2.Add(DateTime.Now.ToString(DateFormat, CultureInfo.InvariantCulture));
            valid2.Add(DateTime.Today.ToString(DateFormat, CultureInfo.InvariantCulture));

            invalid1.Add(DateTime.Today.ToString("yyyy-dd-MM", CultureInfo.InvariantCulture));

            invalid2.Add(new object());
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            iv = null;
            valid1 = null;
            valid2 = null;
            invalid1 = null;
            invalid2 = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// ItemValidator()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(iv, "Contructor returns null.");
        }

        /// <summary>
        /// Tests the constructor when param is null
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            iv = new ItemValidator(null);
        }

        /// <summary>
        /// Tests the constructor when param is empty
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail2()
        {
            iv = new ItemValidator("   ");
        }

        /// <summary>
        /// Tests the Validate function when list of string is valid.
        /// </summary>
        [Test]
        public void TestValidate1()
        {
            Assert.IsTrue(iv.Validate(valid2), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when IEnumerable datasource is valid
        /// </summary>
        [Test]
        public void TestValidate2()
        {
            Assert.IsTrue(iv.Validate(valid1), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when array of strings is valid
        /// </summary>
        [Test]
        public void TestValidate3()
        {
            Assert.IsTrue(iv.Validate((valid2 as List<string>).ToArray()), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when ListItemCollection is valid
        /// </summary>
        [Test]
        public void TestValidate4()
        {
            ListItemCollection col = new ListItemCollection();
            col.Add(DateTime.Now.ToString(DateFormat, CultureInfo.InvariantCulture));
            col.Add(DateTime.Today.ToString(DateFormat, CultureInfo.InvariantCulture));

            Assert.IsTrue(iv.Validate(col), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when list of string is invalid.
        /// </summary>
        [Test]
        public void TestValidate5()
        {
            Assert.IsFalse(iv.Validate(invalid1), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when array of strings is invalid
        /// </summary>
        [Test]
        public void TestValidate7()
        {
            //Invalid date format
            valid2.Add("2005-12-23");

            Assert.IsFalse(iv.Validate((valid2 as List<string>).ToArray()), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when ListItemCollection is invalid
        /// </summary>
        [Test]
        public void TestValidate8()
        {
            ListItemCollection col = new ListItemCollection();
            col.Add(DateTime.Now.ToString(DateFormat, CultureInfo.InvariantCulture));
            //Wrong format
            col.Add(DateTime.Today.ToString("MM-DD-YYYY", CultureInfo.InvariantCulture));

            Assert.IsFalse(iv.Validate(col), "Wrong Validate implementation.");
        }

        /// <summary>
        /// Tests the Validate function when list of string is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateFail1()
        {
            iv.Validate((List<string>)null);
        }

        /// <summary>
        /// Tests the Validate function when IEnumerable datasource is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateFail2()
        {
            iv.Validate((List<DateTime>)null);
        }

        /// <summary>
        /// Tests the Validate function when array of strings is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateFail3()
        {
            iv.Validate((string[])null);
        }

        /// <summary>
        /// Tests the Validate function when listItemCollection is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateFail4()
        {
            iv.Validate((ListItemCollection)null);
        }
    }
}
