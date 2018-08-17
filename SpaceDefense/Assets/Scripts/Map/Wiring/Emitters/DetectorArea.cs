//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Detector.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Used by Detectors
    /// </summary>
    public class DetectorArea : MonoBehaviour
    {
        /// <summary>
        /// The detector that is using this area
        /// </summary>
        public Detector Source;

        public Sprite InactiveSprite;
        public Sprite ActiveSprite;

        /// <summary>
        /// The sprite renderer component
        /// </summary>
        private SpriteRenderer _renderer;

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
                    this._renderer.sprite = this.ActiveSprite;
                    Source.Trigger(true);
                }
                else if (this._currentlyInValue > 0 && value == 0)
                {
                    this._renderer.sprite = this.InactiveSprite;
                    Source.Trigger(false);
                }

                this._currentlyInValue = value;
            }
        }
        private int _currentlyInValue;

        /// <summary>
        /// When an enemy enters this beam
        /// </summary>
        public void OnEnemyEnter()
        {
            this._currentlyIn++;
        }

        /// <summary>
        /// when an enemy leaves this beam
        /// </summary>
        public void OnEnemyExit()
        {
            this._currentlyIn--;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._renderer = this.GetComponent<SpriteRenderer>();
        }
    }
}
