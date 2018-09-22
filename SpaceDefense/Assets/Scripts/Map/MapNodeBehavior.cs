//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNodeBehavior.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;
    using Utils;

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
                return !this.TargetNode.LockedBy.Any(nodeId => !MapNodes[nodeId].TargetNode.SaveData.IsBeat);
            }
        }

        /// <summary>
        /// Called when the node was clicked 
        /// </summary>
        public void OnClickNode()
        {
            LevelManager.CurrentInstance.ShowLevelInfo(this);
            UI.LevelSelectCamera.CurrentInstance.OnSelectLevelNode(this);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            var nodeId = this.TargetNode.NodeId;
            if (MapNodeBehavior.MapNodes.ContainsKey(nodeId))
            {
                var existingNode = MapNodeBehavior.MapNodes[nodeId];
                if (existingNode != null)
                {
                    Debug.LogError("Duplicate map node id: " + nodeId);
                    return;
                }
            }

            MapNodeBehavior.MapNodes[nodeId] = this;
            StartCoroutine(this.DrawDependencyBeams());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IEnumerator DrawDependencyBeams()
        {
            yield return new WaitForSeconds(0.1f);
            this.TargetNode.SaveData = SaveManager.CurrentInstance.GetLevelData(this.TargetNode.NodeId);
            yield return new WaitForSeconds(0.1f);

            Color nodeColor;
            var efficiency = this.TargetNode.SaveData.Efficiency;
            if (!this.IsAvailable)
            {
                nodeColor = UISettings.UnavailableNodeColor;
            }
            else if (!this.TargetNode.SaveData.IsBeat)
            {
                nodeColor = UISettings.AvailableNodeColor;
            }
            else if (efficiency == 1)
            {
                nodeColor = UISettings.AcedNodeColor;
            }
            else
            {
                nodeColor = Lerp.LerpColor(UISettings.MinImperfectNodeColor, UISettings.MaxImprefectNodeColor, efficiency);
            }

            this.GetComponent<SpriteRenderer>().color = nodeColor;

            foreach (var lockedById in this.TargetNode.LockedBy)
            {
                var lockedByNode = MapNodes[lockedById];
                var routeVisual = Instantiate(this.DependencyBeamPrefab);
                routeVisual.Attach(this.transform.position, lockedByNode.transform.position);
                if (!lockedByNode.TargetNode.SaveData.IsBeat)
                {
                    routeVisual.GetComponentInChildren<SpriteRenderer>().color = UISettings.UnavailableNodeColor;
                }
            }
        }
    }
}
