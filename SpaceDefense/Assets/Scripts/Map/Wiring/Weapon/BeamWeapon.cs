//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BeamWeapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines a beam weapon that fires a constant beam
    /// </summary>
    public class BeamWeapon : Weapon
    {
        /// <summary>
        /// The actual weapon beam
        /// </summary>
        public BeamWeaponObject Beam;

        /// <summary>
        /// The old fire state
        /// </summary>
        private bool _wasFiring = false;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            var newState = this.Inputs.Any(input => input.IsOn);

            if (newState && !this.InCooldown)
            {
                this.OnFire();
            }
            else
            {
                if (this._wasFiring)
                {
                    this.ApplyCooldown();
                    this.Beam.OnCeaseFire();
                }

                this._wasFiring = false;
            }
        }

        protected override void OnCooldownEnd()
        {
            this.OnInputChange();
        }

        /// <summary>
        /// Called when the weapon is fired
        /// </summary>
        protected override GameObject OnFire()
        {
            this._wasFiring = true;
            this.Beam.OnFire();
            return this.Beam.gameObject;
        }

        protected override void Update()
        {
            if (this._wasFiring)
            {
                GameController.CurrentInstance.AddCost(this.Cost * Time.deltaTime);
            }

            base.Update();
        }
    }
}
