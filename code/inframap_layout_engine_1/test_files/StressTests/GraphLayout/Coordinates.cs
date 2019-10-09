
namespace TopCoder.Graph.Layout
{

    /**
     * <p>
     * The instance of this class represents a single coordinates in a plane, this is the basic data structure used to
     * describe the positions of nodes, ports, labels and the paths of segments.
     * Please note in this component the X axis is horizontal pointing right, the Y axis is vertical pointing down. So the
     * origin(0,0) is the up-left corner of a graph if all the coordinates of elements are non-negative.
     * </p>
     * <p>
     * Thread safety: This class is not thread safe since all the fields are mutable and no synchronizations are performed
     * for the accesses to them.
     * </p>
     * 
     * 
     */
    public class Coordinates
    {

        /**
         * <p>
         * The x position for this coordinates.
         * It's initialized in the constructors and can be gotten through the getter and modified through the setter so It's mutable.
         * It can be any int value.
         * </p>
         * 
         * 
         */
        private int x;

        /**
         * <p>
         * The y position for this coordinates.
         * It's initialized in the constructors and can be gotten through the getter and modified through the setter so It's mutable.
         * It can be any int value.
         * </p>
         * 
         * 
         */
        private int y;

        /**
         * <p>
         * This is the property for the x field.
         * The getter simply returns the field directly. The return value may be any int value.
         * The setter simply sets the field directly. Any int value can be set.
         * </p>
         * 
         * 
         */
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                this.x = value;
            }
        }

        /**
         * <p>
         * This is the property for the y field.
         * The getter simply returns the field directly. The return value may be any int value.
         * The setter simply sets the field directly. Any int value can be set.
         * </p>
         * 
         * 
         */
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                this.y = value;
            }
        }

        /**
         * <p>
         * This constructor creates an origin.
         * Simply delegates to this(0, 0).
         * </p>
         * 
         * 
         */
        public Coordinates()
            : this(0, 0)
        {
            // your code here
        }

        /**
         * <p>
         * This constructor creates a coordinates located in specified position.
         * Simply sets all arguments to the associated fields.
         * </p>
         * 
         * 
         * 
         * @param x The x position for this coordinates. it can be any int value.
         * @param y The y position for this coordinates. it can be any int value.
         */
        public Coordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}