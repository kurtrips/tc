/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using TopCoder.Configuration;
using TopCoder.Util.ConfigurationManager;
using Astraea.Inframap.Data;
using TopCoder.Graph.Layout;
using TopCoder.LoggingWrapper;

namespace Astraea.Inframap.Layout.StressTests
{
    /// <summary>
    /// <para>
    /// Useful common methods for stress tests.
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// This class is thread safe by introducing no state information.
    /// </threadsafety>
    ///
    /// <author>sparemax</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class StressTestsHelper
    {
        /// <summary>
        /// Loads Configuration Manager files.
        /// </summary>
        internal static void LoadConfigMgrFiles()
        {
            ClearConfigMgrFiles();
            ConfigManager.GetInstance().LoadFile("../../test_files/StressTests/Configuration.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/StressTests/ObjectFactoryConfig.xml");
        }

        /// <summary>
        /// Clears Configuration Manager files.
        /// </summary>
        internal static void ClearConfigMgrFiles()
        {
            ConfigManager.GetInstance().Clear(false);
        }

        /// <summary>
        /// <para>
        /// Creates a configuration object used in stress tests.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// the created <see cref="IConfiguration"/> object.
        /// </returns>
        internal static IConfiguration CreateConfig()
        {
            // the 'root'
            IConfiguration config = new DefaultConfiguration("root");

            // attributes
            config.SetSimpleAttribute("default_font_size", "1.0");
            config.SetSimpleAttribute("font_units", "1.0");
            config.SetSimpleAttribute("character_units", "1.0");

            config.SetSimpleAttribute("minimum_port_width", "1");
            config.SetSimpleAttribute("minimum_port_height", "1");
            config.SetSimpleAttribute("minimum_link_space", "1");
            config.SetSimpleAttribute("minimum_unlinked_port_space", "1");
            config.SetSimpleAttribute("minimum_node_width", "1");
            config.SetSimpleAttribute("minimum_node_height", "1");
            config.SetSimpleAttribute("minimum_synthetic_node_width", "1");
            config.SetSimpleAttribute("minimum_synthetic_node_height", "1");

            config.SetSimpleAttribute("graph_layouter_token", "MockLayouter");
            config.SetSimpleAttribute("logger_name", LogManager.DEFAULT_NAMESPACE);

            // define the layouter
            IConfiguration objConfig = new DefaultConfiguration("object_MockLayouter");
            objConfig.SetSimpleAttribute("name", "MockLayouter");
            IConfiguration typeNameConfig = new DefaultConfiguration("type_name");
            typeNameConfig.SetSimpleAttribute("value", typeof(MockGraphLayouter).AssemblyQualifiedName);
            objConfig.AddChild(typeNameConfig);

            config.AddChild(objConfig);

            return config;
        }

        /// <summary>
        /// <para>
        /// Creates a map data used in stress tests.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// the created <see cref="MapData"/> object.
        /// </returns>
        internal static MapData CreateMapData()
        {
            // the map data
            MapData mapdata = new MapData();

            // create nodes
            for (int i = 0; i < 100; i++)
            {
                MapNode node = new MapNode();
                node.Id = i + 1;
                node.Name = "node" + node.Id;

                mapdata.AddNode(node);
            }

            // create links
            MapLink link = new MapLink();
            link.Id = 50;
            link.Name = "link1";
            link.AddAttribute(CreatePathAttribute(1, 1, 2));
            link.AddAttribute(CreatePathAttribute(2, 1, "node50"));

            mapdata.AddLink(link);

            // create ports
            MapPort port = new MapPort();
            port.Id = 1;
            port.Name = "port1";
            port.Node = mapdata.GetNodeById(1);
            port.Node.Ports.Add(port);
            link.AddPort(port);
            mapdata.AddPort(port);

            port = new MapPort();
            port.Id = 50;
            port.Name = "port2";
            port.Node = mapdata.GetNodeById(100);
            port.Node.Ports.Add(port);
            link.AddPort(port);
            mapdata.AddPort(port);

            mapdata.Header = new MapHeader();
            mapdata.Header.Lifecycle = "xxx";

            return mapdata;
        }

        /// <summary>
        /// <para>
        /// Creates a path attribute.
        /// </para>
        /// </summary>
        ///
        /// <param name="id">the attribute id</param>
        /// <param name="ownerId">the attribute owner id</param>
        /// <param name="value">the value of the attribute</param>
        ///
        /// <returns>
        /// a new <see cref="MapAttribute"/> with specified parameters.
        /// </returns>
        private static MapAttribute CreatePathAttribute(int id, int ownerId, int value)
        {
            MapAttribute attribute = new MapAttribute();
            attribute.Id = id;
            attribute.OwnerId = ownerId;
            attribute.OwnerType = "Link";
            attribute.Name = "path";

            attribute.Type = "int";
            attribute.IntValue = value;

            return attribute;
        }

        /// <summary>
        /// <para>
        /// Creates a path attribute.
        /// </para>
        /// </summary>
        ///
        /// <param name="id">the attribute id</param>
        /// <param name="ownerId">the attribute owner id</param>
        /// <param name="value">the value of the attribute</param>
        ///
        /// <returns>
        /// a new <see cref="MapAttribute"/> with specified parameters.
        /// </returns>
        private static MapAttribute CreatePathAttribute(int id, int ownerId, string value)
        {
            MapAttribute attribute = new MapAttribute();
            attribute.Id = id;
            attribute.OwnerId = ownerId;
            attribute.OwnerType = "Link";
            attribute.Name = "path";

            attribute.Type = "string";
            attribute.StringValue = value;

            return attribute;
        }
    }
}
