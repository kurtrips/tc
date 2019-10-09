// MapData.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// <p>Mock MapData implementation.</p>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapData : IGraph
    {
        /// <summary>
        /// The list of nodes for the MapData
        /// </summary>
        IList<INode> nodes = new List<INode>();

        /// <summary>
        /// The list of links for the MapData
        /// </summary>
        IList<ILink> links = new List<ILink>();

        /// <summary>
        /// The list of ports for the MapData
        /// </summary>
        IList<IPort> ports = new List<IPort>();

        /// <summary>
        /// The attributes for the map data.
        /// </summary>
        IList<MapAttribute> attributes = new List<MapAttribute>();

        /// <summary>
        /// Gets or sets the attributes for the map data.
        /// </summary>
        /// <value>the attributes for the map data</value>
        public IList<MapAttribute> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// The MapHeader for the MapData
        /// </summary>
        MapHeader header = null;

        /// <summary>
        /// Mock constructor implementation.
        /// </summary>
        public MapData()
        {
        }

        /// <summary>
        /// The MapHeader for the MapData.
        /// </summary>
        /// <value>The MapHeader for the MapData.</value>
        public MapHeader Header
        {
            get { return header; }
            set { header = value; }
        }

        /// <summary>
        /// The nodes in the MapData 
        /// </summary>
        /// <value>The nodes in the MapData</value>
        public IList<INode> Nodes
        {
            get { return nodes; }
        }

        /// <summary>
        /// The links in the MapData 
        /// </summary>
        /// <value>The links in the MapData</value>
        public IList<ILink> Links
        {
            get { return links; }
        }

        /// <summary>
        /// The ports in the MapData 
        /// </summary>
        /// <value>The ports in the MapData</value>
        public IList<IPort> Ports
        {
            get { return ports; }
        }

        /// <summary>
        /// Gets the node in the mapData for the given id.
        /// </summary>
        /// <param name="nodeId">The node id</param>
        /// <returns>The node in the mapData for the given id.</returns>
        public INode GetNodeById(long nodeId)
        {
            foreach (INode node in nodes)
            {
                if (node.Id == nodeId)
                {
                    return node;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the port in the mapData for the given id.
        /// </summary>
        /// <param name="portId">The port id</param>
        /// <returns>The port in the mapData for the given id.</returns>
        public IPort GetPortById(long portId)
        {
            foreach (IPort port in ports)
            {
                MapPort mapPort = (MapPort)port;
                if (mapPort.Id == portId)
                {
                    return mapPort;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the link with given id
        /// </summary>
        /// <param name="linkId">The id</param>
        /// <returns>The link with given id or null if no such id is present</returns>
        public ILink GetLinkById(long linkId)
        {
            foreach (ILink link in links)
            {
                MapLink mapLink = (MapLink)link;
                if (mapLink.Id == linkId)
                {
                    return mapLink;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the node in the mapData for the given name.
        /// </summary>
        /// <param name="nodeId">The node name</param>
        /// <returns>The node in the mapData for the given name.</returns>
        public INode GetNodeByName(string name)
        {
            foreach (INode node in nodes)
            {
                MapNode mapNode = (MapNode)node;
                if (mapNode.Name == name)
                {
                    return mapNode;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a node to the MapData
        /// </summary>
        /// <param name="node">The node to add</param>
        public void AddNode(INode node)
        {
            nodes.Add(node);
        }

        /// <summary>
        /// Adds a port to the MapData
        /// </summary>
        /// <param name="port">The port to add</param>
        public void AddPort(IPort port)
        {
            ports.Add(port);
        }

        /// <summary>
        /// Adds a link to the MapData
        /// </summary>
        /// <param name="link">The link to add</param>
        public void AddLink(ILink link)
        {
            links.Add(link);
        }

        /// <summary>
        /// Removes a link from the MapData
        /// </summary>
        /// <param name="link">The link to remove</param>
        /// <returns>true if sucessfully removed, false otherwise</returns>
        public bool RemoveLink(ILink link)
        {
            return links.Remove(link);
        }

        /// <summary>
        /// Adds attribute to the attribute list.
        /// </summary>
        /// <param name="attr">The attribute to add</param>
        public void AddAttribute(MapAttribute attr)
        {
            attributes.Add(attr);
        }

        /// <summary>
        /// removes attribute to the attribute list.
        /// </summary>
        /// <param name="attr">The attribute to remove</param>
        public void RemoveAttribute(MapAttribute attr)
        {
            attributes.Remove(attr);
        }
    }
}
