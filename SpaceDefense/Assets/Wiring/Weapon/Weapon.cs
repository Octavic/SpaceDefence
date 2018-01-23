//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Weapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Grid;

    /// <summary>
    /// Defines a base weapon class
    /// </summary>
    public abstract class Weapon : GridEntity, IReceiver
    {
        /// <summary>
        /// A collection of input sockets
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
        /// Called when any of the input changes
        /// </summary>
        public abstract void OnInputChange();
    }
}
