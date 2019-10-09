// Dimension.cs
// Copyright (c) 2007, TopCoder, Inc. All rights reserved.
using System;

namespace TopCoder.Graph.Layout
{
    /// <summary>
    /// A mock implementation of the Dimension class.
    /// </summary>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public class Dimension
    {
        /// <summary>
        /// The height part.
        /// </summary>
        private int height;

        /// <summary>
        /// The width part
        /// </summary>
        private int width;

        /// <summary>
        /// Gets or sets the height
        /// </summary>
        /// <value>The height</value>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Gets or sets the width
        /// </summary>
        /// <value>The width</value>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Creates a new Dimension
        /// </summary>
        public Dimension()
        {
        }

        /// <summary>
        /// Creates a new Dimension with given height and width.
        /// </summary>
        /// <param name="height">The height</param>
        /// <param name="width">The weight</param>
        public Dimension(int height, int width)
        {
            Height = height;
            Width = width;
        }
    }
}
