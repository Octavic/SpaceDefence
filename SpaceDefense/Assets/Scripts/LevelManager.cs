//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelManager.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using Map;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Manages the current level
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the current instance of the <see cref="LevelManager"/> class
        /// </summary>
        public static LevelManager CurrentInstance { get; private set; }

        public MapNode CurrentLevel { get; private set; }
        private MapNodeBehavior _currentLevelBehavior;
            
        public void ShowLevelInfo(MapNodeBehavior level)
        {
            this._currentLevelBehavior = level;
            this.CurrentLevel = level.TargetNode;
            MapNodeInfoPanel.CurrentInstance.Render(level.TargetNode);
        }

        public void LaunchLevel()
        {
            if(!this._currentLevelBehavior.IsAvailable)
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
            LevelManager.CurrentInstance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }
}
