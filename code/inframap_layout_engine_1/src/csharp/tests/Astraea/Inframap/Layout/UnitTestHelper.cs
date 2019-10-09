/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 
using System.Reflection;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;
using TopCoder.Util.ConfigurationManager;
using Astraea.Inframap.Data;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// Helper class for the unit tests.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal static class UnitTestHelper
    {
        /// <summary>
        /// Gets the IConfiguration instance to use for the tests.
        /// </summary>
        /// <returns>The IConfiguration instance to use for the tests.</returns>
        internal static IConfiguration GetTestConfig()
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

            return config;
        }

        /// <summary>
        /// Loads the files to be used by ConfigManager.
        /// </summary>
        internal static void LoadConfigManager()
        {
            ConfigManager.GetInstance().LoadFile("../../test_files/logger.xml");
            ConfigManager.GetInstance().LoadFile("../../test_files/ObjectFactoryConfig.xml");
        }

        /// <summary>
        /// Clears the ConfigManager instance.
        /// </summary>
        internal static void ClearConfigManager()
        {
            ConfigManager.GetInstance().Clear(false);
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
        /// Creates a sample Map Data for the tests.
        /// </summary>
        /// <returns>A sample MapData instance</returns>
        internal static MapData GetSampleMapData()
        {
            MapData md = new MapData();

            //Create some nodes and add them to the mapdata
            MapNode nodeA = new MapNode();
            SetMockIdAndName(nodeA, 123, "nodeA");
            AddMockPort(nodeA, null);

            MapNode nodeB = new MapNode();
            SetMockIdAndName(nodeB, 234, "nodeB");
            AddMockPort(nodeB, null);

            MapNode nodeC = new MapNode();
            SetMockIdAndName(nodeC, 012, "nodeC");

            MapNode nodeX = new MapNode();
            SetMockIdAndName(nodeX, 666, "nodeX");
            AddMockPort(nodeX, null);

            MapNode nodeY = new MapNode();
            SetMockIdAndName(nodeY, 667, "nodeY");
            AddMockPort(nodeY, null);

            MapNode nodeZ = new MapNode();
            SetMockIdAndName(nodeZ, 668, "nodeZ");
            AddMockPort(nodeZ, null);

            md.Nodes.Add(nodeA);
            md.Nodes.Add(nodeB);
            md.Nodes.Add(nodeX);
            md.Nodes.Add(nodeY);
            md.Nodes.Add(nodeZ);

            //Create a link (from A to B) and add it to the mapData
            MapLink linkA = new MapLink();
            SetMockIdAndName(linkA, 1001, "linkA");
            linkA.AddNode(nodeA);
            linkA.AddNode(nodeB);
            //Note ports do not have to be in the same order as the links
            linkA.AddPort((MapPort)nodeB.Ports[0]);
            linkA.AddPort((MapPort)nodeA.Ports[0]);

            //Create a link (from B to C) and add it to the mapData
            MapLink linkB = new MapLink();
            SetMockIdAndName(linkB, 1000, "linkB");
            linkA.AddNode(nodeB);
            linkA.AddNode(nodeC);

            md.Links.Add(linkA);
            md.Links.Add(linkB);

            //Create a linked port
            nodeB.Ports[0].Links.Add(linkA);

            //Set header
            md.Header = new MapHeader();

            //Create linkedport for NodeC
            AddMockPort(nodeC, linkB);


            //path with 3 nodes for LinkA
            MapAttribute ma1 = new MapAttribute();
            ma1.IntValue = 666;
            ma1.Type = "int";
            ma1.Name = "path";
            MapAttribute ma2 = new MapAttribute();
            ma2.IntValue = 667;
            ma2.Type = "int";
            ma2.Name = "path";
            MapAttribute ma3 = new MapAttribute();
            ma3.StringValue = "nodeZ";
            ma3.Type = "string";
            ma3.Name = "path";
            linkA.AddAttribute(ma1);
            linkA.AddAttribute(ma2);
            linkA.AddAttribute(ma3);

            return md;
        }

        /// <summary>
        /// Sets the id and name of a MapElement
        /// </summary>
        /// <param name="elem">The MapElement for which to set properties.</param>
        /// <param name="id">The id to set</param>
        /// <param name="name">The name to set</param>
        private static void SetMockIdAndName(MapElement elem, long id, string name)
        {
            elem.Id = id;
            elem.Name = name;
        }

        /// <summary>
        /// Creates and adds a port to a node. Also sets the Node property of the port.
        /// </summary>
        /// <param name="node">The node to process.</param>
        /// <param name="link">Link, if linked port is required</param>
        private static void AddMockPort(INode node, ILink link)
        {
            MapPort mapPort = new MapPort();
            MapNode mapNode = (MapNode)node;
            mapPort.Id = mapNode.Id;
            mapPort.Name = mapNode.Name;

            //Add port to node
            mapNode.AddPort(mapPort);

            //Add node to port
            mapPort.Node = mapNode;

            //Linked port is required
            if (link != null)
            {
                mapPort.Links.Add(link);
            }
        }
    }
}
