/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Collections.Generic;

using TopCoder.Services.WCF;
using TopCoder.LoggingWrapper;
using TopCoder.Util.ObjectFactory;
using TopCoder.Util.ExceptionManager.SDE;
using TopCoder.Util.ConfigurationManager;
using TopCoder.Services.WCF.Audit.Entities;

using HermesNS.Entity.Common;
using HermesNS.TC.Entity.Validation;
using HermesNS.TC.Services.Feeds.Clients;
using HermesNS.TC.LoggingWrapperPublisher;
using HermesNS.TC.Services.Feeds.Validation;
using HermesNS.TC.Services.Resources.Company;
using HermesNS.TC.Services.Feeds.Persistence;
using HermesNS.TC.Services.AuditTrail.Entities;
using HermesNS.TC.Services.Feeds.Entities.Reference;
using HermesNS.TC.Services.Resources.Company.Entities;
using Hermes.Services.Security.Authorization.Client.Common;
using System.Transactions;

namespace HermesNS.TC.Services.Feeds
{
    /// <summary>
    /// <para>
    /// This class is the WCF service that manages all the entities in this component.
    /// </para>
    ///
    /// <para>
    /// One can create an entity, save it, delete it, or retrieve all entities of a given type.
    /// </para>
    ///
    /// <para>
    /// This service is marked with <see cref="ServiceContractAttribute"/> and all its business
    /// methods are marked with <see cref="OperationContractAttribute"/>.
    /// </para>
    ///
    /// <para>
    /// It extends <see cref="TCWcfServiceBase"/>.
    /// </para>
    ///
    /// <para>
    /// If there are errors during any operation, they are logged using the <see cref="HermesLogger"/>
    /// and thrown as <see cref="FaultException"/> with a <see cref="TCFaultException"/> that is assembled
    /// from the errors.
    /// </para>
    ///
    /// <para>
    /// It uses the <see cref="ConfigManager"/> and <see cref="ObjectFactory"/> to perform all initialization.
    /// </para>
    ///
    /// <para>
    /// This class provides a layer of validation, authorization, and auditing before performing
    /// the persistence operation. Validation is performed using the Hermes validators provided
    /// with this component, and auditing is done mostly in the entities. Audit method and uses
    /// the <see cref="AuditServiceClient"/> to save these audit records.
    /// </para>
    ///
    /// <para>
    /// This component makes use of another service <see cref="CompanyService"/> to retrieve entities
    /// that make up the entities in this component.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    ///
    /// 1) Configuration Sample : <br/>
    ///
    ///  <!-- This is a valid configuration for FeedReferenceService -->
    ///  &lt;namespace name=&quot;HermesNS.TC.Services.Feeds.FeedReferenceService&quot;&gt;
    ///      &lt;property name =&quot;objectFactoryNamespace&quot;&gt;
    ///          &lt;value&gt;TopCoder.Util.ObjectFactory&lt;/value&gt;
    ///      &lt;/property&gt;
    ///      &lt;property name=&quot;persistenceKey&quot;&gt;
    ///          &lt;value&gt;persistence&lt;/value&gt;
    ///      &lt;/property&gt;
    ///      &lt;property name=&quot;auditClientKey&quot;&gt;
    ///          &lt;value&gt;AuditServiceClient&lt;/value&gt;
    ///      &lt;/property&gt;
    ///      &lt;property name=&quot;companyServiceKey&quot;&gt;
    ///          &lt;value&gt;CompanyService&lt;/value&gt;
    ///      &lt;/property&gt;
    ///      &lt;property name=&quot;logger&quot;&gt;
    ///          &lt;value&gt;HermesLogger&lt;/value&gt;
    ///      &lt;/property&gt;
    ///   &lt;/namespace&gt;
    ///
    /// </para>
    /// <para>
    /// 2) Demo Usage : <br/>
    ///
    /// There are 26 entities in this component, and showing them all in a demo would be very cumbersome.<br/>
    /// Since the entities are virtually the same, this demo will focus on the CRUD operation of a single<br/>
    /// type  <see cref="AdType"/>  that will act as a representative for all other entities.<br/>
    ///
    ///     // Create several add types supported by our organization
    ///     AdType adType1 = client.CreateAdType(&quot;television ad&quot;);
    ///     AdType adType2 = client.CreateAdType(&quot;newspaper ad&quot;);
    ///
    ///     // Get and update the first ad type
    ///     IList&lt;AdType&gt; allAdTypes = client.GetAllAdTypes();
    ///
    ///     // We will use the first one to update
    ///     AdType retrievedAdType = allAdTypes[0];
    ///     // rename it from -television ad- to -TV ad- and save
    ///     retrievedAdType.Name = &quot;TV ad&quot;;
    ///     AdType savedAdType = client.SaveAdType(retrievedAdType);
    ///
    ///     // Our organization does not do newspapaer ads anymore, so delete the ad type
    ///     client.DeleteAdType(adType2);
    ///
    /// </para>
    /// </remarks>
    ///
    /// <threadsafety>
    /// It is immutable and thread-safe.
    /// </threadsafety>
    ///
    /// <author>argolite</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    [ServiceContract(SessionMode = SessionMode.Required)]
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession,
        TransactionIsolationLevel=IsolationLevel.RepeatableRead)]
    public class FeedReferenceService : TCWcfServiceBase
    {
        /// <summary>
        /// <para>
        /// Represents array of the instance fields' names in this class.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// It is used when calling <see cref="Helper.BuildSelfDocumentingException"/> method.
        /// </para>
        /// </remarks>
        private static readonly string[] InstanceNames = new string[] {
            "_auditClientKey", "_auditServiceFactory", "_companyService", "_logger", "_persistence" };

        /// <summary>
        /// <para>
        /// Represents the configuration property name of object factory namespace.
        /// </para>
        /// </summary>
        private const string ConfigObjectFactoryNamespace = "objectFactoryNamespace";

        /// <summary>
        /// <Pare>
        /// Represents the configuration property name of audit client object name.
        /// </Pare>
        /// </summary>
        private const string ConfigAuditClientKey = "auditClientKey";

        /// <summary>
        /// <para>
        /// Represents the configuration property name of persistence object name.
        /// </para>
        /// </summary>
        private const string ConfigPersistenceObjectKey = "persistenceKey";

        /// <summary>
        /// <para>
        /// Represents the configuration property name of company service object name.
        /// </para>
        /// </summary>
        private const string ConfigCompanyServiceObjectKey = "companyServiceKey";

        /// <summary>
        /// <para>
        /// Represents the configuration property name of logger object name.
        /// </para>
        /// </summary>
        private const string ConfigLoggerObjectKey = "logger";

        /// <summary>
        /// <para>
        /// Represents the key to use with the <see cref="ObjectFactory"/> instance
        /// to retrieve a new <see cref="AuditServiceClient"/>.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Set in the constructor and will not be null or empty string.
        /// </para>
        /// </remarks>
        private readonly string _auditClientKey;

        /// <summary>
        /// <para>
        /// Represents the <see cref="ObjectFactory"/> instance use to obtain
        /// the <see cref="AuditServiceClient"/> instances in every method that needs it.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Set in the constructor and will not be null.
        /// </para>
        /// </remarks>
        private readonly ObjectFactory _auditServiceFactory;

        /// <summary>
        /// <para>
        /// Represents the <see cref="IFeedReferencePersistence"/> instance used
        /// to perform all persistence operations in all methods.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Set in the constructor and will not be null.
        /// </para>
        /// </remarks>
        private readonly IFeedReferencePersistence _persistence;

        /// <summary>
        /// <para>
        /// Represents the <see cref="CompanyService"/> instance used to perform
        /// all retrival operations for companies.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Set in the constructor and will not be null.
        /// </para>
        /// </remarks>
        private readonly CompanyService _companyService;

        /// <summary>
        /// <para>
        /// Represents the <see cref="HermesLogger"/> instance used to log
        /// all errors in the service operatins.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Set in the constructor and will not be null.
        /// </para>
        /// </remarks>
        private readonly HermesLogger _logger;

        /// <summary>
        /// <para>
        /// Default constructor, creates <see cref="FeedReferenceService"/> instance
        /// from configuration using default namespace:
        /// &quot;HermesNS.TC.Services.Feeds.FeedReferenceService&quot;
        /// </para>
        /// </summary>
        ///
        /// <exception cref="FeedReferenceConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing
        /// required values, or errors while constructing the persistence or the service.
        /// </exception>
        public FeedReferenceService()
            : this(typeof(FeedReferenceService).FullName)
        {
        }

        /// <summary>
        /// <para>
        /// Constructor, creates <see cref="FeedReferenceService"/> instance
        /// from configuration using the given namespace.
        /// </para>
        /// </summary>
        ///
        /// <param name="nameSpace">
        /// The namespace of the properties needed by this class.
        /// </param>
        ///
        /// <exception cref="InvalidArgumentException">
        /// If the passed argument is null, or empty string.
        /// </exception>
        /// <exception cref="FeedReferenceConfigurationException">
        /// If any configuration error occurs, such as unknown namespace, or missing
        /// required values, or errors while constructing the persistence or the service.
        /// </exception>
        public FeedReferenceService(string nameSpace)
        {
            CheckStringSDE(nameSpace, "nameSpace");

            ConfigManager cm = null;
            string loggerKey = null;
            string persistenceKey = null;
            string companyServiceKey = null;
            string objectFactoryNamespace = null;

            try
            {
                // Get ConfigManager instance
                cm = ConfigManager.GetInstance();

                // Get 'objectFactoryNamespace' property from configuration, optional
                objectFactoryNamespace = Helper.ReadString(cm, nameSpace, ConfigObjectFactoryNamespace, false);

                // Get 'auditClientKey' property from configuration, required
                _auditClientKey = Helper.ReadString(cm, nameSpace, ConfigAuditClientKey, true);

                // Get 'persistenceKey' property from configuration, required
                persistenceKey = Helper.ReadString(cm, nameSpace, ConfigPersistenceObjectKey, true);

                // Get 'companyServiceKey' property from configuration, required
                companyServiceKey = Helper.ReadString(cm, nameSpace, ConfigCompanyServiceObjectKey, true);

                // Get 'logger' property from configuration, required
                loggerKey = Helper.ReadString(cm, nameSpace, ConfigLoggerObjectKey, true);

                // Create ObjectFactory instance
                _auditServiceFactory = objectFactoryNamespace == null
                    ? ObjectFactory.GetDefaultObjectFactory()
                    : ObjectFactory.GetDefaultObjectFactory(objectFactoryNamespace);

                // Create persistence instance
                _persistence = (IFeedReferencePersistence)_auditServiceFactory.CreateDefinedObject(persistenceKey);

                // Create CompanyService instance
                _companyService = (CompanyService)_auditServiceFactory.CreateDefinedObject(companyServiceKey);

                // Create HermesLogger instance
                _logger = (HermesLogger)_auditServiceFactory.CreateDefinedObject(loggerKey);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(null,
                    new FeedReferenceConfigurationException("Failed to create FeedReferenceService instance.", ex),
                    new string[] { "nameSpace" }, new object[] { nameSpace },
                    new string[] { "cm", "loggerKey", "persistenceKey", "companyServiceKey", "objectFactoryNamespace" },
                    new object[] { cm, loggerKey, persistenceKey, companyServiceKey, objectFactoryNamespace }, false);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="TransponderType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="TransponderType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateTransponderType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public TransponderType CreateTransponderType(string name)
        {
            string userName = null;
            TransponderType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of TransponderType
                entity = CreateEntity<TransponderType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                // Call persistence method with the same name
                return _persistence.CreateTransponderType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex,
                    new string[] { "name" }, new object[] { name },
                    new string[] { "userName", "entity" }, new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="ChannelType"/> with the given name and <see cref="CompanyHeader"/>,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        /// <param name="company">
        /// The <see cref="CompanyHeader"/> of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="ChannelType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateChannelType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ChannelType CreateChannelType(string name, CompanyHeader company)
        {
            string userName = null;
            ChannelType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of ChannelType
                entity = CreateEntity<ChannelType>(userName, name);
                entity.Company = company;

                // Validate the entity
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateChannelType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex,
                    new string[] { "name", "company" }, new object[] { name, company },
                    new string[] { "userName", "entity" }, new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="VideoIpClusterIdType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="VideoIpClusterIdType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdateVideoIpClusterIdType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public VideoIpClusterIdType CreateVideoIpClusterIdType(string name)
        {
            string userName = null;
            VideoIpClusterIdType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of VideoIpClusterIdType
                entity = CreateEntity<VideoIpClusterIdType>(userName, name);

                // Validate entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateVideoIpClusterIdType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex,
                    new string[] { "name" }, new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="PhoneCouplerNumberType"/> with the given name
        /// and <see cref="PhoneCouplerType"/>, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        /// <param name="phoneCoupler">
        /// The <see cref="PhoneCouplerType"/> of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="PhoneCouplerNumberType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdatePhoneCouplerNumberType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public PhoneCouplerNumberType CreatePhoneCouplerNumberType(string name, PhoneCouplerType phoneCoupler)
        {
            string userName = null;
            PhoneCouplerNumberType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of PhoneCouplerNumberType
                entity = CreateEntity<PhoneCouplerNumberType>(userName, name);
                entity.PhoneCoupler = phoneCoupler;

                // Validate the entity using PhoneCouplerNumberTypeValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreatePhoneCouplerNumberType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex,
                    new string[] { "name" }, new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="PhoneCouplerType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="PhoneCouplerType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdatePhoneCouplerType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public PhoneCouplerType CreatePhoneCouplerType(string name)
        {
            string userName = null;
            PhoneCouplerType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of PhoneCouplerType
                entity = CreateEntity<PhoneCouplerType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreatePhoneCouplerType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="TieLineType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// the name of this entity
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="TieLineType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateTieLineType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public TieLineType CreateTieLineType(string name)
        {
            string userName = null;
            TieLineType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of TieLineType
                entity = CreateEntity<TieLineType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateTieLineType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="VideoStandardType"/> with the given name,
        /// and returns the instance with a new Id
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly create <see cref="VideoStandardType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateVideoStandardType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public VideoStandardType CreateVideoStandardType(string name)
        {
            string userName = null;
            VideoStandardType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of VideoStandardType
                entity = CreateEntity<VideoStandardType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateVideoStandardType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="CompressionFormatType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="CompressionFormatType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdateCompressionFormatType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public CompressionFormatType CreateCompressionFormatType(string name)
        {
            string userName = null;
            CompressionFormatType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of CompressionFormatType
                entity = CreateEntity<CompressionFormatType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateCompressionFormatType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="AudioEncodingType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="AudioEncodingType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateAudioEncodingType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AudioEncodingType CreateAudioEncodingType(string name)
        {
            string userName = null;
            AudioEncodingType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of AudioEncodingType
                entity = CreateEntity<AudioEncodingType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateAudioEncodingType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="AudioType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="AudioType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateAudioType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AudioType CreateAudioType(string name)
        {
            string userName = null;
            AudioType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of AudioType
                entity = CreateEntity<AudioType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateAudioType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="AspectRatioType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="AspectRatioType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateAspectRatioType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AspectRatioType CreateAspectRatioType(string name)
        {
            string userName = null;
            AspectRatioType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of AspectRatioType
                entity = CreateEntity<AspectRatioType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateAspectRatioType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="AdType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="AdType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateAdType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AdType CreateAdType(string name)
        {
            string userName = null;
            AdType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of AdType
                entity = CreateEntity<AdType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateAdType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="ModulationType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="ModulationType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateModulationType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ModulationType CreateModulationType(string name)
        {
            string userName = null;
            ModulationType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of ModulationType
                entity = CreateEntity<ModulationType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateModulationType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="DownlinkPolarityType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="DownlinkPolarityType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdateDownlinkPolarityType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public DownlinkPolarityType CreateDownlinkPolarityType(string name)
        {
            string userName = null;
            DownlinkPolarityType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of DownlinkPolarityType
                entity = CreateEntity<DownlinkPolarityType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateDownlinkPolarityType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="ShowCodeType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="ShowCodeType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateShowCodeType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ShowCodeType CreateShowCodeType(string name)
        {
            string userName = null;
            ShowCodeType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of ShowCodeType
                entity = CreateEntity<ShowCodeType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateShowCodeType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="UplinkPolarityType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="UplinkPolarityType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateUplinkPolarityType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public UplinkPolarityType CreateUplinkPolarityType(string name)
        {
            string userName = null;
            UplinkPolarityType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of UplinkPolarityType
                entity = CreateEntity<UplinkPolarityType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateUplinkPolarityType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="TapeLiveType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="TapeLiveType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateTapeLiveType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public TapeLiveType CreateTapeLiveType(string name)
        {
            string userName = null;
            TapeLiveType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of TapeLiveType
                entity = CreateEntity<TapeLiveType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateTapeLiveType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="EncryptionType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="EncryptionType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateEncryptionType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public EncryptionType CreateEncryptionType(string name)
        {
            string userName = null;
            EncryptionType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of EncryptionType
                entity = CreateEntity<EncryptionType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateEncryptionType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="FECType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="FECType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateFECType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public FECType CreateFECType(string name)
        {
            string userName = null;
            FECType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of FECType
                entity = CreateEntity<FECType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateFECType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="AiringType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="AiringType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateAiringType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
            TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AiringType CreateAiringType(string name)
        {
            string userName = null;
            AiringType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of AiringType
                entity = CreateEntity<AiringType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateAiringType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="NetworkType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="NetworkType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateNetworkType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public NetworkType CreateNetworkType(string name)
        {
            string userName = null;
            NetworkType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of NetworkType
                entity = CreateEntity<NetworkType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateNetworkType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="ChromaType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="ChromaType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateChromaType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ChromaType CreateChromaType(string name)
        {
            string userName = null;
            ChromaType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of ChromaType
                entity = CreateEntity<ChromaType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateChromaType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="SportType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="SportType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateSportType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public SportType CreateSportType(string name)
        {
            string userName = null;
            SportType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of SportType
                entity = CreateEntity<SportType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateSportType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="HaulType"/> with the given name, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="HaulType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateHaulType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public HaulType CreateHaulType(string name)
        {
            string userName = null;
            HaulType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of HaulType
                entity = CreateEntity<HaulType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateHaulType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="VideoFormatType"/> with the given name, <see cref="AdType"/>,
        /// and <see cref="VideoStandardType"/>, and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        /// <param name="ad">
        /// The <see cref="AdType"/> of this entity.
        /// </param>
        /// <param name="videoStandard">
        /// The <see cref="VideoStandardType"/> of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="VideoFormatType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateVideoFormatType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public VideoFormatType CreateVideoFormatType(string name, AdType ad, VideoStandardType videoStandard)
        {
            string userName = null;
            VideoFormatType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of VideoFormatType
                entity = CreateEntity<VideoFormatType>(userName, name);
                entity.Ad = ad;
                entity.VideoStandard = videoStandard;

                // Validate the entity using VideoFormatTypeValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateVideoFormatType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Creates the <see cref="SatelliteType"/> with the given name,
        /// and returns the instance with a new Id.
        /// </para>
        /// </summary>
        ///
        /// <param name="name">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The newly created <see cref="SatelliteType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null or empty string, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateSatelliteType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public SatelliteType CreateSatelliteType(string name)
        {
            string userName = null;
            SatelliteType entity = null;

            try
            {
                CheckStringSDE(name, "name");

                // Authorize method call
                userName = PerformAuthorization();

                // Create new instance of SatelliteType
                entity = CreateEntity<SatelliteType>(userName, name);

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(entity);

                // Audit entity and save the audit records
                SaveAuditInfo(entity.Audit(null));

                return _persistence.CreateSatelliteType(entity);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(entity), ex, new string[] { "name" },
                    new object[] { name }, new string[] { "userName", "entity" },
                    new object[] { userName, entity }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="TransponderType"/> in persistence, and return the saved entity.
        /// </para>
        /// </summary>
        ///
        /// <param name="transponderType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="TransponderType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateTransponderType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public TransponderType SaveTransponderType(TransponderType transponderType)
        {
            try
            {
                CheckNotNullSDE(transponderType, "transponderType");

                // Authorize method call and Set entity properties
                transponderType.LastModifiedBy = PerformAuthorization();
                transponderType.LastModifiedDate = DateTime.Now;

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(transponderType);

                // Audit entity and save the audit records
                SaveAuditInfo(transponderType.Audit(
                    GetEntityFromList(_persistence.GetAllTransponderTypes(), transponderType.Id)));

                // Call persistence method with the same name
                return _persistence.SaveTransponderType(transponderType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(transponderType), ex,
                    new string[] { "transponderType" }, new object[] { transponderType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="AdType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="adType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="AdType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or the entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateAdType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AdType SaveAdType(AdType adType)
        {
            try
            {
                CheckNotNullSDE(adType, "adType");

                // Authorize method call and set entity properties
                adType.LastModifiedBy = PerformAuthorization(); ;
                adType.LastModifiedDate = DateTime.Now;

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(adType);

                // Audit entity and save the audit records
                SaveAuditInfo(adType.Audit(GetEntityFromList(_persistence.GetAllAdTypes(), adType.Id)));

                return _persistence.SaveAdType(adType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(adType), ex,
                    new string[] { "adType" }, new object[] { adType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="ChannelType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="channelType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="ChannelType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or the entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateChannelType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ChannelType SaveChannelType(ChannelType channelType)
        {
            try
            {
                CheckNotNullSDE(channelType, "channelType");

                // Authorize method call and set entity properties
                channelType.LastModifiedDate = DateTime.Now;
                channelType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using ChennelTypeValidator
                ValidateEntity(channelType);

                // Audit entity and save the audit records
                SaveAuditInfo(channelType.Audit(
                    GetEntityFromList(_persistence.GetAllChannelTypes(), channelType.Id)));

                return _persistence.SaveChannelType(channelType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(channelType), ex,
                    new string[] { "channelType" }, new object[] { channelType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="VideoIpClusterIdType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="videoIpClusterIdType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="VideoIpClusterIdType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is nul, or entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdateVideoIpClusterIdType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public VideoIpClusterIdType SaveVideoIpClusterIdType(VideoIpClusterIdType videoIpClusterIdType)
        {
            try
            {
                CheckNotNullSDE(videoIpClusterIdType, "videoIpClusterIdType");

                // Authorize method call and Set entity properties
                videoIpClusterIdType.LastModifiedDate = DateTime.Now;
                videoIpClusterIdType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(videoIpClusterIdType);

                // Audit entity and save the audit records
                SaveAuditInfo(videoIpClusterIdType.Audit(
                    GetEntityFromList(_persistence.GetAllVideoIpClusterIdTypes(), videoIpClusterIdType.Id)));

                return _persistence.SaveVideoIpClusterIdType(videoIpClusterIdType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(videoIpClusterIdType), ex,
                    new string[] { "videoIpClusterIdType" },
                    new object[] { videoIpClusterIdType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="PhoneCouplerNumberType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="phoneCouplerNumberType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="PhoneCouplerNumberType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdatePhoneCouplerNumberType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public PhoneCouplerNumberType SavePhoneCouplerNumberType(PhoneCouplerNumberType phoneCouplerNumberType)
        {
            try
            {
                CheckNotNullSDE(phoneCouplerNumberType, "phoneCouplerNumberType");

                // Authorize method call and set entity properties
                phoneCouplerNumberType.LastModifiedDate = DateTime.Now;
                phoneCouplerNumberType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using PhoneCouplerNumberTypeValidator
                ValidateEntity(phoneCouplerNumberType);

                // Audit entity and save the audit records
                SaveAuditInfo(phoneCouplerNumberType.Audit(GetEntityFromList(
                    _persistence.GetPhoneCouplerNumberTypes(phoneCouplerNumberType.PhoneCoupler),
                    phoneCouplerNumberType.Id)));

                return _persistence.SavePhoneCouplerNumberType(phoneCouplerNumberType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(phoneCouplerNumberType), ex,
                    new string[] { "phoneCouplerNumberType" },
                    new object[] { phoneCouplerNumberType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="PhoneCouplerType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="phoneCouplerType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="PhoneCouplerType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdatePhoneCouplerType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public PhoneCouplerType SavePhoneCouplerType(PhoneCouplerType phoneCouplerType)
        {
            try
            {
                CheckNotNullSDE(phoneCouplerType, "phoneCouplerType");

                // Authorize method call and set entity properites
                phoneCouplerType.LastModifiedDate = DateTime.Now;
                phoneCouplerType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(phoneCouplerType);

                // Audit entity and save the audit records
                SaveAuditInfo(phoneCouplerType.Audit(GetEntityFromList(_persistence.GetAllPhoneCouplerTypes(),
                    phoneCouplerType.Id)));

                return _persistence.SavePhoneCouplerType(phoneCouplerType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(phoneCouplerType), ex,
                    new string[] { "phoneCouplerType" },
                    new object[] { phoneCouplerType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="TieLineType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="tieLineType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="TieLineType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateTieLineType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public TieLineType SaveTieLineType(TieLineType tieLineType)
        {
            try
            {
                CheckNotNullSDE(tieLineType, "tieLineType");

                // Authorize method call and set entity properties
                tieLineType.LastModifiedDate = DateTime.Now;
                tieLineType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(tieLineType);

                // Audit entity and save the audit records
                SaveAuditInfo(tieLineType.Audit(GetEntityFromList(_persistence.GetAllTieLineTypes(),
                    tieLineType.Id)));

                return _persistence.SaveTieLineType(tieLineType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(tieLineType), ex,
                    new string[] { "tieLineType" },
                    new object[] { tieLineType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="VideoStandardType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="videoStandardType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="VideoStandardType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateVideoStandardType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public VideoStandardType SaveVideoStandardType(VideoStandardType videoStandardType)
        {
            try
            {
                CheckNotNullSDE(videoStandardType, "videoStandardType");

                // Authorize method call and set entity properites
                videoStandardType.LastModifiedDate = DateTime.Now;
                videoStandardType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(videoStandardType);

                // Audit entity and save the audit records
                SaveAuditInfo(videoStandardType.Audit(GetEntityFromList(_persistence.GetAllVideoStandardTypes(),
                    videoStandardType.Id)));

                return _persistence.SaveVideoStandardType(videoStandardType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(videoStandardType), ex,
                    new string[] { "videoStandardType" },
                    new object[] { videoStandardType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="CompressionFormatType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="compressionFormatType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="CompressionFormatType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdateCompressionFormatType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public CompressionFormatType SaveCompressionFormatType(CompressionFormatType compressionFormatType)
        {
            try
            {
                CheckNotNullSDE(compressionFormatType, "compressionFormatType");

                // Authorize method call and set entity properites
                compressionFormatType.LastModifiedDate = DateTime.Now;
                compressionFormatType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(compressionFormatType);

                // Audit entity and save the audit records
                SaveAuditInfo(compressionFormatType.Audit(GetEntityFromList(
                    _persistence.GetAllCompressionFormatTypes(), compressionFormatType.Id)));

                return _persistence.SaveCompressionFormatType(compressionFormatType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(compressionFormatType), ex,
                    new string[] { "compressionFormatType" },
                    new object[] { compressionFormatType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="AudioEncodingType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="audioEncodingType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="AudioEncodingType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If name is null, or if the entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateAudioEncodingType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AudioEncodingType SaveAudioEncodingType(AudioEncodingType audioEncodingType)
        {
            try
            {
                CheckNotNullSDE(audioEncodingType, "audioEncodingType");

                // Authorize method call and set entity properties
                audioEncodingType.LastModifiedDate = DateTime.Now;
                audioEncodingType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(audioEncodingType);

                // Audit entity and save the audit records
                SaveAuditInfo(audioEncodingType.Audit(GetEntityFromList(_persistence.GetAllAudioEncodingTypes(),
                    audioEncodingType.Id)));

                return _persistence.SaveAudioEncodingType(audioEncodingType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(audioEncodingType), ex,
                    new string[] { "audioEncodingType" },
                    new object[] { audioEncodingType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="AudioType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="audioType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="AudioType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateAudioType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AudioType SaveAudioType(AudioType audioType)
        {
            try
            {
                CheckNotNullSDE(audioType, "audioType");

                // Authorize method call and set entity properites
                audioType.LastModifiedDate = DateTime.Now;
                audioType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(audioType);

                // Audit entity and save the audit records
                SaveAuditInfo(audioType.Audit(GetEntityFromList(_persistence.GetAllAudioTypes(), audioType.Id)));

                return _persistence.SaveAudioType(audioType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(audioType), ex,
                    new string[] { "audioType" }, new object[] { audioType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="AspectRatioType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="aspectRatioType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="AspectRatioType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateAspectRatioType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AspectRatioType SaveAspectRatioType(AspectRatioType aspectRatioType)
        {
            try
            {
                CheckNotNullSDE(aspectRatioType, "aspectRatioType");

                // Authorize method call and set entity properties
                aspectRatioType.LastModifiedDate = DateTime.Now;
                aspectRatioType.LastModifiedBy = PerformAuthorization();

                // Vaidate the entity using FeedReferenceValidator
                ValidateEntity(aspectRatioType);

                // Audit entity and save the audit records
                SaveAuditInfo(aspectRatioType.Audit(GetEntityFromList(_persistence.GetAllAspectRatioTypes(),
                    aspectRatioType.Id)));

                return _persistence.SaveAspectRatioType(aspectRatioType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(aspectRatioType), ex,
                    new string[] { "aspectRatioType" }, new object[] { aspectRatioType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="AiringType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="airingType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="AiringType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateAiringType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public AiringType SaveAiringType(AiringType airingType)
        {
            try
            {
                CheckNotNullSDE(airingType, "airingType");

                // Authorize method call and set entity properties
                airingType.LastModifiedDate = DateTime.Now;
                airingType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(airingType);

                // Audit entity and save the audit records
                SaveAuditInfo(
                    airingType.Audit(GetEntityFromList(_persistence.GetAllAiringTypes(), airingType.Id)));

                return _persistence.SaveAiringType(airingType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(airingType), ex,
                    new string[] { "airingType" }, new object[] { airingType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="ModulationType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="modulationType">
        /// The name of this entity.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="ModulationType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if the entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateModulationType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ModulationType SaveModulationType(ModulationType modulationType)
        {
            try
            {
                CheckNotNullSDE(modulationType, "modulationType");

                // Authorize method call and set entity properties
                modulationType.LastModifiedDate = DateTime.Now;
                modulationType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(modulationType);

                // Audit entity and save the audit records
                SaveAuditInfo(modulationType.Audit(GetEntityFromList(_persistence.GetAllModulationTypes(),
                    modulationType.Id)));

                return _persistence.SaveModulationType(modulationType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(modulationType), ex,
                    new string[] { "modulationType" }, new object[] { modulationType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="DownlinkPolarityType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="downlinkPolarityType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="DownlinkPolarityType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("UpdateDownlinkPolarityType")]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public DownlinkPolarityType SaveDownlinkPolarityType(DownlinkPolarityType downlinkPolarityType)
        {
            try
            {
                CheckNotNullSDE(downlinkPolarityType, "downlinkPolarityType");

                // Authorize method call and set entity properties
                downlinkPolarityType.LastModifiedDate = DateTime.Now;
                downlinkPolarityType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(downlinkPolarityType);

                // Audit entity and save the audit records
                SaveAuditInfo(downlinkPolarityType.Audit(GetEntityFromList(_persistence.GetAllDownlinkPolarityTypes(),
                    downlinkPolarityType.Id)));

                return _persistence.SaveDownlinkPolarityType(downlinkPolarityType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(downlinkPolarityType), ex,
                    new string[] { "downlinkPolarityType" },
                    new object[] { downlinkPolarityType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="ShowCodeType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="showCodeType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="ShowCodeType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateShowCodeType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ShowCodeType SaveShowCodeType(ShowCodeType showCodeType)
        {
            try
            {
                CheckNotNullSDE(showCodeType, "showCodeType");

                // Authorize method call and set entity properties
                showCodeType.LastModifiedDate = DateTime.Now;
                showCodeType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(showCodeType);

                // Audit entity and save the audit records
                SaveAuditInfo(showCodeType.Audit(GetEntityFromList(_persistence.GetAllShowCodeTypes(),
                    showCodeType.Id)));

                return _persistence.SaveShowCodeType(showCodeType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(showCodeType), ex,
                    new string[] { "showCodeType" }, new object[] { showCodeType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="UplinkPolarityType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="uplinkPolarityType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="UplinkPolarityType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateUplinkPolarityType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public UplinkPolarityType SaveUplinkPolarityType(UplinkPolarityType uplinkPolarityType)
        {
            try
            {
                CheckNotNullSDE(uplinkPolarityType, "uplinkPolarityType");

                // Authorize method call and set entity properties
                uplinkPolarityType.LastModifiedDate = DateTime.Now;
                uplinkPolarityType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(uplinkPolarityType);

                // Audit entity and save the audit records
                SaveAuditInfo(uplinkPolarityType.Audit(GetEntityFromList(_persistence.GetAllUplinkPolarityTypes(),
                    uplinkPolarityType.Id)));

                return _persistence.SaveUplinkPolarityType(uplinkPolarityType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(uplinkPolarityType), ex,
                    new string[] { "uplinkPolarityType" }, new object[] { uplinkPolarityType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="TapeLiveType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="tapeLiveType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="TapeLiveType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateTapeLiveType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public TapeLiveType SaveTapeLiveType(TapeLiveType tapeLiveType)
        {
            try
            {
                CheckNotNullSDE(tapeLiveType, "tapeLiveType");

                // Authorize method call and set entity properties
                tapeLiveType.LastModifiedDate = DateTime.Now;
                tapeLiveType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(tapeLiveType);

                // Audit entity and save the audit records
                SaveAuditInfo(tapeLiveType.Audit(GetEntityFromList(_persistence.GetAllTapeLiveTypes(),
                    tapeLiveType.Id)));

                return _persistence.SaveTapeLiveType(tapeLiveType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(tapeLiveType), ex,
                    new string[] { "tapeLiveType" }, new object[] { tapeLiveType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="EncryptionType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="encryptionType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="EncryptionType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateEncryptionType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public EncryptionType SaveEncryptionType(EncryptionType encryptionType)
        {
            try
            {
                CheckNotNullSDE(encryptionType, "encryptionType");

                // Authorize method call and set entity properties
                encryptionType.LastModifiedDate = DateTime.Now;
                encryptionType.LastModifiedBy = PerformAuthorization();

                // Validate entity
                ValidateEntity(encryptionType);

                // Audit entity and save the audit records
                SaveAuditInfo(encryptionType.Audit(GetEntityFromList(_persistence.GetAllEncryptionTypes(),
                    encryptionType.Id)));

                return _persistence.SaveEncryptionType(encryptionType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(encryptionType), ex,
                    new string[] { "encryptionType" }, new object[] { encryptionType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="FECType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="fecType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="FECType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateFECType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public FECType SaveFECType(FECType fecType)
        {
            try
            {
                CheckNotNullSDE(fecType, "fecType");

                // Authorize method call and set entity properties
                fecType.LastModifiedDate = DateTime.Now;
                fecType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(fecType);

                // Audit entity and save the audit records
                SaveAuditInfo(fecType.Audit(GetEntityFromList(_persistence.GetAllFECTypes(), fecType.Id)));

                return _persistence.SaveFECType(fecType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(fecType), ex,
                    new string[] { "fecType" }, new object[] { fecType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="ChromaType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="chromaType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="ChromaType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateChromaType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public ChromaType SaveChromaType(ChromaType chromaType)
        {
            try
            {
                CheckNotNullSDE(chromaType, "chromaType");

                // Authorize method call and set entity properties
                chromaType.LastModifiedDate = DateTime.Now;
                chromaType.LastModifiedBy = PerformAuthorization(); ;

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(chromaType);

                // Audit entity and save the audit records
                SaveAuditInfo(chromaType.Audit(
                    GetEntityFromList(_persistence.GetAllChromaTypes(), chromaType.Id)));

                return _persistence.SaveChromaType(chromaType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(chromaType), ex,
                    new string[] { "chromaType" }, new object[] { chromaType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="NetworkType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="networkType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="NetworkType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateNetworkType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public NetworkType SaveNetworkType(NetworkType networkType)
        {
            try
            {
                CheckNotNullSDE(networkType, "networkType");

                // Authorize method call and set entity properties
                networkType.LastModifiedDate = DateTime.Now;
                networkType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(networkType);

                // Audit entity and save the audit records
                SaveAuditInfo(networkType.Audit(
                    GetEntityFromList(_persistence.GetAllNetworkTypes(), networkType.Id)));

                return _persistence.SaveNetworkType(networkType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(networkType), ex,
                    new string[] { "networkType" }, new object[] { networkType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Save the <see cref="HaulType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="haulType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="HaulType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in the persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("UpdateHaulType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public HaulType SaveHaulType(HaulType haulType)
        {
            try
            {
                CheckNotNullSDE(haulType, "haulType");

                // Authorize method call and set entity properties
                haulType.LastModifiedDate = DateTime.Now;
                haulType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(haulType);

                // Audit entity and save the audit records
                SaveAuditInfo(haulType.Audit(
                    GetEntityFromList(_persistence.GetAllHaulTypes(), haulType.Id)));

                return _persistence.SaveHaulType(haulType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(haulType), ex,
                    new string[] { "haulType" }, new object[] { haulType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="SportType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="sportType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="SportType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateSportType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public SportType SaveSportType(SportType sportType)
        {
            try
            {
                CheckNotNullSDE(sportType, "sportType");

                // Authorize method call and set entity properties
                sportType.LastModifiedDate = DateTime.Now;
                sportType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(sportType);

                // Audit entity and save the audit records
                SaveAuditInfo(sportType.Audit(
                    GetEntityFromList(_persistence.GetAllSportTypes(), sportType.Id)));

                return _persistence.SaveSportType(sportType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(sportType), ex,
                    new string[] { "sportType" }, new object[] { sportType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="SatelliteType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="satelliteType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="SatelliteType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if the entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateSatelliteType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public SatelliteType SaveSatelliteType(SatelliteType satelliteType)
        {
            try
            {
                CheckNotNullSDE(satelliteType, "satelliteType");

                // Authorize method call and set entity properites
                satelliteType.LastModifiedDate = DateTime.Now;
                satelliteType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using FeedReferenceValidator
                ValidateEntity(satelliteType);

                // Audit entity and save the audit records
                SaveAuditInfo(satelliteType.Audit(
                    GetEntityFromList(_persistence.GetAllSatelliteTypes(), satelliteType.Id)));

                return _persistence.SaveSatelliteType(satelliteType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(satelliteType), ex,
                    new string[] { "satelliteType" }, new object[] { satelliteType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Saves the <see cref="VideoFormatType"/> in persistence, and returns the saved instance.
        /// </para>
        /// </summary>
        ///
        /// <param name="videoFormatType">
        /// The entity to save.
        /// </param>
        ///
        /// <returns>
        /// The saved <see cref="VideoFormatType"/>.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("UpdateVideoFormatType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FaultContract(typeof(HermesValidationFaultException))]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public VideoFormatType SaveVideoFormatType(VideoFormatType videoFormatType)
        {
            try
            {
                CheckNotNullSDE(videoFormatType, "videoFormatType");

                // Authorize method call and set entity properties
                videoFormatType.LastModifiedDate = DateTime.Now;
                videoFormatType.LastModifiedBy = PerformAuthorization();

                // Validate the entity using VideoFormatTypeValidator
                ValidateEntity(videoFormatType);

                // Audit entity and save the audit records
                SaveAuditInfo(videoFormatType.Audit(
                    GetEntityFromList(_persistence.GetAllVideoFormatTypes(), videoFormatType.Id)));

                return _persistence.SaveVideoFormatType(videoFormatType);
            }
            catch (FaultException<HermesValidationFaultException>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(videoFormatType), ex,
                    new string[] { "videoFormatType" }, new object[] { videoFormatType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="TransponderType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="transponderType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveTransponderType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteTransponderType(TransponderType transponderType)
        {
            try
            {
                CheckNotNullSDE(transponderType, "transponderType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(transponderType,
                    GetEntityFromList(_persistence.GetAllTransponderTypes(), transponderType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(transponderType));

                // Delete the entity from persistence
                _persistence.DeleteTransponderType(transponderType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(transponderType), ex,
                    new string[] { "transponderType" }, new object[] { transponderType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="ChannelType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="channelType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveChannelType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteChannelType(ChannelType channelType)
        {
            try
            {
                CheckNotNullSDE(channelType, "channelType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(channelType,
                    GetEntityFromList(_persistence.GetAllChannelTypes(), channelType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(channelType));

                // Delete the entity from persistence
                _persistence.DeleteChannelType(channelType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(channelType), ex,
                    new string[] { "channelType" }, new object[] { channelType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="VideoIpClusterIdType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="videoIpClusterIdType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("RemoveVideoIpClusterIdType")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteVideoIpClusterIdType(VideoIpClusterIdType videoIpClusterIdType)
        {
            try
            {
                CheckNotNullSDE(videoIpClusterIdType, "videoIpClusterIdType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(videoIpClusterIdType,
                    GetEntityFromList(_persistence.GetAllVideoIpClusterIdTypes(), videoIpClusterIdType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(videoIpClusterIdType));

                // Delete the entity from persistence
                _persistence.DeleteVideoIpClusterIdType(videoIpClusterIdType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(videoIpClusterIdType), ex,
                    new string[] { "videoIpClusterIdType" },
                    new object[] { videoIpClusterIdType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="PhoneCouplerNumberType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="phoneCouplerNumberType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("RemovePhoneCouplerNumberType")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeletePhoneCouplerNumberType(PhoneCouplerNumberType phoneCouplerNumberType)
        {
            try
            {
                CheckNotNullSDE(phoneCouplerNumberType, "phoneCouplerNumberType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(phoneCouplerNumberType, GetEntityFromList(
                    _persistence.GetPhoneCouplerNumberTypes(phoneCouplerNumberType.PhoneCoupler),
                    phoneCouplerNumberType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(phoneCouplerNumberType));

                // Delete the entity from persistence
                _persistence.DeletePhoneCouplerNumberType(phoneCouplerNumberType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(phoneCouplerNumberType), ex,
                    new string[] { "phoneCouplerNumberType" },
                    new object[] { phoneCouplerNumberType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="PhoneCouplerType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="phoneCouplerType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("RemovePhoneCouplerType")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeletePhoneCouplerType(PhoneCouplerType phoneCouplerType)
        {
            try
            {
                CheckNotNullSDE(phoneCouplerType, "phoneCouplerType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(phoneCouplerType,
                    GetEntityFromList(_persistence.GetAllPhoneCouplerTypes(), phoneCouplerType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(phoneCouplerType));

                // Delete the entity from persistence
                _persistence.DeletePhoneCouplerType(phoneCouplerType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(phoneCouplerType), ex,
                    new string[] { "phoneCouplerType" },
                    new object[] { phoneCouplerType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="TieLineType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="tieLineType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveTieLineType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteTieLineType(TieLineType tieLineType)
        {
            try
            {
                CheckNotNullSDE(tieLineType, "tieLineType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(tieLineType,
                    GetEntityFromList(_persistence.GetAllTieLineTypes(), tieLineType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(tieLineType));

                // Delete the entity from persistence
                _persistence.DeleteTieLineType(tieLineType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(tieLineType), ex,
                    new string[] { "tieLineType" },
                    new object[] { tieLineType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="VideoStandardType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="videoStandardType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveVideoStandardType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteVideoStandardType(VideoStandardType videoStandardType)
        {
            try
            {
                CheckNotNullSDE(videoStandardType, "videoStandardType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(videoStandardType,
                    GetEntityFromList(_persistence.GetAllVideoStandardTypes(), videoStandardType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(videoStandardType));

                // Delete the entity from persistence
                _persistence.DeleteVideoStandardType(videoStandardType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(videoStandardType), ex,
                    new string[] { "videoStandardType" },
                    new object[] { videoStandardType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="CompressionFormatType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="compressionFormatType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("RemoveCompressionFormatType")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteCompressionFormatType(CompressionFormatType compressionFormatType)
        {
            try
            {
                CheckNotNullSDE(compressionFormatType, "compressionFormatType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(compressionFormatType,
                    GetEntityFromList(_persistence.GetAllCompressionFormatTypes(), compressionFormatType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(compressionFormatType));

                // Delete the entity from persistence
                _persistence.DeleteCompressionFormatType(compressionFormatType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(compressionFormatType), ex,
                    new string[] { "compressionFormatType" },
                    new object[] { compressionFormatType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="AudioEncodingType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="audioEncodingType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveAudioEncodingType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteAudioEncodingType(AudioEncodingType audioEncodingType)
        {
            try
            {
                CheckNotNullSDE(audioEncodingType, "audioEncodingType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(audioEncodingType,
                    GetEntityFromList(_persistence.GetAllAudioEncodingTypes(), audioEncodingType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(audioEncodingType));

                // Delete the entity from persistence
                _persistence.DeleteAudioEncodingType(audioEncodingType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(audioEncodingType), ex,
                    new string[] { "audioEncodingType" }, new object[] { audioEncodingType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="AudioType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="audioType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RemoveAudioType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteAudioType(AudioType audioType)
        {
            try
            {
                CheckNotNullSDE(audioType, "audioType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(audioType, GetEntityFromList(_persistence.GetAllAudioTypes(), audioType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(audioType));

                // Delete the entity from persistence
                _persistence.DeleteAudioType(audioType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(audioType), ex,
                    new string[] { "audioType" }, new object[] { audioType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="AspectRatioType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="aspectRatioType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveAspectRatioType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteAspectRatioType(AspectRatioType aspectRatioType)
        {
            try
            {
                CheckNotNullSDE(aspectRatioType, "aspectRatioType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(aspectRatioType,
                    GetEntityFromList(_persistence.GetAllAspectRatioTypes(), aspectRatioType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(aspectRatioType));

                // Delete the entity from persistence
                _persistence.DeleteAspectRatioType(aspectRatioType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(aspectRatioType), ex,
                    new string[] { "aspectRatioType" }, new object[] { aspectRatioType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="AdType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="adType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RemoveAdType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteAdType(AdType adType)
        {
            try
            {
                CheckNotNullSDE(adType, "adType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(adType, GetEntityFromList(_persistence.GetAllAdTypes(), adType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(adType));

                // Delete the entity from persistence
                _persistence.DeleteAdType(adType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(adType), ex,
                    new string[] { "adType" }, new object[] { adType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="ModulationType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="modulationType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveModulationType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteModulationType(ModulationType modulationType)
        {
            try
            {
                CheckNotNullSDE(modulationType, "modulationType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(modulationType,
                    GetEntityFromList(_persistence.GetAllModulationTypes(), modulationType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(modulationType));

                // Delete the entity from persistence
                _persistence.DeleteModulationType(modulationType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(modulationType), ex,
                    new string[] { "modulationType" }, new object[] { modulationType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="DownlinkPolarityType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="downlinkPolarityType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [FunctionalAbilities("RemoveDownlinkPolarityType")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteDownlinkPolarityType(DownlinkPolarityType downlinkPolarityType)
        {
            try
            {
                CheckNotNullSDE(downlinkPolarityType, "downlinkPolarityType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(downlinkPolarityType,
                    GetEntityFromList(_persistence.GetAllDownlinkPolarityTypes(), downlinkPolarityType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(downlinkPolarityType));

                // Delete the entity from persistence
                _persistence.DeleteDownlinkPolarityType(downlinkPolarityType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(downlinkPolarityType), ex,
                    new string[] { "downlinkPolarityType" },
                    new object[] { downlinkPolarityType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="ShowCodeType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="showCodeType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveShowCodeType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteShowCodeType(ShowCodeType showCodeType)
        {
            try
            {
                CheckNotNullSDE(showCodeType, "showCodeType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(showCodeType,
                    GetEntityFromList(_persistence.GetAllShowCodeTypes(), showCodeType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(showCodeType));

                // Delete the entity from persistence
                _persistence.DeleteShowCodeType(showCodeType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(showCodeType), ex,
                    new string[] { "showCodeType" },
                    new object[] { showCodeType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="UplinkPolarityType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="uplinkPolarityType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveUplinkPolarityType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteUplinkPolarityType(UplinkPolarityType uplinkPolarityType)
        {
            try
            {
                CheckNotNullSDE(uplinkPolarityType, "uplinkPolarityType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(uplinkPolarityType,
                    GetEntityFromList(_persistence.GetAllUplinkPolarityTypes(), uplinkPolarityType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(uplinkPolarityType));

                // Delete the entity from persistence
                _persistence.DeleteUplinkPolarityType(uplinkPolarityType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(uplinkPolarityType), ex,
                    new string[] { "uplinkPolarityType" },
                    new object[] { uplinkPolarityType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="TapeLiveType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="tapeLiveType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveTapeLiveType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteTapeLiveType(TapeLiveType tapeLiveType)
        {
            try
            {
                CheckNotNullSDE(tapeLiveType, "tapeLiveType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(tapeLiveType,
                    GetEntityFromList(_persistence.GetAllTapeLiveTypes(), tapeLiveType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(tapeLiveType));

                // Delete the entity from persistence
                _persistence.DeleteTapeLiveType(tapeLiveType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(tapeLiveType), ex,
                    new string[] { "tapeLiveType" }, new object[] { tapeLiveType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="EncryptionType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="encryptionType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveEncryptionType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteEncryptionType(EncryptionType encryptionType)
        {
            try
            {
                CheckNotNullSDE(encryptionType, "encryptionType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(encryptionType,
                    GetEntityFromList(_persistence.GetAllEncryptionTypes(), encryptionType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(encryptionType));

                // Delete the entity from persistence
                _persistence.DeleteEncryptionType(encryptionType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(encryptionType), ex,
                    new string[] { "encryptionType" }, new object[] { encryptionType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="FECType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="fecType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RemoveFECType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteFECType(FECType fecType)
        {
            try
            {
                CheckNotNullSDE(fecType, "fecType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(fecType, GetEntityFromList(_persistence.GetAllFECTypes(), fecType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(fecType));

                // Delete the entity from persistence
                _persistence.DeleteFECType(fecType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(fecType), ex,
                    new string[] { "fecType" }, new object[] { fecType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="AiringType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="airingType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RemoveAiringType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteAiringType(AiringType airingType)
        {
            try
            {
                CheckNotNullSDE(airingType, "airingType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(airingType,
                    GetEntityFromList(_persistence.GetAllAiringTypes(), airingType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(airingType));

                // Delete the entity from persistence
                _persistence.DeleteAiringType(airingType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(airingType), ex,
                    new string[] { "airingType" }, new object[] { airingType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="NetworkType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="networkType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveNetworkType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteNetworkType(NetworkType networkType)
        {
            try
            {
                CheckNotNullSDE(networkType, "networkType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(networkType,
                    GetEntityFromList(_persistence.GetAllNetworkTypes(), networkType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(networkType));

                // Delete the entity from persistence
                _persistence.DeleteNetworkType(networkType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(networkType), ex,
                    new string[] { "networkType" }, new object[] { networkType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="ChromaType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="chromaType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveChromaType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteChromaType(ChromaType chromaType)
        {
            try
            {
                CheckNotNullSDE(chromaType, "chromaType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(chromaType,
                    GetEntityFromList(_persistence.GetAllChromaTypes(), chromaType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(chromaType));

                // Delete the entity from persistence
                _persistence.DeleteChromaType(chromaType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(chromaType), ex,
                    new string[] { "chromaType" }, new object[] { chromaType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="SportType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="sportType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RemoveSportType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteSportType(SportType sportType)
        {
            try
            {
                CheckNotNullSDE(sportType, "sportType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(sportType,
                    GetEntityFromList(_persistence.GetAllSportTypes(), sportType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(sportType));

                // Delete the entity from persistence
                _persistence.DeleteSportType(sportType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(sportType), ex,
                    new string[] { "sportType" }, new object[] { sportType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="HaulType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="haulType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RemoveHaulType")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteHaulType(HaulType haulType)
        {
            try
            {
                CheckNotNullSDE(haulType, "haulType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(haulType, GetEntityFromList(_persistence.GetAllHaulTypes(), haulType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(haulType));

                // Delete the entity from persistence
                _persistence.DeleteHaulType(haulType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(haulType), ex,
                    new string[] { "haulType" }, new object[] { haulType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="VideoFormatType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="videoFormatType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveVideoFormatType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteVideoFormatType(VideoFormatType videoFormatType)
        {
            try
            {
                CheckNotNullSDE(videoFormatType, "videoFormatType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(videoFormatType,
                    GetEntityFromList(_persistence.GetAllVideoFormatTypes(), videoFormatType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(videoFormatType));

                // Delete the entity from persistence
                _persistence.DeleteVideoFormatType(videoFormatType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(videoFormatType), ex,
                    new string[] { "videoFormatType" }, new object[] { videoFormatType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Deletes the passed <see cref="SatelliteType"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="satelliteType">
        /// The entity to delete.
        /// </param>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If argument is null, or if entity not in persistence,
        /// or if it is different from its current persisted state,
        /// or if any other errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RemoveSatelliteType")]
        [TransactionFlow(TransactionFlowOption.Mandatory)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public void DeleteSatelliteType(SatelliteType satelliteType)
        {
            try
            {
                CheckNotNullSDE(satelliteType, "satelliteType");

                // Authorize method call
                PerformAuthorization();

                // Check that the given entity match the one in persistence
                CheckEntitiesMatch(satelliteType,
                    GetEntityFromList(_persistence.GetAllSatelliteTypes(), satelliteType.Id));

                // Create HermesAuditRecord for the deleted entity and save the audit record
                SaveAuditInfo(GetAuditRecordsForDeletedEntity(satelliteType));

                // Delete the entity from persistence
                _persistence.DeleteSatelliteType(satelliteType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(satelliteType), ex,
                    new string[] { "satelliteType" }, new object[] { satelliteType }, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="TransponderType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="TransponderType"/> instances.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveTransponderTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<TransponderType> GetAllTransponderTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllTransponderTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="ChannelType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="ChannelType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveChannelTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<ChannelType> GetAllChannelTypes()
        {
            int i = 0;
            IList<ChannelType> channelTypes = null;

            try
            {
                // Perform authorization
                PerformAuthorization();

                // Get all channel types from persistence
                channelTypes = _persistence.GetAllChannelTypes();

                // Get the company associated with each element
                // in the list using CompanyService Service
                for (i = 0; i < channelTypes.Count; ++i)
                {
                    channelTypes[i].Company = _companyService.GetCompanyHeader(channelTypes[i].Company.Id);
                }

                return channelTypes;
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null,
                    new string[] { "i", "channelTypes" }, new object[] { i, channelTypes }, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="VideoIpClusterIdType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="VideoIpClusterIdType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveVideoIpClusterIdTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<VideoIpClusterIdType> GetAllVideoIpClusterIdTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllVideoIpClusterIdTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="PhoneCouplerType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="PhoneCouplerType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrievePhoneCouplerTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<PhoneCouplerType> GetAllPhoneCouplerTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllPhoneCouplerTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="TieLineType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="TieLineType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveTieLineTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<TieLineType> GetAllTieLineTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllTieLineTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="VideoStandardType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="VideoStandardType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveVideoStandardTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<VideoStandardType> GetAllVideoStandardTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllVideoStandardTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="CompressionFormatType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="CompressionFormatType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveCompressionFormatTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<CompressionFormatType> GetAllCompressionFormatTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllCompressionFormatTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="AudioEncodingType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="AudioEncodingType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveAudioEncodingTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<AudioEncodingType> GetAllAudioEncodingTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllAudioEncodingTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="AudioType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="AudioType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveAudioTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<AudioType> GetAllAudioTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllAudioTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="AspectRatioType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="AspectRatioType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveAspectRatioTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<AspectRatioType> GetAllAspectRatioTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllAspectRatioTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="AdType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="AdType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FunctionalAbilities("RetrieveAdTypes")]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<AdType> GetAllAdTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllAdTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="ModulationType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="ModulationType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveModulationTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<ModulationType> GetAllModulationTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllModulationTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="DownlinkPolarityType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="DownlinkPolarityType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveDownlinkPolarityTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<DownlinkPolarityType> GetAllDownlinkPolarityTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllDownlinkPolarityTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="ShowCodeType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="ShowCodeType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveShowCodeTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<ShowCodeType> GetAllShowCodeTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllShowCodeTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="UplinkPolarityType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="UplinkPolarityType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrieveUplinkPolarityTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<UplinkPolarityType> GetAllUplinkPolarityTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllUplinkPolarityTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="TapeLiveType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="TapeLiveType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveTapeLiveTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<TapeLiveType> GetAllTapeLiveTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllTapeLiveTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="EncryptionType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="EncryptionType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveEncryptionTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<EncryptionType> GetAllEncryptionTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllEncryptionTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="FECType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="FECType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveFECTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<FECType> GetAllFECTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllFECTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="AiringType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="AiringType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveAiringTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<AiringType> GetAllAiringTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllAiringTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="NetworkType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="NetworkType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveNetworkTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<NetworkType> GetAllNetworkTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllNetworkTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="ChromaType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="ChromaType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveChromaTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<ChromaType> GetAllChromaTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllChromaTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="SportType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="SportType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveSportTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<SportType> GetAllSportTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllSportTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="HaulType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="HaulType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveHaulTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<HaulType> GetAllHaulTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllHaulTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="VideoFormatType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="VideoFormatType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveVideoFormatTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<VideoFormatType> GetAllVideoFormatTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllVideoFormatTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="SatelliteType"/> instances. Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="SatelliteType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [FunctionalAbilities("RetrieveSatelliteTypes")]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<SatelliteType> GetAllSatelliteTypes()
        {
            try
            {
                PerformAuthorization();
                return _persistence.GetAllSatelliteTypes();
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// Gets a list of all <see cref="PhoneCouplerNumberType"/> instances.
        /// Returns empty list if none found.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The list of all <see cref="PhoneCouplerNumberType"/> instances
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// If the given argument is null, or if any errors occur while performing this operation.
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [FunctionalAbilities("RetrievePhoneCouplerNumberTypes")]
        [OperationBehavior(Impersonation = ImpersonationOption.Required,
           TransactionAutoComplete = false, TransactionScopeRequired = true)]
        public IList<PhoneCouplerNumberType> GetPhoneCouplerNumberTypes(PhoneCouplerType phoneCouplerType)
        {
            try
            {
                CheckNotNullSDE(phoneCouplerType, "phoneCouplerType");
                PerformAuthorization();
                return _persistence.GetPhoneCouplerNumberTypes(phoneCouplerType);
            }
            catch (Exception ex)
            {
                throw BuildSelfDocumentingException(ConstructMessage(null), ex, null, null, null, null, true);
            }
        }

        /// <summary>
        /// <para>
        /// This method is called when the Host property is set, to alert the subclass
        /// that the Host property has changed. This allows the subclass to register
        /// with the <see cref="ServiceHost"/> events immediately after the Host has been set.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// Not used in this component.
        /// </para>
        /// </remarks>
        protected override void HostUpdated()
        {
        }

        #region Helper API

        /// <summary>
        /// <para>
        /// Check that <see cref="IFeedReferenceType.LastModifiedDate"/> property
        /// of the given entities are equal.
        /// </para>
        /// </summary>
        ///
        /// <param name="entity1">The first entity to match.</param>
        /// <param name="entity2">The second entity to match.</param>
        ///
        /// <exception cref="SelfDocumentingException">
        /// If the LastModifiedDate property of the given entities are not the same.
        /// </exception>
        private void CheckEntitiesMatch(IFeedReferenceType entity1, IFeedReferenceType entity2)
        {
            if (!Helper.CheckDatesAreEqual(entity1.LastModifiedDate, entity2.LastModifiedDate))
            {
                throw Helper.BuildSelfDocumentingException(null,
                    new SelfDocumentingException(string.Join("|",
                    new string[] { "FeedReferenceService.EntitiesNotMatch", entity1.Id })),
                    InstanceNames, GetInstanceValues(), new string[] { "entity1", "entity2" },
                    new object[] { entity1, entity2 }, null, null);
            }
        }

        /// <summary>
        /// <para>
        /// Creates <see cref="BaseFeedReferenceType{T}"/> entity with the given name, and userID.
        /// </para>
        /// </summary>
        ///
        /// <typeparam name="T">
        /// Derives from <see cref="BaseFeedReferenceType{T}"/> class.
        /// </typeparam>
        ///
        /// <param name="userName">The user name that created the entity.</param>
        /// <param name="name">Used to set <see cref="BaseFeedReferenceType{T}.Name"/> property.</param>
        ///
        /// <returns><see cref="BaseFeedReferenceType{T}"/> instance.</returns>
        private static T CreateEntity<T>(string userName, string name)
            where T : BaseFeedReferenceType<T>, new()
        {
            T entity = new T();
            entity.Id = Guid.NewGuid().ToString();
            entity.LastModifiedBy = userName;
            entity.LastModifiedDate = DateTime.Now;
            entity.Name = name;

            return entity;
        }

        /// <summary>
        /// <para>
        /// Validates the given entity.
        /// </para>
        /// </summary>
        ///
        /// <remarks>
        /// <para>
        /// <see cref="FeedReferenceValidator"/> validator will be used for validation except for
        /// <see cref="ChannelType"/>, <see cref="VideoFormatType"/>, and <see cref="PhoneCouplerNumberType"/>
        /// entities. These entities will be validated using <see cref="ChannelTypeValidator"/>,
        /// <see cref="VideoFormatTypeValidator"/>and <see cref="PhoneCouplerNumberTypeValidator"/>,
        /// consecutively.
        /// </para>
        /// </remarks>
        ///
        /// <param name="entity">The entity to validate.</param>
        ///
        /// <exception cref="FaultException{HermesValidationFaultException}">
        /// If validation failed.
        /// </exception>
        private static void ValidateEntity(IFeedReferenceType entity)
        {
            FeedReferenceValidator validator = null;

            if (entity is ChannelType)
            {
                validator = new ChannelTypeValidator((ChannelType)entity);
            }
            else if (entity is PhoneCouplerNumberType)
            {
                validator = new PhoneCouplerNumberTypeValidator((PhoneCouplerNumberType)entity);
            }
            else if (entity is VideoFormatType)
            {
                validator = new VideoFormatTypeValidator((VideoFormatType)entity);
            }
            else
            {
                validator = new FeedReferenceValidator(entity);
            }

            if (!validator.Validate())
            {
                HermesValidationFaultException hvfe = new HermesValidationFaultException(
                    "Validation failed.", validator.DataValidationRecords);

                throw new FaultException<HermesValidationFaultException>(
                    hvfe, "The given entity is not valid.");
            }
        }

        /// <summary>
        /// <para>
        /// Check whether the given entity's id exist in the given list.
        /// </para>
        /// </summary>
        ///
        /// <typeparam name="T">
        /// Dervies from <see cref="BaseFeedReferenceType{T}"/>.
        /// </typeparam>
        ///
        /// <param name="entities">
        /// The entities used to check.
        /// </param>
        /// <param name="entityID">
        /// The entity's id to search for.
        /// </param>
        ///
        /// <returns>The entity for the given id.</returns>
        ///
        /// <exception cref="SelfDocumentingException">
        /// If no entity in the list matches entityID.
        /// </exception>
        private T GetEntityFromList<T>(IList<T> entities, string entityID)
            where T : BaseFeedReferenceType<T>
        {
            int index = 0;

            for (; index < entities.Count; ++index)
            {
                if (entities[index].Id == entityID)
                {
                    return entities[index];
                }
            }

            throw Helper.BuildSelfDocumentingException(null,
                new SelfDocumentingException(string.Join("|",
                new string[] { "FeedReferenceService.EntityMissing", entityID })),
                InstanceNames, GetInstanceValues(), new string[] { "entities", "entityID" },
                new object[] { entities, entityID }, new string[] { "index" } , new object[] { index });
        }

        /// <summary>
        /// <para>
        /// Returns array of the instance fields' values in this.
        /// </para>
        /// </summary>
        ///
        /// <returns>Array of the instances' values in this.</returns>
        private object[] GetInstanceValues()
        {
            return new object[] { _auditClientKey, _auditServiceFactory,
               _companyService, _logger, _persistence };
        }

        /// <summary>
        /// <para>
        /// Saves the given audit records using Hermes Audit Trail Component.
        /// </para>
        /// </summary>
        ///
        /// <param name="records">
        /// List of <see cref="HermesAuditRecord"/> records to be saved.
        /// </param>
        ///
        /// <exception cref="SelfDocumentingException">
        /// If any error occurs while performing the operation.
        /// </exception>
        private void SaveAuditInfo(IList<HermesAuditRecord> records)
        {
            AuditServiceClient proxy = null;

            try
            {
                // Create AuditServiceClient instance using ObjectFactory
                proxy = (AuditServiceClient)_auditServiceFactory.CreateDefinedObject(_auditClientKey);

                // Save the agiven audit records
                proxy.SaveAuditRecords(records);
            }
            catch (Exception ex)
            {
                throw Helper.BuildSelfDocumentingException("FeedReferenceService.AuditFailed",
                    ex, InstanceNames, GetInstanceValues(), new string[] { "records" },
                    new object[] { records }, new string[] { "proxy" }, new object[] { proxy });
            }
            finally
            {
                try
                {
                    proxy.Close();
                }
                catch
                {
                    try
                    {
                        proxy.Abort();
                    }
                    catch
                    {
                        // Do nothing
                    }
                }
            }
        }

        /// <summary>
        /// <para>
        /// Performs authorization.
        /// </para>
        /// </summary>
        ///
        /// <returns>
        /// The <see cref="Profile.UserName"/> value.
        /// </returns>
        ///
        /// <exception cref="SelfDocumentingException">
        /// If any error occurs while performing the operation.
        /// </exception>
        private string PerformAuthorization()
        {
            Profile profile = null;
            string applicationID = null;

            try
            {
                //applicationID = WcfHelper.GetApplicationID(OperationContext.Current);
                applicationID = "applicationID";
                //profile = WcfHelper.GetProfileFromContext(OperationContext.Current);
                profile = new Profile();
                profile.SessionID = "test_seesion_id";
                profile.SessionToken = "test_seesion_token";
                profile.UserID = "user_id";
                profile.UserName = "test_user_name";
                profile.Culture = "test_culture";
                HermesAuthorizationMediator.MediateMethod(applicationID, profile.SessionID,
                    profile.SessionToken, new StackTrace().GetFrame(1).GetMethod());

                return profile.UserName;
            }
            catch (Exception ex)
            {
                throw Helper.BuildSelfDocumentingException("FeedReferenceService.AuthorizationFailed",
                    ex, InstanceNames, GetInstanceValues(), null, null,
                    new string[] { "profile", "applicationID" }, new object[] { profile, applicationID });
            }
        }

        /// <summary>
        /// <para>
        /// Checks whether the given object is null.
        /// </para>
        /// </summary>
        ///
        /// <param name="value">
        /// The object to check.
        /// </param>
        /// <param name="paramName">
        /// The actual parameter name of the argument being checked.
        /// </param>
        ///
        /// <exception cref="SelfDocumentingException">
        /// If object is null.
        /// </exception>
        private void CheckNotNullSDE(object value, string paramName)
        {
            if (value == null)
            {
                throw Helper.BuildSelfDocumentingException(null, new InvalidArgumentException(
                    string.Join("|", new string[] { "FeedReferenceService.ArgumentNull", paramName }),
                    new ArgumentNullException()), InstanceNames, GetInstanceValues(),
                    new string[] { paramName }, new object[] { value }, null, null);
            }
        }

        /// <summary>
        /// <para>
        /// Checks whether the given string is null or empty.
        /// </para>
        /// </summary>
        ///
        /// <param name="value">
        /// The string to check.
        /// </param>
        /// <param name="paramName">
        /// The actual parameter name of the argument being checked.
        /// </param>
        ///
        /// <exception cref="SelfDocumentingException">
        /// If string is null, or if string is empty after trimming.
        /// </exception>
        private void CheckStringSDE(string value, string paramName)
        {
            if (value == null)
            {
                throw Helper.BuildSelfDocumentingException(null, new InvalidArgumentException(
                    string.Join("|", new string[] { "FeedReferenceService.ArgumentNull", paramName }),
                    new ArgumentNullException()), InstanceNames, GetInstanceValues(),
                    new string[] { paramName }, new object[] { value }, null, null);
            }

            if (value.Trim().Length == 0)
            {
                throw Helper.BuildSelfDocumentingException(null, new InvalidArgumentException(
                    string.Join("|", new string[] { "FeedReferenceService.ArgumentEmpty", paramName }),
                    new ArgumentException()), InstanceNames, GetInstanceValues(),
                    new string[] { paramName }, new object[] { value }, null, null);
            }
        }

        /// <summary>
        /// <para>
        /// Creates <see cref="HermesAuditRecord"/> instance for the deleted entity,
        /// this method is called by Delete methods in this class.
        /// </para>
        /// </summary>
        ///
        /// <param name="entity">The entity used to set EntityId and Message properties.</param>
        ///
        /// <returns>
        /// IList of <see cref="HermesAuditRecord"/> contain one record for the deleted entity.
        /// </returns>
        private static IList<HermesAuditRecord> GetAuditRecordsForDeletedEntity<T>(T entity)
            where T : BaseFeedReferenceType<T>
        {
            HermesAuditRecord[] records = new HermesAuditRecord[1];
            records[0] = Helper.GetHermesAuditRecord(Guid.NewGuid());
            records[0].EntityId = entity.Id;
            records[0].EventOutcomeCode = EventOutcomeCode.ObjectDeleted;
            records[0].Message = string.Format("{0}.DeletedAudit", entity.GetType().Name);

            return new List<HermesAuditRecord>(records);
        }

        /// <summary>
        /// <para>
        /// Construct the <see cref="SelfDocumentingException"/>'s message.
        /// </para>
        ///
        /// <para>
        /// If the given entity is not null, then the message will be in
        /// &quot;ClassName.MethodNameFailed|EntityId&quot; format,
        /// otherwise &quot;ClassName.MethodNameFailed&quot; will be returned.
        /// </para>
        /// </summary>
        ///
        /// <param name="entity">The entity used to get the id from.</param>
        ///
        /// <returns>
        /// The string message.
        /// </returns>
        private static string ConstructMessage(IFeedReferenceType entity)
        {
            string messageID = string.Format("{1}.{0}Failed",
                new StackTrace().GetFrame(1).GetMethod().Name, "FeedReferenceService");
            return (entity == null ? messageID : string.Join("|", new string[] { messageID, entity.Id }));
        }

        /// <summary>
        /// <para>
        /// Build the <see cref="SelfDocumentingException"/> and pin the related data.
        /// </para>
        /// <para>
        /// If wrap parameter is true, then log the exception and
        /// wrap it with <see cref="FaultException{TCFaultException}"/>.
        /// </para>
        /// </summary>
        ///
        /// <param name="message">
        /// A message that describes the error.
        /// </param>
        /// <param name="exception">
        /// The exception that is the cause of the current exception.
        /// </param>
        /// <param name="paramNames">
        /// The parameter variables name array needed to be added.
        /// </param>
        /// <param name="paramValues">
        /// The parameter variables value array needed to be added.
        /// </param>
        /// <param name="localNames">
        /// The local variable name array needed to be added.
        /// </param>
        /// <param name="localValues">
        /// The local variable array needed to be added.
        /// </param>
        /// <param name="wrap">
        /// Flag indicates whether to wrap the exception
        /// with <see cref="FaultException{TCFaultException}"/> or not.
        /// </param>
        ///
        /// <returns>
        /// A <see cref="Exception"/> instance.
        /// </returns>
        private Exception BuildSelfDocumentingException(string message, Exception exception,
            string[] paramNames, object[] paramValues, string[] localNames, object[] localValues, bool wrap)
        {
            SelfDocumentingException sde = Helper.BuildSelfDocumentingException(message, exception,
                InstanceNames, GetInstanceValues(), paramNames, paramValues, localNames, localValues);

            if (wrap)
            {
                try
                {
                    string[] messages = sde.Message.Split('|');
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

                return new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde),
                    sde.Message);
            }

            return sde;
        }

        #endregion
    }
}
