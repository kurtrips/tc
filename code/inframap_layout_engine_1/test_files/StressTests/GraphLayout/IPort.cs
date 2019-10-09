using System.Collections.Generic;
namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * This interface declares the properties for a port. Links may connect via ports, ports have containing nodes. However
     * in this version, all the graph layout algorithms will ignore the concept of ports, links are treated as if they are
     * connected to nodes directly.
     * </p>
     * <p>
     * In this version Port class is provided as the sole implementation for this interface.
     * </p>
     * <p>
     * Thread safety: The implementations of this interface are not required to be thread safe.
     * </p>
     * 
     * 
     */
    public interface IPort
    {

        /**
         * <p>
         * This is the property for the position of the port.(the coordinates for the up-left corner).
         * The getter's return value may be any Coordinates value including null in which case the position is still not calculated out.
         * The setter can set any Coordinates instance including null.
         * </p>
         * 
         */
         TopCoder.Graph.Layout.Coordinates Position
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property for the size of the port.
         * The getter's return value may be any Dimension value including null in which case the size is still not calculated out.
         * The setter can set any Dimension instance including null.
         * </p>
         * 
         */
         TopCoder.Graph.Layout.Dimension Size
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property for the orientation of the port, that is, which side of the node the port is on(north, south, east or
         * west)
         * The getter's return value may only be one of "north", "south", "east" or "west".
         * The setter can only set "north", "south", "east" or "west".
         * </p>
         * 
         * 
         * @throws ArgumentException If the setter sets the value other than "north", "south", "east" or "west".
         */
         string Orientation
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the layer of the port. Elements(nodes, links and ports) are grouped into a stack of layers from
         * 1 to N.(The one at the bottom has layer 1)
         * The getter's return value is a positive integer.
         * </p>
         * 
         */
         int Layer
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the class of the port. 
         * The getter's return value may be any string including null and empty string.
         * </p>
         * 
         */
         string Class
        {
            get;
        }


        /**
         * <p>
         * This is the property getter for the id of the port. 
         * The getter's return value must be a non-negative long value.
         * </p>
         * 
         */
         long Id
        {
            get;
        }


        /**
         * <p>
         * This is the property getter for the name of the port. 
         * The getter's return value may be any string including null and empty string.
         * </p>
         * 
         */
         string Name
        {
            get;
        }


        /**
         * <p>
         * This is the property getter for the node contained in the port. 
         * The getter's return value may be any INode instance including null.
         * </p>
         * 
         */
         TopCoder.Graph.Layout.INode Node
        {
            get;
        }


        /**
         * <p>
         * This is the property getter for the attributes of the port.
         * Null will never be returned, an empty list will be returned if the port has no attributes. The elements contained in the list
         * can't be null. There should not be any duplicate elements(equal references) in the list.
         * </p>
         * 
         */
         IList<IAttribute> Attributes
        {
            get;
        }


        /**
         * <p>
         * This is the property getter for the link which connects to nodes via this port.
         * The getter's return value may be any ILink instance including null.
         * </p>
         * 
         */
         IList<ILink> Links
        {
            get;
        }


        /**
         * <p>
         * This is the property getter for the minimal size of the port. For any lay out algorithm not ignoring ports, the port's size
         * must be assigned to no less than this minimal size(both height and width).[But please note in this version all layout algorithms
         * will ignore ports totally]
         * The getter's return value may be any Dimension value including null in which case there is no requirement about
         * this port's minimal size.
         * </p>
         * 
         */
         TopCoder.Graph.Layout.Dimension MinimalSize
        {
            get;
            set;
        }


        /**
         * <p>
         * This is the property getter for the class id of the port. 
         * The getter's return value must be a non-negative long value.
         * </p>
         * 
         */
         long ClassId
        {
            get;
        }

    }
}

