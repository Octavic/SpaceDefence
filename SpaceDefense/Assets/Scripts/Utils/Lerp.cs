//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Lerp.cs">
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

    public static class Lerp
    {
        public static float LerpFloat(float f1, float f2, float percent)
        {
            return f1 + (f2 - f1) * percent;
        }

        public static Color LerpColor(Color c, Color target, float percent = 0.5f)
        {
            return new Color(
                LerpFloat(c.r, target.r, percent),
                LerpFloat(c.g, target.g, percent),
                LerpFloat(c.b, target.b, percent)
            );
        }
    }
}
