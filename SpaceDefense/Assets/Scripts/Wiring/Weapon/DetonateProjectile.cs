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
        /// The range of the blast
        /// </summary>
        public float BlastMaxRadius;

        /// <summary>
        /// The inner circle where the damage will be the highest. 
        /// Being hit outside of this radius will have damage fall off the further away the enemy is
        /// </summary>
        public float BlastMinRadius;

        /// <summary>
        /// How much damage will the weapon deal
        /// </summary>
        public float BlastDamage;

        private float _blastRadiusDiff;

        /// <summary>
        /// Detonates the projectile
        /// </summary>
        public void Detonate()
        {
            var enemies = SpawnManager.CurrntInstance.CurrentEnemies;
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                var targetEnemy = enemies[i];
                if (targetEnemy == null)
                {
                    continue;
                }

                var distance = (targetEnemy.transform.position - this.transform.position).magnitude;

                // Not hit
                if (distance > this.BlastMaxRadius)
                {
                    continue;
                }
                // Hit with outer radius
                if (distance > this.BlastMinRadius)
                {
                    targetEnemy.TakeDamage(this.BlastDamage * (distance - this.BlastMinRadius) / this._blastRadiusDiff);
                }
                // Perfect hit
                else
                {
                    targetEnemy.TakeDamage(this.BlastDamage, this.Shell.EffectImpacts);
                }
            }
        }

        /// <summary>
        /// Used  for initialization
        /// </summary>
        protected void Start()
        {
            this._blastRadiusDiff = this.BlastMaxRadius - this.BlastMinRadius;
        }
    }
}
