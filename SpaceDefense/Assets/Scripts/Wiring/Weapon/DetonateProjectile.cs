//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="DetonateProjectile.cs">
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
    /// Defines a detonable projectile
    /// </summary>
    public class DetonateProjectile : Projectile
    {
        /// <summary>
        /// The collider component
        /// </summary>
        private Collider2D _collider;

        /// <summary>
        /// Detonates the projectile
        /// </summary>
        public void Detonate()
        {
        }
    }
}
