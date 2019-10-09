// ILabel.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock ILabel interface.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public interface ILabel
    {
        /// <summary>
        /// Gets the text of the label
        /// </summary>
        /// <value>the text of the label</value>
        string Text
        {
            get;
        }

        /// <summary>
        /// Gets the minimal size of the label
        /// </summary>
        /// <value>the minimal size of the label</value>
        Dimension MinimalSize
        {
            get;
        }
    }
}
