/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using Astraea.Inframap.Data;
using TopCoder.Configuration;
using System;
using TopCoder.Util.ExceptionManager.SDE;

namespace Astraea.Inframap.Layout.StressTests
{
    /// <summary>
    /// <para>
    /// A mock implementation for the <c>BaseLayoutEngine</c> abstract class.
    /// </para>
    ///
    /// <para>
    /// <i>NOTE:</i>
    /// It is NOT strictly implemented, just for the purpose of stress tests.
    /// </para>
    /// </summary>
    ///
    /// <author>sparemax</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    internal class MockBaseLayoutEngine : BaseLayoutEngine
    {
        /// <summary>
        /// <para>
        /// This is a contract for pre-processing of map data.
        /// This method will be called before the <c>PerformLayout</c> method is called.
        /// </para>
        /// </summary>
        /// <param name="mapdata">
        /// the map data to be pre-processed.
        /// </param>
        ///
        /// <returns>
        /// the pre-processed map data.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// if the <c>mapdata</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="LayoutPreProcessingException">
        /// if any error occurs.
        /// </exception>
        protected override MapData PreProcess(MapData mapdata)
        {
            // there is no pre-processing in the mock implementation
            return mapdata;
        }

        /// <summary>
        /// <para>
        /// This is a contract for post-processing of map data.
        /// This method will be called after the <c>PerformLayout</c> method is called.
        /// </para>
        /// </summary>
        /// <param name="mapdata">
        /// the map data to be post-processed.
        /// </param>
        ///
        /// <returns>
        /// the post-processed map data.
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// if the <c>mapdata</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="LayoutPostProcessingException">
        /// if any error occurs.
        /// </exception>
        protected override MapData PostProcess(MapData mapdata)
        {
            // there is no post-processing in the mock implementation
            return mapdata;
        }

        /// <summary>
        /// <para>
        /// Constructs a new <c>MockBaseLayoutEngine</c> instance with data from configuration.
        /// </para>
        /// </summary>
        /// <param name="config">
        /// the configuration object.
        /// </param>
        ///
        /// <exception cref="ArgumentNullException">
        /// if the <c>config</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="SelfDocumentingException">
        /// if any required element is missing.
        /// </exception>
        public MockBaseLayoutEngine(IConfiguration config)
            : base(config)
        {
            // empty
        }

        /// <summary>
        /// <para>
        /// Generates a unique id.
        /// </para>
        /// </summary>
        ///
        /// <param name="entityType">
        /// this is the type of entity that we generate the id for.
        /// </param>
        /// <param name="mapdata">
        /// this is the context for which the id must be unique
        /// </param>
        ///
        /// <returns>
        /// the generated unique id
        /// </returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// if <c>mapdata</c> or  is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// if <c>entityType</c> is invalid.
        /// </exception>
        public new long GenerateUniqueId(Type entityType, MapData mapdata)
        {
            return base.GenerateUniqueId(entityType, mapdata);
        }
    }
}
