// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Web.Services.Protocols;
using System.Diagnostics;
using NUnit.Framework;

namespace TopCoder.Security.Cryptography.Mobile.ReferenceLoaders
{
    /// <summary>
    /// Unit tests for the SoapMessageReferenceLoader class.
    /// </summary>
    [TestFixture]
    public class SoapMessageReferenceLoaderTests
    {
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
        }

        /// <summary>
        /// Tests the SoapMessageReferenceLoader. This class can only be tested from within a 
        /// Web Service Client (because the SoapMessage variable of the class cannot be instantiated and
        /// is available from a SoapExtension using Web Service only).
        /// This test starts the Web Service using Process class of .Net framework.
        /// Please refer to the Call_Xml_Signature function in the code of Web Service for more details. 
        /// The code is available at test_files/TestWebServiceClient/Code folder.
        /// 
        /// Test is successful if no exception is encountered.
        /// </summary>
        [Test]
        public void Test()
        {
            Process webServiceClient = new Process();
            string path = "../../test_files/TestWebServiceClient/TestWSClient.exe";

            webServiceClient.StartInfo.FileName = path;
            webServiceClient.Start();
        }
    }
}
