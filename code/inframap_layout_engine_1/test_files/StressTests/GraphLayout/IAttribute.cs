
namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * This interface declares the properties for an attribute. The attribute is used to provide extra info for nodes, links and
     * ports however it doesn't have any effect on how to lay out a graph.
     * </p>
     * <p>
     * In this version Attribute class is provided as the sole implementation for this interface.
     * </p>
     * <p>
     * Thread safety: The implementations of this interface are not required to be thread safe.
     * </p>
     * 
     * 
     */
    public interface IAttribute
    {

        /**
         * <p>
         * This is the property getter for the name of the attribute.
         * The return value can be any string including null and empty string.
         * </p>
         * 
         */
        string Name
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the type of the attribute.
         * The return value can only be "int", "double", "string" or "datetime".
         * </p>
         * 
         */
        string Type
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the int value of the attribute.
         * If the type of this attribute is "int", the return value may be any int value; otherwise InvalidOperationException
         * should be thrown.
         * </p>
         * 
         * 
         * @throws InvalidOperationException if the type for this attribute is not "int".
         */
        int IntValue
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the double value of the attribute.
         * If the type of this attribute is "double", the return value may be any double value; otherwise InvalidOperationException
         * should be thrown.
         * </p>
         * 
         * 
         * @throws InvalidOperationException if the type for this attribute is not "double".
         */
        double DoubleValue
        {
            get;
        }

        /**
         * <p>
         * This is the property getter for the string value of the attribute.
         * If the type of this attribute is "string", the return value may be any string value including null or empty string; otherwise
         * InvalidOperationException should be thrown.
         * </p>
         * 
         * 
         * @throws InvalidOperationException if the type for this attribute is not "string".
         */
        string StringValue
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the DateTime value of the attribute.
         * If the type of this attribute is "datetime", the return value may be any DateTime value; otherwise
         * InvalidOperationException should be thrown.
         * </p>
         * 
         * 
         * @throws InvalidOperationException if the type for this attribute is not "datetime".
         */
        System.DateTime DateTimeValue
        {
            get;
        }
    }
}