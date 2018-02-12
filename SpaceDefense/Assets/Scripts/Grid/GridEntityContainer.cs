//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntityContainer.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;

    /// <summary>
    /// A container that can hold one grid entity on the map
    /// </summary>
    public abstract class GridEntityContainer : GridEntity, IReceiver
    {
        /// <summary>
        /// A list of input sockets
        /// </summary>
        public List<InputSocket> InputSockets;
        public IList<InputSocket> Inputs
        {
            get
            {
                return this.InputSockets;
            }
        }

        /// <summary>
        /// THe grid entity currently in place
        /// </summary>
        public GridEntity CurrentlyHolding = null;

        /// <summary>
        /// Try to add a new entity
        /// </summary>
        /// <param name="newEntity">New entity to be added</param>
        /// <returns>True if operation succed</returns>
        public bool CanAddEntity(GridEntity newEntity)
        {
            // New entity must be 1 x 1
            if (newEntity.ExtrudeX > 0 || newEntity.ExtrudeY > 0)
            {
                return false;
            }

            if (this.CurrentlyHolding != null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes whatever this entity is holding
        /// </summary>
        public void RemoveContent()
        {
            if (this.CurrentlyHolding != null)
            {
                Destroy(this.CurrentlyHolding.gameObject);
            }
            this.CurrentlyHolding = null;
        }

        /// <summary>
        /// Try to add a new entity
        /// </summary>
        /// <param name="newEntity">New entity to be added</param>
        /// <returns>True if operation succeed</returns>
        public abstract bool TryAddEntity(GridEntity newEntity);

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public abstract void OnInputChange();

        /// <summary>
        /// Used for intialization
        /// </summary>
        protected virtual void Start()
        {
            foreach (var input in this.InputSockets)
            {
                input.Receiver = this;
            }
        }

        public InputSocket GetInputSocket(int index)
        {
            if (index < 0)
            {
                return null;
            }

            if (index < this.Inputs.Count)
            {
                return this.Inputs[index];
            }

            var holdingReceiver = this.CurrentlyHolding as IReceiver;
            return holdingReceiver.GetInputSocket(index - this.Inputs.Count);
        }
        public int IndexOf(InputSocket input)
        {
            var result = this.Inputs.IndexOf(input);
            if (result > 0)
            {
                return result;
            }


            var holdingReceiver = this.CurrentlyHolding as IReceiver;
            return holdingReceiver.IndexOf(input) + this.Inputs.Count;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected virtual void Update()
        {
        }
    }
}
