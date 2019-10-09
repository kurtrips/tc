// Copyright (c) 2007, TopCoder, Inc. All rights reserved
// @author TCSDEVELOPER

using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.IO;
using TopCoder.Util.DataValidator;
using TopCoder.Util.ConfigurationManager;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow.Detectors
{
    /// <summary>
    /// Unit tests for the XmlMessageTypeDetector class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class XmlMessageTypeDetectorTests
    {
        /// <summary>
        /// The XmlMessageTypeDetector instance to use throughout the tests.
        /// </summary>
        XmlMessageTypeDetector detector;

        /// <summary>
        /// The ConfigManager instance to use throughout the tests.
        /// </summary>
        ConfigManager configMan;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            configMan = ConfigManager.GetInstance();
            configMan.Clear();
            configMan.LoadFile("../../test_files/MessageParserMainConfig.xml");
            configMan.LoadFile("../../test_files/MessageParserObjectDefinitionsConfig.xml");

            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector");
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            configMan.Clear();
            configMan = null;

            detector = null;
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlMessageTypeDetector(string nameSpace)
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            Assert.IsTrue(detector is IMessageTypeDetector, "Detector has wrong type.");
        }

        /// <summary>
        /// Tests the constructor when validator is specified in config.
        /// XmlMessageTypeDetector(string nameSpace)
        /// </summary>
        [Test]
        public void TestConstructor1_Validator()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetectorWithValidator");
            Assert.IsTrue(detector is IMessageTypeDetector, "Detector has wrong type.");
        }

        /// <summary>
        /// Tests the constructor when the recipe file configured is not found
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail1()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector.NoSuchRecipeFile");
        }

        /// <summary>
        /// Tests the constructor when the detector configured contains duplicate message types.
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail2()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector.DuplicateTypes");
        }

        /// <summary>
        /// Tests the constructor when the namespace is not found.
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail3()
        {
            detector = new XmlMessageTypeDetector("No.Such.Namespace");
        }

        /// <summary>
        /// Tests the constructor when the xpath value is not configured for the detector.
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail4()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector.MissingXPath");
        }

        /// <summary>
        /// Tests the constructor when the xpath_value value is not configured for the detector.
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail5()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector.MissingXPathValue");
        }

        /// <summary>
        /// Tests the constructor when the type_recipe_file value is not configured for the detector.
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail6()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector.MissingTypeRecipeFile");
        }

        /// <summary>
        /// Tests the constructor when one of xpath, xpath_value or type_recipe_file values misses an equal(=) sign.
        /// XmlMessageTypeDetector(string nameSpace)
        /// ConfigurationException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail7()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetector.MissingEqualSign");
        }

        /// <summary>
        /// Tests the constructor when the namespace specified is null
        /// XmlMessageTypeDetector(string nameSpace)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor1_Fail8()
        {
            detector = new XmlMessageTypeDetector((string)null);
        }

        /// <summary>
        /// Tests the constructor when the namespace specified is empty
        /// XmlMessageTypeDetector(string nameSpace)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor1_Fail9()
        {
            detector = new XmlMessageTypeDetector("   ");
        }

        /// <summary>
        /// Tests the constructor when validator specified in config has wrong type
        /// XmlMessageTypeDetector(string nameSpace)
        /// </summary>
        [Test, ExpectedException(typeof(ConfigurationException))]
        public void TestConstructor1_Fail10()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetectorNoSuchValidator");
            Assert.IsTrue(detector is IMessageTypeDetector, "Detector has wrong type.");
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string,XmlDocument>();
            recipeMap.Add("doc", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("some", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor with a validator
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// </summary>
        [Test]
        public void TestConstructor2_WithValidator()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("doc", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("some", "value1");
            Dictionary<string, IValidator> valMap = new Dictionary<string,IValidator>();
            valMap.Add("val", new MockValidator());

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when configParameters is null
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor2_Fail1()
        {
            detector = new XmlMessageTypeDetector(null as IDictionary<string, IDictionary>);
        }

        /// <summary>
        /// Tests the constructor when the recipe dictionary in configParameters is null
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail2()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = null;
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when the recipe dictionary in configParameters is empty
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail3()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when the recipe dictionary in configParameters is of incorrect type
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail4()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, string> recipeMap = new Dictionary<string, string>();
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when one of the dictionary in added with incorrect name.
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail5()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("doc", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["TheseTestsAreLongAndTiring"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when recipeMap dictionary in added with empty key
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail6()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("  ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when recipeMap dictionary in added with null value
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail7()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" a ", null);
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "value1");
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when typeDetectionMap dictionary in added with null value
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail8()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" a ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", null);
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when the typeDetectionMap dictionary in configParameters is of incorrect type
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail9()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" v ", new XmlDocument());
            Dictionary<string, int> typeDetectionMap = new Dictionary<string, int>();
            typeDetectionMap.Add("key1", 3);
            Dictionary<string, IValidator> valMap = null;

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor when the validatorMap dictionary in configParameters is of incorrect type
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor2_Fail10()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" v ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "3");
            Dictionary<string, int> valMap = new Dictionary<string,int>();

            param["recipe-map"] = recipeMap;
            param["detection-data-map"] = typeDetectionMap;
            param["validator-map"] = valMap;

            detector = new XmlMessageTypeDetector(param);
        }

        /// <summary>
        /// Tests the constructor.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// </summary>
        [Test]
        public void TestConstructor3()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" v ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "3");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor with a validator.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// </summary>
        [Test]
        public void TestConstructor3_Validator()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" v ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "3");
            Dictionary<string, IValidator> valMap = new Dictionary<string,IValidator>();
            valMap.Add("val", new MockValidator());

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeToRecipeMap is null.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentNullException is expected
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor3_Fail1()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = null;
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "3");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeDetectionDataMap is null.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructor3_Fail2()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" v ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = null;
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeToRecipeMap is empty.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail3()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("key1", "3");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeDetectionDataMap is empty.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail4()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("a", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when validatorMap is empty.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail10()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("a", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("ac", "de");
            Dictionary<string, IValidator> valMap = new Dictionary<string,IValidator>();

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeToRecipeMap has an empty key.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail5()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("    ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("abc", "def");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeDetectionDataMap has an empty key.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail6()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" ac   ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("  ", "def");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when validatorMap has an empty key.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail11()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" ac   ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add(" s ", "def");
            Dictionary<string, IValidator> valMap = new Dictionary<string,IValidator>();
            valMap.Add("  ", new MockValidator());

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeToRecipeMap has a null value.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail7()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add("  acd  ", null);
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("abc", "def");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeDetectionDataMap has a null value.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail8()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" ac   ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("abc  ", null);
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when validatorMap has a null value.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail12()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" ac   ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("abc  ", "def");
            Dictionary<string, IValidator> valMap = new Dictionary<string,IValidator>();
            valMap.Add("as", null);

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the constructor when typeDetectionDataMap has an empty value.
        /// XmlMessageTypeDetector(Generic IDictionary typeToRecipeMap, Generic IDictionary typeDetectionDataMap,
        /// Generic IDictionary validatorMap)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestConstructor3_Fail9()
        {
            //Create dictionaries
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
            recipeMap.Add(" ac   ", new XmlDocument());
            Dictionary<string, string> typeDetectionMap = new Dictionary<string, string>();
            typeDetectionMap.Add("abc  ", "       ");
            Dictionary<string, IValidator> valMap = null;

            detector = new XmlMessageTypeDetector(recipeMap, typeDetectionMap, valMap);
        }

        /// <summary>
        /// Tests the GetMessageType method.
        /// string GetMessageType(string messageText)
        /// </summary>
        [Test]
        public void TestGetMessageType()
        {
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            string type = detector.GetMessageType(messageText);

            Assert.AreEqual(type, "SRequest", "GetMessageType returns incorrect value.");
        }

        /// <summary>
        /// Tests the GetMessageType method for failure when messageText is null
        /// string GetMessageType(string messageText)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetMessageTypeFail1()
        {
            detector.GetMessageType(null);
        }

        /// <summary>
        /// Tests the GetMessageType method for failure when messageText is empty.
        /// string GetMessageType(string messageText)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetMessageTypeFail2()
        {
            detector.GetMessageType("  ");
        }

        /// <summary>
        /// Tests the GetMessageType method for failure when messageText is not valid xml.
        /// string GetMessageType(string messageText)
        /// UnknownMessageFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownMessageFormatException))]
        public void TestGetMessageTypeFail3()
        {
            detector.GetMessageType("<root><root>");
        }

        /// <summary>
        /// Tests the GetMessageType method for failure when messageText is not valid xml.
        /// string GetMessageType(string messageText)
        /// UnknownMessageFormatException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownMessageFormatException))]
        public void TestGetMessageTypeFail4()
        {
            detector.GetMessageType("<root><root>");
        }

        /// <summary>
        /// Tests the GetMessageType method for failure when messageText is valid but the xpath_value
        /// specified in config is not found. Therefore message type is not found
        /// string GetMessageType(string messageText)
        /// UnknownMessageTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownMessageTypeException))]
        public void TestGetMessageTypeFail5()
        {
            detector = new XmlMessageTypeDetector("type.detector.XmlMessageTypeDetectorWrongMessageType");
            string messageText = File.ReadAllText("../../test_files/testMessage.xml");
            string type = detector.GetMessageType(messageText);
        }

        /// <summary>
        /// Tests the GetMessageTypeParseRecipe method.
        /// Object GetMessageTypeParseRecipe(string messageType)
        /// </summary>
        [Test]
        public void TestGetMessageTypeParseRecipe()
        {
            object recipe = detector.GetMessageTypeParseRecipe("SRequest");

            Assert.IsTrue(recipe is XmlDocument, "GetMessageTypeParseRecipe returns object of incorrect type.");
            Assert.IsNotNull(recipe, "GetMessageTypeParseRecipe returns null.");
        }

        /// <summary>
        /// Tests the GetMessageTypeParseRecipe method when messageType is null
        /// Object GetMessageTypeParseRecipe(string messageType)
        /// ArgumentNullException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestGetMessageTypeParseRecipeFail1()
        {
            detector.GetMessageTypeParseRecipe(null);
        }

        /// <summary>
        /// Tests the GetMessageTypeParseRecipe method when messageType is empty string
        /// Object GetMessageTypeParseRecipe(string messageType)
        /// ArgumentException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestGetMessageTypeParseRecipeFail2()
        {
            detector.GetMessageTypeParseRecipe("    ");
        }

        /// <summary>
        /// Tests the GetMessageTypeParseRecipe method when messageType does not exist.
        /// Object GetMessageTypeParseRecipe(string messageType)
        /// UnknownMessageTypeException is expected.
        /// </summary>
        [Test, ExpectedException(typeof(UnknownMessageTypeException))]
        public void TestGetMessageTypeParseRecipeFail3()
        {
            detector.GetMessageTypeParseRecipe("NoSuchType");
        }

        /// <summary>
        /// Tests the GetValidator method.
        /// IValidator GetValidator(string messageType)
        /// </summary>
        [Test]
        public void TestGetValidator()
        {
            IValidator validator = detector.GetValidator("SRequest");
            Assert.IsNull(validator, "GetValidator returns wrong value");
        }

        /// <summary>
        /// Tests the CanHandleMessage method.
        /// bool CanHandleMessage(string message)
        /// </summary>
        [Test]
        public void TestCanHandleMessage()
        {
            string text = File.ReadAllText("../../test_files/testMessage.xml");

            Assert.IsTrue(detector.CanHandleMessage(text), "Must return true");
            Assert.IsFalse(detector.CanHandleMessage("<root></root>"), "Must return false");
        }

        /// <summary>
        /// Tests the CanHandleMessage method when xml is not well formed
        /// bool CanHandleMessage(string message)
        /// UnknownMessageTypeException is expected
        /// </summary>
        [Test, ExpectedException(typeof(UnknownMessageFormatException))]
        public void TestCanHandleMessageFail1()
        {
            detector.CanHandleMessage("<root>");
        }
    }
}
