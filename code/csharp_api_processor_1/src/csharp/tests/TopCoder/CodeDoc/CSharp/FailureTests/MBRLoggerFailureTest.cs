/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="MBRLogger"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class MBRLoggerFailureTest
    {
        /// <summary>
        /// The MBRLogger instance used for testing.
        /// </summary>
        private MBRLogger logger;

        /// <summary>
        /// Set up the testing environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            logger = new MBRLogger(new EmptyLogger("logname"));
        }

        /// <summary>
        /// Test Constructor with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor_Null()
        {
            new MBRLogger(null);
        }
    }
}
