/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
using System.ServiceModel;
using TopCoder.Services.WCF;
using TopCoder.Util.ExceptionManager.SDE;
using System.Collections.Generic;
namespace Hermes.Services.Security.Authorization
{

    /// <summary>
    /// <para>
    /// This interface specifies the <see cref="ServiceContractAttribute"/>
    /// for Hermes authorization services.
    /// The operations specified by this contract will be executed by the
    /// service and accessed through a WCF client.
    /// </para>
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// Implementations of this interface enforce security at three levels.
    /// Security can be enforced By Role, By Function, and By Entity.
    /// </para>
    ///
    /// <para>
    /// Each method that checks security takes a session id and session token
    /// that uniquely identify  a valid session.
    /// Roles are the general groups of functionality that a user belongs to,
    /// such as Maintenance or IT.
    /// Functions are specific pieces of functionality that a user has rights
    /// to perform, such as Save User Data, or Load Schedule Data.
    /// Entities are the low-level permissions assigned to fields, objects and
    /// collections of data.
    /// </para>
    ///
    /// <para>
    /// Users may have Read, Insert, Update, Delete, and Execute permissions on
    /// individual entities.
    /// There are two ways to query for Entity permissions. The first method is
    /// to pass an enumeration  representing the specific rights requested in
    /// the <see cref="Rights"/> enumeration.
    /// If all of the requested rights are available to the user, then the
    /// result will be a bool with  a value of true.
    /// If any of the requested rights are not available to the user, then it
    /// return will be false.
    /// The second method to determine entity permissions is to request an
    /// enumeration that reflects the permissions for the specified entity.
    /// </para>
    ///
    /// <para>
    /// All exceptions caught in the service methods are wrapped in a
    /// <see cref="SelfDocumentingException "/> (if the exception caught isn't
    /// one already) and then published using the exceptionManager member
    /// variable. As this class is a WCF hosted service, it must throw
    /// <see cref="FaultException"/> (a system class) instances, created from
    /// <see cref="TCFaultException"/> instances from the WCF Base component.
    /// This class should use the <see cref="SelfDocumentingException"/> class
    /// to generate the information needed for the fault exception. When any
    /// exception is caught in this class, this class should create a
    /// <see cref="SelfDocumentingException"/> containing the message inform-
    /// ation from the exception caught, as well as state information about
    /// parameters to the method given and the state of the member variables in
    /// this class.  After the <see cref="SelfDocumentingException"/> has been
    /// created and "Pinned", a <see cref="TCFaultException"/> should be creat-
    /// ed, and then added to a <see cref="FaultException"/> and then thrown.
    /// </para>
    ///
    /// <para>
    /// Note that if the exception caught is a
    /// <see cref="SelfDocumentingException"/>, the methods in this class
    /// should pin their data to that exception, instead of creating a new one.
    /// </para>
    /// </remarks>
    ///
    /// <remarks>
    /// <p>
    /// <strong>Thread Safety:</strong>
    /// Services implementing this interface need to be used in a thread safe
    /// manner. If they are designed to handle requests per call or per session
    /// , thread safety isn't required; if they are designed use a single
    /// instance for all calls, thread safety is required.
    /// </p>
    /// </remarks>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [ServiceContract]
    public interface IAuthorization
    {
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
        /// a problem interacting with the wrapped authentication service).
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        bool CheckRole(string sessionId, string sessionToken, string roleName);

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
        /// a problem interacting with the wrapped authentication service).
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        bool CheckFunction(string sessionId, string sessionToken,
            string functionName);

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
        /// a problem interacting with the wrapped authentication service).
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        bool CheckEntity(string sessionId, string sessionToken,
            string entityName, Rights rights);

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
        /// a problem interacting with the wrapped authentication service).
        /// </exception>
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        Rights GetEntityRights(string sessionId, string sessionToken,
            string entityName);

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
        /// wrapped service FunctionalAttribute instance.
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
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        List<KeyValuePair<string, string>> GetFunctionAttributes(
            string sessionId, string sessionToken, string functionName);

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
        [OperationContract]
        [FaultContract(typeof(TCFaultException))]
        void SetApplication(string appId);
    }
}
