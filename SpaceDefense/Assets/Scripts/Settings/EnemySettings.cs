﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnemySettings.cs">
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
    /// A collection of enemy setings
    /// </summary>
    public static class EnemySettings
    {
        /// <summary>
        /// How long after hit does shield start regenerating
        /// </summary>
        public const float ShieldRegenDelay = 1.0f;

        /// <summary>
        /// How many shield points is regained per second
        /// </summary>
        public const float ShieldRegenSpeed = 3.0f;
    }
}
