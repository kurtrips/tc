// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Diagnostics;
using NUnit.Framework;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ConfigurationManager;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the MBRLogger class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, TopCoder.CodeDoc.CSharp.CoverageExclude]
    public class MBRLoggerTests
    {
        /// <summary>
        /// The MBRLogger instance to use for the tests.
        /// </summary>
        MBRLogger mbrLogger;

        /// <summary>
        /// The Logger instance to use for the tests.
        /// </summary>
        Logger logger;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");

            logger = LogManager.CreateLogger("MyLoggerNamespace");
            mbrLogger = new MBRLogger(logger);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            logger = null;
            mbrLogger = null;
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Tests the constructor.
        /// MBRLogger(Logger logger)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(mbrLogger is MarshalByRefObject, "Wrong type of class.");
        }

        /// <summary>
        /// Tests the constructor when logger is null.
        /// MBRLogger(Logger logger)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            mbrLogger = new MBRLogger(null);
        }

        /// <summary>
        /// Tests the Log method.
        /// void Log(Level level, string message, Object[] param)
        /// </summary>
        [Test]
        public void TestLog1()
        {
            mbrLogger.Log(Level.DEBUG, "Black {0}", new object[] { "Sabbath" });
            CheckForString("Black Sabbath");
        }

        /// <summary>
        /// Tests the Log method.
        /// void Log(string message, Object[] param)
        /// </summary>
        [Test]
        public void TestLog2()
        {
            mbrLogger.Log("Led {0}", new object[] { "Zeppelin" });
            CheckForString("Led Zeppelin");
        }


        /// <summary>
        /// Checks to see if the string is present on the first line of the file.
        /// </summary>
        /// <param name="written">The string to check for.</param>
        private void CheckForString(string written)
        {
            EventLog ea = new EventLog("logger", ".", "logger");

            EventLogEntry[] entries = new EventLogEntry[ea.Entries.Count];
            ea.Entries.CopyTo(entries, 0);

            Assert.AreEqual(written, entries[entries.Length - 1].Message, "String not written to log.");
        }
    }
}
