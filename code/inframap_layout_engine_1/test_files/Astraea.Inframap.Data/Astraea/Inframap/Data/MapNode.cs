// MapNode.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// <p>Mock implementation of the MapNode.</p>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapNode : MapElement, INode
    {
        /// <summary>
        /// The label for the node.
        /// </summary>
        ILabel label = null;

        /// <summary>
        /// The ports for the node.
        /// </summary>
        IList<IPort> portsList = new List<IPort>();

        /// <summary>
        /// The minimal size of the node.
        /// </summary>
        Dimension minimalSize = null;

        /// <summary>
        /// The children of the node.
        /// </summary>
        IList<INode> occupants = new List<INode>();

        /// <summary>
        /// The parent of the node
        /// </summary>
        INode containerNode = null;

        /// <summary>
        /// Creates a new MapNode
        /// </summary>
        public MapNode() { }

        /// <summary>
        /// Gets or sets the parent of the node
        /// </summary>
        /// <value>The parent of the node</value>
        public INode Container
        {
            get { return containerNode; }
            set { containerNode = value; }
        }

        /// <summary>
        /// Gets the id of the node.
        /// </summary>
        /// <value>the id of the node.</value>
        long INode.Id
        {
            get { return ((MapElement)(this)).Id; }
        }

        /// <summary>
        /// Gets the ports of the node.
        /// </summary>
        /// <value>The ports of the node.</value>
        public IList<IPort> Ports
        {
            get { return portsList; }
        }

        /// <summary>
        /// Gets or sets the label for the node.
        /// </summary>
        /// <value>the label for the node.</value>
        public ILabel Label
        {
            get { return label; }
            set { label = value; }
        }

        /// <summary>
        /// Gets or sets the minimal size of the node.
        /// </summary>
        /// <value>the minimal size of the node.</value>
        public Dimension MinimalSize
        {
            get { return minimalSize; }
            set { minimalSize = value; }
        }

        /// <summary>
        /// Adds child to the node.
        /// </summary>
        /// <param name="child">The child node to add</param>
        public void AddOccupant(INode child)
        {
            occupants.Add(child);
        }

        /// <summary>
        /// Adds a port to the node.
        /// </summary>
        /// <param name="port">The port to add.</param>
        public void AddPort(IPort port)
        {
            portsList.Add(port);
        }
    }
}
