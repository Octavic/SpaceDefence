//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNodeInfoPanel.cs">
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
    /// Displays the information about a specific map node
    /// </summary>
    public class MapNodeInfoPanel : MonoBehaviour
    {
        /// <summary>
        /// Renders the panel. Should be called only once until the node changes
        /// </summary>
        /// <param name="targetNode">Target node</param>
        public void Render(MapNode targetNode)
        {
            var allEnemies = targetNode.LevelData.SpawnPaths.Select(path => path.RegularSpawns.Select(regularSpawn => regularSpawn.Enemy));
        }

        /// <summary>
        /// Shows the panel
        /// </summary>
        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the panel
        /// </summary>
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
