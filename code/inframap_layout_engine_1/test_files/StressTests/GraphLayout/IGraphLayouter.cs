namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * This interface declares the method that should be implemented for all the graph layouters. Given a IGraph instance,
     * different implementations of this interface may use different algorithms to lay out the graph, the result is still the input
     * reference but all the sizes, positions of the elements and the link paths should have been calculated out. The mutated
     * input will be returned to the caller.
     * </p>
     * <p>
     * In this version, two different algorithms are provided, one focused on the time spent and the other focused on the link crossings,
     * these algorithms are implemented in TimeFocusedGraphLayouter and LinkCrossingsFocusedGraphLayouter, respectively.
     * </p>
     * <p>
     * Thread safety: The implementations of this interface are not required to be thread safe.
     * </p>
     * 
     * 
     * The graph layouter will use
     * all the data structures defined
     * in this package(and creates Segment,
     * Coordinates, Dimension) to layout the
     * graph. The relationships are
     * simplified to a single interface to
     * package dependency so it doesn't
     * mess the diagram.
     */
    public interface IGraphLayouter
    {
        /**
         * <p>
         * This method calculates the layout of a graph, fill all the elements' layout related properties then return the input
         * reference.
         * </p>
         * <p>
         * Different implementations may use different algorithms to lay out the graph, but the minimum requirement must be met that:
         * 1. All nodes and links in the input will be contained in the output.(This can be automatically done as long as we
         * don't modify the nodes and links in the graph.)
         * 2. Children nodes will be laid out inside its parent node.
         * 3. There will be spacing between nodes.
         * 4. Labels will be contained inside the nodes they belong to.
         * </p>
         * <p>
         * Any non-null input argument should be acceptable since we should assume the input is valid and not perform
         * any graph validation as required in RS. But if really an invalid graph is passed in, the implementations should
         * still perform the layout process, then naturally either it will generate a meaningless output or it will cause an exception 
         * which should then be wrapped to LayoutException.
         * Invalid nput graph includes:
         * 1. Some elements in IGraph#Nodes reference Ports which can't be found in Ports of the graph.
         * 2. Some elements in IGraph#Links reference Nodes or Ports which can't be found in Nodes or Ports of the graph.
         * 3. Some elements in IGraph#Ports reference Nodes or Links which can't be found in Nodes or Links of the graph.
         * 4. Some nodes'parent or children can't be found in the Nodes list.
         * 5. For any pair of nodes A and B, The parent of A is B, but A can't be found in B's children list, or the parent of A
         * is not B, but A is found in B's children list.
         * 6. There are any cycles in the nodes graph, i.e. some node can reach itself by visiting the child one or more times.
         * 7. Any label is held by more than one nodes.
         * Please note the graph can be empty(doesn't contain any nodes), the layouters should accept them as valid
         * params, the Size of The graph can be set to any value including null.
         * </p>
         * <p>
         * If any errors occur during the layout process, the exception should be wrapped to LayoutException in the following way:
         * public class SomeGraphLayouter
         * {
         *   private string _instanceVariable = "Some Variable";
         *  
         *   /// Throws LayoutException for any errors except for null input.
         *   public IGraph Layout(IGraph input)
         *   {
         *     if (input == null) throw new ArgumentNullException(...);
         * 
         *     #region Local Variables
         *     object someLocalVariable = null;
         *     #endregion Local Variables
         *  
         *     #region Outer Try
         *     try 
         *     {
         *       // Do the real layout process...
         *     }
         *     #endregion Outer Try
         *     #region Catch
         *     catch (Exception exception)
         *     {
         *       LayoutException le = null;
         *       if(exception is LayoutException)
         *       {
         *         le = (LayoutException)exception;
         *       }
         *       else
         *       {
         *         le = new LayoutException("Message", exception);
         *       }
         * 
         *       MethodState ms = le.PinMethod(
         *         "SomeNamespace.SomeClass.SomeMethod", 
         *         exception.StackTrace);
         *  
         *       ms.AddInstanceVariable("_instanceVariable", instanceVariable);
         *       ms.AddParemeter("input", input);
         *       ms.AddLocalVariable("someLocalVariable", someLocalVariable);
         *       ms.Lock();
         *  
         *       throw le;
         *     }
         *     #endregion Catch
         *     #region Finally
         *     finally
         *     {
         *       // Clean up!
         *     }
         *     #endregion Finally
         *   }
         * }
         * </p>
         * 
         * 
         * 
         * 
         * @param input The input graph, can't be null.
         * @return Exactly the same reference with the input graph, but all the properties for the elements of the graph should have been calculated out.
         * @throws LayoutException If any other errors occur during the layout process.(except for null input argument issue)
         * @throws ArgumentNullException If the input argument is null.
         */
        IGraph Layout(IGraph input);
    }
}


