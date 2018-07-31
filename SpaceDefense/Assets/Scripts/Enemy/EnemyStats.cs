//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnemyStats.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Enemy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines all enemy stats
    /// </summary>
    [Serializable]
    public struct EnemyStats
    {
        public float Health;
        public float Shield;
        public float Armomr;
    }
}
