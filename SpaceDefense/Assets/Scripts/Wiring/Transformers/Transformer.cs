//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Transformer.cs">
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
    using Settings;
    using Grid;

    /// <summary>
    /// Implements basic functionality for transformer
    /// </summary>
    public abstract class Transformer : GridEntity, ITransformer
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

        public InputSocket GetInputSocket(int index)
        {
            return this.Inputs[index];
        }
        public int IndexOf(InputSocket input)
        {
            return this.Inputs.IndexOf(input);
        }
        public int IndexOf(OutputSocket output)
        {
            return this.Outputs.IndexOf(output);
        }

        public OutputSocket GetOutputSocket(int index)
        {
            return this.Outputs[index];
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
        /// Used for initialization
        /// </summary>
        protected virtual void Start()
        {
            foreach (var input in this.Inputs)
            {
                input.Receiver = this;
            }
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
