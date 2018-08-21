//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelManager.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
        public static LevelManager CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new LevelManager();
                }

                return _currentInstance;
            }
        }
        private static LevelManager _currentInstance;

        public MapNode CurrentLevel { get; private set; }

        public void ShowLevelInfo(MapNode level)
        {
            this.CurrentLevel = level;
            MapNodeInfoPanel.CurrentInstance.Render(level);
        }

        public void LaunchLevel()
        {
            SceneManager.LoadScene("PlayScene");
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
    }
}
