//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ProjectileWeapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines a projectile weapon
    /// </summary>
    public class ProjectileWeapon : InstantFireWeapon
    {
        /// <summary>
        /// The projectile that will be fired when the weapon fires
        /// </summary>
        public GameObject ProjectileShellPrefab;

        /// <summary>
        /// Called when the weapon is fired
        /// </summary>
        protected override GameObject OnFire()
        {
            var newshell = Instantiate(this.ProjectileShellPrefab);
            newshell.transform.eulerAngles = this.transform.eulerAngles;
            newshell.transform.position = this.transform.position;
            return newshell;
        }
    }
}
