// HermesScheduleItemPersistenceHelper.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Data;
using System.Collections.Generic;
using TopCoder.Services.WCF.ScheduleItem.Persistence;
using TopCoder.Services.WCF.ScheduleItem;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.GenericNotes;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// <para>An implementation of the IScheduleItemHelperBase interface that maps data in the IDataReader for the data
    /// model used in the HermesScheduleItemPersistenceProvider.</para>
    /// </summary>
    /// <threadsafety>
    /// It is immutable but not thread-safe dues to working with non-thread-safe entities.
    /// </threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class HermesScheduleItemPersistenceHelper :
        IScheduleItemHelperBase<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
        HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
        HermesGenericNoteItem, HermesGenericNoteItemHistory>
    {
        /// <summary>
        /// Creates a new HermesScheduleItemPersistenceHelper instance.
        /// </summary>
        public HermesScheduleItemPersistenceHelper()
        {
        }

        /// <summary>
        /// Builds the HermesActivity list from the passed reader. Each HermesActivity is fully constructed with all
        /// properties available.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// Column with given name was not found OR reader is closed OR invalid data types in reader etc.
        /// </exception>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>list of activities in the reader</returns>
        public IList<HermesActivity> BuildActivities(IDataReader reader)
        {
            IList<HermesActivity> ret = new List<HermesActivity>();

            try
            {
                Helper.ValidateNotNull(reader, "reader");

                while (reader.Read())
                {
                    ret.Add(CreateHermesActivity(reader));
                }
            }
            //Column with given name was not found OR reader is closed OR invalid data types in reader etc.
            catch (Exception e)
            {
                throw Helper.GetSDE(e, e.Message, "HermesScheduleItemPersistenceHelper.BuildActivities",
                    new string[0], new object[0], new string[] { "reader" }, new object[] { reader },
                    new string[] { "ret" }, new object[] { ret }, typeof(InvalidArgumentException));
            }

            return ret;
        }

        /// <summary>
        /// Builds the HermesActivityGroup list from the passed reader.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// Column with given name was not found OR reader is closed OR invalid data types in reader etc.
        /// </exception>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>list of activity groups in the reader</returns>
        public IList<HermesActivityGroup> BuildActivityGroups(IDataReader reader)
        {
            IList<HermesActivityGroup> ret = new List<HermesActivityGroup>();

            try
            {
                Helper.ValidateNotNull(reader, "reader");

                while (reader.Read())
                {
                    ret.Add(CreateHermesActivityGroup(reader));
                }
            }
            //Column with given name was not found OR reader is closed OR invalid data types in reader etc.
            catch (Exception e)
            {
                throw Helper.GetSDE(e, e.Message, "HermesScheduleItemPersistenceHelper.BuildActivityGroups",
                    new string[0], new object[0], new string[] { "reader" }, new object[] { reader },
                    new string[] { "ret" }, new object[] { ret }, typeof(InvalidArgumentException));
            }

            return ret;
        }

        /// <summary>
        /// Builds the HermesActivityType list from the passed reader.
        /// Each HermesActivityType is fully constructed with all properties available.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// Column with given name was not found OR reader is closed OR invalid data types in reader etc.
        /// </exception>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>list of activity types in the reader</returns>
        public IList<HermesActivityType> BuildActivityTypes(IDataReader reader)
        {
            IList<HermesActivityType> ret = new List<HermesActivityType>();

            try
            {
                Helper.ValidateNotNull(reader, "reader");

                while (reader.Read())
                {
                    ret.Add(CreateHermesActivityType(reader));
                }
            }
            //Column with given name was not found OR reader is closed OR invalid data types in reader etc.
            catch (Exception e)
            {
                throw Helper.GetSDE(e, e.Message, "HermesScheduleItemPersistenceHelper.BuildActivityTypes",
                    new string[0], new object[0], new string[] { "reader" }, new object[] { reader },
                    new string[] { "ret" }, new object[] { ret }, typeof(InvalidArgumentException));
            }

            return ret;
        }

        /// <summary>
        /// Builds the HermesScheduleItem list from the passed reader.
        /// Each HermesScheduleItem is fully constructed with all properties available.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// Column with given name was not found OR reader is closed OR invalid data types in reader etc.
        /// </exception>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>list of schedule items in the reader</returns>
        public IList<HermesScheduleItem> BuildScheduleItems(IDataReader reader)
        {
            IList<HermesScheduleItem> ret = new List<HermesScheduleItem>();

            try
            {
                Helper.ValidateNotNull(reader, "reader");

                while (reader.Read())
                {
                    ret.Add(CreateHermesScheduleItem(reader));
                }
            }
            //Column with given name was not found OR reader is closed OR invalid data types in reader etc.
            catch (Exception e)
            {
                throw Helper.GetSDE(e, e.Message, "HermesScheduleItemPersistenceHelper.BuildScheduleItems",
                    new string[0], new object[0], new string[] { "reader" }, new object[] { reader },
                    new string[] { "ret" }, new object[] { ret }, typeof(InvalidArgumentException));
            }

            return ret;
        }

        /// <summary>
        /// Builds the HermesScheduleItemStatus list from the passed reader
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// Column with given name was not found OR reader is closed OR invalid data types in reader etc.
        /// </exception>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>list of schedule item statuses in the reader</returns>
        public IList<HermesScheduleItemStatus> BuildScheduleItemStatuses(IDataReader reader)
        {
            IList<HermesScheduleItemStatus> ret = new List<HermesScheduleItemStatus>();

            try
            {
                Helper.ValidateNotNull(reader, "reader");

                while (reader.Read())
                {
                    ret.Add(CreateHermesScheduleItemStatus(reader));
                }
            }
            //Column with given name was not found OR reader is closed OR invalid data types in reader etc.
            catch (Exception e)
            {
                throw Helper.GetSDE(e, e.Message, "HermesScheduleItemPersistenceHelper.BuildScheduleItemStatuses",
                    new string[0], new object[0], new string[] { "reader" }, new object[] { reader },
                    new string[] { "ret" }, new object[] { ret }, typeof(InvalidArgumentException));
            }

            return ret;
        }

        /// <summary>
        /// Builds the HermesScheduleItemRequestStatus list from the passed reader.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// Column with given name was not found OR reader is closed OR invalid data types in reader etc.
        /// </exception>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>List of schedule item request statuses in the reader</returns>
        public IList<HermesScheduleItemRequestStatus> BuildScheduleItemRequestStatuses(IDataReader reader)
        {
            IList<HermesScheduleItemRequestStatus> ret = new List<HermesScheduleItemRequestStatus>();

            try
            {
                Helper.ValidateNotNull(reader, "reader");

                while (reader.Read())
                {
                    ret.Add(CreateHermesScheduleItemRequestStatus(reader));
                }
            }
            //Column with given name was not found OR reader is closed OR invalid data types in reader etc.
            catch (Exception e)
            {
                throw Helper.GetSDE(e, e.Message,
                    "HermesScheduleItemPersistenceHelper.BuildScheduleItemRequestStatuses",
                    new string[0], new object[0], new string[] { "reader" }, new object[] { reader },
                    new string[] { "ret" }, new object[] { ret }, typeof(InvalidArgumentException));
            }

            return ret;
        }

        /// <summary>
        /// Creates a single instance of HermesActivityType corresponding to a particular row of the reader.
        /// </summary>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>Instance of HermesActivityType created using the reader.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If a column is tried to be read from reader but that column does not exist.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// If data in a column is tried to be converted into a type which it cannot be converted to.
        /// </exception>
        private static HermesActivityType CreateHermesActivityType(IDataReader reader)
        {
            HermesActivityType hermesActivityType = new HermesActivityType();

            hermesActivityType.Id = new Guid((byte[])reader["activity_type_ID"]).ToString();
            hermesActivityType.Name = reader["activity_type_nm"] as string;
            hermesActivityType.Abbreviation = reader["activity_type_abbr"] as string;
            hermesActivityType.LastModifiedBy = reader["act_typ_last_modified_by"] as string;
            hermesActivityType.LastModifiedDate = Convert.ToDateTime(reader["act_typ_last_modified_dt"]).ToLocalTime();
            hermesActivityType.ActivityGroup = CreateHermesActivityGroup(reader);

            return hermesActivityType;
        }

        /// <summary>
        /// Creates a single instance of HermesActivityGroup corresponding to a particular row of the reader.
        /// </summary>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>Instance of HermesActivityGroup created using the reader.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If a column is tried to be read from reader but that column does not exist.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// If data in a column is tried to be converted into a type which it cannot be converted to.
        /// </exception>
        private static HermesActivityGroup CreateHermesActivityGroup(IDataReader reader)
        {
            HermesActivityGroup hermesActivityGroup = new HermesActivityGroup();

            hermesActivityGroup.Id = new Guid((byte[])reader["activity_group_ID"]).ToString();
            hermesActivityGroup.Abbreviation = reader["act_grp_abbr"] as string;
            hermesActivityGroup.LastModifiedBy = reader["act_grp_last_modified_by"] as string;
            hermesActivityGroup.LastModifiedDate = Convert.ToDateTime(reader["act_grp_last_modified_dt"]).ToLocalTime();
            hermesActivityGroup.Name = reader["act_grp_nm"] as string;

            return hermesActivityGroup;
        }

        /// <summary>
        /// Creates a single instance of HermesScheduleItemStatus corresponding to a particular row of the reader.
        /// </summary>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>Instance of HermesScheduleItemStatus created using the reader.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If a column is tried to be read from reader but that column does not exist.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// If data in a column is tried to be converted into a type which it cannot be converted to.
        /// </exception>
        private static HermesScheduleItemStatus CreateHermesScheduleItemStatus(IDataReader reader)
        {
            HermesScheduleItemStatus hermesScheduleItemStatus = new HermesScheduleItemStatus();

            hermesScheduleItemStatus.Id = new Guid((byte[])reader["sched_status_ID"]).ToString();
            hermesScheduleItemStatus.Abbreviation = reader["s_status_abbr"] as string;
            hermesScheduleItemStatus.LastModifiedBy = reader["s_last_modified_by"] as string;
            hermesScheduleItemStatus.LastModifiedDate = Convert.ToDateTime(reader["s_last_modified_dt"]).ToLocalTime();
            hermesScheduleItemStatus.Description = reader["s_status_desc"] as string;

            return hermesScheduleItemStatus;
        }

        /// <summary>
        /// Creates a single instance of HermesScheduleItemRequestStatus corresponding to particular row of the reader.
        /// </summary>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>Instance of HermesScheduleItemRequestStatus created using the reader.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If a column is tried to be read from reader but that column does not exist.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// If data in a column is tried to be converted into a type which it cannot be converted to.
        /// </exception>
        private static HermesScheduleItemRequestStatus CreateHermesScheduleItemRequestStatus(IDataReader reader)
        {
            HermesScheduleItemRequestStatus hermesScheduleItemRequestStatus = new HermesScheduleItemRequestStatus();

            hermesScheduleItemRequestStatus.Id = new Guid((byte[])reader["sched_request_status_ID"]).ToString();
            hermesScheduleItemRequestStatus.Abbreviation = reader["rs_status_abbr"] as string;
            hermesScheduleItemRequestStatus.LastModifiedBy = reader["rs_last_modified_by"] as string;
            hermesScheduleItemRequestStatus.LastModifiedDate =
                Convert.ToDateTime(reader["rs_last_modified_dt"]).ToLocalTime();
            hermesScheduleItemRequestStatus.Description = reader["rs_status_desc"] as string;

            return hermesScheduleItemRequestStatus;
        }

        /// <summary>
        /// Creates a single instance of HermesActivity corresponding to a particular row of the reader.
        /// </summary>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>Instance of HermesActivity created using the reader.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If a column is tried to be read from reader but that column does not exist.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// If data in a column is tried to be converted into a type which it cannot be converted to.
        /// </exception>
        private static HermesActivity CreateHermesActivity(IDataReader reader)
        {
            HermesActivity hermesActivity = new HermesActivity();

            hermesActivity.Id = new Guid((byte[])reader["activity_ID"]).ToString();
            hermesActivity.DefaultStartTime =
                (int)(Convert.ToDateTime(reader["default_start_time"]).ToLocalTime().
                Subtract(new DateTime(2001, 1, 1))).TotalSeconds;
            hermesActivity.Abbreviation = reader["activity_abbr"] as string;
            hermesActivity.LastModifiedDate = Convert.ToDateTime(reader["act_last_modified_dt"]).ToLocalTime();
            hermesActivity.DefaultExpireDays = Convert.ToInt32(reader["default_expire_days"]);
            hermesActivity.LastModifiedBy = reader["act_last_modified_by"] as string;
            hermesActivity.WorkDayAmount = Convert.ToInt32(reader["act_work_day_amt"]);
            hermesActivity.DefaultDuration = Convert.ToDecimal(reader["act_duration"]);
            hermesActivity.Name = reader["activity_nm"] as string;
            hermesActivity.ExclusiveFlag = Convert.ToBoolean(reader["exclusive_ind"]);
            hermesActivity.Enabled = Convert.ToBoolean(reader["enabled_ind"]);
            hermesActivity.ActivityType = CreateHermesActivityType(reader);

            return hermesActivity;
        }

        /// <summary>
        /// Creates a single instance of HermesScheduleItem corresponding to a particular row of the reader.
        /// </summary>
        /// <param name="reader">IDataReader with the data</param>
        /// <returns>Instance of HermesScheduleItem created using the reader.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// If a column is tried to be read from reader but that column does not exist.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// If data in a column is tried to be converted into a type which it cannot be converted to.
        /// </exception>
        private static HermesScheduleItem CreateHermesScheduleItem(IDataReader reader)
        {
            HermesScheduleItem hermesScheduleItem = new HermesScheduleItem();

            hermesScheduleItem.Id = new Guid((byte[])reader["schedule_item_ID"]).ToString();
            hermesScheduleItem.ExpirationDate = Convert.ToDateTime(reader["expire_dt"]).ToLocalTime();
            hermesScheduleItem.LastModifiedDate = Convert.ToDateTime(reader["last_modified_dt"]).ToLocalTime();
            hermesScheduleItem.WorkDayAmount = Convert.ToDecimal(reader["work_day_amt"]);
            hermesScheduleItem.LastModifiedBy = reader["last_modified_by"] as string;
            hermesScheduleItem.Version = Convert.ToInt32(reader["version"]);
            hermesScheduleItem.ExceptionFlag = (Convert.ToInt32(reader["exception_ind"]) == 1) ? 'Y' : 'N';
            hermesScheduleItem.Duration = Convert.ToDecimal(reader["duration"]);
            hermesScheduleItem.WorkDate = Convert.ToDateTime(reader["work_dt"]).ToLocalTime();
            hermesScheduleItem.Activity = CreateHermesActivity(reader);
            hermesScheduleItem.ScheduleItemRequestStatus = CreateHermesScheduleItemRequestStatus(reader);
            hermesScheduleItem.ScheduleItemStatus = CreateHermesScheduleItemStatus(reader);

            //Set the note property if needed
            if (reader["note_id"] != DBNull.Value)
            {
                HermesGenericNote note = new HermesGenericNote();
                note.Id = new Guid(reader["note_id"] as byte[]).ToString();
                hermesScheduleItem.Note = note;
            }

            return hermesScheduleItem;
        }
    }
}
