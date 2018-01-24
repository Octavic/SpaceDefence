//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Toggle.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring.Transformers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A controller that is togged with an on input
    /// </summary>
    public class Toggle : Transformer
    {
        private bool _currentInputState;
        private bool _currentOutputState;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            var isOn = this.Inputs.Any(input => input.IsOn);
            if (isOn && !this._currentInputState)
            {
                this._currentOutputState = !this._currentOutputState;
                this.Trigger(this._currentOutputState);
            }

            this._currentInputState = isOn;
        }
    }
}
