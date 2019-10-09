/*
 * Copyright (C) 2007 TopCoder Inc., All rights reserved.
 */

using System;
using System.IO;
using System.Xml;
using TopCoder.CodeDoc.CSharp.Reflection;
using NUnit.Framework;

namespace TopCoder.CodeDoc.CSharp.AccuracyTests.Reflection
{
    /// <summary>
    /// Accuracy tests for the <see cref="ReflectionEngine"/> class.
    /// </summary>
    /// <remarks>
    /// Note that the mock class that's used for these tests actually includes more elements. Check out the resulting
    /// file manually as well, some issues are open to interpretation. Some of the existing XPath might be incorrect
    /// as these tests are written when there is no very conformant implementation to check against.
    /// </remarks>
    /// <author>
    /// cnettel
    /// </author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (C) 2007 TopCoder Inc., All rights reserved.
    /// </copyright>
    [TestFixture, CoverageExclude]
    public class ReflectionEngineAccuracy
    {
        /// <summary>
        /// The ReflectionEngine instance to test against.
        /// </summary>
        private ReflectionEngine engine;

        /// <summary>
        /// A result string.
        /// </summary>
        private string resultString;

        /// <summary>
        /// The result document, some verification is done through simple XPath.
        /// </summary>
        private XmlDocument resultDocument;

        /// <summary>
        /// Sets up test environment.
        /// </summary>
        [SetUp]
        public void Setup()
        {            
            ReflectionEngineParameters parameters = new ReflectionEngineParameters();
            parameters.AssemblyFileNames = new string[] { "../../test_files/accuracy/MockClass.dll" };
            parameters.DocumentPrivates = true;
            parameters.ReferencePaths = new string[] { "../../test_files/accuracy" };
            parameters.SlashDocFileNames = new string[] { "../../test_files/accuracy/MockClass.xml" };
            parameters.ModuleNames = new string[] { "MockClass.dll" };
            parameters.TypePrefixes = new string[] { "M", "S", "E", "A", "T" };

            DoResults(parameters);
        }

        /// <summary>
        /// Prepares the result fields.
        /// </summary>
        /// <param name="parameters">The parameters to use.</param>
        protected void DoResults(ReflectionEngineParameters parameters)
        {
            engine = new ReflectionEngine();

            // No existing spec, really, you can't have everything
            resultString = engine.WriteAPISpec(parameters, "<apispec><package name='IDONTEXIST'/></apispec>");

            resultDocument = new XmlDocument();
            resultDocument.LoadXml(resultString);
            resultString = resultDocument.InnerXml;

            Console.WriteLine("RESULTS FOR TEST {0}", GetType());

            Console.WriteLine();
            Console.WriteLine("You can comment out the code that creates a text version of the accuracy XML.");
            Console.WriteLine("Manual inspection might show additional, unexpected errors.");
            Console.WriteLine("Ideally, the results should be well-defined enough for one unique XML structure" +
                "to be expected.");
            Console.WriteLine("When the component has gone through final fixes, a 'gold standard' with the output can");
            Console.WriteLine("be created for future regression testing.");

            // File.WriteAllText(@"../../test_files/accuracy/somelittlefile.xml", resultString);
        }

        /// <summary>
        /// Assertion that exactly one node is found.
        /// </summary>
        /// <param name="xpath">The xpath.</param>
        /// <param name="message">The message.</param>
        private void AssertOne(string xpath, string message)
        {
            Assert.AreEqual(1, resultDocument.SelectNodes(xpath).Count, message);
        }

        /// <summary>
        /// Verify that the mock class is present.
        /// </summary>
        [Test]
        public void MockClassPresent()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']", "The MockClass should be present.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass' and parent='System.Exception']",
                "The MockClass parent should be present.");
        }

        /// <summary>
        /// Verify that the virtual method is present.
        /// </summary>
        [Test]
        public void VirtMethod()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/method[@name='VirtMethod' and @visibility='public' and @modifiers='virtual']",
                "The VirtMethod should be present.");
        }

        /// <summary>
        /// A simple property.
        /// </summary>
        [Test]
        public void Property()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='Property']",
                "The property should be present.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='Property' and @visibility='private' and " + 
                "@modifiers='read-write']",
                "The property should carry correct visibility and modifiers.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='Property']/doc/summary",
                "The property should carry correct documentation.");

            Assert.IsTrue(resultDocument.SelectSingleNode("apispec/package[@name='TopCoder.CodeDoc.CSharp." +
                "AccuracyTests']/class[@name='MockClass']/property[@name='Property']/type").InnerXml.
                Contains(@"<typeref xpath=""/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']"), "The type reference should be correct.");
        }

        /// <summary>
        /// A private property.
        /// </summary>
        [Test]
        public void PrivateProperty()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='PrivateProperty']",
                "The property should be present.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='PrivateProperty' and @visibility='private' and " +
                "@modifiers='read-write']",
                "The property should carry correct visibility and modifiers.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='PrivateProperty']/doc/summary",
                "The property should carry correct documentation.");
        }

        /// <summary>
        /// A static property.
        /// </summary>
        [Test]
        public void StaticProperty()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='StaticProperty']",
                "The property should be present.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/property[@name='StaticProperty' and @visibility='private' and " +
                "@modifiers='static read-write']",
                "The property should carry correct visibility and modifiers.");
        }

        /// <summary>
        /// The indexer. Are overloads handled correctly, or not?
        /// </summary>
        [Test]
        public void Indexer()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/indexer[@name='Item' and @visibility='public' and @modifiers='write']",
                "The property should carry correct visibility and modifiers.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/indexer[@name='Item' and @visibility='public' and @modifiers='write']" +
                "/param[@name='x']",
                "The property should carry correct param info.");

            //AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
            //    "class[@name='MockClass']/indexer[@name='Item' and @visibility='public' and @modifiers='write' and type='System.Int32']",
            //    "The property should carry correct type info.");
        }

        /// <summary>
        /// The indexer. Are overloads handled correctly, or not?
        /// </summary>
        [Test]
        public void Indexer2()
        {
            //AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
            //    "class[@name='MockClass']/indexer[@name='Item' and @visibility='private' and @modifiers='read-write']",
            //    "The property should carry correct visibility and modifiers.");

            //AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
            //    "class[@name='MockClass']/indexer[@name='Item' and @visibility='private' and @modifiers='read-write']" +
            //    "/param[@name='x']",
            //    "The property should carry correct param info.");

            //AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
            //    "class[@name='MockClass']/property[@name='Item' and @visibility='private' and @modifiers='read-write' and type='System.Int32']",
            //    "The property should carry correct type info.");
        }

        /// <summary>
        /// A public field.
        /// </summary>
        [Test]
        public void PublicField()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/field[@name='engine']",
                "The property should be present.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/field[@name='engine' and @visibility='public']",
                "The property should carry correct visibility.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/field[@name='engine']/type/typeref",
                "The type can at least make some attempt to seem correct.");
        }

        /// <summary>
        /// Verifies the existence of the class AnotherInner in the right place.
        /// </summary>
        [Test]
        public void VeryInner()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/class[@name='PrivateInner']/class[@name='AnotherInner']",
                "The inner class should be present.");
            
        }

        /// <summary>
        /// Verify some xpath in docs, in two ways.
        /// </summary>
        [Test]
        public void InnerContstructorRef()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "class[@name='MockClass']/class[@name='PrivateInner']/constructor[2]/doc/summary/see/@xpath",
                "The XPath docs should be present.");

            Assert.IsFalse(resultString.Contains("xpath=\"M:TopCoder.CodeDoc.CSharp.AccuracyTests.MockClass."
                + "PrivateInner.#ctor"), "The constructor reference should be actual XPath.");
        }

        /// <summary>
        /// Inner types can be present inside structs as well.
        /// </summary>
        [Test]
        public void InnerInStruct()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "struct[@name='Struct']/class[@name='Hoj']",
                "Inner types are allowed in structs.");
        }

        /// <summary>
        /// Enums are present. The schema was incorrect...
        /// </summary>
        [Test]
        public void Enum()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "enum[@name='Enum' and type='System.Int32']",
                "Enums are allowed.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "enum[@name='Enum']/value[@name='x']",
                "X is one value.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "enum[@name='Enum']/value[@name='y']",
                "Y is another value.");
        }

        /// <summary>
        /// Delegates can be present.
        /// </summary>
        [Test]
        public void ADelegate()
        {
            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "delegate[@name='ADelegate' and @visibility='public']",
                "A delegate.");

            AssertOne("apispec/package[@name='TopCoder.CodeDoc.CSharp.AccuracyTests']/" +
                "delegate[@name='ADelegate' and @visibility='public']/param[@name='param' and " +
                "@typevaluespec='System.Double']",
                "Delegate argument incorrect.");
        }
    }
}
