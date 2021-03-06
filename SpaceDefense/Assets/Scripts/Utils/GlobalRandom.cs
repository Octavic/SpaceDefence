﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GlobalRandom.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class GlobalRandom
    {
        public static Random random = new Random();
        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }
    }
}
