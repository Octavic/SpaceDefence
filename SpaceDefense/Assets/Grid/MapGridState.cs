//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Grid.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Grid
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
