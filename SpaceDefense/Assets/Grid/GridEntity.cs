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
                return this._extrudeX;
            }
            protected set
            {
                this._extrudeX = value;
            }
        }
        private int _extrudeX =0;

        /// <summary>
        /// The width of the entity
        /// </summary>
        public virtual int ExtrudeY
        {
            get
            {
                return this._extrudeY;
            }
            protected set
            {
                this._extrudeY = value;
            }
        }
        private int _extrudeY = 0;

        /// <summary>
        /// Rotates the object
        /// </summary>
        public void RotateClockwise()
        {
            var newExtrudeX = this.ExtrudeY;
            var newExtrudeY = this.ExtrudeX * -1;
            this._extrudeX = newExtrudeX;
            this._extrudeY = newExtrudeY;
            this.transform.localEulerAngles += new Vector3(0, 0, -90);
        }
    }
}
