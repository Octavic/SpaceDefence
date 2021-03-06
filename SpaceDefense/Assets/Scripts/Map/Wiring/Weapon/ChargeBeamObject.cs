﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChargeBeamObject.cs">
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
    using Map.Enemies;

    /// <summary>
    /// The actual charge beam 
    /// </summary>
    public class ChargeBeamObject : MonoBehaviour, IConstantHitbox
    {
        /// <summary>
        /// Color variation of the beam based on charge level
        /// </summary>
        public Color UnchargedColor;
        public Color FullyChargedColor;

        /// <summary>
        /// The damage the beam does per second when fully charged
        /// </summary>
        public float MaxDamage;

        /// <summary>
        /// The damage the beam does per second when not charged at all
        /// </summary>
        public float MinDamage;

        /// <summary>
        /// How long the beam will stay active for
        /// </summary>
        public float ActiveDuration;

        /// <summary>
        /// The actual damage per second adjusted with charge level
        /// </summary>
        private float _realDamage;

        /// <summary>
        /// How much time must pass before the beam can be activated again
        /// </summary>
        private float _activeDurationLeft;

        /// <summary>
        /// The sprite renderer component
        /// </summary>
        private SpriteRenderer _renderer;

        /// <summary>
        /// Called when the beam was activated
        /// </summary>
        /// <param name="chargeLevel">The charge level</param>
        public void Activate(float chargeLevel)
        {
            if (this._activeDurationLeft > 0)
            {
                return;
            }

            this.gameObject.SetActive(true);
            this._activeDurationLeft = this.ActiveDuration;
            this._realDamage = (this.MaxDamage - this.MinDamage) * chargeLevel * chargeLevel + this.MinDamage;
            this._renderer.color = Lerp.LerpColor(this.UnchargedColor, this.FullyChargedColor, chargeLevel);
        }

        /// <summary>
        /// Called when the beam hits an enemy
        /// </summary>
        /// <param name="hitEnemy">The enemy that was hit by this beam</param>
        public void OnHitEnemy(Enemy hitEnemy)
        {
            hitEnemy.OnHit(this._realDamage * Time.deltaTime, null);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._renderer = this.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (this._activeDurationLeft > 0)
            {
                this._activeDurationLeft -= Time.deltaTime;
                if (this._activeDurationLeft <= 0)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
