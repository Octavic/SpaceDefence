//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Delayer.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Transformers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines a delayer that transfers signals, but with a delay
    /// </summary>
    public class Delayer : Transformer
    {
        /// <summary>
        /// The delay on the delayer
        /// </summary>
        public float TriggerDelay;

        /// <summary>
        /// Override the default computational delay to be instant. The delay will be handled by this class
        /// </summary>
        protected override float TotalTriggerDelay
        {
            get
            {
                return 0.000001f;
            }
        }

        /// <summary>
        /// how often the input is checked
        /// </summary>
        private float _checkInterval;

        /// <summary>
        /// How many seconds since the last check interval
        /// </summary>
        private float _timeSinceCheck;

        /// <summary>
        /// A queue of not yet emitted signals
        /// </summary>
        private Queue<bool> _pendingStates = new Queue<bool>();

        /// <summary>
        /// To keep track of the state of the pending signals and if they are all the same
        /// </summary>
        private bool _sameValue = false;
        private int _sameCount = 4;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            // If the delayer is still active, do nothing. The update loop will handle it
            if (this._sameCount < 4)
            {
                return;
            }

            // Check if the new state is the same as old. If so, do nothing
            var newState = this.Inputs.Any(socket => socket.IsOn);
            if (newState == this._sameValue)
            {
                return;
            }

            // Force update loop to include changes
            this._sameCount--;
        }

        /// <summary>
        /// Calculate the  interval
        /// </summary>
        protected override void Start()
        {
            this._checkInterval = this.TriggerDelay / 4;
            for (int i = 0; i < 4; i++)
            {
                this._pendingStates.Enqueue(false);
            }

            base.Start();
        }

        /// <summary>
        /// Checks the socket and trigger if needed
        /// </summary>
        private void CheckSocket()
        {
            this._timeSinceCheck += Time.deltaTime;

            // No time yet, do nothing
            if (this._timeSinceCheck < this._checkInterval)
            {
                return;
            }

            this._timeSinceCheck -= this._checkInterval;
            var newState = this.Inputs.Any(socket => socket.IsOn);

            if (newState == this._sameValue)
            {
                this._sameCount++;
            }
            else
            {
                this._sameValue = newState;
                this._sameCount = 1;
            }

            this._pendingStates.Enqueue(newState);
            var changeTo = this._pendingStates.Dequeue();
            this.Trigger(changeTo);
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected override void Update()
        {
            if (this._sameCount < 5)
            {
                this.CheckSocket();
            }

            base.Update();
        }
    }
}
