//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntity.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines an entity on the grid
    /// </summary>
    public class GridEntity : MonoBehaviour
    {
        /// <summary>
        /// The width of the entity
        /// </summary>
        public virtual int ExtrudeX
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// The width of the entity
        /// </summary>
        public virtual int ExtrudeY
        {
            get
            {
                return 1;
            }
        }
    }
}
