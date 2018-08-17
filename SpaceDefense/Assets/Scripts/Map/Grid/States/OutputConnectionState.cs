//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="OutputConnectionState.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Grid.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes one connection between an output to an input
    /// </summary>
    [Serializable]
    public class OutputConnectionState
    {
        /// <summary>
        /// Grid coordinate of the connected input grid entity
        /// </summary>
        public int ConnectedX;
        public int ConnectedY;

        /// <summary>
        /// The index of the input that's connected to this output socket
        /// </summary>
        public int InputSocketIndex;
    }
}
