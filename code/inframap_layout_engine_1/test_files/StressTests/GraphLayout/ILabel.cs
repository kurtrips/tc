
namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * This interface declares the properties for a node label. The lebel will record some text and must be contained
     * inside the node it belongs to when lay out the graph.
     * </p>
     * <p>
     * In this version Label class is provided as the sole implementation for this interface.
     * </p>
     * <p>
     * Thread safety: The implementations of this interface are not required to be thread safe.
     * </p>
     * 
     * 
     */
    public interface ILabel
    {

        /**
         * <p>
         * This is the property for the position of the label(the coordinates for the up-left corner).
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
         * This is the property for the size of the label.
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
         * This is the property getter for the text of the label.
         * The return value can be any string including null and empty string.
         * </p>
         * 
         */
         string Text
        {
            get;
            set;
        }

        /**
         * <p>
         * This is the property getter for the minimal size of the label. For any lay out algorithm the label's size must be assigned to no less than
         * this minimal size(both height and width).
         * The getter's return value may be any Dimension value including null in which case there is no requirement about
         * this label's minimal size.
         * </p>
         * 
         */
         TopCoder.Graph.Layout.Dimension MinimalSize
        {
            get;
            set;
        }
    }


}