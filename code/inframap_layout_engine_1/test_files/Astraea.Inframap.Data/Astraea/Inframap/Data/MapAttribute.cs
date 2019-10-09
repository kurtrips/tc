// MapAttribute.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// Mock MapAttribute class for testing.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapAttribute : IAttribute
    {
        /// <summary>
        /// The id of attribute
        /// </summary>
        private long id;

        /// <summary>
        /// Id of attribute
        /// </summary>
        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// The scopeId
        /// </summary>
        private long ownerId;

        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        public long OwnerId
        {
            get { return ownerId; }
            set { ownerId = value; }
        }

        /// <summary>
        /// The owner type
        /// </summary>
        private string ownerType;

        /// <summary>
        /// Gets or sets the owner type
        /// </summary>
        public string OwnerType
        {
            get { return ownerType; }
            set { ownerType = value; }
        }

        /// <summary>
        /// Name of attribute
        /// </summary>
        private string name;

        /// <summary>
        /// Gets and sets the name of the attribute
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// <p>Represents the integer value if attribute type is int.</p>
        /// </summary>
        private int intValue;

        /// <summary>
        /// <p>Represents the string value if attribute type is a string.</p>
        /// </summary>
        private string stringValue;

        /// <summary>
        /// The double value of the attribute
        /// </summary>
        private double doubleValue;

        /// <summary>
        /// Gets or sets the double value of the attribute
        /// </summary>
        public double DoubleValue
        {
            get { return doubleValue; }
            set { doubleValue = value; }
        }

        /// <summary>
        /// The date value of the attribute
        /// </summary>
        private DateTime dateTimeValue;

        /// <summary>
        /// Gets or sets the date value of the attribute
        /// </summary>
        public DateTime DateTimeValue
        {
            get { return dateTimeValue; }
            set { dateTimeValue = value; }
        }

        /// <summary>
        /// The type of attribute
        /// </summary>
        private string type;

        /// <summary>
        /// <p>The type of attribute.</p>
        /// </summary>
        /// <value>The type of attribute.</value>
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

        /// <summary>
        /// <p>The integer value of attribute.</p>
        /// </summary>
        /// <value>The integer value of attribute.</value>
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

        /// <summary>
        /// <p>The string value of attribute.</p>
        /// </summary>
        /// <value>The string value of attribute.</value>
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

        /// <summary>
        /// Creates MapAttribute instance.
        /// </summary>
        public MapAttribute()
        {
        }
    }
}
