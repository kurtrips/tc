// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Text;
using System.Globalization;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile.Digesters
{
    /// <summary>
    /// Unit tests for the SHA256Digester class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    /// <remarks>
    /// These test cases runs official NIST test cases available from the
    /// "SHA Test Vectors for Hashing Byte-Oriented Messages" link at http://csrc.nist.gov/cryptval/shs.htm
    /// </remarks>
    [TestFixture]
    public class SHA256DigesterTests
    {
        /// <summary>
        /// The digester instance to use
        /// </summary>
        SHA256Digester sd;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            sd = new SHA256Digester();
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
        /// SHA256Digester()
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(sd, "Digester instance is null");
            Assert.IsTrue(sd is SHA256Digester, "Digester instance has wrong type");
        }

        /// <summary>
        /// Tests the Digest method for accuracy
        /// Also explains some features of the SHA256Digester
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test]
        public void TestDigest()
        {
            string a = "Nevermind";
            byte[] digest1 = sd.Digest(UnicodeEncoding.Unicode.GetBytes(a));

            //Length of digest is always 32
            Assert.AreEqual(digest1.Length, 32, "Digest length must always be 32 bytes");

            string b = "Nevfrmind";
            byte[] digest2 = sd.Digest(UnicodeEncoding.Unicode.GetBytes(b));

            //Length of digest is always 32
            Assert.AreEqual(digest2.Length, 32, "Digest length must always be 32 bytes");

            //Even a small change in string produces a big difference in the digests
            Assert.IsTrue(CompareByteArrays(digest1 , digest2) > 10);
        }

        /// <summary>
        /// Tests the Digest method for accuracy when input byte array is empty
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test]
        public void TestDigestEmptyString()
        {
            string a = "";
            byte[] calculated = sd.Digest(UnicodeEncoding.Unicode.GetBytes(a));

            //AS per Wikipedia, SHA256("") = e3b0c442 98fc1c14 9afbf4c8 996fb924 27ae41e4 649b934c a495991b 7852b855
            byte[] expected = new byte[] {0xe3, 0xb0, 0xc4, 0x42, 0x98, 0xfc, 0x1c, 0x14, 0x9a, 0xfb, 0xf4, 0xc8,
                                          0x99, 0x6f, 0xb9, 0x24, 0x27, 0xae, 0x41, 0xe4, 0x64, 0x9b, 0x93, 0x4c,
                                          0xa4, 0x95, 0x99, 0x1b, 0x78, 0x52, 0xb8, 0x55};

            Assert.IsTrue(VerifyEquality(calculated, expected), "Calculated and expected digest are different");
        }

        /// <summary>
        /// Tests the Digest method for accuracy.
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test]
        public void TestDigestQuickBrownFox()
        {
            string a = "The quick brown fox jumps over the lazy dog";
            byte[] calculated = sd.Digest(UTF8Encoding.UTF8.GetBytes(a));

            //AS per Wikipedia, SHA256("The quick brown fox jumps over the lazy dog") =
            //d7a8fbb3 07d78094 69ca9abc b0082e4f 8d5651e4 6d3cdb76 2d02d0bf 37c9e592

            byte[] expected = new byte[] {0xd7, 0xa8, 0xfb, 0xb3, 0x07, 0xd7, 0x80, 0x94, 0x69, 0xca, 0x9a, 0xbc, 0xb0,
                                          0x08, 0x2e, 0x4f, 0x8d, 0x56, 0x51, 0xe4, 0x6d, 0x3c, 0xdb, 0x76, 0x2d, 0x02,
                                          0xd0, 0xbf, 0x37, 0xc9, 0xe5, 0x92};

            Assert.IsTrue(VerifyEquality(calculated, expected), "Calculated and expected digest are different");
        }

        /// <summary>
        /// Tests the Digest method for accuracy using NIST test vectors for short strings.
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test]
        public void TestDigestNISTVectorShortMessage()
        {
            TestUsingNISTTestVectors(@"../../test_files/SHA256ShortMsg.txt");
        }

        /// <summary>
        /// Tests the Digest method for accuracy using NIST test vectors for long strings.
        /// Byte[] Digest(Byte[] inputData)
        /// </summary>
        [Test]
        public void TestDigestNISTVectorLongMessage()
        {
            TestUsingNISTTestVectors(@"../../test_files/SHA256LongMsg.txt");
        }

        /// <summary>
        /// Tests the Digest method for failure when input is null.
        /// Byte[] Digest(Byte[] inputData).
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
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

        /// <summary>
        /// Verifies if 2 byte arrays are equal.
        /// </summary>
        /// <param name="arr1">Array 1</param>
        /// <param name="arr2">Array 2</param>
        /// <returns>True if equal else false</returns>
        private bool VerifyEquality(byte[] arr1, byte[] arr2)
        {
            //Must have the same length
            if (arr1.Length != arr2.Length)
            {
                return false;
            }

            //All bytes must be equal
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Reads the NIST test vector files, calculates the digest of the messages and compares them with
        /// the expected messages.
        /// </summary>
        /// <param name="fileName">The file to read</param>
        private void TestUsingNISTTestVectors(string fileName)
        {
            // Read the file
            System.IO.StreamReader file =
               new System.IO.StreamReader(fileName);

            int counter = 0, len = 0;
            string line;
            byte[] message = new byte[0], expectedHash = new byte[0], calculatedHash = new byte[0];

            //Read line by line
            while ((line = file.ReadLine()) != null)
            {
                if (line.StartsWith("Len = "))
                {
                    //Get length of message in bits
                    len = Convert.ToInt32(line.Substring(6));
                }
                else if (line.StartsWith("Msg = "))
                {
                    //Convert the message string in file to a byte array
                    message = new byte[len / 8];
                    for (int i = 6; i < 6 + (len / 4); i += 2)
                    {
                        message[(i - 6) / 2] = byte.Parse(line.Substring(i, 2), NumberStyles.HexNumber);
                    }

                    //Get the hash of the message
                    calculatedHash = sd.Digest(message);
                }
                else if (line.StartsWith("MD = "))
                {
                    //Convert the message digest (MD) string in file to a byte array
                    expectedHash = new byte[(line.Length - 5) / 2];
                    for (int i = 5; i < line.Length; i += 2)
                    {
                        expectedHash[(i - 5) / 2] = byte.Parse(line.Substring(i, 2), NumberStyles.HexNumber);
                    }

                    //Verify that the expected digest and the calcualted digests are equal
                    Assert.IsTrue(VerifyEquality(calculatedHash, expectedHash),
                        "Calculated and expected digest are different for input of length " + len + " bits.");
                }
                else
                {
                    continue;
                }

                counter++;
            }

            file.Close();
        }
    }
}
