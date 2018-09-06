//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IReceiver.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring
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

        /// <summary>
        /// Gets the index of the target input
        /// </summary>
        /// <param name="input">Target input</param>
        /// <returns>-1 if unavailable</returns>
        int IndexOf(InputSocket input);

        /// <summary>
        /// Gets the input socket at index
        /// </summary>
        /// <param name="index">Target index</param>
        /// <returns>The targetted input socket, null if out of range</returns>
        InputSocket GetInputSocket(int index);
    }
}
