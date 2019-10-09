// IGraphLayouter.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock IGraphLayouter interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IGraphLayouter
    {
        /// <summary>
        /// Lays out the graph
        /// </summary>
        /// <param name="graph">The graph</param>
        /// <returns>The layoutted graph.</returns>
        IGraph Layout(IGraph graph);
    }
}
