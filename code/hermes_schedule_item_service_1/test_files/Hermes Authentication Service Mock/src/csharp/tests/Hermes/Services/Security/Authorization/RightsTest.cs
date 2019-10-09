/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using System;
using NUnit.Framework;

namespace Hermes.Services.Security.Authorization
{
    /// <summary>
    /// Test <see cref="Rights"/> class, unit test.
    /// </summary>
    ///
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class RightsTest
    {
        /// <summary>
        /// Test Rights.Read.
        /// </summary>
        [Test]
        public void TestRead()
        {
            Assert.AreEqual("Read", Enum.GetName(typeof(Rights), 1),
                "The value of Read is 1.");
        }

        /// <summary>
        /// Test Rights.Insert.
        /// </summary>
        [Test]
        public void TestInsert()
        {
            Assert.AreEqual("Insert", Enum.GetName(typeof(Rights), 2),
                "The value of Insert is 2.");
        }

        /// <summary>
        /// Test Rights.Update.
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            Assert.AreEqual("Update", Enum.GetName(typeof(Rights), 4),
                "The value of Update is 4.");
        }

        /// <summary>
        /// Test Rights.Delete.
        /// </summary>
        [Test]
        public void TestDelete()
        {
            Assert.AreEqual("Delete", Enum.GetName(typeof(Rights), 8),
                "The value of Delete is 8.");
        }

        /// <summary>
        /// Test Rights.Execute.
        /// </summary>
        [Test]
        public void TestExecute()
        {
            Assert.AreEqual("Execute", Enum.GetName(typeof(Rights), 16),
                "The value of Execute is 16.");
        }
    }
}
