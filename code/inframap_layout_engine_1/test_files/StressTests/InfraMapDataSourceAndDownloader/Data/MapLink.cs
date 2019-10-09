using TopCoder.Graph.Layout;
using System.Collections.Generic;
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents a map link. It extends MapElement for defined common fields and implements ILink to expose the fields provided by MapElement and the link segments, the name of the element type it represents, and lists of the ports and nodes it connects. It also implements the IAnchor interface to provide an anchor ID. This entity will be created and filled in the MapDataDownloader. It contains convenience methods to add and remove members of the lists more easily. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapLink : MapElement, ILink
    {

        /**
         * <p>Represents a list of ports that this link connects.</p>
         * <p>This can be any value. It will be managed with the Ports property and the addPort and removePort convenience methods.</p>
         * 
         * 
         */
        private IList<IPort> ports = new List<IPort>();

        /**
         * <p>Represents a list of nodes that this link connects.</p>
         * <p>This can be any value. It will be managed with the Nodes property and the addNode and removeNode convenience methods.</p>
         * 
         * 
         */
        private IList<INode> nodes = new List<INode>();

        /**
         * <p>Represents a list of segments that this link comprises.</p>
         * <p>This can be any value. It will be managed with the Segments property and the addSegment and removeSegment convenience methods.</p>
         * 
         * 
         */
        private IList<Segment> segments = new List<Segment>();

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
         * <p>This is the property for the name of the element type that this map link belongs to. It will be backed by the ElementType.Name property.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the ElementType.Name Property</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the ElementType.Name Property to the value.</li>
         * </ul>
         * 
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
         * <p>This is the property for the segments field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the segments field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the segments field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<Segment> Segments
        {
            get
            {
                return segments;
            }
            set
            {
                segments = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapLink()
        {
            // your code here
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
         * Convenience method to add a Segment to the segments list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param segment The Segment to add to the segments list
         * @throws ArgumentNullException If segment is null
         */
        public void AddSegment(Segment segment)
        {
            segments.Add(segment);
        }

        /**
         * Convenience method to remove the first occurence of the Segment from the segments list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param segment The Segment to remove from the segments list
         * @throws ArgumentNullException If segment is null
         */
        public bool RemoveSegment(Segment segment)
        {
            return segments.Remove(segment);
        }

        /**
         * Convenience method to remove the first occurence of the Segment with the given Id from the segments list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param segmentId The Id of the Segment to remove from the segments list
         */
        //public bool RemoveSegment(long segmentId)
        //{

        //    foreach (Segment segment in segments)
        //    {
        //        if (segment.Id == segmentId)
        //        {
        //            segments.Remove(segment);
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        /**
         * Generates the anchor ID. Simply returns "link" + Id value.
         * 
         * 
         * @return the anchor ID
         */
        public string GetAnchorId()
        {
            return "link" + Id;
        }
    }
}