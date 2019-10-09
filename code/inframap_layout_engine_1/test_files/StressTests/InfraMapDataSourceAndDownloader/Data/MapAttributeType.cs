
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents an attribute type that is available to a map data. It contains an ID, name, type of the data an attribute will hold, scope ID, and what element type it is. It implements the IAnchor interface to provide an anchor ID. This entity will be created and filled in the MapDataDownloader. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapAttributeType
    {

        /**
         * <p>Represents the primary key of this map entity.</p>
         * <p>This can be any value. It will be managed with the Id property.</p>
         * 
         * 
         */
        private long id = -1;

        /**
         * <p>Represents the type of the data.</p>
         * <p>This can be any value. It will be managed with the DataType property.</p>
         * 
         * 
         */
        private string dataType = null;

        /**
         * <p>Represents name of the attribute type.</p>
         * <p>This can be any value. It will be managed with the Name property.</p>
         * 
         * 
         */
        private string name = null;

        /**
         * <p>Represents the Id of the scope.</p>
         * <p>This can be any value. It will be managed with the OwnerId property.</p>
         * 
         * 
         */
        private int scopeId = -1;

        /**
         * <p>Represents the element type this attribute belongs to.</p>
         * <p>This can be any value. It will be managed with the ElementType property.</p>
         * 
         * 
         */
        private MapElementType elementType = null;

        /**
         * <p>This is the property for the id field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the id field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the id field to the value.</li>
         * </ul>
         * 
         * 
         */
        public long Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /**
         * <p>This is the property for the dataType field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the dataType field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the dataType field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }

        /**
         * <p>This is the property for the name field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the name field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the name field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /**
         * <p>This is the property for the scopeId field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the scopeId field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the scopeId field to the value.</li>
         * </ul>
         * 
         * 
         */
        public int OwnerId
        {
            get
            {
                return scopeId;
            }
            set
            {
                scopeId = value;
            }
        }

        /**
         * <p>This is the property for the elementType field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the elementType field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the elementType field to the value.</li>
         * </ul>
         * 
         * 
         */
        public MapElementType ElementType
        {
            get
            {
                return elementType;
            }
            set
            {
                elementType = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapAttributeType()
        {
            // empty
        }

        /**
         * Generates the anchor ID. Simply returns "attributetype" + Id value.
         * 
         * 
         * @return the anchor ID
         */
        public string GetAnchorId()
        {
            return "attributetype" + id;
        }
    }
}