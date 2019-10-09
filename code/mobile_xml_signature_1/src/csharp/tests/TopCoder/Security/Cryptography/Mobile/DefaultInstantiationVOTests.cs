// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the InstantiationVO class.
    /// </summary>
    [TestFixture]
    public class DefaultInstantiationVOTests
    {
        /// <summary>
        /// The InstantiationVO instance to use throughout the test suite
        /// </summary>
        InstantiationVO ivo;

        /// <summary>
        /// The paramneters collection to use throughout the test suite
        /// </summary>
        IDictionary<string, object> dic;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            dic = new Dictionary<string, object>();
            short aShortVal = 10;
            dic.Add("abcd", aShortVal);
            ivo = new InstantiationVO("key", dic);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            dic = null;
            ivo = null;
        }

        /// <summary>
        /// Tests the constructor for accuracy
        /// </summary>
        [Test]
        public void ConstructorTest()
        {
            Assert.IsNotNull(ivo, "InstantiationVO instance is null");
            Assert.IsTrue(ivo is InstantiationVO, "InstantiationVO instance has wrong type");
        }

        /// <summary>
        /// Tests the constructor for failure
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTestFail1()
        {
            ivo = new InstantiationVO((string)null, dic);
        }

        /// <summary>
        /// Tests the constructor for failure
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ConstructorTestFail2()
        {
            ivo = new InstantiationVO("    ", dic);
        }

        /// <summary>
        /// Tests the constructor for failure
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTestFail3()
        {
            ivo = new InstantiationVO("abcd", (IDictionary<string,object>)null);
        }

        /// <summary>
        /// Tests the constructor for failure
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ConstructorTestFail4()
        {
            dic.Add("               ", "abcdef");
            ivo = new InstantiationVO("abcd", dic);
        }

        /// <summary>
        /// Tests the constructor for failure
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void ConstructorTestFail5()
        {
            dic.Add("abcdef", (string)null);
            ivo = new InstantiationVO("abcd", dic);
        }

        /// <summary>
        /// Tests the Key property of InstantiationVO class
        /// </summary>
        [Test]
        public void KeyPropertyTest()
        {
            Assert.AreEqual(ivo.Key, "key");
        }

        /// <summary>
        /// Tests the Params property of InstantiationVO class
        /// </summary>
        [Test]
        public void PramsPropertyTest()
        {
            IDictionary<string, object> paramC = ivo.Params;
            Assert.AreEqual(paramC["abcd"] , (short)(10));
            Assert.AreEqual(paramC.Count, 1);
        }
    }
}
