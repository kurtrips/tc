// MapAttribute.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// A mock implementation of the MapHeader class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapHeader
    {
        /// <summary>
        /// The lifecycle of the header.
        /// </summary>
        private string lifecycle = null;

        /// <summary>
        /// Sets or gets the lifecycle of the header.
        /// </summary>
        /// <value>the lifecycle of the header</value>
        public string Lifecycle
        {
            get
            {
                return lifecycle;
            }
            set
            {
                lifecycle = value;
            }
        }

        /// <summary>
        /// Creates MapHeader instance.
        /// </summary>
        public MapHeader()
        {
        }
    }
}
