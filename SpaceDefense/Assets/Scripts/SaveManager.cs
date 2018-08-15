//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SaveManager.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using Grid;
    using Grid.States;

    [Serializable]
    public class MapNodeSaveData
    {
        public int LevelId;
        public MapGridState SavedState = null;
        public float HighScore;
        public bool IsBeat = false;

        /// <summary>
        /// A value between 0-1 that represents how well the player did.
        /// </summary>
        public float Efficiency;
    }

    [Serializable]
    class SaveFile
    {
        public List<MapNodeSaveData> NodeProgress = new List<MapNodeSaveData>();
    }

    /// <summary>
    /// Describes a save manager
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the current instance of the <see cref="SaveManager"/> class
        /// </summary>
        public static SaveManager CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    var saveManagerObject = GameObject.FindGameObjectWithTag(Tags.SaveManager);

                    // Only happens when the game is launched from a certain level. Happens during development only
                    if (saveManagerObject == null)
                    {
                        saveManagerObject = new GameObject();
                        saveManagerObject.AddComponent<SaveManager>();
                    }

                    _currentInstance = saveManagerObject.GetComponent<SaveManager>();
                }

                return _currentInstance;
            }
        }
        private static SaveManager _currentInstance;

        /// <summary>
        /// The save path
        /// </summary>
        private string _savePath;

        /// <summary>
        /// The current save file
        /// </summary>
        private SaveFile CurrentSaveFile
        {
            get
            {
                if (this._currentSaveFile == null)
                {
                    this._savePath = Application.persistentDataPath + "/save.dat";
                    this.Load();
                }

                return this._currentSaveFile;
            }
        }
        private SaveFile _currentSaveFile;

        /// <summary>
        /// Save the state into a file
        /// </summary>
        /// <param name="levelId">Id of the level</param>
        /// <param name="state">Target state to save</param>
        public void SaveLevelData(int levelId, MapGridState state)
        {
            var targetLevel = this.CurrentSaveFile.NodeProgress.Find(level => level.LevelId == levelId);
            if (targetLevel != null)
            {
                targetLevel.SavedState = state;
            }
            else
            {
                var newLevelData = new MapNodeSaveData();
                newLevelData.LevelId = levelId;
                newLevelData.SavedState = state;
                this.CurrentSaveFile.NodeProgress.Add(newLevelData);
            }

            this.Save();
        }

        /// <summary>
        /// Updates the high score for the given level
        /// </summary>
        /// <param name="levelId">Target level</param>
        /// <param name="currentScore">The current score</param>
        /// <param name="didWin">If the player beat the level or not</param>
        public void OnLevelEnd(int levelId, float currentScore, bool didWin)
        {
            var targetLevel = this.CurrentSaveFile.NodeProgress.Find(level => level.LevelId == levelId);
            if (targetLevel != null)
            {
                targetLevel.HighScore = Mathf.Max(targetLevel.HighScore, currentScore);
                targetLevel.IsBeat = targetLevel.IsBeat || didWin;
            }
            else
            {
                var newLevelData = new MapNodeSaveData();
                newLevelData.LevelId = levelId;
                newLevelData.HighScore = currentScore;
                newLevelData.IsBeat = didWin;
                this.CurrentSaveFile.NodeProgress.Add(newLevelData);
            }

            this.Save();
        }

        /// <summary>
        /// Gets the save data for the given level
        /// </summary>
        /// <param name="levelId">target level</param>
        /// <returns>Level data for the target level</returns>
        public MapNodeSaveData GetLevelData(int levelId)
        {
            // Lazy initialization
            if (this.CurrentSaveFile == null)
            {
                this.Load();
            }

            if (this.CurrentSaveFile == null)
            {
                return null;
            }

            return this.CurrentSaveFile.NodeProgress.Find(level => level.LevelId == levelId);
        }

        /// <summary>
        /// Loads the saved state from data
        /// </summary>
        /// <param name="levelId">Id of the level</param>
        /// <returns>The save state, null if none</returns>
        public MapGridState LoadLevelState(int levelId)
        {
            var levelData = this.GetLevelData(levelId);
            if (levelData != null)
            {
                return levelData.SavedState;
            }
            else
            {
                return null;
            }
        }

        private void Save()
        {
            var formatter = new BinaryFormatter();
            var targetFile = File.Open(this._savePath, FileMode.Create);

            formatter.Serialize(targetFile, this.CurrentSaveFile);
            targetFile.Close();
        }

        private void Load()
        {
            FileStream targetFile = null;
            try
            {
                var formatter = new BinaryFormatter();
                targetFile = File.Open(this._savePath, FileMode.Open);

                var result = (SaveFile)formatter.Deserialize(targetFile);
                this._currentSaveFile = result;
            }
            catch
            {
                Debug.Log("No save data");
                this._currentSaveFile = new SaveFile();
            }
            finally
            {
                if (targetFile != null)
                {
                    targetFile.Close();
                }
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
