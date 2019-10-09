/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 

using System;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Configuration;
using NUnit.Framework;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// Unit tests for the Helper class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HelperTests
    {
        /// <summary>
        /// Tests the ValidateNotNull method when obj is null.
        /// <see cref="ArgumentNullException" /> is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNull1()
        {
            Helper.ValidateNotNull(null, "a");
        }

        /// <summary>
        /// Tests the ValidateNotNull method when obj is not null.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateNotNull2()
        {
            Helper.ValidateNotNull(new object(), "a");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method when str is empty.
        /// <see cref="ArgumentException" /> is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotEmpty1()
        {
            Helper.ValidateNotEmpty("      ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method when str is not empty.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateNotEmpty2()
        {
            Helper.ValidateNotEmpty("   d    ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when str is null.
        /// <see cref="ArgumentNullException" /> is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNullNotEmpty1()
        {
            Helper.ValidateNotNullNotEmpty(null, "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when str is empty.
        /// <see cref="ArgumentException" /> is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotNullNotEmpty2()
        {
            Helper.ValidateNotNullNotEmpty("      ", "a");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when str is not empty and not null.
        /// No exception is expected
        /// </summary>
        [Test]
        public void TestValidateNotNullNotEmpty3()
        {
            Helper.ValidateNotNullNotEmpty("   s   ", "a");
        }

        /// <summary>
        /// Tests the GetSelfDocumentingException method.
        /// </summary>
        [Test]
        public void TestGetSelfDocumentingException()
        {
            Exception e = new Exception("Message");

            SelfDocumentingException sde = Helper.GetSelfDocumentingException(
                e, "My Mesasge", "Cold.Turkey", new string[0], new object[0],
                new string[0], new object[0], new string[0], new object[0]);

            Assert.AreEqual(e, sde.InnerException, "Inner exception is incorrect.");
            Assert.AreEqual("My Mesasge", sde.Message, "Wrong exception message.");
        }

        /// <summary>
        /// Tests the ReadConfig method.
        /// </summary>
        [Test]
        public void TestReadConfig()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            Assert.AreEqual("TestOFNamespace", Helper.ReadConfig(config, "object_factory_ns", true),
                "ReadConfig returns incorrect data.");
        }

        /// <summary>
        /// Tests the ReadConfig method for failure when required key is not found.
        /// <see cref="SelfDocumentingException" /> must be thrown with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestReadConfigFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.RemoveAttribute("object_factory_ns");

            try
            {
                Helper.ReadConfig(config, "object_factory_ns", true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(),
                    "Exception thrown is of wrong type.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Inner exception is of wrong type.");
            }
        }

        /// <summary>
        /// Tests the ReadConfig method for failure when key value is empty string.
        /// <see cref="SelfDocumentingException" /> must be thrown with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestReadConfigFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("object_factory_ns", string.Empty);

            try
            {
                Helper.ReadConfig(config, "object_factory_ns", true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(),
                    "Exception thrown is of wrong type.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Inner exception is of wrong type.");
            }
        }

        /// <summary>
        /// Tests the ReadConfigInt method.
        /// </summary>
        [Test]
        public void TestReadConfigInt()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            Assert.AreEqual(9, Helper.ReadConfigInt(config, "minimum_node_height"),
                "ReadConfigInt returns incorrect data.");
        }

        /// <summary>
        /// Tests the ReadConfigInt method for failure when key value is not a valid integer.
        /// <see cref="SelfDocumentingException" /> must be thrown with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestReadConfigIntFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_node_height", "1.35");

            try
            {
                Helper.ReadConfigInt(config, "minimum_node_height");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(),
                    "Exception thrown is of wrong type.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Inner exception is of wrong type.");
            }
        }

        /// <summary>
        /// Tests the ReadConfigInt method for failure when key value is not a positive integer.
        /// <see cref="SelfDocumentingException" /> must be thrown with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestReadConfigIntFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_node_height", "0");

            try
            {
                Helper.ReadConfigInt(config, "minimum_node_height");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(),
                    "Exception thrown is of wrong type.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Inner exception is of wrong type.");
            }
        }

        /// <summary>
        /// Tests the ReadConfigDouble method.
        /// </summary>
        [Test]
        public void TestReadConfigDouble()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            Assert.AreEqual(2.456, Helper.ReadConfigDouble(config, "font_units"),
                "ReadConfigDouble returns incorrect data.");
        }

        /// <summary>
        /// Tests the ReadConfigDouble method for failure when key value is not a valid double.
        /// <see cref="SelfDocumentingException" /> must be thrown with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestReadConfigDoubleFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("font_units", "1.5A5");

            try
            {
                Helper.ReadConfigDouble(config, "font_units");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(),
                    "Exception thrown is of wrong type.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Inner exception is of wrong type.");
            }
        }

        /// <summary>
        /// Tests the ReadConfigDouble method for failure when key value is not a positive double.
        /// <see cref="SelfDocumentingException" /> must be thrown with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestReadConfigDoubleFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("font_units", "0.0");

            try
            {
                Helper.ReadConfigDouble(config, "font_units");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(),
                    "Exception thrown is of wrong type.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Inner exception is of wrong type.");
            }
        }
    }
}
