/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using System.Reflection;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;
using TopCoder.XML.CmdLineProcessor;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="AssemblyLoader"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class AssemblyLoaderFailureTest
    {

        /// <summary>
        /// The AssemblyLoaderTester instance used for testing.
        /// </summary>
        private AssemblyLoaderTester loader;

        /// <summary>
        /// Set up testing environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            loader = new AssemblyLoaderTester(new string[] { "../../test_files/failuretests" });
        }
        
        /// <summary>
        /// Test constructor with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor_NullElement1()
        {
            new AssemblyLoader(new string[] { null });
        }

        /// <summary>
        /// Test constructor with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor_EmptyElement1()
        {
            new AssemblyLoader(new string[] { "   " });
        }
        
        /// <summary>
        /// Test constructor with argument containing null element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor_NullElement2()
        {
            new AssemblyLoader(new string[] { null }, new MBRLogger(new EmptyLogger("name")));
        }

        /// <summary>
        /// Test constructor with argument containing empty string element.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor_EmptyElement2()
        {
            new AssemblyLoader(new string[] { "   " }, new MBRLogger(new EmptyLogger("name")));
        }

        /// <summary>
        /// Test constructor with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor_Null1()
        {
            new AssemblyLoader(new string[] { "path" }, null);
        }

        /// <summary>
        /// Test LoadAssembly method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLoadAssembly_Null()
        {
            loader.LoadAssembly(null);
        }

        /// <summary>
        /// Test LoadAssembly method with empty string argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoadAssembly_Empty()
        {
            loader.LoadAssembly("   ");
        }

        /// <summary>
        /// Test LoadAssembly method with non-exist file.
        /// It should throw XmlProcessorException.
        /// </summary>
        [Test, ExpectedException(typeof(XmlProcessorException))]
        public void TestLoadAssembly_Invalid()
        {
            loader.LoadAssembly("non-exist.dll");
        }

        /// <summary>
        /// Test ResolveAssembly method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestResolveAssembly_Null1()
        {
            loader.ResolveAssembly(null, new ResolveEventArgs("arg"));
        }

        /// <summary>
        /// Test ResolveAssembly method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestResolveAssembly_Null2()
        {
            loader.ResolveAssembly(new object(), null);
        }
        
        /// <summary>
        /// This class extends from AssemblyLoader which is aim to expose its protected method to test.
        /// </summary>
        /// <author>Xuchen</author>
        /// <version>1.0</version>
        /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
        [CoverageExclude]
        private class AssemblyLoaderTester : AssemblyLoader
        {
            /// <summary>
            /// <para>Creates a new instance of AssemblyLoaderTester.</para>
            /// </summary>
            ///
            /// <param name="referencePaths">The set of custom paths where to search the desired assembly.</param>
            ///
            /// <exception cref="ArgumentException">If referencePaths contain null or empty element.</exception>
            public AssemblyLoaderTester(string[] referencePaths) : base(referencePaths)
            {
            }

            /// <summary>
            /// Delegate to its base class.
            /// </summary>
            ///
            /// <param name="sender">The source of the event.</param>
            /// <param name="args">A ResolveEventArgs that contains the event data.</param>
            ///
            /// <returns>The resolved assembly.</returns>
            ///
            /// <exception cref="ArgumentNullException">If either parameter is null</exception>
            public new Assembly ResolveAssembly(object sender, ResolveEventArgs args)
            {
                return base.ResolveAssembly(sender, args);
            }
        }
    }
}
