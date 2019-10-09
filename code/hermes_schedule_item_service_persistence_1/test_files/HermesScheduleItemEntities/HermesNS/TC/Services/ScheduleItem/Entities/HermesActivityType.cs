// HermesActivityType.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TopCoder.Services.WCF.ScheduleItem.Entities;
using HermesNS.TC.Services.AuditTrail;
using TopCoder.Util.Indexing;

namespace HermesNS.TC.Services.ScheduleItem.Entities
{
    /// <summary>
    /// <p>This is a concrete implementation of the ActivityTypeBase that uses the concrete versions of the other base
    /// classes, such as HermesActivityType. For the Id, it uses a Guid in the form of a string.</p>  <p>All properties
    /// are backed by the Indexing components SearchableValueList, as this class implements the ISearchable interface.
    /// Since this class is a DataContract, the SearchableValueList must be properly initialized during deserialization.
    /// To this end, the InitValues method is decorated with an OnDeserializingAttribute that will perform the proper
    /// initialization.</p>  <p>This class implements the IAuditable interface to support auditing of the fields. This
    /// entity will be validated using the HermesActivityTypeValidator.</p>  <p>Thread Safety: It is mutable and not
    /// thread-safe.</p>
    /// </summary>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2006, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesActivityType :
        ActivityTypeBase<string, HermesActivityGroup>,
        ISearchable<HermesActivityType>,
        IAuditable<HermesActivityType>
    {
        /// <summary>
        /// <p>Represents the SearchableValueList instance of values for this activity group. All properties will be
        /// backed by this list.</p>  <p>This will be initialized with default values for all properties in the
        /// InitValues method.</p>
        /// </summary>
        /// <remarks>25</remarks>
        private SearchableValueList<HermesActivityType> values = new SearchableValueList<HermesActivityType>();

        /// <summary>
        /// <p>Represents the property for the values field.</p>  <p><strong>Get:</strong></p> <ul type="disc">
        /// <li>Simply return the values instance.</li> </ul>
        /// </summary>
        /// <remarks>35</remarks>
        public SearchableValueList<HermesActivityType> Values
        {
            get
            {
                return values;
            }
        }

        /// <summary>
        /// <p>This is the property for the primary key of this entity.</p> <p>This can be any value. It will be managed
        /// with the SearchableValueList with the key "id".</p> <p>It will be set to a default value in the InitValues()
        /// method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value from the
        /// SearchableValueList values with key "id" and cast to string.</li> </ul>  <p><strong>Set:</strong></p> <ul
        /// type="disc"> <li>Set the the value in the SearchableValueList values with key "id"</li> </ul>
        /// </summary>
        [DataMember]
        public override string Id
        {
            get
            {
                return values["id"].Value as string;
            }
            set
            {
                values["id"] = new SearchableValue<HermesActivityType>("id", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the handle of the user who last modified this entity.</p> <p>This can be any
        /// value. It will be managed with the SearchableValueList with the key "lastModifiedBy".</p> <p>It will be set
        /// to a default value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain
        /// the field value from the SearchableValueList values with key "lastModifiedBy" and cast to string.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "lastModifiedBy"</li> </ul>
        /// </summary>
        /// <remarks>74</remarks>
        [DataMember]
        public override string LastModifiedBy
        {
            get
            {
                return values["lastModifiedBy"].Value as string;
            }
            set
            {
                values["lastModifiedBy"] = new SearchableValue<HermesActivityType>("lastModifiedBy", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the timestamp of the last change performed on this entity.</p> <p>This can be
        /// any value. It will be managed with the SearchableValueList with the key &quot;lastModifiedDate&quot;.</p>
        /// <p>It will be set to a default value in the InitValues() method.</p> <p><strong>Get:</strong></p> <ul
        /// type="disc"> <li>Obtain the field value from the SearchableValueList values with key
        /// &quot;lastModifiedDate&quot; and cast to DateTime.</li> </ul> <p><strong>Set:</strong></p> <ul type="disc">
        /// <li>Set the the value in the SearchableValueList values with key &quot;lastModifiedDate&quot;</li> </ul>
        /// </summary>
        /// <remarks>91</remarks>
        [DataMember]
        public override DateTime LastModifiedDate
        {
            get
            {
                return (DateTime)values["lastModifiedDate"].Value;
            }
            set
            {
                values["lastModifiedDate"] = new SearchableValue<HermesActivityType>("lastModifiedBy", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the abbreviation used for this activity.</p> <p>This can be any value. It will
        /// be managed with the SearchableValueList with the key "abbreviation".</p> <p>It will be set to a default
        /// value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field
        /// value from the SearchableValueList values with key "abbreviation" and cast to string.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "abbreviation"</li> </ul>
        /// </summary>
        [DataMember]
        public override string Abbreviation
        {
            get
            {
                return values["abbreviation"].Value as string;
            }
            set
            {
                values["abbreviation"] = new SearchableValue<HermesActivityType>("abbreviation", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the name of this activity.</p> <p>This can be any value. It will be managed with
        /// the SearchableValueList with the key "name".</p> <p>It will be set to a default value in the InitValues()
        /// method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value from the
        /// SearchableValueList values with key "name" and cast to string.</li> </ul>  <p><strong>Set:</strong></p> <ul
        /// type="disc"> <li>Set the the value in the SearchableValueList values with key "name"</li> </ul>
        /// </summary>
        [DataMember]
        public override string Name
        {
            get
            {
                return values["name"].Value as string;
            }
            set
            {
                values["name"] = new SearchableValue<HermesActivityType>("name", null, value); ;
            }
        }

        /// <summary>
        /// <p>This is the property for the activity group this type belongs to.</p> <p>This can be any value. It will
        /// be managed with the SearchableValueList with the key "activityGroup".</p> <p>It will be set to a default
        /// value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field
        /// value from the SearchableValueList values with key "activityGroup" and cast to HermesActivityType.</li>
        /// </ul>  <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values
        /// with key "activityGroup"</li> </ul>
        /// </summary>
        /// <remarks>142</remarks>
        public override HermesActivityGroup ActivityGroup
        {
            get
            {
                return values["activityGroup"].Value as HermesActivityGroup;
            }
            set
            {
                values["activityGroup"] = new SearchableValue<HermesActivityType>("activityGroup", null, value); ;
            }
        }


        /// <summary>Default constructor. Calls InitValues().</summary>
        public HermesActivityType()
        {
            InitValues();
        }

        /// <remarks>183</remarks>
        /// <summary>
        /// <p>Audits the this HermesActivityType against another HermesActivityType. It makes a field-by-field
        /// comparison testing what has changed.</p>  <p>This method will fill the TextValue# or NumericValue#, and ID
        /// fields of each HermesAuditRecord it generates. It will be up to the service using this entity to fill the
        /// other fields.</p>  <p><strong>Implementation Notes</strong></p> <ol type="disc"> <li>If the passed entity is
        /// null, jus create one HermesAuditRecord</li> <li>Iterate over the Values field. For each changed property:
        /// <ol type="disc"> <li>Create new HermesAuditRecord.</li> <li>If it is numeric, set hermesAuditRecord's
        /// NumericValue2 field to this entity's property value, and the NumericValue1 field to the passed entity's
        /// property value</li> <li>If it is text, set hermesAuditRecord's TextValue2 field to this entity's property
        /// value, and the TextValue1 field to the passed entity's property value</li> </ol> </li> <li>Return the
        /// HermesAuditRecords as an IList of HermesAuditRecord</li> </ol>
        /// </summary>
        /// <exception cref="IllegalAuditItemException">
        /// IllegalAuditItemException If old HermesActivityType is the same object as this
        /// </exception>
        /// <param name="old">HermesActivityType to compary this to</param>
        /// <returns>IList of HermesAuditRecords detailing any changes</returns>
        public IList<HermesAuditRecord> Audit(HermesActivityType old)
        {
            throw new NotImplementedException();
        }

        /// <remarks>202</remarks>
        /// <summary>
        /// <p>Initializes the values to default values. This method will be called by the constructor or on
        /// deserialization.</p> <p>Note to reader: The generic is depicted with square brakets because the Poseidon
        /// parser will fail with angle brackets.</p>  <p><strong>Implementation Notes:</strong></p> <ul type="disc">
        /// <li>values = new SearchableValueList[HermesActivityType](this);</li> <li>values.Add("lastModifiedBy", new
        /// SearchableValue[HermesActivityType]("lastModifiedBy", null));</li> <li>values.Add("lastModifiedDate", new
        /// SearchableValue[HermesActivityType]("lastModifiedDate", DateTime.MinValue));</li>
        /// <li>values.Add("abbreviation", new SearchableValue[HermesActivityType]("abbreviation", null));</li>
        /// <li>values.Add("name", new SearchableValue[HermesActivityType]("name", null));</li> <li>Values.Add("id", new
        /// SearchableValue[HermesActivityType]("id", null));</li> <li>Values.Add("activityGroup", new
        /// SearchableValue[HermesActivityType]("activityGroup", null));</li> </ul>
        /// </summary>
        [OnDeserializing]
        protected void InitValues()
        {
            values = new SearchableValueList<HermesActivityType>(this);

            values.Add("lastModifiedBy",
                new SearchableValue<HermesActivityType>("lastModifiedBy", null, null));
            values.Add("lastModifiedDate",
                new SearchableValue<HermesActivityType>("lastModifiedDate", null, DateTime.MinValue));
            values.Add("abbreviation",
                new SearchableValue<HermesActivityType>("abbreviation", null, null));
            values.Add("name",
                new SearchableValue<HermesActivityType>("name", null, null));
            values.Add("id",
                new SearchableValue<HermesActivityType>("id", null, null));
            values.Add("activityGroup",
                new SearchableValue<HermesActivityType>("activityGroup", null, null));
        }
    }
}
