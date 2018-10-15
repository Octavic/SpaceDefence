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
                    _currentInstance = GameObject.FindObjectOfType<SaveManager>();
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
                    if(node.GeneratingResources != null)
                    {
                        foreach (var resource in node.GeneratingResources)
                        {
                            var targetResource = resource.TargetResource;
                            if (!result.ContainsKey(targetResource))
                            {
                                result[targetResource] = 0;
                            }

                            result[targetResource] += resource.CapacityBoost;
                        }
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
            data.HighScore = Math.Max(data.HighScore, existingData.HighScore);
            data.IsBeat = existingData.IsBeat || data.IsBeat;
            this.CurrentSaveFile.NodeProgress[data.LevelId] = data;
            this.AddIncome(defenseTime);
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
                return new MapNodeSaveData();
            }

            return this.CurrentSaveFile.NodeProgress.Find(level => level.LevelId == levelId) ?? new MapNodeSaveData();
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
            this.UpdatePlayerInventoryUI();
            return true;
        }

        /// <summary>
        /// Sells the given entity
        /// </summary>
        /// <param name="entity">Target entity to be sold</param>
        /// <returns>The amount of resources gained</returns>
        public List<ResourceCost> SellItem(GridEntity entity)
        {
            var result = new List<ResourceCost>();
            foreach(var cost in entity.ManufactureCost)
            {
                var resource = cost.Resource;
                var amountRefunded = cost.Amount * Settings.GeneralSettings.RefundPriceRatio;

                var roomLeft = this.GetResourceCapacity(resource);
                var amountOwned = this.GetResourceOwnedAmount(resource);
                var maxAmount = amountOwned - roomLeft;

                var refundedCost = new ResourceCost();
                refundedCost.Resource = resource;
                refundedCost.Amount = Mathf.Min(amountRefunded, maxAmount);

                this.CurrentSaveFile.Inventory[resource] += refundedCost.Amount;
                result.Add(refundedCost);
            }

            return result;
        }

        private float GetResourceCapacity(ResourceType resource)
        {
            float result = 0;
            this.ResoureceCapacity.TryGetValue(resource, out result);
            return result;
        }

        private float GetResourceOwnedAmount(ResourceType resource)
        {
            float result = 0;
            this.CurrentSaveFile.Inventory.TryGetValue(resource, out result);
            return result;
        }

        /// <summary>
        /// Deletes the save file
        /// </summary>
        public void ResetSaveData()
        {
            File.Delete(this._savePath);
            this.Load();
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
        /// Updates the player inventory's UI to reflect changes
        /// </summary>
        private void UpdatePlayerInventoryUI()
        {
            UI.PlayerInventoryUI.Refresh(this.CurrentSaveFile.Inventory, this.ResoureceCapacity);
        }

        /// <summary>
        /// Add income from the completed nodes
        /// </summary>
        /// <param name="defenseDuration">How long the defense last</param>
        private void AddIncome(float defenseDuration)
        {
            var inventory = this.CurrentSaveFile.Inventory;
            var capacity = this.ResoureceCapacity;

            // Loop through  all nodes
            foreach (var node in this.CurrentSaveFile.NodeProgress)
            {
                // Calculate resource income
                foreach (var resource in node.GeneratingResources)
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
            this.UpdatePlayerInventoryUI();
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            if(CurrentInstance != null && CurrentInstance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            this.UpdatePlayerInventoryUI();
        }
    }
}
