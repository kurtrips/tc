
using System;
using TopCoder.Graph.Layout;
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents attributes of map elements or element types. It implements IAttribute to expose a name, type, and a value fields that are filled depending on the type. It also contains the primary key, the id and type of the owner of this attribute (it can be a node, port, link, or element type). It also implements the IAnchor interface to provide an anchor ID. This entity will be created and filled in the MapDataDownloader. This class is Serializable. It contains convenience methods to locate attributes in this map elements by their name or type.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapAttribute : IAttribute
    {

        /**
         * <p>Represents the primary key of this map entity.</p>
         * <p>This can be any value. It will be managed with the Id property.</p>
         * 
         * 
         */
        private long id = -1;

        /**
         * <p>Represents the primary ID of the owner of this attribute. Depends on the scope type, it will map to Node.id, Link.id, Port.id or ElementType.id.</p>
         * <p>This can be any value. It will be managed with the OwnerId property.</p>
         * 
         * 
         */
        private long scopeId = -1;

        /**
         * <p>Represents the type of the owner. Will be node, link, port or element type.</p>
         * <p>This can be any value. It will be managed with the OwnerType property.</p>
         * 
         * 
         */
        private string scopeType = null;

        /**
         * <p>Represents the type of the attribute. Will be int, double, datetime, or string.</p>
         * <p>This can be any value. It will be managed with the Type property.</p>
         * 
         */
        private string type = null;

        /**
         * <p>Represents the name of the attribute.</p>
         * <p>This can be any value. It will be managed with the Name property.</p>
         * 
         * 
         */
        private string name = null;

        /**
         * <p>Represents the integer value if attribute type is int.</p>
         * <p>This can be any value. It will be managed with the IntValue property.</p>
         * 
         * 
         */
        private int intValue = 0;

        /**
         * <p>Represents the string value if attribute type is a string.</p>
         * <p>This can be any value. It will be managed with the StringValue property.</p>
         * 
         * 
         */
        private string stringValue = null;

        /**
         * <p>Represents the date time value if attribute type is a date time.</p>
         * <p>This can be any value. It will be managed with the DateTimeValue property.</p>
         * 
         * 
         */
        private DateTime dateTimeValue = DateTime.MinValue;

        /**
         * <p>Represents the double value if attribute type is a double.</p>
         * <p>This can be any value. It will be managed with the DoubleValue property.</p>
         * 
         * 
         */
        private double doubleValue = 0.0;

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
        public long OwnerId
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
         * <p>This is the property for the scopeType field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the scopeType field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the scopeType field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string OwnerType
        {
            get
            {
                return scopeType;
            }
            set
            {
                scopeType = value;
            }
        }

        /**
         * <p>This is the property for the type field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the type field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the type field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
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
         * <p>This is the property for the intValue field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the intValue field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the intValue field to the value.</li>
         * </ul>
         * 
         * 
         */
        public int IntValue
        {
            get
            {
                return intValue;
            }
            set
            {
                intValue = value;
            }
        }

        /**
         * <p>This is the property for the stringValue field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the stringValue field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the stringValue field to the value.</li>
         * </ul>
         * 
         * 
         */
        public string StringValue
        {
            get
            {
                return stringValue;
            }
            set
            {
                stringValue = value;
            }
        }

        /**
         * <p>This is the property for the dateTimeValue field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the dateTimeValue field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the dateTimeValue field to the value.</li>
         * </ul>
         * 
         * 
         */
        public DateTime DateTimeValue
        {
            get
            {
                return dateTimeValue;
            }
            set
            {
                dateTimeValue = value;
            }
        }

        /**
         * <p>This is the property for the doubleValue field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the doubleValue field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the doubleValue field to the value.</li>
         * </ul>
         * 
         * 
         */
        public double DoubleValue
        {
            get
            {
                return doubleValue;
            }
            set
            {
                doubleValue = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapAttribute()
        {
            // empty
        }

        /**
         * Generates the anchor ID. Simply returns "attribute" + Id value.
         * 
         * 
         * @return the anchor ID
         */
        public string GetAnchorId()
        {
            return "attribute" + id;
        }
    }
}