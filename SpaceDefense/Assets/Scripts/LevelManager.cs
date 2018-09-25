//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelManager.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Map;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UI;
    using UI.MapNodeInfo;

    /// <summary>
    /// Manages the current level
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the current instance of the <see cref="LevelManager"/> class
        /// </summary>
        public static LevelManager CurrentInstance { get; private set; }

        /// <summary>
        /// The current level and current level mono behavior
        /// </summary>
        public MapNode CurrentLevel { get; private set; }
        private MapNodeBehavior _currentLevelBehavior;

        /// <summary>
        /// Shows the level information
        /// </summary>
        public void ShowLevelInfo(MapNodeBehavior level)
        {
            this._currentLevelBehavior = level;
            this.CurrentLevel = level.TargetNode;
            MapNodeInfoPanel.CurrentInstance.Render(level.TargetNode);
        }

        /// <summary>
        /// Launches the currently selected level
        /// </summary>
        public void LaunchLevel()
        {
            if (!this._currentLevelBehavior.IsAvailable)
            {
                Debug.Log("Unavailable level tried to launch: " + this.CurrentLevel.NodeId);
            }

            SceneManager.LoadScene("PlayScene");
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            if (LevelManager.CurrentInstance != null && LevelManager.CurrentInstance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            LevelManager.CurrentInstance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        /// <summary>
        /// Called  once per frame
        /// </summary>
        protected void Update()
        {
            // Called when left clicked
            if (Input.GetMouseButtonDown(0))
            {
                var mapNodes = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 15)
                    .Select(hit => hit.collider.GetComponent<MapNodeBehavior>())
                    .ToList();

                if (mapNodes.Count == 0)
                {
                    // User clicked on nothing
                    return;
                }

                var mapNode = mapNodes.First();
                if (mapNode != null)
                {
                    this._currentLevelBehavior = mapNode;
                    this.CurrentLevel = this._currentLevelBehavior.TargetNode;
                    MapNodeInfoPanel.CurrentInstance.Render(this.CurrentLevel);
                    LevelSelectCamera.CurrentInstance.OnSelectLevelNode(this._currentLevelBehavior);
                    return;
                }
            }
        }
    }
}
