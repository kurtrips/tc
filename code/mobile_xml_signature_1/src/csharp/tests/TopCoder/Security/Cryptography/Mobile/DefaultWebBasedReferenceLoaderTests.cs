// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile.ReferenceLoaders
{
    /// <summary>
    /// Unit tests for the WebBasedReferenceLoader class.
    /// </summary>
    [TestFixture]
    public class WebBasedReferenceLoaderTests
    {
        /// <summary>
        /// The class instance to use
        /// </summary>
        private WebBasedReferenceLoader obj;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {

        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            obj = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// WebBasedReferenceLoader()
        /// No exception expected
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            obj = new WebBasedReferenceLoader();
            Assert.IsNotNull(obj, "WebBasedReferenceLoader instance is null");
            Assert.IsTrue(obj is WebBasedReferenceLoader, "WebBasedReferenceLoader instance has wrong type");
        }

        /// <summary>
        /// Tests the LoadReferenceData method.
        /// Byte[] LoadReferenceData(string uriString)
        /// No exception expected
        /// </summary>
        [Test]
        public void TestLoadReferenceDataGoogle()
        {
            obj = new WebBasedReferenceLoader();
            string uriString = "http://www.google.com";
            byte[] data = obj.LoadReferenceData(uriString);

            Assert.IsNotNull(data, "Data returned from server is null");

            //Conver to string
            string responseFromServer = new UnicodeEncoding().GetString(data);

            //Verify response is correct
            Assert.IsTrue(responseFromServer.Contains("<html>"), "LoadReferenceData loads incorrect data");
            Assert.IsTrue(responseFromServer.Contains("</html>"), "LoadReferenceData loads incorrect data");
            Assert.IsTrue(responseFromServer.Contains("</head>"), "LoadReferenceData loads incorrect data");
            Assert.IsTrue(responseFromServer.Contains("<head>"), "LoadReferenceData loads incorrect data");
            Assert.IsTrue(responseFromServer.Contains("<title>Google</title>"), 
                "LoadReferenceData loads incorrect data");
            Assert.IsTrue(responseFromServer.Contains("<input name=btnG type=submit value=\"Google Search\">"), 
                "LoadReferenceData loads incorrect data");
        }

        /// <summary>
        /// Tests the LoadReferenceData method.
        /// Byte[] LoadReferenceData(string uriString)
        /// ArgumentNullException expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLoadReferenceDataFail()
        {
            obj = new WebBasedReferenceLoader();
            string uriString = null;
            obj.LoadReferenceData(uriString);
        }

        /// <summary>
        /// Tests the LoadReferenceData method.
        /// Byte[] LoadReferenceData(string uriString)
        /// ReferenceLoadingException expected
        /// </summary>
        [Test, ExpectedException(typeof(ReferenceLoadingException))]
        public void TestLoadReferenceDataFail1()
        {
            obj = new WebBasedReferenceLoader();
            string uriString = "http://www.aabbccddeeff.com/";
            obj.LoadReferenceData(uriString);
        }
    }
}
