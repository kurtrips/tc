using TopCoder.Graph.Layout;
using System.Collections.Generic;
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents a map node. It extends MapElement for defined common fields and implements INode to expose the fields provided by MapElement and position, size and minimal size, a label, the name of the element type it represents, the parent and children as INodes, and ports. It also contains a stub node, and exposes the parent and children via different properties so they use MapNode type. It also implements the IAnchor interface to provide an anchor ID. This entity will be created and filled in the MapDataDownloader. It contains convenience methods to add and remove members of the lists more easily. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapNode : MapElement, INode
    {

        /**
         * <p>Represents the parent node.</p>
         * <p>This can be any value. It will be managed with the Container property.</p>
         * 
         */
        private MapNode container = null;

        /**
         * <p>Represents another node this node connects to in the same layer.</p>
         * <p>This can be any value. It will be managed with the Stub property.</p>
         * 
         */
        private MapNode stub = null;

        /**
         * <p>Represents a list of children.</p>
         * <p>This can be any value. It will be managed with the ContainedNodes property and the addContainedNode and removeContainedNode convenience methods.</p>
         * 
         * 
         */
        private IList<MapNode> containedNodes = new List<MapNode>();

        /**
         * <p>Represents a list of ports the node contains.</p>
         * <p>This can be any value. It will be managed with the Ports property and the addPort and removePort convenience methods.</p>
         * 
         * 
         */
        private IList<IPort> ports = new List<IPort>();

        /**
         * <p>Represents the position of the node.</p>
         * <p>This can be any value. It will be managed with the Position property.</p>
         * 
         * 
         */
        private Coordinates position = null;

        /**
         * <p>Represents the size of the node.</p>
         * <p>This can be any value. It will be managed with the Size property.</p>
         * 
         * 
         */
        private Dimension size = null;

        /**
         * <p>Represents the label associated with the node.</p>
         * <p>This can be any value. It will be managed with the Label property.</p>
         * 
         * 
         */
        private ILabel label = null;

        /**
         * <p>Represents the minimum size for this node.</p>
         * <p>This can be any value. It will be managed with the MinimalSize property.</p>
         * 
         */
        private Dimension minimalSize = null;

        private IList<INode> occupants = new List<INode>();

        /**
         * <p>This is the property for the container field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the container field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the container field to the value.</li>
         * </ul>
         * 
         * 
         */
        //public MapNode Container
        //{
        //    get
        //    {
        //        return container;
        //    }
        //    set
        //    {
        //        container = value;
        //    }
        //}

        /**
         * <p>This is the property for the stub field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the stub field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the stub field to the value.</li>
         * </ul>
         * 
         * 
         */
        public MapNode Stub
        {
            get
            {
                return stub;
            }
            set
            {
                stub = value;
            }
        }

        /**
         * <p>This is the property for the containedNodes field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the containedNodes field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the containedNodes field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<MapNode> ContainedNodes
        {
            get
            {
                return containedNodes;
            }
            set
            {
                containedNodes = value;
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
         * <p>This is the property for the position field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the position field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the position field to the value.</li>
         * </ul>
         * 
         * 
         */
        public Coordinates Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
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
         * <p>This is the property for the label field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the label field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the label field to the value.</li>
         * </ul>
         * 
         * 
         */
        public ILabel Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        /**
         * <p>This is the property for the the parent of this node. It will be backed by the Container property.</p>
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the Container Property</li>
         * </ul>
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the Container Property to the value.</li>
         * </ul>
         * 
         */
        public INode Container
        {
            get
            {
                return container;
            }
            set
            {
                container = (MapNode)value;
            }
        }

        /**
         * <p>This is the property for the child nodes of this node. It will be backed by the ContainedNodes property.</p>
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Return a new List of INode created from the value of the ContainedNodes Property</li>
         * </ul>
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Flush the contents of the ContainedNodes Property, and populate them to the contents, propertly casted to MapNode, of the INode list in the value.</li>
         * </ul>
         * 
         */
        public IList<INode> Occupants
        {
            get
            {
                IList<INode> list = new List<INode>();

                foreach (MapNode node in ContainedNodes)
                {
                    list.Add(node);
                }
                return list;
            }
            set
            {
                ContainedNodes.Clear();

                foreach (INode node in value)
                {
                    ContainedNodes.Add(node as MapNode);
                }
            }
        }

        /**
         * <p>This is the property for the name of the element type that this map node belongs to. It will be backed by the ElementType.Name property.</p>
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the ElementType.Name Property</li>
         * </ul>
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the ElementType.Name Property to the value.</li>
         * </ul>
         * 
         */
        public string Class
        {
            get
            {
                return ElementType.Name;
            }
            set
            {
                ElementType.Name = value;
            }
        }

        /**
         * <p>This is the property for the minimalSize field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the minimalSize field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the minimalSize field to the value.</li>
         * </ul>
         * 
         * 
         */
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

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapNode()
        {
            // empty
        }

        /**
         * Convenience method to add a MapNode to the containedNodes list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param node The MapNode to add to the containedNodes list
         * @throws ArgumentNullException If node is null
         */
        public void AddContainedNode(MapNode node)
        {
            containedNodes.Add(node);
        }

        /**
         * Convenience method to remove the first occurence of the MapNode from the containedNodes list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param node The MapNode to remove from the containedNodes list
         * @throws ArgumentNullException If node is null
         */
        public bool RemoveContainedNode(MapNode node)
        {
            return containedNodes.Remove(node);
        }

        /**
         * Convenience method to remove the first occurence of the MapNode with the given Id from the containedNodes list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param nodeId The Id of the MapNode to remove from the containedNodes list
         */
        public bool RemoveContainedNode(long nodeId)
        {
            foreach (MapNode node in containedNodes)
            {
                if (node.Id == nodeId)
                {
                    containedNodes.Remove(node);
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
            return ports.Remove(port) ;
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
         * Convenience method to add a INode to the children list. Null parameters are not allowed.
         * It will simply defer to AddContainedNode(MapNode) method.
         * 
         * 
         * @param child The INode to add to the children list
         * @throws ArgumentNullException If child is null or not a MapNode
         */
        public void AddOccupant(INode child)
        {
            occupants.Add(child);
        }

        /**
         * Convenience method to remove the first occurence of the INode from the children list. Null parameters are not allowed.
         * It will simply defer to RemoveContainedNode(MapNode) method.
         * 
         * 
         * @return True if removed. False otherwise
         * @param child The INode to remove from the children list
         * @throws ArgumentNullException If child is null or not a MapNode
         */
        public bool RemoveOccupant(INode child)
        {
            return occupants.Remove(child);
        }

        /**
         * Convenience method to remove the first occurence of the INode with the given Id from the children list.
         * It will simply defer to RemoveContainedNode(long) method.
         * 
         * 
         * @return True if removed. False otherwise
         * @param childId The Id of the INode to remove from the children list
         */
        public bool RemoveChild(long childId)
        {
            foreach (INode child in Occupants)
            {
                if (child.Id == childId)
                {
                    Occupants.Remove(child);
                    return true;
                }
            }
            return false;
        }

        /**
         * Generates the anchor ID. Simply returns "node" + Id value.
         * 
         * 
         * @return the anchor ID
         */
        public string GetAnchorId()
        {
            return "node" + Id;
        } 
    }
}