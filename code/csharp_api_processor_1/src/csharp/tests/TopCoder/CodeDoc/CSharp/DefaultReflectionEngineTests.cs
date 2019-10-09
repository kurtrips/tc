// Copyright (c)2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using NUnit.Framework;
using System.Xml;
using System.Xml.Schema;
using System.Collections;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ConfigurationManager;

namespace TopCoder.CodeDoc.CSharp.Reflection
{
    /// <summary>
    /// Unit tests for the ReflectionEngine class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture, TopCoder.CodeDoc.CSharp.CoverageExclude]
    public class ReflectionEngineTests
    {
        /// <summary>
        /// The ReflectionEngine instance to use for the tests.
        /// </summary>
        ReflectionEngine re;

        /// <summary>
        /// The ReflectionEngineParameters instance to use for the tests.
        /// </summary>
        ReflectionEngineParameters rep;

        /// <summary>
        /// Stores the output of WriteAPISpec function once at the start of each test.
        /// </summary>
        XmlDocument xmlOutput;

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private static string METHOD_XPATH_STUB = "method[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string CLASS_XPATH_STUB = "class[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string INTERFACE_XPATH_STUB = "interface[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string ENUM_XPATH_STUB = "enum[type='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string STRUCT_XPATH_STUB = "struct[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string PACKAGE_XPATH_STUB = "package[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string PARAM_XPATH_STUB = "param[{0}]='{1}'";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string FIELD_XPATH_STUB = "field[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string PROPERTY_XPATH_STUB = "property[@name='{0}']";

        /// <summary>
        /// An xpath for a selecting method with given name.
        /// </summary>
        private const string EVENT_XPATH_STUB = "event[@name='{0}']";

        /// <summary>
        /// A general stub for forming a typeref xml node.
        /// </summary>
        private const string TYPEREF_PREFIX_STUB = "<typeref xpath=\"{0}\" />";

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");

            rep = new ReflectionEngineParameters();
            rep.AssemblyFileNames = new string[] { UnitTestHelper.MOCKLIBPATH };
            rep.ReferencePaths = new string[] { UnitTestHelper.REFPATH };
            rep.SlashDocFileNames = new string[] { UnitTestHelper.MOCKXMLPATH };
            rep.DocumentPrivates = true;

            re = new ReflectionEngine();

            //Write the API specification
            string xml = re.WriteAPISpec(rep, "<apispec></apispec>");
            xmlOutput = new XmlDocument();
            xmlOutput.LoadXml(xml);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            re = null;
            rep = null;
            xmlOutput = null;
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Tests the constructor.
        /// ReflectionEngine()
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.AreEqual(typeof(MarshalByRefObject), re.GetType().BaseType, "Must derive from MarshalByRefObject");
        }

        /// <summary>
        /// Tests the constructor.
        /// ReflectionEngine(MBRLogger logger)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            Logger logger = LogManager.CreateLogger("MyLoggerNamespace");
            MBRLogger mbrLogger = new MBRLogger(logger);

            re = new ReflectionEngine(mbrLogger);
            Assert.IsTrue(object.ReferenceEquals(UnitTestHelper.GetPrivateField(re, "logger"), mbrLogger),
                "Incorrect constructor implementation.");
        }

        /// <summary>
        /// Tests the constructor for failure when logger is null
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorFail()
        {
            re = new ReflectionEngine(null);
        }

        /// <summary>
        /// Tests the WriteAPISpec method when document privates is false.
        /// Private types should not be documented for ex. privateInt field in ClassC.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_NoDocPrivate()
        {
            //Write the API specification
            rep.DocumentPrivates = false;
            string xml = re.WriteAPISpec(rep, "<apispec></apispec>");
            xmlOutput = new XmlDocument();
            xmlOutput.LoadXml(xml);

            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'InterfaceD' node
            XmlNode privateIntNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary.Nested']/class[@name='ClassC']/field[@name='privateInt']");
            Assert.IsNull(privateIntNode, "privateInt must not be documented.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassA is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassA()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'ClassA' node
            XmlNode classANode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassA']");
            Assert.IsNotNull(classANode, "Wrong WriteAPISpec implementation.");

            //Check the System.Object methods
            CheckObjectMethods(classANode);
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassB is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassB()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'ClassB' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassB']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check the System.Object methods
            CheckObjectMethods(classNode);

            //Check its 'parent' child nodes as it derives from InterfaceA
            Assert.AreEqual(classNode.SelectNodes("parent")[0].InnerXml,
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/interface[@name='InterfaceA']\">InterfaceA</typeref>",
                "Wrong parent node contents.");

            Assert.AreEqual(classNode.SelectNodes("parent")[1].InnerXml,
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/class[@name='ClassA']\">ClassA</typeref>",
                "Wrong parent node contents.");

            //TestMethod1
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "TestMethod1")),
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/class[@name='ClassA']\">ClassA</typeref>[]",
                new string[] { "objA" }, new string[] { "MockLibrary.ClassA" },
                "public", null);

            //GetName
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "GetName")),
                "System.<typeref xpath=\"/package[@name='System']/class[@name='String']\">String</typeref>",
                new string[] { "id" }, new string[] { "System.Int32" }, "public", "static");

            //GetNames
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "GetNames")),
                "System.Collections.<typeref xpath=\"/package[@name='System.Collections']/interface[@name='IList']\">IList</typeref>",
                new string[] { "ids" }, new string[] { "System.Int32[]" }, "public", "static");

            //SetName
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "SetName")),
                "System.<typeref xpath=\"/package[@name='System']/struct[@name='Void']\">Void</typeref>",
                new string[] { "id", "name" }, new string[] { "System.Int32", "System.String" },
                "public", null);

            //IsPropNull
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "IsPropNull")),
                "System.<typeref xpath=\"/package[@name='System']/struct[@name='Boolean']\">Boolean</typeref>",
                new string[0], new string[0], "public", null);

            //TestProperty1
            CheckPropertyNode(classNode.SelectSingleNode(string.Format(PROPERTY_XPATH_STUB, "TestProperty1")),
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/class[@name='ClassA']\">ClassA</typeref>",
                "public", "virtual read-write");

            //namesIds
            CheckFieldNode(classNode.SelectSingleNode(string.Format(FIELD_XPATH_STUB, "namesIds")),
                "System.Collections.<typeref xpath=\"/package[@name='System.Collections']/interface[@name='IDictionary']\">IDictionary</typeref>",
                "public");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the EnumA is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_EnumA()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'EnumA' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/enum[@name='EnumA']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check type of enum
            Assert.AreEqual("System.Int32", classNode["type"].InnerText, "Must have type int");

            //Check doc of enum
            Assert.IsNotNull(classNode["doc"]["summary"], "Must have a doc with summary.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the delegate AlarmEventHandler is read correctly
        /// from the MockLibrary.dll and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_Delegate()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'AlarmEventHandler' node
            XmlNode delNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/delegate[@name='AlarmEventHandler']");
            Assert.IsNotNull(delNode, "Wrong WriteAPISpec implementation.");

            //Check Annotation child and its value
            CheckNodeValueAndAttributes(delNode.SelectSingleNode("annotation"),
                new string[0], new string[0],
                "System.<typeref xpath=\"/package[@name='System']/class[@name='SerializableAttribute']\">SerializableAttribute</typeref>");

            //Check return child and its value
            CheckNodeValueAndAttributes(delNode.SelectSingleNode("return"),
                new string[0], new string[0], "System.<typeref xpath=\"/package[@name='System']/class[@name='String']\">String</typeref>");

            //Check first param child.
            CheckNodeValueAndAttributes(delNode.SelectNodes("param")[0],
                new string[] { "name", "typevaluespec" },
                new string[] { "sender", "System.Object" },
                "System.<typeref xpath=\"/package[@name='System']/class[@name='Object']\">Object</typeref>");

            //Check second param child.
            CheckNodeValueAndAttributes(delNode.SelectNodes("param")[1],
                new string[] { "name", "typevaluespec" },
                new string[] { "e", "System.EventArgs" },
                "System.<typeref xpath=\"/package[@name='System']/class[@name='EventArgs']\">EventArgs</typeref>");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the event Alarm is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_Alarm()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'event' node
            XmlNode eventNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='WakeMeUp']/event[@name='Alarm']");
            Assert.IsNotNull(eventNode, "Wrong WriteAPISpec implementation.");

            //Check type child and its value (which should be an xpath to the AlarmEventHandler
            CheckNodeValueAndAttributes(eventNode.SelectSingleNode("type"),
                new string[0], new string[0],
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/delegate[@name='AlarmEventHandler']\">AlarmEventHandler</typeref>");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the InterfaceA is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_InterfaceA()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'interface' node
            XmlNode interNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/interface[@name='InterfaceA']");
            Assert.IsNotNull(interNode, "Wrong WriteAPISpec implementation.");

            //Check its method
            CheckMethodNode(interNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "GetAInstance")),
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/enum[type='EnumA']\">EnumA</typeref>",
                new string[] { "b" }, new string[] { "MockLibrary.ClassB" }, "public", "abstract");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the InterfaceB is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_InterfaceB()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'interface' node
            XmlNode interNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/interface[@name='InterfaceB']");
            Assert.IsNotNull(interNode, "Wrong WriteAPISpec implementation.");

            //Check its method
            CheckMethodNode(interNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "JustDoIt")),
                "System.<typeref xpath=\"/package[@name='System']/class[@name='String']\">String</typeref>",
                new string[] { "listParam", "boolParam", "arrayParam" },
                new string[] { "System.Collections.IList", "System.Boolean", "System.String[]" },
                "public", "abstract");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the InterfaceC is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_InterfaceC()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'interface' node
            XmlNode interNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/interface[@name='InterfaceC']");
            Assert.IsNotNull(interNode, "Wrong WriteAPISpec implementation.");

            //Check its method
            CheckMethodNode(interNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "SayHelloToTheWorld")),
                "System.<typeref xpath=\"/package[@name='System']/struct[@name='Void']\">Void</typeref>",
                new string[0], new string[0], "public", "abstract");

            //Check its 'parent' child nodes as it derives from InterfaceA, InterfaceB
            Assert.AreEqual(interNode.SelectNodes("parent")[1].InnerXml,
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/interface[@name='InterfaceA']\">InterfaceA</typeref>",
                "Wrong parent node contents.");
            Assert.AreEqual(interNode.SelectNodes("parent")[0].InnerXml,
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/interface[@name='InterfaceB']\">InterfaceB</typeref>",
                "Wrong parent node contents.");

        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the StructA is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_StructA()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'struct' node
            XmlNode structNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/struct[@name='StructA']");
            Assert.IsNotNull(structNode, "Wrong WriteAPISpec implementation.");

            //Check if all the object methods are present
            CheckObjectMethods(structNode);

            //Check its Method1 method
            CheckMethodNode(structNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "Method1")),
                "System.<typeref xpath=\"/package[@name='System']/class[@name='String']\">String</typeref>",
                new string[] { "doc" }, new string[] { "System.Xml.XmlDocument" }, "public", null);

            //Check its 'parent' child nodes as it derives from InterfaceA
            Assert.AreEqual(structNode.SelectNodes("parent")[0].InnerXml,
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/interface[@name='InterfaceA']\">InterfaceA</typeref>",
                "Wrong parent node contents.");

            Assert.AreEqual(structNode.SelectNodes("parent")[1].InnerXml,
                "System.<typeref xpath=\"/package[@name='System']/class[@name='ValueType']\">ValueType</typeref>",
                "Wrong parent node contents.");

            //Check its array field
            XmlNode node = structNode.SelectSingleNode(string.Format(FIELD_XPATH_STUB, "array"));
            CheckFieldNode(node,
                "MockLibrary.<typeref xpath=\"/package[@name='MockLibrary']/struct[@name='StructA']\">StructA</typeref>[]",
                "public");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassF is read correctly from the MockLibrary.dll
        /// and written to xml correctly. ClassF derives from BaseLibrary.ClassBaseA but here BaseLibrary is not
        /// documented.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassF_1()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'classF' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassF']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check if all te object methods are present
            CheckObjectMethods(classNode);

            //Check its 'ADependentFunction' method.
            //Note here that xpath is not formed for the ClassBaseA class.
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "ADependentFunction")),
                "BaseLibrary.<typeref xpath=\"/package[@name='BaseLibrary']/class[@name='ClassBaseA']\">ClassBaseA</typeref>",
                new string[0], new string[0], "public", null);

            //Check its 'Doit' method
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "Doit")),
                "System.<typeref xpath=\"/package[@name='System']/class[@name='String']\">String</typeref>",
                new string[0], new string[0], "public", null);

            //Check its 'parent' child nodes as it derives from ClassBaseA
            //Note here that xpath is not formed for the ClassBaseA class.
            Assert.AreEqual(classNode.SelectNodes("parent")[0].InnerXml,
                "BaseLibrary.<typeref xpath=\"/package[@name='BaseLibrary']/class[@name='ClassBaseA']\">ClassBaseA</typeref>",
                "Wrong parent node contents.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassF is read correctly from the MockLibrary.dll
        /// and written to xml correctly. ClassF derives from BaseLibrary.ClassBaseA .Here BaseLibrary is
        /// documented.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassF_2()
        {
            //This time document the BaseLibrary also.
            rep.AssemblyFileNames = new string[] { UnitTestHelper.MOCKLIBPATH,
                UnitTestHelper.BASELIBPATH };
            re = new ReflectionEngine();
            string xml = re.WriteAPISpec(rep, "<apispec></apispec>");
            xmlOutput = new XmlDocument();
            xmlOutput.LoadXml(xml);

            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'classF' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassF']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check if all te object methods are present
            CheckObjectMethods(classNode);

            //Check its 'ADependentFunction' method.
            //Note here that xpath is formed for the ClassBaseA class.
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "ADependentFunction")),
                "BaseLibrary.<typeref xpath=\"/package[@name='BaseLibrary']/class[@name='ClassBaseA']\">ClassBaseA</typeref>",
                new string[0], new string[0], "public", null);

            //Check its 'Doit' method
            CheckMethodNode(classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "Doit")),
                "System.<typeref xpath=\"/package[@name='System']/class[@name='String']\">String</typeref>",
                new string[0], new string[0], "public", null);

            //Check its 'parent' child nodes as it derives from ClassBaseA
            //Note here that xpath is formed for the ClassBaseA class.
            Assert.AreEqual(classNode.SelectNodes("parent")[0].InnerXml,
                "BaseLibrary.<typeref xpath=\"/package[@name='BaseLibrary']/class[@name='ClassBaseA']\">ClassBaseA</typeref>",
                "Wrong parent node contents.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassC is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassC()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'ClassC' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary.Nested']/class[@name='ClassC']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check the System.Object methods
            CheckObjectMethods(classNode);

            //ClassD
            XmlNode node = classNode.SelectSingleNode(string.Format(CLASS_XPATH_STUB, "ClassD"));
            Assert.IsNotNull(node, "Class is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            //InterfaceD
            node = classNode.SelectSingleNode(string.Format(INTERFACE_XPATH_STUB, "InterfaceD"));
            Assert.IsNotNull(node, "Interface is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassD is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassD()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'ClassD' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary.Nested']/class[@name='ClassD']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check the System.Object methods
            CheckObjectMethods(classNode);

            //MethodC
            XmlNode node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "MethodC"));
            Assert.IsNotNull(node, "Entity is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "protected", "Wrong visibility.");
            Assert.AreEqual(node.Attributes["modifiers"].Value, "virtual", "Wrong visibility.");

            //ClassEvent
            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "ClassEvent"));
            Assert.IsNotNull(node, "Entity is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            //Prop
            node = classNode.SelectSingleNode(string.Format(PROPERTY_XPATH_STUB, "Prop"));
            Assert.IsNotNull(node, "Entity is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "protected", "Wrong visibility.");
            Assert.AreEqual(node.Attributes["modifiers"].Value, "read-write", "Wrong visibility.");

            //count
            node = classNode.SelectSingleNode(string.Format(FIELD_XPATH_STUB, "count"));
            Assert.IsNotNull(node, "Entity is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");
            Assert.AreEqual(node.Attributes["modifiers"].Value, "static readonly", "Wrong visibility.");

            //ClassEvent
            node = classNode.SelectSingleNode(string.Format(EVENT_XPATH_STUB, "anEvent"));
            Assert.IsNotNull(node, "Entity is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            //ClassE
            node = classNode.SelectSingleNode(string.Format(CLASS_XPATH_STUB, "ClassE"));
            Assert.IsNotNull(node, "Entity is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Tests that the ClassE is read correctly from the MockLibrary.dll
        /// and written to xml correctly.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_ClassE()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'ClassE' node
            XmlNode classNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary.Nested']/class[@name='ClassE']");
            Assert.IsNotNull(classNode, "Wrong WriteAPISpec implementation.");

            //Check the System.Object methods
            CheckObjectMethods(classNode);
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a method.
        /// See documentation of MockLibrary.ClassB.GetNames method.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Method()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.ClassB.GetNames
            XmlNode seeNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassB']/method[@name='GetNames']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points the int overload of MockLibrary.ClassB.GetName
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec/package[@name='MockLibrary']" +
                "/class[@name='ClassB']/method[@name='GetName' and param[1]='System.Int32']"),
                nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a field.
        /// See documentation of MockLibrary.ClassB.SetName(int, string) method.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Field()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.ClassB.SetName(int, string)
            XmlNode seeNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassB']/method[@name='SetName']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the int MockLibrary.ClassB.namesIds field
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec" +
                "/package[@name='MockLibrary']/class[@name='ClassB']/field[@name='namesIds']")
                , nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a property.
        /// See documentation of MockLibrary.ClassB.IsPropNull() method.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Property()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.ClassB.IsPropNull()
            XmlNode seeNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassB']" +
                "/method[@name='IsPropNull']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.ClassB.TestProperty1
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec" +
                "/package[@name='MockLibrary']/class[@name='ClassB']/property[@name='TestProperty1']")
                , nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a class.
        /// See documentation of MockLibrary.TestProperty1 property.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Class1()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.ClassB.TestProperty1
            XmlNode seeNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary']/class[@name='ClassB']"
                + "/property[@name='TestProperty1']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.ClassA
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec" +
                "/package[@name='MockLibrary']/class[@name='ClassA']")
                , nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a nested class.
        /// See documentation of MockLibrary.Nested.ClassC.ClassD.ClassE class.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Class2()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.Nested.ClassC.ClassD.ClassE class.
            XmlNode seeNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary.Nested']/class[@name='ClassE']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.Nested.ClassC.ClassD
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec" +
                "/package[@name='MockLibrary.Nested']/class[@name='ClassC']/class[@name='ClassD']")
                , nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a event.
        /// See documentation of MockLibrary.WakeMeUp.constructor().
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Event()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.WakeMeUp.constructor()
            XmlNode seeNode = xmlOutput.SelectNodes("apispec/package[@name='MockLibrary']/class[@name='WakeMeUp']" +
                "/constructor[@name='.ctor']/doc/summary/see")[1];

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.WakeMeUp.Alarm event
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec" +
                "/package[@name='MockLibrary']/class[@name='WakeMeUp']/event[@name='Alarm']")
                , nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a delegate.
        /// See documentation of MockLibrary.WakeMeUp.constructor().
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Delegate()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.WakeMeUp.constructor()
            XmlNode seeNode = xmlOutput.SelectNodes("apispec/package[@name='MockLibrary']/class[@name='WakeMeUp']" +
                "/constructor[@name='.ctor']/doc/summary/see")[0];

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.AlarmEventHandler delegate
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec" +
                "/package[@name='MockLibrary']/delegate[@name='AlarmEventHandler']")
                , nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to a nested interface.
        /// See documentation of MockLibrary.Nested.ClassC.InterfaceD.MakeMyDay method.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Interface()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.Nested.ClassC.InterfaceD.MakeMyDay method
            XmlNode seeNode = xmlOutput.SelectSingleNode(
                "apispec/package[@name='MockLibrary.Nested']/class[@name='ClassC']" +
                "/interface[@name='InterfaceD']/method[@name='MakeMyDay']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.Nested.ClassC.InterfaceD interface.
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to an enum.
        /// See documentation of MockLibrary.StructA.GetInstance method.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Enum()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.StructA.GetInstance method
            XmlNode seeNode = xmlOutput.SelectSingleNode("apispec/package[@name='MockLibrary']" +
                "/struct[@name='StructA']/method[@name='GetAInstance']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.EnumA interface.
            //XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            //Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            //Assert.AreEqual(xmlOutput.SelectSingleNode("apispec/package[@name='MockLibrary']/enum[type='EnumA']"),
            //    nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method. Test the xpath for a 'see cref' when it points to an struct.
        /// See documentation of MockLibrary.StructA.array field.
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// </summary>
        [Test]
        public void TestWriteAPISpec_SeeCref_Struct()
        {
            //First check output as per schema
            Assert.IsTrue(IsValidXmlAsPerSchema(xmlOutput.OuterXml), "Invalid xml output as per schema.");

            //Select the 'see' node for the doc in MockLibrary.StructA.array method
            XmlNode seeNode = xmlOutput.SelectSingleNode("apispec/package[@name='MockLibrary']" +
                "/struct[@name='StructA']/field[@name='array']/doc/summary/see");

            //Get the xpath attribute value
            string xpath = seeNode.Attributes["xpath"].Value;

            //Make sure it points to the MockLibrary.EnumA interface.
            XmlNodeList nodeList = xmlOutput.SelectNodes("apispec" + xpath);
            Assert.AreEqual(1, nodeList.Count, "Xpath must point to only 1 node in the xml.");
            Assert.AreEqual(xmlOutput.SelectSingleNode("apispec/package[@name='MockLibrary']/struct[@name='StructA']"),
                nodeList[0], "Both nodes must be the same.");
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when parameters is null
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestWriteAPISpecFail1()
        {
            re.WriteAPISpec(null, "<apispec></apispec>");
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when apiSpec is null
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestWriteAPISpecFail2()
        {
            re.WriteAPISpec(rep, null);
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when apiSpec is empty
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpecFail3()
        {
            re.WriteAPISpec(rep, "    ");
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when AssemblyFileNames of parameters is null
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpecFail4()
        {
            rep.AssemblyFileNames = null;
            re.WriteAPISpec(rep, "<apispec></apispec>");
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when AssemblyFileNames of parameters is empty
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpecFail5()
        {
            rep.AssemblyFileNames = new string[0];
            re.WriteAPISpec(rep, "<apispec></apispec>");
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when apispec is not valid xml
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpecFail6()
        {
            re.WriteAPISpec(rep, "<apispec <apispec>");
        }

        /// <summary>
        /// Tests the WriteAPISpec method for failure when apiSpec does not contain root as 'apispec'
        /// string WriteAPISpec(ReflectionEngineParameters parameters, string apiSpec)
        /// ArgumentException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestWriteAPISpecFail7()
        {
            re.WriteAPISpec(rep, "<root><root>");
        }

        /// <summary>
        /// Checks if the xml string is valid as per the apiSpec.xsd
        /// </summary>
        /// <param name="xml">The xml string</param>
        /// <returns>True if valid else false.</returns>
        private static bool IsValidXmlAsPerSchema(string xml)
        {
            try
            {
                //Check validity
                XmlDocument doc = new XmlDocument();
                doc.Schemas.Add(null, "../../test_files/apiSpec.xsd");
                doc.LoadXml(xml);
                doc.Validate(null);

                return true;
            }
            catch (XmlException xe)
            {
                Console.Out.WriteLine(xe.Message);
                return false;
            }
            catch (XmlSchemaException xse)
            {
                Console.Out.WriteLine(xse.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks whether the protected and public methods of System.Object are present in a class's doc.
        /// </summary>
        /// <param name="classNode">The class node</param>
        private static void CheckObjectMethods(XmlNode classNode)
        {
            XmlNode node;

            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "GetType"));
            Assert.IsNotNull(node, "Method is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "MemberwiseClone"));
            Assert.IsNotNull(node, "Method is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "protected", "Wrong visibility.");

            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "ToString"));
            Assert.IsNotNull(node, "Method is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "Equals"));
            Assert.IsNotNull(node, "Method is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "GetHashCode"));
            Assert.IsNotNull(node, "Method is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "public", "Wrong visibility.");

            node = classNode.SelectSingleNode(string.Format(METHOD_XPATH_STUB, "Finalize"));
            Assert.IsNotNull(node, "Method is not found.");
            Assert.AreEqual(node.Attributes["visibility"].Value, "protected", "Wrong visibility.");
        }

        /// <summary>
        /// Checks any node for a set of attributes and their values.
        /// Also checks the value of the node itself
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attrNames"></param>
        /// <param name="attrValues"></param>
        /// <param name="nodeValue"></param>
        private static void CheckNodeValueAndAttributes(XmlNode node,
            string[] attrNames, string[] attrValues, string nodeValue)
        {
            //Check node attributes.
            for (int i = 0; i < attrNames.Length; i++)
            {
                Assert.IsNotNull(node.Attributes[attrNames[i]], "Missing attribute for node.");
                Assert.AreEqual(node.Attributes[attrNames[i]].Value, attrValues[i], "Incorrect value for node.");
            }

            //Check node value
            Assert.AreEqual(node.InnerXml, nodeValue);
        }

        /// <summary>
        /// Gets a string represenetation of a typeref node with xpath set
        /// using the nodeNames and their @name attributes pointing to nameValues.
        /// </summary>
        /// <param name="nodeNames">The node names to form the xpath</param>
        /// <param name="nameValues">The value of the name atrtibutes of the node names for teh xpath.</param>
        /// <returns>a string represenetation of a typeref node</returns>
        private static string GetGeneralTyperef(string[] nodeNames, string[] nameValues)
        {
            string generalXpath = "/{0}[@name='{1}']";
            string ret = string.Empty;

            for (int i = 0; i < nodeNames.Length; i++)
            {
                if (nodeNames[i] == "enum")
                {
                    ret += string.Format("/" + ENUM_XPATH_STUB, nameValues[i]);
                }
                else
                {
                    ret += string.Format(generalXpath, nodeNames[i], nameValues[i]);
                }
            }

            return string.Format(TYPEREF_PREFIX_STUB, ret);
        }

        /// <summary>
        /// Checks a method node output by the WriteApiSpec method for correctness.
        /// </summary>
        /// <param name="methodNode">The node to check</param>
        /// <param name="returnValue">The value of return child of node</param>
        /// <param name="paramNames">The names of the param child nodes formed</param>
        /// <param name="paramValues">The values of the @typevaluespec of param child nodes formed</param>
        /// <param name="visibility">The value of the visibility attribute formed</param>
        /// <param name="modifiers">The value of the modifiers attribute formed</param>
        private static void CheckMethodNode(XmlNode methodNode, string returnValue,
            string[] paramNames, string[] paramValues, string visibility, string modifiers)
        {
            Assert.IsNotNull(methodNode, "Method node was not created.");

            //Check visibility
            Assert.AreEqual(methodNode.Attributes["visibility"].Value, visibility, "visibility value is incorrect.");

            //Check modifers
            if (modifiers != null)
            {
                Assert.AreEqual(methodNode.Attributes["modifiers"].Value, modifiers, "modifiers value is incorrect.");
            }

            //Check return value. Must be xpath
            Assert.AreEqual(methodNode["return"].InnerXml, returnValue, "Return value of method is incorrect.");

            //Check parameters
            for (int i = 0; i < paramNames.Length; i++)
            {
                XmlNode paramNode = methodNode.SelectNodes("param")[i];
                Assert.IsNotNull(paramNode, "Missing parameter node");

                //Check name, typevaluespec attributes
                Assert.AreEqual(paramNode.Attributes["name"].Value, paramNames[i],
                    "Wrong parameter name attribute.");
                Assert.AreEqual(paramNode.Attributes["typevaluespec"].Value, paramValues[i],
                    "Wrong parameter typevaluespec attribute.");
            }
        }

        /// <summary>
        /// Checks a field node output by the WriteApiSpec method for correctness.
        /// </summary>
        /// <param name="fieldNode">the node to check</param>
        /// <param name="type">The type of return child of node</param>
        /// <param name="visibility">The value of the visibility attribute of the node</param>
        private static void CheckFieldNode(XmlNode fieldNode, string type, string visibility)
        {
            Assert.IsNotNull(fieldNode, "Field node was not created.");
            Assert.AreEqual(fieldNode.Attributes["visibility"].Value, visibility, "Wrong visibilty of field.");
            Assert.AreEqual(fieldNode["type"].InnerXml, type, "Wrong type of field.");
        }

        /// <summary>
        /// Checks a field node output by the WriteApiSpec method for correctness.
        /// </summary>
        /// <param name="propNode">the node to check</param>
        /// <param name="type">The type of return child of node</param>
        /// <param name="visibility">The value of the visibility attribute of the node</param>
        /// <param name="modifiers">The value of the modifiers attribute formed</param>
        private static void CheckPropertyNode(XmlNode propNode, string type, string visibility, string modifiers)
        {
            Assert.IsNotNull(propNode, "Property node was not created.");

            //Check visibility
            Assert.AreEqual(propNode.Attributes["visibility"].Value, visibility, "visibility value is incorrect.");

            //Check modifers
            if (modifiers != null)
            {
                Assert.AreEqual(propNode.Attributes["modifiers"].Value, modifiers, "modifiers value is incorrect.");
            }

            Assert.AreEqual(propNode["type"].InnerXml, type, "Wrong type of field.");
        }
    }
}
