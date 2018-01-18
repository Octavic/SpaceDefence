//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridCoordinate.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines a grid coordinate
    /// </summary>
    public class GridCoordinate
    {
        /// <summary>
        /// Gets or sets the x coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="GridCoordinate"/> class
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        public GridCoordinate(int x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }

        public static GridCoordinate operator +(GridCoordinate c1, GridCoordinate c2)
        {
            return new GridCoordinate(c1.X + c2.X, c1.Y + c2.Y);
        }
        public static GridCoordinate operator -(GridCoordinate c1, GridCoordinate c2)
        {
            return new GridCoordinate(c1.X - c2.X, c1.Y - c2.Y);
        }
    }
}
