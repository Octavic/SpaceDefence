//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Delayer.cs">
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

    /// <summary>
    /// Defines a delayer that transfers signals, but with a delay
    /// </summary>
    public class Delayer : Transformer
    {
        /// <summary>
        /// The delay on the delayer
        /// </summary>
        public float TriggerDelay;

        protected override float TotalTriggerDelay
        {
            get
            {
                return this.TriggerDelay;
            }
        }
        public override void OnInputChange()
        {
            this.Trigger(this.Inputs.Any(input => input.IsOn));
        }
    }
}
