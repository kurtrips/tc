// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Xml;
using System.Reflection;
using NUnit.Framework;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ConfigurationManager;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the XPathResolver class.
    /// </summary>
    ///
    /// <remarks>
    /// IMPORTANT NOTE:
    /// ResolveXPath function is not directly tested here because it depends on the inputDoc private field of the
    /// XPathResolver class to be set. Setting of a private fiels of a class in not possible.
    /// However ResolveXPath is obviously indirectly tested by the TestWriteAPISpecXXX function as they check whether
    /// the xpath is correctly formed or not.
    /// </remarks>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, TopCoder.CodeDoc.CSharp.CoverageExclude]
    public class XPathResolverTests
    {
        /// <summary>
        /// The XPathResolver instance to use for the tests.
        /// </summary>
        XPathResolver xpr;

        /// <summary>
        /// The MBRLogger instance to use for the tests.
        /// </summary>
        MBRLogger mbrLogger;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");

            mbrLogger = new MBRLogger(LogManager.CreateLogger("MyLoggerNamespace"));
            xpr = new XPathResolver();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            xpr = null;
            mbrLogger = null;
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Tests the constructor.
        /// XPathResolver()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(xpr, "Constructor returns null!");
        }

        /// <summary>
        /// Tests the constructor.
        /// XPathResolver(MBRLogger)
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            xpr = new XPathResolver(mbrLogger);
            Assert.IsTrue(UnitTestHelper.GetPrivateField(xpr, "logger") == mbrLogger,
                "references must be same.");
        }

        /// <summary>
        /// Tests the constructor when logger is null.
        /// XPathResolver(MBRLogger)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            xpr = new XPathResolver(null);
        }

        /// <summary>
        /// Tests the AddXPathReferences for when apiSpec is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddXPathReferencesFail1()
        {
            xpr.AddXPathReferences(null);
        }

        /// <summary>
        /// Tests the AddXPathReferences for when apiSpec does not have root with name apispec.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddXPathReferencesFail2()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<a></a>");
            xpr.AddXPathReferences(doc);
        }
    }
}
