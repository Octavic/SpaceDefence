//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnemyStats.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Enemies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Settings;

    /// <summary>
    /// Defines all enemy stats
    /// </summary>
    [Serializable]
    public struct EnemyStats
    {
        /// <summary>
        /// All stats
        /// </summary>
        public float Health;
        public float Shield;
        public float Armor;
        public float Speed;

        /// <summary>
        /// Normal constructor
        /// </summary>
        /// <param name="health">Desired health base/diff</param>
        /// <param name="shield">Desired shield base/diff</param>
        /// <param name="armor">Desired armor base/diff</param>
        /// <param name="speed">Desired speed base/diff</param>
        public EnemyStats(float health, float shield, float armor, float speed)
        {
            this.Health = health;
            this.Shield = shield;
            this.Armor = armor;
            this.Speed = speed;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="stats">The copy source</param>
        public EnemyStats(EnemyStats stats)
        {
            this.Health = stats.Health;
            this.Shield = stats.Shield;
            this.Armor = stats.Armor;
            this.Speed = stats.Speed;
        }

        /// <summary>
        /// applies the diff
        /// </summary>
        /// <param name="stats1">Base stats</param>
        /// <param name="stats2">Diff stats</param>
        /// <returns>Resulting stats</returns>
        public static EnemyStats operator +(EnemyStats stats1, EnemyStats stats2)
        {
            return new EnemyStats(
                stats1.Health + stats2.Health,
                stats1.Shield + stats2.Shield,
                stats1.Armor + stats2.Armor,
                stats1.Speed + stats2.Speed)
            ;
        }

        /// <summary>
        /// Applies the effect
        /// </summary>
        /// <param name="effect">Target effect</param>
        /// <returns>A new modified stats</returns>
        public void ApplyEffect(EffectEnum effect)
        {
            switch (effect)
            {
                case EffectEnum.Frozen:
                    this.Speed = 0;
                    break;
                case EffectEnum.Shielded:
                    this.Shield += EffectSettings.ShieldedAmount;
                    break;
                case EffectEnum.Slowed:
                    this.Speed *= EffectSettings.SlowSpeedMultiplier;
                    break;
            }
        }
    }
}
