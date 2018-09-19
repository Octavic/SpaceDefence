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
        /// If the shot pierces through enemies
        /// </summary>
        public bool DoesPenetrate;

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
            var muzzlePos = this.MuzzleLocation.transform.position;
            // Fire a bunch
            for (int i = 0; i < this.FireCount; i++)
            {
                // Gets the angle diff based on random inaccuracy
                var degAngleDiff = GlobalRandom.NextFloat()
                    * this.Inaccuracy
                    * (GlobalRandom.random.Next(2) == 0 ? 1 : -1);

                // Where to fire
                var fireAngleRad = (curFacing + degAngleDiff) * Mathf.Deg2Rad;
                var allValidHits = Physics2D.RaycastAll(
                        muzzlePos,
                        new Vector2(Mathf.Cos(fireAngleRad), Mathf.Sin(fireAngleRad))
                    )
                    .Where(hit => hit.collider.GetComponent<IHittable>() != null)
                    .ToList();

                if (!this.DoesPenetrate)
                {
                    allValidHits = new List<RaycastHit2D>() { allValidHits.First() };
                }

                // If we hit anything
                if (allValidHits.Count > 0)
                {
                    // Deal damage
                    foreach (var validHit in allValidHits)
                    {
                        validHit.collider.GetComponent<IHittable>().OnHit(this.Damage, this.EffectImpacts);
                    }

                    // Draw line
                    var newLine = Instantiate(this.BulletLiniePrefab);
                    newLine.DrawHit(muzzlePos, allValidHits);
                }
            }

            return null;
        }
    }
}
