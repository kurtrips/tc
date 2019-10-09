
using System.Collections.Generic;
namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * This interface declares the properties for a node. A node can optionally have a parent node and child nodes,
     * but neither child nor parent are required. For any graph layout algorithm, children nodes will be laid out inside
     * its parent node and the label of a node should also be laid out inside that node, nodes stored in the same link
     * will be connected together.
     * </p>
     * <p>
     * In this version Node class is provided as the sole implementation for this interface.
     * </p>
     * <p>
     * Thread safety: The implementations of this interface are not required to be thread safe.
     * </p>
     * 
     * 
     */
    public interface INode
    {

        /**
         * <p>
         * This is the property for the position of the node.(the coordinates for the up-left corner).
         * The getter's return value may be any Coordinates value including null in which case the position is still not calculated out.
         * The setter can set any Coordinates instance including null.
         * </p>
         * 
         */
        Coordinates Position
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property for the size of the node.
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
         * This is the property getter for the layer of the node. Elements(nodes, links and ports) are grouped into a stack of layers from
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
         * This is the property getter for the label of the node. 
         * The getter's return value may be any ILabel instance including null(in which case this node has no label)
         * </p>
         * 
         */
        ILabel Label
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the parent of the node. 
         * The getter's return value may be any INode instance including null in which case the node has no parent.
         * </p>
         * 
         */
        INode Container
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for children of this node.
         * Null will never be returned, an empty list will be returned if the node has no children. The elements contained in the list
         * can't be null. There should not be any duplicate elements(equal references) in the list.
         * </p>
         * 
         * 
         */
        IList<INode> Occupants
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the class of the node. 
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
         * This is the property getter for the id of the node. 
         * The getter's return value must be a non-negative long value.
         * </p>
         * 
         */
        long Id
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the name of the node. 
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
         * This is the property getter for the attributes of the node.
         * Null will never be returned, an empty list will be returned if the link has no attributes. The elements contained in the list
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
         * This is the property getter for the ports which the links connecting this node will go through.
         * Null will never be returned, an empty list will be returned if the links of this node go through no ports. The elements
         * contained in the list can't be null. There should not be any duplicate elements(equal references) in the list.
         * </p>
         * 
         */
        IList<IPort> Ports
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the minimal size of the node. For any lay out algorithm the node's size must be assigned to no less than
         * this minimal size(both height and width).
         * The getter's return value may be any Dimension value including null in which case there is no requirement about
         * this node's minimal size.
         * </p>
         * 
         */
        Dimension MinimalSize
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the class id of the node. 
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