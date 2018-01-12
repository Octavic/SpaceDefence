﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="InputSocket.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;

    /// <summary>
    /// Describes an input socket
    /// </summary>
    public class InputSocket : MonoBehaviour, ISocket
    {
        /// <summary>
        /// Gets the current status of the input socket
        /// </summary>
        public bool IsOn { get; private set; }

        /// <summary>
        /// A collection of connected sources => Current state
        /// </summary>
        public Dictionary<OutputSocket, bool> ConnectedOutputs;

        /// <summary>
        /// The amount of connected outputs that's currently on
        /// </summary>
        private int triggeredOutputCount;

        /// <summary>
        /// Try to add a new connected output
        /// </summary>
        /// <param name="newOutput">resulting output</param>
        /// <returns>True if operation successful</returns>
        public bool TryAddOutput(OutputSocket newOutput)
        {
            if (this.ConnectedOutputs.ContainsKey(newOutput))
            {
                return false;
            }

            if (this.ConnectedOutputs.Count > GeneralSettings.MaxConnectionPerSocket)
            {
                return false;
            }

            this.ConnectedOutputs[newOutput] = false;
            return true;
        }

        /// <summary>
        /// Called when the output is turned on
        /// </summary>
        /// <param name="source">The one turning on this socket</param>
        /// <param name="newState">The new state applied</param>
        public void Trigger(OutputSocket source, bool newState)
        {
            bool currentState;
            if (!this.ConnectedOutputs.TryGetValue(source, out currentState))
            {
                Debug.LogError("Input triggered by unconnected output");
                return;
            }

            if (currentState == newState)
            {
                Debug.LogError("Input triggered on by the same output multiple times.");
                return;
            }

            this.ConnectedOutputs[source] = newState;
            if (newState)
            {
                this.triggeredOutputCount++;
            }
            else
            {
                this.triggeredOutputCount--;
            }

            this.IsOn = newState;
        }
    }
}
