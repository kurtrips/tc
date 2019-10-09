/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;
using TopCoder.XML.CmdLineProcessor;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="ReflectionEngine"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class ReflectionEngineFailureTest
    {
        /// <summary>
        /// The ReflectionEngine instance used for testing.
        /// </summary>
        private ReflectionEngine engine;

        /// <summary>
        /// The ReflectionEngineParameters instance used for testing.
        /// </summary>
        private ReflectionEngineParameters param;

        /// <summary>
        /// Set up testing environment.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            engine = new ReflectionEngine();

            param = new ReflectionEngineParameters();
            param.AssemblyFileNames = new string[] { "TopCoder.LoggingWrapper.dll" };
            param.ReferencePaths = new string[] { "../../test_files/failuretests" };
        }

        /// <summary>
        /// Test Constructor with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor_Null()
        {
            new ReflectionEngine(null);
        }

        /// <summary>
        /// Test WriteAPISpec method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestWriteAPISpec_Null1()
        {
            engine.WriteAPISpec(null, "<apispec></apispec>");
        }

        /// <summary>
        /// Test WriteAPISpec method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestWriteAPISpec_Null2()
        {
            engine.WriteAPISpec(param, null);
        }

        /// <summary>
        /// Test WriteAPISpec method with empty string argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpec_Empty()
        {
            engine.WriteAPISpec(param, "   ");
        }

        /// <summary>
        /// Test WriteAPISpec method with invalid apiSpec string argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpec_InvalidRoot()
        {
            engine.WriteAPISpec(param, "<abc></abc>");
        }

        /// <summary>
        /// Test WriteAPISpec method with invalid apiSpec string argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpec_InvalidContent()
        {
            engine.WriteAPISpec(param, "invalid");
        }

        /// <summary>
        /// Test WriteAPISpec method with invalid param argument which does not contain any assembly files.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpec_InvalidParam()
        {
            engine.WriteAPISpec(new ReflectionEngineParameters(), "<apispec/>");
        }

        /// <summary>
        /// Test WriteAPISpec method with invalid param argument which does not contain any assembly files.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpec_InvalidParam2()
        {
            param = new ReflectionEngineParameters();
            param.AssemblyFileNames = new string[0];

            engine.WriteAPISpec(param, "<apispec/>");
        }

        /// <summary>
        /// Test WriteAPISpec method with param argument which contains assembly file name which does not exist.
        /// It should throw XmlProcessorException.
        /// </summary>
        [Test, ExpectedException(typeof(XmlProcessorException))]
        public void TestWriteAPISpec_NotExistAssemblyFile()
        {
            param = new ReflectionEngineParameters();
            param.AssemblyFileNames = new string[]{"non-exist.dll"};

            engine.WriteAPISpec(param, "<apispec/>");
        }
    }
}
