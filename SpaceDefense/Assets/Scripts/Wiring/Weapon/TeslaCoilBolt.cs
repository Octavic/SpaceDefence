//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TeslaCoilWeapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// The bolt between two tesla coils
    /// </summary>
    public class TeslaCoilBolt : AttachableBeam, IConstantHitbox
    {
        /// <summary>
        /// Called when the bolt hits an enemy
        /// </summary>
        /// <param name="hitEnemy">The enmy hit</param>
        public void OnHitEnemy(Enemy hitEnemy)
        {
            throw new NotImplementedException();
        }
    }
}
