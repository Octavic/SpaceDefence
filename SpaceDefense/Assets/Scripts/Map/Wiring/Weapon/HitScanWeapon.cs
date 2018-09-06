//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HitScanWeapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Utils;

    /// <summary>
    /// Defines a weapon that uses hit scan calculation
    /// </summary>
    public class HitScanWeapon : InstantFireWeapon
    {
        /// <summary>
        /// How many lines to fire at the same time
        /// </summary>
        public int FireCount;

        /// <summary>
        /// A number from 0-90 (degrees) to show how inaccurate the spread can be
        /// </summary>
        public float Inaccuracy;

        /// <summary>
        /// Called when the weapon is fired
        /// </summary>
        /// <returns>nothing in this case</returns>
        protected override GameObject OnFire()
        {
            var curFacing = this.transform.eulerAngles.z;
            for (int i = 0; i < this.FireCount; i++)
            {
                var degAngleDiff = GlobalRandom.random.NextDouble()
                    * this.Inaccuracy
                    * GlobalRandom.random.Next(2) == 0 ? 1 : -1;
                var fireAngleRad = (curFacing + degAngleDiff) * Mathf.Rad2Deg;
                var cast = Physics2D.RaycastAll(this.transform.position, new Vector2(Mathf.Cos(fireAngleRad), Mathf.Sin(fireAngleRad)));
            }
            return null;
        }
    }
}
