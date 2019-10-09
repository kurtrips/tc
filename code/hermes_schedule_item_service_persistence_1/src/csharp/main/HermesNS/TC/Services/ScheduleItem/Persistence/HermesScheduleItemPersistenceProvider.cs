// HermesScheduleItemPersistenceProvider.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Data;
using System.Transactions;
using System.ServiceModel;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using HermesNS.Entity.Common;
using HermesNS.TC.Services.GenericNotes;
using HermesNS.TC.Services.ScheduleItem.Entities;
using TopCoder.Util.ObjectFactory;
using TopCoder.Services.WCF.ScheduleItem.Persistence;
using TopCoder.Services.WCF.ScheduleItem;
using Hermes.Services.Security.Authorization.Client.Common;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// <para>
    /// Complete implementation of the IScheduleItemPersistenceProvider.
    /// Provide facility for reading, writing and deleting entities to and from the database.
    /// The operations are backed by an Oracle 10g database, and the OracleConnectionHelper will be used to obtain
    /// connections. All operations will use stored procedures to perform all CRUD steps.
    /// </para>
    /// <para>
    /// This class goes beyond the interface to allow for the management of published schedule items,
    /// so this implementation will be used directly by the service.
    /// </para>
    /// <para>
    /// A sample configuration for this class is as below:
    /// &lt;ConfigManager&gt;
    ///  &lt;!--Configuration properties for default namespace--&gt;
    ///  &lt;namespace name="HermesNS.TC.Services.ScheduleItem.Persistence.HermesScheduleItemPersistenceProvider"&gt;
    ///   &lt;property name="objectFactoryNamespace"&gt;
    ///    &lt;value&gt;TestOFNamespace&lt;/value&gt;
    ///   &lt;/property&gt;
    ///   &lt;property name="connectionName"&gt;
    ///    &lt;value&gt;myConnectionName&lt;/value&gt;
    ///   &lt;/property&gt;
    ///   &lt;property name="helper"&gt;
    ///    &lt;value&gt;TestHelper&lt;/value&gt;
    ///   &lt;/property&gt;
    ///  &lt;/namespace&gt;
    ///
    ///  &lt;!--Object Factory's own defintion (using TestOFNamespace)--&gt;
    ///   &lt;namespace name="TestOFNamespace.default"&gt;
    ///    &lt;property name="type_name"&gt;
    ///     &lt;value&gt;TopCoder.Util.ObjectFactory.ConfigurationObjectFactory&lt;/value&gt;
    ///    &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="TestOFNamespace.default.parameters"&gt;
    ///    &lt;property name="p1:string"&gt;
    ///     &lt;value&gt;PersistenceTest&lt;/value&gt;
    ///    &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///
    ///  &lt;!--Object Definition for helper class--&gt;
    ///   &lt;namespace name="PersistenceTest.TestHelper"&gt;
    ///    &lt;property name="type_name"&gt;
    ///     &lt;value&gt;HermesNS.TC.Services.ScheduleItem.Persistence.HermesScheduleItemPersistenceHelper&lt;/value&gt;
    ///    &lt;/property&gt;
    ///   &lt;property name="assembly"&gt;
    ///    &lt;value&gt;HermesScheduleItemServicePersistence.dll&lt;/value&gt;
    ///   &lt;/property&gt;    
    ///  &lt;/namespace&gt;
    ///
    /// &lt;/ConfigManager&gt;
    /// </para>
    /// <para>
    /// Usage of this class is easy:
    /// <code>
    /// HermesScheduleItemPersistenceProvider hsipp = new HermesScheduleItemPersistenceProvider();
    /// hsipp.SaveActivity(activity);
    /// hsipp.GetActivity(activity);
    /// //etc.
    /// </code>
    /// </para>
    /// </summary>
    /// <threadsafety>
    /// <para>It is immutable but not thread-safe dues to working with non-thread-safe entities.</para>
    /// </threadsafety>
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class HermesScheduleItemPersistenceProvider :
        IScheduleItemPersistenceProvider<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
        HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
        HermesGenericNoteItem, HermesGenericNoteItemHistory>
    {
        /// <summary>
        /// Represents the name of the connection to obtain from the OracleConnectionHelper. This is created in the
        /// constructor, and will never change or be null/empty.
        /// </summary>
        private readonly string connectionName;

        /// <summary>
        /// Represents the helper object used to map IDataReader objects to ILists of the appropriate generic type. Set
        /// in the constructor and will not be null.
        /// </summary>
        private readonly IScheduleItemHelperBase<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
            HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
            HermesGenericNoteItem, HermesGenericNoteItemHistory> helper;

        /// <summary>
        /// Represents the default namespace used by the default constructor to access configuration info in the
        /// construction.
        /// </summary>
        public const string DefaultNamespace =
            "HermesNS.TC.Services.ScheduleItem.Persistence.HermesScheduleItemPersistenceProvider";

        /// <summary>
        /// Creates a new HermesScheduleItemPersistenceProvider instance using the default namespace.
        /// </summary>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing required values,
        /// or errors while constructing the persistence.
        /// </exception>
        public HermesScheduleItemPersistenceProvider() : this(DefaultNamespace)
        {
        }

        /// <summary>
        /// This constructor initializes the connectionName from the Configuration Manager using the given namespace.
        /// The Helper key is read from Configuration Manager and the Helper is obtained
        /// using ObjectFactory from that key.
        /// </summary>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing required values,
        /// or errors while constructing the helper.
        /// </exception>
        /// <exception cref="InvalidArgumentException">If nameSpace given is null or empty</exception>
        /// <param name="nameSpace">Configuration namespace to load the configuration values from</param>
        public HermesScheduleItemPersistenceProvider(string nameSpace)
        {
            try
            {
                Helper.ValidateNotNullNotEmpty(nameSpace, "nameSpace");

                //Create Object Factory
                string ofNs = Helper.GetConfigValue(nameSpace, "objectFactoryNamespace", false);
                ObjectFactory of = ofNs == null ?
                    ObjectFactory.GetDefaultObjectFactory() :
                    ObjectFactory.GetDefaultObjectFactory(ofNs);

                //Get the connection name
                connectionName = Helper.GetConfigValue(nameSpace, "connectionName", true);

                //Get the helper
                string helperKey = Helper.GetConfigValue(nameSpace, "helper", true);
                helper = (IScheduleItemHelperBase<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
                    HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
                    HermesGenericNoteItem, HermesGenericNoteItemHistory>)of.CreateDefinedObject(helperKey);
            }
            catch (InvalidArgumentException ive)
            {
                throw Helper.GetSDE(ive, ive.Message, "HermesScheduleItemPersistenceProvider(string)",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "nameSpace" }, new object[] { nameSpace },
                    new string[0], new object[0], null);
            }
            catch (ScheduleItemConfigurationException sice)
            {
                throw Helper.GetSDE(sice, sice.Message, "HermesScheduleItemPersistenceProvider(string)",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "nameSpace" }, new object[] { nameSpace },
                    new string[0], new object[0], null);
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to construct HermesScheduleItemPersistenceProvider instance.",
                    "HermesScheduleItemPersistenceProvider(string)",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "nameSpace" }, new object[] { nameSpace },
                    new string[0], new object[0], typeof(ScheduleItemConfigurationException));
            }
        }

        /// <summary>
        /// Creates a new HermesScheduleItemPersistenceProvider insatnce by setting the parameters to namesake fields.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// If connectionName given is null or empty, or helper is null
        /// </exception>
        /// <param name="connectionName">The connection string with which to connect to database.</param>
        /// <param name="helper">
        /// The IScheduleItemHelperBase instance used for creating entities using IDataReader.
        /// </param>
        public HermesScheduleItemPersistenceProvider(string connectionName,
            IScheduleItemHelperBase<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
            HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
            HermesGenericNoteItem, HermesGenericNoteItemHistory> helper)
        {
            try
            {
                Helper.ValidateNotNullNotEmpty(connectionName, "connectionName");
                Helper.ValidateNotNull(helper, "helper");

                this.connectionName = connectionName;
                this.helper = helper;
            }
            catch (InvalidArgumentException ive)
            {
                throw Helper.GetSDE(ive, ive.Message,
                    "HermesScheduleItemPersistenceProvider(string, IScheduleItemHelperBase)",
                    new string[] { "connectionName", "helper" }, new object[] { this.connectionName, this.helper },
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[0], new object[0], null);
            }
        }

        /// <summary>
        /// Saves the activity, creating a new one if necessary.
        /// </summary>
        /// <exception cref="InvalidArgumentException">If activity is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save</exception>
        /// <param name="activity">the HermesActivity to save</param>
        /// <returns>The saved HermesActivity</returns>
        public HermesActivity SaveActivity(HermesActivity activity)
        {
            try
            {
                Helper.ValidateNotNull(activity, "activity");
                Helper.ValidateGuid(activity.Id);

                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command = new OracleCommand("schedule_item.sp_save_activity", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_activity_id", OracleDbType.Raw).Value =
                                new Guid(activity.Id).ToByteArray();
                            command.Parameters.Add("p_activity_type_id", OracleDbType.Raw).Value =
                                new Guid(activity.ActivityType.Id).ToByteArray();
                            command.Parameters.Add("p_default_start_time", OracleDbType.Date).Value =
                                new DateTime(2001, 1, 1).ToUniversalTime().AddSeconds(activity.DefaultStartTime);
                            command.Parameters.Add("p_last_modified_dt", OracleDbType.Date).Value =
                                activity.LastModifiedDate.ToUniversalTime();
                            command.Parameters.Add("p_default_expire_days", OracleDbType.Int32).Value =
                                activity.DefaultExpireDays;
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                activity.LastModifiedBy;
                            command.Parameters.Add("p_work_day_amt", OracleDbType.Int32).Value =
                                activity.WorkDayAmount;
                            command.Parameters.Add("p_duration", OracleDbType.Decimal).Value =
                                activity.DefaultDuration;
                            command.Parameters.Add("p_activity_nm", OracleDbType.Varchar2).Value =
                                activity.Name;
                            command.Parameters.Add("p_enabled_ind", OracleDbType.Int32).Value =
                                activity.Enabled;
                            command.Parameters.Add("p_activity_abbr", OracleDbType.Varchar2).Value =
                                activity.Abbreviation;
                            command.Parameters.Add("p_exclusive_ind", OracleDbType.Int32).Value =
                                activity.ExclusiveFlag;

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    scope.Complete();
                }
                return activity;
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to save activity.", "SaveActivity",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "activity" }, new object[] { activity },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// <para>Saves the activity group, creating a new one if necessary.</para>
        /// </summary>
        /// <exception cref="InvalidArgumentException">If activity group is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save</exception>
        /// <param name="activityGroup">the HermesActivityGroup to save</param>
        /// <returns>The saved HermesActivityGroup</returns>
        public HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        {
            try
            {
                Helper.ValidateNotNull(activityGroup, "activityGroup");
                Helper.ValidateGuid(activityGroup.Id);

                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_save_activity_group", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_activity_group_ID", OracleDbType.Raw).Value =
                                new Guid(activityGroup.Id).ToByteArray();
                            command.Parameters.Add("p_last_modified_dt", OracleDbType.Date).Value =
                                activityGroup.LastModifiedDate.ToUniversalTime();
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                activityGroup.LastModifiedBy;
                            command.Parameters.Add("p_act_grp_abbr", OracleDbType.Varchar2).Value =
                                activityGroup.Abbreviation;
                            command.Parameters.Add("p_act_grp_nm", OracleDbType.Varchar2).Value =
                                activityGroup.Name;

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    scope.Complete();
                }
                return activityGroup;
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to save activity group.", "SaveActivityGroup",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "activityGroup" }, new object[] { activityGroup },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Saves the activity type, creating a new one if necessary.
        /// </summary>
        /// <exception cref="InvalidArgumentException">If activity type is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save</exception>
        /// <param name="activityType">the HermesActivityType to save</param>
        /// <returns>The saved HermesActivityType</returns>
        public HermesActivityType SaveActivityType(HermesActivityType activityType)
        {
            try
            {
                Helper.ValidateNotNull(activityType, "activityType");
                Helper.ValidateGuid(activityType.Id);

                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_save_activity_type", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_activity_type_ID", OracleDbType.Raw).Value =
                                new Guid(activityType.Id).ToByteArray();
                            command.Parameters.Add("p_activity_group_ID", OracleDbType.Raw).Value =
                                new Guid(activityType.ActivityGroup.Id).ToByteArray();
                            command.Parameters.Add("p_activity_type_abbr", OracleDbType.Varchar2).Value =
                                activityType.Abbreviation;
                            command.Parameters.Add("p_last_modified_date", OracleDbType.Date).Value =
                                activityType.LastModifiedDate.ToUniversalTime();
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                activityType.LastModifiedBy;
                            command.Parameters.Add("p_activity_type_nm", OracleDbType.Varchar2).Value =
                                activityType.Name;

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    //Complete transaction
                    scope.Complete();
                }

                return activityType;
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to save activity type.", "SaveActivityType",
                    new string[] {"connectionName", "helper"}, new object[] {connectionName, helper},
                    new string[] {"activityType"}, new object[] {activityType},
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Saves the schedule item, creating a new one if necessary.
        /// </summary>
        /// <exception cref="InvalidArgumentException">If schedule item is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save
        /// </exception>
        /// <param name="scheduleItem">the HermesScheduleItem to save</param>
        /// <returns>The saved HermesScheduleItem</returns>
        public HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        {
            try
            {
                Helper.ValidateNotNull(scheduleItem, "scheduleItem");
                Helper.ValidateGuid(scheduleItem.Id);

                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_save_sched_item", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_schedule_item_id", OracleDbType.Raw).Value =
                                new Guid(scheduleItem.Id).ToByteArray();
                            command.Parameters.Add("p_sched_request_status_id", OracleDbType.Raw).Value =
                                new Guid(scheduleItem.ScheduleItemRequestStatus.Id).ToByteArray();
                            command.Parameters.Add("p_sched_status_id", OracleDbType.Raw).Value =
                                new Guid(scheduleItem.ScheduleItemStatus.Id).ToByteArray();

                            //The note or its id can be null.
                            if (scheduleItem.Note != null && scheduleItem.Note.Id != null)
                            {
                                command.Parameters.Add("p_note_id", OracleDbType.Raw).Value =
                                    new Guid(scheduleItem.Note.Id).ToByteArray();
                                command.Parameters.Add("p_sched_role_note_id", OracleDbType.Raw).Value =
                                    new Guid(scheduleItem.Note.Id).ToByteArray();
                            }
                            else
                            {
                                command.Parameters.Add("p_note_id", OracleDbType.Raw).Value = null;
                                command.Parameters.Add("p_sched_role_note_id", OracleDbType.Raw).Value = null;
                            }

                            command.Parameters.Add("p_activity_id", OracleDbType.Raw).Value =
                                new Guid(scheduleItem.Activity.Id).ToByteArray();
                            command.Parameters.Add("p_last_modified_dt", OracleDbType.Date).Value =
                                scheduleItem.LastModifiedDate.ToUniversalTime();
                            command.Parameters.Add("p_work_day_amt", OracleDbType.Decimal).Value =
                                scheduleItem.WorkDayAmount;
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                scheduleItem.LastModifiedBy;
                            command.Parameters.Add("p_version", OracleDbType.Int32).Value =
                                scheduleItem.Version;
                            command.Parameters.Add("p_duration", OracleDbType.Decimal).Value =
                                scheduleItem.Duration;
                            command.Parameters.Add("p_exception_ind", OracleDbType.Int32).Value =
                                scheduleItem.ExceptionFlag == 'Y' ? 1 : 0;
                            command.Parameters.Add("p_expire_dt", OracleDbType.Date).Value =
                                scheduleItem.ExpirationDate.ToUniversalTime();
                            command.Parameters.Add("p_work_dt", OracleDbType.Date).Value =
                                scheduleItem.WorkDate.ToUniversalTime();

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    //Complete transaction
                    scope.Complete();
                }

                return scheduleItem;
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to save schedule item.", "SaveScheduleItem",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "scheduleItem" }, new object[] { scheduleItem },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// <para>Saves the schedule item status, creating a new one if necessary.</para>
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// InvalidArgumentException If schedule item status is null
        /// </exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// ScheduleItemPersistenceException If there are any errors during the save
        /// </exception>
        /// <param name="scheduleItemStatus">the HermesScheduleItemStatus to save</param>
        /// <returns>The saved HermesScheduleItemStatus</returns>
        public HermesScheduleItemStatus SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)
        {
            try
            {
                Helper.ValidateNotNull(scheduleItemStatus, "scheduleItemStatus");
                Helper.ValidateGuid(scheduleItemStatus.Id);

                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_save_schedule_item_status", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_sched_status_ID", OracleDbType.Raw).Value =
                                new Guid(scheduleItemStatus.Id).ToByteArray();
                            command.Parameters.Add("p_last_modified_dt", OracleDbType.Date).Value =
                                scheduleItemStatus.LastModifiedDate.ToUniversalTime();
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                scheduleItemStatus.LastModifiedBy;
                            command.Parameters.Add("p_status_desc", OracleDbType.Varchar2).Value =
                                scheduleItemStatus.Description;
                            command.Parameters.Add("p_status_abbr", OracleDbType.Varchar2).Value =
                                scheduleItemStatus.Abbreviation;

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    //Complete transaction
                    scope.Complete();
                }

                return scheduleItemStatus;
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to save schedule item status.", "SaveScheduleItemStatus",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "scheduleItemStatus" }, new object[] { scheduleItemStatus },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// <para>Saves the schedule item request status, creating a new one if necessary.</para>
        /// </summary>
        /// <exception cref="InvalidArgumentException">If schedule item request status is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">If there are any errors during the save</exception>
        /// <param name="scheduleRequestStatus">the HermesScheduleItemRequestStatus to save</param>
        /// <returns>The saved HermesScheduleItemRequestStatus</returns>
        public HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
            HermesScheduleItemRequestStatus scheduleRequestStatus)
        {
            try
            {
                Helper.ValidateNotNull(scheduleRequestStatus, "scheduleRequestStatus");
                Helper.ValidateGuid(scheduleRequestStatus.Id);

                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_save_sched_item_req_stat", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_sched_req_status_ID", OracleDbType.Raw).Value =
                                new Guid(scheduleRequestStatus.Id).ToByteArray();
                            command.Parameters.Add("p_last_modified_dt", OracleDbType.Date).Value =
                                scheduleRequestStatus.LastModifiedDate.ToUniversalTime();
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                scheduleRequestStatus.LastModifiedBy;
                            command.Parameters.Add("p_status_desc", OracleDbType.Varchar2).Value =
                                scheduleRequestStatus.Description;
                            command.Parameters.Add("p_status_abbr", OracleDbType.Varchar2).Value =
                                scheduleRequestStatus.Abbreviation;

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    //Complete transaction
                    scope.Complete();
                }

                return scheduleRequestStatus;
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to save schedule item request status.", "SaveScheduleItemRequestStatus",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "scheduleRequestStatus" }, new object[] { scheduleRequestStatus },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Deletes the activity given its primary key id
        /// </summary>
        /// <exception cref="EntityNotFoundException">
        /// If the activity with the given ID is not found in the persistence.
        /// </exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the delete.
        /// </exception>
        /// <param name="id">The primary ID of the activity to delete.</param>
        public void DeleteActivity(string id)
        {
            try
            {
                using (OracleConnection connection = Helper.GetConnection(connectionName))
                {
                    using (OracleCommand command =
                        new OracleCommand("schedule_item.sp_delete_activity", connection))
                    {
                        //Set type of command
                        command.CommandType = CommandType.StoredProcedure;

                        //Set the parameters
                        command.Parameters.Add("p_activity_id", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add("p_rows_affected", OracleDbType.Int32).Direction =
                            ParameterDirection.InputOutput;

                        //Call the procedure
                        connection.Open();
                        command.ExecuteNonQuery();

                        if ((int)command.Parameters["p_rows_affected"].Value == 0)
                        {
                            throw new EntityNotFoundException(
                                "Activity with id: " + id + " was not found in database.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete activity.", "DeleteActivity",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "id" }, new object[] { id },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Deletes the activity group given its primary key id.
        /// </summary>
        /// <exception cref="EntityNotFoundException">
        /// If the activity group with the given ID is not found in the persistence</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the delete</exception>
        /// <param name="id">The primary ID of the activity group to delete</param>
        public void DeleteActivityGroup(string id)
        {
            try
            {
                using (OracleConnection connection = Helper.GetConnection(connectionName))
                {
                    using (OracleCommand command =
                        new OracleCommand("schedule_item.sp_delete_activity_group", connection))
                    {
                        //Set type of command
                        command.CommandType = CommandType.StoredProcedure;

                        //Set the parameters
                        command.Parameters.Add("p_activity_group_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add("p_rows_affected", OracleDbType.Int32).Direction =
                            ParameterDirection.InputOutput;

                        //Call the procedure
                        connection.Open();
                        command.ExecuteNonQuery();

                        if ((int)command.Parameters["p_rows_affected"].Value == 0)
                        {
                            throw new EntityNotFoundException(
                                "Activity group with id: " + id + " was not found in database.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete activity group.", "DeleteActivityGroup",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "id" }, new object[] { id },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Deletes the activity type given its primary key id.
        /// </summary>
        /// <exception cref="EntityNotFoundException">
        /// If the activity type with the given ID is not found in the persistence
        /// </exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the delete
        /// </exception>
        /// <param name="id">The primary ID of the activity type to delete</param>
        public void DeleteActivityType(string id)
        {
            try
            {
                using (OracleConnection connection = Helper.GetConnection(connectionName))
                {
                    using (OracleCommand command =
                        new OracleCommand("schedule_item.sp_delete_activity_type", connection))
                    {
                        //Set type of command
                        command.CommandType = CommandType.StoredProcedure;

                        //Set the parameters
                        command.Parameters.Add("p_activity_type_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add("p_rows_affected", OracleDbType.Int32).Direction =
                            ParameterDirection.InputOutput;

                        //Call the procedure
                        connection.Open();
                        command.ExecuteNonQuery();

                        if ((int)command.Parameters["p_rows_affected"].Value == 0)
                        {
                            throw new EntityNotFoundException(
                                "Activity type with id: " + id + " was not found in database.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete activity type.", "DeleteActivityType",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "id" }, new object[] { id },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Deletes the schedule item given its primary key id.
        /// </summary>
        /// <exception cref="EntityNotFoundException">
        /// If the entity with the given ID is not found in the persistence
        /// </exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the delete
        /// </exception>
        /// <param name="id">The primary ID of the entity to delete</param>
        public void DeleteScheduleItem(string id)
        {
            try
            {
                using (OracleConnection connection = Helper.GetConnection(connectionName))
                {
                    using (OracleCommand command =
                        new OracleCommand("schedule_item.sp_delete_sched_item", connection))
                    {
                        //Set type of command
                        command.CommandType = CommandType.StoredProcedure;

                        //Set the parameters
                        command.Parameters.Add("p_schedule_item_id", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add("p_rows_affected", OracleDbType.Int32).Direction =
                            ParameterDirection.InputOutput;

                        //Call the procedure
                        connection.Open();
                        command.ExecuteNonQuery();

                        if ((int)command.Parameters["p_rows_affected"].Value == 0)
                        {
                            throw new EntityNotFoundException(
                                "Schedule item with id: " + id + " was not found in database.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete schedule item.", "DeleteScheduleItem",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "id" }, new object[] { id },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Deletes the schedule item status given its primary key id.
        /// </summary>
        /// <exception cref="EntityNotFoundException">
        /// If the schedule item status with the given ID is not found in the persistence
        /// </exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the delete
        /// </exception>
        /// <param name="id">The primary ID of the schedule item status to delete</param>
        public void DeleteScheduleItemStatus(string id)
        {
            try
            {
                using (OracleConnection connection = Helper.GetConnection(connectionName))
                {
                    using (OracleCommand command =
                        new OracleCommand("schedule_item.sp_delete_schedule_item_status", connection))
                    {
                        //Set type of command
                        command.CommandType = CommandType.StoredProcedure;

                        //Set the parameters
                        command.Parameters.Add("p_sched_status_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add("p_rows_affected", OracleDbType.Int32).Direction =
                            ParameterDirection.InputOutput;

                        //Call the procedure
                        connection.Open();
                        command.ExecuteNonQuery();

                        if ((int)command.Parameters["p_rows_affected"].Value == 0)
                        {
                            throw new EntityNotFoundException(
                                "Schedule item status with id: " + id + " was not found in database.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete schedule item status.", "DeleteScheduleItemStatus",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "id" }, new object[] { id },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Deletes the schedule item request status given its primary key id.
        /// </summary>
        /// <exception cref="EntityNotFoundException">
        /// If the schedule item request status with the given ID is not found in the persistence.
        /// </exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the delete
        /// </exception>
        /// <param name="id">The primary ID of the schedule item request status to delete</param>
        public void DeleteScheduleItemRequestStatus(string id)
        {
            try
            {
                using (OracleConnection connection = Helper.GetConnection(connectionName))
                {
                    using (OracleCommand command =
                        new OracleCommand("schedule_item.sp_delete_sched_item_req_stat", connection))
                    {
                        //Set type of command
                        command.CommandType = CommandType.StoredProcedure;

                        //Set the parameters
                        command.Parameters.Add("p_sched_req_status_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add("p_rows_affected", OracleDbType.Int32).Direction =
                            ParameterDirection.InputOutput;

                        //Call the procedure
                        connection.Open();
                        command.ExecuteNonQuery();

                        if ((int)command.Parameters["p_rows_affected"].Value == 0)
                        {
                            throw new EntityNotFoundException(
                                "Schedule item request status with id: " + id + " was not found in database.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete schedule item request status.",
                    "DeleteScheduleItemRequestStatus",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "id" }, new object[] { id },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Gets the activity with the given ID, or null if not found.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="id">The id of the activity</param>
        /// <returns>The activity or null if not found</returns>
        public HermesActivity GetActivity(string id)
        {
            IList<HermesActivity> retHermesActivities = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_activity", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_activity_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesActivities = helper.BuildActivities(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get activity.", "GetActivity",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "id" }, new object[] { id },
                            new string[] { "retHermesActivities" }, new object[] { retHermesActivities },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }
            return retHermesActivities.Count == 0 ? null : retHermesActivities[0];
        }

        /// <summary>
        /// Gets the activity group with the given ID, or null if not found.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="id">The id of the activity group</param>
        /// <returns>The activity group or null if not found</returns>
        public HermesActivityGroup GetActivityGroup(string id)
        {
            IList<HermesActivityGroup> retHermesActivityGroups = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_activity_group", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_activity_group_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesActivityGroups = helper.BuildActivityGroups(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get activity group.", "GetActivityGroup",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "id" }, new object[] { id },
                            new string[] { "retHermesActivityGroups" }, new object[] { retHermesActivityGroups },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }
            return retHermesActivityGroups.Count == 0 ? null : retHermesActivityGroups[0];
        }

        /// <summary>
        /// Gets the activity type with the given ID, or null if not found.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// ScheduleItemPersistenceException If there are any errors during the retrieval
        /// </exception>
        /// <param name="id">The id of the activity type</param>
        /// <returns>The activity type or null if not found</returns>
        public HermesActivityType GetActivityType(string id)
        {
            IList<HermesActivityType> retActivityTypes = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_activity_type", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_activity_type_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retActivityTypes = helper.BuildActivityTypes(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get activity type.", "GetActivityType",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "id" }, new object[] { id },
                            new string[] { "retActivityTypes" }, new object[] { retActivityTypes },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }
            return retActivityTypes.Count == 0 ? null : retActivityTypes[0];
        }

        /// <summary>
        /// Gets the schedule item with the given ID, or null if not found.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="id">The id of the schedule item</param>
        /// <returns>The schedule item or null if not found</returns>
        public HermesScheduleItem GetScheduleItem(string id)
        {
            IList<HermesScheduleItem> retHermesScheduleItems = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_sched_item", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_schedule_item_id", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesScheduleItems = helper.BuildScheduleItems(reader);
                        }
                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get schedule item.", "GetScheduleItem",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "id" }, new object[] { id },
                            new string[] { "retHermesScheduleItems" }, new object[] { retHermesScheduleItems },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesScheduleItems.Count == 0 ? null : retHermesScheduleItems[0];
        }

        /// <summary>
        /// Gets the schedule item status with the given ID, or null if not found.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="id">The id of the schedule item status</param>
        /// <returns>The schedule item status or null if not found</returns>
        public HermesScheduleItemStatus GetScheduleItemStatus(string id)
        {
            IList<HermesScheduleItemStatus> retHermesScheduleItemStatuses = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_schedule_item_status", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_sched_status_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesScheduleItemStatuses = helper.BuildScheduleItemStatuses(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get schedule item status.", "GetScheduleItemStatus",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "id" }, new object[] { id },
                            new string[] { "retHermesScheduleItemStatuses" },
                            new object[] { retHermesScheduleItemStatuses },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesScheduleItemStatuses.Count == 0 ? null : retHermesScheduleItemStatuses[0];
        }

        /// <summary>
        /// Gets the schedule item request status with the given ID, or null if not found.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="id">The id of the schedule item request status</param>
        /// <returns>The schedule item request status or null if not found</returns>
        public HermesScheduleItemRequestStatus GetScheduleItemRequestStatus(string id)
        {
            IList<HermesScheduleItemRequestStatus> retHermesScheduleItemRequestStatuses = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_sched_item_req_stat", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_sched_req_status_ID", OracleDbType.Raw).Value =
                            new Guid(id).ToByteArray();
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesScheduleItemRequestStatuses = helper.BuildScheduleItemRequestStatuses(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get schedule item request status.",
                            "GetScheduleItemRequestStatus",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "id" }, new object[] { id },
                            new string[] { "retHermesScheduleItems" },
                            new object[] { retHermesScheduleItemRequestStatuses },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesScheduleItemRequestStatuses.Count == 0 ? null : retHermesScheduleItemRequestStatuses[0];
        }

        /// <summary>
        /// Gets all activities (possibly just active). Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="showDisabled">Flag whether disabled activities should be included in the returned list</param>
        /// <returns>A list of all activities (possibly just active)</returns>
        public IList<HermesActivity> GetAllActivities(bool showDisabled)
        {
            IList<HermesActivity> retHermesActivities = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command = new OracleCommand("schedule_item.sp_get_all_activities", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add("p_hideDisabled", OracleDbType.Int32).Value =
                            showDisabled ? 0 : 1;
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesActivities = helper.BuildActivities(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get all activities.", "GetAllActivities",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[] { "showDisabled" }, new object[] { showDisabled },
                            new string[] { "retHermesActivities" }, new object[] { retHermesActivities },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesActivities;
        }

        /// <summary>
        /// Gets all activity groups. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all activity groups</returns>
        public IList<HermesActivityGroup> GetAllActivityGroups()
        {
            IList<HermesActivityGroup> retHermesActivityGroups = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_activity_groups", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesActivityGroups = helper.BuildActivityGroups(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get all activity groups.", "GetAllActivityGroups",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[0], new object[0],
                            new string[] { "retHermesActivityGroups" }, new object[] { retHermesActivityGroups },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesActivityGroups;
        }

        /// <summary>
        /// Gets all activity types. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all activity types</returns>
        public IList<HermesActivityType> GetAllActivityTypes()
        {
            IList<HermesActivityType> retHermesActivityTypes = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_activity_types", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesActivityTypes = helper.BuildActivityTypes(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get all activity types.", "GetAllActivityTypes",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[0], new object[0],
                            new string[] { "retHermesActivityTypes" }, new object[] { retHermesActivityTypes },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesActivityTypes;
        }

        /// <summary>
        /// Gets all schedule item statuses. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all schedule item statuses</returns>
        public IList<HermesScheduleItemStatus> GetAllScheduleItemStatuses()
        {
            IList<HermesScheduleItemStatus> retHermesScheduleItemStatuses = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_sched_item_stats", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesScheduleItemStatuses = helper.BuildScheduleItemStatuses(reader);
                        }

                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get all schedule item statuses.",
                            "GetAllScheduleItemStatuses",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[0], new object[0],
                            new string[] { "retHermesScheduleItemStatuses" },
                            new object[] { retHermesScheduleItemStatuses },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesScheduleItemStatuses;
        }

        /// <summary>
        /// Gets all schedule item request statuses. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all schedule item request statuses</returns>
        public IList<HermesScheduleItemRequestStatus> GetAllScheduleItemRequestStatuses()
        {
            IList<HermesScheduleItemRequestStatus> retHermesScheduleItemRequestStatuses = null;

            using (OracleConnection connection = Helper.GetConnection(connectionName))
            {
                using (OracleCommand command =
                    new OracleCommand("schedule_item.sp_get_all_sched_item_req_stat", connection))
                {
                    //Set type of command
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        //Set the parameters
                        command.Parameters.Add(new OracleParameter("p_results_cursor", OracleDbType.RefCursor))
                            .Direction = ParameterDirection.Output;

                        //Call the procedure
                        connection.Open();
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            retHermesScheduleItemRequestStatuses = helper.BuildScheduleItemRequestStatuses(reader);
                        }
                    }
                    catch (Exception e)
                    {
                        throw Helper.GetSDE(e, "Unable to get all schedule item request statuses.",
                            "GetAllScheduleItemRequestStatuses",
                            new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                            new string[0], new object[0],
                            new string[] { "retHermesScheduleItemRequestStatuses" },
                            new object[] { retHermesScheduleItemRequestStatuses },
                            typeof(ScheduleItemPersistenceException));
                    }
                }
            }

            return retHermesScheduleItemRequestStatuses;
        }

        /// <summary>
        /// Relates a parent HermesScheduleItem entity to its editCopy.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the operation</exception>
        /// <param name="parent">
        /// HermesScheduleItem whose association is to be created with the passed edit copy
        /// </param>
        /// <param name="editCopy">HermesScheduleItem whose association is to be created with the passed parent</param>
        public void CreateScheduleItemPublishEditCopyRelationship(
            HermesScheduleItem parent, HermesScheduleItem editCopy)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_relate_sched_items", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_published_schedule_item_id", OracleDbType.Raw).Value =
                                new Guid(parent.Id).ToByteArray();
                            command.Parameters.Add("p_edit_schedule_item_id", OracleDbType.Raw).Value =
                                new Guid(editCopy.Id).ToByteArray();
                            command.Parameters.Add("p_last_modified_dt", OracleDbType.Date).Value =
                                DateTime.Today.ToUniversalTime();

                            Profile profile = WcfHelper.GetProfileFromContext(OperationContext.Current);
                            command.Parameters.Add("p_last_modified_by", OracleDbType.Varchar2).Value =
                                profile.UserID;

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to create relation between parent schedule item and its edit copy.",
                    "CreateScheduleItemPublishEditCopyRelationship",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "parent", "editCopy" }, new object[] { parent, editCopy },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Removes the relationship between the edit copy and its parent.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the operation</exception>
        /// <param name="editCopy">HermesScheduleItem whose association with the parent is to be removed</param>
        public void DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem editCopy)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_delete_sched_item_relation", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_edit_schedule_item_id", OracleDbType.Raw).Value =
                                new Guid(editCopy.Id).ToByteArray();

                            //Call the procedure
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to delete relation between parent schedule item and its edit copy.",
                    "DeleteScheduleItemPublishEditCopyRelationship",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "editCopy" }, new object[] { editCopy },
                    new string[0], new object[0], typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Given a parent, returns its Edit Copy, or null if it does not have a edit copy.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">If there are any errors during the operation</exception>
        /// <param name="parent">HermesScheduleItem whose edit copy is to be retrieved</param>
        /// <returns>The edit copy of the passed parent</returns>
        public HermesScheduleItem GetScheduleItemEditCopy(HermesScheduleItem parent)
        {
            IList<HermesScheduleItem> retHermesScheduleItem = null;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_get_sched_item_edit_copy", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_published_schedule_item_id", OracleDbType.Raw).Value =
                                new Guid(parent.Id).ToByteArray();
                            command.Parameters.Add("p_results_cursor", OracleDbType.RefCursor).Direction =
                                ParameterDirection.Output;

                            //Call the procedure
                            connection.Open();
                            using (OracleDataReader reader = command.ExecuteReader())
                            {
                                retHermesScheduleItem = helper.BuildScheduleItems(reader);
                            }
                        }
                    }

                    scope.Complete();
                }

                return retHermesScheduleItem.Count == 0 ? null : retHermesScheduleItem[0];
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to get edit copy for given parent schedule item.",
                    "GetScheduleItemEditCopy",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "parent" }, new object[] { parent },
                    new string[] { "retHermesScheduleItem" }, new object[] { retHermesScheduleItem },
                    typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Given an Edit Copy, returns the parent, or null if it does not have a parent.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the operation</exception>
        /// <param name="editCopy">HermesScheduleItem whose parent is to be retrieved</param>
        /// <returns>The parent of the passed edit copy</returns>
        public HermesScheduleItem GetScheduleItemParentCopy(HermesScheduleItem editCopy)
        {
            IList<HermesScheduleItem> retHermesScheduleItem = null;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (OracleConnection connection = Helper.GetConnection(connectionName))
                    {
                        using (OracleCommand command =
                            new OracleCommand("schedule_item.sp_get_sched_item_parent", connection))
                        {
                            //Set type of command
                            command.CommandType = CommandType.StoredProcedure;

                            //Set the parameters
                            command.Parameters.Add("p_edit_schedule_item_ID", OracleDbType.Raw).Value =
                                new Guid(editCopy.Id).ToByteArray();
                            command.Parameters.Add("p_results_cursor", OracleDbType.RefCursor).Direction =
                                ParameterDirection.Output;

                            //Call the procedure
                            connection.Open();
                            using (OracleDataReader reader = command.ExecuteReader())
                            {
                                retHermesScheduleItem = helper.BuildScheduleItems(reader);
                            }
                        }
                    }

                    scope.Complete();
                }

                return retHermesScheduleItem.Count == 0 ? null : retHermesScheduleItem[0];
            }
            catch (Exception e)
            {
                throw Helper.GetSDE(e, "Unable to get parent for given edit copy of schedule item.",
                    "GetScheduleItemParentCopy",
                    new string[] { "connectionName", "helper" }, new object[] { connectionName, helper },
                    new string[] { "editCopy" }, new object[] { editCopy },
                    new string[] { "retHermesScheduleItem" }, new object[] { retHermesScheduleItem },
                    typeof(ScheduleItemPersistenceException));
            }
        }

        /// <summary>
        /// Gets the default ScheduleItemStatus present in the database.
        /// Simply returns the first record in the database.
        /// </summary>
        /// <returns>Simply returns the first record in the database.</returns>
        public HermesScheduleItemStatus GetDefaultScheduleItemStatus()
        {
            return GetAllScheduleItemStatuses()[0];
        }

        /// <summary>
        /// Gets the default ScheduleItemRequestStatus present in the database.
        /// Simply returns the first record in the database.
        /// </summary>
        /// <returns>Simply returns the first record in the database.</returns>
        public HermesScheduleItemRequestStatus GetDefaultScheduleItemRequestStatus()
        {
            return GetAllScheduleItemRequestStatuses()[0];
        }
    }
}
