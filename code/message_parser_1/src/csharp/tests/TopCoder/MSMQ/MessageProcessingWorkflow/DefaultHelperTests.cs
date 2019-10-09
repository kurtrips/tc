// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow.Parsers
{
    /// <summary>
    /// Unit tests for the Helper class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class HelperTests
    {
        /// <summary>
        /// Tests the ValidateNotEmpty method when string is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotEmptyFail()
        {
            Helper.ValidateNotEmpty("  ", "abc");
        }

        /// <summary>
        /// Tests the ValidateNotEmpty method when string is not empty.
        /// No exception is expected.
        /// </summary>
        [Test]
        public void TestValidateNotEmpty()
        {
            Helper.ValidateNotEmpty("  a  ", "abc");
        }

        /// <summary>
        /// Tests the ValidateNotNull method when object is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNullFail()
        {
            Helper.ValidateNotNull(null, "abc");
        }

        /// <summary>
        /// Tests the ValidateNotNull method when object is not null.
        /// No exception is expected.
        /// </summary>
        [Test]
        public void TestValidateNotNull()
        {
            Helper.ValidateNotNull(new object(), "abc");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when string is not null and not empty.
        /// No exception is expected.
        /// </summary>
        [Test]
        public void TestValidateNotNullNotEmpty()
        {
            Helper.ValidateNotNullNotEmpty(" a  ", "abc");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when string is null.
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestValidateNotNullNotEmptyFail1()
        {
            Helper.ValidateNotNullNotEmpty(null, "abc");
        }

        /// <summary>
        /// Tests the ValidateNotNullNotEmpty method when string is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateNotNullNotEmptyFail2()
        {
            Helper.ValidateNotNullNotEmpty("        ", "abc");
        }

        /// <summary>
        /// Tests the ValidateCollection method when dictionary is null.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateCollectionFail1()
        {
            Helper.ValidateCollection<string>(null, "dic");
        }

        /// <summary>
        /// Tests the ValidateCollection method when dictionary is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateCollectionFail2()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Helper.ValidateCollection<string>(dic, "dic");
        }

        /// <summary>
        /// Tests the ValidateCollection method when dictionary key is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateCollectionFail3()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("     ", "abcd");
            Helper.ValidateCollection<string>(dic, "dic");
        }

        /// <summary>
        /// Tests the ValidateCollection method when dictionary value is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateCollectionFail4()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(" abcd    ", "    ");
            Helper.ValidateCollection<string>(dic, "dic");
        }

        /// <summary>
        /// Tests the ValidateCollection method when dictionary value is null.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestValidateCollectionFail5()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add(" abcd    ", null);
            Helper.ValidateCollection<string>(dic, "dic");
        }

        //TODO - tests XmlDoc
    }
}
