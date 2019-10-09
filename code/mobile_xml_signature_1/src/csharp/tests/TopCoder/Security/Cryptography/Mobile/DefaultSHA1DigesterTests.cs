// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Text;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile.Digesters
{
    /// <summary>
    /// Unit tests for the SHA1Digester class.
    /// </summary>
    [TestFixture]
    public class SHA1DigesterTests
    {
        /// <summary>
        /// The digester instance to use
        /// </summary>
        SHA1Digester sd;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            sd = new SHA1Digester();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sd = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// SHA1Digester()
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(sd, "Digester instance is null");
            Assert.IsTrue(sd is SHA1Digester, "Digester instance has wrong type");
        }

        /// <summary>
        /// Tests the Digest method for accuracy
        /// Also explains some features of the SHA1 digest
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test]
        public void TestDigest()
        {
            string a = "Nevermind";
            byte[] digest1 = sd.Digest(UnicodeEncoding.Unicode.GetBytes(a));

            //Length of digest is always 20
            Assert.AreEqual(digest1.Length, 20);

            string b = "Nevfrmind";
            byte[] digest2 = sd.Digest(UnicodeEncoding.Unicode.GetBytes(b));

            //Length of digest is always 20
            Assert.AreEqual(digest2.Length, 20);

            //Even a small change in string produces a big difference in the digests
            Assert.Greater(CompareByteArrays(digest1 , digest2) , 10);
        }

        /// <summary>
        /// Tests the Digest method for failure when input is null
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test , ExpectedException(typeof(ArgumentNullException))]
        public void TestDigestFail1()
        {
            byte[] input = null;
            byte[] digest1 = sd.Digest(input);
        }

        /// <summary>
        /// Returns the number of characters in the 2 arrays that are different from one another
        /// </summary>
        /// <param name="arr1">array 1</param>
        /// <param name="arr2">array 2</param>
        /// <returns>Returns the number of characters that are different from one another</returns>
        private int CompareByteArrays(byte[] arr1 , byte[] arr2)
        {
            int change = 0;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    change++;
                }
            }
            return change;
        }
    }
}
