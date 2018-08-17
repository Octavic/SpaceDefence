//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Projectile.cs">
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
    using Map.Enemies;

    /// <summary>
    /// The projectile fired
    /// </summary>
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// The shell that fired  this projectile
        /// </summary>
        public ProjectileShell Shell;

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            this.transform.localPosition += new Vector3(this.Shell.Velocity * Time.deltaTime, 0);
        }

        /// <summary>
        /// Called when hits an enemy
        /// </summary>
        /// <param name="hitEnemy">Enemy hit</param>
        public void OnHittingEnemy(Enemy hitEnemy)
        {
            this.Shell.OnHittingEnemy(hitEnemy);
        }
    }
}
