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
        /// Prefab for the  socket beam 
        /// </summary>
        public AttachedBeam BeamPrefab;

        /// <summary>
        /// A  collection of connected input sockets => their respective beams
        /// </summary>
        public Dictionary<InputSocket, AttachedBeam> ConnectedInputs = new Dictionary<InputSocket, AttachedBeam>();

        /// <summary>
        /// The current state for the socket
        /// </summary>
        private bool _currentState = false;

        /// <summary>
        /// Try to add another input socket onto the list of subscribers
        /// </summary>
        /// <param name="newInput">New input socket</param>
        /// <returns>True if successful</returns>
        public bool TryAddInputSocket(InputSocket newInput)
        {
            if (this.ConnectedInputs.ContainsKey(newInput))
            {
                return false;
            }

            if (this.ConnectedInputs.Count > GeneralSettings.MaxConnectionPerSocket)
            {
                return false;
            }

            if (!newInput.TryAddOutput(this))
            {
                return false;
            }

            var newBeam = Instantiate(this.BeamPrefab.gameObject).GetComponent<AttachedBeam>();
            newBeam.Attach(this.transform.position, newInput.transform.position);
            this.ConnectedInputs[newInput] = newBeam;
            newInput.Trigger(this, this._currentState);
            newBeam.Trigger(this._currentState);
            return true;
        }

        /// <summary>
        /// /Disconnects the given input
        /// </summary>
        /// <param name="input">Target input</param>
        public void DisconnectInputSocket(InputSocket input)
        {
            if (input == null)
            {
                return;
            }

            AttachedBeam beam;
            if (!this.ConnectedInputs.TryGetValue(input, out beam))
            {
                return;
            }

            Destroy(beam.gameObject);
            input.DisconnectOutput(this);
            this.ConnectedInputs.Remove(input);
        }

        /// <summary>
        /// Changes the state of the output socket
        /// </summary>
        /// <param name="newState">The new state</param>
        public void Trigger(bool newState)
        {
            this._currentState = newState;

            foreach (var input in this.ConnectedInputs)
            {
                input.Key.Trigger(this, newState);
                input.Value.Trigger(newState);
            }
        }
    }
}
