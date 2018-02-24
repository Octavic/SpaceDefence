//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Weapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Grid;
    using UnityEngine;

    /// <summary>
    /// Defines a base weapon class
    /// </summary>
    public abstract class Weapon : GridEntity, IReceiver
    {
        /// <summary>
        /// Cooldown for the weapon
        /// </summary>
        public float Cooldown;

        /// <summary>
        /// Amount of time left until the weapon can be fired again
        /// </summary>
        private float _cooldownLeft;

        /// <summary>
        /// Returns a state indicating whether or not this weapon is in the middle of cooling down after firing
        /// </summary>
        protected bool InCooldown
        {
            get
            {
                return this._cooldownLeft > 0;
            }
        }

        /// <summary>
        /// A collection of input sockets
        /// </summary>
        public List<InputSocket> InputSockets;
        public IList<InputSocket> Inputs
        {
            get
            {
                return this.InputSockets;
            }
        }

        public InputSocket GetInputSocket(int index)
        {
            return this.InputSockets[index];
        }
        public int IndexOf(InputSocket input)
        {
            return this.InputSockets.IndexOf(input);
        }

        /// <summary>
        /// Called when any of the input changes
        /// </summary>
        public abstract void OnInputChange();

        public override void OnMove()
        {
            Utils.Utils.UpdateAllBeams(this.Inputs);
        }

        /// <summary>
        /// Called when the weapon fires
        /// </summary>
        protected abstract void OnFire();

        /// <summary>
        /// Called when the cooldown of the weapon starts
        /// </summary>
        protected virtual void OnCooldownEnd()
        {
        }

        /// <summary>
        /// Apply the cooldown
        /// </summary>
        protected void ApplyCooldown()
        {
            this._cooldownLeft = this.Cooldown;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected virtual void Update()
        {
            if (this._cooldownLeft > 0)
            {
                this._cooldownLeft -= Time.deltaTime;
                if (this._cooldownLeft <= 0)
                {
                    this.OnCooldownEnd();
                }
            }
        }

        /// <summary>
        /// Called when initializing
        /// </summary>
        protected virtual void Start()
        {
            foreach (var input in this.Inputs)
            {
                input.Receiver = this;
            }
        }
    }
}
