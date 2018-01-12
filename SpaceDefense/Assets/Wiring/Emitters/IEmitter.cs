//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IEmitter.cs">
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
    /// Describes an item that can emit items
    /// </summary>
    public interface IEmitter
    {
        /// <summary>
        /// Gets a list of output sockets
        /// </summary>
        IList<OutputSocket> Outputs { get; }

        /// <summary>
        /// Called when the emitter is triggered
        /// </summary>
        void Trigger(bool newState);
    }
}
