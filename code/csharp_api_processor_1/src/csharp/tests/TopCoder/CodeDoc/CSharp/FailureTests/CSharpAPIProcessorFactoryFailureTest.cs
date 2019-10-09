/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp;
using TopCoder.Configuration;
using TopCoder.Util.CommandLine;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="CSharpAPIProcessorFactory"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class CSharpAPIProcessorFactoryFailureTest
    {
        /// <summary>
        /// <para>The CSharpAPIProcessorFactory instance used for testing.</para>
        /// </summary>
        private CSharpAPIProcessorFactory factory;

        /// <summary>
        /// Set up the testing environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            factory = new CSharpAPIProcessorFactory();
        }

        /// <summary>
        /// Tests the accuracy of the constructor <c>CSharpAPIProcessorFactory()</c>.
        /// The instance should be initialized correctly.
        /// </summary>
        [Test]
        public void TestCtorAccuracy()
        {
            new CSharpAPIProcessorFactory();
        }

        /// <summary>
        /// Tests ConfigureCommandLineParser method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConfigureCommandLineParser_Null1()
        {
            factory.ConfigureCommandLineParser(null, new DefaultConfiguration("config"));
        }

        /// <summary>
        /// Tests ConfigureCommandLineParser method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConfigureCommandLineParser_Null2()
        {
            factory.ConfigureCommandLineParser(new CommandLineParser(), null);
        }

        /// <summary>
        /// Tests NewXmlProcessor method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestNewXmlProcessor_Null1()
        {
            factory.NewXmlProcessor((CommandLineSwitch[])null, new DefaultConfiguration("config"));
        }
        
        /// <summary>
        /// Tests NewXmlProcessor method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestNewXmlProcessor_Null2()
        {
            factory.NewXmlProcessor(new CommandLineSwitch[] { new CommandLineSwitch("switch") }, null);
        }

        /// <summary>
        /// Tests NewXmlProcessor method with null element in array.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestNewXmlProcessor_NullElement()
        {
            factory.NewXmlProcessor(new CommandLineSwitch[] { null }, new DefaultConfiguration("config"));
        }
    }
}
