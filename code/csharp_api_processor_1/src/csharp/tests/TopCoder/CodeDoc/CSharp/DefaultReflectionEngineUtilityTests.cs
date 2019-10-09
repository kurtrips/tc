// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Reflection;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the ReflectionEngineUtility class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class ReflectionEngineUtilityTests
    {
        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        /// Tests the GetUniqueID method for method type.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID1()
        {
            MemberInfo mi = typeof(TestClass).GetMember("AMethod")[0];
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "M:TopCoder.CodeDoc.CSharp.Reflection.TestClass.AMethod(System.Int32[])",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for constructor type.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID2()
        {
            MemberInfo mi = typeof(TestClass).GetConstructor(new Type[] { typeof(int) });
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "M:TopCoder.CodeDoc.CSharp.Reflection.TestClass.#ctor(System.Int32)",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for event type.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID3()
        {
            MemberInfo mi = typeof(TestClass).GetMember("AnEvent")[0];
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "E:TopCoder.CodeDoc.CSharp.Reflection.TestClass.AnEvent",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a field type.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID4()
        {
            MemberInfo mi = typeof(TestClass).GetMember("aField")[0];
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "F:TopCoder.CodeDoc.CSharp.Reflection.TestClass.aField",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a property type.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID5()
        {
            MemberInfo mi = typeof(TestClass).GetMember("AProperty")[0];
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "P:TopCoder.CodeDoc.CSharp.Reflection.TestClass.AProperty",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a delegate type.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID6()
        {
            MemberInfo mi = typeof(TestClass).GetMember("ADelegate")[0];
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "T:TopCoder.CodeDoc.CSharp.Reflection.TestClass.ADelegate",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a nested class.
        /// GetUniqueID(MemberInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID7()
        {
            MemberInfo mi = typeof(TestClass).GetMember("NestedClass")[0];
            string id = ReflectionEngineUtility.GetUniqueID(mi);
            Assert.AreEqual(id,
                "T:TopCoder.CodeDoc.CSharp.Reflection.TestClass.NestedClass",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method when memberInfo is null.
        /// GetUniqueID(MemberInfo)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueIDFail1()
        {
            ReflectionEngineUtility.GetUniqueID((MemberInfo)null);
        }

        /// <summary>
        /// Tests the GetUniqueID method for a Type.
        /// GetUniqueID(Type)
        /// </summary>
        [Test]
        public void TestGetUniqueID8()
        {
            string id = ReflectionEngineUtility.GetUniqueID(typeof(TestClass));
            Assert.AreEqual(id,
                "T:TopCoder.CodeDoc.CSharp.Reflection.TestClass",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a Type.
        /// GetUniqueID(Type)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID9()
        {
            ReflectionEngineUtility.GetUniqueID((Type)null);
        }

        /// <summary>
        /// Tests the GetUniqueID method for a FieldInfo.
        /// GetUniqueID(FieldInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID10()
        {
            string id = ReflectionEngineUtility.GetUniqueID(typeof(TestClass).GetField("aField"));
            Assert.AreEqual(id,
                "F:TopCoder.CodeDoc.CSharp.Reflection.TestClass.aField",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a FieldInfo.
        /// GetUniqueID(FieldInfo)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID11()
        {
            ReflectionEngineUtility.GetUniqueID((FieldInfo)null);
        }

        /// <summary>
        /// Tests the GetUniqueID method for a EventInfo.
        /// GetUniqueID(EventInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID12()
        {
            string id = ReflectionEngineUtility.GetUniqueID(typeof(TestClass).GetEvent("AnEvent"));
            Assert.AreEqual(id,
                "E:TopCoder.CodeDoc.CSharp.Reflection.TestClass.AnEvent",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a EventInfo.
        /// GetUniqueID(EventInfo)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID13()
        {
            ReflectionEngineUtility.GetUniqueID((EventInfo)null);
        }

        /// <summary>
        /// Tests the GetUniqueID method for a PropertyInfo.
        /// GetUniqueID(PropertyInfo)
        /// </summary>
        [Test]
        public void TestGetUniqueID14()
        {
            string id = ReflectionEngineUtility.GetUniqueID(typeof(TestClass).GetProperty("AProperty"));
            Assert.AreEqual(id,
                "P:TopCoder.CodeDoc.CSharp.Reflection.TestClass.AProperty",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a PropertyInfo when it is null.
        /// GetUniqueID(PropertyInfo)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID15()
        {
            ReflectionEngineUtility.GetUniqueID((PropertyInfo)null);
        }

        /// <summary>
        /// Tests the GetUniqueID method for a MethodBase.
        /// GetUniqueID(MethodBase)
        /// </summary>
        [Test]
        public void TestGetUniqueID16()
        {
            string id = ReflectionEngineUtility.GetUniqueID(typeof(TestClass).GetMethod("AMethod"));
            Assert.AreEqual(id,
                "M:TopCoder.CodeDoc.CSharp.Reflection.TestClass.AMethod(System.Int32[])",
                "Wrong value for unique id.");
        }

        /// <summary>
        /// Tests the GetUniqueID method for a MethodBase when it is null.
        /// GetUniqueID(MethodBase)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID17()
        {
            ReflectionEngineUtility.GetUniqueID((MethodBase)null);
        }

        /// <summary>
        /// Tests the GetFieldVisibility method.
        /// GetFieldVisibility(FieldInfo)
        /// </summary>
        [Test]
        public void TestGetFieldVisibility()
        {
            FieldInfo fld = typeof(TestClass).GetField("aField");
            string vis = ReflectionEngineUtility.GetFieldVisibility(fld);
            Assert.AreEqual(vis, "public", "Wrong value for visibility.");

            fld = typeof(TestClass).GetField("aField1", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetFieldVisibility(fld);
            Assert.AreEqual(vis, "protected", "Wrong value for visibility.");

            fld = typeof(TestClass).GetField("aField2", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetFieldVisibility(fld);
            Assert.AreEqual(vis, "private", "Wrong value for visibility.");

            fld = typeof(TestClass).GetField("aField3", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetFieldVisibility(fld);
            Assert.AreEqual(vis, "internal", "Wrong value for visibility.");

            fld = typeof(TestClass).GetField("aField4", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetFieldVisibility(fld);
            Assert.AreEqual(vis, "protected internal", "Wrong value for visibility.");
        }

        /// <summary>
        /// Tests the GetFieldVisibility method when FieldInfo is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetFieldVisibilityFail1()
        {
            ReflectionEngineUtility.GetFieldVisibility(null);
        }

        /// <summary>
        /// Tests the GetFieldModifiers method.
        /// GetFieldModifiers(FieldInfo)
        /// </summary>
        [Test]
        public void TestGetFieldModifiers()
        {
            FieldInfo fld = typeof(TestClass).GetField("aField5");
            string mod = ReflectionEngineUtility.GetFieldModifiers(fld);
            Assert.AreEqual(mod, "static readonly", "Wrong value for modifier.");

            fld = typeof(TestClass).GetField("aField6");
            mod = ReflectionEngineUtility.GetFieldModifiers(fld);
            Assert.AreEqual(mod, "static const", "Wrong value for modifier.");
        }

        /// <summary>
        /// Tests the GetFieldModifiers method when FieldInfo is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetFieldModifiersFail1()
        {
            ReflectionEngineUtility.GetFieldModifiers(null);
        }

        /// <summary>
        /// Tests the GetPropertyVisibility method.
        /// GetPropertyVisibility(PropertyInfo)
        /// </summary>
        [Test]
        public void TestGetPropertyVisibility()
        {
            PropertyInfo fld = typeof(TestClass).GetProperty("AProperty");
            string vis = ReflectionEngineUtility.GetPropertyVisibility(fld);
            Assert.AreEqual(vis, "public", "Wrong value for visibility.");

            fld = typeof(TestClass).GetProperty("AProperty1", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetPropertyVisibility(fld);
            Assert.AreEqual(vis, "protected", "Wrong value for visibility.");

            fld = typeof(TestClass).GetProperty("AProperty2", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetPropertyVisibility(fld);
            Assert.AreEqual(vis, "private", "Wrong value for visibility.");

            fld = typeof(TestClass).GetProperty("AProperty3", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetPropertyVisibility(fld);
            Assert.AreEqual(vis, "internal", "Wrong value for visibility.");

            fld = typeof(TestClass).GetProperty("AProperty4", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetPropertyVisibility(fld);
            Assert.AreEqual(vis, "protected internal", "Wrong value for visibility.");
        }

        /// <summary>
        /// Tests the GetPropertyVisibility method when PropertyInfo is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetPropertyVisibilityFail1()
        {
            ReflectionEngineUtility.GetPropertyVisibility(null);
        }

        /// <summary>
        /// Tests the GetPropertyModifiers method.
        /// GetPropertyModifiers(PropertyInfo)
        /// </summary>
        [Test]
        public void TestGetPropertyModifiers()
        {
            PropertyInfo fld = typeof(TestClass).GetProperty("AProperty");
            string vis = ReflectionEngineUtility.GetPropertyModifiers(fld);
            Assert.AreEqual(vis, "virtual read-write", "Wrong value for visibility.");
        }

        /// <summary>
        /// Tests the GetPropertyModifiers method when PropertyInfo is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetPropertyModifiersFail1()
        {
            ReflectionEngineUtility.GetPropertyModifiers(null);
        }

        /// <summary>
        /// Tests the GetMethodVisibility method.
        /// GetMethodVisibility(MethodBase)
        /// </summary>
        [Test]
        public void TestGetMethodVisibility()
        {
            MethodInfo fld = typeof(TestClass).GetMethod("AMethod");
            string vis = ReflectionEngineUtility.GetMethodVisibility(fld);
            Assert.AreEqual(vis, "public", "Wrong value for visibility.");

            fld = typeof(TestClass).GetMethod("AMethod1", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetMethodVisibility(fld);
            Assert.AreEqual(vis, "protected", "Wrong value for visibility.");

            fld = typeof(TestClass).GetMethod("AMethod2",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static );
            vis = ReflectionEngineUtility.GetMethodVisibility(fld);
            Assert.AreEqual(vis, "private", "Wrong value for visibility.");

            fld = typeof(TestClass).GetMethod("AMethod3", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetMethodVisibility(fld);
            Assert.AreEqual(vis, "internal", "Wrong value for visibility.");

            fld = typeof(TestClass).GetMethod("AMethod4", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetMethodVisibility(fld);
            Assert.AreEqual(vis, "protected internal", "Wrong value for visibility.");
        }

        /// <summary>
        /// Tests the GetMethodVisibility method when MethodBase is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetMethodVisibilityFail1()
        {
            ReflectionEngineUtility.GetMethodVisibility(null);
        }

        /// <summary>
        /// Tests the GetMethodModifiers method.
        /// GetMethodModifiers(MethodBase)
        /// </summary>
        [Test]
        public void TestGetMethodModifiers()
        {
            MethodInfo fld = typeof(TestClass).GetMethod("AMethod");
            string vis = ReflectionEngineUtility.GetMethodModifiers(fld);
            Assert.AreEqual(vis, "override", "Wrong value for modifiers.");

            fld = typeof(TestClass).GetMethod("AMethod1", BindingFlags.Instance | BindingFlags.NonPublic);
            vis = ReflectionEngineUtility.GetMethodModifiers(fld);
            Assert.AreEqual(vis, "virtual", "Wrong value for modifiers.");

            fld = typeof(TestClass).GetMethod("AMethod2",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
            vis = ReflectionEngineUtility.GetMethodModifiers(fld);
            Assert.AreEqual(vis, "static", "Wrong value for modifiers.");

            fld = typeof(TestBaseClass).GetMethod("AMethod");
            vis = ReflectionEngineUtility.GetMethodModifiers(fld);
            Assert.AreEqual(vis, "abstract", "Wrong value for modifiers.");
        }

        /// <summary>
        /// Tests the GetMethodModifiers method when MethodBase is null.
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetMethodModifiersFail1()
        {
            ReflectionEngineUtility.GetMethodModifiers(null);
        }

        /// <summary>
        /// Tests the GetTypeVisibility method.
        /// GetTypeVisibility(Type)
        /// </summary>
        [Test]
        public void TestGetTypeVisibility()
        {
            string vis = ReflectionEngineUtility.GetTypeVisibility(typeof(TestClass));
            Assert.AreEqual(vis, "internal", "Wrong value for visibility.");
        }

        /// <summary>
        /// Tests the GetTypeVisibility method when type is null.
        /// GetTypeVisibility(Type)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetTypeVisibilityFail1()
        {
            ReflectionEngineUtility.GetTypeVisibility(null);
        }

        /// <summary>
        /// Tests the GetTypeModifiers method.
        /// GetTypeModifiers(Type)
        /// </summary>
        [Test]
        public void TestGetTypeModifiers()
        {
            string vis = ReflectionEngineUtility.GetTypeModifiers(typeof(TestBaseClass));
            Assert.AreEqual(vis, "abstract", "Wrong value for modifiers.");

            vis = ReflectionEngineUtility.GetTypeModifiers(typeof(TestClassSealed));
            Assert.AreEqual(vis, "sealed", "Wrong value for modifiers.");
        }

        /// <summary>
        /// Tests the GetTypeModifiers method when type is null.
        /// GetTypeModifiers(Type)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetTypeModifiersFail1()
        {
            ReflectionEngineUtility.GetTypeModifiers(null);
        }
    }

    /// <summary>
    /// Represents a class for testing the methods of ReflectionEngineUtility class.
    /// This class is base class of TestClass.
    /// </summary>
    internal abstract class TestBaseClass
    {
        /// <summary>
        /// An abtract method
        /// </summary>
        /// <param name="arr">An array of ints</param>
        /// <returns>A string</returns>
        public abstract string AMethod(int[] arr);
    }

    /// <summary>
    /// Represents a class for testing the methods of ReflectionEngineUtility class.
    /// </summary>
    [CoverageExclude]
    internal class TestClass : TestBaseClass
    {
        /// <summary>
        /// A test constructor
        /// </summary>
        /// <param name="x">An int</param>
        public TestClass(int x)
        {
            AnEvent += new ADelegate(AnEventHandler);
            aField = true;
            aField1 = true;
            aField2 = true;
            aField3 = true;
            aField4 = true;
        }

        /// <summary>
        /// A test method
        /// </summary>
        /// <param name="arr">Array of ints</param>
        /// <returns>A string</returns>
        public override string AMethod(int[] arr)
        {
            return aField2.ToString();
        }

        /// <summary>
        /// A protected test method
        /// </summary>
        /// <param name="arr">Array of ints</param>
        /// <returns>A string</returns>
        protected virtual string AMethod1(int[] arr)
        {
            return "";
        }

        /// <summary>
        /// A private test method
        /// </summary>
        /// <param name="arr">Array of ints</param>
        /// <returns>A string</returns>
        private static string AMethod2(int[] arr)
        {
            return "";
        }

        /// <summary>
        /// A internal test method
        /// </summary>
        /// <param name="arr">Array of ints</param>
        /// <returns>A string</returns>
        internal string AMethod3(int[] arr)
        {
            return "";
        }

        /// <summary>
        /// A internal test method
        /// </summary>
        /// <param name="arr">Array of ints</param>
        /// <returns>A string</returns>
        protected internal string AMethod4(int[] arr)
        {
            return "";
        }

        /// <summary>
        /// An event handler
        /// </summary>
        /// <param name="s">A string param</param>
        public void AnEventHandler(string s)
        {
        }

        /// <summary>
        /// A test event
        /// </summary>
        public event ADelegate AnEvent;

        /// <summary>
        /// A test event
        /// </summary>
        public bool aField;

        /// <summary>
        /// A protected field
        /// </summary>
        protected bool aField1;

        /// <summary>
        /// A private field
        /// </summary>
        private bool aField2;

        /// <summary>
        /// A internal field
        /// </summary>
        internal bool aField3;

        /// <summary>
        /// A protected internal field
        /// </summary>
        protected internal bool aField4;

        /// <summary>
        /// A public static readonly field
        /// </summary>
        public static readonly bool aField5 = true;

        /// <summary>
        /// A const field
        /// </summary>
        public const string aField6 = "SUPERNAUT";

        /// <summary>
        /// A property
        /// </summary>
        public virtual short AProperty
        {
            get
            {
                return 0;
            }
            set { }
        }

        /// <summary>
        /// A private property
        /// </summary>
        private short AProperty2
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// A protected property
        /// </summary>
        protected short AProperty1
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// A internal property
        /// </summary>
        internal short AProperty3
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// A internal property
        /// </summary>
        protected internal short AProperty4
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// A delegate
        /// </summary>
        public delegate void ADelegate(string s);

        /// <summary>
        /// A nested class.
        /// </summary>
        public class NestedClass
        {
        }
    }

    /// <summary>
    /// A test class which is sealed.
    /// </summary>
    internal sealed class TestClassSealed
    {
    }
}
