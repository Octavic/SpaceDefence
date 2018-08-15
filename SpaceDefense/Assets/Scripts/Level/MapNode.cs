//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNode.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Specific production of a resource type
    /// </summary>
    [Serializable]
    public class MapNodeResources
    {
        public ResourceType Resrouce;
        public float Amount;
    }

    /// <summary>
    /// Defines a node on a map
    /// </summary>
    public class MapNode
    {
        /// <summary>
        /// Name of the node
        /// </summary>
        public string Name;

        /// <summary>
        /// Difficulty of the node
        /// </summary>
        public MapNodeDifficulty Difficulty;

        /// <summary>
        /// The amount of resources produced
        /// </summary>
        public List<MapNodeResources> Productions;

        /// <summary>
        /// The save data for this node
        /// </summary>
        public MapNodeSaveData SaveData;
    }
}
