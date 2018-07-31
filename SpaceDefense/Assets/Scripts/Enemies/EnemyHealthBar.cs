//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnemyHealthBar.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Enemies
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

        /// <summary>
        /// Updates  the given bar with the given ratio
        /// </summary>
        /// <param name="bar">Target bar to update</param>
        /// <param name="newRatio">The new ratio to set the bar to</param>
        private static void _updateBar(GameObject bar, float newRatio)
        {
            newRatio = Mathf.Clamp(newRatio, 0, 1);

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
            _updateBar(this.HealthBar, this.TargetEnemy.CurrentHealth / this.TargetEnemy.BaseHealth);

            var totalShield = this.TargetEnemy.BaseShield;
            if (totalShield > 0)
            {
                this.ShieldBar.SetActive(true);
                _updateBar(this.ShieldBar, this.TargetEnemy.CurrentShield / totalShield);
            }
            else
            {
                this.ShieldBar.SetActive(false);
            }
        }
    }
}
