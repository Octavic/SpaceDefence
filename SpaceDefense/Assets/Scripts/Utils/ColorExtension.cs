//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ColorExtension.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Extends  the UnityEngine Color class
    /// </summary>
    public static class ColorExtension
    {
        public static Color Lerp(this Color c, Color target, float percent = 0.5f)
        {
            return new Color((c.r + target.r) * percent, (c.g + target.g) * percent, (c.b + target.b) * percent);
        }
    }
}
