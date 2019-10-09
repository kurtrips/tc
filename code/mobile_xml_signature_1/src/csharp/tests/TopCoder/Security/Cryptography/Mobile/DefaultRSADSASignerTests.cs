// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile.Signers
{
    /// <summary>
    /// Unit tests for the DSASigner class.
    /// </summary>
    [TestFixture]
    public class DSASignerTests
    {
        /// <summary>
        /// The signer instance to use for all the tests
        /// </summary>
        DSASigner signer;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            signer = new DSASigner();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            signer = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// RSADSASigner()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(signer, "DSASigner instance is null");
            Assert.IsTrue(signer is DSASigner, "DSASigner instance has wrong type");
        }

        /// <summary>
        /// Tests the DSAKeyInfo method.
        /// </summary>
        [Test]
        public void Test_DSAKeyInfo()
        {
            DSA dsa = new DSACryptoServiceProvider();
            DSAParameters dsaParams = dsa.ExportParameters(true);
            signer.DSAKeyInfo = dsaParams;

            Assert.AreEqual(signer.DSAKeyInfo.GetType(), typeof(DSAParameters),
                "DSAKeyInfo property is of incorrect type");
            Assert.AreEqual(signer.DSAKeyInfo, dsaParams, "DSAKeyInfo property holds incorrect value");
        }

        /// <summary>
        /// Tests the Sign method.
        /// string Sign(Byte[] data)
        /// </summary>
        [Test]
        public void TestSignAccuracy()
        {
            //Set DSA parameters
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider();
            //Include private key
            signer.DSAKeyInfo = dsa.ExportParameters(true);

            string a = "Nirvana";

            //Signer needs hash so create it
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(a));

            string signed = signer.Sign(hash);

            Console.Out.WriteLine("The ouptut of DSASigner's Sign method for input " + a + " is:");
            Console.Out.WriteLine(signed);
            
        }

        /// <summary>
        /// Tests the Sign method for failure when input is null
        /// string Sign(Byte[] data)
        /// </summary>
        [Test , ExpectedException(typeof(ArgumentNullException))]
        public void TestSignFail1()
        {
            //Set DSA parameters
            DSA dsa = new DSACryptoServiceProvider();
            //Include private key
            DSAParameters dsaParam = dsa.ExportParameters(true);
            signer.DSAKeyInfo = dsaParam;

            byte[] a = null;
            string signed = signer.Sign(a);
        }

        /// <summary>
        /// Tests the Sign method for failure when private key is not included
        /// string Sign(Byte[] data)
        /// </summary>
        [Test, ExpectedException(typeof(SigningException))]
        public void TestSignFail2()
        {
            //Set DSA parameters
            DSA dsa = new DSACryptoServiceProvider();
            //Do not Include private key
            DSAParameters dsaParams = dsa.ExportParameters(false);
            signer.DSAKeyInfo = dsaParams;

            string a = "Nirvana";
            string signed = signer.Sign(Encoding.Unicode.GetBytes(a));
        }
    }

    /// <summary>
    /// Unit tests for the RSASigner class.
    /// </summary>
    [TestFixture]
    public class RSASignerTests
    {
        /// <summary>
        /// The signer instance to use for all the tests
        /// </summary>
        RSASigner signer;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            signer = new RSASigner();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            signer = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// RSARSASigner()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(signer, "RSASigner instance is null");
            Assert.IsTrue(signer is RSASigner, "RSASigner instance has wrong type");
        }

        /// <summary>
        /// Tests the RSAKeyInfo method.
        /// </summary>
        [Test]
        public void Test_RSAKeyInfo()
        {
            RSA rsa = new RSACryptoServiceProvider();
            RSAParameters rsaParams = rsa.ExportParameters(true);
            signer.RSAKeyInfo = rsaParams;

            Assert.AreEqual(signer.RSAKeyInfo.GetType(), typeof(RSAParameters),
                "RSAKeyInfo property is of incorrect type");
            Assert.AreEqual(signer.RSAKeyInfo, rsaParams, "RSAKeyInfo property holds incorrect value");
        }

        /// <summary>
        /// Tests the Sign method.
        /// string Sign(Byte[] data)
        /// </summary>
        [Test]
        public void TestSignAccuracy()
        {
            //Set RSA parameters
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //Include private key
            signer.RSAKeyInfo = rsa.ExportParameters(true);

            string a = "Nirvana";

            //Signer needs hash so create it
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            byte[] hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(a));

            string signed = signer.Sign(hash);

            Console.Out.WriteLine("The ouptut of RSASigner's Sign method for input " + a + " is:");
            Console.Out.WriteLine(signed);
            
        }

        /// <summary>
        /// Tests the Sign method for failure when input is null
        /// string Sign(Byte[] data)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestSignFail1()
        {
            //Set RSA parameters
            RSA rsa = new RSACryptoServiceProvider();
            //Include private key
            RSAParameters rsaParam = rsa.ExportParameters(true);
            signer.RSAKeyInfo = rsaParam;

            byte[] a = null;
            string signed = signer.Sign(a);
        }

        /// <summary>
        /// Tests the Sign method for failure when private key is not included
        /// string Sign(Byte[] data)
        /// </summary>
        [Test, ExpectedException(typeof(SigningException))]
        public void TestSignFail2()
        {
            //Set RSA parameters
            RSA rsa = new RSACryptoServiceProvider();
            //Do not Include private key
            RSAParameters rsaParams = rsa.ExportParameters(false);
            signer.RSAKeyInfo = rsaParams;

            string a = "Nirvana";
            string signed = signer.Sign(Encoding.Unicode.GetBytes(a));
        }
    }
}
