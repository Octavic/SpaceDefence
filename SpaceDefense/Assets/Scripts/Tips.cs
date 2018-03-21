//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Tips.cs">
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
    /// A collection of tips
    /// </summary>
    public static class Tips
    {
        public static List<String> Elemental = new List<string>()
        {
            "Poison damage bypasses shields, making them effective against heavily shielded enemies",
            "Ignited enemies will damage everyone around them when they die",
            "Cloaked enemies can pass through detectors without triggering them!",
            "Vulnerable enemies take twice as much damage"
        };
    }
}
