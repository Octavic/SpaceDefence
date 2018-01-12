//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Transformer.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring.Transformers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;
    
    /// <summary>
    /// Implements basic functionality for transformer
    /// </summary>
    public abstract class Transformer : MonoBehaviour, ITransformer
    {
        /// <summary>
        /// The time delay between triggering and actually emitting the signal
        /// </summary>
        private float _triggerDelay;

        /// <summary>
        /// the old state
        /// </summary>
        private bool _oldState = false;

        protected virtual float TotalTriggerDelay
        {
            get
            {
                return GeneralSettings.TransformerDefaultTriggerDelay;
            }
        }

        /// <summary>
        /// A collection of inputs
        /// </summary>
        public List<InputSocket> InputSockets;
        public IList<InputSocket> Inputs
        {
            get
            {
                return this.InputSockets;
            }
        }

        /// <summary>
        /// A collection of outputs
        /// </summary>
        public List<OutputSocket> OutputSockets;
        public IList<OutputSocket> Outputs
        {
            get
            {
                return this.OutputSockets;
            }
        }

        /// <summary>
        /// Called when the input is changed
        /// </summary>
        public abstract void OnInputChange();

        /// <summary>
        /// Called when the transformer is triggered
        /// </summary>
        /// <param name="newState">The new stat</param>
        public void Trigger(bool newState)
        {
            if (this._oldState == newState)
            {
                return;
            }

            this._oldState = newState;
            this._triggerDelay = this.TotalTriggerDelay;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected virtual void Update()
        {
            if (this._triggerDelay > 0)
            {
                this._triggerDelay -= Time.deltaTime;
                if (this._triggerDelay < 0)
                {
                    this._triggerDelay = 0;
                    foreach (var output in this.OutputSockets)
                    {
                        output.Trigger(this._oldState);
                    }
                }
            }
        }
    }
}
