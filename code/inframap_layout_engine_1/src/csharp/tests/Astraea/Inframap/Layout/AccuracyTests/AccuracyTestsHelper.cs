/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */


using Astraea.Inframap.Data;
using Astraea.Inframap.Layout.Impl;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;
using TopCoder.Util.ConfigurationManager;
using System.Reflection;
namespace Astraea.Inframap.Layout.AccuracyTests
{
    /// <summary>
    /// Defines helper methods used for accuracy tests.
    /// </summary>
    ///
    /// <threadsafety>
    /// All static methods are thread safe.
    /// </threadsafety>
    ///
    /// <author>jueyey</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class AccuracyTestsHelper
    {
        /// <summary>
        /// <para>
        /// Loads configuration.
        /// </para>
        /// </summary>
        internal static void LoadConfig()
        {
            ClearConfig();

            ConfigManager cm = ConfigManager.GetInstance();
            cm.LoadFile("../../test_files/AccuracyTests/logger.xml");
            cm.LoadFile("../../test_files/AccuracyTests/ObjectFactoryConfig.xml");
            
        }

      

        /// <summary>
        /// <para>
        /// Clears configuration.
        /// </para>
        /// </summary>
        internal static void ClearConfig()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// Gets the IConfiguration instance to use for the tests.
        /// </summary>
        /// <returns>The IConfiguration instance to use for the tests.</returns>
        internal static IConfiguration CreateConfiguration()
        {
            IConfiguration config = new DefaultConfiguration("TestConfig");
            config.SetSimpleAttribute("default_font_size", "1.414");
            config.SetSimpleAttribute("font_units", "2.456");
            config.SetSimpleAttribute("character_units", "3.321");
            config.SetSimpleAttribute("minimum_port_width", "4");
            config.SetSimpleAttribute("minimum_port_height", "5");
            config.SetSimpleAttribute("minimum_link_space", "6");
            config.SetSimpleAttribute("minimum_unlinked_port_space", "7");
            config.SetSimpleAttribute("minimum_node_width", "8");
            config.SetSimpleAttribute("minimum_node_height", "9");
            config.SetSimpleAttribute("minimum_synthetic_node_width", "10");
            config.SetSimpleAttribute("minimum_synthetic_node_height", "11");
            config.SetSimpleAttribute("graph_layouter_token", "TestGraphLayouterToken");
            config.SetSimpleAttribute("object_factory_ns", "TestOFNamespace");
            config.SetSimpleAttribute("logger_name", "TestLoggerNamespace");

            config.AddChild(GetObjectFactoryConfiguration());

            return config;
        }

        /// <summary>
        /// Gets the value of a non-public property of a class via reflection.
        /// </summary>
        /// <param name="obj">The object for which to get the value of the property for.</param>
        /// <param name="name">The name of the property</param>
        /// <returns>The value of the non-public property</returns>
        internal static object GetNonPublicPropertyValue(object obj, string name)
        {
            PropertyInfo prop = obj.GetType().GetProperty(name,
                BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic);
            return prop.GetValue(obj, null);
        }

        /// <summary>
        /// Create the map data for testing.
        /// </summary>
        /// <returns>the created map data</returns>
        internal static MapData CreateMapData()
        {
            return UnitTestHelper.GetSampleMapData();
        }


        /// <summary>
        /// <para>Creates a valid <see cref="IConfiguration"/> child for the
        /// configuration object to be used with the
        /// <see cref="LayoutEngine"/> ctor. The returned child
        /// contains the object definition for the LayoutEngine's graphLayouter
        /// member.</para>
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetObjectFactoryConfiguration()
        {
            IConfiguration config = new DefaultConfiguration(
                "TestOFNamespace");
            IConfiguration object1 = new DefaultConfiguration("object1");

            object1.SetSimpleAttribute("name", "GraphLayouter");
            IConfiguration typeNameConfig = new DefaultConfiguration("type_name");
            typeNameConfig.SetSimpleAttribute("value",
                typeof(IGraphLayouter).AssemblyQualifiedName);
            object1.AddChild(typeNameConfig);

            config.AddChild(object1);
            return config;
        }

    }
}
