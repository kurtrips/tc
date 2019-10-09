/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 

using System;
using NUnit.Framework;
using Astraea.Inframap.Data;
using Astraea.Inframap.Layout.Impl;
using TopCoder.Configuration;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Graph.Layout;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ObjectFactory;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// Unit tests for the BaseLayoutEngine class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class BaseLayoutEngineTests
    {
        /// <summary>
        /// The IConfiguration instance to be used for the constructor.
        /// </summary>
        IConfiguration configForCtor;

        /// <summary>
        /// The BaseLayoutEngine instance to use for the tests.
        /// </summary>
        BaseLayoutEngine ble;

        /// <summary>
        /// The MapData instance to use for the Layout method.
        /// </summary>
        MapData md;

        /// <summary>
        /// The BaseLayoutEngineProtectedTests instance to be used for tesing protected methods.
        /// </summary>
        BaseLayoutEngineProtectedTests blept;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            UnitTestHelper.LoadConfigManager();

            configForCtor = UnitTestHelper.GetTestConfig();
            ble = new LayoutEngine(configForCtor);

            blept = new BaseLayoutEngineProtectedTests();

            md = UnitTestHelper.GetSampleMapData();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            UnitTestHelper.ClearConfigManager();
        }

        /// <summary>
        /// Tests the type of BaseLayoutEngine.
        /// </summary>
        [Test]
        public void TestType()
        {
            Assert.IsTrue(ble is ILayoutEngine, "Must inherit from ILayoutEngine.");
        }

        /// <summary>
        /// Tests the constructor. The Logger, Config and GraphLayouter properties must be set correctly.
        /// </summary>
        [Test]
        public void TestConstructor1()
        {
            //Check logger
            Assert.IsNotNull(ble.Logger, "Logger must be set.");
            Assert.AreEqual("TopCoder.LoggingWrapper.DiagnosticImpl", ble.Logger.GetType().FullName,
                "Correct logger must be created.");

            //Check GraphLayouter
            Assert.IsNotNull(ble.GraphLayouter, "GraphLayouter must be set.");
            Assert.AreEqual("TopCoder.Graph.Layout.Layouter.MockLayouter", ble.GraphLayouter.GetType().FullName,
                "Correct GraphLayouter must be created.");

            //Check Config
            IConfiguration config = (IConfiguration)UnitTestHelper.GetNonPublicPropertyValue(ble, "Config");
            Assert.IsNotNull(config, "Config must be set.");
            Assert.IsFalse(object.ReferenceEquals(config, configForCtor), "Clone should be created by constructor.");
        }

        /// <summary>
        /// Tests the constructor when the default object factory namespace is being used.
        /// The Logger, Config and GraphLayouter properties must be set correctly.
        /// </summary>
        [Test]
        public void TestConstructor2()
        {
            configForCtor.RemoveAttribute("object_factory_ns");
            ble = new LayoutEngine(configForCtor);

            TestConstructor1();
        }

        /// <summary>
        /// Tests the constructor when config is null.
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException
        /// </summary>
        [Test]
        public void TestConstructorFail1()
        {
            try
            {
                new LayoutEngine(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the constructor when config has logger_name which does not exist in configuration file.
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ConfigException
        /// </summary>
        [Test]
        public void TestConstructorFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("logger_name", "NoSuchLogger");
            try
            {
                new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the constructor when config has object_factory_ns which does not exist in configuration file.
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ObjectSourceException
        /// </summary>
        [Test]
        public void TestConstructorFail3()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("object_factory_ns", "NoSuchOFNamespace");
            try
            {
                new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ObjectSourceException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the constructor when config has graph_layouter_token which does not exist in configuration file.
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ObjectSourceException
        /// </summary>
        [Test]
        public void TestConstructorFail4()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("graph_layouter_token", "NoSuchGraphLayouter");
            try
            {
                new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ObjectSourceException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the constructor when config is missing a required property.
        /// <see cref="SelfDocumentingException"/> is expected
        /// with inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestConstructorFail5()
        {
            configForCtor.RemoveAttribute("graph_layouter_token");
            try
            {
                new LayoutEngine(configForCtor);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the constructor when config has a property which must be a string but is not.
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as InvalidCastException
        /// </summary>
        [Test]
        public void TestConstructorFail6()
        {
            configForCtor.SetSimpleAttribute("graph_layouter_token", new object());
            try
            {
                new LayoutEngine(configForCtor);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(InvalidCastException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the DefaultFontSize method.
        /// double DefaultFontSize()
        /// </summary>
        [Test]
        public void TestDefaultFontSize()
        {
            Assert.AreEqual(1.414, ble.DefaultFontSize, "Incorrect DefaultFontSize implementation.");
        }

        /// <summary>
        /// Tests the DefaultFontSize method for failure when configuration has invalid double value.
        /// double DefaultFontSize()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>
        /// </summary>
        [Test]
        public void TestDefaultFontSizeFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("default_font_size", "9.o02");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the DefaultFontSize method for failure when configuration has non-positive double value.
        /// double DefaultFontSize()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestDefaultFontSizeFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("default_font_size", "0.0");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the FontUnits method.
        /// double FontUnits()
        /// </summary>
        [Test]
        public void TestFontUnits()
        {
            Assert.AreEqual(2.456, ble.FontUnits, "Incorrect FontUnits implementation.");
        }

        /// <summary>
        /// Tests the TestFontUnits method for failure when configuration has invalid double value.
        /// double FontUnits()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestFontUnitsFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("font_units", "9.o02");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the TestFontUnits method for failure when configuration has non-positive double value.
        /// double FontUnits()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestFontUnitsFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("font_units", "0.0");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the CharacterUnits method.
        /// double CharacterUnits()
        /// </summary>
        [Test]
        public void TestCharacterUnits()
        {
            Assert.AreEqual(3.321, ble.CharacterUnits, "Incorrect CharacterUnits implementation.");
        }

        /// <summary>
        /// Tests the CharacterUnits method for failure when configuration has invalid double value.
        /// double CharacterUnits()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestCharacterUnitsFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("character_units", "9.o02");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the CharacterUnits method for failure when configuration has non-positive double value.
        /// double CharacterUnits()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestCharacterUnitsFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("character_units", "0.0");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumPortWidth method.
        /// int MinimumPortWidth()
        /// </summary>
        [Test]
        public void TestMinimumPortWidth()
        {
            Assert.AreEqual(4, ble.MinimumPortWidth, "Incorrect MinimumPortWidth implementation.");
        }

        /// <summary>
        /// Tests the MinimumPortWidth method for failure when configuration has invalid integer value.
        /// int MinimumPortWidth()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumPortWidthFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_port_width", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumPortWidth method for failure when configuration has non-positive integer value.
        /// integer MinimumPortWidth()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumPortWidthFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_port_width", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumPortHeight method.
        /// int MinimumPortHeight()
        /// </summary>
        [Test]
        public void TestMinimumPortHeight()
        {
            Assert.AreEqual(5, ble.MinimumPortHeight, "Incorrect MinimumPortHeight implementation.");
        }

        /// <summary>
        /// Tests the MinimumPortHeight method for failure when configuration has invalid integer value.
        /// int MinimumPortHeight()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumPortHeightFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_port_height", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumPortHeight method for failure when configuration has non-positive integer value.
        /// integer MinimumPortHeight()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumPortHeightFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_port_height", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumLinkSpace method.
        /// int MinimumLinkSpace()
        /// </summary>
        [Test]
        public void TestMinimumLinkSpace()
        {
            Assert.AreEqual(6, ble.MinimumLinkSpace, "Incorrect MinimumLinkSpace implementation.");
        }

        /// <summary>
        /// Tests the MinimumLinkSpace method for failure when configuration has invalid integer value.
        /// int MinimumLinkSpace()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumLinkSpaceFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_link_space", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumLinkSpace method for failure when configuration has non-positive integer value.
        /// integer MinimumLinkSpace()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumLinkSpaceFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_link_space", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumNodeWidth method.
        /// int MinimumNodeWidth()
        /// </summary>
        [Test]
        public void TestMinimumNodeWidth()
        {
            Assert.AreEqual(8, ble.MinimumNodeWidth, "Incorrect MinimumNodeWidth implementation.");
        }

        /// <summary>
        /// Tests the MinimumNodeWidth method for failure when configuration has invalid integer value.
        /// int MinimumNodeWidth()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumNodeWidthFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_node_width", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumNodeWidth method for failure when configuration has non-positive integer value.
        /// integer MinimumNodeWidth()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumNodeWidthFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_node_width", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumNodeHeight method.
        /// int MinimumNodeHeight()
        /// </summary>
        [Test]
        public void TestMinimumNodeHeight()
        {
            Assert.AreEqual(9, ble.MinimumNodeHeight, "Incorrect MinimumNodeHeight implementation.");
        }

        /// <summary>
        /// Tests the MinimumNodeHeight method for failure when configuration has invalid integer value.
        /// int MinimumNodeHeight()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumNodeHeightFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_node_height", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumNodeHeight method for failure when configuration has non-positive integer value.
        /// integer MinimumNodeHeight()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumNodeHeightFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_node_height", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumSyntheticNodeWidth method.
        /// int MinimumSyntheticNodeWidth()
        /// </summary>
        [Test]
        public void TestMinimumSyntheticNodeWidth()
        {
            Assert.AreEqual(10, ble.MinimumSyntheticNodeWidth, "Incorrect MinimumSyntheticNodeWidth implementation.");
        }

        /// <summary>
        /// Tests the MinimumSyntheticNodeWidth method for failure when configuration has invalid integer value.
        /// int MinimumSyntheticNodeWidth()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumSyntheticNodeWidthFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_synthetic_node_width", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumSyntheticNodeWidth method for failure when configuration has non-positive integer value.
        /// integer MinimumSyntheticNodeWidth()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumSyntheticNodeWidthFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_synthetic_node_width", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumSyntheticNodeHeight method.
        /// int MinimumSyntheticNodeHeight()
        /// </summary>
        [Test]
        public void TestMinimumSyntheticNodeHeight()
        {
            Assert.AreEqual(11, ble.MinimumSyntheticNodeHeight, "Incorrect MinimumSyntheticNodeHeight implementation.");
        }

        /// <summary>
        /// Tests the MinimumSyntheticNodeHeight method for failure when configuration has invalid integer value.
        /// int MinimumSyntheticNodeHeight()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumSyntheticNodeHeightFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_synthetic_node_height", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumSyntheticNodeHeight method for failure when configuration has non-positive integer value.
        /// integer MinimumSyntheticNodeHeight()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumSyntheticNodeHeightFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_synthetic_node_height", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumUnlinkedPortSpace method.
        /// int MinimumUnlinkedPortSpace()
        /// </summary>
        [Test]
        public void TestMinimumUnlinkedPortSpace()
        {
            Assert.AreEqual(7, ble.MinimumUnlinkedPortSpace, "Incorrect MinimumUnlinkedPortSpace implementation.");
        }

        /// <summary>
        /// Tests the MinimumUnlinkedPortSpace method for failure when configuration has invalid integer value.
        /// int MinimumUnlinkedPortSpace()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumUnlinkedPortSpaceFail1()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_unlinked_port_space", "12A");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the MinimumUnlinkedPortSpace method for failure when configuration has non-positive integer value.
        /// integer MinimumUnlinkedPortSpace()
        /// <see cref="SelfDocumentingException"/> is expected with
        /// inner exception as <see cref="ConfigurationAPIException"/>.
        /// </summary>
        [Test]
        public void TestMinimumUnlinkedPortSpaceFail2()
        {
            IConfiguration config = UnitTestHelper.GetTestConfig();
            config.SetSimpleAttribute("minimum_unlinked_port_space", "00");

            try
            {
                ble = new LayoutEngine(config);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ConfigurationAPIException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the GraphLayouter property.
        /// IGraphLayouter GraphLayouter()
        /// </summary>
        [Test]
        public void TestGraphLayouter()
        {
            //Check GraphLayouter
            Assert.IsNotNull(ble.GraphLayouter, "GraphLayouter must be set.");
            Assert.AreEqual("TopCoder.Graph.Layout.Layouter.MockLayouter", ble.GraphLayouter.GetType().FullName,
                "Correct GraphLayouter must be created.");
        }

        /// <summary>
        /// Tests the Logger property.
        /// Logger Logger()
        /// </summary>
        [Test]
        public void TestLogger()
        {
            //Check logger
            Assert.IsNotNull(ble.Logger, "Logger must be set.");
            Assert.AreEqual("TopCoder.LoggingWrapper.DiagnosticImpl", ble.Logger.GetType().FullName,
                "Correct logger must be created.");
        }

        /// <summary>
        /// Tests the Config property.
        /// IConfiguration Config()
        /// </summary>
        [Test]
        public void TestConfig()
        {
            //Check Config
            IConfiguration config = (IConfiguration)UnitTestHelper.GetNonPublicPropertyValue(ble, "Config");
            Assert.IsNotNull(config, "Config must be set.");
            Assert.IsFalse(object.ReferenceEquals(config, configForCtor), "Clone should be created by constructor.");
        }

        /// <summary>
        /// Tests the Layout method.
        /// This just tests that PreProcess and PerformLayout methods are called.
        /// For detailed tests of PreProcess see LayoutEngineTests.
        /// MapData Layout(MapData mapdata)
        /// </summary>
        [Test]
        public void TestLayout()
        {
            MapData ret = ble.Layout(md);

            //Preprocess has been called if new nodes (synthetic) have been added. Original count of nodes was 5
            Assert.AreNotEqual(5, ret.Nodes.Count, "PreProcess was not called.");

            //PerformLayout has been called if a node with id as 7 has been added
            Assert.IsNotNull(ret.GetNodeById(7), "PerformLayout was not called.");

            //Also test the Header
            Assert.AreEqual("layout", ret.Header.Lifecycle, "Header Lifecycle was not properly set.");
        }

        /// <summary>
        /// Tests the Layout method when mapdata is null.
        /// MapData Layout(MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        [Test]
        public void TestLayoutFail1()
        {
            try
            {
                ble.Layout(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the PerformLayout method.
        /// MapData PerformLayout(MapData mapdata)
        /// </summary>
        [Test]
        public void TestPerformLayout()
        {
            blept.TestPerformLayout(md);
        }

        /// <summary>
        /// Tests the PerformLayout method when mapdata is null.
        /// MapData PerformLayout(MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        [Test]
        public void TestPerformLayoutFail1()
        {
            blept.TestPerformLayoutFail1();
        }

        /// <summary>
        /// Tests the PerformLayout method when mapdata is invalid and LayoutException is thrown.
        /// MapData PerformLayout(MapData mapdata)
        /// LayoutException is expected.
        /// </summary>
        [Test]
        public void TestPerformLayoutFail2()
        {
            blept.TestPerformLayoutFail2();
        }

        /// <summary>
        /// Tests the GenerateUniqueId method.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// </summary>
        [Test]
        public void TestGenerateUniqueId()
        {
            blept.TestGenerateUniqueId(md);
        }

        /// <summary>
        /// Tests the GenerateUniqueId method when type is null.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        [Test]
        public void TestGenerateUniqueIdFail1()
        {
            blept.TestGenerateUniqueIdFail1();
        }

        /// <summary>
        /// Tests the GenerateUniqueId method when mapdata is null.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        [Test]
        public void TestGenerateUniqueIdFail2()
        {
            blept.TestGenerateUniqueIdFail2();
        }

        /// <summary>
        /// Tests the GenerateUniqueId method when entityType is not supported.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentException.
        /// </summary>
        [Test]
        public void TestGenerateUniqueIdFail3()
        {
            blept.TestGenerateUniqueIdFail3();
        }
    }

    /// <summary>
    /// A class for testing the protected methods of BaseLayoutEngine.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class BaseLayoutEngineProtectedTests : BaseLayoutEngine
    {
        /// <summary>
        /// A mock constructor for this test class.
        /// </summary>
        public BaseLayoutEngineProtectedTests()
            : base(UnitTestHelper.GetTestConfig())
        {
        }

        /// <summary>
        /// A mock implementation of PreProcess abstract method.
        /// </summary>
        /// <param name="mapdata">MapData instance</param>
        /// <returns>null</returns>
        protected override MapData PreProcess(MapData mapdata)
        {
            return null;
        }

        /// <summary>
        /// A mock implementation of PostProcess abstract method.
        /// </summary>
        /// <param name="mapdata">MapData instance</param>
        /// <returns>null</returns>
        protected override MapData PostProcess(MapData mapdata)
        {
            return null;
        }

        /// <summary>
        /// Tests the PerformLayout method.
        /// MapData PerformLayout(MapData mapdata)
        /// </summary>
        public void TestPerformLayout(MapData md)
        {
            MapData ret = PerformLayout(md);

            //PerformLayout has been called if a node with id as 7 has been added
            Assert.IsNotNull(ret.GetNodeById(7), "PerformLayout was not called.");
        }

        /// <summary>
        /// Tests the PerformLayout method when mapdata is null.
        /// MapData PerformLayout(MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        public void TestPerformLayoutFail1()
        {
            try
            {
                PerformLayout(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the PerformLayout method when mapdata is invalid and LayoutException is thrown.
        /// MapData PerformLayout(MapData mapdata)
        /// LayoutException is expected.
        /// </summary>
        public void TestPerformLayoutFail2()
        {
            try
            {
                //Forces a LayoutException
                MapData md = UnitTestHelper.GetSampleMapData();
                md.Nodes.Clear();
                PerformLayout(md);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(LayoutException), e.GetType(), "Must throw error of type LayoutException.");
            }
        }

        /// <summary>
        /// Tests the GenerateUniqueId method.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// </summary>
        public void TestGenerateUniqueId(MapData md)
        {
            //Since 668 is the largest id for nodes in md, it should return 669
            long id1 = GenerateUniqueId(typeof(MapNode), md);
            Assert.AreEqual(669, id1, "Wrong GenerateUniqueId implementation.");

            //Since 1001 is the largest id for links in md, it should return 1002
            id1 = GenerateUniqueId(typeof(MapLink), md);
            Assert.AreEqual(1002, id1, "Wrong GenerateUniqueId implementation.");

            //Since there are no ports in md, it should return 1
            id1 = GenerateUniqueId(typeof(MapPort), md);
            Assert.AreEqual(1, id1, "Wrong GenerateUniqueId implementation.");
        }

        /// <summary>
        /// Tests the GenerateUniqueId method when type is null.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        public void TestGenerateUniqueIdFail1()
        {
            try
            {
                GenerateUniqueId(null, new MapData());
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the GenerateUniqueId method when mapdata is null.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentNullException.
        /// </summary>
        public void TestGenerateUniqueIdFail2()
        {
            try
            {
                GenerateUniqueId(typeof(MapNode), null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the GenerateUniqueId method when entityType is not supported.
        /// long GenerateUniqueId(Type entityType, MapData mapdata)
        /// <see cref="SelfDocumentingException"/> is expected with inner exception as ArgumentException.
        /// </summary>
        public void TestGenerateUniqueIdFail3()
        {
            try
            {
                GenerateUniqueId(typeof(MapElement), new MapData());
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }
    }
}
