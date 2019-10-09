/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using NUnit.Framework;
using System;
using TopCoder.Configuration;
using Astraea.Inframap.Data;

namespace Astraea.Inframap.Layout.StressTests
{
    /// <summary>
    /// <para>
    /// Stress tests for the <see cref="BaseLayoutEngine"/> class.
    /// Uses the <see cref="MockBaseLayoutEngine"/> class to test.
    /// </para>
    /// </summary>
    ///
    /// <author>sparemax</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class BaseLayoutEngineStressTests
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
        private const int Times = 1000;

        /// <summary>
        /// <para>
        /// Stress test for the constructor <c>BaseLayoutEngine(IConfiguration config)</c>.
        /// Uses <c>MockBaseLayoutEngine(IConfiguration config)</c> to test.
        /// </para>
        /// </summary>
        [Test]
        public void TestStressCtor()
        {
            long start = Environment.TickCount;
            for (int i = 1; i <= Times; ++i)
            {
                BaseLayoutEngine engine = new MockBaseLayoutEngine(StressTestsHelper.CreateConfig());

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
        /// Stress test for the method <c>GenerateUniqueId(Type entityType, MapData mapdata)</c>.
        /// Uses <c>MockBaseLayoutEngine(IConfiguration config)</c> to test.
        /// </para>
        /// </summary>
        [Test]
        public void TestStressGenerateUniqueId1()
        {
            long start = Environment.TickCount;
            for (int i = 1; i <= Times; ++i)
            {
                MockBaseLayoutEngine engine = new MockBaseLayoutEngine(StressTestsHelper.CreateConfig());

                // check results
                Assert.AreEqual(101, engine.GenerateUniqueId(typeof(MapNode), StressTestsHelper.CreateMapData()),
                    "GenerateUniqueId should be correct.");
                Assert.AreEqual(101, engine.GenerateUniqueId(typeof(MapNode), StressTestsHelper.CreateMapData()),
                    "GenerateUniqueId should be correct.");
            }

            Console.WriteLine("Total time : " + (Environment.TickCount - start) + "ms.");
        }

        /// <summary>
        /// <para>
        /// Stress test for the method <c>GenerateUniqueId(Type entityType, MapData mapdata)</c>.
        /// Uses <c>MockBaseLayoutEngine(IConfiguration config)</c> to test.
        /// </para>
        /// </summary>
        [Test]
        public void TestStressGenerateUniqueId2()
        {
            long start = Environment.TickCount;
            for (int i = 1; i <= Times; ++i)
            {
                MockBaseLayoutEngine engine = new MockBaseLayoutEngine(StressTestsHelper.CreateConfig());

                // check results
               Assert.AreEqual(51, engine.GenerateUniqueId(typeof(MapLink), StressTestsHelper.CreateMapData()),
                    "GenerateUniqueId should be correct.");
                Assert.AreEqual(51, engine.GenerateUniqueId(typeof(MapLink), StressTestsHelper.CreateMapData()),
                    "GenerateUniqueId should be correct.");
            }

            Console.WriteLine("Total time : " + (Environment.TickCount - start) + "ms.");
        }

        /// <summary>
        /// <para>
        /// Stress test for the method <c>GenerateUniqueId(Type entityType, MapData mapdata)</c>.
        /// Uses <c>MockBaseLayoutEngine(IConfiguration config)</c> to test.
        /// </para>
        /// </summary>
        [Test]
        public void TestStressGenerateUniqueId3()
        {
            long start = Environment.TickCount;
            for (int i = 1; i <= Times; ++i)
            {
                MockBaseLayoutEngine engine = new MockBaseLayoutEngine(StressTestsHelper.CreateConfig());

                // check results
                Assert.AreEqual(51, engine.GenerateUniqueId(typeof(MapPort), StressTestsHelper.CreateMapData()),
                    "GenerateUniqueId should be correct.");
                Assert.AreEqual(51, engine.GenerateUniqueId(typeof(MapPort), StressTestsHelper.CreateMapData()),
                    "GenerateUniqueId should be correct.");
            }

            Console.WriteLine("Total time : " + (Environment.TickCount - start) + "ms.");
        }
    }
}
