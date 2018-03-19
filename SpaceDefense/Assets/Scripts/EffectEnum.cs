﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EffectEnum.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of all possible effects
    /// </summary>
    public enum EffectEnum
    {
        /// <summary>
        /// No effect
        /// </summary>
        None,
        /// <summary>
        /// Movement speed reduced by 50%
        /// </summary>
        Slowed,
        /// <summary>
        /// Unable to move at all
        /// </summary>
        Frozen,
        /// <summary>
        /// Damage over time that negates shield
        /// </summary>
        Poisoned,
        /// <summary>
        /// Enemy takes extra damage
        /// </summary>
        Vulnerable,
        /// <summary>
        /// Enemy explodes upon death
        /// </summary>
        Ignited,
        /// <summary>
        /// Enemy cannot be hit
        /// </summary>
        Cloaked
    }
}
