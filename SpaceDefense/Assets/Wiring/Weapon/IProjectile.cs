//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameController.cs">
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
    public interface IProjectile
    {
        /// <summary>
        /// Called when the projectile hits an enemy
        /// </summary>
        /// <param name="hitEnemy">The enmy that was hit</param>
        void OnHittingEnemy(Enemy hitEnemy);
    }
}
