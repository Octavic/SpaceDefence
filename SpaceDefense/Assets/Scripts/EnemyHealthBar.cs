//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnemyHealthBar.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// The health bar floating over an enemy's head
    /// </summary>
    public class EnemyHealthBar : MonoBehaviour
    {
        /// <summary>
        /// The object of the actual bar
        /// </summary>
        public GameObject Bar;

        /// <summary>
        /// Called when the health bar needs to be updated
        /// </summary>
        /// <param name="targetEnemy">The enemy that this bar represents</param>
        public void UpdateHealth(Enemy targetEnemy)
        {
            var ratio = targetEnemy.CurrentHealth / targetEnemy.TotalHealth;
            Bar.transform.localScale = new Vector3(ratio, 1, 1);
            Bar.transform.localPosition = new Vector3(ratio / 2 - 0.5f, 0);
        }
    }
}
