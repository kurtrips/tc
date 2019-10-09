/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Astraea.Inframap.Data;
using Astraea.Inframap.Layout.Impl;
using TopCoder.Configuration;

namespace Astraea.Inframap.Layout.AccuracyTests
{
    /// <summary>
    /// Mock <see cref="LayoutEngine"/> to expose protected methods for accuracy tests.
    /// </summary>
    ///
    ///
    /// <author>jueyey</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class MockLayoutEngine : LayoutEngine
    {
        /// <summary>
        /// A mock constructor for this class.
        /// </summary>
        /// <param name="config">The configuration instance to use for the class.</param>
        public MockLayoutEngine(IConfiguration config)
            : base(config)
        {
        }

      

        /// <summary>
        /// Mock <c>GenerateUniqueId</c> method for testing.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="mapdata"></param>
        /// <returns></returns>
        public long MockGenerateUniqueId(Type entityType, MapData mapdata)
        {
            return base.GenerateUniqueId(entityType, mapdata);
        }

        /// <summary>
        /// Mock <c>PreProcess</c> method for testing.
        /// </summary>
        /// <param name="mapdata">the input map data</param>
        /// <returns>the processed result</returns>
        public MapData MockPreProcess(MapData mapdata)
        {
            return base.PreProcess(mapdata);
        }

        /// <summary>
        /// Mock <c>PostProcess</c> method for testing.
        /// </summary>
        /// <param name="mapdata">the input map data</param>
        /// <returns>the processed result</returns>
        public MapData MockPostProcess(MapData mapdata)
        {
            return base.PostProcess(mapdata);
        }
    }
}
