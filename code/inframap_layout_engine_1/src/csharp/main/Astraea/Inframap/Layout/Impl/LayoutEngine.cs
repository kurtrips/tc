/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 
using System;
using System.Collections.Generic;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ExceptionManager.SDE;
using Astraea.Inframap.Data;

namespace Astraea.Inframap.Layout.Impl
{
    /// <summary>
    /// <para>
    /// This is an implementation for a layout engine which utilizes the IGraphLayouter for
    /// the actual processing and which implements the pre-processing logic:
    /// <li>add necessary synthetic nodes to specific node lists</li>
    /// <li>link properly the created nodes (create new link, remove old link)</li>
    /// <li>create and place labels for nodes (excluding synthetic nodes)</li>
    /// <li>calculate minimal size for all nodes (excluding synthetic nodes)</li>
    /// <li>initialize all synthetic nodes with pre-configured minimal size.</li>
    /// </para>
    /// </summary>
    ///
    /// <threadsafety>
    /// The current implementation is thread safe as it is immutable.
    /// </threadsafety>
    ///
    /// <example>
    /// For a given MapData instance and an IConfiguration instance the following code is all that needs to be done:
    /// <code>
    /// LayoutEngine le = new LayoutEngine(config);
    /// MapData ret = le.Layout(mapdata);
    /// </code>
    /// </example>
    ///
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class LayoutEngine : BaseLayoutEngine
    {
        /// <summary>
        /// The name of the attributes to get for processing the links in the MapData.
        /// </summary>
        private const string Path = "path";

        /// <summary>
        /// The type of attribute when it stores its id in the integer form.
        /// </summary>
        private const string AttrTypeInt = "int";

        /// <summary>
        /// The type of attribute when it stores its id in the string form.
        /// </summary>
        private const string AttrTypeString = "string";

        /// <summary>
        /// The prefix to use when giving a name to a new created MapNode
        /// </summary>
        private const string MapNodeNamePrefix = "MapNode_";

        /// <summary>
        /// The prefix to use when giving a name to a new created MapLink
        /// </summary>
        private const string MapLinkNamePrefix = "MapLink_";

        /// <summary>
        /// The prefix to use when giving a name to a new created MapPort
        /// </summary>
        private const string MapPortNamePrefix = "MapPort_";

        /// <summary>
        /// <para>Creates a new LayoutEngine instance using the data from configuration.</para>
        /// </summary>
        /// <param name="config">configuration object</param>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if config is null.
        /// Wraps ConfigurationAPIException if any required configuration property is missing or has invalid value.
        /// </exception>
        public LayoutEngine(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// <para>This is specific implementation for pre-processing of map data.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be pre-processed</param>
        /// <returns>the pre-processed mapdata</returns>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if mapdata is null.</exception>
        /// <exception cref="LayoutPreProcessingException">Wraps any other exception in the pre-processing</exception>
        protected override MapData PreProcess(MapData mapdata)
        {
            Helper.Log(Logger, Level.INFO,
                "Entering PreProcess with mapdata: {0}.", new object[] { mapdata });

            try
            {
                Helper.ValidateNotNull(mapdata, "mapdata");

                //Perform the preprocessing logic
                AddNewLinksAndSyntheticNodes(mapdata);
                CreateLabelsForNonSyntheticNodes(mapdata);
                CalculateMinimalSizeForPorts(mapdata);
                CalculateMinimalSizeForNodes(mapdata);

                return mapdata;
            }
            catch (ArgumentNullException ane)
            {
                Helper.Log(Logger, Level.ERROR,
                    "Error in PreProcess with error message: {0}.", new object[] { ane.Message });

                throw Helper.GetSelfDocumentingException(ane, "Unable to pre-process the mapdata.",
                    "Astraea.Inframap.Layout.LayoutEngine.PreProcess",
                    new string[0], new object[0],
                    new string[] { "mapdata" }, new object[] { mapdata },
                    new string[0], new object[0]);
            }
            catch (Exception e)
            {
                Helper.Log(Logger, Level.ERROR,
                    "Error in PreProcess with error message: {0}.", new object[] { e.Message });

                throw Helper.GetSelfDocumentingException(
                    new LayoutPreProcessingException(e.Message, e),
                    "Unable to pre-process the mapdata.",
                    "Astraea.Inframap.Layout.LayoutEngine.PreProcess",
                    new string[0], new object[0],
                    new string[] { "mapdata" }, new object[] { mapdata },
                    new string[0], new object[0]);
            }
            finally
            {
                Helper.Log(Logger, Level.INFO,
                    "Exiting PreProcess with mapdata: {0}.", new object[] { mapdata });
            }
        }

        /// <summary>
        /// <para>This is an empty implementation that simply returns the input object.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be post-processed</param>
        /// <returns>the post-processed mapdata</returns>
        /// <exception cref="SelfDocumentingException">Wraps ArgumentNullException if mapdata is null.</exception>
        protected override MapData PostProcess(MapData mapdata)
        {
            Helper.Log(Logger, Level.INFO,
                "Entering PostProcess with mapdata: {0}.", new object[] { mapdata });

            try
            {
                Helper.ValidateNotNull(mapdata, "mapdata");

                Helper.Log(Logger, Level.INFO,
                    "Exiting PostProcess with return value: {0}.", new object[] { mapdata });

                //Simply return the input object
                return mapdata;
            }
            catch (ArgumentNullException ane)
            {
                Helper.Log(Logger, Level.ERROR,
                    "Error in PostProcess with error message: {0}.", new object[] { ane.Message });

                throw Helper.GetSelfDocumentingException(ane, "Unable to post-process the mapdata.",
                    "Astraea.Inframap.Layout.LayoutEngine.PostProcess",
                    new string[0], new object[0],
                    new string[] { "mapdata" }, new object[] { mapdata },
                    new string[0], new object[0]);
            }
            finally
            {
                Helper.Log(Logger, Level.INFO,
                    "Exiting PostProcess with mapdata: {0}.", new object[] { mapdata });
            }
        }

        /// <summary>
        /// This method performs the following logic:
        /// For each link in the mapdata, it gets the nodes defining the link from its 'path' attributes.
        /// For each such node, it creates a new synthetic node as a child of the node.
        /// For each such synthetic node, it creates a port for it.
        /// It then forms new links between the nodes of the original link and the new ports formed.
        /// Finally it adds all the new links formed to the mapdata and removes the original link.
        /// </summary>
        /// <param name="mapdata">MapData to be processed</param>
        private void AddNewLinksAndSyntheticNodes(MapData mapdata)
        {
            IList<MapLink> linksToDelete = new List<MapLink>();
            IList<MapLink> linksToAdd = new List<MapLink>();

            foreach (ILink link in mapdata.Links)
            {
                MapLink mapLink = (MapLink)link;

                //Get the path attributes
                IList<IAttribute> pathAttrs = mapLink.GetAttributesByName(Path);

                //Preserve the original link if no path attributes are found
                if (pathAttrs == null || pathAttrs.Count == 0)
                {
                    continue;
                }

                //Get the nodes of the path
                IList<MapNode> pathNodes = new List<MapNode>();
                foreach (IAttribute attr in pathAttrs)
                {
                    MapAttribute pathAttr = (MapAttribute)attr;
                    if (pathAttr.Type.Equals(AttrTypeInt))
                    {
                        pathNodes.Add((MapNode)mapdata.GetNodeById(pathAttr.IntValue));
                    }
                    else if (pathAttr.Type.Equals(AttrTypeString))
                    {
                        pathNodes.Add((MapNode)mapdata.GetNodeByName(pathAttr.StringValue));
                    }
                }

                //Create the synthetic nodes for each path node.
                IList<MapNode> syntheticNodes = new List<MapNode>();
                foreach (MapNode pathNode in pathNodes)
                {
                    if (mapdata.Nodes.Contains(pathNode))
                    {
                        //Get the unique id for the new synthetic node and set it
                        long syntheticNodeId = GetUniqueId(typeof(MapNode), mapdata, 0);
                        MapNode syntheticNode = new MapNode();
                        syntheticNode.Id = syntheticNodeId;
                        syntheticNode.Name = MapNodeNamePrefix + syntheticNodeId;

                        //Add it as child node of pathNode
                        pathNode.AddOccupant(syntheticNode);
                        syntheticNode.Container = pathNode;

                        //Add it to the mapData
                        mapdata.AddNode(syntheticNode);

                        //Create and add port to synthetic node.
                        long syntheticNodePortId = GetUniqueId(typeof(MapPort), mapdata, 0);
                        MapPort syntheticNodePort = new MapPort();
                        syntheticNodePort.Id = syntheticNodePortId;
                        syntheticNodePort.Name = MapPortNamePrefix + syntheticNodePortId;
                        syntheticNode.AddPort(syntheticNodePort);

                        //Add synthetic node to the port
                        syntheticNodePort.Node = syntheticNode;

                        //Add created port to the mapdata
                        mapdata.AddPort(syntheticNodePort);

                        //Save it further processing
                        syntheticNodes.Add(syntheticNode);
                    }
                }

                //None of the path nodes were found in the mapdata nodes.
                if (syntheticNodes.Count == 0)
                {
                    continue;
                }

                //Create new links
                //1)Note that we first get port and then get the node from the port.
                //See http://forums.topcoder.com/?module=Thread&threadID=592050&start=0&mc=24#885358
                //2)Links are guaranteed to have exactly 2 nodes
                MapPort startPort = (MapPort)mapLink.Ports[0];
                MapNode startNode = (MapNode)startPort.Node;
                MapNode endNode = null;
                MapPort endPort = null;
                for (int i = 0; i < syntheticNodes.Count; i++)
                {
                    endNode = syntheticNodes[i];
                    endPort = (MapPort)syntheticNodes[i].Ports[0];

                    //Create link and save it for adding later
                    linksToAdd.Add(CreateNewLink(startNode, endNode, startPort, endPort, mapdata, mapLink, linksToAdd.Count));

                    startNode = endNode;
                    startPort = endPort;
                }
                endPort = (MapPort)mapLink.Ports[1];
                endNode = (MapNode)endPort.Node;
                //Create last link and save it for adding later
                linksToAdd.Add(CreateNewLink(startNode, endNode, startPort, endPort, mapdata, mapLink, linksToAdd.Count));

                //Save original link for deleting it later
                linksToDelete.Add(mapLink);
            }

            //Remove the original links
            foreach (MapLink linkToDelete in linksToDelete)
            {
                //Remove link's attributes from mapdata
                foreach (IAttribute linkAttr in linkToDelete.Attributes)
                {
                    mapdata.RemoveAttribute((MapAttribute)linkAttr);
                }
                //REmove the link itself from mapdata
                mapdata.RemoveLink(linkToDelete);
            }

            //Add the new links
            foreach (MapLink linkToAdd in linksToAdd)
            {
                mapdata.AddLink(linkToAdd);
            }
        }

        /// <summary>
        /// This method creates a new label for all the nodes of the mapdata.
        /// It also sets the text and minimal size of these labels.
        /// </summary>
        /// <param name="mapdata">MapData to be processed</param>
        private void CreateLabelsForNonSyntheticNodes(MapData mapdata)
        {
            //Create label for all non-synthetic nodes
            foreach (INode node in mapdata.Nodes)
            {
                MapNode mapDataNode = (MapNode)node;
                if (mapDataNode.Id >= 0)
                {
                    //Create label for the node and set its text and minimal size
                    MapLabel nodeLabel = new MapLabel();
                    nodeLabel.Text = mapDataNode.Name;
                    nodeLabel.MinimalSize = new Dimension(
                        (int)(FontUnits * DefaultFontSize), (int)(mapDataNode.Name.Length * CharacterUnits));
                    mapDataNode.Label = nodeLabel;
                }
            }
        }

        /// <summary>
        /// This method sets the minimal size of each port in the mapdata.
        /// </summary>
        /// <param name="mapdata">MapData to be processed</param>
        private void CalculateMinimalSizeForPorts(MapData mapdata)
        {
            //calculate the minimal size for each port
            foreach (IPort port in mapdata.Ports)
            {
                MapPort mapPort = (MapPort)port;
                //Set its minimal size
                mapPort.MinimalSize = new Dimension(MinimumPortHeight, MinimumPortWidth);
            }
        }

        /// <summary>
        /// This method sets the minimal size of all nodes (normal and synthetic) of the mapdata
        /// </summary>
        /// <param name="mapdata">MapData to be processed.</param>
        private void CalculateMinimalSizeForNodes(MapData mapdata)
        {
            //calculate the minimal size for each non-synthetic node
            foreach (INode node in mapdata.Nodes)
            {
                MapNode mapNode = (MapNode)node;
                if (node.Id >= 0)
                {
                    //Get the number of linked ports
                    int numLinkedPorts = 0;
                    foreach (IPort nodePort in node.Ports)
                    {
                        if (nodePort.Links != null && nodePort.Links.Count > 0)
                        {
                            numLinkedPorts++;
                        }
                    }

                    int portSize = Math.Max(MinimumPortHeight, MinimumPortWidth);
                    //Get s1
                    int s1 = numLinkedPorts * portSize + (numLinkedPorts + 1) * MinimumLinkSpace;

                    //Get s2
                    int numNonLinkedPorts = node.Ports.Count - numLinkedPorts;
                    int s2 = numNonLinkedPorts * portSize + (numNonLinkedPorts + 1) * MinimumUnlinkedPortSpace;

                    //Get s3
                    int s3 = (node.Ports.Count / 4 + 1) * portSize +
                        (node.Ports.Count / 4 + 2) * Math.Max(MinimumLinkSpace, MinimumUnlinkedPortSpace);

                    //Set the minimalSize for the node
                    int minHeight = Math.Max(Math.Max(s1, s2), Math.Max(s3, MinimumNodeHeight));
                    int minWidth = Math.Max(Math.Max(s1, s2), Math.Max(s3, MinimumNodeWidth));
                    mapNode.MinimalSize = new Dimension(minHeight, minWidth);
                }
                //For each synthetic node, get the minimal size from configuration.
                else
                {
                    mapNode.MinimalSize = new Dimension(MinimumSyntheticNodeHeight, MinimumSyntheticNodeWidth);
                }
            }

        }

        /// <summary>
        /// Creates a new link between the startNode and the endNode.
        /// The startPort and endPort are the ports for the start and end nodes.
        /// </summary>
        /// <param name="startNode">The starting node</param>
        /// <param name="endNode">The ending node</param>
        /// <param name="startPort">The starting port</param>
        /// <param name="endPort">the ending port</param>
        /// <param name="mapdata">The Mapdata to use for generating the unique id of the link.</param>
        /// <param name="origLink">The original link</param>
        /// <param name="toBeAddedLinks">The number of links yet to be added.</param> 
        /// <returns>The new link created.</returns>
        private MapLink CreateNewLink(
            MapNode startNode, MapNode endNode, MapPort startPort, MapPort endPort, MapData mapdata,
            MapLink origLink, int toBeAddedLinks)
        {
            //Create new link
            long newLinkId = GetUniqueId(typeof(MapLink), mapdata, toBeAddedLinks);
            MapLink newLink = new MapLink();
            newLink.Id = newLinkId;
            newLink.Name = MapLinkNamePrefix + newLinkId;

            //Add the nodes
            newLink.AddNode(startNode);
            newLink.AddNode(endNode);

            //Add the ports
            newLink.AddPort(startPort);
            newLink.AddPort(endPort);

            //Copy attributes of original link and set owner id of the new attributes. Also add to mapdata
            CopyAttributes(origLink, newLink, newLinkId, mapdata);

            //create a new attribute to store the original link id
            MapAttribute origLinkIdAttr = new MapAttribute();
            origLinkIdAttr.Name = "origLinkId";
            origLinkIdAttr.StringValue = origLink.Id.ToString();
            long newAttrId = GetUniqueId(typeof(MapAttribute), mapdata, 0);
            origLinkIdAttr.Id = newAttrId;
            origLinkIdAttr.OwnerType = "link";
            origLinkIdAttr.OwnerId = newLink.Id;
            origLinkIdAttr.Type = AttrTypeString;

            //Add the new attribute to the new link and the map data
            newLink.Attributes.Add(origLinkIdAttr);
            mapdata.AddAttribute(origLinkIdAttr);

            //Set new link's element type from original link
            newLink.ElementType = origLink.ElementType;

            return newLink;
        }

        /// <summary>
        /// Gets a new unique id for an entity type in the map data.
        /// </summary>
        /// <param name="entityType">The entity type</param>
        /// <param name="mapData">The map data</param>
        /// <param name="toBeSavedElementsCount">The number of entities which are yet to be saved</param>
        /// <returns>The new generated unique id</returns>
        private long GetUniqueId(Type entityType, MapData mapData, int toBeSavedElementsCount)
        {
            long uniqueId = GenerateUniqueId(entityType, mapData);
            return (uniqueId + toBeSavedElementsCount) * -1;
        }

        /// <summary>
        /// Copies the attributes of an old link to a new one.
        /// </summary>
        /// <param name="origLink">The old link</param>
        /// <param name="newLink">The new link</param>
        /// <param name="newLinkId">The id of the new link</param>
        /// <param name="mapdata">The mapdata into which the new attributes will be added.</param>
        private void CopyAttributes(MapLink origLink, MapLink newLink, long newLinkId, MapData mapdata)
        {
            foreach (MapAttribute attr in origLink.Attributes)
            {
                //Create a new attribute and set its properties
                MapAttribute newattr = new MapAttribute();
                newattr.Id = GetUniqueId(typeof(MapAttribute), mapdata, 0);
                newattr.OwnerId = newLinkId;
                newattr.Name = attr.Name;
                newattr.OwnerType = attr.OwnerType;
                newattr.Type = attr.Type;
                newattr.IntValue = attr.IntValue;
                newattr.StringValue = attr.StringValue;
                newattr.DoubleValue = attr.DoubleValue;
                newattr.DateTimeValue = attr.DateTimeValue;

                //Add new attribute to the new link
                newLink.AddAttribute(newattr);

                //Add new attribute to the new mapdata
                mapdata.AddAttribute(newattr);
            }
        }
    }
}
