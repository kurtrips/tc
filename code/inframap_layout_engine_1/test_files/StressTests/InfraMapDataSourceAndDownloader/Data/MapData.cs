using TopCoder.Graph.Layout;
using System.Collections.Generic;

namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents the main and top-level entity of the map data. It implements IGraph to expose all nodes, ports, and links, and its size. It also contains a list of all element types and legal attribute types, and a complete list of every attribute in every element. This entity will be created and filled in the MapDataDownloader. It contains convenience methods to add and remove members of the lists more easily. This class is Serializable. It contains convenience methods to locate nodes, ports, and links in this map by their name or type.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapData : IGraph
    {

        /**
         * <p>Represents the header info of thie data.</p>
         * <p>This can be any value. It will be managed with the Header property.</p>
         * 
         * 
         */
        private MapHeader header = null;

        /**
         * <p>Represents a list of all nodes in this map data.</p>
         * <p>This can be any value. It will be managed with the Nodes property and the addNode and removeNode convenience methods.
         * It will also be accessed by the GetNodeByName and GetNodeById methods.</p>
         * 
         * 
         */
        private IList<INode> nodes = new List<INode>();

        /**
         * <p>Represents a list of all links in this map data.</p>
         * <p>This can be any value. It will be managed with the Links property and the addLink and removeLink convenience methods.
         * It will also be accessed by the GetLinkByName and GetLinkById methods.</p>
         * 
         * 
         */
        private IList<ILink> links = new List<ILink>();

        /**
         * <p>Represents a list of all ports in this map data.</p>
         * <p>This can be any value. It will be managed with the Ports property and the addPort and removePort convenience methods.
         * It will also be accessed by the GetPortByName and GetPortById methods.</p>
         * 
         * 
         */
        private IList<IPort> ports = new List<IPort>();

        /**
         * <p>Represents all present attribute types.</p>
         * <p>This can be any value. It will be managed with the AttributeTypes property and the addAttributeType and removeAttributeType convenience methods.</p>
         * 
         * 
         */
        private IList<MapAttributeType> attributeTypes = new List<MapAttributeType>();

        /**
         * <p>Represents all present element types.</p>
         * <p>This can be any value. It will be managed with the ElementTypes property and the addElementType and removeElementType convenience methods.</p>
         * 
         * 
         */
        private IList<MapElementType> elementTypes = new List<MapElementType>();

        /**
         * <p>Represents all attributes of all elements.</p>
         * <p>This can be any value. It will be managed with the Attributes property and the addAttribute and removeAttribute convenience methods.</p>
         * 
         * 
         */
        private IList<MapAttribute> attributes = new List<MapAttribute>();

        /**
         * <p>Represents the size of the graph of this map.</p>
         * <p>This can be any value. It will be managed with the Size property.</p>
         * 
         * 
         */
        private Dimension size = null;

        /**
         * <p>This is the property for the header field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the header field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the header field to the value.</li>
         * </ul>
         * 
         * 
         */
        public MapHeader Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }

        /**
         * <p>This is the property for the nodes field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the nodes field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the nodes field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<INode> Nodes
        {
            get
            {
                return nodes;
            }
            set
            {
                nodes = value;
            }
        }

        /**
         * <p>This is the property for the links field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the links field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the links field to the value.</li>
         * </ul>
         * 
         * 
         */
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

        /**
         * <p>This is the property for the ports field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the ports field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the ports field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<IPort> Ports
        {
            get
            {
                return ports;
            }
            set
            {
                ports = value;
            }
        }

        /**
         * <p>This is the property for the attributeTypes field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the attributeTypes field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the attributeTypes field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<MapAttributeType> AttributeTypes
        {
            get
            {
                return attributeTypes;
            }
            set
            {
                attributeTypes = value;
            }
        }

        /**
         * <p>This is the property for the elementTypes field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the elementTypes field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the elementTypes field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<MapElementType> ElementTypes
        {
            get
            {
                return elementTypes;
            }
            set
            {
                elementTypes = value;
            }
        }

        /**
         * <p>This is the property for the attributes field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the attributes field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the attributes field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<MapAttribute> Attributes
        {
            get
            {
                return attributes;
            }
            set
            {
                attributes = value;
            }
        }

        /**
         * <p>This is the property for the size field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the size field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the size field to the value.</li>
         * </ul>
         * 
         * 
         */
        public Dimension Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapData()
        {
            // empty
        }

        /**
         * Convenience method to add a MapNode to the nodes list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param node The MapNode to add to the nodes list
         * @throws ArgumentNullException If node is null
         */
        public void AddNode(MapNode node)
        {
            nodes.Add(node);
        }

        /**
         * Convenience method to remove the first occurence of the MapNode from the nodes list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param node The MapNode to remove from the nodes list
         * @throws ArgumentNullException If node is null
         */
        public bool RemoveNode(MapNode node)
        {
            return nodes.Remove(node);
        }

        /**
         * Convenience method to remove the first occurence of the MapNode with the given Id from the nodes list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param nodeId The Id of the MapNode to remove from the nodes list
         */
        public bool RemoveNode(long nodeId)
        {
            foreach (MapNode node in nodes)
            {
                if (node.Id == nodeId)
                {
                    nodes.Remove(node);
                    return true;
                }
            }
            return false;
        }

        /**
         * Convenience method to add a MapLink to the links list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param link The MapLink to add to the links list
         * @throws ArgumentNullException If link is null
         */
        public void AddLink(MapLink link)
        {
            links.Add(link);
        }

        /**
         * Convenience method to remove the first occurence of the MapLink from the links list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param link The MapLink to remove from the links list
         * @throws ArgumentNullException If link is null
         */
        public bool RemoveLink(MapLink link)
        {
            return links.Remove(link);
        }

        /**
         * Convenience method to remove the first occurence of the MapLink with the given Id from the links list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param linkId The Id of the MapLink to remove from the links list
         */
        public bool RemoveLink(long linkId)
        {
            foreach (MapLink link in links)
            {
                if (link.Id == linkId)
                {
                    links.Remove(link);
                    return true;
                }
            }
            return false;
        }

        /**
         * Convenience method to add a MapPort to the ports list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param port The MapPort to add to the ports list
         * @throws ArgumentNullException If port is null
         */
        public void AddPort(MapPort port)
        {
            ports.Add(port);
        }

        /**
         * Convenience method to remove the first occurence of the MapPort from the ports list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param port The MapPort to remove from the ports list
         * @throws ArgumentNullException If port is null
         */
        public bool RemovePort(MapPort port)
        {
            return ports.Remove(port);
        }

        /**
         * Convenience method to remove the first occurence of the MapPort with the given Id from the ports list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param portId The Id of the MapPort to remove from the ports list
         */
        public bool RemovePort(long portId)
        {
            foreach (MapPort port in ports)
            {
                if (port.Id == portId)
                {
                    ports.Remove(port);
                    return true;
                }
            }
            return false;
        }

        /**
         * Convenience method to add a MapAttributeType to the attributeTypes list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param attributeType The MapAttributeType to add to the attributeTypes list
         * @throws ArgumentNullException If attributeType is null
         */
        public void AddAttributeType(MapAttributeType attributeType)
        {
            attributeTypes.Add(attributeType);
        }

        /**
         * Convenience method to remove the first occurence of the MapAttributeType from the attributeTypes list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param attributeType The MapAttributeType to remove from the attributeTypes list
         * @throws ArgumentNullException If attributeType is null
         */
        public bool RemoveAttributeType(MapAttributeType attributeType)
        {
            return attributeTypes.Remove(attributeType);
        }

        /**
         * Convenience method to remove the first occurence of the MapAttributeType with the given Id from the attributeTypes list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param attributeTypeId The Id of the MapAttributeType to remove from the attributeTypes list
         */
        public bool RemoveAttributeType(long attributeTypeId)
        {

            foreach (MapAttributeType attributeType in attributeTypes)
            {
                if (attributeType.Id == attributeTypeId)
                {
                    attributeTypes.Remove(attributeType);
                    return true;
                }
            }
            return false;
        }

        /**
         * Convenience method to add a MapElementType to the elementTypes list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param elementType The MapElementType to add to the elementTypes list
         * @throws ArgumentNullException If elementType is null
         */
        public void AddElementType(MapElementType elementType)
        {
            elementTypes.Add(elementType);
        }

        /**
         * Convenience method to remove the first occurence of the MapElementType from the elementTypes list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param elementType The MapElementType to remove from the elementTypes list
         * @throws ArgumentNullException If elementType is null
         */
        public bool RemoveElementType(MapElementType elementType)
        {
            return elementTypes.Remove(elementType);
        }

        /**
         * Convenience method to remove the first occurence of the MapElementType with the given Id from the elementTypes list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param elementTypeId The Id of the MapElementType to remove from the elementTypes list
         */
        public bool RemoveElementType(long elementTypeId)
        {

            foreach (MapElementType elementType in elementTypes)
            {
                if (elementType.Id == elementTypeId)
                {
                    elementTypes.Remove(elementType);
                    return true;
                }
            }
            return false;
        }

        /**
         * Convenience method to add a MapAttribute to the attributes list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param attribute The MapAttribute to add to the attributes list
         * @throws ArgumentNullException If attribute is null
         */
        public void AddAttribute(MapAttribute attribute)
        {
            attributes.Add(attribute);
        }

        /**
         * Convenience method to remove the first occurence of the MapAttribute from the attributes list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param attribute The MapAttribute to remove from the attributes list
         * @throws ArgumentNullException If attribute is null
         */
        public bool RemoveAttribute(MapAttribute attribute)
        {
            return attributes.Remove(attribute);
        }

        /**
         * Convenience method to remove the first occurence of the MapAttribute with the given Id from the attributes list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param attributeId The Id of the MapAttribute to remove from the attributes list
         */
        public bool RemoveAttribute(long attributeId)
        {

            foreach (MapAttribute attribute in attributes)
            {
                if (attribute.Id == attributeId)
                {
                    attributes.Remove(attribute);
                    return true;
                }
            }
            return false;
        }

        /**
         * Gets a node in this map entity that has the given Id. It will return a null if no such node exists.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>Simply iterate over the nodes list and recover the node with the given Id</li>
         * </ul>
         * 
         * 
         * @return a node with the given ID, or null if not found
         * @param nodeId ID of the node to retrieve
         */
        public MapNode GetNodeById(long nodeId)
        {
            foreach (MapNode node in nodes)
            {
                if (node.Id == nodeId)
                {
                    return node;
                }
            }
            return null;
        }

        /**
         * Gets a node in this map entity that has the given name. It will return a null if no such node exists.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>Simply iterate over the nodes list and recover the node with the given name</li>
         * </ul>
         * 
         * 
         * @return a node with the given name, or null if not found
         * @param name Name of the node to retrieve
         */
        public MapNode GetNodeByName(string name)
        {
            foreach (MapNode node in nodes)
            {
                if (string.Compare(node.Name, name) == 0)
                {
                    return node;
                }
            }
            return null;
        }
        /**
         * Gets a port in this map entity that has the given Id. It will return a null if no such port exists.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>Simply iterate over the ports list and recover the port with the given Id</li>
         * </ul>
         * 
         * 
         * @return a port with the given ID, or null if not found
         * @param portId ID of the port to retrieve
         */
        public MapPort GetPortById(long portId)
        {

            foreach (MapPort port in ports)
            {
                if (port.Id == portId)
                {
                    return port;
                }
            }
            return null;
        }

        /**
         * Gets a port in this map entity that has the given name. It will return a null if no such port exists.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>Simply iterate over the ports list and recover the port with the given name</li>
         * </ul>
         * 
         * 
         * @return a port with the given name, or null if not found
         * @param name Name of the port to retrieve
         */
        public MapPort GetPortByName(string name)
        {

            foreach (MapPort port in ports)
            {
                if (string.Compare(port.Name, name) == 0)
                {
                    return port;
                }
            }
            return null;
        }

        /**
         * Gets a link in this map entity that has the given Id. It will return a null if no such link exists.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>Simply iterate over the links list and recover the link with the given Id</li>
         * </ul>
         * 
         * 
         * @return a link with the given ID, or null if not found
         * @param linkId ID of the link to retrieve
         */
        public MapLink GetLinkById(long linkId)
        {

            foreach (MapLink link in links)
            {
                if (link.Id == linkId)
                {
                    return link;
                }
            }
            return null;
        }

        /**
         * Gets a link in this map entity that has the given name. It will return a null if no such link exists.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>Simply iterate over the links list and recover the link with the given name</li>
         * </ul>
         * 
         * 
         * @return a link with the given name, or null if not found
         * @param name Name of the link to retrieve
         */
        public MapLink GetLinkByName(string name)
        {

            foreach (MapLink link in links)
            {
                if (string.Compare(link.Name, name) == 0)
                {
                    return link;
                }
            }
            return null;
        }
    }
}