/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */


using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Layout.StressTests
{
    /// <summary>
    /// <para>
    /// A mock implementation for the <c>IGraphLayouter</c> interface.
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
    public class MockGraphLayouter : IGraphLayouter
    {
        /// <summary>
        /// <para>
        /// Constructs a default <c>MockGraphLayouter</c> instance.
        /// </para>
        /// </summary>
        public MockGraphLayouter()
        {
            // empty
        }

        /// <summary>
        /// Simply returns the argument.
        /// </summary>
        ///
        /// <param name="graph">
        /// The graph to layout
        /// </param>
        ///
        /// <returns>
        /// The layoutted graph.
        /// </returns>
        public IGraph Layout(IGraph graph)
        {
            return graph;
        }
    }
}
