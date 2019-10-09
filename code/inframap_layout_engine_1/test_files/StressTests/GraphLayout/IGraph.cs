
using System.Collections.Generic;
namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * This interface declares the properties for a graph. A graph is described as a set of nodes, links and ports, the graph
     * layout algorithm will use the instance of this interface as the input, calculate the positions and size (and path for
     * links) for the elements(ports are ignored in this version), then the size of the whole graph is also calculated out so
     * the graph's area can contain all its elements. Please note a graph's position(the coordinates for the up-left corner)
     * will always be deemed as the origin(0,0).
     * </p>
     * <p>
     * In this version Graph class is provided as the sole implementation for this interface.
     * </p>
     * <p>
     * Thread safety: The implementations of this interface are not required to be thread safe.
     * </p>
     * 
     * 
     */
    public interface IGraph
    {

        /**
         * <p>
         * This is the property for the size of the graph.
         * The getter's return value may be any Dimension value including null in which case the size is still not calculated out.
         * The setter can set any Dimension instance including null.
         * </p>
         * 
         */
         TopCoder.Graph.Layout.Dimension Size
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the nodes in this graph.
         * Null will never be returned, an empty list will be returned if the graph has no nodes. The elements contained in the list
         * can't be null. There should not be any duplicate elements(equal references) in the list.
         * </p>
         * 
         */
         IList<INode> Nodes
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the links in this graph.
         * Null will never be returned, an empty list will be returned if the graph has no links. The elements contained in the list
         * can't be null. There should not be any duplicate elements(equal references) in the list.
         * </p>
         * 
         */
         IList<ILink> Links
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the ports in this graph.
         * Null will never be returned, an empty list will be returned if the graph has no ports. The elements contained in the list
         * can't be null. There should not be any duplicate elements(equal references) in the list.
         * </p>
         * 
         */
         IList<IPort> Ports
        {
            get;
        }
    }


}