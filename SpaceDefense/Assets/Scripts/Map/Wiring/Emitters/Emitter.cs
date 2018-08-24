//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Emitter.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Map.Grid;
    using Utils;

    /// <summary>
    /// Defines a base emitter
    /// </summary>
    public abstract class Emitter : GridEntity, IEmitter
    {
        /// <summary>
        /// A list of outputs
        /// </summary>
        public List<OutputSocket> OutputSockets;
        public IList<OutputSocket> Outputs
        {
            get
            {
                return this.OutputSockets;
            }
        }

        public int IndexOf(OutputSocket output)
        {
            return this.OutputSockets.IndexOf(output);
        }
        public OutputSocket GetOutputSocket(int index)
        {
            return this.OutputSockets[index];
        }

        /// <summary>
        /// The old state
        /// </summary>
        private bool _oldState;

        /// <summary>
        /// Called when something enters or leaves the detection
        /// </summary>
        /// <param name="newState">The new state of the emitter</param>
        public void Trigger(bool newState)
        {
            if (this._oldState == newState)
            {
                return;
            }

            this._oldState = newState;
            foreach (var outputsocket in this.Outputs)
            {
                outputsocket.Trigger(newState);
            }
        }

        public override void OnMove()
        {
            Utils.UpdateAllBeams(this.Outputs); ;
        }
    }
}
