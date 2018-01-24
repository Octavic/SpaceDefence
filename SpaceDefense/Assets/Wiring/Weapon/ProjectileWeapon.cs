//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ProjectileWeapon.cs">
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
    /// Defines a projectile weapon
    /// </summary>
    public abstract class ProjectileWeapon : Weapon
    {
        /// <summary>
        /// The projectile that will be fired when the weapon fires
        /// </summary>
        public Projectile FiredProjectile;

        /// <summary>
        /// Mode of fire
        /// </summary>
        public ProjectileWeaponFireMode FireMode;
    }
}
