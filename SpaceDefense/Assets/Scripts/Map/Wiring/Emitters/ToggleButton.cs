//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ToggleButton.cs">
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
    /// Trigger button
    /// </summary>
    public class ToggleButton : ManualControl
    {
        /// <summary>
        /// Gets the current state of the button
        /// </summary>
        public bool State { get; private set; }

        /// <summary>
        /// Called when the player clicks this element
        /// </summary>
        public override void OnClick()
        {
            this.State = !this.State;
            this.Trigger(this.State);
        }

        /// <summary>
        /// Called when the player lets go of the element
        /// </summary>
        public override void OnUnclick()
        {
            // Do nothing here
        }
    }
}
