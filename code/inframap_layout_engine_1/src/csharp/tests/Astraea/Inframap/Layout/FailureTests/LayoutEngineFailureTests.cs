using System;
using Astraea.Inframap.Data;
using TopCoder.Configuration;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace Astraea.Inframap.Layout.FailureTests
{
    /// <summary>
    /// Failure tests of the LayoutEngine class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class LayoutEngineFailureTests
    {
        /// <summary>
        /// The LayoutEngine to use for the tests.
        /// </summary>
        LayoutEngineTester engine;

        /// <summary>
        /// The IConfiguration instance to use.
        /// </summary>
        IConfiguration config;


        /// <summary>
        /// The ConfigManager used in the test.
        /// </summary>
        ConfigManager cm = ConfigManager.GetInstance();

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            cm.Clear();
            cm.LoadFile("../../test_files/failure/failure.xml");

            config = GetConfig();
            engine = new LayoutEngineTester(config);
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            cm.Clear();
        }


        /// <summary>
        /// Failure test of the LayoutEngine class.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLayoutEngineConstructorA()
        {
            new LayoutEngineTester(null);
        }

        /// <summary>
        /// Failure test of the LayoutEngine class.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLayoutEngineConstructorD()
        {
            config.SetSimpleAttribute("logger_name", "hello");
            new LayoutEngineTester(config);
        }

        /// <summary>
        /// Failure test of the LayoutEngine class.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLayoutEngineConstructorB()
        {
            config.SetSimpleAttribute("object_factory_ns", "hello");
            new LayoutEngineTester(config);
        }

        /// <summary>
        /// Failure test of the LayoutEngine class.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLayoutEngineConstructorC()
        {
            config.SetSimpleAttribute("graph_layouter_token", "hello");
            new LayoutEngineTester(config);
        }

        /// <summary>
        /// Failure test of the PerformLayout method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestPerformLayoutA()
        {
            engine.PerformLayout(null);
        }



        /// <summary>
        /// Failure test of the GenerateUniqueId method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestGenerateUniqueIdA()
        {
            engine.GenerateUniqueId(null, new MapData());
        }

        /// <summary>
        /// Failure test of the GenerateUniqueId method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestGenerateUniqueIdB()
        {
            engine.GenerateUniqueId(typeof(MapNode), null);
        }

        /// <summary>
        /// Failure test of the GenerateUniqueId method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestGenerateUniqueIdC()
        {
            engine.GenerateUniqueId(typeof(MapElement), new MapData());
        }

        /// <summary>
        /// Failure test of the Layout method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestLayoutA()
        {
            engine.Layout(null);
        }

        /// <summary>
        /// Failure test of the PostProcess method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestPostProcessA()
        {
            engine.PostProcess(null);
        }

        /// <summary>
        /// Failure test of the PreProcess method.
        /// </summary>
        [Test]
        [ExpectedException(typeof(SelfDocumentingException))]
        public void TestPreProcessA()
        {
            engine.PreProcess(null);
        }

        /// <summary>
        /// Gets the IConfiguration instance to use for the tests.
        /// </summary>
        /// <returns>The IConfiguration instance.</returns>
        private static IConfiguration GetConfig()
        {
            IConfiguration config = new DefaultConfiguration("default");
            config.SetSimpleAttribute("default_font_size", "2.145");
            config.SetSimpleAttribute("font_units", "2.356");
            config.SetSimpleAttribute("character_units", "5.56");
            config.SetSimpleAttribute("minimum_port_width", "4");
            config.SetSimpleAttribute("minimum_port_height", "4");
            config.SetSimpleAttribute("minimum_link_space", "4");
            config.SetSimpleAttribute("minimum_unlinked_port_space", "13");
            config.SetSimpleAttribute("minimum_node_width", "9");
            config.SetSimpleAttribute("minimum_node_height", "9");
            config.SetSimpleAttribute("minimum_synthetic_node_width", "120");
            config.SetSimpleAttribute("minimum_synthetic_node_height", "161");
            config.SetSimpleAttribute("graph_layouter_token", "Layouter");
            config.SetSimpleAttribute("object_factory_ns", "TopCoder.Util.ObjectFactory");

            return config;
        }

    }
}
