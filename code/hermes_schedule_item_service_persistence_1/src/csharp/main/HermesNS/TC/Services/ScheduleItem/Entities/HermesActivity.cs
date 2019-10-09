// HermesActivity.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HermesNS.TC.Services.AuditTrail;
using TopCoder.Util.Indexing;
using TopCoder.Services.WCF.ScheduleItem.Entities;

namespace HermesNS.TC.Services.ScheduleItem.Entities
{
    /// <summary>
    /// <para>This is a concrete implementation of the ActivityBase that uses the concrete versions of the other base
    /// classes, such as HermesActivityType. For the Id, it uses a Guid in the form of a string.</para>
    /// <para>All properties are indexed.</para>
    /// <para>This class implements the IAuditable interface to support auditing of the fields.
    /// This entity is validated using the HermesActivityValidator.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe.</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesActivity :
        ActivityBase<string, HermesActivityType, HermesActivityGroup>,
        ISearchable<HermesActivity>,
        IAuditable<HermesActivity>
    {
        /// <summary>
        /// <para>Represents the SearchableValueList instance of values for this activity.
        /// All properties are backed by this list.</para>
        /// <para>This is initialized with default values for all properties in the InitValues method.</para>
        /// </summary>
        private SearchableValueList<HermesActivity> values = new SearchableValueList<HermesActivity>();

        /// <summary>
        /// <para>Gets a SearchableValueList of the properties of the entity.</para>
        /// </summary>
        /// <value>A SearchableValueList of the properties of the entity.</value>
        public SearchableValueList<HermesActivity> Values
        {
            get
            {
                return values;
            }
        }

        /// <summary>
        /// <para>Gets or sets the primary key of this entity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
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
                values["id"] = new SearchableValue<HermesActivity>("id", null, value);
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
                values["lastModifiedBy"] = new SearchableValue<HermesActivity>("lastModifiedBy", null, value);
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
                values["lastModifiedDate"] = new SearchableValue<HermesActivity>("lastModifiedDate", null, value);
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
                values["abbreviation"] = new SearchableValue<HermesActivity>("abbreviation", null, value);
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
                values["name"] = new SearchableValue<HermesActivity>("name", null, value); ;
            }
        }

        /// <summary>
        /// <para>Gets or sets the type of this activity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The type of this activity.</value>
        [DataMember]
        public override HermesActivityType ActivityType
        {
            get
            {
                return values["activityType"].Value as HermesActivityType;
            }
            set
            {
                values["activityType"] = new SearchableValue<HermesActivity>("activityType", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the default duration of this activity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The default duration of this activity.</value>
        [DataMember]
        public override Decimal DefaultDuration
        {
            get
            {
                return Convert.ToDecimal(values["defaultDuration"].Value);
            }
            set
            {
                values["defaultDuration"] = new SearchableValue<HermesActivity>("defaultDuration", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the default amount of days before it expires.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The default amount of days before it expires.</value>
        [DataMember]
        public override Int32 DefaultExpireDays
        {
            get
            {
                return (int)values["defaultExpireDays"].Value;
            }
            set
            {
                values["defaultExpireDays"] = new SearchableValue<HermesActivity>("defaultExpireDays", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the default start time of this activity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The default start time of this activity.</value>
        [DataMember]
        public override Int32 DefaultStartTime
        {
            get
            {
                return (int)values["defaultStartTime"].Value;
            }
            set
            {
                values["defaultStartTime"] = new SearchableValue<HermesActivity>("defaultStartTime", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the flag whether this activity is enabled.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The flag whether this activity is enabled.</value>
        [DataMember]
        public override Boolean Enabled
        {
            get
            {
                return (bool)values["enabled"].Value;
            }
            set
            {
                values["enabled"] = new SearchableValue<HermesActivity>("enabled", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the flag whether this activity is exclusive.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The flag whether this activity is exclusive.</value>
        [DataMember]
        public override Boolean ExclusiveFlag
        {
            get
            {
                return (bool)values["exclusiveFlag"].Value;
            }
            set
            {
                values["exclusiveFlag"] = new SearchableValue<HermesActivity>("exclusiveFlag", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the amount of work days of this activity.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList</para>
        /// </summary>
        /// <value>The amount of work days of this activity.</value>
        [DataMember]
        public override Int32 WorkDayAmount
        {
            get
            {
                return (int)values["workDayAmount"].Value;
            }
            set
            {
                values["workDayAmount"] = new SearchableValue<HermesActivity>("workDayAmount", null, value);
            }
        }

        /// <summary>
        /// Creates a new HermesActivity instance
        /// </summary>
        public HermesActivity()
        {
            InitValues(new StreamingContext());
        }

        /// <summary>
        /// <para>
        /// Audits the this HermesActivity against another HermesActivity.
        /// It makes a field-by-field comparison testing what has changed. For each property
        /// that has a value different from other, one HermesAuditRecord is added to the returned list.
        /// </para>
        /// <para>
        /// If a property is a text type then the TextValue1 and TextValue2 proeprties
        /// of the HermesAuditRecord are populated with the property value of the old and current instance.
        /// If a property is a numeric type then the NumericValue1 and NumericValue2 proeprties
        /// of the HermesAuditRecord are populated with the property value of the old and current instance.
        /// </para>
        /// <para>The ActivityType property is compared on the basis of its Name.</para>
        /// </summary>
        /// <exception cref="IllegalAuditItemException">
        /// If old HermesActivity is the same object as this instance.
        /// </exception>
        /// <param name="old">HermesActivity to compary this to</param>
        /// <returns>IList of HermesAuditRecords detailing any changes</returns>
        public IList<HermesAuditRecord> Audit(HermesActivity old)
        {
            if (object.ReferenceEquals(old, this))
            {
                IllegalAuditItemException e = new IllegalAuditItemException("Cannot audit an item against itself.");
                throw Helper.GetSelfDocumentingException(e, e.Message,
                    GetType().FullName + "Audit(HermesActivity old)",
                    new string[] { "values" }, new object[] { values },
                    new string[] { "old" }, new object[] { old }, new string[0], new object[0]);
            }

            return Helper.GetAuditRecords<HermesActivity>(this, old, Id);
        }

        /// <summary>
        /// <para>Initializes the properties of the entity to their default values.
        /// This method is called by the constructor or on deserialization.</para>
        /// </summary>
        /// <param name="streamingContext">This parameter is not used.</param>
        [OnDeserializing]
        protected void InitValues(StreamingContext streamingContext)
        {
            values = new SearchableValueList<HermesActivity>(this);

            values.Add("lastModifiedBy",
                new SearchableValue<HermesActivity>("lastModifiedBy", null, null));
            values.Add("lastModifiedDate",
                new SearchableValue<HermesActivity>("lastModifiedDate", null, DateTime.MinValue));
            values.Add("abbreviation",
                new SearchableValue<HermesActivity>("abbreviation", null, null));
            values.Add("name",
                new SearchableValue<HermesActivity>("name", null, null));
            values.Add("defaultDuration",
                new SearchableValue<HermesActivity>("defaultDuration", null, 0));
            values.Add("defaultExpireDays",
                new SearchableValue<HermesActivity>("defaultExpireDays", null, 0));
            values.Add("defaultStartTime",
                new SearchableValue<HermesActivity>("defaultStartTime", null, 0));
            values.Add("enabled",
                new SearchableValue<HermesActivity>("enabled", null, false));
            values.Add("exclusiveFlag",
                new SearchableValue<HermesActivity>("exclusiveFlag", null, false));
            values.Add("workDayAmount",
                new SearchableValue<HermesActivity>("workDayAmount", null, 0));
            values.Add("id",
                new SearchableValue<HermesActivity>("id", null, null));
            values.Add("activityType",
                new SearchableValue<HermesActivity>("activityType", null, null));
        }
    }
}
