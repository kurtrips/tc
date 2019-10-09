/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */
using System;
using System.Reflection;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="ReflectionEngineUtility"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class ReflectionEngineUtilityFailureTest
    {
        /// <summary>
        /// Test GetUniqueID(MemberInfo) with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID_Null1()
        {
            ReflectionEngineUtility.GetUniqueID((MemberInfo) null);
        }

        /// <summary>
        /// Test GetUniqueID(Type) with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID_Null2()
        {
            ReflectionEngineUtility.GetUniqueID((Type)null);
        }

        /// <summary>
        /// Test GetUniqueID(FieldInfo) with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID_Null3()
        {
            ReflectionEngineUtility.GetUniqueID((FieldInfo)null);
        }

        /// <summary>
        /// Test GetUniqueID(EventInfo) with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID_Null4()
        {
            ReflectionEngineUtility.GetUniqueID((EventInfo)null);
        }

        /// <summary>
        /// Test GetUniqueID(Type) with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID_Null5()
        {
            ReflectionEngineUtility.GetUniqueID((PropertyInfo)null);
        }

        /// <summary>
        /// Test GetUniqueID(Type) with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetUniqueID_Null6()
        {
            ReflectionEngineUtility.GetUniqueID((MethodBase)null);
        }

        /// <summary>
        /// Test GetFieldVisibility with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetFieldVisibility_Null()
        {
            ReflectionEngineUtility.GetFieldVisibility(null);
        }

        /// <summary>
        /// Test GetFieldModifiers with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetFieldModifiers_Null()
        {
            ReflectionEngineUtility.GetFieldModifiers(null);
        }

        /// <summary>
        /// Test GetPropertyVisibility with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetPropertyVisibility_Null()
        {
            ReflectionEngineUtility.GetFieldVisibility(null);
        }

        /// <summary>
        /// Test GetPropertyModifiers with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetPropertyModifiers_Null()
        {
            ReflectionEngineUtility.GetPropertyModifiers(null);
        }

        /// <summary>
        /// Test GetMethodVisibility with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetMethodVisibility_Null()
        {
            ReflectionEngineUtility.GetMethodVisibility(null);
        }

        /// <summary>
        /// Test GetMethodModifiers with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetMethodModifiers_Null()
        {
            ReflectionEngineUtility.GetMethodModifiers(null);
        }

        /// <summary>
        /// Test GetTypeVisibility with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetTypeVisibility_Null()
        {
            ReflectionEngineUtility.GetTypeVisibility(null);
        }

        /// <summary>
        /// Test GetTypeModifiers with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetTypeModifiers_Null()
        {
            ReflectionEngineUtility.GetTypeModifiers(null);
        }
    }
}
