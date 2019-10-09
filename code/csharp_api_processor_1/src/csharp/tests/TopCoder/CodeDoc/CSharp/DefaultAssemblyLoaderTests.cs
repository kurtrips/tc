// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using TopCoder.LoggingWrapper;
using TopCoder.XML.CmdLineProcessor;
using TopCoder.Util.ConfigurationManager;
using System.Reflection;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the AssemblyLoader class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, TopCoder.CodeDoc.CSharp.CoverageExclude]
    public class AssemblyLoaderTests
    {
        /// <summary>
        /// AssemblyLoader instance to use for the tests.
        /// </summary>
        AssemblyLoader al;

        /// <summary>
        /// MBRLogger instance to use for the tests.
        /// </summary>
        MBRLogger logger;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");

            logger = new MBRLogger(LogManager.CreateLogger("MyLoggerNamespace"));
            al = new AssemblyLoader(new string[] { UnitTestHelper.REFPATH }, logger);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            al = null;
            logger = null;
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Tests the constructor.
        /// AssemblyLoader(string[])
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            al = new AssemblyLoader(new string[] { UnitTestHelper.REFPATH });
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(al, "referencePaths"), "referencePaths was not set");
        }

        /// <summary>
        /// Tests the constructor when referencePaths contains null element.
        /// AssemblyLoader(string[])
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail1()
        {
            al = new AssemblyLoader(new string[] { null });
        }

        /// <summary>
        /// Tests the constructor when referencePaths contains empty element.
        /// AssemblyLoader(string[])
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructorFail2()
        {
            al = new AssemblyLoader(new string[] { "       " });
        }

        /// <summary>
        /// Tests the constructor overload.
        /// AssemblyLoader(string[], MBRLogger)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(al, "logger"), "logger was not set");
            Assert.IsNotNull(UnitTestHelper.GetPrivateField(al, "referencePaths"), "referencePaths was not set");
        }

        /// <summary>
        /// Tests the constructor overload when referencePaths contains null element.
        /// AssemblyLoader(string[], MBRLogger)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2Fail1()
        {
            al = new AssemblyLoader(new string[] { null }, logger);
        }

        /// <summary>
        /// Tests the constructor overload when referencePaths contains empty element.
        /// AssemblyLoader(string[], MBRLogger)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2Fail2()
        {
            al = new AssemblyLoader(new string[] { "  " }, logger);
        }

        /// <summary>
        /// Tests the constructor overload when logger is null.
        /// AssemblyLoader(string[], MBRLogger)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor2Fail3()
        {
            al = new AssemblyLoader(new string[] { "vxdfvs" }, null);
        }


        /// <summary>
        /// Tests the LoadAssembly.
        /// LoadAssembly(string)
        /// </summary>
        [Test]
        public void TestLoadAssembly1()
        {
            Assembly assem = al.LoadAssembly(UnitTestHelper.MOCKLIBPATH);
            Assert.IsNotNull(assem, "Failed to load assembly");
            Assert.AreEqual(assem.FullName, "MockLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "Wrong assembly loaded");
        }

        /// <summary>
        /// Tests the LoadAssembly when the assembly is already loaded in the current domain.
        /// LoadAssembly(string)
        /// </summary>
        [Test]
        public void TestLoadAssembly2()
        {
            //Load it up first
            Assembly assem = al.LoadAssembly(UnitTestHelper.MOCKLIBPATH);

            //Load it again
            Assembly assem2 = al.LoadAssembly(UnitTestHelper.MOCKLIBPATH);
            Assert.IsTrue(object.ReferenceEquals(assem, assem2), "A new assembly should not be loaded.");
        }

        /// <summary>
        /// Tests the LoadAssembly method when the library is not present in the path.
        /// LoadAssembly(string)
        /// XmlProcessorException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(XmlProcessorException))]
        public void TestLoadAssemblyFail1()
        {
            al.LoadAssembly("../../test_files/BaseLibrary.dll");
        }

        /// <summary>
        /// Tests the LoadAssembly method when the fileName is null
        /// LoadAssembly(string)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestLoadAssemblyFail2()
        {
            al.LoadAssembly(null);
        }

        /// <summary>
        /// Tests the LoadAssembly method when the fileName is empty
        /// LoadAssembly(string)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestLoadAssemblyFail3()
        {
            al.LoadAssembly("      ");
        }

        /// <summary>
        /// Tests the ResolveAssembly method.
        /// ResolveAssembly(object sender, ResolveEventArgs args)
        /// </summary>
        [Test]
        public void TestResolveAssembly1()
        {
            Assembly baseLibrary = (Assembly) typeof(AssemblyLoader).InvokeMember("ResolveAssembly",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, al, new object[] {
                    al, new ResolveEventArgs("BaseLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") }
                );

            Assert.IsNotNull(baseLibrary, "Failed to load assembly from reference path.");
            Assert.AreEqual(baseLibrary.FullName, "BaseLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                "Wrong assembly loaded");
        }

        /// <summary>
        /// Tests the ResolveAssembly method when there are no reference paths. Must simple return null.
        /// ResolveAssembly(object sender, ResolveEventArgs args)
        /// </summary>
        [Test]
        public void TestResolveAssembly2()
        {
            al = new AssemblyLoader(null);

            Assembly baseLibrary = (Assembly)typeof(AssemblyLoader).InvokeMember("LoadAssemblyFromReferencePaths",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, al,
                new object[] { "BaseLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "BaseLibrary.dll" }
                );

            Assert.IsNull(baseLibrary, "If reference path is null this method must return null.");
        }

        /// <summary>
        /// Tests the ResolveAssembly method when library is not able to be found. The method must return null.
        /// ResolveAssembly(object sender, ResolveEventArgs args)
        /// </summary>
        [Test]
        public void TestResolveAssembly3()
        {
            Assembly lib = (Assembly) typeof(AssemblyLoader).InvokeMember("ResolveAssembly",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, al, new object[] {
                    al, new ResolveEventArgs("NoSuchLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") }
                );

            Assert.IsNull(lib, "Must be null.");
        }

        /// <summary>
        /// Tests the ResolveAssembly method when library is found but is invalid.
        /// The method must return null and log error.
        /// ResolveAssembly(object sender, ResolveEventArgs args)
        /// </summary>
        [Test]
        public void TestResolveAssembly4()
        {
            Assembly lib = (Assembly)typeof(AssemblyLoader).InvokeMember("ResolveAssembly",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, al, new object[] {
                    al, new ResolveEventArgs("InvalidAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") }
                );

            Assert.IsNull(lib, "Must be null.");
        }

        /// <summary>
        /// Tests the ResolveAssembly method when sender is null.
        /// ResolveAssembly(object sender, ResolveEventArgs args)
        /// </summary>
        [Test]
        public void TestResolveAssemblyFail1()
        {
            try
            {
                Assembly baseLibrary = (Assembly)typeof(AssemblyLoader).InvokeMember("ResolveAssembly",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, al,
                    new object[] { null,
                        new ResolveEventArgs("BaseLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null") }
                    );
            }
            catch (TargetInvocationException t)
            {
                Assert.IsTrue(t.InnerException is ArgumentNullException,
                    "The inner exception must be ArgumentNullException");
            }
        }

        /// <summary>
        /// Tests the ResolveAssembly method when args is null.
        /// ResolveAssembly(object sender, ResolveEventArgs args)
        /// </summary>
        [Test]
        public void TestResolveAssemblyFail2()
        {
            try
            {
                Assembly baseLibrary = (Assembly)typeof(AssemblyLoader).InvokeMember("ResolveAssembly",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, al,
                    new object[] { al, null }
                    );
            }
            catch (TargetInvocationException t)
            {
                Assert.IsTrue(t.InnerException is ArgumentNullException,
                    "The inner exception must be ArgumentNullException");
            }
        }
    }
}
