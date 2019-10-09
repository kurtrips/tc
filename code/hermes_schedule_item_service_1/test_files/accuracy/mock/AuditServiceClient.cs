/*
 * Copyright (c) 2007, TopCoder, Inc. All rights reserved
 */
using HermesNS.TC.Services.AuditTrail;
using System.Collections.Generic;

namespace HermesNS.TC.Services.ScheduleItem.Clients
{
    /// <summary>
    /// Mock.
    /// </summary>
    ///
    /// <author>zaixiang</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c)2007, TopCoder, Inc. All rights reserved.</copyright>
    public class AuditServiceClient
    {
        /// <summary>
        /// Mock.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Mock.
        /// </summary>
        public void Open()
        {
        }

        /// <summary>
        /// Mock.
        /// </summary>
        public void Abort()
        {
        }

        /// <summary>
        /// Mock.
        /// </summary>
        /// <param name="records">Mock.</param>
        public void SaveAuditRecords(IList<HermesAuditRecord> records)
        {
        }
    }
}
