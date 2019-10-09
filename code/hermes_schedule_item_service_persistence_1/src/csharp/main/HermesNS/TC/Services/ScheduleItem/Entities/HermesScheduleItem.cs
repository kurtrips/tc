// HermesScheduleItem.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TopCoder.Services.WCF.ScheduleItem.Entities;
using HermesNS.TC.Services.AuditTrail;
using HermesNS.TC.Services.GenericNotes;
using TopCoder.Util.Indexing;

namespace HermesNS.TC.Services.ScheduleItem.Entities
{
    /// <summary>
    /// <para>This is a concrete implementation of the ScheduleItemBase that uses the concrete Hermes versions
    /// of the other base classes, such as HermesActivity, HermesGenericNote, HermesScheduleItemStatus, and
    /// HermesScheduleItemRequestStatus. For the Id, it uses a Guid in the form of a string.</para>
    /// <para>All properties are indexed, as this class implements the ISearchable interface.</para>
    /// <para>This class implements the IAuditable interface to support auditing of the fields. This
    /// entity is validated using the HermesScheduleItemValidator.</para>
    /// </summary>
    /// <threadsafety>It is mutable and not thread-safe.</threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesScheduleItem :
        ScheduleItemBase<string, HermesActivity, HermesScheduleItemStatus, HermesScheduleItemRequestStatus,
        HermesGenericNote, HermesActivityType, HermesActivityGroup, HermesGenericNoteItem,
        HermesGenericNoteItemHistory>,
        ISearchable<HermesScheduleItem>,
        IAuditable<HermesScheduleItem>
    {
        /// <summary>
        /// <para>Represents the SearchableValueList instance of values for this activity group.
        /// All properties is backed by this list.</para>
        /// <para>This is initialized with default values for all properties in the InitValues method.</para>
        /// </summary>
        private SearchableValueList<HermesScheduleItem> values = new SearchableValueList<HermesScheduleItem>();

        /// <summary>
        /// <para>Gets a SearchableValueList of the properties of the entity.</para>
        /// </summary>
        /// <value>A SearchableValueList of the properties of the entity.</value>
        public SearchableValueList<HermesScheduleItem> Values
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
                values["id"] = new SearchableValue<HermesScheduleItem>("id", null, value);
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
                values["lastModifiedBy"] = new SearchableValue<HermesScheduleItem>("lastModifiedBy", null, value);
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
                values["lastModifiedDate"] = new SearchableValue<HermesScheduleItem>("lastModifiedDate", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the activity template associated with this schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The activity template associated with this schedule item.</value>
        [DataMember]
        public override HermesActivity Activity
        {
            get
            {
                return (HermesActivity)values["activity"].Value;
            }
            set
            {
                values["activity"] = new SearchableValue<HermesScheduleItem>("activity", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the duration of this schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The duration of this schedule item.</value>
        [DataMember]
        public override Decimal Duration
        {
            get
            {
                return (Decimal)values["duration"].Value;
            }
            set
            {
                values["duration"] = new SearchableValue<HermesScheduleItem>("duration", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the flag whether this schedule item is expired.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The flag whether this schedule item is expired.</value>
        [DataMember]
        public override Char ExceptionFlag
        {
            get
            {
                return (Char)values["exceptionFlag"].Value;
            }
            set
            {
                values["exceptionFlag"] = new SearchableValue<HermesScheduleItem>("exceptionFlag", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the day this schedule item will expire.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The day this schedule item will expire.</value>
        [DataMember]
        public override DateTime ExpirationDate
        {
            get
            {
                return (DateTime)values["expirationDate"].Value;
            }
            set
            {
                values["expirationDate"] = new SearchableValue<HermesScheduleItem>("expirationDate", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the general notes section for the schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The general notes section for the schedule item.</value>
        [DataMember]
        public override HermesGenericNote Note
        {
            get
            {
                return (HermesGenericNote)values["note"].Value;
            }
            set
            {
                values["note"] = new SearchableValue<HermesScheduleItem>("note", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the status of this schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The status of this schedule item.</value>
        [DataMember]
        public override HermesScheduleItemStatus ScheduleItemStatus
        {
            get
            {
                return (HermesScheduleItemStatus)values["scheduleItemStatus"].Value;
            }
            set
            {
                values["scheduleItemStatus"] =
                    new SearchableValue<HermesScheduleItem>("scheduleItemStatus", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the request status of this schedule item.</para>
        /// <para>This can be any value. It will be managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The request status of this schedule item.</value>
        [DataMember]
        public override HermesScheduleItemRequestStatus ScheduleItemRequestStatus
        {
            get
            {
                return (HermesScheduleItemRequestStatus)values["scheduleItemRequestStatus"].Value;
            }
            set
            {
                values["scheduleItemRequestStatus"] =
                    new SearchableValue<HermesScheduleItem>("scheduleItemRequestStatus", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the version of this schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The version of this schedule item.</value>
        [DataMember]
        public override Int32 Version
        {
            get
            {
                return (Int32)values["version"].Value;
            }
            set
            {
                values["version"] = new SearchableValue<HermesScheduleItem>("version", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the date of this schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The date of this schedule item.</value>
        [DataMember]
        public override DateTime WorkDate
        {
            get
            {
                return (DateTime)values["workDate"].Value;
            }
            set
            {
                values["workDate"] = new SearchableValue<HermesScheduleItem>("workDate", null, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets the amount of work days of this schedule item.</para>
        /// <para>This can be any value. It is managed with the SearchableValueList.</para>
        /// </summary>
        /// <value>The amount of work days of this schedule item.</value>
        [DataMember]
        public override Decimal WorkDayAmount
        {
            get
            {
                return (Decimal)values["workDayAmount"].Value;
            }
            set
            {
                values["workDayAmount"] = new SearchableValue<HermesScheduleItem>("workDayAmount", null, value);
            }
        }

        /// <summary>
        /// Creates a new HermesScheduleItem instance.
        /// </summary>
        public HermesScheduleItem()
        {
            InitValues(new StreamingContext());
        }

        /// <summary>
        /// <para>
        /// Audits the this HermesScheduleItem against another HermesScheduleItem.
        /// It makes a field-by-field comparison testing what has changed. For each property
        /// that has a value different from other, one HermesAuditRecord is added to the returned list.
        /// </para>
        /// <para>
        /// If a property is a text type then the TextValue1 and TextValue2 proeprties
        /// of the HermesAuditRecord are populated with the property value of the old and current instance.
        /// If a property is a numeric type then the NumericValue1 and NumericValue2 proeprties
        /// of the HermesAuditRecord are populated with the property value of the old and current instance.
        /// </para>
        /// <para>
        /// The Activity property is compared on the basis of its Name.
        /// The Note, ScheduleItemRequestStatus and the ScheduleItemStatus properties
        /// are compared on the basis of their Description properties.
        /// </para>
        /// </summary>
        /// <exception cref="IllegalAuditItemException">
        /// If old HermesScheduleItem is the same object as this instance.
        /// </exception>
        /// <param name="old">HermesScheduleItem to compary this to</param>
        /// <returns>IList of HermesAuditRecords detailing any changes</returns>
        public IList<HermesAuditRecord> Audit(HermesScheduleItem old)
        {
            if (object.ReferenceEquals(old, this))
            {
                IllegalAuditItemException e = new IllegalAuditItemException("Cannot audit an item against itself.");
                throw Helper.GetSelfDocumentingException(e, e.Message,
                    GetType().FullName + "Audit(HermesScheduleItem old)",
                    new string[] { "values" }, new object[] { values },
                    new string[] { "old" }, new object[] { old }, new string[0], new object[0]);
            }

            return Helper.GetAuditRecords<HermesScheduleItem>(this, old, Id);
        }

        /// <summary>
        /// <para>Initializes the properties of the entity to their default values.
        /// This method is called by the constructor or on deserialization.</para>
        /// </summary>
        [OnDeserializing]
        protected void InitValues(StreamingContext streamingContext)
        {
            values = new SearchableValueList<HermesScheduleItem>(this);

            values.Add("lastModifiedBy",
                new SearchableValue<HermesScheduleItem>("lastModifiedBy", null, null));
            values.Add("lastModifiedDate",
                new SearchableValue<HermesScheduleItem>("lastModifiedDate", null, DateTime.MinValue));
            values.Add("duration",
                new SearchableValue<HermesScheduleItem>("duration", null, new Decimal(0)));
            values.Add("exceptionFlag",
                new SearchableValue<HermesScheduleItem>("exceptionFlag", null, 'N'));
            values.Add("expirationDate",
                new SearchableValue<HermesScheduleItem>("expirationDate", null, DateTime.MinValue));
            values.Add("note",
                new SearchableValue<HermesScheduleItem>("note", null, null));
            values.Add("version",
                new SearchableValue<HermesScheduleItem>("version", null, 0));
            values.Add("workDate",
                new SearchableValue<HermesScheduleItem>("workDate", null, DateTime.MinValue));
            values.Add("workDayAmount",
                new SearchableValue<HermesScheduleItem>("workDayAmount", null, new Decimal(0)));
            values.Add("id",
                new SearchableValue<HermesScheduleItem>("id", null, null));
            values.Add("activity",
                new SearchableValue<HermesScheduleItem>("activity", null, null));
            values.Add("scheduleItemStatus",
                new SearchableValue<HermesScheduleItem>("scheduleItemStatus", null, null));
            values.Add("scheduleItemRequestStatus",
                new SearchableValue<HermesScheduleItem>("scheduleItemRequestStatus", null, null));
        }

    }
}
