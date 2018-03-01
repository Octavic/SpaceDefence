//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ProjectileWeapon.cs">
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
    /// Defines a projectile weapon
    /// </summary>
    public class ProjectileWeapon : Weapon
    {
        /// <summary>
        /// The projectile that will be fired when the weapon fires
        /// </summary>
        public GameObject ProjectileShellPrefab;

        /// <summary>
        /// Mode of fire
        /// </summary>
        public ProjectileWeaponFireMode FireMode;

        /// <summary>
        /// Called when the weapon is fired
        /// </summary>
        protected override void OnFire()
        {
            var newshell = Instantiate(this.ProjectileShellPrefab);
            newshell.transform.eulerAngles = this.transform.eulerAngles;
            newshell.transform.position = this.transform.position;
            GameController.CurrentInstance.AddCost(this.Cost);
            this.ApplyCooldown();
        }

        /// <summary>
        /// Called when there was a change in input
        /// </summary>
        public override void OnInputChange()
        {
            var fired = this.Inputs.Any(input => input.IsOn);
            if (fired && !this.InCooldown)
            {
                this.OnFire();
            }
        }
    }
}
