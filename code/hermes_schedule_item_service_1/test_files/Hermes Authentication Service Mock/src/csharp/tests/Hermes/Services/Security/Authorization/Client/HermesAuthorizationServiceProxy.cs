/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Diagnostics;
using System.ServiceModel.Channels;
namespace Hermes.Services.Security.Authorization.Client
{
    /// <summary>
    /// <para>
    /// Class generated from svcUtil.exe
    /// </para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [ServiceContractAttribute(ConfigurationName = "IAuthorizationGenerated")]
    [CoverageExclude]
    public interface IAuthorizationGenerated
    {
        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="roleName">Role name.</param>
        /// <returns>Check result.</returns>
        [OperationContractAttribute(Action =
            "http://tempuri.org/IAuthorization/CheckRole",
            ReplyAction =
            "http://tempuri.org/IAuthorization/CheckRoleResponse")]
        bool CheckRole(string sessionId, string sessionToken, string roleName);

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="functionName">function name.</param>
        /// <returns>Check result.</returns>
        [OperationContractAttribute(Action =
            "http://tempuri.org/IAuthorization/CheckFunction",
            ReplyAction =
            "http://tempuri.org/IAuthorization/CheckFunctionResponse")]
        bool CheckFunction(string sessionId, string sessionToken,
            string functionName);

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="entityName">entity name.</param>
        /// <param name="rights">
        /// user right.
        /// </param>
        /// <returns>Check result.</returns>
        [OperationContractAttribute(Action =
            "http://tempuri.org/IAuthorization/CheckEntity",
            ReplyAction =
            "http://tempuri.org/IAuthorization/CheckEntityResponse")]
        bool CheckEntity(string sessionId, string sessionToken,
            string entityName, Rights rights);

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="entityName">entity name.</param>
        /// <returns>user right.</returns>
        [OperationContractAttribute(Action =
            "http://tempuri.org/IAuthorization/GetEntityRights",
            ReplyAction =
            "http://tempuri.org/IAuthorization/GetEntityRightsResponse")]
        Rights GetEntityRights(string sessionId, string sessionToken,
            string entityName);
    }

    /// <summary>
    /// <para>
    /// Class generated from svcUtil.exe
    /// </para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [CoverageExclude]
    public interface IAuthorizationGeneratedChannel : IAuthorizationGenerated,
        IClientChannel
    {
    }

    /// <summary>
    /// <para>
    /// Class generated from svcUtil.exe
    /// </para>
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [DebuggerStepThroughAttribute()]
    [GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [CoverageExclude]
    public partial class AuthorizationClient
        : ClientBase<IAuthorizationGenerated>, IAuthorizationGenerated
    {

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        public AuthorizationClient()
        {
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        public AuthorizationClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        public AuthorizationClient(string endpointConfigurationName,
            string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        public AuthorizationClient(string endpointConfigurationName,
            EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        public AuthorizationClient(Binding binding,
            EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="roleName">Role name.</param>
        /// <returns>Check result.</returns>
        public bool CheckRole(string sessionId, string sessionToken,
            string roleName)
        {
            return base.Channel.CheckRole(sessionId, sessionToken,
                roleName);
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="functionName">function name.</param>
        /// <returns>Check result.</returns>
        public bool CheckFunction(string sessionId, string sessionToken,
            string functionName)
        {
            return base.Channel.CheckFunction(sessionId, sessionToken,
                functionName);
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="entityName">entity name.</param>
        /// <param name="rights">
        /// user right.
        /// </param>
        /// <returns>Check result.</returns>
        public bool CheckEntity(string sessionId, string sessionToken,
            string entityName, Rights rights)
        {
            return base.Channel.CheckEntity(sessionId, sessionToken,
                entityName, rights);
        }

        /// <summary>
        /// <para>
        /// Method Generated from svcUtil.
        /// </para>
        /// </summary>
        /// <param name="sessionId">Session id</param>
        /// <param name="sessionToken">
        /// Session token
        /// </param>
        /// <param name="entityName">entity name.</param>
        /// <returns>user right.</returns>
        public Rights GetEntityRights(string sessionId, string sessionToken,
            string entityName)
        {
            return base.Channel.GetEntityRights(sessionId, sessionToken,
                entityName);
        }
    }
}
