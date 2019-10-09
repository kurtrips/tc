/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="SlashDocCache"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class SlashDocCacheFailureTest
    {
        /// <summary>
        /// The SlashDocCache instance used for testing.
        /// </summary>
        private SlashDocCache cache;

        /// <summary>
        /// Set up testing environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            cache = new SlashDocCache();
        }

        /// <summary>
        /// Test Constructor with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor_Null()
        {
            new SlashDocCache(null);
        }

        /// <summary>
        /// Test AddSlashDocFile method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddSlashDocFile_Null1()
        {
            cache.AddSlashDocFile(null);
        }

        /// <summary>
        /// Test AddSlashDocFile method with empty string argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddSlashDocFile_Empty()
        {
            cache.AddSlashDocFile("   ");
        }

        /// <summary>
        /// Test AddSlashDocFiles method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddSlashDocFiles_Null1()
        {
            cache.AddSlashDocFiles(null);
        }

        /// <summary>
        /// Test AddSlashDocFiles method with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddSlashDocFiles_NullElement()
        {
            cache.AddSlashDocFiles(new string[]{null});
        }

        /// <summary>
        /// Test AddSlashDocFiles method with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddSlashDocFiles_EmptyElement()
        {
            cache.AddSlashDocFiles(new string[] { "   " });
        }

        /// <summary>
        /// Test indexer method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestIndexerWithNullId()
        {
            string value = cache[null];
        }

        /// <summary>
        /// Test indexer method with empty string argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestIndexerWithEmptyId1()
        {
            string doc = cache["     "];
        }
    }
}
