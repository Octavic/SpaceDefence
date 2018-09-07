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
        /// the effect that's carried
        /// </summary>
        public List<EffectEnum> Effects;
        public List<float> Impacts;

        /// <summary>
        /// Damage of the weapon
        /// </summary>
        public float Damage;

        /// <summary>
        /// The composed effects dictionary
        /// </summary>
        public Dictionary<EffectEnum, float> EffectImpacts { get; private set; }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected override void Start()
        {
            this.EffectImpacts = Utils.ConvertListToDictionary(this.Effects, this.Impacts);
            base.Start();
        }

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
                var hittables = Physics.RaycastAll(
                        this.transform.position,
                        new Vector2(Mathf.Cos(fireAngleRad), Mathf.Sin(fireAngleRad))
                    )
                    .Select(hit => hit.collider.GetComponent<IHittable>())
                    .ToList();
                foreach (var hittalbe in hittables)
                {
                    if (hittalbe == null)
                    {
                        continue;
                    }

                    hittalbe.OnHit(this.Damage, this.EffectImpacts);
                }
            }
            return null;
        }
    }
}
