/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.IO;
using TopCoder.MSMQ.MessageProcessingWorkflow.Detectors;
using TopCoder.Util.DataValidator;
using TopCoder.Util.ConfigurationManager;
using NUnit.Framework;

namespace TopCoder.MSMQ.MessageProcessingWorkflow
{
    /// <summary>
    /// <para>
    /// Accuracy tests for the <c>XmlMessageTypeDetector</c> class.
    /// </para>
    /// </summary>
    /// <author>tianniu</author>
    /// <version>1.1</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class XmlMessageTypeDetectorAccuracyTest
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
            configMan.LoadFile("../../test_files/accuracyTests/MessageParserMainConfig.xml");
            configMan.LoadFile("../../test_files/accuracyTests/MessageParserObjectDefinitionsConfig.xml");

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
        /// Tests the constructor.
        /// XmlMessageTypeDetector(IDictionary_Generic configParameters)
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            //Create param
            IDictionary<string, IDictionary> param = new Dictionary<string, IDictionary>();

            //Create internal dictionary
            Dictionary<string, XmlDocument> recipeMap = new Dictionary<string, XmlDocument>();
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
        /// Tests the GetMessageType method.
        /// string GetMessageType(string messageText)
        /// </summary>
        [Test]
        public void TestGetMessageType()
        {
            string messageText = File.ReadAllText("../../test_files/accuracyTests/message/SRequest.xml");
            string type = detector.GetMessageType(messageText);

            Assert.AreEqual(type, "SRequest", "GetMessageType returns incorrect value.");
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
            string text = File.ReadAllText("../../test_files/accuracyTests/message/SRequest.xml");

            Assert.IsTrue(detector.CanHandleMessage(text), "Must return true");
            Assert.IsFalse(detector.CanHandleMessage("<root></root>"), "Must return false");
        }
    }
}
