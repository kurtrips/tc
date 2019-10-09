// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER
using System;
using System.Reflection;
using TopCoder.Configuration;

namespace TopCoder.CodeDoc.CSharp
{
    /// <summary>
    /// Helper class for the unit tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [CoverageExclude]
    internal static class UnitTestHelper
    {
        /// <summary>
        /// The path of the MockLibrary dll used for testing.
        /// </summary>
        public const string MOCKLIBPATH = "../../test_files/MockLibrary.dll";

        /// <summary>
        /// The reference path to be used for testing.
        /// </summary>
        public const string REFPATH = "../../test_files/ReferencePath";

        /// <summary>
        /// The path of the xml documentation of the MockLibrary dll to be used for testing.
        /// </summary>
        public const string MOCKXMLPATH = "../../test_files/MockLibrary.xml";

        /// <summary>
        /// The path of the BaseLibrary dll to be used for testing.
        /// </summary>
        public const string BASELIBPATH = REFPATH + "/BaseLibrary.dll";

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
        /// Gets the configuration for creating the CommandLineProcessor
        /// </summary>
        /// <returns>The IConfiguration object.</returns>
        public static IConfiguration GetConfig()
        {
            //CSharpAPIProcessor configuration
            IConfiguration config = new DefaultConfiguration("processor");
            config.SetSimpleAttribute("processor_factory",
                "TopCoder.CodeDoc.CSharp.CSharpAPIProcessorFactory, " +
                "TopCoder.CodeDoc.CSharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            config.SetSimpleAttribute("log", "MyLoggerNamespace");

            //CSharpAPIProcessorFactory configuration
            IConfiguration processorFactoryConfig = new DefaultConfiguration("processor_factory_config");
            processorFactoryConfig.SetAttribute("reference_paths", new string[] { REFPATH });
            processorFactoryConfig.SetSimpleAttribute("logger_namespace", "MyLoggerNamespace");

            config.AddChild(processorFactoryConfig);
            return config;
        }
    }
}
