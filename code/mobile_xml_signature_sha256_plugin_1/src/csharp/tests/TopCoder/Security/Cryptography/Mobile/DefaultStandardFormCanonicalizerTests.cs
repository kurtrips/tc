// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author kurtrips

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

        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sfc = null;
            validSignedInfo = null;
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
        /// Tests the BringToCanonicalForm method for accuracy when passed parameter is valid SignedInfo node
        /// string BringToCanonicalForm(string text)
        /// </summary>
        [Test]
        public void TestBringToCanonicalFormFailAccuracy()
        {
            string result = sfc.BringToCanonicalForm(validSignedInfo);
            
            //Verify Result
            //Verify empty elements have ending tags
            Assert.IsTrue(result.IndexOf("</SignatureMethod>") != 1, "Ending tags not closed by canonicalizer");
            Assert.IsTrue(result.IndexOf("</CanonicalizationMethod>") != 1, "Ending tags not closed by canonicalizer");

            //Verify line breaks are normalized
            Assert.IsFalse(result.IndexOf("\r\n") != -1, "Line breaks are not normalized");
            Assert.IsFalse(result.IndexOf("\r") != -1, "Line breaks are not normalized");
        }
    }
}
