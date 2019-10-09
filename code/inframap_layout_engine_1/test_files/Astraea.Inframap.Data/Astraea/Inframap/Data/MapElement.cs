// MapElement.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// <p>This is an abstract class that provides common properties to the MapNode, MapPort, and MapLink. It contains
    /// the Id, name, type of element this is, the layer Id, and the list of attributes associated with this element. It
    /// contains convenience methods to add and remove members of the lists more easily. This class is Serializable.</p>
    ///  <p>Thread Safety: This class is mutable and not thread-safe.</p>
    /// </summary>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [Serializable]
    public abstract class MapElement
    {
        /// <summary>
        /// The id of the element.
        /// </summary>
        long id = -1;

        /// <summary>
        /// The name of the element.
        /// </summary>
        string name = null;

        /// <summary>
        /// Sets or gets the id of the element
        /// </summary>
        /// <value>the id of the element</value>
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Sets or gets the name of the element
        /// </summary>
        /// <value>the name of the element</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns attributes with given name. 
        /// This is a mock implementation of the GetAttributesByName function.
        /// </summary>
        /// <param name="name">The name of the attribute to find.</param>
        /// <returns>
        /// For LinkA: 3 attributes, 2 of type int and 1 of type string.
        /// For LinkB: No attributes.
        /// </returns>
        public IList<IAttribute> GetAttributesByName(string name)
        {
            IList<IAttribute> ret = new List<IAttribute>();

            foreach (IAttribute attr in Attributes)
            {
                if (attr.Name == "path")
                {
                    ret.Add((MapAttribute)attr);
                }
            }

            return ret;
        }

        /// <summary>
        /// Mock for Attributes property.
        /// </summary>
        IList<IAttribute> attributes = new List<IAttribute>();
        public IList<IAttribute> Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        /// <summary>
        /// Mock for ElementType property
        /// </summary>
        MapElementType elementType = null;
        public MapElementType ElementType
        {
            get { return elementType; }
            set { elementType = value; }
        }
    }
}
