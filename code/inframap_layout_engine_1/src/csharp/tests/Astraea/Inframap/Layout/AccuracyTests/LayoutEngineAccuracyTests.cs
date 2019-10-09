/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Astraea.Inframap.Data;
using Astraea.Inframap.Layout.Impl;
using NUnit.Framework;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Layout.AccuracyTests
{
    /// <summary>
    /// Accuracy tests for <see cref=" LayoutEngine"/> class.
    /// </summary>
    ///
    ///
    /// <author>jueyey</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class LayoutEngineAccuracyTests
    {
        /// <summary>
        /// Represents the <see cref=" LayoutEngine"/> for testing.
        /// </summary>
        private MockLayoutEngine instance;

        /// <summary>
        /// Represents the <see cref="IConfiguration"/> for testing.
        /// </summary>
        private IConfiguration config;

        /// <summary>
        /// Represents the <see cref="MapData"/> instance for testing.
        /// </summary>
        private MapData mapData;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            AccuracyTestsHelper.LoadConfig();
            config = AccuracyTestsHelper.CreateConfiguration();
            // create the instance
            instance = new MockLayoutEngine(config);
            // create map data
            mapData = AccuracyTestsHelper.CreateMapData();
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            AccuracyTestsHelper.ClearConfig();
        }

        /// <summary>
        /// Tests the constructor LayoutEngine(IConfiguration config) for accuracy.
        /// The Logger, Config and GraphLayouter properties must be set correctly.
        /// 
        /// </summary>
        [Test]
        public void TestConstructor_Accuracy()
        {
            Assert.IsNotNull(instance, "LayoutEngine instance should be properly created.");
            // verify
            Assert.IsNotNull(instance.Logger, "Logger must be properly set.");
            
            //Check GraphLayouter
            Assert.IsNotNull(instance.GraphLayouter, "GraphLayouter must be set.");
            
            //Check Config
            IConfiguration conf = (IConfiguration)AccuracyTestsHelper.GetNonPublicPropertyValue(instance, "Config");
            Assert.IsNotNull(conf, "Config must be set.");
            Assert.IsFalse(ReferenceEquals(conf, config), "Clone should be created by constructor.");
        }

        /// <summary>
        /// Tests the <c>GenerateUniqueId</c> method for accuracy.
        /// UniqueId should be generated.
        /// </summary>
        [Test]
        public void TestGenerateUniqueId()
        {
            // MapNode 
            for (int i = 0; i < 10; i++)
            {
                long id1 = instance.MockGenerateUniqueId(typeof (MapNode), mapData);
                Assert.AreEqual(669, id1, "Wrong GenerateUniqueId implementation.");
            }

            
            // since 1001 is the largest id for links in md
            // MapNode 
            for (int i = 0; i < 10; i++)
            {
                long id1 = instance.MockGenerateUniqueId(typeof(MapLink), mapData);
                Assert.AreEqual(1002, id1, "Wrong GenerateUniqueId implementation.");
            }
           

            // Since there are no ports in md, it should return 1
            // MapNode 
            for (int i = 0; i < 10; i++)
            {
                long id1 = instance.MockGenerateUniqueId(typeof(MapPort), mapData);
                Assert.AreEqual(1, id1, "Wrong GenerateUniqueId implementation.");
            }
         
            
        }

        /// <summary>
        /// Test that the <c>PostProcess</c> method for accuracy.
        /// The mapData should be properly processed.
        /// </summary>
        [Test]
        public void TestPostProcess_Accuracy()
        {
            MapData ret = instance.MockPostProcess(mapData);
            // verify
            Assert.AreEqual(mapData, ret,"The mapData should be properly processed.");

            Assert.AreEqual(mapData, ret, "The reference of the MapData must remain same.");
            Assert.AreEqual(mapData.Ports, ret.Ports, "The reference must remain same.");
            Assert.AreEqual(mapData.Ports.Count, ret.Ports.Count, "The port count must remain same.");
            Assert.AreEqual(mapData.Nodes, ret.Nodes, "The reference must remain same.");
            Assert.AreEqual(mapData.Nodes.Count, ret.Nodes.Count, "The node count must remain same.");
            Assert.AreEqual(mapData.Links, ret.Links, "The reference must remain same.");
            Assert.AreEqual(mapData.Links.Count, ret.Links.Count, "The links count must remain same.");
            Assert.AreEqual(mapData.Header, ret.Header, "The Header must remain same.");
        }

        /// <summary>
        /// Test that the <c>PreProcess</c> method for accuracy. The new links should be 
        /// correctly created and the original link should be deleted.
        /// </summary>
        [Test]
        public void TestPreProcess_NewLinks_Accuracy()
        {
            MapData ret = instance.MockPreProcess(mapData);

            // The original link (LinkA) with id 1001 should not be present.
            Assert.IsNull(ret.GetLinkById(1001), "The link should be removed.");

            // The original link (LinkB) must be present as it had no path attributes
            Assert.IsNotNull(ret.GetLinkById(1000), "LinkB must be ignored and should still be present.");


            // check individual links
            MapLink link1 = (MapLink)ret.Links[1];
            Assert.AreEqual(-1002, link1.Id, "The id of the link should be correct.");
            Assert.AreEqual(234, ((MapNode)link1.Nodes[0]).Id, "The first link must start from NodeB");
            Assert.AreEqual(-669, ((MapNode)link1.Nodes[1]).Id,
                "The first link must end at synthetic node of NodeX");

            Assert.AreEqual(4, link1.Attributes.Count, "The original link attributes should be copied.");
          

            MapLink link2 = (MapLink)ret.Links[2];
            Assert.AreEqual(-1003, link2.Id, "The id of the link should be correct.");
            Assert.AreEqual(-669, ((MapNode)link2.Nodes[0]).Id,
                "The second link must start from synthetic node of NodeX");
            Assert.AreEqual(-670, ((MapNode)link2.Nodes[1]).Id,
                "The second link must end at synthetic node of NodeY");

            Assert.AreEqual(4, link2.Attributes.Count, "The original link attributes should be copied.");
           

            MapLink link3 = (MapLink)ret.Links[3];
            Assert.AreEqual(-1004, link3.Id, "The id of the link should be correct.");
            Assert.AreEqual(-670, ((MapNode)link3.Nodes[0]).Id,
                "The third link must start from synthetic node of NodeY");
            Assert.AreEqual(-671, ((MapNode)link3.Nodes[1]).Id,
                "The third link must end at synthetic node of NodeZ");

            Assert.AreEqual(4, link3.Attributes.Count, "The original link attributes should be copied.");

            MapLink link4 = (MapLink)ret.Links[4];
            Assert.AreEqual(-1005, link4.Id, "The id of the link should be correct.");
            Assert.AreEqual(-671, ((MapNode)link4.Nodes[0]).Id,
                "The fourth link must start from synthetic node of NodeY");
            Assert.AreEqual(123, ((MapNode)link4.Nodes[1]).Id,
                "The fourth link must end at nodeA");

            Assert.AreEqual(4, link3.Attributes.Count, "The original link attributes should be copied.");
        }

        /// <summary>
        /// Test that the <c>PreProcess</c> method for accuracy, the new synthetic nodes should be correctly
        /// created.
        /// </summary>
        [Test]
        public void TestPreProcess_SyntheticNodes_Accuracy()
        {
            MapData ret = instance.MockPreProcess(mapData);

            //Node X
            MapNode nodeX = (MapNode)ret.GetNodeById(666);

            //The synthetic node for nodeX
            MapNode synX = (MapNode)ret.GetNodeById(-669);
            Assert.IsNotNull(synX, "Synthetic node for nodeX must be created.");
            Assert.AreEqual(nodeX, synX.Container, "The synthetic node must be child of the nodeX.");

            //Port for nodeX
            MapPort portX = (MapPort)ret.GetPortById(-1);
            Assert.IsNotNull(portX, "The new port created must be added to mapdata.");
            Assert.AreEqual(synX, portX.Node, "The synthetic node for X must be the node of portX");
            Assert.AreEqual(portX, synX.Ports[0], "The new portX must be added to synthetic node X's ports.");


            //Node Y
            MapNode nodeY = (MapNode)ret.GetNodeById(667);

            //The synthetic node for nodeY
            MapNode synY = (MapNode)ret.GetNodeById(-670);
            Assert.IsNotNull(synY, "Synthetic node for nodeY must be created.");
            Assert.AreEqual(nodeY, synY.Container, "The synthetic node must be child of the nodeY.");

            //Port for nodeY
            MapPort portY = (MapPort)ret.GetPortById(-2);
            Assert.IsNotNull(portY, "The new port created must be added to mapdata.");
            Assert.AreEqual(synY, portY.Node, "The synthetic node for Y must be the node of portY");
            Assert.AreEqual(portY, synY.Ports[0], "The new portY must be added to synthetic node Y's ports.");


            //Node Z
            MapNode nodeZ = (MapNode)ret.GetNodeById(668);

            //The synthetic node for nodeZ
            MapNode synZ = (MapNode)ret.GetNodeById(-671);
            Assert.IsNotNull(synZ, "Synthetic node for nodeZ must be created.");
            Assert.AreEqual(nodeZ, synZ.Container, "The synthetic node must be child of the nodeZ.");

            //Port for nodeZ
            MapPort portZ = (MapPort)ret.GetPortById(-3);
            Assert.IsNotNull(portZ, "The new port created must be added to mapdata.");
            Assert.AreEqual(synZ, portZ.Node, "The synthetic node for Z must be the node of portZ");
            Assert.AreEqual(portZ, synZ.Ports[0], "The new portZ must be added to synthetic node Z's ports.");
        }

        /// <summary>
        /// Test that the <c>PreProcess</c> method for accuracy , the labels correctly for the non-synthetic nodes
        /// should be created properly.
        /// </summary>
        [Test]
        public void TestPreProcess_NonSyntheticNodesLabels_Accuracy()
        {
            MapData ret = instance.MockPreProcess(mapData);

            foreach (INode node in ret.Nodes)
            {
                MapNode mapDataNode = (MapNode)node;
                if (mapDataNode.Id >= 0)
                {
                    Assert.IsNotNull(mapDataNode.Label, "Label of all non-synthetic nodes must be created.");
                    Assert.AreEqual(mapDataNode.Name, mapDataNode.Label.Text,
                        "Label of non-synthetic node has wrong text.");
                    Assert.AreEqual(new Dimension(
                        (int)(instance.FontUnits * instance.DefaultFontSize),
                        (int)(mapDataNode.Name.Length * instance.CharacterUnits)).Height,
                        mapDataNode.Label.MinimalSize.Height, "Minimal size is incorrect.");
                    Assert.AreEqual(new Dimension(
                        (int)(instance.FontUnits * instance.DefaultFontSize), 
                        (int)(mapDataNode.Name.Length * instance.CharacterUnits)).Width,
                        mapDataNode.Label.MinimalSize.Width, "Minimal size is incorrect.");
                }
            }
        }

        /// <summary>
        /// Test that the <c>PreProcess</c> method for accuracy, the minimal sizes for all ports
        /// should be properly set.
        /// </summary>
        [Test]
        public void TestPreProcess_PortSizes_Accuracy()
        {
            MapData ret = instance.MockPreProcess(mapData);


            foreach (IPort port in ret.Ports)
            {
                MapPort mapPort = (MapPort)port;
                Assert.AreEqual(mapPort.MinimalSize.Height, instance.MinimumPortHeight,
                    "Incorrect minimal size for port.");
                Assert.AreEqual(mapPort.MinimalSize.Width, instance.MinimumPortWidth, 
                    "Incorrect minimal size for port.");
            }
        }

        /// <summary>
        /// Test that the PreProcess method sets the correct minimal sizes for all nodes
        /// </summary>
        [Test]
        public void TestPreProcess_NodeSizes_Accuracy()
        {
            MapData ret = instance.MockPreProcess(mapData);

            foreach (INode node in ret.Nodes)
            {
                MapNode mapNode = (MapNode)node;
                if (mapNode.Id < 0)
                {
                    Assert.AreEqual(mapNode.MinimalSize.Height, instance.MinimumSyntheticNodeHeight,
                        "Incorrect minimal size for node.");
                    Assert.AreEqual(mapNode.MinimalSize.Width, instance.MinimumSyntheticNodeWidth,
                        "Incorrect minimal size for node.");
                }
            }

            MapNode node1 = (MapNode)ret.GetNodeById(123);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)ret.GetNodeById(234);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)ret.GetNodeById(666);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)ret.GetNodeById(667);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)ret.GetNodeById(668);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
        }
    }
}
