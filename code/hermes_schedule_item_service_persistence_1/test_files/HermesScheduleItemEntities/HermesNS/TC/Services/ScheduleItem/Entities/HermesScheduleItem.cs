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
    /// <p>This is a concrete implementation of the ScheduleItemBase that uses the concrete Hermes versions of the other
    /// base classes, such as HermesActivity, HermesGenericNote, HermesScheduleItemStatus, and
    /// HermesScheduleItemRequestStatus. For the Id, it uses a Guid in the form of a string.</p>  <p>All properties are
    /// backed by the Indexing components SearchableValueList, as this class implements the ISearchable interface.
    /// Since this class is a DataContract, the SearchableValueList must be properly initialized during deserialization.
    /// To this end, the InitValues method is decorated with an OnDeserializingAttribute that will perform the proper
    /// initialization.</p>  <p>This class implements the IAuditable interface to support auditing of the fields. This
    /// entity will be validated using the HermesScheduleItemValidator.</p>  <p>Thread Safety: It is mutable and not
    /// thread-safe.</p>
    /// </summary>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2006, TopCoder, Inc. All rights reserved.</copyright>
    [DataContract]
    public class HermesScheduleItem :
        ScheduleItemBase<string, HermesActivity, HermesScheduleItemStatus, HermesScheduleItemRequestStatus, HermesGenericNote,
        HermesActivityType, HermesActivityGroup, HermesGenericNoteItem, HermesGenericNoteItemHistory>,
        ISearchable<HermesScheduleItem>, IAuditable<HermesScheduleItem>
    {
        /// <summary>
        /// <p>Represents the SearchableValueList instance of values for this activity group. All properties will be
        /// backed by this list.</p>  <p>This will be initialized with default values for all properties in the
        /// InitValues method.</p>
        /// </summary>
        /// <remarks>26</remarks>
        private SearchableValueList<HermesScheduleItem> values = new SearchableValueList<HermesScheduleItem>();

        /// <summary>
        /// <p>Represents the property for the values field.</p>  <p><strong>Get:</strong></p> <ul type="disc">
        /// <li>Simply return the values instance.</li> </ul>
        /// </summary>
        /// <remarks>36</remarks>
        public SearchableValueList<HermesScheduleItem> Values
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
                values["id"] = new SearchableValue<HermesScheduleItem>("id", null, value);
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
                values["lastModifiedBy"] = new SearchableValue<HermesScheduleItem>("lastModifiedBy", null, value);
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
                values["lastModifiedDate"] = new SearchableValue<HermesScheduleItem>("lastModifiedBy", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the activity template associated with this schedule item.</p> <p>This can be any
        /// value. It will be managed with the SearchableValueList with the key "activity".</p> <p>It will be set to a
        /// default value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the
        /// field value from the SearchableValueList values with key "activity" and cast to HermesActivity.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "activity"</li> </ul>
        /// </summary>
        /// <remarks>109</remarks>
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
        /// <p>This is the property for the duration of this schedule item.</p> <p>This can be any value. It will be
        /// managed with the SearchableValueList with the key "duration".</p> <p>It will be set to a default value in
        /// the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value from
        /// the SearchableValueList values with key "duration" and cast to Decimal.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "duration"</li> </ul>
        /// </summary>
        /// <remarks>126</remarks>
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
        /// <p>This is the property for a flag whether this schedule item is expired.</p> <p>This can be any value. It
        /// will be managed with the SearchableValueList with the key "exceptionFlag".</p> <p>It will be set to a
        /// default value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the
        /// field value from the SearchableValueList values with key "exceptionFlag" and cast to Char.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "exceptionFlag"</li> </ul>
        /// </summary>
        /// <remarks>143</remarks>
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
        /// <p>This is the property for the day this schedule item will expire.</p> <p>This can be any value. It will be
        /// managed with the SearchableValueList with the key "expirationDate".</p> <p>It will be set to a default value
        /// in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value
        /// from the SearchableValueList values with key "expirationDate" and cast to DateTime.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "expirationDate"</li> </ul>
        /// </summary>
        /// <remarks>160</remarks>
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
        /// <p>This is the property for a general notes section for the schedule item.</p> <p>This can be any value. It
        /// will be managed with the SearchableValueList with the key "note".</p> <p>It will be set to a default value
        /// in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value
        /// from the SearchableValueList values with key "note" and cast to HermesGenericNote.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "note"</li> </ul>
        /// </summary>
        /// <remarks>177</remarks>
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
        /// <p>This is the property for the status of this schedule item.</p> <p>This can be any value. It will be
        /// managed with the SearchableValueList with the key "scheduleItemStatus".</p> <p>It will be set to a default
        /// value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field
        /// value from the SearchableValueList values with key "scheduleItemStatus" and cast to
        /// HermesScheduleItemStatus.</li> </ul>  <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in
        /// the SearchableValueList values with key "scheduleItemStatus"</li> </ul>
        /// </summary>
        /// <remarks>194</remarks>
        [DataMember]
        public override HermesScheduleItemStatus ScheduleItemStatus
        {
            get
            {
                return (HermesScheduleItemStatus)values["scheduleItemStatus"].Value;
            }
            set
            {
                values["scheduleItemStatus"] = new SearchableValue<HermesScheduleItem>("scheduleItemStatus", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the request status of this schedule item.</p> <p>This can be any value. It will
        /// be managed with the SearchableValueList with the key "scheduleItemRequestStatus".</p> <p>It will be set to a
        /// default value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the
        /// field value from the SearchableValueList values with key "scheduleItemRequestStatus" and cast to
        /// HermesScheduleItemRequestStatus.</li> </ul>  <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the
        /// value in the SearchableValueList values with key "scheduleItemRequestStatus"</li> </ul>
        /// </summary>
        /// <remarks>211</remarks>
        [DataMember]
        public override HermesScheduleItemRequestStatus ScheduleItemRequestStatus
        {
            get
            {
                return (HermesScheduleItemRequestStatus)values["scheduleItemRequestStatus"].Value;
            }
            set
            {
                values["scheduleItemRequestStatus"] = new SearchableValue<HermesScheduleItem>("scheduleItemRequestStatus", null, value);
            }
        }

        /// <summary>
        /// <p>This is the property for the version of this schedule item.</p> <p>This can be any value. It will be
        /// managed with the SearchableValueList with the key "version".</p> <p>It will be set to a default value in the
        /// InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value from the
        /// SearchableValueList values with key "version" and cast to Int32.</li> </ul>  <p><strong>Set:</strong></p>
        /// <ul type="disc"> <li>Set the the value in the SearchableValueList values with key "version"</li> </ul>
        /// </summary>
        /// <remarks>228</remarks>
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
        /// <p>This is the property for the date of this schedule item.</p> <p>This can be any value. It will be managed
        /// with the SearchableValueList with the key "workDate".</p> <p>It will be set to a default value in the
        /// InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the field value from the
        /// SearchableValueList values with key "workDate" and cast to DateTime.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "workDate"</li> </ul>
        /// </summary>
        /// <remarks>245</remarks>
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
        /// <p>This is the property for the amount of work days of this schedule item.</p> <p>This can be any value. It
        /// will be managed with the SearchableValueList with the key "workDayAmount".</p> <p>It will be set to a
        /// default value in the InitValues() method.</p>  <p><strong>Get:</strong></p> <ul type="disc"> <li>Obtain the
        /// field value from the SearchableValueList values with key "workDayAmount" and cast to Decimal.</li> </ul>
        /// <p><strong>Set:</strong></p> <ul type="disc"> <li>Set the the value in the SearchableValueList values with
        /// key "workDayAmount"</li> </ul>
        /// </summary>
        /// <remarks>262</remarks>
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

        /// <remarks>273</remarks>
        /// <summary>Default constructor. Calls InitValues().</summary>
        public HermesScheduleItem()
        {
            InitValues();
        }

        /// <remarks>301</remarks>
        /// <summary>
        /// <p>Audits the this HermesScheduleItem against another HermesScheduleItem. It makes a field-by-field
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
        /// IllegalAuditItemException If old HermesScheduleItem is the same object as this
        /// </exception>
        /// <param name="old">HermesScheduleItem to compary this to</param>
        /// <returns>IList of HermesAuditRecords detailing any changes</returns>
        public IList<HermesAuditRecord> Audit(HermesScheduleItem old)
        {
            throw new NotImplementedException();
        }

        /// <remarks>326</remarks>
        /// <summary>
        /// <p>Initializes the values to default values. This method will be called by the constructor or on
        /// deserialization.</p> <p>Note to reader: The generic is depicted with square brakets because the Poseidon
        /// parser will fail with angle brackets.</p>  <p><strong>Implementation Notes:</strong></p> <ul type="disc">
        /// <li>values = new SearchableValueList[HermesScheduleItem](this);</li> <li>values.Add("lastModifiedBy", new
        /// SearchableValue[HermesScheduleItem]("lastModifiedBy", null));</li> <li>values.Add("lastModifiedDate", new
        /// SearchableValue[HermesScheduleItem]("lastModifiedDate", DateTime.MinValue));</li> <li>values.Add("duration",
        /// new SearchableValue[HermesScheduleItem]("duration", 0));</li> <li>values.Add("exceptionFlag", new
        /// SearchableValue[HermesScheduleItem]("exceptionFlag", 'f'));</li> <li>values.Add("expirationDate", new
        /// SearchableValue[HermesScheduleItem]("expirationDate", DateTime.MinValue));</li> <li>values.Add("note", new
        /// SearchableValue[HermesScheduleItem]("note", null));</li> <li>values.Add("version", new
        /// SearchableValue[HermesScheduleItem]("version", 0));</li> <li>values.Add("workDate", new
        /// SearchableValue[HermesScheduleItem]("workDate", DateTime.MinValue));</li> <li>values.Add("workDayAmount",
        /// new SearchableValue[HermesScheduleItem]("workDayAmount", 0));</li> <li>Values.Add("id", new
        /// SearchableValue[HermesScheduleItem]("id", null));</li> <li>Values.Add("activity", new
        /// SearchableValue[HermesScheduleItem]("activity", null));</li> <li>Values.Add("scheduleItemStatus", new
        /// SearchableValue[HermesScheduleItem]("scheduleItemStatus", null));</li>
        /// <li>Values.Add("scheduleItemRequestStatus", new
        /// SearchableValue[HermesScheduleItem]("scheduleItemRequestStatus", null));</li> </ul>
        /// </summary>
        [OnDeserializing]
        protected void InitValues()
        {
            values = new SearchableValueList<HermesScheduleItem>(this);

            values.Add("lastModifiedBy",
                new SearchableValue<HermesScheduleItem>("lastModifiedBy", null, null));
            values.Add("lastModifiedDate",
                new SearchableValue<HermesScheduleItem>("lastModifiedDate", null, DateTime.MinValue));
            values.Add("duration",
                new SearchableValue<HermesScheduleItem>("duration", null, 0));
            values.Add("exceptionFlag",
                new SearchableValue<HermesScheduleItem>("exceptionFlag", null, 'f'));
            values.Add("expirationDate",
                new SearchableValue<HermesScheduleItem>("expirationDate", null, DateTime.MinValue));
            values.Add("note",
                new SearchableValue<HermesScheduleItem>("note", null, null));
            values.Add("version",
                new SearchableValue<HermesScheduleItem>("version", null, 0));
            values.Add("workDate",
                new SearchableValue<HermesScheduleItem>("workDate", null, DateTime.MinValue));
            values.Add("workDayAmount",
                new SearchableValue<HermesScheduleItem>("workDayAmount", null, 0));
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
