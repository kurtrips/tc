using System;
using System.Xml;
using NUnit.Framework;
using TopCoder.CodeDoc.CSharp.Reflection;

namespace TopCoder.CodeDoc.CSharp.FailureTests
{
    /// <summary>
    /// The failure test cases for <see cref="XPathResolver"/> class.
    /// </summary>
    /// <author>Xuchen</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
    [TestFixture, CoverageExclude]
    public class XPathResolverFailureTest
    {
        /// <summary>
        /// The XPathResolverTester instance used for testing.
        /// </summary>
        private XPathResolverTester resolver;

        /// <summary>
        /// Set up testing environment.
        /// </summary>
        [SetUp]
        protected void SetUp()
        {
            resolver = new XPathResolverTester();
        }

        /// <summary>
        /// Test Constructor with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor_Null()
        {
            new XPathResolver(null);
        }

        /// <summary>
        /// Test AddXPathReferences method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestAddXPathReferences_Null1()
        {
            resolver.AddXPathReferences(null);
        }

        /// <summary>
        /// Test AddXPathReferences method with invalid document.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddXPathReferences_Invalid1()
        {
            XmlDocument doc = new XmlDocument();
            resolver.AddXPathReferences(doc);
        }
        
        /// <summary>
        /// Test AddXPathReferences method with invalid document.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddXPathReferences_Invalid2()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<abc></abc>");
            resolver.AddXPathReferences(doc);
        }

        /// <summary>
        /// Test ResolveXPath method with null argument.
        /// It should throw ArgumentNullException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestResolveXPath_Null1()
        {
            resolver.ResolveXPath(null);
        }

        /// <summary>
        /// Test ResolveXPath method with null argument.
        /// It should throw ArgumentException.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestResolveXPath_Empty1()
        {
            resolver.ResolveXPath("     ");
        }
        
        /// <summary>
        /// This class extends from XPathResolver and is aimed to expose the protected method to test.
        /// </summary>
        /// <author>Xuchen</author>
        /// <version>1.0</version>
        /// <copyright>Copyright (C) 2007 TopCoder Inc., All rights reserved.</copyright>
        [CoverageExclude]
        private class XPathResolverTester : XPathResolver
        {
            /// <summary>
            /// Create an instance of XPathResolverTester.
            /// </summary>
            public XPathResolverTester()
            {
            }

            /// <summary>Delegate to call its name-like method in base class.</summary>
            ///
            /// <param name="identifier">The identifier</param>
            ///
            /// <returns>the XPath represetation for the given identifier.</returns>
            ///
            /// <exception cref="ArgumentNullException">if argument is null.</exception>
            /// <exception cref="ArgumentException">if argument is empty string.</exception>
            public new string ResolveXPath(string identifier)
            {
                return base.ResolveXPath(identifier);
            }
        }
    }
}
