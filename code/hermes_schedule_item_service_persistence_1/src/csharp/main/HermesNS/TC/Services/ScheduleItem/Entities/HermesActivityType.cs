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
    /// <para>
    /// This is a concrete implementation of the ActivityTypeBase that uses the concrete versions of the other base
    /// classes, such as HermesActivityType. For the Id, it uses a Guid in the form of a string.
    /// </para>
    /// <para>All properties are indexed as this class implements the ISearchable interface.</para>
    /// <para>This class implements the IAuditable interface to support auditing of the fields.
    /// This entity is validated using the HermesActivityTypeValidator.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe.</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesActivityType :
        ActivityTypeBase<string, HermesActivityGroup>,
        ISearchable<HermesActivityType>,
        IAuditable<HermesActivityType>
    {
        /// <summary>
        /// <para>Represents the SearchableValueList instance of values for this activity group.
        /// All properties is backed by this list.</para>
        /// <para>This is initialized with default values for all properties in the InitValues method.</para>
        /// </summary>
        private SearchableValueList<HermesActivityType> values = new SearchableValueList<HermesActivityType>();

        /// <summary>
        /// <para>Gets a SearchableValueList of the properties of the entity.</para>
        /// </summary>
        /// <value>A SearchableValueList of the properties of the entity.</value>
        public SearchableValueList<HermesActivityType> Values
        {
            get
            {
                return values;
            }
        }

        /// <summary>
        /// <para>Gets or sets the primary key of this entity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The primary key for the entity.</value>
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
        /// <para>Gets or sets the handle of the user who last modified this entity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The handle of the user who last modified this entity.</value>
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
        /// <para>Gets or sets the timestamp of the last change performed on this entity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The timestamp of the last change performed on this entity.</value>
        [DataMember]
        public override DateTime LastModifiedDate
        {
            get
            {
                return (DateTime)values["lastModifiedDate"].Value;
            }
            set
            {
                values["lastModifiedDate"] = new SearchableValue<HermesActivityType>("lastModifiedDate", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the abbreviation used for this activity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The abbreviation used for this activity.</value>
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
        /// <para>Gets or sets the name of this activity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The name of this activity.</value>
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
        /// <para>Gets or sets the activity group this type belongs to.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The activity group this type belongs to.</value>
        [DataMember]
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

        /// <summary>
        /// Creates a new HermesActivityType instance.
        /// </summary>
        public HermesActivityType()
        {
            InitValues(new StreamingContext());
        }

        /// <summary>
        /// <para>
        /// Audits the this HermesActivityType against another HermesActivityType.
        /// It makes a field-by-field comparison testing what has changed. For each property
        /// that has a value different from other, one HermesAuditRecord is added to the returned list.
        /// </para>
        /// <para>
        /// If a property is a text type then the TextValue1 and TextValue2 proeprties
        /// of the HermesAuditRecord are populated with the property value of the old and current instance.
        /// If a property is a numeric type then the NumericValue1 and NumericValue2 proeprties
        /// of the HermesAuditRecord are populated with the property value of the old and current instance.
        /// </para>
        /// <para>The ActivityGroup property is compared on the basis of its Name.</para>
        /// </summary>
        /// <exception cref="IllegalAuditItemException">
        /// If old HermesActivityType is the same object as this instance.
        /// </exception>
        /// <param name="old">HermesActivityType to compary this to</param>
        /// <returns>IList of HermesAuditRecords detailing any changes</returns>
        public IList<HermesAuditRecord> Audit(HermesActivityType old)
        {
            if (object.ReferenceEquals(old, this))
            {
                IllegalAuditItemException e = new IllegalAuditItemException("Cannot audit an item against itself.");
                throw Helper.GetSelfDocumentingException(e, e.Message,
                    GetType().FullName + "Audit(HermesActivityType old)",
                    new string[] { "values" }, new object[] { values },
                    new string[] { "old" }, new object[] { old }, new string[0], new object[0]);
            }

            return Helper.GetAuditRecords<HermesActivityType>(this, old, Id);
        }

        /// <summary>
        /// <para>Initializes the properties of the entity to their default values.
        /// This method is called by the constructor or on deserialization.</para>
        /// </summary>
        /// <param name="streamingContext">This parameter is not used.</param>
        [OnDeserializing]
        protected void InitValues(StreamingContext streamingContext)
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
