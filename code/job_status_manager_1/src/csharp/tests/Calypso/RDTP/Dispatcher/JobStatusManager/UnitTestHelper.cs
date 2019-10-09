/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Reflection;
using Calypso.RDTP.Dispatcher.JobStatusManager.Prioritizers;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// Helper class for the unit tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class UnitTestHelper
    {
        /// <summary>
        /// The name of the JobComparer nested class.
        /// </summary>
        private const string JOBCOMPARER = "JobComparer";

        /// <summary>
        /// Gets the value of a private field by name for an object.
        /// </summary>
        /// <param name="obj">The object from which to get the private field value.</param>
        /// <param name="fieldName">The name of the field.</param>
        /// <returns>The value of the private field.</returns>
        public static object GetPrivateField(object obj, string fieldName)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Gets an instance of the JobComparer nested class of JobSortedCollection.
        /// </summary>
        /// <returns>The JobComparer instance</returns>
        public static IPrioritizer GetJobComparerInstance(IPrioritizer ctorParam)
        {
            //Get the type of JobComparer class
            Type t = typeof(JobSortedCollection).GetNestedType(JOBCOMPARER, BindingFlags.NonPublic);

            //Get its constructor
            ConstructorInfo c = t.GetConstructor(new Type[] { typeof(IPrioritizer) });

            //Invoke the constructor.
            IPrioritizer jobComparer = (IPrioritizer)c.Invoke(new object[] { ctorParam });

            return jobComparer;
        }
    }
}
