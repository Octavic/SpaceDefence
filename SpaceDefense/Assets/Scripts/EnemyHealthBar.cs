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
        /// The target enemy
        /// </summary>
        public Enemy TargetEnemy;

        /// <summary>
        /// The object of the actual bar
        /// </summary>
        public GameObject HealthBar;
        public GameObject ShieldBar;

        private static void _updateBar(GameObject bar, float newRatio)
        {
            var oldScale = bar.transform.localScale;
            bar.transform.localScale = new Vector3(newRatio, oldScale.y, oldScale.z);

            var oldPos = bar.transform.localPosition;
            bar.transform.localPosition = new Vector3(newRatio / 2 - 0.5f, oldPos.y, oldPos.z);
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            var curHealth = this.TargetEnemy.CurrentHealth;
            var curShield = this.TargetEnemy.CurrentShield;

            _updateBar(this.HealthBar, curHealth == 0 ? 0 : curHealth / this.TargetEnemy.TotalHealth);
            _updateBar(this.ShieldBar, curShield == 0 ? 0 : curShield / this.TargetEnemy.TotalShield);
        }
    }
}
