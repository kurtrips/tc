/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using System.Xml;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="CSharpAPIProcessor"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class CSharpAPIProcessorFailureTest
    {
        /// <summary>
        /// The CSharpAPIProcessor instance used for testing.
        /// </summary>
        private CSharpAPIProcessor processor;

        /// <summary>
        /// The ReflectionEngineParameters instance used for testing.
        /// </summary>
        private ReflectionEngineParameters rep;

        /// <summary>
        /// Sets up test environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            rep = new ReflectionEngineParameters();

            processor = new CSharpAPIProcessor(rep);
        }

        /// <summary>
        /// Test the constructor with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor_Null()
        {
            new CSharpAPIProcessor(null);
        }

        /// <summary>
        /// Test ProcessDocument method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestProcessDocument_Null()
        {
            processor.ProcessDocument(null);
        }

        /// <summary>
        /// Test ProcessDocument method with invalid document.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestProcessDocument_Invalid1()
        {
            processor.ProcessDocument(new XmlDocument());
        }

        /// <summary>
        /// Test ProcessDocument method with invalid document.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestProcessDocument_Invalid2()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<abc></abc>");
            
            processor.ProcessDocument(doc);
        }
    }
}
