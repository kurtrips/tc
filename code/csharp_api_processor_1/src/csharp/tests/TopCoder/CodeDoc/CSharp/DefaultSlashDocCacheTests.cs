// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ConfigurationManager;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the SlashDocCache class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, TopCoder.CodeDoc.CSharp.CoverageExclude]
    public class SlashDocCacheTests
    {
        /// <summary>
        /// SlashDocCache instance to use for the tests.
        /// </summary>
        SlashDocCache sdc;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");
            sdc = new SlashDocCache();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sdc = null;
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Tests the constructor.
        /// SlashDocCache()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsNull(UnitTestHelper.GetPrivateField(sdc, "logger"), "Logger must be null.");
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(sdc, "docs"), "doc must be a new hashtable.");
        }

        /// <summary>
        /// Tests the constructor.
        /// SlashDocCache(MBRLogger logger)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            MBRLogger logger = new MBRLogger(LogManager.CreateLogger("MyLoggerNamespace"));
            sdc = new SlashDocCache(logger);
            Assert.IsTrue(object.ReferenceEquals(UnitTestHelper.GetPrivateField(sdc, "logger"), logger),
                "Wrong constructor implementation");
        }

        /// <summary>
        /// Tests the constructor when logger is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            sdc = new SlashDocCache(null);
        }

        /// <summary>
        /// Tests the AddSlashDocFile method. Total keys in the docs should be 53.
        /// void AddSlashDocFile(string fileName)
        /// </summary>
        [Test]
        public void TestAddSlashDocFile()
        {
            MBRLogger logger = new MBRLogger(LogManager.CreateLogger("MyLoggerNamespace"));
            sdc = new SlashDocCache(logger);

            sdc.AddSlashDocFile(UnitTestHelper.MOCKXMLPATH);

            //Check a few entries in the hashtable
            Hashtable tab = (Hashtable)UnitTestHelper.GetPrivateField(sdc, "docs");

            Assert.AreEqual(tab.Count, 53, "Wrong AddSlashDocFile implementation.");
            Assert.IsTrue(tab.ContainsKey("M:MockLibrary.Nested.ClassC.ClassD.#ctor(System.Int32)"),
                "Wrong AddSlashDocFile implementation.");
            Assert.IsTrue(tab.ContainsKey("M:MockLibrary.WakeMeUp.AlarmRang(System.Object,System.EventArgs)"),
                "Wrong AddSlashDocFile implementation.");
            Assert.IsTrue(tab.ContainsKey("E:MockLibrary.WakeMeUp.Alarm"),
                "Wrong AddSlashDocFile implementation.");
        }

        /// <summary>
        /// Tests the AddSlashDocFile method when file is not found.
        /// No exception is expected.
        /// void AddSlashDocFile(string fileName)
        /// </summary>
        [Test]
        public void TestAddSlashDocFile1()
        {
            MBRLogger logger = new MBRLogger(LogManager.CreateLogger("MyLoggerNamespace"));
            sdc = new SlashDocCache(logger);

            sdc.AddSlashDocFile("../../noSuch/skunk.xml");

            //Check a few entries in the hashtable
            Hashtable tab = (Hashtable)UnitTestHelper.GetPrivateField(sdc, "docs");

            Assert.AreEqual(tab.Count, 0, "Wrong AddSlashDocFile implementation.");
        }

        /// <summary>
        /// Tests the AddSlashDocFile method when file name is null
        /// void AddSlashDocFile(string fileName)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddSlashDocFileFail1()
        {
            sdc.AddSlashDocFile(null);
        }

        /// <summary>
        /// Tests the AddSlashDocFile method when file name is empty string
        /// void AddSlashDocFile(string fileName)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddSlashDocFileFail2()
        {
            sdc.AddSlashDocFile("        ");
        }

        /// <summary>
        /// Tests the AddSlashDocFiles method. Adds the same file twice. No exception must be thrown.
        /// The total count of the keys must still be 53.
        /// void AddSlashDocFiles(String[] fileNames)
        /// </summary>
        [Test]
        public void TestAddSlashDocFiles()
        {
            MBRLogger logger = new MBRLogger(LogManager.CreateLogger("MyLoggerNamespace"));
            sdc = new SlashDocCache(logger);

            sdc.AddSlashDocFiles(
                new string[] { UnitTestHelper.MOCKXMLPATH, UnitTestHelper.MOCKXMLPATH });

            Hashtable tab = (Hashtable)UnitTestHelper.GetPrivateField(sdc, "docs");
            Assert.AreEqual(tab.Count, 53, "Wrong AddSlashDocFile implementation.");
        }

        /// <summary>
        /// Tests the AddSlashDocFiles method when fileNames is null.
        /// void AddSlashDocFiles(String[] fileNames)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddSlashDocFilesFail1()
        {
            sdc.AddSlashDocFiles(null);
        }

        /// <summary>
        /// Tests the AddSlashDocFiles method when fileNames has empty element.
        /// void AddSlashDocFiles(String[] fileNames)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddSlashDocFilesFail2()
        {
            sdc.AddSlashDocFiles(new string[] { " " });
        }

        /// <summary>
        /// Tests the AddSlashDocFiles method when fileNames has null element.
        /// void AddSlashDocFiles(String[] fileNames)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddSlashDocFilesFail3()
        {
            sdc.AddSlashDocFiles(new string[] { null });
        }

        /// <summary>
        /// Tests the Item indexer.
        /// string Item(string uniqueID)
        /// </summary>
        [Test]
        public void TestItem()
        {
            //First add a doc file
            TestAddSlashDocFile();

            string doc = sdc["E:MockLibrary.WakeMeUp.Alarm"];
            Assert.AreEqual(doc, "<summary>\r\n            An event\r\n            </summary>", "Wrong documentation.");
        }

        /// <summary>
        /// Tests the Item indexer when uniqueID is null
        /// string Item(string uniqueID)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestItemFail1()
        {
            string s = sdc[null];
        }

        /// <summary>
        /// Tests the Item indexer when uniqueID is empty.
        /// string Item(string uniqueID)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestItemFail2()
        {
            string s = sdc["         "];
        }

        /// <summary>
        /// Tests the UniqueIDs property.
        /// </summary>
        [Test]
        public void TestUniqueIDs()
        {
            //First add a doc file
            TestAddSlashDocFile();

            Assert.AreEqual(sdc.UniqueIDs.Length, 53, "Incorrect UniqueIDs implememntation.");
        }
    }
}
