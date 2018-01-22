//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Projectile.cs">
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
    /// Defines a projectile
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// the effect that's carried
        /// </summary>
        public EffectEnum CarriedEffect;

        /// <summary>
        /// Gets or sets the damage for the projectile
        /// </summary>
        public float Damage;

        /// <summary>
        /// Called when the projectile hits an enemy
        /// </summary>
        /// <param name="hitEnemy">The enemy that was hit</param>
        public void OnHittingEnemy(Enemy hitEnemy)
        {
            hitEnemy.TakeDamage(this.Damage, this.CarriedEffect);
        }
    }
}
