﻿//  --------------------------------------------------------------------------------------------------------------------
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
        public int ExtrudeX;

        /// <summary>
        /// The width of the entity
        /// </summary>
        public int ExtrudeY;

        /// <summary>
        /// Rotates the object
        /// </summary>
        public virtual void RotateClockwise()
        {
            var newExtrudeX = this.ExtrudeY;
            var newExtrudeY = this.ExtrudeX * -1;
            this.ExtrudeX = newExtrudeX;
            this.ExtrudeY = newExtrudeY;
            this.transform.localEulerAngles += new Vector3(0, 0, -90);
            this.Rotation++;
        }

        /// <summary>
        /// Called when the entity moves to update the attached beams
        /// </summary>
        public abstract void OnMove();
    }
}
