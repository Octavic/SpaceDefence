//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Detector.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Used by 
    /// </summary>
    public class Detector : MonoBehaviour
    {
        /// <summary>
        /// The emitter that emitted this detector
        /// </summary>
        public Emitter Source;

        /// <summary>
        /// The number of enemies currently in detection
        /// </summary>
        private int _currentlyIn
        {
            get
            {
                return this._currentlyInValue;
            }
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Trigger count less than 0");
                    value = 0;
                }

                if (this._currentlyInValue == 0 && value > 0)
                {
                    Source.Trigger(true);
                }
                else if (this._currentlyInValue > 0 && value == 0)
                {
                    Source.Trigger(false);
                }

                this._currentlyInValue = value;
            }
        }
        private int _currentlyInValue;

        /// <summary>
        /// When a collision happens
        /// </summary>
        /// <param name="collision">The collision that occurred</param>
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.otherCollider.gameObject.tag != Tags.Enemy)
            {
                // Not an enemy, do nothing
                return;
            }

            this._currentlyIn++;
        }

        /// <summary>
        /// Called when a watched entity leaves the detection range
        /// </summary>
        /// <param name="collision">The collision that left</param>
        protected void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.otherCollider.gameObject.tag != Tags.Enemy)
            {
                // Not an enemy, do nothing
                return;
            }

            this._currentlyIn--;
        }
    }
}
