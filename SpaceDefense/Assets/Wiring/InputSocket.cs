//  --------------------------------------------------------------------------------------------------------------------
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
        /// Get or set the receiver that'll be notified if this socket changes
        /// </summary>
        public IReceiver Receiver { get; set; }

        /// <summary>
        /// A collection of connected sources => Current state
        /// </summary>
        public Dictionary<OutputSocket, bool> ConnectedOutputs = new Dictionary<OutputSocket, bool>();
        
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
        /// Disconnects the given output
        /// </summary>
        /// <param name="output">Target output</param>
        public void DisconnectOutput(OutputSocket output)
        {
            this.ConnectedOutputs.Remove(output);
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

            this.ConnectedOutputs[source] = newState;

            // No need to trigger receiver if the input didn't change
            if (newState == this.IsOn)
            {
                return;
            }

            // Possible change of state, recalculate
            var newCurrentState = this.ConnectedOutputs.Any(output => output.Value);

            // Re-evaluate
            if (newCurrentState != this.IsOn)
            {
                this.IsOn = newCurrentState;

                if (this.Receiver != null)
                {
                    this.Receiver.OnInputChange();
                }
            }
        }
    }
}
