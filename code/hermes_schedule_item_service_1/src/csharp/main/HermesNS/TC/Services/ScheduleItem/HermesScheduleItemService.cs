/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Transactions;
using System.ServiceModel;
using System.Collections.Generic;
using Oracle.DataAccess.Client;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Util.ObjectFactory;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Services.WCF;
using TopCoder.Services.WCF.Audit;
using TopCoder.Services.WCF.Audit.Entities;
using TopCoder.Services.WCF.ScheduleItem;
using TopCoder.Services.WCF.ScheduleItem.Persistence;
using TopCoder.Services.WCF.ScheduleItem.Entities;
using HermesNS.TC.Services.ScheduleItem.Entities;
using HermesNS.TC.Services.ScheduleItem.Validators;
using HermesNS.TC.Services.ScheduleItem.Persistence;
using HermesNS.TC.Services.GenericNotes.Entities;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.LoggingWrapperPublisher;
using HermesNS.Entity.Common;
using HermesNS.TC.Services.AuditTrail;
using HermesNS.TC.Services.ScheduleItem.Clients;
using Hermes.Services.Security.Authorization.Client.Common;

namespace HermesNS.TC.Services.ScheduleItem
{
    /// <summary>
    /// <para>
    /// This class uses the concrete Hermes types provided by the companion Hermes Schedule Item Entities component,
    /// string for the Id, and HermesGenericNote for ScheduleItemServiceBase.
    /// This class extends ScheduleItemServiceBase to add a layer of validation, authentication, and auditing.
    /// It implements all methods. Most calls involve a combination of authentication, validation, and auditing before
    /// deferring to the base method.
    /// It also uses the Generic Note Service to manage Generic Notes of the HermesScheduleItem.
    /// Validation is performed using the Hermes validator implementations in the companion
    /// Hermes Schedule Item Entities component, and auditing is done mostly in the entities’ Audit methods.
    /// To manage publishing of schedule items, this service implementation provides additional methods not
    /// given in the base class. As such, although it obtains the persistence via ObjectFactory,
    /// it still uses the provided implementation directly to perform publishing management.
    /// </para>
    /// <para>
    /// The following configuration can be used for setting up this component.
    /// </para>
    /// <para>
    ///   &lt;namespace name="HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService"&gt;
    ///     &lt;property name="objectFactoryNamespace"&gt;
    ///       &lt;value&gt;TestOFNamespace&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="auditClientKey"&gt;
    ///       &lt;value&gt;TestAuditClientKey&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="genericNotesClientKey"&gt;
    ///       &lt;value&gt;TestGenericNotesClientKey&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="persistence"&gt;
    ///       &lt;value&gt;TestPersistenceKey&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   
    ///   &lt;namespace name="TestOFNamespace.default"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;TopCoder.Util.ObjectFactory.ConfigurationObjectFactory&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="TestOFNamespace.default.parameters"&gt;
    ///     &lt;property name="p1:string"&gt;
    ///       &lt;value&gt;HermesScheduleItemService&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;  
    ///   
    ///   &lt;!--Object definition for the audit client--&gt;
    ///   &lt;namespace name="HermesScheduleItemService.TestAuditClientKey"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;HermesAuditTrailSaveServiceClient&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="assembly"&gt;
    ///       &lt;value&gt;HermesNS.TC.Services.ScheduleItem.Entities.dll&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.TestAuditClientKey.parameters"&gt;
    ///     &lt;property name="p1:object"&gt;
    ///       &lt;value&gt;AuditBindingKey&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="p2:object"&gt;
    ///       &lt;value&gt;AuditEndPointAddress&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.AuditBindingKey"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;System.ServiceModel.BasicHttpBinding&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.AuditEndPointAddress"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;System.ServiceModel.EndpointAddress&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.AuditEndPointAddress.parameters"&gt;
    ///     &lt;property name="p1:string"&gt;
    ///       &lt;value&gt;http://localhost:22222/HermesAuditTrailSaveService&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   
    ///   &lt;namespace name="HermesScheduleItemService.TestGenericNotesClientKey"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;HermesGenericNoteServiceClient&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="assembly"&gt;
    ///       &lt;value&gt;HermesNS.TC.Services.ScheduleItem.Entities.dll&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.TestGenericNotesClientKey.parameters"&gt;
    ///     &lt;property name="p1:object"&gt;
    ///       &lt;value&gt;NoteBindingKey&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="p2:object"&gt;
    ///       &lt;value&gt;NoteEndPointAddress&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.NoteBindingKey"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;System.ServiceModel.BasicHttpBinding&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.NoteEndPointAddress"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;System.ServiceModel.EndpointAddress&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   &lt;namespace name="HermesScheduleItemService.NoteEndPointAddress.parameters"&gt;
    ///     &lt;property name="p1:string"&gt;
    ///       &lt;value&gt;http://localhost:44444/HermesGenericNoteService&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///   
    ///   &lt;!--Object definition for the persistence provider--&gt;
    ///   &lt;namespace name="HermesScheduleItemService.TestPersistenceKey"&gt;
    ///     &lt;property name="type_name"&gt;
    ///       &lt;value&gt;HermesScheduleItemPersistenceProvider&lt;/value&gt;
    ///     &lt;/property&gt;
    ///     &lt;property name="assembly"&gt;
    ///       &lt;value&gt;HermesNS.TC.Services.ScheduleItem.Entities.dll&lt;/value&gt;
    ///     &lt;/property&gt;
    ///   &lt;/namespace&gt;  
    /// </para>
    /// </summary>
    /// 
    ///
    /// <threadsafety>
    /// It is immutable but not thread-safe due to working with non-thread-safe entities.
    /// </threadsafety>
    ///
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [ServiceContract(SessionMode = SessionMode.Required)]
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.PerSession,
        TransactionIsolationLevel = IsolationLevel.RepeatableRead)]
    public class HermesScheduleItemService :
        ScheduleItemServiceBase<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
        HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
        HermesGenericNoteItem, HermesGenericNoteItemHistory>
    {
        #region Variables

        /// <summary>
        /// The name for the 'published' schedule item status.
        /// </summary>
        private const string Published = "published";

        /// <summary>
        /// The name for the 'edit copy' schedule item status.
        /// </summary>
        private const string EditCopy = "edit copy";

        /// <summary>
        /// The name for the 'retired' schedule item status.
        /// </summary>
        private const string Retired = "retired";

        /// <summary>
        /// The name of the object factory key for the audit client.
        /// </summary>
        private const string AuditClientKey = "auditClientKey";

        /// <summary>
        /// The name of the object factory key for the generic notes client.
        /// </summary>
        private const string GenericNotesClientKey = "genericNotesClientKey";

        /// <summary>
        /// The name of the object factory key for the HermesLogger.
        /// </summary>
        private const string HermesLoggerKey = "hermesLoggerKey";

        /// <summary>
        /// The name of the object factory key for creating the object factory itself.
        /// </summary>
        private const string ObjectFactoryNamespace = "objectFactoryNamespace";

        /// <summary>
        /// The object factory key for the persistence class.
        /// </summary>
        private const string persistence = "persistence";

        /// <summary>
        /// The session namespace incoming header name
        /// </summary>
        private static readonly string Session_Ns = "session_ns";

        /// <summary>
        /// The session application id incoming header name
        /// </summary>
        private static readonly string Session_Application_Id_Name = "session_application_id";

        /// <summary>
        /// Represents the key to use with the ObjectFactory instance to retrieve a new GenericNotesServiceClient.
        /// Set in the constructor and will not be null/empty.
        /// </summary>
        private readonly string _genericNotesClientKey;

        /// <summary>
        /// Represents the key to use with the ObjectFactory instance
        /// to retrieve a new HermesAuditTrailSaveServiceClient.
        /// Set in the constructor and will not be null/empty.
        /// </summary>
        private readonly string _auditClientKey;

        /// <summary>
        /// Represents the ObjectFactory instance use to obtain the GenericNotesServiceClient and
        /// HermesAuditTrailSaveServiceClient instances in every method that needs it.
        /// Set in the constructor and will not be null.
        /// </summary>
        private readonly ObjectFactory _objectFactory;

        /// <summary>
        /// <para>Represents the HermesLogger instance used to log all errors in the service operations.</para>
        /// </summary>
        private readonly HermesLogger _logger;

        /// <summary>
        /// Represents the default namespace used by the default constructor to
        /// access configuration information in the construction.
        /// </summary>
        public const string DefaultNamespace = "HermesNS.TC.Services.ScheduleItem.HermesScheduleItemService";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HermesScheduleItemService using the default namespace.
        /// </summary>
        ///
        /// <remarks>
        /// The configuration for Persistence must be for an instance of
        /// HermesScheduleItemPersistenceProvider or else exception is thrown.
        /// </remarks>
        ///
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing required values,
        /// or errors while constructing the persistence or if the persistence instance created from
        /// configuration is not of HermesScheduleItemPersistenceProvider.
        /// </exception>
        public HermesScheduleItemService() : this(DefaultNamespace)
        {
        }

        /// <summary>
        /// Creates a new instance of HermesScheduleItemService using the given namespace.
        /// </summary>
        ///
        /// <remarks>
        /// The configuration for Persistence must be for an instance of
        /// HermesScheduleItemPersistenceProvider or else exception is thrown.
        /// </remarks>
        ///
        /// <param name="nameSpace">The Configuration Manager namespace from which to initialize this class.</param>
        ///
        /// <exception cref="InvalidArgumentException">If namespace is null or empty.</exception>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any configuration error occurs, such as unknown namespace,
        /// or missing required values, or errors while constructing the persistence
        /// or if the persistence instance created from configuration is not of HermesScheduleItemPersistenceProvider.
        /// </exception>
        public HermesScheduleItemService(string nameSpace)
        {
            string ofNs = null;
            string persistenceKey = null;
            string loggerKey = null;

            try
            {
                if (nameSpace == null || nameSpace.Trim().Equals(string.Empty))
                {
                    throw new InvalidArgumentException("nameSpace cannot be null or empty.");
                }

                //Get client keys
                _auditClientKey = GetConfigString(nameSpace, AuditClientKey, true);
                _genericNotesClientKey = GetConfigString(nameSpace, GenericNotesClientKey, true);

                //Create object factory
                ofNs = GetConfigString(nameSpace, ObjectFactoryNamespace, false);
                _objectFactory = ofNs == null
                    ? ObjectFactory.GetDefaultObjectFactory()
                    : ObjectFactory.GetDefaultObjectFactory(ofNs);

                //Create persistence instance
                persistenceKey = GetConfigString(nameSpace, persistence, true);
                Persistence = (IScheduleItemPersistenceProvider<string, HermesScheduleItem, HermesActivity,
                    HermesScheduleItemStatus, HermesScheduleItemRequestStatus, HermesActivityGroup,
                    HermesActivityType, HermesGenericNote, HermesGenericNoteItem, HermesGenericNoteItemHistory>)
                    _objectFactory.CreateDefinedObject(persistenceKey);

                // Create HermesLogger instance
                loggerKey = GetConfigString(nameSpace, HermesLoggerKey, true);
                _logger = (HermesLogger)_objectFactory.CreateDefinedObject(loggerKey);
            }
            catch (SelfDocumentingException sde)
            {
                throw GetSDE(sde, "Unable to create HermesScheduleItemService instance.",
                    "HermesScheduleItemService(string)", new string[] { "nameSpace" }, new object[] { nameSpace },
                    new string[] { "ofNs", "persistenceKey" }, new object[] { ofNs, persistenceKey });
            }
            catch (Exception e)
            {
                ScheduleItemConfigurationException sice =
                    new ScheduleItemConfigurationException("Unable to create HermesScheduleItemService instance.", e);

                throw GetSDE(sice, sice.Message, "HermesScheduleItemService(string)",
                    new string[] { "nameSpace" }, new object[] { nameSpace },
                    new string[] { "ofNs", "persistenceKey", "loggerKey" },
                    new object[] { ofNs, persistenceKey, loggerKey });
            }
        }

        /// <summary>
        /// Creates a new instance of HermesScheduleItemService using the passed parameters.
        /// </summary>
        /// <remarks>
        /// persistence must be an instance of HermesScheduleItemPersistenceProvider or else exception is thrown.
        /// </remarks>
        ///
        /// <param name="persistence">
        /// The HermesScheduleItemPersistenceProvider instance to use for creating the instance.
        /// </param>
        /// <param name="objectFactory">
        /// The key to use to create the Object Factory instance.
        /// </param>
        /// <param name="genericNotesClientKey">
        /// The key to use with the ObjectFactory instance to retrieve a new GenericNotesServiceClient.
        /// </param>
        /// <param name="auditClientKey">
        /// The key to use with the ObjectFactory instance to retrieve a new HermesAuditTrailSaveServiceClient.
        /// </param>
        /// <param name="logger">The HermesLogger instance to use for logging.</param>
        ///
        /// <exception cref="InvalidArgumentException">
        /// If persistence or objectFactory or genericNotesClientKey or auditClientKey is null.
        /// If genericNotesClientKey or auditClientKey is empty.
        /// </exception>
        /// <exception cref="ScheduleItemConfigurationException">
        /// If any persistence is not an instance of HermesScheduleItemPersistenceProvider.
        /// </exception>
        public HermesScheduleItemService(
            IScheduleItemPersistenceProvider<string, HermesScheduleItem, HermesActivity, HermesScheduleItemStatus,
            HermesScheduleItemRequestStatus, HermesActivityGroup, HermesActivityType, HermesGenericNote,
            HermesGenericNoteItem, HermesGenericNoteItemHistory> persistence,
            ObjectFactory objectFactory, string genericNotesClientKey, string auditClientKey, HermesLogger logger)
        {
            try
            {
                //Validate
                if (persistence == null)
                {
                    throw new InvalidArgumentException("persistence cannot be null.");
                }
                if (objectFactory == null)
                {
                    throw new InvalidArgumentException("objectFactory cannot be null.");
                }
                if (genericNotesClientKey == null || genericNotesClientKey.Trim().Equals(string.Empty))
                {
                    throw new InvalidArgumentException("genericNotesClientKey cannot be null or empty.");
                }
                if (auditClientKey == null || auditClientKey.Trim().Equals(string.Empty))
                {
                    throw new InvalidArgumentException("auditClientKey cannot be null or empty.");
                }
                if (logger == null)
                {
                    throw new InvalidArgumentException("logger cannot be null.");
                }

                //Assign
                Persistence = (HermesScheduleItemPersistenceProvider)persistence;
                _objectFactory = objectFactory;
                _genericNotesClientKey = genericNotesClientKey;
                _auditClientKey = auditClientKey;
                _logger = logger;
            }
            catch (SelfDocumentingException sde)
            {
                throw GetSDE(sde, "Unable to create HermesScheduleItemService instance.",
                    "HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string , string)",
                    new string[] { "persistence", "objectFactory", "genericNotesClientKey", "auditClientKey" },
                    new object[] { persistence, objectFactory, genericNotesClientKey, auditClientKey },
                    new string[0], new object[0]);
            }
            catch (InvalidCastException ice)
            {
                ScheduleItemConfigurationException sice =
                    new ScheduleItemConfigurationException("Unable to create HermesScheduleItemService instance.", ice);

                throw GetSDE(sice, sice.Message,
                    "HermesScheduleItemService(IScheduleItemPersistenceProvider, ObjectFactory, string , string)",
                    new string[] { "persistence", "objectFactory", "genericNotesClientKey", "auditClientKey" },
                    new object[] { persistence, objectFactory, genericNotesClientKey, auditClientKey },
                    new string[0], new object[0]);
            }
        }
        #endregion

        #region Service methods

        /// <summary>
        /// Creates a new activity with the given name and activity type.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="name">The name of the activity</param>
        /// <param name="type">The activity type of the activity</param>
        /// <returns>The created activity</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the activity.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("CreateActivity")]
        public override HermesActivity CreateActivity(string name, HermesActivityType type)
        {
            HermesActivity activity = null;
            try
            {
                ValidateNotNullNotEmpty(name, "name");
                ValidateNotNull(type, "type");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                activity = new HermesActivity();
                activity.Name = name;
                activity.ActivityType = type;
                SetIdAndModification(activity, true);

                //Perform audit related tasks
                PerformAuditTasks<HermesActivity>(activity, null);

                return base.SaveActivity(activity);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "CreateActivity",
                    new string[] { "name", "type" }, new object[] { name, type },
                    new string[] { "activity" }, new object[] { activity });
            }
        }

        /// <summary>
        /// Creates a new activity group with the given abbreviation.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the activity group</param>
        /// <returns>The created activity group</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the activity group.
        /// </exception>
        [OperationContract]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("CreateActivityGroup")]
        public override HermesActivityGroup CreateActivityGroup(string abbreviation)
        {
            HermesActivityGroup hermesActivityGroup = null;
            try
            {
                ValidateNotNullNotEmpty(abbreviation, "abbreviation");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                hermesActivityGroup = new HermesActivityGroup();
                hermesActivityGroup.Abbreviation = abbreviation;
                SetIdAndModification(hermesActivityGroup, true);

                //Perform audit related tasks
                PerformAuditTasks<HermesActivityGroup>(hermesActivityGroup, null);

                return base.SaveActivityGroup(hermesActivityGroup);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "CreateActivityGroup",
                    new string[] { "abbreviation" }, new object[] { abbreviation },
                    new string[] { "hermesActivityGroup" }, new object[] { hermesActivityGroup });
            }
        }

        /// <summary>
        /// Creates a new activity type with the given abbreviation and activity group.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the activity group.</param>
        /// <param name="activityGroup">The activity group for the activity type.</param>
        /// <returns>The created activity type.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the activity type.
        /// </exception>
        [OperationContract]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("CreateActivityType")]
        public override HermesActivityType CreateActivityType(string abbreviation, HermesActivityGroup activityGroup)
        {
            HermesActivityType hermesActivityType = null;
            try
            {
                ValidateNotNullNotEmpty(abbreviation, "abbreviation");
                ValidateNotNull(activityGroup, "activityGroup");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                hermesActivityType = new HermesActivityType();
                hermesActivityType.Abbreviation = abbreviation;
                hermesActivityType.ActivityGroup = activityGroup;
                SetIdAndModification(hermesActivityType, true);

                //Perform audit related tasks
                PerformAuditTasks<HermesActivityType>(hermesActivityType, null);

                return base.SaveActivityType(hermesActivityType);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "CreateActivityType",
                    new string[] { "abbreviation", "activityGroup" }, new object[] { abbreviation, "activityGroup" },
                    new string[] { "hermesActivityType" }, new object[] { hermesActivityType });
            }
        }

        /// <summary>
        /// Creates a new schedule item with the given workdate and activity.
        /// This method also performs authentication and auditing.
        /// The ScheduleItemStatus and ScheduleItemRequestStatus for the schedule item are set
        /// to the default values of these respective types from the database.
        /// </summary>
        /// <param name="workDate">The work date for the schedule item.</param>
        /// <param name="activity">The activity for the schedule item.</param>
        /// <returns>The created schedule item.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the schedule item.
        /// </exception>
        [OperationContract]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("CreateScheduleItem")]
        public override HermesScheduleItem CreateScheduleItem(DateTime workDate, HermesActivity activity)
        {
            HermesScheduleItem hermesScheduleItem = null;
            try
            {
                ValidateNotNull(activity, "activity");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                hermesScheduleItem = new HermesScheduleItem();
                hermesScheduleItem.Activity = activity;
                hermesScheduleItem.WorkDate = workDate;
                hermesScheduleItem.Version = 0;
                hermesScheduleItem.ExceptionFlag = 'N';
                SetIdAndModification(hermesScheduleItem, true);
                hermesScheduleItem.ScheduleItemRequestStatus = Persistence.GetDefaultScheduleItemRequestStatus();
                hermesScheduleItem.ScheduleItemStatus = Persistence.GetDefaultScheduleItemStatus();

                //Calculate ExpireDate
                if (hermesScheduleItem.Activity != null &&
                    hermesScheduleItem.Activity.ActivityType != null &&
                    hermesScheduleItem.Activity.ActivityType.Name == "Earned" &&
                    hermesScheduleItem.Activity.ActivityType.ActivityGroup != null &&
                    hermesScheduleItem.Activity.ActivityType.ActivityGroup.Abbreviation == "PTO")
                {
                    hermesScheduleItem.ExpirationDate =
                        DateTime.Today.AddDays(hermesScheduleItem.Activity.DefaultExpireDays);
                }

                //Perform audit related tasks
                PerformAuditTasks<HermesScheduleItem>(hermesScheduleItem, null);

                return base.SaveScheduleItem(hermesScheduleItem);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "CreateScheduleItem",
                    new string[] { "workDate", "activity" }, new object[] { workDate, activity },
                    new string[] { "hermesScheduleItem" }, new object[] { hermesScheduleItem });
            }
        }

        /// <summary>
        /// Creates a new schedule item status with the given description and abbreviation.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="description">The description for the schedule item status.</param>
        /// <param name="abbreviation">The abbreviation for the schedule item status.</param>
        /// <returns>The created schedule item status.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the schedule item status
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("CreateScheduleItemStatus")]
        public override HermesScheduleItemStatus CreateScheduleItemStatus(string abbreviation, string description)
        {
            HermesScheduleItemStatus hermesScheduleItemStatus = null;
            try
            {
                ValidateNotNullNotEmpty(description, "description");
                ValidateNotNullNotEmpty(abbreviation, "abbreviation");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                hermesScheduleItemStatus = new HermesScheduleItemStatus();
                hermesScheduleItemStatus.Description = description;
                hermesScheduleItemStatus.Abbreviation = abbreviation;
                SetIdAndModification(hermesScheduleItemStatus, true);

                //Perform audit related tasks
                PerformAuditTasks<HermesScheduleItemStatus>(hermesScheduleItemStatus, null);

                return base.SaveScheduleItemStatus(hermesScheduleItemStatus);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "CreateScheduleItemStatus",
                    new string[] { "description", "abbreviation" }, new object[] { description, abbreviation },
                    new string[] { "hermesScheduleItemStatus" }, new object[] { hermesScheduleItemStatus });
            }
        }

        /// <summary>
        /// Creates a new schedule item request status with the given description and abbreviation.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="description">The description for the schedule item request status.</param>
        /// <param name="abbreviation">The abbreviation for the schedule item request status.</param>
        /// <returns>The created schedule item request status.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the schedule item request status.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("CreateScheduleItemRequestStatus")]
        public override HermesScheduleItemRequestStatus CreateScheduleItemRequestStatus(
            string abbreviation, string description)
        {
            HermesScheduleItemRequestStatus hermesScheduleItemRequestStatus = null;
            try
            {
                ValidateNotNullNotEmpty(description, "description");
                ValidateNotNullNotEmpty(abbreviation, "abbreviation");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                hermesScheduleItemRequestStatus = new HermesScheduleItemRequestStatus();
                hermesScheduleItemRequestStatus.Description = description;
                hermesScheduleItemRequestStatus.Abbreviation = abbreviation;
                SetIdAndModification(hermesScheduleItemRequestStatus, true);

                //Perform audit related tasks
                PerformAuditTasks<HermesScheduleItemRequestStatus>(hermesScheduleItemRequestStatus, null);

                return base.SaveScheduleItemRequestStatus(hermesScheduleItemRequestStatus);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e),
                    "CreateScheduleItemRequestStatus",
                    new string[] { "description", "abbreviation" }, new object[] { description, abbreviation },
                    new string[] { "hermesScheduleItemRequestStatus" },
                    new object[] { hermesScheduleItemRequestStatus });
            }
        }

        /// <summary>
        /// Deletes an activity for the given id.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="id">The id for the activity to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the activity.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteActivity")]
        public override void DeleteActivity(string id)
        {
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesActivity.DeletedAudit", id, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                base.DeleteActivity(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e), "DeleteActivity",
                    new string[] { "id" }, new object[] { id },
                    new string[] { "auditRecords" }, new object[] { auditRecords });
            }
        }

        /// <summary>
        /// Deletes activities for the given ids.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="ids">The ids for the activities to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the activities.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteActivity")]
        public override void DeleteActivities(IList<string> ids)
        {
            try
            {
                ValidateList<string>(ids, "ids");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks for deletions
                PerformAuditTasksForDeletions("HermesActivity.DeletedAudit", ids, EventOutcomeCode.ObjectDeleted);

                base.DeleteActivities(ids);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(ids, e), "DeleteActivities",
                    new string[] { "ids" }, new object[] { ids },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Deletes an activity group for the given id.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="id">The id for the activity group to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the activity group.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteActivityGroup")]
        public override void DeleteActivityGroup(string id)
        {
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesActivityGroup.DeletedAudit", id, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                base.DeleteActivityGroup(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "DeleteActivityGroup",
                    new string[] { "id" }, new object[] { id },
                    new string[] { "auditRecords" }, new object[] { auditRecords });
            }
        }

        /// <summary>
        /// Deletes activity groups for the given ids.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="ids">The ids for the activity groups to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the activity groups.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteActivityGroup")]
        public override void DeleteActivityGroups(IList<string> ids)
        {
            try
            {
                ValidateList<string>(ids, "ids");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks for deletions
                PerformAuditTasksForDeletions("HermesActivityGroup.DeletedAudit", ids, EventOutcomeCode.ObjectDeleted);

                base.DeleteActivityGroups(ids);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(ids, e), "DeleteActivityGroups",
                    new string[] { "ids" }, new object[] { ids },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Deletes an activity type for the given id.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="id">The id for the activity type to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the activity type.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteActivityType")]
        public override void DeleteActivityType(string id)
        {
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesActivityType.DeletedAudit", id, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                base.DeleteActivityType(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e), "DeleteActivityType",
                    new string[] { "id" }, new object[] { id },
                    new string[] { "auditRecords" }, new object[] { auditRecords });
            }
        }

        /// <summary>
        /// Deletes activity types for the given ids.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="ids">The ids for the activity types to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the activity types.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteActivityType")]
        public override void DeleteActivityTypes(IList<string> ids)
        {
            try
            {
                ValidateList<string>(ids, "ids");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks for deletions
                PerformAuditTasksForDeletions("HermesActivityType.DeletedAudit", ids, EventOutcomeCode.ObjectDeleted);

                base.DeleteActivityTypes(ids);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(ids, e), "DeleteActivityTypes",
                    new string[] { "ids" }, new object[] { ids },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Deletes a schedule item for the given id.
        /// This method also performs authentication and auditing.
        /// Also deletes the note if it exists for the schedule item.
        /// </summary>
        /// <param name="id">The id for the schedule item to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the schedule item.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItem")]
        public override void DeleteScheduleItem(string id)
        {
            Profile profile = null;
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                profile = SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Delete the generic note for the schedule item
                DeleteNoteForScheduleItem(id, profile);

                //Perform audit related tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItem.DeletedAudit", id, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                base.DeleteScheduleItem(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e), "DeleteScheduleItem",
                    new string[] { "id" }, new object[] { id },
                    new string[] { "auditRecords", "profile" }, new object[] { auditRecords, profile });
            }
        }

        /// <summary>
        /// Deletes the schedule items for the given ids.
        /// This method also performs authentication and auditing.
        /// Also deletes the notes if they exist for their respective schedule items.
        /// </summary>
        /// <param name="ids">The ids for the schedule item to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the schedule items.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItem")]
        public override void DeleteScheduleItems(IList<string> ids)
        {
            Profile profile = null;

            try
            {
                ValidateList<string>(ids, "ids");

                //Authorize
                profile = SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks for deletions
                PerformAuditTasksForDeletions("HermesScheduleItem.DeletedAudit", ids, EventOutcomeCode.ObjectDeleted);

                //Delete the generic notes for the schedule items
                DeleteNotesForScheduleItems(ids, profile);

                base.DeleteScheduleItems(ids);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(ids, e), "DeleteScheduleItems",
                    new string[] { "ids" }, new object[] { ids },
                    new string[] { "profile" }, new object[] { profile });
            }
        }

        /// <summary>
        /// Deletes the schedule item status for the given id.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="id">The id for the schedule item status to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the schedule item status.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItemStatus")]
        public override void DeleteScheduleItemStatus(string id)
        {
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItemStatus.DeletedAudit", id, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                base.DeleteScheduleItemStatus(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "DeleteScheduleItemStatus",
                    new string[] { "id" }, new object[] { id },
                    new string[] { "auditRecords" }, new object[] { auditRecords });
            }
        }

        /// <summary>
        /// Deletes the schedule item statuses for the given ids.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="ids">The ids for the schedule item statuses to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the schedule item statuses.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItemStatus")]
        public override void DeleteScheduleItemStatuses(IList<string> ids)
        {
            try
            {
                ValidateList<string>(ids, "ids");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks for deletions
                PerformAuditTasksForDeletions(
                    "HermesScheduleItemStatus.DeletedAudit", ids, EventOutcomeCode.ObjectDeleted);

                base.DeleteScheduleItemStatuses(ids);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(ids, e), "DeleteScheduleItemStatuses",
                    new string[] { "ids" }, new object[] { ids },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Deletes the schedule item request status for the given id.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="id">The id for the schedule item request status to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the schedule item request status.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItemRequestStatus")]
        public override void DeleteScheduleItemRequestStatus(string id)
        {
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItemRequestStatus.DeletedAudit", id, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                base.DeleteScheduleItemRequestStatus(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "DeleteScheduleItemRequestStatus", new string[] { "id" }, new object[] { id },
                    new string[] { "auditRecords" }, new object[] { auditRecords });
            }
        }

        /// <summary>
        /// Deletes the schedule item request statuses for the given ids.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="ids">The ids for the schedule item request statuses to remove.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the schedule item request statuses.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItemRequestStatus")]
        public override void DeleteScheduleItemRequestStatuses(IList<string> ids)
        {
            try
            {
                ValidateList<string>(ids, "ids");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit related tasks for deletions
                PerformAuditTasksForDeletions(
                    "HermesScheduleItemRequestStatus.DeletedAudit", ids, EventOutcomeCode.ObjectDeleted);

                base.DeleteScheduleItemRequestStatuses(ids);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(ids, e),
                    "DeleteScheduleItemRequestStatuses",
                    new string[] { "ids" }, new object[] { ids }, new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets the activity for the given id.
        /// This method also performs authentication.
        /// </summary>
        /// <param name="id">The id for the activity to get.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the activity.
        /// </exception>
        /// <returns>The activity for the given id or null if no activity with given id exists.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetActivity")]
        public override HermesActivity GetActivity(string id)
        {
            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetActivity(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "GetActivity", new string[] { "id" }, new object[] { id }, new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets the activity group for the given id.
        /// This method also performs authentication.
        /// </summary>
        /// <param name="id">The id for the activity group to get.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the activity group.
        /// </exception>
        /// <returns>The activity group for the given id or null if no activity group with given id exists.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetActivityGroup")]
        public override HermesActivityGroup GetActivityGroup(string id)
        {
            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetActivityGroup(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "GetActivityGroup",
                    new string[] { "id" }, new object[] { id }, new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets the activity type for the given id.
        /// This method also performs authentication.
        /// </summary>
        /// <param name="id">The id for the activity type to get.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the activity type.
        /// </exception>
        /// <returns>The activity type for the given id or null if no activity type with given id exists.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetActivityType")]
        public override HermesActivityType GetActivityType(string id)
        {
            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetActivityType(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "GetActivityType",
                    new string[] { "id" }, new object[] { id }, new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets the schedule item for the given id.
        /// This method also performs authentication.
        /// This method also gets the note information using the GernericNotesServiceClient
        /// if the schedule item contains a note with a valid note id.
        /// </summary>
        /// <param name="id">The id for the schedule item to get.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the schedule item.
        /// </exception>
        /// <returns>The schedule item for the given id or null if no schedule item with given id exists.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItem")]
        public override HermesScheduleItem GetScheduleItem(string id)
        {
            HermesScheduleItem schedItem = null;
            GenericNotesServiceClient genericNotesClient = null;
            Profile profile = null;

            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                profile = SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Get the schedule item from persistence
                schedItem = base.GetScheduleItem(id);

                //Get the note data if needed
                if (schedItem != null && schedItem.Note != null && schedItem.Note.Id != null)
                {
                    //Get note client
                    genericNotesClient =
                        (GenericNotesServiceClient)_objectFactory.CreateDefinedObject(_genericNotesClientKey);
                    genericNotesClient.Open();

                    schedItem.Note = genericNotesClient.GetGenericNote(
                        schedItem.Note.Id, TimeZone.CurrentTimeZone, profile.UserName, profile.UserID);
                }

                return schedItem;
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "GetScheduleItem",
                    new string[] { "id" }, new object[] { id },
                    new string[] { "schedItem", "genericNotesClient", "profile" },
                    new object[] { schedItem, genericNotesClient, profile });
            }
            finally
            {
                if (genericNotesClient != null)
                {
                    try
                    {
                        genericNotesClient.Close();
                    }
                    catch
                    {
                        genericNotesClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Gets the schedule item status for the given id.
        /// This method also performs authentication.
        /// </summary>
        /// <param name="id">The id for the schedule item status to get.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the schedule item status.
        /// </exception>
        /// <returns>
        /// The schedule item status for the given id or null if no schedule item status with given id exists.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItemStatus")]
        public override HermesScheduleItemStatus GetScheduleItemStatus(string id)
        {
            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetScheduleItemStatus(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "GetScheduleItemStatus",
                    new string[] { "id" }, new object[] { id }, new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets the schedule item request status for the given id.
        /// This method also performs authentication.
        /// </summary>
        /// <param name="id">The id for the schedule item request status to get.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the schedule item request status.
        /// </exception>
        /// <returns>
        /// The schedule item request status for the given id or
        /// null if no schedule item request status with given id exists.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItemRequestStatus")]
        public override HermesScheduleItemRequestStatus GetScheduleItemRequestStatus(string id)
        {
            try
            {
                ValidateNotNullNotEmpty(id, "id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetScheduleItemRequestStatus(id);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(id == null ? "null" : id, e),
                    "GetScheduleItemRequestStatus", new string[] { "id" }, new object[] { id },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets all the activites depending on whether disabled activities need to be retreived.
        /// This method also performs authentication.
        /// </summary>
        /// <param name="showDisabled">Whether to retreive disabled activities or not.</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting all the activities.
        /// </exception>
        /// <returns>
        /// All the activites depending on whether disabled activities need to be retreived.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetActivity")]
        public override IList<HermesActivity> GetAllActivities(bool showDisabled)
        {
            try
            {
                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetAllActivities(showDisabled);
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "GetAllActivities",
                    new string[] { "showDisabled" }, new object[] { showDisabled },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets all the activity groups in persistence.
        /// This method also performs authentication.
        /// </summary>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting all the activity groups.
        /// </exception>
        /// <returns>
        /// List of all the activity groups in persistence.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetActivityGroup")]
        public override IList<HermesActivityGroup> GetAllActivityGroups()
        {
            try
            {
                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetAllActivityGroups();
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "GetAllActivityGroups",
                    new string[0], new object[0], new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets all the activity types in persistence.
        /// This method also performs authentication.
        /// </summary>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting all the activity types.
        /// </exception>
        /// <returns>
        /// List of all the activity types in persistence.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetActivityType")]
        public override IList<HermesActivityType> GetAllActivityTypes()
        {
            try
            {
                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetAllActivityTypes();
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e), "GetAllActivityTypes",
                    new string[0], new object[0], new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets all the schedule item request statuses types in persistence.
        /// This method also performs authentication.
        /// </summary>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting all the schedule item request statuses.
        /// </exception>
        /// <returns>
        /// List of all the schedule item request statuses in persistence.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItemRequestStatus")]
        public override IList<HermesScheduleItemRequestStatus> GetAllScheduleItemRequestStatuses()
        {
            try
            {
                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetAllScheduleItemRequestStatuses();
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e),
                    "GetAllScheduleItemRequestStatuses",
                    new string[0], new object[0], new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets all the schedule item statuses types in persistence.
        /// This method also performs authentication.
        /// </summary>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting all the schedule item statuses.
        /// </exception>
        /// <returns>
        /// List of all the schedule item statuses in persistence.
        /// </returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItemStatus")]
        public override IList<HermesScheduleItemStatus> GetAllScheduleItemStatuses()
        {
            try
            {
                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return base.GetAllScheduleItemStatuses();
            }
            catch (Exception e)
            {
                throw LogAndCreateFaultException(e, ConstructMessage(null, e),
                    "GetAllScheduleItemStatuses", new string[0], new object[0],
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Saves the activity in the persistence. If the activity does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// The activity must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="activity">The HermesActivity instance to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the activity fails validation by the HermesActivityValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving the activity in database.
        /// </exception>
        /// <returns>The saved activity.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveActivity")]
        public override HermesActivity SaveActivity(HermesActivity activity)
        {
            try
            {
                ValidateNotNull(activity, "activity");
                ValidateNotNullNotEmpty(activity.Id, "activity.Id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Set modification properties
                SetIdAndModification(activity, false);

                //Perform Validations
                PerformValidations<HermesActivity>(new HermesActivityValidator(activity, this));

                //Perform audit
                PerformAuditTasks<HermesActivity>(activity, Persistence.GetActivity(activity.Id));

                return base.SaveActivity(activity);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                string entityId = (activity != null && activity.Id != null) ? activity.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "SaveActivity",
                    new string[] { "activity" }, new object[] { activity },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Saves the activity group in the persistence. If the activity group does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// The activity group must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="activityGroup">The HermesActivityGroup instance to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the activity group fails validation by the HermesActivityGroupValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving the activity group in database.
        /// </exception>
        /// <returns>The saved activity group.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveActivityGroup")]
        public override HermesActivityGroup SaveActivityGroup(HermesActivityGroup activityGroup)
        {
            try
            {
                ValidateNotNull(activityGroup, "activityGroup");
                ValidateNotNullNotEmpty(activityGroup.Id, "activityGroup.Id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Set modification properties
                SetIdAndModification(activityGroup, false);

                //Perform Validations
                PerformValidations<HermesActivityGroup>(new HermesActivityGroupValidator(activityGroup, this));

                //Perform audit
                PerformAuditTasks<HermesActivityGroup>(activityGroup, Persistence.GetActivityGroup(activityGroup.Id));

                return base.SaveActivityGroup(activityGroup);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                string entityId = (activityGroup != null && activityGroup.Id != null) ? activityGroup.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "SaveActivityGroup",
                    new string[] { "activityGroup" }, new object[] { activityGroup },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Saves the activity type in the persistence. If the activity type does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// The activity type must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="activityType">The HermesActivityType instance to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the activity type fails validation by the HermesActivityTypeValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving the activity type in database.
        /// </exception>
        /// <returns>The saved activity type.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveActivityType")]
        public override HermesActivityType SaveActivityType(HermesActivityType activityType)
        {
            try
            {
                ValidateNotNull(activityType, "activityType");
                ValidateNotNullNotEmpty(activityType.Id, "activityType.Id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Set modification properties
                SetIdAndModification(activityType, false);

                //Perform Validations
                PerformValidations<HermesActivityType>(new HermesActivityTypeValidator(activityType, this));

                //Perform audit
                PerformAuditTasks<HermesActivityType>(activityType, Persistence.GetActivityType(activityType.Id));

                return base.SaveActivityType(activityType);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                string entityId = (activityType != null && activityType.Id != null) ? activityType.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "SaveActivityType",
                    new string[] { "activityType" }, new object[] { activityType },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Saves the schedule item in the persistence. If the schedule item does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// If the schedule item has a non-null Note property then the note is created or updated
        /// using the HermesGenericNoteServiceClient.
        /// </summary>
        /// <remarks>
        /// The schedule item must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="scheduleItem">The HermesScheduleItem instance to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the schedule item fails validation by the HermesScheduleItemValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving the schedule item in database.
        /// </exception>
        /// <returns>The saved schedule item.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveScheduleItem")]
        public override HermesScheduleItem SaveScheduleItem(HermesScheduleItem scheduleItem)
        {
            GenericNotesServiceClient genericNoteClient = null;
            Profile profile = null;

            try
            {
                ValidateNotNull(scheduleItem, "scheduleItem");
                ValidateNotNullNotEmpty(scheduleItem.Id, "scheduleItem.Id");

                //Authorize
                profile = SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Set modification properties
                SetIdAndModification(scheduleItem, false);

                //Perform Validations
                PerformValidations<HermesScheduleItem>(new HermesScheduleItemValidator(scheduleItem, this));

                //Perform audit
                PerformAuditTasks<HermesScheduleItem>(scheduleItem, Persistence.GetScheduleItem(scheduleItem.Id));

                //Handle GenericNote
                if (scheduleItem.Note != null)
                {
                    //Get generic note client
                    genericNoteClient =
                        (GenericNotesServiceClient)_objectFactory.CreateDefinedObject(_genericNotesClientKey);
                    genericNoteClient.Open();

                    //Add or update depending on whether the id of the note exists or not.
                    if (scheduleItem.Note.Id == null)
                    {
                        scheduleItem.Note =
                            genericNoteClient.AddGenericNote(scheduleItem.Note, profile.UserName, profile.UserID);
                    }
                    else
                    {
                        scheduleItem.Note =
                            genericNoteClient.UpdateGenericNote(scheduleItem.Note, profile.UserName, profile.UserID);
                    }
                }

                return base.SaveScheduleItem(scheduleItem);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                string entityId = (scheduleItem != null && scheduleItem.Id != null) ? scheduleItem.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "SaveScheduleItem",
                    new string[] { "scheduleItem" }, new object[] { scheduleItem },
                    new string[] { "genericNoteClient", "profile" },
                    new object[] { genericNoteClient, profile });
            }
            finally
            {
                if (genericNoteClient != null)
                {
                    try
                    {
                        genericNoteClient.Close();
                    }
                    catch
                    {
                        genericNoteClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Saves the schedule item status in the persistence.
        /// If the schedule item status does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// The schedule item status must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="scheduleItemStatus">The HermesScheduleItemStatus instance to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the schedule item status fails validation by the HermesScheduleItemStatusValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving the schedule item status in database.
        /// </exception>
        /// <returns>The saved schedule item status.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveScheduleItemStatus")]
        public override HermesScheduleItemStatus SaveScheduleItemStatus(HermesScheduleItemStatus scheduleItemStatus)
        {
            try
            {
                ValidateNotNull(scheduleItemStatus, "scheduleItemStatus");
                ValidateNotNullNotEmpty(scheduleItemStatus.Id, "scheduleItemStatus.Id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Set modification properties
                SetIdAndModification(scheduleItemStatus, false);

                //Perform Validations
                PerformValidations<HermesScheduleItemStatus>(
                    new HermesScheduleItemStatusValidator(scheduleItemStatus, this));

                //Perform audit
                PerformAuditTasks<HermesScheduleItemStatus>(scheduleItemStatus,
                    Persistence.GetScheduleItemStatus(scheduleItemStatus.Id));

                return base.SaveScheduleItemStatus(scheduleItemStatus);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                string entityId =
                    (scheduleItemStatus != null && scheduleItemStatus.Id != null) ? scheduleItemStatus.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "SaveScheduleItemStatus",
                    new string[] { "scheduleItemStatus" }, new object[] { scheduleItemStatus },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Saves the schedule item request status in the persistence.
        /// If the schedule item request status does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// The schedule item request status must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="scheduleRequestStatus">The HermesScheduleItemRequestStatus instance to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the schedule item request status fails validation by the HermesScheduleItemRequestStatusValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving the schedule item request status in database.
        /// </exception>
        /// <returns>The saved schedule item request status.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveScheduleItemRequestStatus")]
        public override HermesScheduleItemRequestStatus SaveScheduleItemRequestStatus(
            HermesScheduleItemRequestStatus scheduleRequestStatus)
        {
            try
            {
                ValidateNotNull(scheduleRequestStatus, "scheduleRequestStatus");
                ValidateNotNullNotEmpty(scheduleRequestStatus.Id, "scheduleRequestStatus.Id");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Set modification properties
                SetIdAndModification(scheduleRequestStatus, false);

                //Perform Validations
                PerformValidations<HermesScheduleItemRequestStatus>(
                    new HermesScheduleItemRequestStatusValidator(scheduleRequestStatus, this));

                //Perform audit
                PerformAuditTasks<HermesScheduleItemRequestStatus>(scheduleRequestStatus,
                    Persistence.GetScheduleItemRequestStatus(scheduleRequestStatus.Id));

                return base.SaveScheduleItemRequestStatus(scheduleRequestStatus);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                string entityId = (scheduleRequestStatus != null && scheduleRequestStatus.Id != null)
                    ? scheduleRequestStatus.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "SaveScheduleItemRequestStatus",
                    new string[] { "scheduleRequestStatus" }, new object[] { scheduleRequestStatus },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Saves the specified activities in the persistence.
        /// If any of the the activities does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// Each activity must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="activities">List of HermesActivity instances to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If any of the activity fails validation by the HermesActivityValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving any of the activities in database.
        /// </exception>
        /// <returns>List of the saved activities.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveActivity")]
        public override IList<HermesActivity> SaveActivities(IList<HermesActivity> activities)
        {
            List<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            HermesActivity locActivity = null;

            try
            {
                ValidateList<HermesActivity>(activities, "activities");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                foreach (HermesActivity activity in activities)
                {
                    //For SDE!
                    locActivity = activity;

                    ValidateNotNullNotEmpty(activity.Id, "activity.Id");

                    //Set modification properties
                    SetIdAndModification(activity, false);

                    //Perform Validations
                    PerformValidations<HermesActivity>(new HermesActivityValidator(activity, this));

                    //Get audit records
                    auditRecords.AddRange(activity.Audit(Persistence.GetActivity(activity.Id)));
                }

                //Call audit service only once
                PerformAuditTasks(auditRecords);

                return base.SaveActivities(activities);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                IList<string> entityIds = new List<string>();
                for (int i = 0; (activities != null && i < activities.Count); i++)
                {
                    entityIds.Add(activities[i] == null ? "null" : activities[i].Id);
                }

                throw LogAndCreateFaultException(e, ConstructMessage(entityIds, e),
                    "SaveActivities",
                    new string[] { "activities" }, new object[] { activities },
                    new string[] { "auditRecords", "locActivity" },
                    new object[] { auditRecords, locActivity });
            }
        }

        /// <summary>
        /// Saves the specified activity groups in the persistence.
        /// If any of the the activity groups does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// Each activity group must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="activityGroups">List of HermesActivityGroup instances to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If any of the activity group fails validation by the HermesActivityGroupValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving any of the activity groups in database.
        /// </exception>
        /// <returns>List of the saved activity groups.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveActivityGroup")]
        public override IList<HermesActivityGroup> SaveActivityGroups(IList<HermesActivityGroup> activityGroups)
        {
            List<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            HermesActivityGroup locActivityGroup = null;

            try
            {
                ValidateList<HermesActivityGroup>(activityGroups, "activityGroups");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                foreach (HermesActivityGroup activityGroup in activityGroups)
                {
                    //For SDE!
                    locActivityGroup = activityGroup;

                    ValidateNotNullNotEmpty(activityGroup.Id, "activityGroup.Id");

                    //Set modification properties
                    SetIdAndModification(activityGroup, false);

                    //Perform Validations
                    PerformValidations<HermesActivityGroup>(new HermesActivityGroupValidator(activityGroup, this));

                    //Get audit records
                    auditRecords.AddRange(activityGroup.Audit(Persistence.GetActivityGroup(activityGroup.Id)));
                }

                //Call audit service only once
                PerformAuditTasks(auditRecords);

                return base.SaveActivityGroups(activityGroups);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                IList<string> entityIds = new List<string>();
                for (int i = 0; (activityGroups != null && i < activityGroups.Count); i++)
                {
                    entityIds.Add(activityGroups[i] == null ? "null" : activityGroups[i].Id);
                }

                throw LogAndCreateFaultException(e, ConstructMessage(entityIds, e), "SaveActivityGroups",
                    new string[] { "activityGroups" }, new object[] { activityGroups },
                    new string[] { "auditRecords", "locActivityGroup" },
                    new object[] { auditRecords, locActivityGroup });
            }
        }

        /// <summary>
        /// Saves the specified activity types in the persistence.
        /// If any of the the activity types does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// Each activity type must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="activityTypes">List of HermesActivityType instances to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If any of the activity type fails validation by the HermesActivityTypeValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving any of the activity type in database.
        /// </exception>
        /// <returns>List of the saved activity types.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveActivityType")]
        public override IList<HermesActivityType> SaveActivityTypes(IList<HermesActivityType> activityTypes)
        {
            List<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            HermesActivityType locActivityType = null;

            try
            {
                ValidateList<HermesActivityType>(activityTypes, "activityTypes");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                foreach (HermesActivityType activityType in activityTypes)
                {
                    //For SDE!
                    locActivityType = activityType;

                    ValidateNotNullNotEmpty(activityType.Id, "activityType.Id");

                    //Set modification properties
                    SetIdAndModification(activityType, false);

                    //Perform Validations
                    PerformValidations<HermesActivityType>(new HermesActivityTypeValidator(activityType, this));

                    //Get audit records
                    auditRecords.AddRange(activityType.Audit(Persistence.GetActivityType(activityType.Id)));
                }

                //Call audit service only once
                PerformAuditTasks(auditRecords);

                return base.SaveActivityTypes(activityTypes);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                IList<string> entityIds = new List<string>();
                for (int i = 0; (activityTypes != null && i < activityTypes.Count); i++)
                {
                    entityIds.Add(activityTypes[i] == null ? "null" : activityTypes[i].Id);
                }

                throw LogAndCreateFaultException(e, ConstructMessage(entityIds, e), "SaveActivityTypes",
                    new string[] { "activityTypes" }, new object[] { activityTypes },
                    new string[] { "auditRecords", "locActivityType" },
                    new object[] { auditRecords, locActivityType });
            }
        }

        /// <summary>
        /// Saves the specified schedule item request statuses in the persistence.
        /// If any of the the schedule item request status does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// Each schedule item request status must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="scheduleRequestStatuses">List of HermesScheduleItemRequestStatus instances to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If any of the schedule item request statuses fails validation
        /// by the HermesScheduleItemRequestStatusValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving any of the schedule item request status in database.
        /// </exception>
        /// <returns>List of the saved schedule item request statuses.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveScheduleItemRequestStatus")]
        public override IList<HermesScheduleItemRequestStatus> SaveScheduleItemRequestStatuses(
            IList<HermesScheduleItemRequestStatus> scheduleRequestStatuses)
        {
            List<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            HermesScheduleItemRequestStatus locReqStatus = null;

            try
            {
                ValidateList<HermesScheduleItemRequestStatus>(scheduleRequestStatuses, "scheduleRequestStatuses");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                foreach (HermesScheduleItemRequestStatus scheduleRequestStatus in scheduleRequestStatuses)
                {
                    //For SDE!
                    locReqStatus = scheduleRequestStatus;

                    ValidateNotNullNotEmpty(scheduleRequestStatus.Id, "scheduleRequestStatus.Id");

                    //Set modification properties
                    SetIdAndModification(scheduleRequestStatus, false);

                    //Perform Validations
                    PerformValidations<HermesScheduleItemRequestStatus>(
                        new HermesScheduleItemRequestStatusValidator(scheduleRequestStatus, this));

                    //Get audit records
                    auditRecords.AddRange(scheduleRequestStatus.Audit(
                        Persistence.GetScheduleItemRequestStatus(scheduleRequestStatus.Id)));
                }

                //Call audit service only once
                PerformAuditTasks(auditRecords);

                return base.SaveScheduleItemRequestStatuses(scheduleRequestStatuses);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                IList<string> entityIds = new List<string>();
                for (int i = 0; (scheduleRequestStatuses != null && i < scheduleRequestStatuses.Count); i++)
                {
                    entityIds.Add(scheduleRequestStatuses[i] == null ? "null" : scheduleRequestStatuses[i].Id);
                }

                throw LogAndCreateFaultException(e, ConstructMessage(entityIds, e),
                    "SaveScheduleItemRequestStatuses",
                    new string[] { "scheduleRequestStatuses" }, new object[] { scheduleRequestStatuses },
                    new string[] { "auditRecords", "locReqStatus" },
                    new object[] { auditRecords, locReqStatus });
            }
        }

        /// <summary>
        /// Saves the specified schedule items in the persistence.
        /// If any of the the schedule item status does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// If any of the schedule items has a non-null Note property then the note is created or updated
        /// using the HermesGenericNoteServiceClient.
        /// </summary>
        /// <remarks>
        /// Each schedule item must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="scheduleItems">List of HermesScheduleItem instances to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If any of the schedule items fails validation by the HermesScheduleItemValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving any of the schedule items in database.
        /// </exception>
        /// <returns>List of the saved schedule items.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveScheduleItem")]
        public override IList<HermesScheduleItem> SaveScheduleItems(IList<HermesScheduleItem> scheduleItems)
        {
            HermesScheduleItem locScheduleItem = null;
            List<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            Profile profile = null;

            try
            {
                ValidateList<HermesScheduleItem>(scheduleItems, "scheduleItems");

                //Authorize
                profile = SetUserNameAndAuthorize(MethodBase.GetCurrentMethod());

                foreach (HermesScheduleItem scheduleItem in scheduleItems)
                {
                    //For SDE!
                    locScheduleItem = scheduleItem;

                    ValidateNotNullNotEmpty(scheduleItem.Id, "scheduleItem.Id");

                    //Set modification properties
                    SetIdAndModification(scheduleItem, false);

                    //Perform Validations
                    PerformValidations<HermesScheduleItem>(new HermesScheduleItemValidator(scheduleItem, this));

                    //Get audit records
                    auditRecords.AddRange(scheduleItem.Audit(Persistence.GetScheduleItem(scheduleItem.Id)));
                }

                //Perform audit
                PerformAuditTasks(auditRecords);

                //Save notes
                SaveNotesForScheduleItems(scheduleItems, profile);

                return base.SaveScheduleItems(scheduleItems);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                IList<string> entityIds = new List<string>();
                for (int i = 0; (scheduleItems != null && i < scheduleItems.Count); i++)
                {
                    entityIds.Add(scheduleItems[i] == null ? "null" : scheduleItems[i].Id);
                }

                throw LogAndCreateFaultException(e, ConstructMessage(entityIds, e), "SaveScheduleItems",
                    new string[] { "scheduleItems" }, new object[] { scheduleItems },
                    new string[] { "auditRecords", "locScheduleItem", "profile" },
                    new object[] { auditRecords, locScheduleItem, profile });
            }
        }

        /// <summary>
        /// Saves the specified schedule item statuses in the persistence.
        /// If any of the the schedule item status does not exist, a new one is created.
        /// This method also performs authentication, validation and auditing.
        /// </summary>
        /// <remarks>
        /// Each schedule item status must have a non-null and non-empty id or else FaultException is thrown.
        /// </remarks>
        /// <param name="scheduleItemStatuses">List of HermesScheduleItemStatus instances to save.</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If any of the schedule item statuses fails validation
        /// by the HermesScheduleItemStatusValidator.
        /// </exception>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when saving any of the schedule item status in database.
        /// </exception>
        /// <returns>List of the saved schedule item statuses.</returns>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FaultContract(typeof(HermesValidationFaultException))]
        [FunctionalAbilities("SaveScheduleItemStatus")]
        public override IList<HermesScheduleItemStatus> SaveScheduleItemStatuses(
            IList<HermesScheduleItemStatus> scheduleItemStatuses)
        {
            List<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            HermesScheduleItemStatus locStatus = null;

            try
            {
                ValidateList<HermesScheduleItemStatus>(scheduleItemStatuses, "scheduleItemStatuses");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                foreach (HermesScheduleItemStatus scheduleItemStatus in scheduleItemStatuses)
                {
                    //For SDE!
                    locStatus = scheduleItemStatus;

                    ValidateNotNullNotEmpty(scheduleItemStatus.Id, "scheduleItemStatus.Id");

                    //Set modification properties
                    SetIdAndModification(scheduleItemStatus, false);

                    //Perform Validations
                    PerformValidations<HermesScheduleItemStatus>(
                        new HermesScheduleItemStatusValidator(scheduleItemStatus, this));

                    //Get audit records
                    auditRecords.AddRange(scheduleItemStatus.Audit(
                        Persistence.GetScheduleItemStatus(scheduleItemStatus.Id)));
                }

                //Call audit service only once
                PerformAuditTasks(auditRecords);

                return base.SaveScheduleItemStatuses(scheduleItemStatuses);
            }
            catch (FaultException<HermesValidationFaultException> fe)
            {
                throw fe;
            }
            catch (Exception e)
            {
                IList<string> entityIds = new List<string>();
                for (int i = 0; (scheduleItemStatuses != null && i < scheduleItemStatuses.Count); i++)
                {
                    entityIds.Add(scheduleItemStatuses[i] == null ? "null" : scheduleItemStatuses[i].Id);
                }

                throw LogAndCreateFaultException(e, ConstructMessage(entityIds, e), "SaveScheduleItemStatuses",
                    new string[] { "scheduleItemStatuses" }, new object[] { scheduleItemStatuses },
                    new string[] { "auditRecords", "locStatus" },
                    new object[] { auditRecords, locStatus });
            }
        }

        /// <summary>
        /// Creates a new edit copy for the given parent schedule item.
        /// The edit copy has the same value as parent for all properties except that its status is 'edit copy'.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <remarks>
        /// If parent item does not exist or does not have a status of published, then FaultException is raised
        /// </remarks>
        /// <param name="parent">The parent schedule item</param>
        /// <returns>The edit copy that is created.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the edit copy schedule item in database.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("CreateScheduleItemEditCopy")]
        public virtual HermesScheduleItem CreateScheduleItemEditCopy(HermesScheduleItem parent)
        {
            HermesScheduleItem parentItem = null;
            HermesScheduleItem newItem = null;
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNull(parent, "parent");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Get parent and verify it
                parentItem = Persistence.GetScheduleItem(parent.Id);
                if (parentItem == null || parentItem.ScheduleItemStatus.Description != Published)
                {
                    throw new ApplicationException("Parent schedule item either does not exist or is invalid.");
                }

                //Set new item's properties
                newItem = new HermesScheduleItem();
                CopyScheduleItemProperties(parent, newItem, EditCopy);

                //Perform audit records
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItem.CreatedEditCopy", null, EventOutcomeCode.ObjectCreated);
                PerformAuditTasks(auditRecords);

                return base.SaveScheduleItem(newItem);
            }
            catch (Exception e)
            {
                string entityId = (parent != null && parent.Id != null) ? parent.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "CreateScheduleItemEditCopy",
                    new string[] { "parent" }, new object[] { parent },
                    new string[] { "parentItem", "newItem", "auditRecords" },
                    new object[] { parentItem, newItem, auditRecords });
            }
        }

        /// <summary>
        /// Creates a parent - edit copy realtion between the parent and editCopy schedule item paramaters.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <remarks>
        /// The parent and edit copy must both exist in database. The parent must have status of published and
        /// edit copy must have status as edit copy.
        /// </remarks>
        /// <param name="parent">The parent schedule item</param>
        /// <param name="editCopy">The edit copy schedule item</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when creating the parent - edit copy realtionship.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("CreateScheduleItemPublishEditCopyRelationship")]
        public virtual void CreateScheduleItemPublishEditCopyRelationship(
            HermesScheduleItem parent, HermesScheduleItem editCopy)
        {
            HermesScheduleItem parentItem = null;
            HermesScheduleItem editCopyItem = null;
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNull(parent, "parent");
                ValidateNotNull(editCopy, "editCopy");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Get parent and verify it
                parentItem = Persistence.GetScheduleItem(parent.Id);
                if (parentItem == null || parentItem.ScheduleItemStatus.Description != Published)
                {
                    throw new ApplicationException("Parent schedule item either does not exist or is invalid.");
                }

                //Get parent and verify it
                editCopyItem = Persistence.GetScheduleItem(editCopy.Id);
                if (editCopyItem == null || editCopyItem.ScheduleItemStatus.Description != EditCopy)
                {
                    throw new ApplicationException("Edit copy either does not exist or is invalid.");
                }

                //Perform audit tasks
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItem.CreatedPublishRelationship", null, EventOutcomeCode.ObjectCreated);
                PerformAuditTasks(auditRecords);

                //Delegate to persistence method
                (Persistence as HermesScheduleItemPersistenceProvider).
                    CreateScheduleItemPublishEditCopyRelationship(parent, editCopy);
            }
            catch (Exception e)
            {
                string[] parentAndEditCopyIds = new string[2];
                parentAndEditCopyIds[0] = (parent != null && parent.Id != null) ? parent.Id : "null";
                parentAndEditCopyIds[1] = (editCopy != null && editCopy.Id != null) ? editCopy.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(string.Join("|", parentAndEditCopyIds), e),
                    "CreateScheduleItemPublishEditCopyRelationship",
                    new string[] { "parent", "editCopy" }, new object[] { parent, editCopy },
                    new string[] { "parentItem", "editCopyItem", "auditRecords" },
                    new object[] { parentItem, editCopyItem, auditRecords });
            }
        }

        /// <summary>
        /// Deletes the relationship between the parent and edit copy.
        /// Note that both parent and edit copy are themeselves are not deleted from database,
        /// just their relation is deleted.
        /// This method also performs authentication and auditing.
        /// </summary>
        /// <param name="editCopy">The edit copy</param>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when deleting the parent - edit copy realtionship.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("DeleteScheduleItemPublishEditCopyRelationship")]
        public virtual void DeleteScheduleItemPublishEditCopyRelationship(HermesScheduleItem editCopy)
        {
            IList<HermesAuditRecord> auditRecords = null;
            try
            {
                ValidateNotNull(editCopy, "editCopy");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Perform audit
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItem.DeletedPublishRelationship", null, EventOutcomeCode.ObjectDeleted);
                PerformAuditTasks(auditRecords);

                //Delegate to persistence method
                (Persistence as HermesScheduleItemPersistenceProvider).
                    DeleteScheduleItemPublishEditCopyRelationship(editCopy);
            }
            catch (Exception e)
            {
                string entityId = (editCopy != null && editCopy.Id != null) ? editCopy.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e),
                    "DeleteScheduleItemPublishEditCopyRelationship",
                    new string[] { "editCopy" }, new object[] { editCopy },
                    new string[] { "auditRecords" }, new object[] { auditRecords });
            }
        }

        /// <summary>
        /// Gets the edit copy for the parent.
        /// This method performs authentication only. There is no auditing.
        /// </summary>
        /// <param name="parent">The parent schedule item</param>
        /// <returns>The edit copy for the parent or null if it has no edit copy.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the edit copy for the parent.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItemEditCopy")]
        public virtual HermesScheduleItem GetScheduleItemEditCopy(HermesScheduleItem parent)
        {
            try
            {
                ValidateNotNull(parent, "parent");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return (Persistence as HermesScheduleItemPersistenceProvider).GetScheduleItemEditCopy(parent);
            }
            catch (Exception e)
            {
                string entityId = (parent != null && parent.Id != null) ? parent.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "GetScheduleItemEditCopy",
                    new string[] { "parent" }, new object[] { parent },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Gets the parent for the edit copy.
        /// This method performs authentication only. There is no auditing.
        /// </summary>
        /// <param name="editCopy">The edit copy schedule item</param>
        /// <returns>The parent for the edit copy or null if it has no parent.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when getting the parent for the edit copy.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("GetScheduleItemParentCopy")]
        public virtual HermesScheduleItem GetScheduleItemParentCopy(HermesScheduleItem editCopy)
        {
            try
            {
                ValidateNotNull(editCopy, "editCopy");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                return (Persistence as HermesScheduleItemPersistenceProvider).GetScheduleItemParentCopy(editCopy);
            }
            catch (Exception e)
            {
                string entityId = (editCopy != null && editCopy.Id != null) ? editCopy.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "GetScheduleItemParentCopy",
                    new string[] { "editCopy" }, new object[] { editCopy },
                    new string[0], new object[0]);
            }
        }

        /// <summary>
        /// Publishes the edit copy and returns the updated edit copy.
        /// If it has status 'edit copy', then its status is set to 'retired'
        /// and its parent's status is set to 'published'.
        /// If it has any other status, then its status is set to 'published'
        /// </summary>
        /// <remarks>
        /// Note that the edit copy must exist in the database and must not have a status of 'published' or else
        /// FaultException is thrown.
        /// </remarks>
        /// <param name="editCopy">The edit copy</param>
        /// <returns>The updated edit copy.</returns>
        /// <exception cref="FaultException&lt;TCFaultException&gt;">
        /// If there is any problem when publishing the schedule item.
        /// </exception>
        [OperationContract]
        [OperationBehavior(
            Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false,
            TransactionScopeRequired = true)]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("PublishScheduleItem")]
        public virtual HermesScheduleItem PublishScheduleItem(HermesScheduleItem editCopy)
        {
            HermesScheduleItem editCopyDb = null;
            HermesScheduleItem parentDb = null;
            IList<HermesAuditRecord> auditRecords = null;

            try
            {
                ValidateNotNull(editCopy, "editCopy");

                //Authorize
                SetUserNameAndAuthorize(MethodInfo.GetCurrentMethod());

                //Verify editCopy is in persistence and does not have status of "published"
                editCopyDb = Persistence.GetScheduleItem(editCopy.Id);
                if (editCopyDb == null || editCopy.ScheduleItemStatus.Description == Published)
                {
                    throw new ApplicationException(
                        "Edit copy is either not present in persistence or has invalid status.");
                }

                //Perform audit
                auditRecords = CreateSingleAuditRecordList(
                    "HermesScheduleItem.Published", null, EventOutcomeCode.DataModified);
                PerformAuditTasks(auditRecords);

                if (editCopy.ScheduleItemStatus.Description == EditCopy)
                {
                    //Get parent, update it and save it.
                    parentDb = (Persistence as HermesScheduleItemPersistenceProvider).
                        GetScheduleItemParentCopy(editCopy);
                    CopyScheduleItemProperties(editCopy, parentDb, Published);
                    Persistence.SaveScheduleItem(parentDb);

                    editCopy.ScheduleItemStatus = GetScheduleItemStatusByDescription(Retired);
                    return Persistence.SaveScheduleItem(editCopy);
                }
                else
                {
                    editCopy.ScheduleItemStatus = GetScheduleItemStatusByDescription(Published);
                    return Persistence.SaveScheduleItem(editCopy);
                }
            }
            catch (Exception e)
            {
                string entityId = (editCopy != null && editCopy.Id != null) ? editCopy.Id : "null";
                throw LogAndCreateFaultException(e, ConstructMessage(entityId, e), "PublishScheduleItem",
                    new string[] { "editCopy" }, new object[] { editCopy },
                    new string[] { "auditRecords", "editCopyDb", "parentDb" },
                    new object[] { auditRecords, editCopyDb, parentDb });
            }
        }

        /// <summary>
        /// This method is called when the Host property is set, to alert
        /// the subclass that the Host property has changed.
        /// This allows the subclass to register with the ServiceHost events
        /// immediately after the Host has been set.
        /// </summary>
        protected override void HostUpdated()
        {
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Wraps an exception into SelfDocumentingException if it already
        /// is not an instance of SelfDocumentingException.
        /// Also pins a method's state to the created instance.
        /// </summary>
        /// <param name="e">The exception to wrap into SelfDocumentingException</param>
        /// <param name="message">The message of the SelfDocumentingException</param>
        /// <param name="methodName">The name of the method from which exception was thrown</param>
        /// <param name="methodParamNames">The paramater names of the method from which exception was thrown.</param>
        /// <param name="methodParams">The paramater values of the method from which exception was thrown.</param>
        /// <param name="localVarNames">The local variable names of the method from which exception was thrown.</param>
        /// <param name="localVars">The local variable values of the method from which exception was thrown.</param>
        /// <returns>The created SelfDocumentingException instance.</returns>
        private SelfDocumentingException GetSDE(Exception e, string message, string methodName,
            string[] methodParamNames, object[] methodParams, string[] localVarNames, object[] localVars)
        {
            SelfDocumentingException sde = e as SelfDocumentingException;
            if (sde == null)
            {
                sde = new SelfDocumentingException(message, e);
            }
            MethodState ms = sde.PinMethod(GetType().FullName + "." + methodName, e.StackTrace);

            for (int i = 0; i < methodParamNames.Length; i++)
            {
                try
                {
                    ms.AddMethodParameter(methodParamNames[i], methodParams[i]);
                }
                catch { }
            }

            for (int i = 0; i < localVarNames.Length; i++)
            {
                try
                {
                    ms.AddLocalVariable(localVarNames[i], localVars[i]);
                }
                catch { }
            }

            ms.AddInstanceVariable("_genericNotesClientKey", _genericNotesClientKey);
            ms.AddInstanceVariable("_auditClientKey", _auditClientKey);
            ms.AddInstanceVariable("_objectFactory", _objectFactory);

            ms.Lock();
            return sde;
        }

        /// <summary>
        /// Wraps an exception into SelfDocumentingException, logs it using DMSExceptionHelper and then
        /// creates a FaultException&lt;TCFaultException&gt; using the SelfDocumentingException instance
        /// </summary>
        /// <param name="e">The exception to wrap.</param>
        /// <param name="message">The message of the exception.</param>
        /// <param name="methodShortName">The short name of method from which exception was thrown.</param>
        /// <param name="methodParamNames">The paramater names of the method from which exception was thrown.</param>
        /// <param name="methodParams">The paramater values of the method from which exception was thrown.</param>
        /// <param name="localVarNames">The local variable names of the method from which exception was thrown.</param>
        /// <param name="localVars">The local variable values of the method from which exception was thrown.</param>
        /// <returns>The created FaultException&lt;TCFaultException&gt;</returns>
        private FaultException<TCFaultException> LogAndCreateFaultException(Exception e, string message,
            string methodShortName, string[] methodParamNames, object[] methodParams,
            string[] localVarNames, object[] localVars)
        {
            //Log using HermesLogger
            try
            {
                string[] messages = message.Split('|');
                string[] formatValues = new string[messages.Length - 1];

                if (messages.Length > 1)
                {
                    Array.Copy(messages, 1, formatValues, 0, messages.Length - 1);
                }

                // Publish the exception
                _logger.Log(Level.ERROR, messages[0], formatValues);
            }
            catch
            {
                // Ignore exception silently
            }


            //Log using DMSExceptionHelper
            try
            {
                new DMSExceptionHelper().PublishException(
                    WcfHelper.GetProfileFromContext(OperationContext.Current), e);
            }
            catch
            {
                // Ignore exception silently
            }


            //Create FaultException and return
            //Wrap to SDE
            SelfDocumentingException sde = GetSDE(e, e.Message, methodShortName,
                methodParamNames, methodParams, localVarNames, localVars);
            return new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), e.Message);
        }

        /// <summary>
        /// Authorizes a method using the current profile and the current running method.
        /// </summary>
        /// <param name="methodBase">The current method.</param>
        /// <returns>The current profile</returns>
        /// <exception cref="SelfDocumentingException">
        /// If there is any problem from getting the profile or if mediator fails the authorization.
        /// </exception>
        private Profile SetUserNameAndAuthorize(MethodBase methodBase)
        {
            Profile profile = null;
            string applicationId = null;
            try
            {
                profile = WcfHelper.GetProfileFromContext(OperationContext.Current);
                UserName = profile.UserID;

                applicationId = WcfHelper.GetApplicationID(OperationContext.Current);

                HermesAuthorizationMediator.MediateMethod(applicationId,
                    profile.SessionID, profile.SessionToken, methodBase);

                return profile;
            }
            catch (Exception e)
            {
                throw GetSDE(e, e.Message, "SetUserNameAndAuthorize",
                    new string[] { "methodBase" }, new object[] { methodBase },
                    new string[] { "profile", "applicationId" }, new object[] { profile, applicationId });
            }
        }

        /// <summary>
        /// Performs validation of an entity using its validator
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="validator">The entity to validate</param>
        /// <exception cref="FaultException&lt;HermesValidationFaultException&gt;">
        /// If the validation fails for the given entity.
        /// </exception>
        private static void PerformValidations<T>(HermesValidatorBase<T> validator)
        {
            if (!validator.Validate())
            {
                HermesValidationFaultException validationDetail =
                    new HermesValidationFaultException("Validation failed.", validator.DataValidationRecords);

                throw new FaultException<HermesValidationFaultException>(validationDetail, validationDetail.Message);
            }
        }

        /// <summary>
        /// Creates a list with a single HermesAuditRecord in it. The audit record has its Message and EntityId
        /// set to the the message and id paramters. The EntityId is set only if id is non-null.
        /// </summary>
        /// <param name="message">The message to set of the audit record.</param>
        /// <param name="id">The id to set of the audit record. May be null.</param>
        /// <param name="eventOutcome">The outcome code to set for the audit record created</param>
        /// <returns>The list with a single audit record.</returns>
        private static IList<HermesAuditRecord> CreateSingleAuditRecordList(
            string message, string id, EventOutcomeCode eventOutcome)
        {
            IList<HermesAuditRecord> auditRecords = new List<HermesAuditRecord>();
            auditRecords.Add(CreateAuditRecord(message, id, eventOutcome));
            return auditRecords;
        }

        /// <summary>
        /// Creates an audit record with Message and EntityId
        /// set to the the message and id paramters. The EntityId is set only if id is non-null.
        /// </summary>
        /// <param name="message">The message to set of the audit record.</param>
        /// <param name="id">The id to set of the audit record. May be null.</param>
        /// <param name="eventOutcome">The outcome code to set for the audit record created</param>
        /// <returns>The created HermesAuditRecord instance.</returns>
        private static HermesAuditRecord CreateAuditRecord(string message, string id, EventOutcomeCode eventOutcome)
        {
            HermesAuditRecord auditRecord = new HermesAuditRecord();
            auditRecord.Message = message;

            if (id != null)
            {
                auditRecord.EntityId = id;
            }

            auditRecord.CurrentApplication = OperationContext.Current == null ? null :
                OperationContext.Current.IncomingMessageHeaders.GetHeader<string>(
                Session_Application_Id_Name, Session_Ns);
            auditRecord.CallerIdentity = Thread.CurrentPrincipal.Identity.Name;
            auditRecord.CallerIdentityDomain = Environment.UserDomainName;
            auditRecord.CreatedTimeStamp = DateTime.Now;
            auditRecord.Category = AuditEventCategory.Information;
            auditRecord.TransactionId = Transaction.Current == null ? null :
                Transaction.Current.TransactionInformation.DistributedIdentifier.ToString();
            auditRecord.Guid = Guid.NewGuid();
            auditRecord.EventOutcomeCode = eventOutcome;

            return auditRecord;
        }

        /// <summary>
        /// Performs auditing of an auditable entity given the old entity.
        /// If the old entity is null then a single audit record is created otherwise many audit records are created
        /// for each property that differs in the old and new entities.
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="auditable">The new entity.</param>
        /// <param name="oldEntity">The old entity.</param>
        private void PerformAuditTasks<T>(IAuditable<T> auditable, T oldEntity)
        {
            //Create audit records
            IList<HermesAuditRecord> auditRecords = null;
            auditRecords = auditable.Audit(oldEntity);

            //Save using audit client
            PerformAuditTasks(auditRecords);
        }

        /// <summary>
        /// Performs the actual saving of the audit records using the HermesAuditTrailSaveService
        /// using a client for the service.
        /// </summary>
        /// <remarks>
        /// Note that any exception raised by the Audit Trail Service are allowed to bubble through
        /// and are caught in the respective methods calling this method.
        /// </remarks>
        /// <param name="auditRecords">The audit records to save</param>
        private void PerformAuditTasks(IList<HermesAuditRecord> auditRecords)
        {
            AuditServiceClient auditClient = null;
            try
            {
                //Get audit client:
                auditClient = (AuditServiceClient)
                    _objectFactory.CreateDefinedObject(_auditClientKey);

                //Open audit client
                auditClient.Open();

                //Save audit client
                auditClient.SaveAuditRecords(auditRecords);
            }
            finally
            {
                //Close client even if exception is thrown
                if (auditClient != null)
                {
                    try
                    {
                        auditClient.Close();
                    }
                    catch
                    {
                        auditClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the Id of an entity to a new guid, LastModifiedBy to the current user and
        /// the LastModifiedDate to the current date and time.
        /// The Id is set only if setId is true.
        /// </summary>
        /// <param name="entity">The entity for which to set the properties</param>
        /// <param name="setId">Whether to set the id or not</param>
        private void SetIdAndModification(BaseIdEntity<string> entity, bool setId)
        {
            entity.LastModifiedBy = UserName;
            entity.LastModifiedDate = DateTime.Now;

            if (setId)
            {
                entity.Id = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// Gets the value of a Configuration Manager property given its key and namespace.
        /// </summary>
        /// <param name="nameSpace">The ConfigManager namespace to read from</param>
        /// <param name="key">The property key to read from.</param>
        /// <param name="isRequired">Whether the property must exist and should not be empty string.</param>
        /// <returns>The value of the property found</returns>
        /// <exception cref="ScheduleItemConfigurationException">
        /// The property is required and it is either not found or has empty value.
        /// </exception>
        private string GetConfigString(string nameSpace, string key, bool isRequired)
        {
            string strValue = null;
            try
            {
                strValue = ConfigManager.GetInstance().GetValue(nameSpace, key);
            }
            catch (Exception e)
            {
                throw GetSDE(e, e.Message, "GetConfigString",
                    new string[] { "nameSpace", "key", "isRequired" }, new object[] { nameSpace, key, isRequired },
                    new string[] { "strValue" }, new object[] { strValue });
            }

            //Required and not found or has empty value
            if ((isRequired && strValue == null) || (strValue != null && strValue.Trim().Equals(string.Empty)))
            {
                throw new ScheduleItemConfigurationException(
                    "Either required property is missing or it has empty value.");
            }

            return strValue;
        }

        /// <summary>
        /// Sets the properties of the target schedule item to those of the source schedule item.
        /// The ScheduleItemStatus is set to the status with description 'statusToSet'.
        /// The Id is set only if the target already does not have an id.
        /// </summary>
        /// <param name="source">The source schedule item</param>
        /// <param name="target">The target schedule item</param>
        /// <param name="statusToSet">The decription of the status to set.</param>
        private void CopyScheduleItemProperties(HermesScheduleItem source,
            HermesScheduleItem target, string statusToSet)
        {
            target.Activity = source.Activity;
            target.Duration = source.Duration;
            target.ExceptionFlag = source.ExceptionFlag;
            target.ExpirationDate = source.ExpirationDate;
            target.LastModifiedBy = source.LastModifiedBy;
            target.LastModifiedDate = source.LastModifiedDate;
            target.Note = source.Note;
            target.ScheduleItemRequestStatus = source.ScheduleItemRequestStatus;
            target.ScheduleItemStatus = GetScheduleItemStatusByDescription(statusToSet);
            target.Version = source.Version;
            target.WorkDate = source.WorkDate;
            target.WorkDayAmount = source.WorkDayAmount;

            if (target.Id == null)
            {
                target.Id = Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// Gets the schedule item status with the given description.
        /// </summary>
        /// <param name="description">The description</param>
        /// <returns>The schedule item status with the given description.</returns>
        private HermesScheduleItemStatus GetScheduleItemStatusByDescription(string description)
        {
            IList<HermesScheduleItemStatus> allStatuses = Persistence.GetAllScheduleItemStatuses();
            foreach (HermesScheduleItemStatus status in allStatuses)
            {
                if (status.Description == description)
                {
                    return status;
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes the note for the given schedule item id using a client for the HermesGenericNoteService.
        /// </summary>
        /// <param name="schedItemId">The id of the schedule item</param>
        /// <param name="profile">The profile for getting the current user name and user id.</param>
        /// <exception cref="SelfDocumentingException">
        /// If there is any problem in deleting the note using the client for the HermesGenericNoteService.
        /// </exception>
        private void DeleteNoteForScheduleItem(string schedItemId, Profile profile)
        {
            HermesScheduleItem existingSchedItem = null;
            GenericNotesServiceClient genericNotesClient = null;

            try
            {
                //Get the existing schedule item
                existingSchedItem = Persistence.GetScheduleItem(schedItemId);
                if (existingSchedItem != null && existingSchedItem.Note != null)
                {
                    //Get Hermes Generic Note Service client
                    genericNotesClient = (GenericNotesServiceClient)
                        _objectFactory.CreateDefinedObject(_genericNotesClientKey);
                    genericNotesClient.Open();

                    //Get the generic note again as there may be changes in the data for the note id.
                    existingSchedItem.Note = genericNotesClient.GetGenericNote(
                        existingSchedItem.Note.Id, TimeZone.CurrentTimeZone, profile.UserName, profile.UserID);

                    //Delete generic note
                    genericNotesClient.DeleteGenericNote(existingSchedItem.Note, profile.UserName, profile.UserID);
                }
            }
            catch (Exception e)
            {
                throw GetSDE(e, "Unable to delete note for schedule item.", "DeleteNoteForScheduleItem",
                    new string[] { "schedItemId" }, new object[] { schedItemId },
                    new string[] { "existingSchedItem", "genericNotesClient" },
                    new object[] { existingSchedItem, genericNotesClient });
            }
            finally
            {
                //Client needs to be closed even if exception is thrown.
                if (genericNotesClient != null)
                {
                    try
                    {
                        genericNotesClient.Close();
                    }
                    catch
                    {
                        genericNotesClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the notes for the given schedule item ids using a client for the HermesGenericNoteService.
        /// The advantage of using this method is that it opens the client for the HermesGenericNoteService only once
        /// and not for each schedule item.
        /// </summary>
        /// <param name="schedItemIds">The ids of the schedule items for which to delete the notes</param>
        /// <param name="profile">The profile for getting the current user name and user id.</param>
        /// <exception cref="SelfDocumentingException">
        /// If there is any problem in deleting the notes using the client for the HermesGenericNoteService.
        /// </exception>
        private void DeleteNotesForScheduleItems(IList<string> schedItemIds, Profile profile)
        {
            HermesScheduleItem existingSchedItem = null;
            GenericNotesServiceClient genericNotesClient = null;
            string locSchedItemId = null;

            try
            {
                foreach (string schedItemId in schedItemIds)
                {
                    //For SDE
                    locSchedItemId = schedItemId;

                    //Get the existing schedule item
                    existingSchedItem = Persistence.GetScheduleItem(schedItemId);

                    if (existingSchedItem != null && existingSchedItem.Note != null
                        && existingSchedItem.Note.Id != null)
                    {
                        //Open the client only once and not for each schedule item
                        if (genericNotesClient == null)
                        {
                            //Get Hermes Generic Note Service client
                            genericNotesClient = (GenericNotesServiceClient)
                                _objectFactory.CreateDefinedObject(_genericNotesClientKey);
                            genericNotesClient.Open();
                        }

                        //Get the generic note again as there may be changes in the data for the note id.
                        existingSchedItem.Note = genericNotesClient.GetGenericNote(
                            existingSchedItem.Note.Id, TimeZone.CurrentTimeZone, profile.UserName, profile.UserID);

                        //Delete generic note
                        genericNotesClient.DeleteGenericNote(existingSchedItem.Note, profile.UserName, profile.UserID);
                    }
                }
            }
            catch (Exception e)
            {
                throw GetSDE(e, "Unable to delete notes for schedule items.", "DeleteNotesForScheduleItems",
                    new string[] { "schedItemIds" }, new object[] { schedItemIds },
                    new string[] { "existingSchedItem", "genericNotesClient", "locSchedItemId" },
                    new object[] { existingSchedItem, genericNotesClient, locSchedItemId });
            }
            finally
            {
                //Client needs to be closed even if exception is thrown.
                if (genericNotesClient != null)
                {
                    try
                    {
                        genericNotesClient.Close();
                    }
                    catch
                    {
                        genericNotesClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Saves the notes for a list of schedule items. Any schedule item which has null for its Note is ignored.
        /// For other schedule items, a new note is created if its note has a null id otherwise it is updated.
        /// This method opens the note client only once and not for each schedule item.
        /// </summary>
        /// <param name="schedItems">The list of the schedule items which need to have their notes saved.</param>
        /// <param name="profile">The profile for getting the current user name and user id.</param>
        /// <exception cref="SelfDocumentingException">
        /// If there is any problem in saving the notes using the client for the HermesGenericNoteService.
        /// </exception>
        private void SaveNotesForScheduleItems(IList<HermesScheduleItem> schedItems, Profile profile)
        {
            GenericNotesServiceClient genericNotesClient = null;
            HermesScheduleItem locSchedItem = null;

            try
            {
                foreach (HermesScheduleItem schedItem in schedItems)
                {
                    //For SDE!
                    locSchedItem = schedItem;

                    if (schedItem.Note != null)
                    {
                        //Open the client only once and not for each schedule item
                        if (genericNotesClient == null)
                        {
                            //Get Hermes Generic Note Service client
                            genericNotesClient = (GenericNotesServiceClient)
                                _objectFactory.CreateDefinedObject(_genericNotesClientKey);
                            genericNotesClient.Open();
                        }

                        //Add or update note
                        if (schedItem.Note.Id != null)
                        {
                            schedItem.Note =
                                genericNotesClient.UpdateGenericNote(schedItem.Note, profile.UserName, profile.UserID);
                        }
                        else
                        {
                            schedItem.Note =
                                genericNotesClient.AddGenericNote(schedItem.Note, profile.UserName, profile.UserID);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw GetSDE(e, "Unable to save notes for schedule items.", "SaveNotesForScheduleItems",
                    new string[] { "schedItems" }, new object[] { schedItems },
                    new string[] { "genericNotesClient", "locSchedItem" },
                    new object[] { genericNotesClient, locSchedItem });
            }
            finally
            {
                //Client needs to be closed even if exception is thrown.
                if (genericNotesClient != null)
                {
                    try
                    {
                        genericNotesClient.Close();
                    }
                    catch
                    {
                        genericNotesClient.Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Performs auditing for all ids (of any type of entity) which are being deleted.
        /// </summary>
        /// <param name="message">The message to set of the audit record</param>
        /// <param name="eventOutcome">The outcome code to set for the audit record created</param>
        /// <param name="ids">List of all entities being deleted.</param>
        /// <exception cref="SelfDocumentingException">
        /// If there is any problem in performing auditing.
        /// </exception>
        private void PerformAuditTasksForDeletions(string message, IList<string> ids, EventOutcomeCode eventOutcome)
        {
            IList<HermesAuditRecord> auditRecords = null;
            string locId = null;
            try
            {
                auditRecords = new List<HermesAuditRecord>();
                foreach (string id in ids)
                {
                    //For SDE!
                    locId = id;

                    auditRecords.Add(CreateAuditRecord(message, id, eventOutcome));
                }

                PerformAuditTasks(auditRecords);
            }
            catch (Exception e)
            {
                throw GetSDE(e, "Unable to perform audit for deletions.",
                    "PerformAuditTasksForDeletions",
                    new string[] { "message", "ids" }, new object[] { message, ids },
                    new string[] { "auditRecords", "locId" }, new object[] { auditRecords, locId });
            }
        }

        /// <summary>
        /// Checks whether an object is not null.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <param name="name">The name of the object variable.</param>
        /// <exception cref="ArgumentNullException">If the object is null.</exception>
        private static void ValidateNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name, name + " must not be null.");
            }
        }

        /// <summary>
        /// Checks whether a string is not empty after trimming.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="name">The name of the string variable.</param>
        /// <exception cref="ArgumentException">If the string is empty after trimming.</exception>
        private static void ValidateNotEmpty(string str, string name)
        {
            if (str != null && str.Trim().Equals(string.Empty))
            {
                throw new ArgumentException(name + " must not be empty string.", name);
            }
        }

        /// <summary>
        /// Checks if a string is not null and not empty.
        /// </summary>
        /// <param name="str">The string to check.</param>
        /// <param name="name">The name of the string variable.</param>
        /// <exception cref="ArgumentException">If the string is empty after trimming.</exception>
        /// <exception cref="ArgumentNullException">If the string is null.</exception>
        private static void ValidateNotNullNotEmpty(string str, string name)
        {
            ValidateNotNull(str, name);
            ValidateNotEmpty(str, name);
        }

        /// <summary>
        /// Validates that a list must not be null and must not contain any null elements.
        /// If the type of list elements is string, it also checks that none of the elements is empty after trimming.
        /// </summary>
        /// <typeparam name="T">The type of the list elements</typeparam>
        /// <param name="list">The list to check</param>
        /// <param name="name">The name of the list variable.</param>
        /// <exception cref="ArgumentException">If the list element is null or empty string.</exception>
        /// <exception cref="ArgumentNullException">If the list is null.</exception>
        private static void ValidateList<T>(IList<T> list, string name)
        {
            ValidateNotNull(list, name);

            if (typeof(T).Equals(typeof(string)))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == null)
                    {
                        throw new ArgumentException(
                            "List element must not be null.", "Item at index " + i + " in list " + name);
                    }
                    ValidateNotEmpty(list[i] as string, "Item at index " + i + " in list " + name);
                }
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == null)
                    {
                        throw new ArgumentException(
                            "List element must not be null.", "Item at index " + i + " in list " + name);
                    }
                }
            }
        }

        /// <summary>
        /// <para>Construct the SelfDocumentingException's message for use by logger.</para>
        /// </summary>
        /// <param name="entityIds">Can be an entity id or a list of entity ids</param>
        /// <param name="e">The actual error.</param>
        /// <returns>The string message.</returns>
        private static string ConstructMessage(object entityIds, Exception e)
        {
            Exception locExp = e;
            string fullMessage = string.Empty;
            string methodName = new StackTrace().GetFrame(1).GetMethod().Name;
            
            //Get all the exception details
            while (locExp != null)
            {
                fullMessage += string.Format("{1}.{0} failed with error: {2}\n",
                    methodName, "HermesScheduleItemService", e.Message);

                locExp = locExp.InnerException;
            }

            //Get all the entity ids
            string loggerParams = fullMessage;
            string formattedEntityIds = string.Empty;
            if (entityIds != null)
            {
                //A single entity id.
                if (entityIds.GetType().Equals(typeof(string)))
                {
                    formattedEntityIds = entityIds.ToString();
                }
                //A list of entity ids
                else if (entityIds.GetType().Equals(typeof(IList<string>)))
                {
                    IList<string> entityIdList = (IList<string>)entityIds;
                    for (int i = 0; i < entityIdList.Count; i++)
                    {
                        formattedEntityIds += entityIdList[i] == null ? "null" : entityIdList[i];
                        if (i != entityIdList.Count - 1)
                        {
                            formattedEntityIds += ", ";
                        }
                    }
                }

                loggerParams = string.Join("|", new string[] { loggerParams, formattedEntityIds });
            }

            //Prepend message id and return
            return string.Join("|", new string[] { "HermesScheduleItemService." + methodName, loggerParams });
        }

        # endregion
    }
}
