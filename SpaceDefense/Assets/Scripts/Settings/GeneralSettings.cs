//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GeneralSettings.cs">
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
        /// The size  of the grid blocks
        /// </summary>
        public const float GridSize = 0.32f;

        /// <summary>
        /// How many steps we can step back to
        /// </summary>
        public const int GridUndoSteps = 10;

        /// <summary>
        /// How much x enemy worth the player will be punished if they let an enemy through
        /// </summary>
        public const float EnemySurvivalPenaltyMultiplier = 5;

        /// <summary>
        /// How many seconds the player must defend to complete a level
        /// </summary>
        public const float TotalDefendDuration = 60;
    }
}
