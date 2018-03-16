﻿//  --------------------------------------------------------------------------------------------------------------------
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
        public const float EffectResistance = -2000.0f;

        /// <summary>
        /// How long any of the effects last
        /// </summary>
        public const float EffectDuration = 5.0f;

        /// <summary>
        /// How much poison will do per second
        /// </summary>
        public const float PoisonDamagePerSecond = 200.0f;

        /// <summary>
        /// The amount that effect resistance is gained per second
        /// </summary>
        public const float EffectBuildUpDecayPerSecond = -400.0f;
    }
}
