using TopCoder.Graph.Layout;
using System.Collections.Generic;
namespace Astraea.Inframap.Data
{

    /**
     * <p>This is an abstract class that provides common properties to the MapNode, MapPort, and MapLink. It contains the Id, classId, name, type of element this is, the layer Id, and the list of attributes associated with this element. It contains convenience methods to add and remove members of the lists more easily. This class is Serializable.</p>
     * <p>Thread Safety: This class is mutable and not thread-safe.</p>
     * 
     */
    public abstract class MapElement
    {

        /**
         * <p>Represents the primary key of this map entity.</p>
         * <p>This can be any value. It will be managed with the Id property.</p>
         * 
         * 
         */
        private long id = -1;

        /**
         * <p>Represents name of the element.</p>
         * <p>This can be any value. It will be managed with the Name property.</p>
         * 
         * 
         */
        private string name = null;

        /**
         * <p>Represents element type that this element belongs to.</p>
         * <p>This can be any value. It will be managed with the ElementType property.</p>
         * 
         * 
         */
        private MapElementType elementType = null;

        /**
         * <p>Represents the layer of the element.</p>
         * <p>This can be any value. It will be managed with the Layer property.</p>
         * 
         * 
         */
        private int layer = -1;

        /**
         * <p>Represents the attributes of the element.</p>
         * <p>This can be any value. It will be managed with the Attributes property and the addAttribute and removeAttribute convenience methods.
         * It will also be accessed by the GetAttibutesByName and GetAttibutesByType methods.</p>
         * 
         * 
         */
        private IList<IAttribute> attributes = new List<IAttribute>();

        /**
         * <p>This is the property for the id field.</p>
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the id field</li>
         * </ul>
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the id field to the value.</li>
         * </ul>
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
        public IList<IAttribute> Attributes
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
         * <p>This is the property for the class ID. It will be backed by the ElementType.Id property.</p>
         * <p><strong>Get:</strong></p>
         * <ul type="disc">
         * <li>Simply return the value of the ElementType.Id Property</li>
         * </ul>
         * <p><strong>Set:</strong></p>
         * <ul type="disc">
         * <li>Set the ElementType.Id Property to the value.</li>
         * </ul>
         * <p></p>
         * 
         */
        public long ClassId
        {
            get
            {
                return ElementType.Id;
            }
            set
            {
                ElementType.Id = value;
            }
        }

        /**
         * Default constructor. Does nothing.
         * 
         */
        protected MapElement()
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
            return attributes.Remove(attribute) ;
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
         * Gets attributes of this map entity that have the given name. It will return a list of 0 to many MapAttribute objects.
         * It will never return null or a list with null elements.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>It should simply iterate over the attributes list and recover all that have the given name</li>
         * </ul>
         * 
         * 
         * @return a list of 0 to many MapAttribute objects
         * @param name Name of the attributes to retrieve
         */
        public IList<IAttribute> GetAttributesByName(string name)
        {
            IList<IAttribute> attrs = new List<IAttribute>();
            foreach (MapAttribute attribute in attributes)
            {
                if (string.Compare(attribute.Name, name) == 0)
                {
                    attrs.Add(attribute);
                }
            }
            return attrs;
        }

        /**
         * Gets attributes of this map entity that have the given type. It will return a list of 0 to many MapAttribute objects.
         * It will never return null or a list with null elements.
         * <p><strong>Implementation Notes</strong></p>
         * <ul type="disc">
         * <li>It should simply iterate over the attributes list and recover all that have the given type</li>
         * </ul>
         * 
         * 
         * @return a list of 0 to many MapAttribute objects
         * @param type Type of the attributes to retrieve
         */
        public IList<IAttribute> GetAttributesByType(string type)
        {
            IList<IAttribute> attrs = new List<IAttribute>();
            foreach (MapAttribute attribute in attributes)
            {
                if (string.Compare(attribute.Type, type) == 0)
                {
                    attrs.Add(attribute);
                }
            }
            return attrs;
        }
    }
}