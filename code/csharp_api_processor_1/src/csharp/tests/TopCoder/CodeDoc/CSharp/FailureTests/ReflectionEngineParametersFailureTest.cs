/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="ReflectionEngineParameters"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class ReflectionEngineParametersFailureTest
    {
        /// <summary>
        /// The ReflectionEngineParameters instance used for testing.
        /// </summary>
        private ReflectionEngineParameters param;

        /// <summary>
        /// Set up testing environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            param = new ReflectionEngineParameters();
        }

        /// <summary>
        /// Test setter of AssemblyFileNames with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAssemblyFileNames_NullElement()
        {
            param.AssemblyFileNames = new string[] { null };
        }

        /// <summary>
        /// Test setter of AssemblyFileNames with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAssemblyFileNames_EmptyElement()
        {
            param.AssemblyFileNames = new string[] { "    " };
        }

        /// <summary>
        /// Test setter of SlashDocFileNames with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSlashDocFileNames_NullElement()
        {
            param.SlashDocFileNames = new string[] { null };
        }

        /// <summary>
        /// Test setter of SlashDocFileNames with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSlashDocFileNames_EmptyElement()
        {
            param.SlashDocFileNames = new string[] { "    " };
        }

        /// <summary>
        /// Test setter of ReferencePaths with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestReferencePaths_NullElement()
        {
            param.ReferencePaths = new string[] { null };
        }

        /// <summary>
        /// Test setter of ReferencePaths with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestReferencePaths_EmptyElement()
        {
            param.ReferencePaths = new string[] { "    " };
        }

        /// <summary>
        /// Test setter of ModuleNames with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestModuleNames_NullElement()
        {
            param.ModuleNames = new string[] { null };
        }

        /// <summary>
        /// Test setter of ModuleNames with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestModuleNames_EmptyElement()
        {
            param.ModuleNames = new string[] { "    " };
        }

        /// <summary>
        /// Test setter of TypePrefixes with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestTypePrefixes_NullElement()
        {
            param.TypePrefixes = new string[] { null };
        }

        /// <summary>
        /// Test setter of TypePrefixes with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestTypePrefixes_EmptyElement()
        {
            param.TypePrefixes = new string[] { "    " };
        }

        /// <summary>
        /// Test setter of LoggerNamespace with empty string.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoggerNamespace_EmptyElement()
        {
            param.LoggerNamespace = "       ";
        }
    }
}
