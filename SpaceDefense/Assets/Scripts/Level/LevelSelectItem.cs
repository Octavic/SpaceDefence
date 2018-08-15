//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelSelectItem.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.LevelSelector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Renders an item showing a certain level in the level selection screen
    /// </summary>
    public class LevelSelectItem : MonoBehaviour
    {
        /// <summary>
        /// The  level that this item represents
        /// </summary>
        public int LevelId;

        /// <summary>
        /// The button used to select the level
        /// </summary>
        public Button LevelSelectButton;

        /// <summary>
        /// The highscore for this level
        /// </summary>
        public Text HighScore;

        private MapNodeSaveData CurrentLevelData
        {
            get
            {
                if (this._currentLevelData == null)
                {
                    this._currentLevelData = SaveManager.CurrentInstance.GetLevelData(this.LevelId);
                }

                return this._currentLevelData;
            }
        }
        private MapNodeSaveData _currentLevelData;

        /// <summary>
        /// Gets a value representing if this level is available
        /// </summary>
        private bool IsAvailable
        {
            get
            {
                // First level is always available
                if (LevelId == 1)
                {
                    return true;
                }

                var prevLevel = SaveManager.CurrentInstance.GetLevelData(this.LevelId - 1);
                
                // Has not even been attempted
                if (prevLevel == null)
                {
                    return false;
                }

                return prevLevel.IsBeat;
            }
        }

        /// <summary>
        /// Called when the player selected this level to play
        /// </summary>
        public void OnStartLevel()
        {
            SceneManager.LoadScene(Settings.LevelSettings.GetSceneIdForLevel(this.LevelId));
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            var levelData = this.CurrentLevelData;
            this.HighScore.text = levelData != null ? levelData.HighScore.ToString() : "";
            this.LevelSelectButton.interactable = this.IsAvailable;
        }
    }
}
