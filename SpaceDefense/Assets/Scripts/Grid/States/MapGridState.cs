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
    /// Defines a certain state of the map grid
    /// </summary>
    [Serializable]
    public class MapGridState
    {
        /// <summary>
        /// The size of the grid
        /// </summary>
        public int SizeX;
        public int SizeY;

        /// <summary>
        /// A collection of grid entities
        /// </summary>
        public List<GridEntityState> GridEntities =  new List<GridEntityState>();
    }
}
