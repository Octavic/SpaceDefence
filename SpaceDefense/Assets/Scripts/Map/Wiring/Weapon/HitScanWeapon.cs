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
    using Visuals;

    /// <summary>
    /// Defines a weapon that uses hit scan calculation
    /// </summary>
    public class HitScanWeapon : InstantFireWeapon
    {
        /// <summary>
        /// prefab for the bullet line visuals
        /// </summary>
        public HitScanBulletLine BulletLiniePrefab;

        /// <summary>
        /// Location of the gun muzzle
        /// </summary>
        public GameObject MuzzleLocation;

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
                var degAngleDiff = GlobalRandom.NextFloat()
                    * this.Inaccuracy
                    * (GlobalRandom.random.Next(2) == 0 ? 1 : -1);
                var fireAngleRad = (curFacing + degAngleDiff) * Mathf.Deg2Rad;
                var allHits = Physics2D.RaycastAll(
                        this.MuzzleLocation.transform.position,
                        new Vector2(Mathf.Cos(fireAngleRad), Mathf.Sin(fireAngleRad))
                    )
                    .Where(this.ProcessRaycastHit)
                    .ToList();
                if (allHits.Count > 0)
                {
                    var newLine = Instantiate(this.BulletLiniePrefab);
                    newLine.DrawHit(this.MuzzleLocation.transform.position, allHits);
                }
            }

            return null;
        }

        private bool ProcessRaycastHit(RaycastHit2D hit)
        {
            var hittalbe = hit.collider.GetComponent<IHittable>();
            if (hittalbe == null)
            {
                return false;
            }

            hittalbe.OnHit(this.Damage, this.EffectImpacts);
            return true;
        }
    }
}
