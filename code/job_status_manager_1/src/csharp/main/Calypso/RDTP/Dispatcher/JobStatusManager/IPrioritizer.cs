/*
 * Copyright (C) 2007 TopCoder Inc., All Rights Reserved.
 */

using System;
using System.Collections.Generic;
using Calypso.RDTP.Entity.Job;

namespace Calypso.RDTP.Dispatcher.JobStatusManager
{
    /// <summary>
    /// This interface declares contract for any classes implemented <see cref="ITradeActivityItem"/> comparers.
    /// This interface is simply another name of IComparer&lt;<see cref="ITradeActivityItem"/>&gt;.
    /// </summary>
    /// <remarks>The implementation of this interface must be thread safe.</remarks>
    /// <author>dfn</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IPrioritizer : IComparer<ITradeActivityItem>
    {
    }
}
