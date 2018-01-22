//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ManualControl.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines an emitter that's manually controlled
    /// </summary>
    public abstract class ManualControl : Emitter
    {
        /// <summary>
        /// Called when the element is clicked
        /// </summary>
        public abstract void OnClick();

        /// <summary>
        /// Called when the mouse is let go over the element
        /// </summary>
        public abstract void OnUnclick();
    }
}
