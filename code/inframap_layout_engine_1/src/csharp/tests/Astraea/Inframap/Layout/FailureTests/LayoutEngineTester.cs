using System;
using Astraea.Inframap.Data;
using TopCoder.Configuration;
using Astraea.Inframap.Layout.Impl;

namespace Astraea.Inframap.Layout.FailureTests
{
    /// <summary>
    /// <para>
    /// This the tester class for LayoutEngine that exposes the protected methods of the
    /// LayoutEngine class.
    /// </para>
    /// </summary>
    public class LayoutEngineTester : LayoutEngine
    {

        /// <summary>
        /// <para>This constructor initializes the instance state with data from configuration.</para>
        /// </summary>
        /// <param name="config">configuration object</param>
        public LayoutEngineTester(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// <para>This is specific implementation for pre-processing of map data.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be pre-processed</param>
        /// <returns>the pre-processed mapdata</returns>
        public new MapData PreProcess(MapData mapdata)
        {
            return base.PreProcess(mapdata);
        }

        /// <summary>
        /// <para>This is an empty implementation that simply returns the input object.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be post-processed</param>
        /// <returns>the post-processed mapdata</returns>
        public new MapData PostProcess(MapData mapdata)
        {
            return base.PostProcess(mapdata);
        }

        /// <summary>
        /// <para>This method generates a unique id (always positive but unique amongst both
        /// negative and positive ids) for the specific type and within the specific mapdata context.</para>
        /// </summary>
        /// <param name="entityType">this is the type of entity that we generate the id for.</param>
        /// <param name="mapdata">this is the context for which the id must be unique</param>
        /// <returns>the generated unique id</returns>
        public new long GenerateUniqueId(Type entityType, MapData mapdata)
        {
            return base.GenerateUniqueId(entityType, mapdata);
        }

        /// <summary>
        /// <para>This is a concrete method which simply delegates to the graphLayouter.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be processed with layouting</param>
        /// <returns>mapdata that has been processed with layouting</returns>
        public new MapData PerformLayout(MapData mapdata)
        {
            return base.PerformLayout(mapdata);
        }
    }
}
