// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.FinancialService.Utility
{
    /// <summary>
    /// Unit tests for the SecurityIdDetails class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class SecurityIdDetailsTests
    {
        /// <summary>
        /// The SecurityIdDetails instance to use for the tests.
        /// </summary>
        SecurityIdDetails sid;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            sid = new SecurityIdDetails("A", FinancialMarket.AMEX);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sid = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// SecurityIdDetails(string id, string type)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.AreEqual(sid.Id, "A", "Wrong constructor implementation.");
            Assert.AreEqual(sid.Type, FinancialMarket.AMEX, "Wrong constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor for failure when id is null
        /// SecurityIdDetails(string id, string type)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail1()
        {
            sid = new SecurityIdDetails(null, "ABCD");
        }

        /// <summary>
        /// Tests the constructor for failure when type is null
        /// SecurityIdDetails(string id, string type)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail2()
        {
            sid = new SecurityIdDetails("A", null);
        }

        /// <summary>
        /// Tests the constructor for failure when id is empty
        /// SecurityIdDetails(string id, string type)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail3()
        {
            sid = new SecurityIdDetails("     ", "ABCD");
        }

        /// <summary>
        /// Tests the constructor for failure when type is empty
        /// SecurityIdDetails(string id, string type)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail4()
        {
            sid = new SecurityIdDetails("A", "     ");
        }

        /// <summary>
        /// Tests the Id getter.
        /// </summary>
        [Test]
        public void TestId()
        {
            Assert.AreEqual(sid.Id, "A", "Wrong getter implementation.");
        }

        /// <summary>
        /// Tests the Type getter.
        /// </summary>
        [Test]
        public void TestType()
        {
            Assert.AreEqual(sid.Type, FinancialMarket.AMEX, "Wrong getter implementation.");
        }
    }
}
