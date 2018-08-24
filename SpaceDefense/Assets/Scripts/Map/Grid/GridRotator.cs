//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridRotator.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;

    /// <summary>
    /// Describes a rotator that can rotate the entity that it contains
    /// </summary>
    public class GridRotator : GridEntityContainer
    {
        /// <summary>
        /// The thing that actually rotates
        /// </summary>
        public GameObject RotatorBase;

        /// <summary>
        /// How many degrees the rotator can turn per second
        /// </summary>
        public float TurnSpeed;

        /// <summary>
        /// Where to turn
        /// </summary>
        private int _turnDirection = 0;

        /// <summary>
        /// Try to add a new entity
        /// </summary>
        /// <param name="newEntity">New entity to be added</param>
        /// <returns>True if operation succeed</returns>
        public override bool TryAddEntity(GridEntity newEntity)
        {
            if (!this.CanAddEntity(newEntity))
            {
                return false;
            }

            this.CurrentlyHolding = newEntity;
            newEntity.transform.parent = this.RotatorBase.transform;
            newEntity.transform.localPosition = Vector3.zero;
            return true;
        }

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            var leftOn = this.InputSockets[0].IsOn;
            var rightOn = this.InputSockets[1].IsOn;

            if (leftOn == rightOn)
            {
                this._turnDirection = 0;
            }
            else if (leftOn)
            {
                this._turnDirection = -1;
            }
            else if (rightOn)
            {
                this._turnDirection = 1;
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected override void Update()
        {
            if (this._turnDirection != 0)
            {
                this.RotatorBase.transform.localEulerAngles += new Vector3(0, 0, this.TurnSpeed * this._turnDirection * Time.deltaTime);
                this.CurrentlyHolding.OnMove();
            }

            base.Update();
        }
    }
}
