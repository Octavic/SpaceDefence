//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Grid2DPlatformMover.cs">
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
    using Settings;

    /// <summary>
    /// Defines a platform mover
    /// </summary>
    public class Grid2DPlatformMover : GridEntityContainer
    {
        /// <summary>
        /// The platform itself
        /// </summary>
        public GridPlatform Platform;

        /// <summary>
        /// How fast the platform moves
        /// </summary>
        public float MoveSpeed;

        /// <summary>
        /// in  which direction is the platform omving
        /// </summary>
        private float _moveDirection;

        /// <summary>
        /// The minimal x position of the platform
        /// </summary>
        private float _minX;

        /// <summary>
        /// The maximal x position of the platform
        /// </summary>
        private float _maxX;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            var leftOn = this.InputSockets[0].IsOn;
            var rightOn = this.InputSockets[1].IsOn;

            if (leftOn == rightOn)
            {
                this._moveDirection = 0;
            }
            else if (leftOn)
            {
                this._moveDirection = -1;
            }
            else if (rightOn)
            {
                this._moveDirection = 1;
            }
        }

        /// <summary>
        /// Try to add an entity
        /// </summary>
        /// <param name="newEntity"></param>
        /// <returns></returns>
        public override bool TryAddEntity(GridEntity newEntity)
        {
            if (!this.CanAddEntity(newEntity))
            {
                return false;
            }

            this.CurrentlyHolding = newEntity;
            newEntity.transform.parent = this.Platform.transform;
            newEntity.transform.localPosition = Vector3.zero;
            return true;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected override void Start()
        {
            this._minX = GeneralSettings.GridSize;
            this._maxX = GeneralSettings.GridSize * (this.ExtrudeX - 1);

            base.Start();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected override void Update()
        {
            if (this._moveDirection != 0)
            {
                var newX = this.Platform.transform.localPosition.x + this._moveDirection * this.MoveSpeed * Time.deltaTime;
                newX = Math.Max(newX, this._minX);
                newX = Math.Min(newX, this._maxX);
                this.Platform.transform.localPosition = new Vector3(newX, 0);
                this.CurrentlyHolding.OnMove();
            }

            base.Update();
        }
    }
}
