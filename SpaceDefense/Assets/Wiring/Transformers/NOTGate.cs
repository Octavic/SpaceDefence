//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="NOTGate.cs">
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
    /// Defines a NOT gate
    /// </summary>
    public class NOTGate : Transformer
    {
        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            this.Trigger(!this.Inputs[0].IsOn);
        }
    }
}
