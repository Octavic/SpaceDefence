//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BeamWeapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring.Weapon
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
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            var newState = this.Inputs.Any(input => input.IsOn);
            if (newState)
            {
                this.Beam.OnFire();
            }
            else
            {
                this.Beam.OnCeaseFire();
            }
        }

        protected override void OnFire()
        {
            this.Beam.OnFire();
        }
    }
}
