
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents a map style that is available to a map data. It implements IStyle to expose a property name and value, classId, and the name of the element type it represents. It also contains an ID and the ID and name of the element type it represents. This entity will be created and filled in the MapDataDownloader. This class is Serializable.</p>
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     */
    public class MapStyle
    {

        /**
         * <p>Represents the primary key of this map entity.</p>
         * <p>This can be any value. It will be managed with the Id property.</p>
         * 
         * 
         */
        private long id = -1;

        /**
         * <p>Represents the property of this style.</p>
         * <p>This can be any value. It will be managed with the Property property.</p>
         * 
         * 
         */
        private string property = null;

        /**
         * <p>Represents the value of the style property.</p>
         * <p>This can be any value. It will be managed with the Value property.</p>
         * 
         * 
         */
        private string value = null;

        /**
         * <p>Represents the Id of the element type associated with this style.</p>
         * <p>This can be any value. It will be managed with the ElementTypeId property.</p>
         * 
         * 
         */
        private long elementTypeId = -1;

        /**
         * <p>Represents name of the element type that this style belongs to.</p>
         * <p>This can be any value. It will be managed with the ElementTypeName property.</p>
         * 
         * 
         */
        private string elementTypeName = null;

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
         * <p>This is the property for the property field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the property field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the property field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Property
        {
            get
            {
                return property;
            }
            set
            {
                property = value;
            }
        }

        /**
         * <p>This is the property for the value field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the value field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the value field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        /**
         * <p>This is the property for the elementTypeId field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the elementTypeId field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the elementTypeId field to the value.</li>
         * </ul>
         * 
         * 
         */
        public long ElementTypeId
        {
            get
            {
                return elementTypeId;
            }
            set
            {
                elementTypeId = value;
            }
        }

        /**
         * <p>This is the property for the elementTypeName field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the elementTypeName field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the elementTypeName field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string ElementTypeName
        {
            get
            {
                return elementTypeName;
            }
            set
            {
                elementTypeName = value;
            }
        }

        /**
         * <p>This is the property for the name of the element type that this style belongs to. It will be backed by the ElementTypeName property.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the ElementTypeName Property</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the ElementTypeName Property to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Class
        {
            get
            {
                return ElementTypeName;
            }
            set
            {
                ElementTypeName = value;
            }
        }

        /**
         * <p>This is the property for the class ID. It will be backed by the ElementTypeId property.</p>
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the ElementTypeId Property</li>
         * </ul>
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the ElementTypeId Property to the value.</li>
         * </ul>
         * <p></p>
         * 
         */
        public long ClassId
        {
            get
            {
                return ElementTypeId;
            }
            set
            {
                ElementTypeId = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapStyle()
        {
            // empty
        }
    }
}