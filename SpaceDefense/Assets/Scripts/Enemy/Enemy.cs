//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Enemy.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Enemy
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
        /// The amount of base hit points for the enemy
        /// </summary>
        public float BaseHealth;

        /// <summary>
        /// THe amount of base armor for the enemy
        /// </summary>
        public float BaseShield;

        /// <summary>
        /// The amount of base armor for the enemy
        /// </summary>
        public float BaseArmor;

        /// <summary>
        /// Bonus shield and health
        /// </summary>
        public float BonusHealth { get; private set; }
        public float BonusShield { get; private set; }
        public float BonusArmor { get; private set; }

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
        protected Dictionary<EffectEnum, float> EffectBuildUp = new Dictionary<EffectEnum, float>();

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
        public void TakeDamage(float damage, IDictionary<EffectEnum, float> carriedEffects = null)
        {
            bool isZapped = this.Effects.ContainsKey(EffectEnum.Zapped);
            bool isVulnerable = this.Effects.ContainsKey(EffectEnum.Vulnerable);

            float healthDamage = 0;

            if (isZapped)
            {
                damage *= EffectSettings.ZappedShieldDamageMultiplier;
            }

            if (this.CurrentShield > 0)
            {
                if (this.CurrentShield >= damage)
                {
                    this.CurrentShield -= damage;
                    healthDamage = 0;
                }
                else
                {
                    healthDamage -= this.CurrentShield;
                    this.CurrentShield = 0;
                }
            }
            else
            {
                healthDamage = damage;
            }

            if (healthDamage != 0)
            {
                if (isZapped)
                {
                    healthDamage /= EffectSettings.ZappedShieldDamageMultiplier;
                }

                if (isVulnerable)
                {
                    healthDamage *= EffectSettings.VulnerableHealthDamageMultiplier;
                }

                this.CurrentHealth -= healthDamage;
            }

            this._shieldRegenDelay = EnemySettings.ShieldRegenDelay;

            if (this.CurrentHealth <= 0)
            {
                GameController.CurrentInstance.AddIncome(this.Worth);
                Destroy(this.gameObject);
                return;
            }

            if (carriedEffects != null)
            {
                var effects = carriedEffects.Keys;

                foreach (var effect in effects)
                {
                    var impact = carriedEffects[effect];
                    float oldValue;
                    if (!this.EffectBuildUp.TryGetValue(effect, out oldValue))
                    {
                        oldValue = 0;
                    }

                    this.EffectBuildUp[effect] = oldValue + impact;

                    if (this.EffectBuildUp[effect] > EffectSettings.EffectProcLimit)
                    {
                        this.ApplyEffect(effect);
                        this.StatusBar.AddEffect(effect);
                        this.EffectBuildUp[effect] = -EffectSettings.EffectProcLimit;
                    }
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
            this.CurrentHealth = this.BaseHealth;
            this.CurrentShield = this.BaseShield;
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

            var movementThisFrame = this.Speed * Time.deltaTime;
            if (this.Effects.ContainsKey(EffectEnum.Slowed))
            {
                movementThisFrame *= EffectSettings.SlowSpeedMultiplier;
            }
            else if (this.Effects.ContainsKey(EffectEnum.Frozen))
            {
                movementThisFrame *= 0.01f;
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
                this.transform.position = curGoal;
            }
            else
            {
                //  Not yet reached, proceed
                this.transform.position = curPos + diff.normalized * movementThisFrame;
            }

            // Apply poison
            if (this.Effects.ContainsKey(EffectEnum.Poisoned))
            {
                var damage = EffectSettings.PoisonDamagePerSecond * Time.deltaTime;
                if (this.Effects.ContainsKey(EffectEnum.Vulnerable))
                {
                    damage *= 2;
                }
                this.CurrentHealth -= damage;
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
                else
                {
                    this.Effects[key] -= decay;
                }
            }

            // Decay effect resistance
            keys = new List<EffectEnum>(this.EffectBuildUp.Keys);
            decay *= EffectSettings.EffectBuildUpDecayPerSecond;
            foreach (var key in keys)
            {
                var curValue = this.EffectBuildUp[key];
                if (curValue > EffectSettings.EffectProcLimit)
                {
                    this.EffectBuildUp[key] -= decay;
                }
            }

            // Shield regeneration
            if (this._shieldRegenDelay > 0)
            {
                this._shieldRegenDelay -= Time.deltaTime;
            }
            else if (this.CurrentShield < this.BaseShield)
            {
                var regenAmount = Time.deltaTime * EnemySettings.ShieldRegenSpeed;
                if (this.Effects.ContainsKey(EffectEnum.Zapped))
                {
                    regenAmount *= EffectSettings.ZappedShieldRegenMultiplier;
                }

                this.CurrentShield = Mathf.Min(this.BaseShield, this.CurrentShield + regenAmount);
            }
        }

        /// <summary>
        /// Called when a collider stays within detection
        /// </summary>
        /// <param name="collision">The collision</param>
        private void OnTriggerStay2D(Collider2D collision)
        {
            var beamObject = collision.gameObject.GetComponent<IConstantHitbox>();
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
