//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IRotatable.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines something that can be rotated
    /// </summary>
    public interface IRotatable
    {
        /// <summary>
        /// The current rotation
        /// </summary>
        float CurrentRotation { get; set; }

        /// <summary>
        /// Called when the item is rotated
        /// </summary>
        /// <param name="diff">How much to rotate the item by</param>
        void OnRotate(float diff);
    }
}
