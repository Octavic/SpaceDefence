//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="OutputSocket.cs">
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
    /// Defines an output socket
    /// </summary>
    public class OutputSocket : MonoBehaviour, ISocket
    {
        /// <summary>
        /// A  collection of connected input sockets
        /// </summary>
        public HashSet<InputSocket> ConnectedInputs;

        /// <summary>
        /// Try to add another input socket onto the list of subscribers
        /// </summary>
        /// <param name="newInput">New input socket</param>
        /// <returns>True if successful</returns>
        public bool TryAddInputSocket(InputSocket newInput)
        {
            if (this.ConnectedInputs.Contains(newInput))
            {
                return false;
            }

            if (this.ConnectedInputs.Count > GeneralSettings.MaxConnectionPerSocket)
            {
                return false;
            }

            this.ConnectedInputs.Add(newInput);
            return true;
        }

        /// <summary>
        /// Changes the state of the output socket
        /// </summary>
        /// <param name="newState">The new state</param>
        public void Trigger(bool newState)
        {
            foreach (var input in this.ConnectedInputs)
            {
                input.Trigger(this, newState);
            }
        }
    }
}
