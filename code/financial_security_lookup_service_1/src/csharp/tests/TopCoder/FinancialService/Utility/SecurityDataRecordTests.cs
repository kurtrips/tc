// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the SecurityDataRecord class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SecurityDataRecordTests
    {
        /// <summary>
        /// The SecurityDataRecord instance to use for the tests.
        /// </summary>
        SecurityDataRecord sdr;

        /// <summary>
        /// The SecurityData to use for the SecurityDataRecord instance.
        /// </summary>
        SecurityData sd;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            sd = new SecurityData("A", "B");
            sdr = new SecurityDataRecord(sd, true);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sd = null;
            sdr = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.AreEqual(sdr.SecurityData, sd, "Wrong constructor implementation.");
            Assert.AreEqual(sdr.IsLookedUp, true, "Wrong constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor for failure when securityData is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail11()
        {
            sdr = new SecurityDataRecord(null, true);
        }

        /// <summary>
        /// Tests the SecurityData getter.
        /// </summary>
        [Test]
        public void TestSecurityData()
        {
            Assert.AreEqual(sdr.SecurityData, sd, "Wrong getter implementation.");
        }

        /// <summary>
        /// Tests the IsLookedUp getter.
        /// </summary>
        [Test]
        public void TestIsLookedUp()
        {
            Assert.AreEqual(sdr.IsLookedUp, true, "Wrong getter implementation.");
        }
    }
}
