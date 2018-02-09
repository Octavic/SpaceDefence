//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntity.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;

    /// <summary>
    /// Defines an entity on the grid
    /// </summary>
    public abstract class GridEntity : MonoBehaviour
    {
        /// <summary>
        /// Gets the unique ID for the grid entity
        /// </summary>
        public int ID;

        /// <summary>
        /// how many times this entity was rotated
        /// </summary>
        public int Rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                this._rotation = value % 4;
            }
        }

        /// <summary>
        /// The rotation of the GridEntity
        /// </summary>
        private int _rotation;

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
        public virtual void RotateClockwise()
        {
            var newExtrudeX = this.ExtrudeY;
            var newExtrudeY = this.ExtrudeX * -1;
            this._extrudeX = newExtrudeX;
            this._extrudeY = newExtrudeY;
            this.transform.localEulerAngles += new Vector3(0, 0, -90);
            this.Rotation++;
        }
    }
}
