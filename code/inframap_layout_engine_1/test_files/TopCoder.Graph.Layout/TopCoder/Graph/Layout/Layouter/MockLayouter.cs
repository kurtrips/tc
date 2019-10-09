// MockLayouter.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Graph.Layout.Layouter
{
    /// <summary>
    /// A mock implementation of the IGraphLayouter interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MockLayouter : IGraphLayouter
    {
        /// <summary>
        /// A mock implementation of the Layout method.
        /// Simply adds a new node with id 7 to the graph.
        /// </summary>
        /// <param name="graph">The graph to layout</param>
        /// <returns>The layoutted graph</returns>
        public IGraph Layout(IGraph graph)
        {
            //A mock LayoutException
            if (graph.Nodes.Count == 0)
            {
                throw new LayoutException("This is just a mock exception.");
            }

            graph.Nodes.Add(new Node());
            return graph;
        }
    }
}
