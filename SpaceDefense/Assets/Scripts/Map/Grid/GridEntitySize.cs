//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntitySize.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the size of a grid entity
    /// </summary>
    [Serializable]
    public struct GridEntitySize
    {
        public int ExtrudeX;
        public int ExtrudeY;

        public GridEntitySize(int extrudeX, int extrudeY)
        {
            this.ExtrudeX = extrudeX;
            this.ExtrudeY = extrudeY;
        }

        /// <summary>
        /// Rotates the size
        /// </summary>
        /// <param name="isClockWise">If the rotation is clockwise. Defaults to true</param>
        /// <returns>The new rotated size</returns>
        public GridEntitySize Rotate(bool isClockWise = true)
        {
            return new GridEntitySize(isClockWise ? this.ExtrudeY : this.ExtrudeY * -1,
            isClockWise ? this.ExtrudeX * -1 : this.ExtrudeX);
        }
    }
}
