//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNodeBehavior.cs">
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
    /// Defines the unity behavior of a map node
    /// </summary>
    public class MapNodeBehavior : MonoBehaviour
    {
        /// <summary>
        /// A hash of map node name => map node behavior
        /// </summary>
        public static Dictionary<int, MapNodeBehavior> MapNodes = new Dictionary<int, MapNodeBehavior>();

        /// <summary>
        /// Prefab for the line segment  to show map node dependency
        /// </summary>
        public MapNodeDependencyBeam DependencyBeamPrefab;

        /// <summary>
        /// The node that this behavior represents
        /// </summary>
        public MapNode TargetNode;

        /// <summary>
        /// If the map node is available
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                if (!this._isAvailable.HasValue)
                {
                    this._isAvailable = !this.TargetNode.LockedBy.Any(nodeId => !MapNodes[nodeId].TargetNode.SaveData.IsBeat);
                }

                return this._isAvailable.Value;
            }
        }
        private bool? _isAvailable;

        /// <summary>
        /// Called when the node was clicked 
        /// </summary>
        public void OnClickNode()
        {
            LevelManager.CurrentInstance.ShowLevelInfo(this);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            var nodeId = this.TargetNode.NodeId;
            if (MapNodeBehavior.MapNodes.ContainsKey(nodeId))
            {
                Debug.LogError("Duplicate map node id: " + nodeId);
                return;
            }

            MapNodeBehavior.MapNodes[nodeId] = this;

            foreach (var lockedById in this.TargetNode.LockedBy)
            {
                var lockedByNode = MapNodes[lockedById];
                var routeVisual = Instantiate(this.DependencyBeamPrefab);
                routeVisual.Attach(this.transform.position, lockedByNode.transform.position);
            }
        }
    }
}
