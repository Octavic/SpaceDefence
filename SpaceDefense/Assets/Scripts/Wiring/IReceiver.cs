//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IReceiver.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes an item that can receive signals
    /// </summary>
    public interface IReceiver
    {
        /// <summary>
        /// Gets or sets the inputs 
        /// </summary>
        IList<InputSocket> Inputs { get; }

        /// <summary>
        /// Called when the inputs are changed
        /// </summary>
        void OnInputChange();
    }
}
