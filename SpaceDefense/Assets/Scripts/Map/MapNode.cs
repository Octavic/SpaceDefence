//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNode.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Specific production of a resource type
    /// </summary>
    [Serializable]
    public class MapNodeResource
    {
        /// <summary>
        /// Creates a deep copy
        /// </summary>
        /// <param name="copySource"></param>
        public MapNodeResource(MapNodeResource copySource = null)
        {
            if(copySource != null)
            {
                this.TargetResource = copySource.TargetResource;
                this.ProduceAmount = copySource.ProduceAmount;
                this.CapacityBoost = copySource.CapacityBoost;
            }
        }

        public ResourceType TargetResource;
        public float ProduceAmount;
        public float CapacityBoost;
    }

    /// <summary>
    /// Defines a node on a map
    /// </summary>
    [Serializable]
    public class MapNode
    {
        /// <summary>
        /// Name of the node
        /// </summary>
        public string Name;

        /// <summary>
        /// Id of the map node
        /// </summary>
        public int NodeId;

        /// <summary>
        /// Difficulty of the node
        /// </summary>
        public MapNodeDifficulty Difficulty;

        /// <summary>
        /// The source nodes that once any is unlocked, will make this node accessible
        /// </summary>
        public List<int> LockedBy;

        /// <summary>
        /// The amount of resources rewarded when this map node is captured
        /// </summary>
        public List<MapNodeResource> ResourceReward;

        /// <summary>
        /// How much power is generated when this map node is captured
        /// </summary>
        public float PowerGenerated;

        /// <summary>
        /// The save data for this node
        /// </summary>
        [HideInInspector]
        public MapNodeSaveData SaveData;

        /// <summary>
        /// All information required to play the level
        /// </summary>
        public MapNodeLevelData LevelData;
    }
}
