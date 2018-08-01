//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PluseBuffEnemy.cs">
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
    /// Defines an enemy that generates a pulse to  buff nearby enemies
    /// </summary>
    public class PluseBuffEnemy : Enemy
    {
        /// <summary>
        /// The buff effect
        /// </summary>
        public EffectEnum BuffEffect;

        /// <summary>
        /// Range of the effect
        /// </summary>
        public float EffectRange;

        /// <summary>
        /// The amount of time between each pulse
        /// </summary>
        public float EffectFrequency;
        private float _timeTillNextPulse;

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected override void Update()
        {
            if (this._timeTillNextPulse > 0)
            {
                this._timeTillNextPulse -= Time.deltaTime;
            }
            else
            {
                this._timeTillNextPulse = this.EffectFrequency;
                var curPos = this.transform.position;
                var range = this.EffectRange;
                var enemiesInRange =
                    SpawnManager
                        .CurrntInstance
                        .CurrentEnemies
                        .Where(enemy => (enemy.transform.position - curPos).magnitude < range);

                foreach(var enemy in enemiesInRange)
                {
                    enemy.ApplyEffect(this.BuffEffect);
                }
            }

            base.Update();
        }
    }
}
