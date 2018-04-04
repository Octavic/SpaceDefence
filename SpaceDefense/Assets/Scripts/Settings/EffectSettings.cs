//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EffectSettings.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of effect and status related settings
    /// </summary>
    public static class EffectSettings
    {
        /// <summary>
        /// The counter weight that must be cleared before an enemy is afflicted by a certain effect
        /// </summary>
        public const float EffectProcLimit = 100.0f;

        /// <summary>
        /// How long any of the effects last
        /// </summary>
        public const float EffectDuration = 5.0f;

        /// <summary>
        /// How much poison will do per second
        /// </summary>
        public const float PoisonDamagePerSecond = 20.0f;

        /// <summary>
        /// How much the movement will slow
        /// </summary>
        public const float SlowSpeedMultiplier = 0.5f;

        /// <summary>
        /// The amount that effect resistance is gained per second
        /// </summary>
        public const float EffectBuildUpDecayPerSecond = 40.0f;

        /// <summary>
        /// How much more damage the shield takes when zapped
        /// </summary>
        public const float ZappedShieldDamageMultiplier = 3.0f;

        /// <summary>
        /// The multiplier applied to shield regen when the enmy is zapped
        /// </summary>
        public const float ZappedShieldRegenMultiplier = 0.5f;

        /// <summary>
        /// How much more health the enemy takes when vulnerable
        /// </summary>
        public const float VulnerableHealthDamageMultiplier = 2.0f;
    }
}
