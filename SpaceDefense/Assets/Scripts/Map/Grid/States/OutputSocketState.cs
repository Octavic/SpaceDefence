//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="OutputSocketState.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Grid.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// State of the output socket
    /// </summary>
    [Serializable]
    public class OutputSocketState
    {
        /// <summary>
        /// A list of connections to input sockets
        /// </summary>
        public List<OutputConnectionState> Connections = new List<OutputConnectionState>();
    }
}
