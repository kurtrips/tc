// IGraph.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock IGraph interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IGraph
    {
        /// <summary>
        /// Gets the nodes of the graph.
        /// </summary>
        /// <value>the nodes of the graph.</value>
        IList<INode> Nodes
        {
            get;
        }

        /// <summary>
        /// Gets the links of the graph.
        /// </summary>
        /// <value>the links of the graph.</value>
        IList<ILink> Links
        {
            get;
        }

        /// <summary>
        /// Gets the ports of the graph.
        /// </summary>
        /// <value>the ports of the graph.</value>
        IList<IPort> Ports
        {
            get;
        }
    }
}
