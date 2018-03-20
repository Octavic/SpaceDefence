//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Enemy.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
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
        /// The status bar
        /// </summary>
        public EnemyStatusBar StatusBar;

        /// <summary>
        /// What kind of enemy this is
        /// </summary>
        public EnemyType Type;

        /// <summary>
        /// How fast the enemy moves per frame
        /// </summary>
        public float Speed;

        /// <summary>
        /// How much the enemy is worth
        /// </summary>
        public float Worth;

        /// <summary>
        /// The amount of total hit points for the enemy
        /// </summary>
        public float TotalHealth;

        /// <summary>
        /// THe amount of total shields for the enemy
        /// </summary>
        public float TotalShield;

        /// <summary>
        /// Gets the current health level of the enemy
        /// </summary>
        public float CurrentHealth { get; private set; }

        /// <summary>
        /// The amount of shield that's currently present
        /// </summary>
        public float CurrentShield { get; private set; }

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
        /// How long after hit does the shield start regenerating
        /// </summary>
        private float _shieldRegenDelay = 0;

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
        public void TakeDamage(float damage, EffectEnum carriedEffect = EffectEnum.None)
        {
            if (this.Effects.ContainsKey(EffectEnum.Vulnerable))
            {
                damage *= 2;
            }

            if (this.CurrentShield > 0)
            {
                if (this.CurrentShield >= damage)
                {
                    this.CurrentShield -= damage;
                }
                else
                {
                    this.CurrentHealth -= (damage - this.CurrentShield);
                    this.CurrentShield = 0;
                }
            }
            else
            {
                this.CurrentHealth -= damage;
            }

            this._shieldRegenDelay =EnemySettings.ShieldRegenDelay;

            if (this.CurrentHealth <= 0)
            {
                GameController.CurrentInstance.AddIncome(this.Worth);
                Destroy(this.gameObject);
                return;
            }

            if (carriedEffect != EffectEnum.None)
            {
                float oldValue;
                if (!this.EffectResistance.TryGetValue(carriedEffect, out oldValue))
                {
                    oldValue = 0;
                }

                this.EffectResistance[carriedEffect] = oldValue + damage;

                if (this.EffectResistance[carriedEffect] > 0)
                {
                    this.ApplyEffect(carriedEffect);
                    this.StatusBar.AddEffect(carriedEffect);
                    this.EffectResistance[carriedEffect] = EffectSettings.EffectResistance;
                }
            }
        }

        /// <summary>
        /// Applies the given effect
        /// </summary>
        /// <param name="effect">Target effect</param>
        public void ApplyEffect(EffectEnum effect)
        {
            this.Effects[effect] = EffectSettings.EffectDuration;
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
            this.CurrentShield = this.TotalShield;
        }

        /// <summary>
        /// Called once per frame 
        /// </summary>
        protected virtual void Update()
        {
            // Movement
            Vector2 curPos = this.transform.position;
            Vector2 curGoal = this.Path[this._currentPathNodeIndex];
            Vector2 diff = curGoal - curPos;

            if (!this.Effects.ContainsKey(EffectEnum.Frozen))
            {
                var movementThisFrame = this.Speed * Time.deltaTime;
                if (this.Effects.ContainsKey(EffectEnum.Slowed))
                {
                    movementThisFrame *= EffectSettings.SlowSpeedMultiplier;
                }

                if (diff.magnitude < movementThisFrame)
                {
                    // Can reach destination this frame, proceed to next node as goal
                    this._currentPathNodeIndex++;
                    if (this._currentPathNodeIndex >= this.Path.Count)
                    {
                        GameController.CurrentInstance.OnEnemyReachEnd(this);
                        return;
                    }
                    this._rigidbody.MovePosition(curGoal);
                }
                else
                {
                    //  Not yet reached, proceed
                    this._rigidbody.MovePosition(curPos + diff.normalized * movementThisFrame);
                }
            }

            // Apply poison
            if (this.Effects.ContainsKey(EffectEnum.Poisoned))
            {
                this.CurrentHealth -= EffectSettings.PoisonDamagePerSecond * Time.deltaTime;
                if (this.CurrentHealth <= 0)
                {
                    GameController.CurrentInstance.AddIncome(this.Worth);
                    Destroy(this.gameObject);
                    return;
                }
            }

            // Decay effects
            var keys = new List<EffectEnum>(this.Effects.Keys);
            var decay = Time.deltaTime;
            foreach (var key in keys)
            {
                var remainingDuration = this.Effects[key];
                if (remainingDuration <= decay)
                {
                    this.StatusBar.RemoveEffect(key);
                    this.Effects.Remove(key);
                }
                this.Effects[key] -= decay;
            }

            // Decay effect resistance
            keys = new List<EffectEnum>(this.EffectResistance.Keys);
            decay *= EffectSettings.EffectBuildUpDecayPerSecond;
            foreach (var key in keys)
            {
                var curValue = this.EffectResistance[key];
                if (curValue > EffectSettings.EffectResistance)
                {
                    this.EffectResistance[key] -= decay;
                }
            }

            // Shield regeneration
            if (this._shieldRegenDelay > 0)
            {
                this._shieldRegenDelay -= Time.deltaTime;
            }
            else if (this.CurrentShield < this.TotalShield)
            {
                this.CurrentShield = Mathf.Min(this.TotalShield, this.CurrentShield + Time.deltaTime *EnemySettings.ShieldRegenSpeed);
            }
        }

        /// <summary>
        /// Called when a collider stays within detection
        /// </summary>
        /// <param name="collision">The collision</param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            var beamObject = collision.gameObject.GetComponent<BeamWeaponObject>();
            if (beamObject != null)
            {
                beamObject.OnHitEnemy(this);
            }
        }

        /// <summary>
        /// Called when trigger enters
        /// </summary>
        /// <param name="collision">The collision that happened</param>
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (!this.Effects.ContainsKey(EffectEnum.Cloaked))
            {
                var detector = collision.gameObject.GetComponent<DetectorArea>();
                if (detector != null)
                {
                    detector.OnEnemyEnter();
                    return;
                }
            }

            var projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.OnHittingEnemy(this);
                return;
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
