//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ProjectileShell.cs">
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
    /// Defines a projectile
    /// </summary>
    public class ProjectileShell : MonoBehaviour
    {
        /// <summary>
        /// The actual projectile itself
        /// </summary>
        public Projectile ProjectileObject;

        /// <summary>
        /// the effect that's carried
        /// </summary>
        public List<EffectEnum> Effects;
        public List<float> Impacts;

        /// <summary>
        /// Speed of the bullet
        /// </summary>
        public float Velocity;

        /// <summary>
        /// Gets or sets the damage for the projectile
        /// </summary>
        public float Damage;

        /// <summary>
        /// If the projectile penetrates an enemy
        /// </summary>
        public bool DoesPenetrate;

        /// <summary>
        /// The minimal amount of time it can exist
        /// </summary>
        public float MinExistTime;

        /// <summary>
        /// The maximal amount of time it can exit
        /// </summary>
        public float MaxExistTime;

        /// <summary>
        /// How long the item has existed
        /// </summary>
        private float _timeExisted = 0;

        /// <summary>
        /// If the projectile has hit anything yet
        /// </summary>
        private bool _projectileHit = false;

        /// <summary>
        /// The composed effects dictionary
        /// </summary>
        public Dictionary<EffectEnum, float> EffectImpacts { get; private set; }

        /// <summary>
        /// Called when the projectile hits an enemy
        /// </summary>
        /// <param name="hitEnemy">The enemy that was hit</param>
        public virtual void OnHittingEnemy(Enemy hitEnemy)
        {
            hitEnemy.TakeDamage(this.Damage, this.EffectImpacts);
            if (!this.DoesPenetrate)
            {
                Destroy(this.ProjectileObject.gameObject);
                this._projectileHit = true;
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this.EffectImpacts = Utils.ConvertListToDictionary(this.Effects, this.Impacts);
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            this._timeExisted += Time.deltaTime;
            if (this._timeExisted >= this.MaxExistTime)
            {
                Destroy(this.gameObject);
                return;
            }

            if (this._timeExisted >= this.MinExistTime && this._projectileHit)
            {
                Destroy(this.gameObject);
                return;
            }
        }
    }
}
