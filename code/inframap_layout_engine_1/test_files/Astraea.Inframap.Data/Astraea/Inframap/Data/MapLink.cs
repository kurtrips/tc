// MapLink.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// <p>A mock implementation of the MapLink class.</p>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapLink : MapElement, ILink
    {
        /// <summary>
        /// The nodes for the link.
        /// </summary>
        private IList<INode> nodes;

        /// <summary>
        /// Gets the nodes collection for the link
        /// </summary>
        /// <value>The nodes collection for the link</value>
        public IList<INode> Nodes
        {
            get
            {
                return nodes;
            }
        }

        /// <summary>
        /// The ports for the link.
        /// </summary>
        private IList<IPort> ports;

        /// <summary>
        /// Gets the ports collection for the link
        /// </summary>
        /// <value>The ports collection for the link</value>
        public IList<IPort> Ports
        {
            get
            {
                return ports;
            }
        }

        /// <summary>
        /// Adds a node to the link.
        /// </summary>
        /// <param name="mapNode">The node to add.</param>
        public void AddNode(INode mapNode)
        {
            nodes.Add(mapNode);
        }

        /// <summary>
        /// Adds a port to the link.
        /// </summary>
        /// <param name="mapPort">The port to add.</param>
        public void AddPort(IPort mapPort)
        {
            ports.Add(mapPort);
        }

        /// <summary>
        /// Mock constructor.
        /// </summary>
        public MapLink()
        {
            nodes = new List<INode>();
            ports = new List<IPort>();
        }

        /// <summary>
        /// Adds an attribute to link
        /// </summary>
        /// <param name="mapAttribute">Attribute to add</param>
        public void AddAttribute(MapAttribute mapAttribute)
        {
            Attributes.Add(mapAttribute);
        }
    }
}
