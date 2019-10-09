// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile.Canonicalizers
{
    /// <summary>
    /// Unit tests for the StandardFormCanonicalizer class.
    /// </summary>
    [TestFixture]
    public class StandardFormCanonicalizerTests
    {
        /// <summary>
        /// The StandardFormCanonicalizer instance to use throughout tests
        /// </summary>
        private StandardFormCanonicalizer sfc;

        /// <summary>
        /// A valid SignedInfo node
        /// </summary>
        private string validSignedInfo;

        /// <summary>
        /// An invalid SignedInfo node
        /// </summary>
        private string invalidSignedInfo;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            sfc = new StandardFormCanonicalizer();

            //Create valid signedInfo node
            validSignedInfo = "";
            validSignedInfo += "<SignedInfo Id=\"myFirstSign\">";
            validSignedInfo += "<CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\"";
            validSignedInfo += "/><SignatureMethod Algorithm=\"xml:dig:signer:rsa-dsa\" />";
            validSignedInfo += "<Reference URI=\"http://www.google.com\"><DigestMethod>";
            validSignedInfo += "http://www.w3.org/2000/09/xmldsig#sha1</DigestMethod><DigestValue>";
            validSignedInfo += "bTKJD7cSqIIsDioMYGfAvb9hNug=</DigestValue></Reference></SignedInfo>";

            //Create invalid signedInfo node
            invalidSignedInfo = "";
            invalidSignedInfo += "<SignedInfo Id=\"myFirstSign\"><CanonicalizationMethod ";
            invalidSignedInfo += "Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\"/>";
            invalidSignedInfo += "<Reference URI=\"http://www.google.com\"><DigestMethod>";
            invalidSignedInfo += "http://www.w3.org/2000/09/xmldsig#sha1</DigestMethod><DigestValue>";
            invalidSignedInfo += "bTKJD7cSqIIsDioMYGfAvb9hNug=</DigestValue></Reference></SignedInfo>";

        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sfc = null;
            validSignedInfo = null;
            invalidSignedInfo = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// StandardFormCanonicalizer()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsNotNull(sfc, "StandardFormCanonicalizer instance is null");
            Assert.IsTrue(sfc is StandardFormCanonicalizer, "StandardFormCanonicalizer instance has wrong type");
        }

        /// <summary>
        /// Tests the get_Encoding method.
        /// Encoding get_Encoding()
        /// </summary>
        [Test]
        public void Testget_Encoding()
        {
            Assert.IsNotNull(sfc.Encoding, "Encoding property returns null");
            Assert.IsTrue(sfc.Encoding is System.Text.UTF8Encoding,
                "Encoding property has incorrect type");
        }

        /// <summary>
        /// Tests the BringToCanonicalForm method for failure when parameter is null
        /// string BringToCanonicalForm(string text)
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestBringToCanonicalFormFail()
        {
            string s = null;
            sfc.BringToCanonicalForm(s);
        }

        /// <summary>
        /// Tests the BringToCanonicalForm method for failure when passed parameter is invalid xml string
        /// string BringToCanonicalForm(string text)
        /// </summary>
        [Test, ExpectedException(typeof(CanonicalizationException))]
        public void TestBringToCanonicalFormFail1()
        {
            string s = "abc";
            sfc.BringToCanonicalForm(s);
        }

        /// <summary>
        /// Tests the BringToCanonicalForm method for failure when passed parameter is not valid SignedInfo node
        /// string BringToCanonicalForm(string text)
        /// </summary>
        [Test, ExpectedException(typeof(CanonicalizationException))]
        public void TestBringToCanonicalFormFail2()
        {
            sfc.BringToCanonicalForm(invalidSignedInfo);
        }

        /// <summary>
        /// Tests the BringToCanonicalForm method for accuracy when passed parameter is valid SignedInfo node
        /// string BringToCanonicalForm(string text)
        /// </summary>
        [Test]
        public void TestBringToCanonicalFormFailAccuracy()
        {
            string result = sfc.BringToCanonicalForm(validSignedInfo);
            
            //Verify Result
            //Verify empty elements have ending tags
            Assert.IsTrue(result.Contains("</SignatureMethod>"), "Ending tags not closed by canonicalizer");
            Assert.IsTrue(result.Contains("</CanonicalizationMethod>"), "Ending tags not closed by canonicalizer");

            //Verify line breaks are normalized
            Assert.IsFalse(result.Contains("\r\n"), "Line breaks are not normalized");
            Assert.IsFalse(result.Contains("\r"), "Line breaks are not normalized");
        }
    }
}
