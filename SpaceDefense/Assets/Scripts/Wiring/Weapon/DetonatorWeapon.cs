//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DetonatorWeapon.cs">
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
    /// Defines a weapon that fires a projectile that can be manually detonated
    /// </summary>
    public class DetonatorWeapon : ProjectileWeapon
    {
        /// <summary>
        /// A list of all fired projectiles
        /// </summary>
        private List<DetonateProjectile> _firedProjectile = new List<DetonateProjectile>();

        protected override GameObject OnFire()
        {
            var newShell = base.OnFire();
            this._firedProjectile.Add(newShell.transform.GetChild(0).GetComponent<DetonateProjectile>());
            return newShell;
        }

        private void DetonateAll()
        {
            var firedList = this._firedProjectile;
            this._firedProjectile = new List<DetonateProjectile>();
            for (int i = firedList.Count - 1; i >= 0; i--)
            {
                var fired = firedList[i];
                if (fired != null)
                {
                    fired.Detonate();
                    Destroy(fired.gameObject);
                }
            }
        }

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            if (this.Inputs[0].IsOn && !this.InCooldown)
            {
                this.OnFire();
            }

            if (this.Inputs[1].IsOn)
            {
                this.DetonateAll();
            }
        }
    }
}
