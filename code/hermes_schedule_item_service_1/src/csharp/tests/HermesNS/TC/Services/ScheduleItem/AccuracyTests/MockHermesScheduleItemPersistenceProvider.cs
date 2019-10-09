/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Data;
using System.Transactions;
using System.ServiceModel;
using HermesNS.Entity.Common;
using System.Collections.Generic;
using TopCoder.Util.ObjectFactory;
using TopCoder.Util.ExceptionManager.SDE;
using HermesNS.TC.Services.GenericNotes.Entities;
using TopCoder.Services.WCF.ScheduleItem;
using HermesNS.TC.Services.ScheduleItem.Entities;
using TopCoder.Services.WCF.ScheduleItem.Persistence;
using Hermes.Services.Security.Authorization.Client.Common;

namespace HermesNS.TC.Services.ScheduleItem.Persistence
{
    /// <summary>
    /// <para>
    /// Complete implementation of the IScheduleItemPersistenceProvider. Provide facility for reading,
    /// writing and deleting entities to and from the database. The operations are backed by an Oracle
    /// 10g database, and the OracleConnectionHelper will be used to obtain connections. All operations
    /// will use stored procedures to perform all CRUD steps.</para>
    /// <para>This class goes beyond the interface to allow for the management of published schedule items,
    /// so this implementation will be used directly by the service.</para>
    /// </summary>
    /// <threadsafety>
    /// <para>It is immutable but not thread-safe dues to working with non-thread-safe entities.</para>
    /// </threadsafety>
    /// <author>argolite</author>
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2006, TopCoder, Inc. All rights reserved.</copyright>
    public class MockHermesScheduleItemPersistenceProvider :
        IScheduleItemPersistenceProvider<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
        HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
        HermesGenericNoteItem, HermesGenericNoteItemHistory>
    {
        /// <summary>
        /// Represents the default namespace used by the default constructor to access configuration info in the
        /// construction.
        /// </summary>
        public const string DefaultNamespace =
            "HermesNS.TC.Services.ScheduleItem.Persistence.HermesScheduleItemPersistenceProvider";

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesActivity> activites = new Dictionary<string, HermesActivity>();

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesActivityGroup> activityGroups =
            new Dictionary<string, HermesActivityGroup>();

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesActivityType> activityTypes =
            new Dictionary<string, HermesActivityType>();

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesScheduleItem> items =
            new Dictionary<string, HermesScheduleItem>();

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesScheduleItemStatus> statuses =
            new Dictionary<string, HermesScheduleItemStatus>();

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesScheduleItemRequestStatus> requestStatuses =
            new Dictionary<string, HermesScheduleItemRequestStatus>();

        /// <summary>
        /// Mock used in the accuracy tests.
        /// </summary>
        public static IDictionary<string, HermesScheduleItem> relationTable =
            new Dictionary<string, HermesScheduleItem>();

        /// <summary>
        /// Clear DB
        /// </summary>
        public static void ClearDB()
        {
            activites.Clear();
            activityGroups.Clear();
            activityTypes.Clear();
            items.Clear();
            statuses.Clear();
            requestStatuses.Clear();
            relationTable.Clear();
        }

        /// <summary>
        /// Creates a new MockHermesScheduleItemPersistenceProvider instance using the default namespace.
        /// </summary>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing required values,
        /// or errors while constructing the persistence.
        /// </exception>
        public MockHermesScheduleItemPersistenceProvider() : this(DefaultNamespace)
        {
        }

        /// <summary>
        /// This constructor initializes the connectionName from the Configuration Manager using the
        /// given namespace. The Helper key is read from Configuration Manager and the Helper is obtained
        /// using ObjectFactory from that key.
        /// </summary>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing required values,
        /// or errors while constructing the helper.
        /// </exception>
        /// <exception cref="InvalidArgumentException">If nameSpace given is null or empty</exception>
        /// <param name="nameSpace">Configuration namespace to load the configuration values from</param>
        public MockHermesScheduleItemPersistenceProvider(string nameSpace)
        {
        }

        /// <summary>
        /// Creates a new MockHermesScheduleItemPersistenceProvider insatnce by setting the parameters
        /// to namesake fields.
        /// </summary>
        /// <exception cref="InvalidArgumentException">
        /// If connectionName given is null or empty, or helper is null
        /// </exception>
        /// <param name="connectionName">The connection string with which to connect to database.</param>
        /// <param name="helper">
        /// The IScheduleItemHelperBase instance used for creating entities using IDataReader.
        /// </param>
        public MockHermesScheduleItemPersistenceProvider(string connectionName,
            IScheduleItemHelperBase<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
            HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
            HermesGenericNoteItem, HermesGenericNoteItemHistory> helper)
        {
            
        }

        /// <summary>
        /// Saves the activity, creating a new on if necessary.
        /// </summary>
        /// <exception cref="InvalidArgumentException">If activity is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save</exception>
        /// <param name="activity">the HermesActivity to save</param>
        /// <returns>The saved HermesActivity</returns>
        public HermesActivity SaveActivity(HermesActivity activity)
        {
            if (activity.Name == "Exception")
            {
                throw new Exception();
            }
            if (activites.ContainsKey(activity.Id))
            {
                activites[activity.Id] = activity;
            }
            else
            {
                activites.Add(activity.Id, activity);
            }
            return activity;
        }

        /// <summary>
        /// <para>Saves the activity group, creating a new on if necessary.</para>
        /// </summary>
        /// <exception cref="InvalidArgumentException">If activity group is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save</exception>
        /// <param name="activityGroup">the HermesActivityGroup to save</param>
        /// <returns>The saved HermesActivityGroup</returns>
        public HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        {
            if (activityGroup.Abbreviation == "Exception")
            {
                throw new Exception();
            }
            if (activityGroups.ContainsKey(activityGroup.Id))
            {
                activityGroups[activityGroup.Id] = activityGroup;
            }
            else
            {
                activityGroups.Add(activityGroup.Id, activityGroup);
            }
            return activityGroup;
        }

        /// <summary>
        /// Saves the activity type, creating a new on if necessary.
        /// </summary>
        /// <exception cref="InvalidArgumentException">If activity type is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save</exception>
        /// <param name="activityType">the HermesActivityType to save</param>
        /// <returns>The saved HermesActivityType</returns>
        public HermesActivityType SaveActivityType(HermesActivityType activityType)
        {
            if (activityType.Abbreviation == "Exception")
            {
                throw new Exception();
            }
            if (activityTypes.ContainsKey(activityType.Id))
            {
                activityTypes[activityType.Id] = activityType;
            }
            else
            {
                activityTypes.Add(activityType.Id, activityType);
            }
            return activityType;
        }

        /// <summary>
        /// Saves the schedule item, creating a new on if necessary.
        /// </summary>
        /// <exception cref="InvalidArgumentException">If schedule item is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the save
        /// </exception>
        /// <param name="scheduleItem">the HermesScheduleItem to save</param>
        /// <returns>The saved HermesScheduleItem</returns>
        public HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        {
            if (items.ContainsKey(scheduleItem.Id))
            {
                items[scheduleItem.Id] = scheduleItem;
            }
            else
            {
                items.Add(scheduleItem.Id, scheduleItem);
            }

            return scheduleItem;
        }

        /// <summary>
        /// <para>Saves the schedule item status, creating a new on if necessary.</para>
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
            if (scheduleItemStatus.Abbreviation == "Exception")
            {
                throw new Exception();
            }
            if (statuses.ContainsKey(scheduleItemStatus.Id))
            {
                statuses[scheduleItemStatus.Id] = scheduleItemStatus;
            }
            else
            {
                statuses.Add(scheduleItemStatus.Id, scheduleItemStatus);
            }
            return scheduleItemStatus;
        }

        /// <summary>
        /// <para>Saves the schedule item request status, creating a new on if necessary.</para>
        /// </summary>
        /// <exception cref="InvalidArgumentException">If schedule item request status is null</exception>
        /// <exception cref="ScheduleItemPersistenceException">If there are any errors during the save</exception>
        /// <param name="scheduleRequestStatus">the HermesScheduleItemRequestStatus to save</param>
        /// <returns>The saved HermesScheduleItemRequestStatus</returns>
        public HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
            HermesScheduleItemRequestStatus scheduleRequestStatus)
        {
            if (scheduleRequestStatus.Abbreviation == "Exception")
            {
                throw new Exception();
            }
            if (requestStatuses.ContainsKey(scheduleRequestStatus.Id))
            {
                requestStatuses[scheduleRequestStatus.Id] = scheduleRequestStatus;
            }
            else
            {
                requestStatuses.Add(scheduleRequestStatus.Id, scheduleRequestStatus);
            }
            return scheduleRequestStatus;
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            activites.Remove(id);
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            activityGroups.Remove(id);
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            activityTypes.Remove(id);
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            items.Remove(id);
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            statuses.Remove(id);
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            requestStatuses.Remove(id);
         
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            if (activites.ContainsKey(id))
            {
                return activites[id];
            }
            return null;
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            if (activityGroups.ContainsKey(id))
            {
                return activityGroups[id];
            }
            return null;
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            if (activityTypes.ContainsKey(id))
            {
                return activityTypes[id];
            }
            return null;
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            if (items.ContainsKey(id))
            {
                return items[id];
            }
            return null;
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            if (statuses.ContainsKey(id))
            {
                return statuses[id];
            }
            return null;
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
            if (id == "Exception")
            {
                throw new Exception();
            }
            if (requestStatuses.ContainsKey(id))
            {
                return requestStatuses[id];
            }
            return null;
        }

        /// <summary>
        /// Gets all activities (possibly just active). Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <param name="showDisabled">Flag whether disabled activities should be included in
        /// the returned list</param>
        /// <returns>A list of all activities (possibly just active)</returns>
        public IList<HermesActivity> GetAllActivities(bool showDisabled)
        {
            return new List<HermesActivity>(activites.Values);
        }

        /// <summary>
        /// Gets all activity groups. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all activity groups</returns>
        public IList<HermesActivityGroup> GetAllActivityGroups()
        {
            return new List<HermesActivityGroup>(activityGroups.Values);
        }

        /// <summary>
        /// Gets all activity types. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all activity types</returns>
        public IList<HermesActivityType> GetAllActivityTypes()
        {
            return new List<HermesActivityType>(activityTypes.Values);
        }

        /// <summary>
        /// Gets all schedule item statuses. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all schedule item statuses</returns>
        public IList<HermesScheduleItemStatus> GetAllScheduleItemStatuses()
        {
            return new List<HermesScheduleItemStatus>(statuses.Values);
        }

        /// <summary>
        /// Gets all schedule item request statuses. Returns an empty list if there are none.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">
        /// If there are any errors during the retrieval</exception>
        /// <returns>A list of all schedule item request statuses</returns>
        public IList<HermesScheduleItemRequestStatus> GetAllScheduleItemRequestStatuses()
        {
            return new List<HermesScheduleItemRequestStatus>(requestStatuses.Values);
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
            if (relationTable.ContainsKey(editCopy.Id))
            {
                relationTable[editCopy.Id] = parent;
            }
            else
            {
                relationTable.Add(editCopy.Id, parent);
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
            if (relationTable.ContainsKey(editCopy.Id))
            {
                relationTable.Remove(editCopy.Id);
            }
        }

        /// <summary>
        /// Given a parent, returns its Edit Copy, or null if it does not have a edit copy.
        /// </summary>
        /// <exception cref="ScheduleItemPersistenceException">If there are any errors during the operation</exception>
        /// <param name="parent">HermesScheduleItem whos edit copy is to be retrieved</param>
        /// <returns>The edit copy of the passed parent</returns>
        public HermesScheduleItem GetScheduleItemEditCopy(HermesScheduleItem parent)
        {
            foreach (KeyValuePair<string, HermesScheduleItem> pair in relationTable)
            {
                if (pair.Value.Id == parent.Id)
                {
                    return items[pair.Key];
                }
            }
            return null;
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
            if (relationTable.ContainsKey(editCopy.Id))
            {
                return relationTable[editCopy.Id];
            }
            return null;
        }

        /// <summary>
        /// A mock implementation of the GetDefaultScheduleItemStatus method.
        /// </summary>
        /// <returns>Simply returns the Schedule Item Status for guid
        /// 44444444444444444444444444444444</returns>
        public HermesScheduleItemStatus GetDefaultScheduleItemStatus()
        {
            return GetScheduleItemStatus(new Guid("44444444444444444444444444444444").ToString());
        }

        /// <summary>
        /// A mock implementation of the GetDefaultScheduleItemRequestStatus method.
        /// </summary>
        /// <returns>Simply returns the Schedule Item Request Status for guid
        /// 55555555555555555555555555555555</returns>
        public HermesScheduleItemRequestStatus GetDefaultScheduleItemRequestStatus()
        {
            return GetScheduleItemRequestStatus(new Guid("55555555555555555555555555555555").ToString());
        }
    }
}
