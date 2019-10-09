/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 
using System;
using Astraea.Inframap.Data;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// <para>This is a simple contract for a layout engine processor which would basically
    /// provide the layout for the input map data.</para>
    /// </summary>
    /// <remarks>
    /// Implementations should be thread safe.
    /// </remarks>
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ILayoutEngine
    {
        /// <summary>
        /// <para>This is a contract interface which will be implementing by layouting
        /// implementation that will process the input MapData elements and properly size then and locate them according
        /// to some specfic needs.
        /// </para>
        /// </summary>
        /// <param name="mapdata">mapdata that we will be processing the layout for</param>
        /// <returns>the processed MapData</returns>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if mapdata is null.
        /// </exception>
        /// <exception cref="LayoutException">
        /// If there are any exceptions in the laying out of the map data.
        /// </exception>
        MapData Layout(MapData mapdata);
    }
}
