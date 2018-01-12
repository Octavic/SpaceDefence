//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IEnemy.cs">
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

    /// <summary>
    /// Describes an enemy entity
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        /// <summary>
        /// The effects currently active and their respective duration left
        /// </summary>
        protected Dictionary<EffectsEnum, float> Effects;

        /// <summary>
        /// How fast the enemy moves per frame
        /// </summary>
        public float Speed;

        /// <summary>
        /// Gets or sets the route that the enemy will take
        /// </summary>
        public List<Vector2> Path { get; set; }

        /// <summary>
        /// The node in the route that the enemy is trying to reach
        /// </summary>
        private int _currentGoalNode;

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
        /// If the enemy has the  target effect
        /// </summary>
        /// <param name="effect">Target effect</param>
        /// <returns>True if the enemy has the effect</returns>
        public bool HasEffect(EffectsEnum effect)
        {
            return this.Effects.ContainsKey(effect);
        }

        /// <summary>
        /// Called once per frame 
        /// </summary>
        protected virtual void Update()
        {
            Vector2 curPos = this.transform.position;
            Vector2 curGoal = this.Path[this._currentGoalNode];
            Vector2 diff = curGoal - curPos;
            var movementThisFrame = this.Speed * Time.deltaTime;
            if (diff.magnitude < movementThisFrame)
            {
                // Can reach destination this frame, proceed to 
                this._currentGoalNode++;
                if (this._currentGoalNode >= this.Path.Count)
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
        }
    }
}
