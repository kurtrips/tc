// MapPort.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// <p>Mock implementation of a map port.</p>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapPort : MapElement, IPort
    {
        /// <summary>
        /// <p>Represents the node in which this port is contained.</p>
        /// </summary>
        private INode node;

        /// <summary>
        /// <p>Represents the link that connects this port to something else.</p>
        /// </summary>
        private IList<ILink> links = new List<ILink>();

        /// <summary>
        /// <p>Represents the minimum size for this port.</p>
        /// </summary>
        private Dimension minimalSize;

        /// <summary>
        /// <p>Gets or sets the node for the port.</p>
        /// </summary>
        /// <value>The node of the port.</value>
        public INode Node
        {
            get
            {
                return node;
            }
            set
            {
                node = value;
            }
        }

        /// <summary>
        /// <p>Gets or sets the links of the port</p>
        /// </summary>
        /// <value>the links of the port</value>
        public IList<ILink> Links
        {
            get
            {
                return links;
            }
            set
            {
                links = value;
            }
        }

        /// <summary>
        /// <p>Gets or sets the minimalSize of the port</p>
        /// </summary>
        /// <value>The minimal size of the port</value>
        public Dimension MinimalSize
        {
            get
            {
                return minimalSize;
            }
            set
            {
                minimalSize = value;
            }
        }

        /// <summary>
        /// Creates a new MapPort.
        /// </summary>
        public MapPort()
        {
        }
    }
}
