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
        /// The node that this behavior represents
        /// </summary>
        public MapNode TargetNode;

        /// <summary>
        /// Called when the node was clicked 
        /// </summary>
        public void OnCliCkNode()
        {
            LevelManager.CurrentInstance.ShowLevelInfo(this.TargetNode);
        }
    }
}
