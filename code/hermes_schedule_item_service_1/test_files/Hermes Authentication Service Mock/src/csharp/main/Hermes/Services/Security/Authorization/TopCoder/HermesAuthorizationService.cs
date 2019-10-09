/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using TopCoder.Services.WCF;
using TopCoder.Util.ExceptionManager.SDE;

namespace Hermes.Services.Security.Authorization.TopCoder
{
 
    /// <summary>
    /// <para>The HermesAuthorizationService provided by TopCoder for the purpose of testing.</para>
    /// <para>In its constructor, it will read values from configuration file. And all behaviors
    /// of this class depends on configuration.</para>
    /// </summary>
    /// 
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class HermesAuthorizationService : TCWcfServiceBase, IAuthorization, IDisposable
    {
        /// <summary>
        /// <para>The string representation for 'login_namespace' property.</para>
        /// </summary>
        private const string LoginNamespaceProperty = "login_namespace";

        /// <summary>
        /// <para>The string representation for 'role_namespace' property.</para>
        /// </summary>
        private const string RoleNamespaceProperty = "role_namespace";

        /// <summary>
        /// <para>The string representation for 'function_namespace' property.</para>
        /// </summary>
        private const string FunctionNamespaceProperty = "function_namespace";

        /// <summary>
        /// <para>The string representation for 'entity_namespace' property.</para>
        /// </summary>
        private const string EntityNamespaceProperty = "entity_namespace";

        /// <summary>
        /// <para>The string representation for 'sessionIds' property.</para>
        /// </summary>
        private const string SessionIdsProperty = "sessionIds";

        /// <summary>
        /// <para>The string representation for '_allow' postfix.</para>
        /// </summary>
        private const string AllowPostfix = "_allow";

        /// <summary>
        /// <para>The string representation for '_deny' postfix.</para>
        /// </summary>
        private const string DenyPostfix = "_deny";

        /// <summary>
        /// <para>The string representation for 'roleNames' property.</para>
        /// </summary>
        private const string RoleNamesProperty = "roleNames";

        /// <summary>
        /// <para>The string representation for '_permission' postfix.</para>
        /// </summary>
        private const string PermissionPostfix = "_permission";
        
        /// <summary>
        /// <para>The string representation for 'funtionNames' property.</para>
        /// </summary>
        private const string FunctionNamesProperty = "functionNames";

        /// <summary>
        /// <para>The string representation for '_attributeNames' postfix.</para>
        /// </summary>
        private const string AttributeNamesPostfix = "_attributesNames";

        /// <summary>
        /// <para>The string representation for '_attributeValues' postfix.</para>
        /// </summary>
        private const string AttributeValuesPostfix = "_attributesValues";

        /// <summary>
        /// <para>The string representation for 'entityNames' property.</para>
        /// </summary>
        private const string EntityNamesProperty = "entityNames";

        /// <summary>
        /// <para>The string representation for '_rights' postfix.</para>
        /// </summary>
        private const string RightsPostfix = "_rights";
        
        /// <summary>
        /// <para>Maps from 'sessionId' and 'sessionToken' pair to permission.</para>
        /// </summary>
        private readonly IDictionary<KeyValuePair<string, string>, Boolean> _Logins 
            = new Dictionary<KeyValuePair<string, string>, Boolean>();

        /// <summary>
        /// <para>Maps from  role name to permission.</para>
        /// </summary>
        private readonly IDictionary<string, Boolean> _Roles
            = new Dictionary<string, Boolean>();

        /// <summary>
        /// <para>Maps from  function name to permission.</para>
        /// </summary>
        private readonly IDictionary<string, Boolean> _Functions
            = new Dictionary<string, Boolean>();

        /// <summary>
        /// <para>Maps from  funtion name to a list of attributes defined. The list of attributes
        /// consisted of 'attribute name' and 'attribute value' pairs.</para>
        /// </summary>
        private readonly IDictionary<string, List<KeyValuePair<string, string>>> _FunctionsAttributes
            = new Dictionary<string, List<KeyValuePair<string, string>>>();

        /// <summary>
        /// <para>Maps from  entity name to its Rights enum value.</para>
        /// </summary>
        private readonly IDictionary<string, Rights> _Entities
            = new Dictionary<string, Rights>();

        /// <summary>
        /// <para>The application id. It is initialized as empty string and can be set via 
        /// SetApplicationId(string) method. When setting, it is not allowed to be null or empty string.
        /// </para>
        /// </summary>
        private string _AppId = String.Empty;

        /// <summary>
        /// <para>This is a flag to indicate if this object has been disposed or not.</para>
        /// </summary>
        /// <remarks>
        /// <para>It is initialized to false, and set to true in the <see cref="Dispose(bool)"/> method. 
        /// It is checked by all of the public service operations, which will throw 
        /// <see cref="ObjectDisposedException"/> if it's true.</para>
        /// <para>Reads and writes to this field are atomic, but it's marked as volitile since it's assumed 
        /// that <see cref="Dispose()"/> may be called from a different thread than the main operations.</para>
        /// </remarks>
        private volatile bool _IsDisposed = false;
        
        /// <summary>
        /// <para>The constructor which will load configuration from default namespace:
        /// "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService".
        /// </para>
        /// </summary>
        /// <exception cref="SelfDocumentingException">
        /// Inner exception:
        /// <see cref="ConfigurationException"/> (if a required parameter is m-
        /// issing or if there's a problem reading the given namespace from co-
        /// nfiguration).
        /// </exception>
        public HermesAuthorizationService() : this(typeof(HermesAuthorizationService).FullName)
        {
        }
        
        /// <summary>
        /// <para>The constructor which will load configuration from given namespace.</para>
        /// </summary>
        /// <param name="nameSpace">the namespace to load configuration</param>
        /// <exception cref="SelfDocumentingException">
        /// Inner exception:
        /// <see cref="ConfigurationException"/> (if a required parameter is m-
        /// issing or if there's a problem reading the given namespace from co-
        /// nfiguration), <see cref="ArgumentNullException"/> (if the
        /// <paramref name="nameSpace"/> is null),
        /// <see cref="ArgumentException"/>(if the <paramref name="nameSpace"/>
        /// is trimmed empty).
        /// </exception>
        public HermesAuthorizationService(string nameSpace)
        {
            try
            {
                Helper.CheckString(nameSpace, "nameSpace");

                InitializeLogins(Helper.LoadPropertyString(nameSpace, LoginNamespaceProperty, true));
                InitializeRoles(Helper.LoadPropertyString(nameSpace, RoleNamespaceProperty, true));
                InitializeFunctions(Helper.LoadPropertyString(nameSpace, FunctionNamespaceProperty, true));
                InitializeEntities(Helper.LoadPropertyString(nameSpace, EntityNamespaceProperty, true));
            }
            catch (Exception e)
            {
                throw PopulateSDE("Failed to create instance of HermesAuthorizationService.", e,
                                  "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService",
                                  new string[] {"nameSpace"}, new object[] {nameSpace},
                                  new string[0], new object[0],
                                  new string[] {"_Logins", "_Roles", "_Functions", "_FunctionsAttributes", 
                                      "_Entities", "_AppId"}, 
                                  new object[] {_Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId});
            }
        }
        
        /// <summary>
        /// <para>Initialize _Logins with configuration from given namespace.</para>
        /// </summary>
        /// <param name="nameSpace">the namespace to load configuration</param>
        /// <exception cref="ConfigurationException">if any property value is empty string,
        /// or if error occurs while reading configuration</exception>
        private void InitializeLogins(string nameSpace)
        {
            string[] sessionIds = Helper.GetValues(nameSpace, SessionIdsProperty, false);
            foreach (string sessionId in sessionIds)
            {
                // for <sessionId>_allow
                string[] allowedTokens = Helper.GetValues(nameSpace, sessionId + AllowPostfix, true);
                foreach (string allowedToken in allowedTokens)
                {
                    _Logins[new KeyValuePair<string, string>(sessionId, allowedToken)] = true;
                }
                
                // for <sessionId>_deny
                string[] deniedTokens = Helper.GetValues(nameSpace, sessionId + DenyPostfix, true);
                foreach (string deniedToken in deniedTokens)
                {
                    _Logins[new KeyValuePair<string, string>(sessionId, deniedToken)] = false;
                }
            }
        }

        /// <summary>
        /// <para>Initialize _Roles with configuration from given namespace.</para>
        /// </summary>
        /// <param name="nameSpace">the namespace to load configuration</param>
        /// <exception cref="ConfigurationException">if any property value is empty string,
        /// or if error occurs while reading configuration</exception>
        private void InitializeRoles(string nameSpace)
        {
            string[] roleNames = Helper.GetValues(nameSpace, RoleNamesProperty, false);
            foreach (string roleName in roleNames)
            {
                // for <roleName>_permission
                string permission = Helper.LoadPropertyString(nameSpace, roleName + PermissionPostfix, false);

                bool isPermissioned = permission == null ? false : 
                                      permission.ToLower().Equals("true") || permission.ToLower().Equals("yes");

                _Roles[roleName] = isPermissioned;
            }
        }

        /// <summary>
        /// <para>Initialize _Functions and _FuntionAttributes with configuration from given namespace.</para>
        /// </summary>
        /// <param name="nameSpace">the namespace to load configuration</param>
        /// <exception cref="ConfigurationException">if any property value is empty string,
        /// or if error occurs while reading configuration</exception>
        private void InitializeFunctions(string nameSpace)
        {
            string[] functionNames = Helper.GetValues(nameSpace, FunctionNamesProperty, false);
            foreach (string functionName in functionNames)
            {
                // for <functionName>_permission
                string permission = Helper.LoadPropertyString(nameSpace, functionName + PermissionPostfix, false);

                bool isPermissioned = permission == null ? false :
                                      permission.ToLower().Equals("true") || permission.ToLower().Equals("yes");

                _Functions[functionName] = isPermissioned;
                _FunctionsAttributes[functionName] = new List<KeyValuePair<string, string>> ();
                
                // for <functionName>_attributesNames and <functionName>_attributesValues
                string[] attributesNames = Helper.GetValues(nameSpace, functionName + AttributeNamesPostfix, false);
                string[] attributesValues = Helper.GetValues(nameSpace, functionName + AttributeValuesPostfix, false);
                if (attributesNames.Length != attributesValues.Length)
                {
                    throw new ConfigurationErrorsException(
                        "The length of attribute names is not the same as the length of attribute values " 
                        + "for function name: " + functionName);
                }

                for (int i = 0; i < attributesNames.Length; i++)
                {
                    _FunctionsAttributes[functionName].Add(
                        new KeyValuePair<string, string>(attributesNames[i], attributesValues[i]));
                }
            }
        }

        /// <summary>
        /// <para>Initialize _Entities with configuration from given namespace.</para>
        /// </summary>
        /// <param name="nameSpace">the namespace to load configuration</param>
        /// <exception cref="ConfigurationException">if any property value is empty string,
        /// or if error occurs while reading configuration</exception>
        private void InitializeEntities(string nameSpace)
        {
            string[] entityNames = Helper.GetValues(nameSpace, EntityNamesProperty, false);
            foreach (string entityName in entityNames)
            {
                // for <entityName>_rights
                string[] rights = Helper.GetValues(nameSpace, entityName + RightsPostfix, false);

                Rights rightsValue = (Rights) 0;
                foreach (string right in rights)
                {
                    rightsValue |= (Rights) Enum.Parse(typeof (Rights), right, true);
                }

                _Entities[entityName] = rightsValue;
            }
        }
        
        /// <summary>
        /// <para>
        /// Check if the user for the specified session belongs to the
        /// specified role.
        /// </para>
        /// </summary>
        ///
        /// <param name="sessionId">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="roleName">the name of the role to check</param>
        /// <return>
        /// true if the user belongs to the role; false otherwise
        /// </return>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// The inner exception contains:
        /// <see cref="ArgumentNullException"/> (if any argument is null),
        /// <see cref="ArgumentException"/> (if <paramref name="sessionId"/>
        /// or <paramref name="roleName"/> is empty),
        /// <see cref="InvalidSessionException"/>(if the specified session is
        /// not valid), <see cref="AuthorizationServiceException"/>(if there's
        /// a problem interacting with the wrapped authentication service),
        /// <see cref="ObjectDisposedException"/>(if the object has been
        /// disposed).
        /// </exception>
        public bool CheckRole(string sessionId, string sessionToken, string roleName)
        {
            try
            {
                Helper.CheckString(roleName, "roleName");

                CheckDisposed();
                
                AuthLogin(sessionId, sessionToken);
                
                if (_Roles.ContainsKey(roleName))
                {
                    return _Roles[roleName];
                }
                else
                {
                    throw new AuthorizationServiceException(
                        String.Format("The role name {0} is not configured.", roleName));
                }
            }
            catch (Exception e)
            {
                Exception sde = PopulateSDE(
                    "Error occurs while checking role.", e,
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService.CheckRole",
                    new string[] {"sessionId", "sessionToken", "roleName"}, 
                    new object[]{sessionId, sessionToken, roleName},
                    new string[0], new object[0],
                    new string[] {"_Logins", "_Roles", "_Functions", "_FunctionsAttributes", "_Entities", "_AppId"},
                    new object[] { _Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId });
                
                throw new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), sde.Message);
            }
        }

        /// <summary>
        /// <para>Use session id and session token to login.</para>
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="sessionToken">The session token.</param>
        /// <exception cref="InvalidSessionException">If login with <paramref name="sessionId"/> and
        /// <paramref name="sessionToken"/> failed.</exception>
        /// <exception cref="ArgumentNullException">if <paramref name="sessionId"/> or <paramref name="sessionToken"/>
        /// is null.</exception>
        /// <exception cref="ArgumentException">if <paramref name="sessionId"/> is empty.</exception>
        private void AuthLogin(string sessionId, string sessionToken)
        {
            Helper.CheckString(sessionId, "sessionId");
            Helper.CheckNotNull(sessionToken, "sessionToken");

            KeyValuePair<string, string> pair = new KeyValuePair<string, string>(sessionId, sessionToken);
            if (!_Logins.ContainsKey(pair) || !_Logins[pair])
            {
                throw new InvalidSessionException(
                    string.Format("Login with sessionId: {0} sessionToken: {1} failed.",
                                  new object[]{sessionId, sessionToken}));
            }
        }
        
        /// <summary>
        /// <para>
        /// Check if the user for the specified session can execute the
        /// specified function.
        /// </para>
        /// </summary>
        ///
        /// <param name="sessionId">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="functionName">the name of the function to check</param>
        /// <return>
        /// true if the current user has rights to perform the specified
        /// function
        /// </return>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// The inner exception contains:
        /// <see cref="ArgumentNullException"/> (if any argument is null),
        /// <see cref="ArgumentException"/> (if <paramref name="sessionId"/> or
        /// <paramref name="functionName"/> is empty),
        /// <see cref="InvalidSessionException"/>(if the specified session is
        /// not valid), <see cref="AuthorizationServiceException"/>(if there's
        /// a problem interacting with the wrapped authentication service), 
        /// <see cref="ObjectDisposedException"/>(if the object has been
        /// disposed).
        /// </exception>
        public bool CheckFunction(string sessionId, string sessionToken, string functionName)
        {
            try
            {
                Helper.CheckString(functionName, "functionName");

                CheckDisposed();
                
                AuthLogin(sessionId, sessionToken);

                if (_Functions.ContainsKey(functionName))
                {
                    return _Functions[functionName];
                }
                else
                {
                    throw new AuthorizationServiceException(
                        String.Format("The function name {0} is not configured.", functionName));
                }
            }
            catch (Exception e)
            {
                Exception sde = PopulateSDE(
                    "Error occurs while checking function.", e,
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService.CheckFunction",
                    new string[] { "sessionId", "sessionToken", "functionName" },
                    new object[] { sessionId, sessionToken, functionName },
                    new string[0], new object[0],
                    new string[] { "_Logins", "_Roles", "_Functions", "_FunctionsAttributes", "_Entities", "_AppId" },
                    new object[] { _Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId });

                throw new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), sde.Message);
            }
        }

        /// <summary>
        /// <para>
        /// Check if the user for the current session has a specified set of
        /// rights to a given entity.
        /// </para>
        /// </summary>
        ///
        /// <param name="sessionId">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="entityName">the name of the entity to check</param>
        /// <param name="rights">
        /// an enumeration of all the rights to check
        /// </param>
        /// <return>
        /// true if the current user has all of the specified rights for the
        /// given entity
        /// </return>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// The inner exception contains:
        /// <see cref="ArgumentNullException"/> (if any argument is null),
        /// <see cref="ArgumentException"/> (if <paramref name="sessionId"/> or
        /// <paramref name="entityName"/> is empty),
        /// <see cref="InvalidSessionException"/>(if the specified session is
        /// not valid), <see cref="AuthorizationServiceException"/>(if there's
        /// a problem interacting with the wrapped authentication service), 
        /// <see cref="ObjectDisposedException"/>(if the object has been
        /// disposed).
        /// </exception>
        public bool CheckEntity(string sessionId, string sessionToken, string entityName, Rights rights)
        {
            try
            {
                Helper.CheckString(entityName, "entityName");

                CheckRights(rights, "rights");
                CheckDisposed();
                
                AuthLogin(sessionId, sessionToken);
                
                if (!_Entities.ContainsKey(entityName))
                {
                    throw new AuthorizationServiceException(
                        String.Format("The entity name {0} is not configured.", entityName));
                }

                return (_Entities[entityName] & rights) == rights;
            }
            catch (Exception e)
            {
                Exception sde = PopulateSDE(
                    "Error occurs while checking rights for entigy.", e,
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService.CheckEntity",
                    new string[] { "sessionId", "sessionToken", "entityName", "rights" },
                    new object[] { sessionId, sessionToken, entityName, rights },
                    new string[0], new object[0],
                    new string[] { "_Logins", "_Roles", "_Functions", "_FunctionsAttributes", "_Entities", "_AppId" },
                    new object[] { _Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId });

                throw new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), sde.Message);
            }
        }

        /// <summary>
        /// <para>
        /// Retrieve all of the rights a user has for a given entity.
        /// </para>
        /// </summary>
        ///
        /// <param name="sessionId">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="entityName">the name of the entity to check</param>
        /// <return>
        /// all of the rights the user has for the given entity
        /// </return>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// The inner exception contains:
        /// <see cref="ArgumentNullException"/> (if any argument is null),
        /// <see cref="ArgumentException"/> (if <paramref name="sessionId"/> or
        /// <paramref name="entityName"/> is empty),
        /// <see cref="InvalidSessionException"/>(if the specified session is
        /// not valid), <see cref="AuthorizationServiceException"/>(if there's
        /// a problem interacting with the wrapped authentication service), 
        /// <see cref="ObjectDisposedException"/>(if the object has been
        /// disposed).
        /// </exception>
        public Rights GetEntityRights(string sessionId, string sessionToken, string entityName)
        {
            try
            {
                Helper.CheckString(entityName, "entityName");

                CheckDisposed();
                
                AuthLogin(sessionId, sessionToken);

                if (!_Entities.ContainsKey(entityName))
                {
                    throw new AuthorizationServiceException(
                        String.Format("The entity name {0} is not configured.", entityName));
                }

                return _Entities[entityName];
            }
            catch (Exception e)
            {
                Exception sde = PopulateSDE(
                    "Error occurs while getting rights for entigy.", e,
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService.GetEntityRights",
                    new string[] { "sessionId", "sessionToken", "entityName" },
                    new object[] { sessionId, sessionToken, entityName },
                    new string[0], new object[0],
                    new string[] { "_Logins", "_Roles", "_Functions", "_FunctionsAttributes", "_Entities", "_AppId" },
                    new object[] { _Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId });

                throw new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), sde.Message);
            }
        }

        /// <summary>
        /// <para>
        /// Get the attributes from the specified function.
        /// </para>
        /// </summary>
        /// <param name="sessionId">id of the current session</param>
        /// <param name="sessionToken">token for the current session</param>
        /// <param name="functionName">
        /// the name of the function.
        /// </param>
        /// <returns>
        /// The name-value pairs of the Attributes property for a specific
        /// wrapped FunctionalAttribute instance.
        /// </returns>
        ///
        /// <exception cref="FaultException{TCFaultException}">
        /// The inner exception contains:
        /// <see cref="ArgumentNullException"/> (if any argument is null),
        /// <see cref="ArgumentException"/> (if <paramref name="sessionId"/> or
        /// <paramref name="functionName"/> is empty),
        /// <see cref="InvalidSessionException"/>(if the specified session is
        /// not valid), <see cref="AuthorizationServiceException"/>(if there's
        /// a problem interacting with the wrapped authentication service),
        /// <see cref="ObjectDisposedException"/>(if the object has been
        /// disposed).
        /// </exception>
        public List<KeyValuePair<string, string>> GetFunctionAttributes(string sessionId, 
                                                                        string sessionToken, 
                                                                        string functionName)
        {
            try
            {
                Helper.CheckString(functionName, "functionName");

                CheckDisposed();
                
                AuthLogin(sessionId, sessionToken);

                if (_FunctionsAttributes.ContainsKey(functionName))
                {
                    return _FunctionsAttributes[functionName];
                }
                else
                {
                    throw new AuthorizationServiceException(
                        String.Format("The function name {0} is not configured.", functionName));
                }
            }
            catch (Exception e)
            {
                Exception sde = PopulateSDE(
                    "Error occurs while checking function.", e,
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService.GetFunctionAttributes",
                    new string[] { "sessionId", "sessionToken", "functionName" },
                    new object[] { sessionId, sessionToken, functionName },
                    new string[0], new object[0],
                    new string[] { "_Logins", "_Roles", "_Functions", "_FunctionsAttributes", "_Entities", "_AppId" },
                    new object[] { _Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId });

                throw new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), sde.Message);
            }
        }

        /// <summary>
        /// <para>
        /// Set the applicaton id.
        /// </para>
        /// </summary>
        /// <param name="appId">
        /// The applicaton id to be set.
        /// </param>
        /// <exception cref="FaultException{TCFaultException}">
        /// The inner exception contains:
        /// <see cref="ArgumentNullException"/> (if any argument is null),
        /// <see cref="ArgumentException"/> (if <paramref name="appId"/>
        /// is empty), 
        /// <see cref="ObjectDisposedException"/>(if the object has been
        /// disposed).
        /// </exception>
        public void SetApplication(string appId)
        {
            try
            {
                Helper.CheckString(appId, "appId");

                CheckDisposed();
                
                _AppId = appId;
            }
            catch (Exception e)
            {
                Exception sde = PopulateSDE(
                    "Error occurs while checking function.", e,
                    "Hermes.Services.Security.Authorization.TopCoder.HermesAuthorizationService.SetApplication",
                    new string[] { "appId" },
                    new object[] { appId },
                    new string[0], new object[0],
                    new string[] { "_Logins", "_Roles", "_Functions", "_FunctionsAttributes", "_Entities", "_AppId" },
                    new object[] { _Logins, _Roles, _Functions, _FunctionsAttributes, _Entities, _AppId });
                throw new FaultException<TCFaultException>(TCFaultException.CreateFromException(sde), sde.Message);
            }
        }
        
        /// <summary>
        /// <para>
        /// This method isn't needed by this service.
        /// </para>
        /// </summary>
        protected override void HostUpdated()
        {
        }

        /// <summary>
        /// <para>
        /// Checks this service has been disposed.
        /// </para>
        /// </summary>
        ///
        /// <exception cref="ObjectDisposedException">If it has been disposed.</exception>
        private void CheckDisposed()
        {
            if (_IsDisposed)
            {
                throw new ObjectDisposedException("object is disposed.");
            }
        }
        
        /// <summary>
        /// <para>
        /// Disposes of this object and related resources.
        /// </para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        
        /// <summary>
        /// <para>
        /// Disposes of the resources held by this class.
        /// </para>
        /// </summary>
        /// <param name="disposing">
        /// indicates whether the object is disposing (true) or in the destructor (false)
        /// </param>
        protected void Dispose(bool disposing)
        {
            if (_IsDisposed)
            {
                return;
            }
            if (disposing)
            {
                _IsDisposed = true;
            }
        }

        /// <summary>
        /// <para>Destructor for this class disposes recourse used.</para>
        /// </summary>
        ~HermesAuthorizationService()
        {
            Dispose(false);
        }

        /// <summary>
        /// <para>
        /// Validate value is <see cref="Rights"/>.
        /// </para>
        /// </summary>
        /// <param name="value">
        /// The <see cref="Rights"/> instance to be validated.
        /// </param>
        /// <param name="paramName">
        /// The actual parameter name of the argument being checked.
        /// </param>
        /// <exception cref="ArgumentException">
        /// If the value is not valid <see cref="Rights"/> instance.
        /// </exception>
        private static void CheckRights(Rights value, string paramName)
        {
            string[] names = Enum.GetNames(typeof(Rights));
            Rights values = Rights.Delete;
            foreach (string name in names)
            {
                values |= (Rights)Enum.Parse(typeof(Rights), name);
            }
            if (value < Rights.Read || value > values)
            {
                throw new ArgumentException(
                    string.Format("{0} is not valid Enum.", paramName),
                    paramName);
            }
        }
        
        /// <summary>
        /// Throw an instance of <see cref="SelfDocumentingException"/>.
        /// </summary>
        ///
        /// <param name="msg">
        /// The message for exception.
        /// </param>
        /// <param name="e">
        /// The cause.
        /// </param>
        /// <param name="methodName">
        /// The method name.
        /// </param>
        /// <param name="paramNames">
        /// The parameter names.
        /// </param>
        /// <param name="paramValues">
        /// The parameter values.
        /// </param>
        /// <param name="localNames">
        /// The local variable names.
        /// </param>
        /// <param name="localValues">
        /// The local variable values.
        /// </param>
        /// <param name="instanceNames">
        /// The instance variable names.
        /// </param>
        /// <param name="instanceValues">
        /// The instance variable values.
        /// </param>
        /// <returns>
        /// An instance of <see cref="SelfDocumentingException"/>.
        /// </returns>
        private static SelfDocumentingException PopulateSDE(string msg, Exception e, string methodName,
            string[] paramNames, object[] paramValues, string[] localNames, object[] localValues,
            string[] instanceNames, object[] instanceValues)
        {
            // Create SelfDocumentingException instance if necessary
            SelfDocumentingException sde;
            if (e is SelfDocumentingException)
            {
                sde = (SelfDocumentingException)e;
            }
            else
            {
                // Create SelfDocumentingException
                sde = new SelfDocumentingException(msg, e);
            }

            // pin method
            MethodState ms = sde.PinMethod(methodName, sde.StackTrace);

            // add method parameter
            for (int i = 0; i < paramNames.Length; i++)
            {
                ms.AddMethodParameter(paramNames[i], paramValues[i]);
            }

            // add local variable
            for (int i = 0; i < localNames.Length; i++)
            {
                ms.AddLocalVariable(localNames[i], localValues[i]);
            }

            // add instance variable
            for (int i = 0; i < instanceNames.Length; i++)
            {
                ms.AddInstanceVariable(instanceNames[i], instanceValues[i]);
            }

            // lock exception info
            ms.Lock();

            // return it
            return sde;
        }
    }
}
