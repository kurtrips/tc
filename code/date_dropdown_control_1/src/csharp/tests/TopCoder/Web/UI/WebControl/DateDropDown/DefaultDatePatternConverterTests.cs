// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using NUnit.Framework;

namespace TopCoder.Web.UI.WebControl.DateDropDown
{
    /// <summary>
    /// Unit tests for the DatePatternConverter class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class DatePatternConverterTests
    {
        /// <summary>
        /// The DatePatternConverter to use throughout the tests
        /// </summary>
        DatePatternConverter dpc;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dpc = new DatePatternConverter();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dpc = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// DatePatternConverter()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(dpc, "Constructor returns null!");
        }

        /// <summary>
        /// Tests the CanConvertFrom method when sourceType is string.
        /// bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        /// </summary>
        [Test]
        public void TestCanConvertFrom1()
        {
            Assert.IsTrue(dpc.CanConvertFrom(new CustomTypeDescriptorContext(), typeof(string)),
                "Wrong CanConvertFrom implementation.");
        }

        /// <summary>
        /// Tests the CanConvertFrom method when source type is object
        /// bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        /// </summary>
        [Test]
        public void TestCanConvertFrom2()
        {
            Assert.IsFalse(dpc.CanConvertFrom(new CustomTypeDescriptorContext(), typeof(object)),
                "Wrong CanConvertFrom implementation.");
        }

        /// <summary>
        /// Tests the CanConvertFrom method when context is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCanConvertFromFail1()
        {
            dpc.CanConvertFrom(null, typeof(string));
        }

        /// <summary>
        /// Tests the CanConvertFrom method when sourceType is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCanConvertFromFail2()
        {
            dpc.CanConvertFrom(new CustomTypeDescriptorContext(), null);
        }

        /// <summary>
        /// Tests the CanConvertTo method when sourceType is string.
        /// bool CanConvertFrom(ITypeDescriptorContext, Type)
        /// </summary>
        [Test]
        public void TestCanConvertTo1()
        {
            Assert.IsTrue(dpc.CanConvertTo(new CustomTypeDescriptorContext(), typeof(string)),
                "Wrong CanConvertFrom implementation.");
        }

        /// <summary>
        /// Tests the CanConvertTo method when source type is object
        /// bool CanConvertTo(ITypeDescriptorContext, Type)
        /// </summary>
        [Test]
        public void TestCanConvertTo2()
        {
            Assert.IsFalse(dpc.CanConvertTo(new CustomTypeDescriptorContext(), typeof(object)),
                "Wrong CanConvertFrom implementation.");
        }

        /// <summary>
        /// Tests the CanConvertTo method when context is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCanConvertToFail1()
        {
            dpc.CanConvertTo(null, typeof(string));
        }

        /// <summary>
        /// Tests the CanConvertTo method when sourceType is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestCanConvertToFail2()
        {
            dpc.CanConvertTo(new CustomTypeDescriptorContext(), null);
        }


        /// <summary>
        /// Tests the ConvertFrom method.
        /// Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        /// </summary>
        [Test]
        public void TestConvertFrom()
        {
            string stringRep = File.ReadAllText("../../test_files/TestStringRep.txt");
            object dp = dpc.ConvertFrom(new CustomTypeDescriptorContext(), new CultureInfo("en"), stringRep);

            Assert.IsTrue(dp is DatePattern, "Wrong ConvertFrom implementation.");
            DatePattern objDp = dp as DatePattern;

            //Check the DatePattern formed
            Assert.AreEqual(objDp.DisplayDateFormat, "MM/dd/yyyy", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.InputDateFormat, "yyyy-MM-dd", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.StartDate, "2007-01-01", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.StopDate, "2008-12-31", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.Rules.Count, 14, "Wrong ConvertFrom implementation.");

            //Check one rule also
            Assert.AreEqual(objDp.Rules[3].DateType, "DayMonth", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.Rules[3].DateValue, "02-29", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.Rules[3].Interleave, 0, "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.Rules[3].ReverseOrder, false, "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.Rules[3].TimeValue, "6:00 PM", "Wrong ConvertFrom implementation.");
            Assert.AreEqual(objDp.Rules[3].YearDivisor, 4, "Wrong ConvertFrom implementation.");
        }

        /// <summary>
        /// Tests the ConvertFrom method when the string is invalid representation of a DatePattern object.
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConvertFromFail1()
        {
            dpc.ConvertFrom(null, CultureInfo.InvariantCulture, "abc");
        }

        /// <summary>
        /// Tests the ConvertTo method.
        /// Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        /// </summary>
        [Test]
        public void TestConvertTo()
        {
            string stringRep = File.ReadAllText("../../test_files/TestStringRep.txt");
            object dp = dpc.ConvertFrom(new CustomTypeDescriptorContext(), new CultureInfo("en"), stringRep);

            //Convert back to string
            string converted = (string)dpc.ConvertTo(new CustomTypeDescriptorContext(), new CultureInfo("en"),
                dp, typeof(string));
            Assert.AreEqual(stringRep, converted, "Wrong ConvertTo implementation.");
        }

        /// <summary>
        /// Tests the ConvertTo method when culture is null.
        /// Object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, Object value, Type destinationType)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConvertToFail1()
        {
            dpc.ConvertTo(new CustomTypeDescriptorContext(), new CultureInfo("en"), new DatePattern(), null);
        }
    }

    /// <summary>
    /// A cutom implementation of ITypeDescriptorContext for usage in the tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class CustomTypeDescriptorContext : ITypeDescriptorContext
    {
        /// <summary>
        /// Custom implementation of the base interface method
        /// </summary>
        /// <value>null</value>
        public IContainer Container
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Custom implementation of the base interface method
        /// </summary>
        /// <value>null</value>
        public object Instance
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Custom implementation of the base interface method
        /// </summary>
        /// <value>null</value>
        public PropertyDescriptor PropertyDescriptor
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Custom implementation of the base interface method
        /// </summary>
        public void OnComponentChanged() { }

        /// <summary>
        /// Custom implementation of the base interface method
        /// </summary>
        public bool OnComponentChanging()
        {
            return false;
        }

        /// <summary>
        /// Custom implementation of the base interface method
        /// </summary>
        public object GetService(Type serviceType)
        {
            return null;
        }
    }
}
