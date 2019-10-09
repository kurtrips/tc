/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 

using System;
using Astraea.Inframap.Data;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;
using TopCoder.Util.ExceptionManager.SDE;
using NUnit.Framework;

namespace Astraea.Inframap.Layout.Impl
{
    /// <summary>
    /// Unit tests for the LayoutEngine class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class LayoutEngineTests
    {
        /// <summary>
        /// The LayoutEngine to use for the tests.
        /// </summary>
        LayoutEngine le;

        /// <summary>
        /// The IConfiguration instance to use for the LayoutEngine constructor.
        /// </summary>
        IConfiguration config;

        /// <summary>
        /// The LayoutEngineProtectedTests instance to use for testing the protected methods of LayoutEngine.
        /// </summary>
        LayoutEngineProtectedTests lept;

        /// <summary>
        /// The MapData instance to use for the tests.
        /// </summary>
        MapData md;

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            UnitTestHelper.LoadConfigManager();
            config = UnitTestHelper.GetTestConfig();
            le = new LayoutEngine(config);
            lept = new LayoutEngineProtectedTests(config);
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
        /// Tests the type of LayoutEngine.
        /// LayoutEngine(IConfiguration config)
        /// </summary>
        [Test]
        public void TestType()
        {
            Assert.IsTrue(le is BaseLayoutEngine, "Must derive from BaseLayoutEngine.");
        }

        /// <summary>
        /// Tests the constructor. The Logger, Config and GraphLayouter properties must be set correctly.
        /// LayoutEngine(IConfiguration config)
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            //Check logger
            Assert.IsNotNull(le.Logger, "Logger must be set.");
            Assert.AreEqual("TopCoder.LoggingWrapper.DiagnosticImpl", le.Logger.GetType().FullName,
                "Correct logger must be created.");

            //Check GraphLayouter
            Assert.IsNotNull(le.GraphLayouter, "GraphLayouter must be set.");
            Assert.AreEqual("TopCoder.Graph.Layout.Layouter.MockLayouter", le.GraphLayouter.GetType().FullName,
                "Correct GraphLayouter must be created.");

            //Check Config
            IConfiguration conf = (IConfiguration)UnitTestHelper.GetNonPublicPropertyValue(le, "Config");
            Assert.IsNotNull(conf, "Config must be set.");
            Assert.IsFalse(ReferenceEquals(conf, config), "Clone should be created by constructor.");
        }

        /// <summary>
        /// Test that the PreProcess method creates the new links correctly.
        /// MapData PreProcess(MapData mapdata)
        /// </summary>
        [Test]
        public void TestPreProcessNewLinks()
        {
            lept.TestPreProcessNewLinks(md);
        }

        /// <summary>
        /// Test that the PreProcess method creates the new synthetic nodes and ports correctly.
        /// </summary>
        [Test]
        public void TestPreProcessNewSyntheticNodes()
        {
            lept.TestPreProcessNewSyntheticNodes(md);
        }

        /// <summary>
        /// Test that the PreProcess method creates the labels correctly for the non-synthetic nodes.
        /// </summary>
        [Test]
        public void TestPreProcessNonSyntheticNodesLabels()
        {
            lept.TestPreProcessNonSyntheticNodesLabels(md);
        }

        /// <summary>
        /// Test that the PreProcess method sets the correct minimal sizes for all ports.
        /// </summary>
        [Test]
        public void TestPreProcessNonPortSizes()
        {
            lept.TestPreProcessPortSizes(md);
        }

        /// <summary>
        /// Test that the PreProcess method sets the correct minimal sizes for all nodes
        /// </summary>
        [Test]
        public void TestPreProcessNodeSizes()
        {
            lept.TestPreProcessNodeSizes(md);
        }

        /// <summary>
        /// Tests the PostProcess Method. The input MapData must be returned unchanged.
        /// </summary>
        [Test]
        public void TestPostProcess()
        {
            lept.TestPostProcess(md);
        }

        /// <summary>
        /// Test that the PreProcess method when null is passed.
        /// SelfDocumentingException is expected with inner exception as ArgumentNullException.
        /// </summary>
        [Test]
        public void TestPreProcessFail1()
        {
            lept.TestPreProcessFail1();
        }

        /// <summary>
        /// Test that the PostProcess method when null is passed.
        /// SelfDocumentingException is expected with inner exception as ArgumentNullException.
        /// </summary>
        [Test]
        public void TestPostProcessFail1()
        {
            lept.TestPostProcessFail1();
        }
    }

    /// <summary>
    /// Unit tests for protected methods of the LayoutEngine class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    internal sealed class LayoutEngineProtectedTests : LayoutEngine
    {
        /// <summary>
        /// A mock constructor for this class.
        /// </summary>
        /// <param name="config">The configuration instance to use for the class.</param>
        public LayoutEngineProtectedTests(IConfiguration config)
            : base(config)
        {
        }

        /// <summary>
        /// Test that the PreProcess method creates the new links correctly and also deletes the original link.
        /// </summary>
        /// <param name="md">The MapDat inputted to the PreProcess method.</param>
        public void TestPreProcessNewLinks(MapData md)
        {
            MapData ret = PreProcess(md);

            //The original link (LinkA) with id 1001 should not be present.
            //The original link (LinkB) must be present as it had no path attributes
            bool linkBFound = false;
            foreach (ILink link in ret.Links)
            {
                Assert.AreNotEqual(1001, ((MapLink)link).Id, "original link with id 1001 should not be present.");
                if (((MapLink)link).Id == 1000)
                {
                    linkBFound = true;
                }
            }
            Assert.IsTrue(linkBFound, "LinkB must be ignored and should still be present.");


            //Check individual links
            MapLink link1 = (MapLink)ret.Links[1];
            Assert.AreEqual(-1002, link1.Id, "The id of the link should be correct.");
            Assert.AreEqual(234, ((MapNode)link1.Nodes[0]).Id, "The first link must start from NodeB");
            Assert.AreEqual(-669, ((MapNode)link1.Nodes[1]).Id,
                "The first link must end at synthetic node of NodeX");

            MapLink link2 = (MapLink)ret.Links[2];
            Assert.AreEqual(-1003, link2.Id, "The id of the link should be correct.");
            Assert.AreEqual(-669, ((MapNode)link2.Nodes[0]).Id,
                "The second link must start from synthetic node of NodeX");
            Assert.AreEqual(-670, ((MapNode)link2.Nodes[1]).Id,
                "The second link must end at synthetic node of NodeY");

            MapLink link3 = (MapLink)ret.Links[3];
            Assert.AreEqual(-1004, link3.Id, "The id of the link should be correct.");
            Assert.AreEqual(-670, ((MapNode)link3.Nodes[0]).Id,
                "The third link must start from synthetic node of NodeY");
            Assert.AreEqual(-671, ((MapNode)link3.Nodes[1]).Id,
                "The third link must end at synthetic node of NodeZ");

            MapLink link4 = (MapLink)ret.Links[4];
            Assert.AreEqual(-1005, link4.Id, "The id of the link should be correct.");
            Assert.AreEqual(-671, ((MapNode)link4.Nodes[0]).Id,
                "The fourth link must start from synthetic node of NodeY");
            Assert.AreEqual(123, ((MapNode)link4.Nodes[1]).Id,
                "The fourth link must end at nodeA");
        }

        /// <summary>
        /// Test that the PreProcess method creates the new synthetic nodes correctly.
        /// </summary>
        /// <param name="md">The MapDat inputted to the PreProcess method.</param>
        public void TestPreProcessNewSyntheticNodes(MapData md)
        {
            MapData ret = PreProcess(md);

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
        /// Test that the PreProcess method creates the labels correctly for the non-synthetic nodes
        /// </summary>
        /// <param name="md">The MapDat inputted to the PreProcess method.</param>
        public void TestPreProcessNonSyntheticNodesLabels(MapData md)
        {
            MapData ret = PreProcess(md);

            foreach (INode node in md.Nodes)
            {
                MapNode mapDataNode = (MapNode)node;
                if (mapDataNode.Id >= 0)
                {
                    Assert.IsNotNull(mapDataNode.Label, "Label of all non-synthetic nodes must be created.");
                    Assert.AreEqual(mapDataNode.Name, mapDataNode.Label.Text,
                        "Label of non-synthetic node has wrong text.");
                    Assert.AreEqual(new Dimension(
                        (int)(FontUnits * DefaultFontSize), (int)(mapDataNode.Name.Length * CharacterUnits)).Height,
                        mapDataNode.Label.MinimalSize.Height, "Minimal size is incorrect.");
                    Assert.AreEqual(new Dimension(
                        (int)(FontUnits * DefaultFontSize), (int)(mapDataNode.Name.Length * CharacterUnits)).Width,
                        mapDataNode.Label.MinimalSize.Width, "Minimal size is incorrect.");
                }
            }
        }

        /// <summary>
        /// Test that the PreProcess method sets the correct minimal sizes for all ports
        /// </summary>
        /// <param name="md">The MapDat inputted to the PreProcess method.</param>
        public void TestPreProcessPortSizes(MapData md)
        {
            MapData ret = PreProcess(md);

            foreach (IPort port in md.Ports)
            {
                MapPort mapPort = (MapPort)port;
                Assert.AreEqual(mapPort.MinimalSize.Height, MinimumPortHeight, "Incorrect minimal size for port.");
                Assert.AreEqual(mapPort.MinimalSize.Width, MinimumPortWidth, "Incorrect minimal size for port.");
            }
        }

        /// <summary>
        /// Test that the PreProcess method sets the correct minimal sizes for all nodes
        /// </summary>
        /// <param name="md">The MapDat inputted to the PreProcess method.</param>
        public void TestPreProcessNodeSizes(MapData md)
        {
            MapData ret = PreProcess(md);

            foreach (INode node in md.Nodes)
            {
                MapNode mapNode = (MapNode)node;
                if (mapNode.Id < 0)
                {
                    Assert.AreEqual(mapNode.MinimalSize.Height, MinimumSyntheticNodeHeight,
                        "Incorrect minimal size for node.");
                    Assert.AreEqual(mapNode.MinimalSize.Width, MinimumSyntheticNodeWidth,
                        "Incorrect minimal size for node.");
                }
            }

            MapNode node1 = (MapNode)md.GetNodeById(123);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)md.GetNodeById(234);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)md.GetNodeById(666);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)md.GetNodeById(667);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");

            node1 = (MapNode)md.GetNodeById(668);
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
            Assert.AreEqual(19, node1.MinimalSize.Height, "Incorrect minimal size for node.");
        }

        /// <summary>
        /// Test that the PreProcess method when null is passed.
        /// SelfDocumentingException is expected with inner exception as ArgumentNullException.
        /// </summary>
        public void TestPreProcessFail1()
        {
            try
            {
                PreProcess(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }

        /// <summary>
        /// Tests the PostProcess Method. The input MapData must be returned unchanged.
        /// </summary>
        /// <param name="md">The input MapData for the method call.</param>
        public void TestPostProcess(MapData md)
        {
            MapData ret = PostProcess(md);

            //Everything must be unchanged. All links, ports and nodes must be the same
            Assert.AreEqual(md, ret, "The reference of the MapData must remain same.");
            Assert.AreEqual(md.Ports, ret.Ports, "The reference must remain same.");
            Assert.AreEqual(md.Ports.Count, ret.Ports.Count, "The port count must remain same.");
            Assert.AreEqual(md.Nodes, ret.Nodes, "The reference must remain same.");
            Assert.AreEqual(md.Nodes.Count, ret.Nodes.Count, "The node count must remain same.");
            Assert.AreEqual(md.Links, ret.Links, "The reference must remain same.");
            Assert.AreEqual(md.Links.Count, ret.Links.Count, "The links count must remain same.");
            Assert.AreEqual(md.Header, ret.Header, "The Header must remain same.");
        }

        /// <summary>
        /// Test that the PostProcess method when null is passed.
        /// SelfDocumentingException is expected with inner exception as ArgumentNullException.
        /// </summary>
        public void TestPostProcessFail1()
        {
            try
            {
                PostProcess(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(SelfDocumentingException), e.GetType(), "Wrong type of exception.");
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType(),
                    "Wrong type of inner exception.");
            }
        }
    }
}
