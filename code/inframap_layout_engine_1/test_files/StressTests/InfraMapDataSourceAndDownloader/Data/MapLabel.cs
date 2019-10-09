using TopCoder.Graph.Layout;
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents a text label of a node. It implements ILabel. It has a position, size, and minimal size. This entity will be created and filled in the MapDataDownloader. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapLabel : ILabel
    {

        /**
         * <p>Represents the position of the label.</p>
         * <p>This can be any value. It will be managed with the Position property.</p>
         * 
         * 
         */
        private Coordinates position;

        /**
         * <p>Represents the size of the label.</p>
         * <p>This can be any value. It will be managed with the Size property.</p>
         * 
         * 
         */
        private Dimension size;

        /**
         * <p>Represents the text in the label.</p>
         * <p>This can be any value. It will be managed with the Text property.</p>
         * 
         * 
         */
        private string text;

        /**
         * <p>Represents the minimum size for this label.</p>
         * <p>This can be any value. It will be managed with the MinimalSize property.</p>
         * 
         * 
         */
        private Dimension minimalSize;

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
         * <p>This is the property for the text field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the text field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the text field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
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
        public MapLabel()
        {
            // empty
        }
    }
}