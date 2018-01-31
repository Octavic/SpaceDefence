//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Enemy.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;
    using Wiring.Emitters;
    using Wiring.Weapon;

    /// <summary>
    /// Describes an enemy entity
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// The health bar component 
        /// </summary>
        public EnemyHealthBar HealthBar;

        /// <summary>
        /// How fast the enemy moves per frame
        /// </summary>
        public float Speed;

        /// <summary>
        /// The amount of total hit points for the enemy
        /// </summary>
        public float TotalHealth;

        /// <summary>
        /// Gets the current health level of the enemy
        /// </summary>
        public float CurrentHealth { get; private set; }

        /// <summary>
        /// Gets or sets the route that the enemy will take
        /// </summary>
        public List<Vector2> Path { get; set; }

        /// <summary>
        /// The effects currently active and their respective duration left
        /// </summary>
        protected Dictionary<EffectEnum, float> Effects = new Dictionary<EffectEnum, float>();

        /// <summary>
        /// A dictionary of effect => value until proc
        /// </summary>
        protected Dictionary<EffectEnum, float> EffectResistance = new Dictionary<EffectEnum, float>();

        /// <summary>
        /// The node in the route that the enemy is trying to reach
        /// </summary>
        private int _currentPathNodeIndex = 0;

        /// <summary>
        /// The rigidbody component
        /// </summary>
        protected Rigidbody2D _rigidbody
        {
            get
            {
                return this.GetComponent<Rigidbody2D>();
            }
        }

        /// <summary>
        /// Called when the enemy takes damage
        /// </summary>
        /// <param name="damage">How much damage to take</param>
        /// <param name="carriedEffect">The effect carried</param>
        public void TakeDamage(float damage, EffectEnum carriedEffect)
        {
            this.CurrentHealth -= damage;
            if (this.CurrentHealth <= 0)
            {
                Destroy(this.HealthBar.gameObject);
                Destroy(this.gameObject);
                return;
            }

            this.HealthBar.UpdateHealth(this);
            float oldValue;
            if (!this.EffectResistance.TryGetValue(carriedEffect, out oldValue))
            {
                oldValue = 0;
            }

            this.EffectResistance[carriedEffect] = oldValue + damage;

            if (this.EffectResistance[carriedEffect] > 0)
            {
                this.ApplyEffect(carriedEffect);
                this.EffectResistance[carriedEffect] = GeneralSettings.EffectResistance;
            }
        }

        /// <summary>
        /// Applies the given effect
        /// </summary>
        /// <param name="effect">Target effect</param>
        public void ApplyEffect(EffectEnum effect)
        {
            this.Effects[effect] = GeneralSettings.EffectDuration;
        }

        /// <summary>
        /// If the enemy has the  target effect
        /// </summary>
        /// <param name="effect">Target effect</param>
        /// <returns>True if the enemy has the effect</returns>
        public bool HasEffect(EffectEnum effect)
        {
            return this.Effects.ContainsKey(effect);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected virtual void Start()
        {
            this.CurrentHealth = this.TotalHealth;
        }

        /// <summary>
        /// Called once per frame 
        /// </summary>
        protected virtual void Update()
        {
            Vector2 curPos = this.transform.position;
            Vector2 curGoal = this.Path[this._currentPathNodeIndex];
            Vector2 diff = curGoal - curPos;
            var movementThisFrame = this.Speed * Time.deltaTime;
            if (diff.magnitude < movementThisFrame)
            {
                // Can reach destination this frame, proceed to next node as goal
                this._currentPathNodeIndex++;
                if (this._currentPathNodeIndex >= this.Path.Count)
                {
                    GameController.CurrentInstancce.OnEnemyReachEnd(this);
                    return;
                }
                this._rigidbody.MovePosition(curGoal);
            }
            else
            {
                //  Not yet reached, proceed
                this._rigidbody.MovePosition(curPos + diff.normalized * movementThisFrame);
            }

            // Decay effects
            var keys = new List<EffectEnum>(this.Effects.Keys);
            var decay = Time.deltaTime;
            foreach (var key in keys)
            {
                var remainingDuration = this.Effects[key];
                if (remainingDuration <= decay)
                {
                    this.Effects.Remove(key);
                }
                this.Effects[key] -= decay;
            }

            // Decay effect proc
            decay *= GeneralSettings.EffectBuildUpDecayPerSecond;
            foreach (var key in this.EffectResistance.Keys)
            {
                var curValue = this.EffectResistance[key];
                if (curValue > GeneralSettings.EffectResistance)
                {
                    this.EffectResistance[key] -= decay;
                }
            }
        }

        /// <summary>
        /// Called when trigger enters
        /// </summary>
        /// <param name="collision">The collision that happened</param>
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            var detector = collision.gameObject.GetComponent<DetectorArea>();
            if (detector != null)
            {
                detector.OnEnemyEnter();
            }

            var projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.OnHittingEnemy(this);
            }
        }

        /// <summary>
        /// Called when trigger enters
        /// </summary>
        /// <param name="collision">The collision that happened</param>
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            var detector = collision.gameObject.GetComponent<DetectorArea>();
            if (detector != null)
            {
                detector.OnEnemyExit();
            }
        }
    }
}
