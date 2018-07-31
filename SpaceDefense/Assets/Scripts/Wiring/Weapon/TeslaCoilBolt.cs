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
    using Utils;
    using Enemies;

    /// <summary>
    /// The bolt between two tesla coils
    /// </summary>
    public class TeslaCoilBolt : AttachableBeam, IConstantHitbox
    {
        public float DamagePerSecond;

        public List<EffectEnum> Effects;
        public List<float> Impacts;

        private Dictionary<EffectEnum, float> _effects;

        /// <summary>
        /// Called when the bolt hits an enemy
        /// </summary>
        /// <param name="hitEnemy">The enmy hit</param>
        public void OnHitEnemy(Enemy hitEnemy)
        {
            hitEnemy.TakeDamage(this.DamagePerSecond * Time.deltaTime, this._effects);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._effects = Utils.ConvertListToDictionary(this.Effects, this.Impacts);
        }
    }
}
