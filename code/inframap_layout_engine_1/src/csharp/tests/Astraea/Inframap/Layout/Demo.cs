/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/
using System;
using NUnit.Framework;
using Astraea.Inframap.Data;
using Astraea.Inframap.Layout.Impl;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// Demonstrates the usage of the component.
    /// </summary>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [TestFixture]
    public class Demo
    {
        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            UnitTestHelper.LoadConfigManager();
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
        /// Demonstrates the usage of the component.
        /// </summary>
        [Test]
        public void DemoTest()
        {
            //The MapData and IConfiguration come from external sources.
            MapData input = UnitTestHelper.GetSampleMapData();
            IConfiguration config = UnitTestHelper.GetTestConfig();

            //Usage is extremely simple.

            //create a new engine with configuration data
            LayoutEngine layoutEngine = new LayoutEngine(config);

            Console.WriteLine("MAPDATA INFO BEFORE LAYOUT:");
            DisplayMapdataInfo(input);
            
            // process layout request for some data
            MapData ret = layoutEngine.Layout(input);

            Console.WriteLine("MAPDATA INFO AFTER LAYOUT:");
            DisplayMapdataInfo(ret);
        }

        /// <summary>
        /// A utility function for displaying the properties of the MapData
        /// </summary>
        /// <param name="md">The mapdata to display</param>
        private static void DisplayMapdataInfo(MapData md)
        {
            Console.WriteLine("MapData header lifecycle: " + md.Header.Lifecycle);
            
            foreach (ILink link in md.Links)
            {
                MapLink ml = (MapLink)link;
                Console.WriteLine("Link : ");
                Console.WriteLine("    Name: " + ml.Name);
                Console.WriteLine("    Id: " + ml.Id);
            }

            foreach (IPort port in md.Ports)
            {
                MapPort mp = (MapPort)port;
                Console.WriteLine("Port : ");
                Console.WriteLine("    Name: " + mp.Name);
                Console.WriteLine("    Id: " + mp.Id);
            }

            foreach (INode node in md.Nodes)
            {
                MapNode mn = node as MapNode;
                if (mn != null)
                {
                    Console.WriteLine("Node : ");
                    Console.WriteLine("    Name: " + mn.Name);
                    Console.WriteLine("    Id: " + mn.Id);
                }
            }

            Console.WriteLine();
        }
    }
}
