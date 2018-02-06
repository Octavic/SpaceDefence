﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Toggle.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Transformers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A controller that is togged with an on input
    /// </summary>
    public class Toggle : Transformer
    {
        /// <summary>
        /// sprites for when the toggle is on/off
        /// </summary>
        public Sprite OnSprite;
        public Sprite OffSprite;

        private bool _currentInputState;
        private bool _currentOutputState;

        /// <summary>
        /// The sprite renderer component
        /// </summary>
        private SpriteRenderer _renderer;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            var isOn = this.Inputs.Any(input => input.IsOn);
            if (isOn && !this._currentInputState)
            {
                this._currentOutputState = !this._currentOutputState;
                this._renderer.sprite = isOn ? OnSprite : OffSprite;
                this.Trigger(this._currentOutputState);
            }

            this._currentInputState = isOn;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected override void Start()
        {
            this._renderer = this.GetComponent<SpriteRenderer>();
            base.Start();
        }
    }
}
