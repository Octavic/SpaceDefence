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

    /// <summary>
    /// Defines a weapon that fires a projectile that can be manually detonated
    /// </summary>
    public class DetonatorWeapon : ProjectileWeapon
    {
        /// <summary>
        /// A list of all fired projectiles
        /// </summary>
        private List<DetonateProjectile> _firedProjectile;
    }
}
