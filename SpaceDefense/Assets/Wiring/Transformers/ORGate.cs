//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ANDGate.cs">
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
    /// Defines an OR gate
    /// </summary>
    public class ORGate : Transformer
    {
        /// <summary>
        /// Called when the input is changed
        /// </summary>
        public override void OnInputChange()
        {
            this.Trigger(this.Inputs.Any(input => input.IsOn));
        }
    }
}
