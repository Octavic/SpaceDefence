//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IEmitter.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Emitters
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

        /// <summary>
        /// Gets the index of the output
        /// </summary>
        /// <param name="output">Target output</param>
        /// <returns>Index of the target output, -1 if unavailable</returns>
        int IndexOf(OutputSocket output);

        /// <summary>
        /// Gets the output socket at index
        /// </summary>
        /// <param name="index">Target index</param>
        /// <returns>The targeted output socket, null if out of range </returns>
        OutputSocket GetOutputSocket(int index);
    }
}
