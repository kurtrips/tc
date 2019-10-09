using TopCoder.Graph.Layout;
using System.Collections.Generic;
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents a map port. It extends MapElement for defined common fields and implements IPort to expose the fields provided by MapElement and position, size and minimal size, orientation, the name of the element type it represents, the INode this port is in and a ILink that potentially connects this port to something else. It also implements the IAnchor interface to provide an anchor ID. This entity will be created and filled in the MapDataDownloader. It contains convenience methods to add and remove members of the lists more easily. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapPort : MapElement, IPort
    {

        /**
         * <p>Represents the node in which this port is contained.</p>
         * <p>This can be any value. It will be managed with the Node property.</p>
         * 
         * 
         */
        private INode node = null;

        /**
         * <p>Represents the link that connects this port to something else.</p>
         * <p>This can be any value. It will be managed with the Link property.</p>
         * 
         * 
         */
        private IList<ILink> links = null;

        /**
         * <p>Represents the position of the port.</p>
         * <p>This can be any value. It will be managed with the Position property.</p>
         * 
         * 
         */
        private Coordinates position = null;

        /**
         * <p>Represents the size of the port.</p>
         * <p>This can be any value. It will be managed with the Size property.</p>
         * 
         * 
         */
        private Dimension size = null;

        /**
         * <p>Represents the oriantation of the port.</p>
         * <p>This can be any value. It will be managed with the Orientation property.</p>
         * 
         * 
         */
        private string orientation = null;

        /**
         * <p>Represents the minimum size for this port.</p>
         * <p>This can be any value. It will be managed with the MinimalSize property.</p>
         * 
         * 
         */
        private Dimension minimalSize = null;

        /**
         * <p>This is the property for the node field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the node field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the node field to the value.</li>
         * </ul>
         * 
         * 
         */
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

        /**
         * <p>This is the property for the link field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the link field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the link field to the value.</li>
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
         * <p>This is the property for the orientation field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the orientation field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the orientation field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Orientation
        {
            get
            {
                return orientation;
            }
            set
            {
                orientation = value;
            }
        }

        /**
         * <p>This is the property for the name of the element type that this map port belongs to. It will be backed by the ElementType.Name property.</p>
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
        public MapPort()
        {
            // empty
        }

        /**
         * Generates the anchor ID. Simply returns "port" + Id value.
         * 
         * 
         * @return the anchor ID
         */
        public string GetAnchorId()
        {
            return "port" + Id;
        }
    }
}