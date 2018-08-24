//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ANDGate.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Transformers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Describes an AND gate
    /// </summary>
    public class ANDGate : Transformer
    {
        /// <summary>
        /// Called when the input is changed
        /// </summary>
        public override void OnInputChange()
        {
            this.Trigger( this.Inputs.All(input => input.IsOn));
        }
    }
}
