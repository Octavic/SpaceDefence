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
    using Enemies;

    /// <summary>
    /// Displays the information about a specific map node
    /// </summary>
    public class MapNodeInfoPanel : MonoBehaviour
    {
        /// <summary>
        /// The current instance
        /// </summary>
        public static MapNodeInfoPanel CurrentInstance
        {
            get
            {
                if(_currentInstance == null)
                {
                    _currentInstance = GameObject.FindObjectOfType<MapNodeInfoPanel>();
                }
                return _currentInstance;
            }
        }
        private static MapNodeInfoPanel _currentInstance;

        /// <summary>
        /// Renders the panel. Should be called only once until the node changes
        /// </summary>
        /// <param name="targetNode">Target node</param>
        public void Render(MapNode targetNode)
        {
            var allEnemies = new List<EnemyType>();
            foreach (var path in targetNode.LevelData.SpawnPaths)
            {
                allEnemies = allEnemies
                    .Concat(path.RegularSpawns.Select(spawn => spawn.Enemy))
                    .Concat(path.SpecialSpawns.Select(spawn => spawn.Enemy))
                    .ToList();
            }

            this.Show();
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

        /// <summary>
        /// Launches the level
        /// </summary>
        public void LaunchLevel()
        {
            this.Hide();
            LevelManager.CurrentInstance.LaunchLevel();
        }
    }
}
