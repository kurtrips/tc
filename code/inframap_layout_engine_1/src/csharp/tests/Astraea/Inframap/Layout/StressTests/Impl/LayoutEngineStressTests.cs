/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using NUnit.Framework;
using System;
using TopCoder.Configuration;
using Astraea.Inframap.Data;
using Astraea.Inframap.Layout.Impl;
using TopCoder.Graph.Layout;
using System.Collections.Generic;

namespace Astraea.Inframap.Layout.StressTests.Impl
{
    /// <summary>
    /// <para>
    /// Stress tests for the <see cref="LayoutEngine"/> class.
    /// </para>
    /// </summary>
    ///
    /// <author>sparemax</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class LayoutEngineStressTests
    {
        /// <summary>
        /// Setup
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            StressTestsHelper.LoadConfigMgrFiles();
        }

        /// <summary>
        /// TearDown
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            StressTestsHelper.ClearConfigMgrFiles();
        }

        /// <summary>
        /// The times that tests will be ran.
        /// </summary>
        private const int Times = 10;

        /// <summary>
        /// <para>
        /// Stress test for the constructor <c>LayoutEngine(IConfiguration config)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void TestStressCtor()
        {
            long start = Environment.TickCount;
            for (int i = 1; i <= Times; ++i)
            {
                LayoutEngine engine = new LayoutEngine(StressTestsHelper.CreateConfig());

                // check results
                Assert.AreEqual(1, (int)engine.DefaultFontSize, "DefaultFontSize should be correct.");
                Assert.AreEqual(1, (int)engine.FontUnits, "FontUnits should be correct.");
                Assert.AreEqual(1, (int)engine.CharacterUnits, "CharacterUnits should be correct.");

                Assert.AreEqual(1, engine.MinimumPortWidth, "MinimumPortWidth should be correct.");
                Assert.AreEqual(1, engine.MinimumPortHeight, "MinimumPortHeight should be correct.");
                Assert.AreEqual(1, engine.MinimumLinkSpace, "MinimumLinkSpace should be correct.");
                Assert.AreEqual(1, engine.MinimumUnlinkedPortSpace, "MinimumUnlinkedPortSpace should be correct.");
                Assert.AreEqual(1, engine.MinimumNodeWidth, "MinimumNodeWidth should be correct.");
                Assert.AreEqual(1, engine.MinimumNodeHeight, "MinimumNodeHeight should be correct.");
                Assert.AreEqual(1, engine.MinimumSyntheticNodeWidth, "MinimumSyntheticNodeWidth should be correct.");
                Assert.AreEqual(1, engine.MinimumSyntheticNodeHeight, "FontUnits should be correct.");
            }

            Console.WriteLine("Total time : " + (Environment.TickCount - start) + "ms.");
        }

        /// <summary>
        /// <para>
        /// Stress test for the constructor <c>LayoutEngine(IConfiguration config)</c>.
        /// </para>
        /// </summary>
        [Test]
        public void TestStressLayoutEngine()
        {
            LayoutEngine engine = new LayoutEngine(StressTestsHelper.CreateConfig());

            long start = Environment.TickCount;
            for (int i = 1; i <= Times; ++i)
            {
                MapData md = engine.Layout(StressTestsHelper.CreateMapData());

                IList<int> ids = new List<int>();

                // check results
                Assert.AreEqual(3, md.Links.Count, "Layout should be correct.");
                ids.Add((int)md.Links[0].Id);
                ids.Add((int)md.Links[1].Id);
                ids.Add((int)md.Links[2].Id);

                Assert.IsTrue(ids.Contains(-51), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(-52), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(-53), "Layout should be correct.");

                ids.Clear();
                ILink link = md.Links[0];
                Assert.AreEqual(2, link.Nodes.Count, "Layout should be correct.");
                ids.Add((int)link.Nodes[0].Id);
                ids.Add((int)link.Nodes[1].Id);
                Assert.IsTrue(ids.Contains(1), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(-101), "Layout should be correct.");
                ids.Clear();
                Assert.AreEqual(2, link.Nodes.Count, "Layout should be correct.");
                ids.Add((int)link.Ports[0].Id);
                ids.Add((int)link.Ports[1].Id);
                Assert.IsTrue(ids.Contains(1), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(-51), "Layout should be correct.");

                ids.Clear();
                link = md.Links[1];
                Assert.AreEqual(2, link.Nodes.Count, "Layout should be correct.");
                ids.Add((int)link.Nodes[0].Id);
                ids.Add((int)link.Nodes[1].Id);
                Assert.IsTrue(ids.Contains(-101), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(-102), "Layout should be correct.");
                ids.Clear();
                Assert.AreEqual(2, link.Nodes.Count, "Layout should be correct.");
                ids.Add((int)link.Ports[0].Id);
                ids.Add((int)link.Ports[1].Id);
                Assert.IsTrue(ids.Contains(-51), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(-52), "Layout should be correct.");

                ids.Clear();
                link = md.Links[2];
                Assert.AreEqual(2, link.Nodes.Count, "Layout should be correct.");
                ids.Add((int)link.Nodes[0].Id);
                ids.Add((int)link.Nodes[1].Id);
                Assert.IsTrue(ids.Contains(-102), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(100), "Layout should be correct.");
                ids.Clear();
                Assert.AreEqual(2, link.Nodes.Count, "Layout should be correct.");
                ids.Add((int)link.Ports[0].Id);
                ids.Add((int)link.Ports[1].Id);
                Assert.IsTrue(ids.Contains(-52), "Layout should be correct.");
                Assert.IsTrue(ids.Contains(50), "Layout should be correct.");

                Assert.AreEqual("layout", md.Header.Lifecycle, "Layout should be correct.");
            }

            Console.WriteLine("Total time : " + (Environment.TickCount - start) + "ms.");
        }
    }
}
