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
                while (value < 0)
                {
                    value += 4;
                }

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
        public GridEntitySize Size;

        /// <summary>
        /// Rotates the object
        /// </summary>
        public virtual void Rotate(bool isClockWise)
        {
            this.Size = this.Size.Rotate(isClockWise);
            this.transform.localEulerAngles += new Vector3(0, 0, isClockWise ? -90 : 90);
            if (isClockWise)
            {
                this.Rotation++;
            }
            else
            {
                this.Rotation--;
            }
            this.OnMove();
        }

        /// <summary>
        /// Called when the entity moves to update the attached beams
        /// </summary>
        public abstract void OnMove();
    }
}
