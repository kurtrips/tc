// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the ReflectionEngineParameters class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, TopCoder.CodeDoc.CSharp.CoverageExclude]
    public class ReflectionEngineParametersTests
    {
        /// <summary>
        /// ReflectionEngineParameters instance to use for the tests.
        /// </summary>
        ReflectionEngineParameters rep;

        /// <summary>
        /// An array of strings for setting properties.
        /// </summary>
        private string[] arr;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            arr = new string[] { "val1", "val2" };
            rep = new ReflectionEngineParameters();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            arr = null;
            rep = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// ReflectionEngineParameters()
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            Assert.IsTrue(rep.GetType().IsSerializable, "Class must be marked serializable.");
        }

        /// <summary>
        /// Tests the AssemblyFileNames property.
        /// </summary>
        [Test]
        public void TestAssemblyFileNames()
        {
            //Set
            rep.AssemblyFileNames = arr;
            //Get
            Assert.AreEqual(rep.AssemblyFileNames, arr, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the AssemblyFileNames property when set to null.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestAssemblyFileNames1()
        {
            //Set
            rep.AssemblyFileNames = null;
            //Get
            Assert.IsNull(rep.AssemblyFileNames, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the AssemblyFileNames property when set to empty array.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestAssemblyFileNames2()
        {
            //Set
            rep.AssemblyFileNames = new string[0];
            //Get
            Assert.AreEqual(rep.AssemblyFileNames.Length, 0, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the AssemblyFileNames property when set to array with empty element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAssemblyFileNamesFail1()
        {
            //Set
            arr[1] = "     ";
            rep.AssemblyFileNames = arr;
        }

        /// <summary>
        /// Tests the AssemblyFileNames property when set to array with null element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAssemblyFileNamesFail2()
        {
            //Set
            arr[1] = null;
            rep.AssemblyFileNames = arr;
        }

        /// <summary>
        /// Tests the SlashDocFileNames property.
        /// </summary>
        [Test]
        public void TestSlashDocFileNames()
        {
            //Set
            rep.SlashDocFileNames = arr;
            //Get
            Assert.AreEqual(rep.SlashDocFileNames, arr, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the SlashDocFileNames property when set to null.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestSlashDocFileNames1()
        {
            //Set
            rep.SlashDocFileNames = null;
            //Get
            Assert.IsNull(rep.SlashDocFileNames, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the SlashDocFileNames property when set to empty array.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestSlashDocFileNames2()
        {
            //Set
            rep.SlashDocFileNames = new string[0];
            //Get
            Assert.AreEqual(rep.SlashDocFileNames.Length, 0, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the SlashDocFileNames property when set to array with empty element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSlashDocFileNamesFail1()
        {
            //Set
            arr[1] = "     ";
            rep.SlashDocFileNames = arr;
        }

        /// <summary>
        /// Tests the SlashDocFileNames property when set to array with null element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSlashDocFileNamesFail2()
        {
            //Set
            arr[1] = null;
            rep.SlashDocFileNames = arr;
        }

        /// <summary>
        /// Tests the ReferencePaths property.
        /// </summary>
        [Test]
        public void TestReferencePaths()
        {
            //Set
            rep.ReferencePaths = arr;
            //Get
            Assert.AreEqual(rep.ReferencePaths, arr, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the ReferencePaths property when set to null.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestReferencePaths1()
        {
            //Set
            rep.ReferencePaths = null;
            //Get
            Assert.IsNull(rep.ReferencePaths, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the ReferencePaths property when set to empty array.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestReferencePaths2()
        {
            //Set
            rep.ReferencePaths = new string[0];
            //Get
            Assert.AreEqual(rep.ReferencePaths.Length, 0, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the ReferencePaths property when set to array with empty element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestReferencePathsFail1()
        {
            //Set
            arr[1] = "     ";
            rep.ReferencePaths = arr;
        }

        /// <summary>
        /// Tests the ReferencePaths property when set to array with null element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestReferencePathsFail2()
        {
            //Set
            arr[1] = null;
            rep.ReferencePaths = arr;
        }

        /// <summary>
        /// Tests the ModuleNames property.
        /// </summary>
        [Test]
        public void TestModuleNames()
        {
            //Set
            rep.ModuleNames = arr;
            //Get
            Assert.AreEqual(rep.ModuleNames, arr, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the ModuleNames property when set to null.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestModuleNames1()
        {
            //Set
            rep.ModuleNames = null;
            //Get
            Assert.IsNull(rep.ModuleNames, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the ModuleNames property when set to empty array.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestModuleNames2()
        {
            //Set
            rep.ModuleNames = new string[0];
            //Get
            Assert.AreEqual(rep.ModuleNames.Length, 0, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the ModuleNames property when set to array with empty element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestModuleNamesFail1()
        {
            //Set
            arr[1] = "     ";
            rep.ModuleNames = arr;
        }

        /// <summary>
        /// Tests the ModuleNames property when set to array with null element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestModuleNamesFail2()
        {
            //Set
            arr[1] = null;
            rep.ModuleNames = arr;
        }

        /// <summary>
        /// Tests the TypePrefixes property.
        /// </summary>
        [Test]
        public void TestTypePrefixes()
        {
            //Set
            rep.TypePrefixes = arr;
            //Get
            Assert.AreEqual(rep.TypePrefixes, arr, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the TypePrefixes property when set to null.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestTypePrefixes1()
        {
            //Set
            rep.TypePrefixes = null;
            //Get
            Assert.IsNull(rep.TypePrefixes, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the TypePrefixes property when set to empty array.
        /// No exception must be thrown.
        /// </summary>
        [Test]
        public void TestTypePrefixes2()
        {
            //Set
            rep.TypePrefixes = new string[0];
            //Get
            Assert.AreEqual(rep.TypePrefixes.Length, 0, "Wrong property implementation.");
        }

        /// <summary>
        /// Tests the TypePrefixes property when set to array with empty element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestTypePrefixesFail1()
        {
            //Set
            arr[1] = "     ";
            rep.TypePrefixes = arr;
        }

        /// <summary>
        /// Tests the TypePrefixes property when set to array with null element.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestTypePrefixesFail2()
        {
            //Set
            arr[1] = null;
            rep.TypePrefixes = arr;
        }

        /// <summary>
        /// Tests the DocumentPrivates property.
        /// </summary>
        [Test]
        public void TestDocumentPrivates()
        {
            Assert.AreEqual(rep.DocumentPrivates, false, "Must be initially false.");
            rep.DocumentPrivates = true;
            Assert.AreEqual(rep.DocumentPrivates, true, "Must be set correctly.");
        }

        /// <summary>
        /// Tests the LoggerNamespace property. Can be set to null.
        /// </summary>
        [Test]
        public void TestLoggerNamespace()
        {
            rep.LoggerNamespace = "abc";
            Assert.AreEqual(rep.LoggerNamespace, "abc", "Must be set correctly.");

            rep.LoggerNamespace = null;
            Assert.IsNull(rep.LoggerNamespace, "Must be set correctly.");
        }

        /// <summary>
        /// Tests the LoggerNamespace property when input is empty.
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoggerNamespaceFail1()
        {
            rep.LoggerNamespace = "   ";
        }
    }
}
