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
        public const float TransformerDefaultTriggerDelay = 0.5f;
    }
}
