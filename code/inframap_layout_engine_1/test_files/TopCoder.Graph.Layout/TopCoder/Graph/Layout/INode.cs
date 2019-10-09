// INode.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using System.Collections.Generic;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock INode interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface INode
    {
        /// <summary>
        /// The Container of the node.
        /// </summary>
        /// <value>The Container of the node.</value>
        INode Container
        {
            get;
        }

        /// <summary>
        /// The Id of the node.
        /// </summary>
        /// <value>The Id of the node.</value>
        long Id
        {
            get;
        }

        /// <summary>
        /// The Ports of the node.
        /// </summary>
        /// <value>The Ports of the node.</value>
        IList<IPort> Ports
        {
            get;
        }

        /// <summary>
        /// The Label of the node.
        /// </summary>
        /// <value>The Label of the node.</value>
        ILabel Label
        {
            get;
        }

        /// <summary>
        /// The MinimalSize of the node.
        /// </summary>
        /// <value>The MinimalSize of the node.</value>
        Dimension MinimalSize
        {
            get;
        }
    }
}
