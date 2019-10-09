// MapLabel.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;
using TopCoder.Graph.Layout;

namespace Astraea.Inframap.Data
{
    /// <summary>
    /// A mock implementation of the MapLable class for use in testing.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class MapLabel : ILabel
    {
        /// <summary>
        /// The text of the label.
        /// </summary>
        private string text = null;

        /// <summary>
        /// The minimal size of the label.
        /// </summary>
        private Dimension minimalSize = null;

        /// <summary>
        /// Creates a new MapLabel
        /// </summary>
        public MapLabel()
        {
        }

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        /// <value>The text of the label.</value>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// Gets or sets the minimal size of the label.
        /// </summary>
        /// <value>The minimal size of the label.</value>
        public Dimension MinimalSize
        {
            get { return minimalSize; }
            set { minimalSize = value; }
        }
    }
}
