// ILink.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock ILink interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ILink
    {
        long Id
        {
            get;
        }

        IList<INode> Nodes
        {
            get;
        }

        IList<IPort> Ports
        {
            get;
        }
    }
}
