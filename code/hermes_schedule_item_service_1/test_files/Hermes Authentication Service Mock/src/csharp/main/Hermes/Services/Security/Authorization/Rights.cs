/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved.
 */
using System;
namespace Hermes.Services.Security.Authorization
{

    /// <summary>
    /// <para>
    /// This is an enumeration of the rights that a user can have to an entity.
    /// Users may have <see cref="Read"/>, <see cref="Insert"/>,
    /// <see cref="Update"/>, <see cref="Delete"/>, and <see cref="Execute"/>
    /// permissions on individual entities.
    /// </para>
    ///
    /// </summary>
    /// <remarks>
    /// <p>
    /// The 'or' operator may be used to combine enumeration members into one
    /// and the 'and' operator can be used to test for common members between
    /// different enumerations.
    /// </p>
    /// </remarks>
    ///
    /// <remarks>
    /// <p>
    /// <strong>Thread Safety:</strong>
    /// Enumerations are thread safe.
    /// </p>
    /// </remarks>
    /// <author>TCSDESIGNER</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>
    /// Copyright (c)2007, TopCoder, Inc. All rights reserved.
    /// </copyright>
    [Flags]
    public enum Rights
    {
        /// <summary>
        /// <para>
        /// Flag indicating right to Read an entity.
        /// </para>
        /// </summary>
        Read = 1,

        /// <summary>
        /// <para>
        /// Flag indicating right to Insert an entity.
        /// </para>
        /// </summary>
        Insert = 2,

        /// <summary>
        /// <para>
        /// Flag indicating right to Update an entity.
        /// </para>
        /// </summary>
        Update = 4,

        /// <summary>
        /// <para>
        /// Flag indicating right to Delete an entity.
        /// </para>
        /// </summary>
        Delete = 8,

        /// <summary>
        /// <para>
        /// Flag indicating right to Execute an entity.
        /// </para>
        /// </summary>
        Execute = 16
    }
}
