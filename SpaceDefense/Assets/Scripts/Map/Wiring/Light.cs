//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Light.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A simple light component that can be turned on or off
    /// </summary>
    public class Light : MonoBehaviour 
    {
        /// <summary>
        /// Sprite for when the light is on
        /// </summary>
        public Sprite OnSprite;

        /// <summary>
        /// Sprite for when the light is off
        /// </summary>
        public Sprite OffSprite;

        /// <summary>
        /// The current state of the light
        /// </summary>
        private bool _state = false;

        /// <summary>
        /// The sprite renderer component
        /// </summary>
        private SpriteRenderer _renderer;

        /// <summary>
        /// Sets the light to be a new state
        /// </summary>
        /// <param name="newState"></param>
        public void Set(bool newState)
        {
            if (newState != this._state)
            {
                this._renderer.sprite = newState ? this.OnSprite : this.OffSprite;
                this._state = newState;
            }
        }

        /// <summary>
        /// Toggles the state
        /// </summary>
        public void Toggle()
        {
            this.Set(!this._state);
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
