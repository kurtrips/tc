// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile
{
    /// <summary>
    /// Unit tests for the Enums in this package.
    /// </summary>
    [TestFixture]
    public class DefaultEnumTests
    {
        /// <summary>
        /// Tests the KeyValueType enum.
        /// </summary>
        [Test]
        public void KeyValueTypeTest()
        {
            Assert.IsTrue(Enum.IsDefined(typeof(KeyValueType), "DSAKeyValue"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyValueType), "RSAKeyValue"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyValueType), 0));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyValueType), 1));

            Assert.IsFalse(Enum.IsDefined(typeof(KeyValueType), 2));
            Assert.IsFalse(Enum.IsDefined(typeof(KeyValueType), "SHAKeyValue"));
        }

        /// <summary>
        /// Tests the KeyInfoProviderType enum.
        /// </summary>
        [Test]
        public void KeyInfoProviderTypeTest()
        {
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "KeyName"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "KeyValue"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "RetrievalMethod"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "X509Data"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "PGPData"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "SKIPData"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), "MgmtData"));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 0));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 1));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 2));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 3));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 4));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 5));
            Assert.IsTrue(Enum.IsDefined(typeof(KeyInfoProviderType), 6));

            Assert.IsFalse(Enum.IsDefined(typeof(KeyInfoProviderType), 7));
            Assert.IsFalse(Enum.IsDefined(typeof(KeyInfoProviderType), "abcd"));
        }
    }
}
