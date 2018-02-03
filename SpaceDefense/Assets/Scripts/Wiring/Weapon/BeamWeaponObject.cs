//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BeamWeaponObject.cs">
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
    /// Describes the actual beam object fired from a beam weapon
    /// </summary>
    public class BeamWeaponObject : MonoBehaviour
    {
        /// <summary>
        /// The effect carried
        /// </summary>
        public EffectEnum CarriedEffect;

        /// <summary>
        /// The damage per second
        /// </summary>
        public float DamagePerSecond;

        /// <summary>
        /// Called wen the beam weapon is fired
        /// </summary>
        public void OnFire()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Called when the beam weapon stops to fire
        /// </summary>
        public void OnCeaseFire()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when the beam comes into contact with an enemy
        /// </summary>
        /// <param name="hitEnemy">The enemy</param>
        public void OnHitEnemy(Enemy hitEnemy)
        {
            hitEnemy.TakeDamage(this.DamagePerSecond * Time.deltaTime, this.CarriedEffect);
        }
    }
}
