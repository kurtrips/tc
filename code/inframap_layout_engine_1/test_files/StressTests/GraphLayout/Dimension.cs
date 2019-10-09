
namespace TopCoder.Graph.Layout
{
    /**
     * <p>
     * The instance of this class describe a rectangular shape, this is the basic data structure used to
     * describe the size (and minimal size) of nodes, ports, labels and the whole graph.
     * </p>
     * <p>
     * Thread safety: This class is not thread safe since all the fields are mutable and no synchronizations are performed
     * for the accesses to them.
     * </p>
     * 
     * 
     */
    public class Dimension
    {

        /**
         * <p>
         * The height of the rectangular area.
         * It's initialized in the constructors and can be gotten through the getter and modified through the setter so It's mutable.
         * It will alwasy be positive.
         * </p>
         * 
         * 
         */
        private int height;

        /**
         * <p>
         * The width of the rectangular area.
         * It's initialized in the constructors and can be gotten through the getter and modified through the setter so It's mutable.
         * It will alwasy be positive.
         * </p>
         * 
         * 
         */
        private int width;

        /**
         * <p>
         * This is the property for the height field.
         * The getter simply returns the field directly. The return value are sure to be positive.
         * The setter simply sets the field directly after performing checkings.
         * </p>
         * 
         * 
         * 
         * @throws ArgumentOutOfRangeException If we set a non-positive value to the field.
         */
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                this.width = value;
            }
        }

        /**
         * <p>
         * This is the property for the width field.
         * The getter simply returns the field directly. The return value are sure to be positive.
         * The setter simply sets the field directly after performing checkings.
         * </p>
         * 
         * 
         * 
         * @throws ArgumentOutOfRangeException If we set a non-positive value to the field.
         */
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                this.width = value;
            }
        }

        /**
         * <p>
         * This constructor creates a dimension instance with height = width = 1.
         * Simply delegates to this(1, 1).
         * </p>
         * 
         * 
         */
        public Dimension()
            : this(1, 1)
        {
            // empty
        }

        /**
         * <p>
         * This constructor creates a dimension instance with specified height and width.
         * Simply sets all arguments to the associated fields using the setters properties.
         * </p>
         * 
         * 
         * 
         * @param height The height of the rectangular area. Must be positive.
         * @param width The width of the rectangular area. Must be positive.
         * @throws ArgumentOutOfRangeException If arguments height or width is not positive.
         */
        public Dimension(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        /**
         * <p>
         * This constructor creates a dimension instance with the same height and width of the argument.
         * Simply perform non-null checking then sets the arguments' height and width to the internal fields.
         * </p>
         * 
         * 
         * 
         * @param dimension The dimension providing the height and width info. Can't be null.
         * @throws ArgumentNullException if the dimension argument is null.
         */
        public Dimension(Dimension dimension)
            : this(dimension.Height, dimension.width)
        {
            // empty
        }
    }
}