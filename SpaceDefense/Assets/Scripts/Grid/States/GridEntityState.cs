//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapGridState.cs">
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
    /// An entity on the map grid state
    /// </summary>
    [Serializable]
    public class GridEntityState
    {
        /// <summary>
        /// ID of the entity
        /// </summary>
        public int EntityID;

        /// <summary>
        /// Positions and rotation of the object
        /// </summary>
        public int PosX;
        public int PosY;
        public int Rotation;

        /// <summary>
        /// The state  of all of the outputs
        /// </summary>
        public List<OutputSocketState> Outputs = new List<OutputSocketState>();
    }
}
