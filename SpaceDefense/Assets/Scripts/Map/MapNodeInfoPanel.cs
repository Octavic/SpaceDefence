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
    using UnityEngine.UI;

    /// <summary>
    /// Displays the information about a specific map node
    /// </summary>
    public class MapNodeInfoPanel : MonoBehaviour
    {
        #region Unity links
        public Text LevelName;
        public Text HighScore;
        #endregion

        /// <summary>
        /// The current instance
        /// </summary>
        public static MapNodeInfoPanel CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
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
            this.LevelName.text = targetNode.Name;
            this.HighScore.text = targetNode.SaveData.HighScore == 0 ? "/" : ((int)(targetNode.SaveData.HighScore)).ToString();

            this.ShowPanel();
        }

        /// <summary>
        /// Shows the panel
        /// </summary>
        public void ShowPanel()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the panel
        /// </summary>
        public void HidePanel()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Launches the level
        /// </summary>
        public void LaunchLevel()
        {
            this.HidePanel();
            LevelManager.CurrentInstance.LaunchLevel();
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        public void Start()
        {
            MapNodeInfoPanel._currentInstance = this;
            this.gameObject.SetActive(false);
        }
    }
}
