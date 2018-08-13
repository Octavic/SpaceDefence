//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="CloakerEnemy.cs">
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
    /// Defines an enemy that cloaks 
    /// </summary>
    public class CloakerEnemy : Enemy
    {
        public override void TakeDamage(float damage, IDictionary<EffectEnum, float> carriedEffects = null)
        {
            var maxHP = this.CurrentStats.Health;
            var curHp = this.HealthRemaining;
            if (curHp < maxHP * Settings.EnemySettings.CloakTriggerThreshold)
            {
                this.ApplyEffect(EffectEnum.Cloaked, 120);
            }
            base.TakeDamage(damage, carriedEffects);
        }
    }
}
