// IPort.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock IPort interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface IPort
    {
        /// <summary>
        /// The node of the port
        /// </summary>
        /// <value>The node of the port</value>
        INode Node
        {
            get;
        }

        /// <summary>
        /// The minimal size of the port
        /// </summary>
        /// <value>The minimal size of the port</value>
        Dimension MinimalSize
        {
            get;
        }

        /// <summary>
        /// The links of the port
        /// </summary>
        /// <value>The links of the port</value>
        IList<ILink> Links
        {
            get;
        }

        /// <summary>
        /// The id
        /// </summary>
        long Id
        {
            get;
        }
    }
}
