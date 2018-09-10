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
    using Map.Grid;
    using Map.Grid.States;
    using Map;

    /// <summary>
    /// The save data related to one map node
    /// </summary>
    [Serializable]
    public class MapNodeSaveData
    {
        /// <summary>
        /// ID for the level
        /// </summary>
        public int LevelId;

        /// <summary>
        /// The saved state for this level 
        /// </summary>
        public MapGridState SavedState = null;

        /// <summary>
        /// Best score achieved
        /// </summary>
        public float HighScore;

        /// <summary>
        /// If the level has been beat yet
        /// </summary>
        public bool IsBeat = false;

        /// <summary>
        /// The energy cost for the last run
        /// </summary>
        public float EnergyCost;

        /// <summary>
        /// A value between 0-1 that represents how well the player did.
        /// </summary>
        public List<MapNodeResources> Resources;
    }

    /// <summary>
    /// Defines the save file
    /// </summary>
    [Serializable]
    class SaveFile
    {
        /// <summary>
        /// All of the beat nodes and their progress
        /// </summary>
        public List<MapNodeSaveData> NodeProgress = new List<MapNodeSaveData>();

        /// <summary>
        /// What the player have in their inventory
        /// </summary>
        public Dictionary<ResourceType, float> Inventory = new Dictionary<ResourceType, float>();
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
        private Dictionary<ResourceType, float> ResoureceCapacity
        {
            get
            {
                var result = new Dictionary<ResourceType, float>();

                foreach (var node in this.CurrentSaveFile.NodeProgress)
                {
                    foreach (var resource in node.Resources)
                    {
                        var targetResource = resource.TargetResource;
                        if (!result.ContainsKey(targetResource))
                        {
                            result[targetResource] = 0;
                        }

                        result[targetResource] += resource.CapacityBoost;
                    }

                }

                return result;
            }
        }

        /// <summary>
        /// Save the state into a file
        /// </summary>
        /// <param name="mapNodeId">Id of the level</param>
        /// <param name="state">Target state to save</param>
        public void SaveMapGridState(int mapNodeId, MapGridState state)
        {
            var targetLevel = this.CurrentSaveFile.NodeProgress.Find(level => level.LevelId == mapNodeId);
            if (targetLevel != null)
            {
                targetLevel.SavedState = state;
            }
            else
            {
                var newLevelData = new MapNodeSaveData();
                newLevelData.LevelId = mapNodeId;
                newLevelData.SavedState = state;
                this.CurrentSaveFile.NodeProgress.Add(newLevelData);
            }

            this.Save();
        }

        /// <summary>
        /// Called when the level was completed
        /// </summary>
        /// <param name="data">The target level that was completed</param>
        /// <param name="defenseTime">How long the defense lasted</param>
        public void OnLevelComplete(MapNodeSaveData data, float defenseTime)
        {
            var existingData = this.GetLevelData(data.LevelId);
            data.HighScore = existingData == null ? data.HighScore : Math.Max(data.HighScore, existingData.HighScore);
            data.IsBeat = existingData.IsBeat || data.IsBeat;
            this.CurrentSaveFile.NodeProgress[data.LevelId] = data;
            this.AddIncome(defenseTime);

        }

        /// <summary>
        /// Updates the high score for the given level
        /// </summary>
        /// <param name="levelId">Target level</param>
        /// <param name="currentScore">The current score</param>
        /// <param name="didWin">If the player beat the level or not</param>
        public void OnLevelComplete(int levelId, float currentScore, bool didWin)
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

        public bool CanAfford(List<ResourceCost> totalCost)
        {
            var inventory = this.CurrentSaveFile.Inventory;
            foreach (var cost in totalCost)
            {
                float currentlyHave;
                if (!inventory.TryGetValue(cost.Resource, out currentlyHave))
                {
                    currentlyHave = 0;
                }

                if (cost.Amount > currentlyHave)
                {
                    return false;
                }
            }

            return true;
        }

        public bool TrySpendResource(List<ResourceCost> totalCost)
        {
            // First, ensure that we have enough resources to spend
            if (!this.CanAfford(totalCost))
            {
                return false;
            }

            // Can afford, spend and commit
            var inventory = this.CurrentSaveFile.Inventory;
            foreach (var cost in totalCost)
            {
                if (cost.Amount > 0)
                {
                    inventory[cost.Resource] -= cost.Amount;
                }
            }

            this.Save();
            return true;
        }

        /// <summary>
        /// Saves the current state
        /// </summary>
        private void Save()
        {
            var formatter = new BinaryFormatter();
            var targetFile = File.Open(this._savePath, FileMode.Create);

            formatter.Serialize(targetFile, this.CurrentSaveFile);
            targetFile.Close();
        }

        /// <summary>
        /// Loads from the save file
        /// </summary>
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
        /// Add income from the completed nodes
        /// </summary>
        /// <param name="defenseDuration">How long the defense last</param>
        private void AddIncome(float defenseDuration)
        {
            var inventory = this.CurrentSaveFile.Inventory;
            var capacity = this.ResoureceCapacity;

            foreach (var node in this.CurrentSaveFile.NodeProgress)
            {
                foreach (var resource in node.Resources)
                {
                    var targetResource = resource.TargetResource;
                    float newValue;
                    if (!inventory.ContainsKey(targetResource))
                    {
                        newValue = 0;
                    }
                    else
                    {
                        newValue = inventory[targetResource] + resource.ProduceAmount * defenseDuration;
                    }

                    float resourceCap;
                    if (!capacity.TryGetValue(targetResource, out resourceCap))
                    {
                        resourceCap = 0;
                    }

                    inventory[targetResource] = Math.Min(newValue, resourceCap);
                }
            }

            this.Save();
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
