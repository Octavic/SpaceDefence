﻿using Assets.Wiring.Emitters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Defines a simple enemy
    /// </summary>
    public class SimpleEnemy : MonoBehaviour
    {
        /// <summary>
        /// Speed of the enemy
        /// </summary>
        public float Speed;

        /// <summary>
        /// The rigidboyd
        /// </summary>
        private Rigidbody2D _rgbd;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._rgbd = this.GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            this._rgbd.MovePosition(this.transform.position + new Vector3(this.Speed * Time.deltaTime, 0));
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
