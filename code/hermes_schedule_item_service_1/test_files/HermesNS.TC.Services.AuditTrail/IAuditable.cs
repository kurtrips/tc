// IAuditable<TParent>.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace HermesNS.TC.Services.AuditTrail
{
    /// <summary>A mock implementation of the IAuditable interface</summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IAuditable<TParent>
    {
        /// <summary>
        /// The method for auditing.
        /// </summary>
        /// <param name="old">The old entity</param>
        /// <returns>A list of HermesAuditRecord</returns>
        IList<HermesAuditRecord> Audit(TParent old);
    }
}
