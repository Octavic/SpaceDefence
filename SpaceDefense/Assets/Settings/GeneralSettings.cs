//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GeneralSettings.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of general settings
    /// </summary>
    public static class GeneralSettings
    {
        /// <summary>
        /// The maximum amount of connection that can exist at the same time on an input/output socket
        /// </summary>
        public const int MaxConnectionPerSocket = 5;

        /// <summary>
        /// By default, transforms have a minimal of 500ms delay when triggered and emitting a new signal
        /// </summary>
        public const float TransformerDefaultTriggerDelay = 0.2f;

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

        /// <summary>
        /// The size  of the grid blocks
        /// </summary>
        public const float GridSize = 0.32f;
    }
}
