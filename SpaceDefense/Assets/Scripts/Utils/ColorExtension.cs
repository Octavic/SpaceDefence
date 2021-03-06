﻿//  --------------------------------------------------------------------------------------------------------------------
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
        public static Color FromInt(int r, int g, int b, float a = 1)
        {
            float rf = r;
            float gf = g;
            float bf = b;

            return new Color(rf / 255, gf / 255, bf / 255, a);
        }
    }
}
