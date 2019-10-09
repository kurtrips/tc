using System.Collections.Generic;
namespace Astraea.Inframap.Data
{

    /**
     * <p>Represents an entity type that is available to a map data. It contains an ID, name, layer ID, and a list of map attributes. It implements the IAnchor interface to provide an anchor ID. This entity will be created and filled in the MapDataDownloader. It contains convenience methods to add and remove members of the lists more easily. This class is Serializable.</p>
     * 
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     * 
     */
    public class MapElementType
    {

        /**
         * <p>Represents the primary key of this map entity.</p>
         * <p>This can be any value. It will be managed with the Id property.</p>
         * 
         * 
         */
        private long id = -1;

        /**
         * <p>Represents the name of the element type.</p>
         * <p>This can be any value. It will be managed with the Name property.</p>
         * 
         * 
         */
        private string name = null;

        /**
         * <p>Represents the ID of the layer of the element type.</p>
         * <p>This can be any value. It will be managed with the Layer property.</p>
         * 
         * 
         */
        private int layer = -1;

        /**
         * <p>Represents attributes of the element type.</p>
         * <p>This can be any value. It will be managed with the Attributes property and the addAttribute and removeAttribute convenience methods.</p>
         * 
         * 
         */
        private IList<MapAttribute> attributes = new List<MapAttribute>();

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
         * <p>This is the property for the layer field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the layer field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the layer field to the value.</li>
         * </ul>
         * 
         * 
         */
        public int Layer
        {
            get
            {
                return layer;
            }
            set
            {
                layer = value;
            }
        }

        /**
         * <p>This is the property for the attributes field.</p>
         * 
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the attributes field</li>
         * </ul>
         * 
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the attributes field to the value.</li>
         * </ul>
         * 
         * 
         */
        public IList<MapAttribute> Attributes
        {
            get
            {
                return attributes;
            }
            set
            {
                attributes = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        public MapElementType()
        {
            // empty
        }

        /**
         * Convenience method to add a MapAttribute to the attributes list. Null parameters are not allowed.
         * If the list is null, just create a new instance and add to it.
         * 
         * 
         * @param attribute The MapAttribute to add to the attributes list
         * @throws ArgumentNullException If attribute is null
         */
        public void AddAttribute(MapAttribute attribute)
        {
            attributes.Add(attribute);
        }

        /**
         * Convenience method to remove the first occurence of the MapAttribute from the attributes list. Null parameters are not allowed.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param attribute The MapAttribute to remove from the attributes list
         * @throws ArgumentNullException If attribute is null
         */
        public bool RemoveAttribute(MapAttribute attribute)
        {
            return attributes.Remove(attribute);
        }

        /**
         * Convenience method to remove the first occurence of the MapAttribute with the given Id from the attributes list.
         * If the list is null, just return false.
         * 
         * 
         * @return True if removed. False otherwise
         * @param attributeId The Id of the MapAttribute to remove from the attributes list
         */
        public bool RemoveAttribute(long attributeId)
        {
            foreach (MapAttribute attribute in attributes)
            {
                if (attribute.Id == attributeId)
                {
                    attributes.Remove(attribute);
                    return true;
                }
            }
            return false;
        }

        /**
         * Generates the anchor ID. Simply returns "elementtype" + Id value.
         * 
         * 
         * @return the anchor ID
         */
        public string GetAnchorId()
        {
            return "elementtype" + id;
        }
    }
}