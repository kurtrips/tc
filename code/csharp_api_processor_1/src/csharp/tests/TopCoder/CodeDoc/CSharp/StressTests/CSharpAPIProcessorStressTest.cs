/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */

using System;
using System.IO;
using System.Xml;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.StressTests
{
    /// <summary>
    /// Stress test for <see cref="CSharpAPIProcessor"/>.
    /// </summary>
    ///
    /// <version>1.0</version>
    /// <author>xxiyy</author>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class CSharpAPIProcessorStressTest
    {
        /// <summary>
        /// Test <c>ProcessDocument(XmlDocument)</c> method.
        /// </summary>
        [Test]
        public void TestProcessDocument_SmallAssembly()
        {
            ReflectionEngineParameters parameters = new ReflectionEngineParameters();
            parameters.AssemblyFileNames = new string[] { "../../test_files/StressTests/SmallAssembly.dll" };
            parameters.DocumentPrivates = true;
            parameters.ReferencePaths = new string[] { "../../test_files/StressTests" };
            parameters.SlashDocFileNames = new string[] { "../../test_files/StressTests/SmallAssembly.xml" };
            parameters.ModuleNames = new string[] { "SmallAssembly.dll" };
            parameters.TypePrefixes = new string[0];

            TestProcessDocument(parameters);
        }

        /// <summary>
        /// Test <c>ProcessDocument(XmlDocument)</c> method.
        /// </summary>
        [Test]
        public void TestProcessDocument_NormalAssembly()
        {
            ReflectionEngineParameters parameters = new ReflectionEngineParameters();
            parameters.AssemblyFileNames =
                new string[] { "../../test_files/StressTests/TopCoder.Util.ConfigurationManager.dll" };
            parameters.DocumentPrivates = true;
            parameters.ReferencePaths = new string[] { "../../test_files/StressTests" };
            parameters.SlashDocFileNames =
                new string[] { "../../test_files/StressTests/TopCoder.Util.ConfigurationManager.xml" };
            parameters.ModuleNames = new string[] { "TopCoder.Util.ConfigurationManager.dll" };
            parameters.TypePrefixes = new string[] { "TopCoder" };

            TestProcessDocument(parameters);
        }

        /// <summary>
        /// Test <c>ProcessDocument(XmlDocument)</c> method.
        /// </summary>
        /// <param name="rep">Some parameter to run ReflectionEngine</param>
        private void TestProcessDocument(ReflectionEngineParameters rep)
        {
            CSharpAPIProcessor processor = new CSharpAPIProcessor(rep);

            DateTime startTime = DateTime.Now;
            int performanceCount = 0;

            while ((startTime + new TimeSpan(0, 0, 10)) > DateTime.Now)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<apispec/>");
                processor.ProcessDocument(doc);
                performanceCount++;
            }

            Console.WriteLine("Generator doc for assembly {0} used {1}s ( {2} times/{3} sec )",
                rep.AssemblyFileNames, 10.0 / performanceCount, performanceCount, 10);
        }
    }
}
