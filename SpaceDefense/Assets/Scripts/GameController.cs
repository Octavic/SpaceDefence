//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameController.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;
    using Wiring;
    using Grid;
    using UI.Graph;
    using Settings;
    using UI.Shop;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Controls the overall game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Level of the id. Must be unique across all levels 
        /// </summary>
        public int LevelId;

        /// <summary>
        /// The total core health
        /// </summary>
        public float TotalCoreHealth;

        /// <summary>
        /// How long to defend for
        /// </summary>
        public float TotalDefenseDuration;

        #region Unity object links
        /// <summary>
        /// Text for the score number and time
        /// </summary>
        public Text ScoreText;
        public Text TimeText;

        /// <summary>
        /// The item shop game object
        /// </summary>
        public ShopMenu ItemShop;

        /// <summary>
        /// Save, reload, undo buttons 
        /// </summary>
        public GameObject Buttons;

        public GameObject SetLookModeButton;
        public GameObject SetBuildModeButton;
        public GameObject SetFightModeButton;
        public GameObject AbortFightButton;

        /// <summary>
        /// The game over screen 
        /// </summary>
        public GameOverScreen GameOverScreenObject;
        #endregion

        public float CurrentCoreHealth
        {
            get
            {
                return this._currentCoreHealth;
            }
            private set
            {
                if (value <= 0)
                {
                    value = 0;
                }

                this._currentCoreHealth = value;
            }
        }
        private float _currentCoreHealth;

        /// <summary>
        /// How many seconds has passed since the fighting started
        /// </summary>
        public float TimeSinceFightStart { get; private set; }

        /// <summary>
        /// Gets the current game phase
        /// </summary>
        public GamePhases CurrentPhasee
        {
            get
            {
                return this._currentPhase ;
            }
            set
            {
                // Cache state
                var isBuild = value == GamePhases.Build;
                var isLook = value == GamePhases.Look;
                var isFight = value == GamePhases.Fight;

                // Toggle visibility
                this.ItemShop.gameObject.SetActive(isBuild);
                this.Buttons.SetActive(isBuild);

                //this.SetLookModeButton.SetActive(isBuild);
                this.SetBuildModeButton.SetActive(isLook);
                this.SetFightModeButton.SetActive(!isFight);
                this.AbortFightButton.SetActive(isFight);

                // Reset score
                this._curIncome = 0;
                this._curCost = 0;
                this.UpdateScore();
                this._incomeData = new List<float>();
                this._costData = new List<float>();
                this._scoreData = new List<float>();

                // Reset timer
                this.TimeSinceFightStart = 0;

                // Reset core health
                this.CurrentCoreHealth = this.TotalCoreHealth;

                // Update spawn manager
                SpawnManager.CurrntInstance.OnGamePhaseChange(value);

                this._isGameOver = false;
                
                this._currentPhase = value;
            }
        }

        /// <summary>
        /// The current game phase
        /// </summary>
        private GamePhases _currentPhase;

        /// <summary>
        /// How often data is collected
        /// </summary>
        private float _collectDataInterval;

        /// <summary>
        /// How much time passed since last time data was collected
        /// </summary>
        private float _timeSinceCollect;

        private List<float> _incomeData = new List<float>();
        private List<float> _costData = new List<float>();
        private List<float> _scoreData = new List<float>();
        private List<float> _coreHealthData = new List<float>();

        private float _curIncome = 0;
        private float _curCost = 0;

        private bool _isGameOver = false;

        /// <summary>
        /// The current grid object
        /// </summary>
        public MapGrid Grid;

        /// <summary>
        /// The current singleton instance of the <see cref="GameController"/> class
        /// </summary>
        public static GameController CurrentInstance
        {
            get
            {
                if (_currentInstane == null)
                {
                    _currentInstane = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
                }
                return _currentInstane;
            }
        }
        private static GameController _currentInstane;

        public void ReturnToLevelSelect()
        {
            SceneManager.LoadScene(LevelSettings.LevelSelectSceneIndex);
        }

        public void SetLookPhase()
        {
            this.CurrentPhasee = GamePhases.Look;
        }
        public void SetBuildPhase()
        {
            this.CurrentPhasee = GamePhases.Build;
        }
        public void SetFightPhase()
        {
            this.Save();
            this.CurrentPhasee = GamePhases.Fight;
        }

        public void Save()
        {
            SaveManager.CurrentInstance.SaveLevelData(this.LevelId, Grid.SaveState());
        }

        public void Load()
        {
            this.Grid.ResetBoard();
            var levelData = SaveManager.CurrentInstance.GetLevelData(this.LevelId);
            if (levelData != null && levelData.SavedState != null)
            {
                this.Grid.TryLoadFromState(levelData.SavedState);
            }
        }

        /// <summary>
        /// Adds costs when firing something
        /// </summary>
        /// <param name="cost">How much cost</param>
        public void AddCost(float cost)
        {
            this._curCost += cost;
        }

        /// <summary>
        /// Adds income when enemies dies
        /// </summary>
        /// <param name="income">How much income</param>
        public void AddIncome(float income)
        {
            this._curIncome += income;
        }

        /// <summary>
        /// Called when the given enemy successfully reached the end of their path
        /// </summary>
        /// <param name="enemy">Target enemy</param>
        public void OnEnemyReachEnd(Enemy enemy)
        {
            Destroy(enemy.gameObject);
            this.CurrentCoreHealth -= enemy.Worth;
        }

        private void UpdateScore()
        {
            this.ScoreText.text = ((int)(this._curIncome - this._curCost)).ToString();
            this.TimeText.text = ((int)(this.TimeSinceFightStart)).ToString();
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            _currentInstane = this;
            this.CurrentCoreHealth = this.TotalCoreHealth;
            this._currentPhase = GamePhases.Look;
            this._collectDataInterval = this.TotalDefenseDuration / GeneralSettings.EndGraphSections;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (this.CurrentPhasee == GamePhases.Fight && !this._isGameOver)
            {
                this.TimeSinceFightStart += Time.deltaTime;
                if (this.TimeSinceFightStart > this.TotalDefenseDuration)
                {
                    this.OnGameOver(true);
                }

                this.UpdateScore();

                this._timeSinceCollect += Time.deltaTime;

                // Collect data every x seconds
                if (this._timeSinceCollect >= this._collectDataInterval)
                {
                    this._timeSinceCollect -= this._collectDataInterval;

                    this._incomeData.Add(this._curIncome);
                    this._costData.Add(this._curCost);
                    this._coreHealthData.Add(this.CurrentCoreHealth);
                    this._scoreData.Add(this._curIncome - this._curCost);
                }
            }   
        }

        /// <summary>
        /// Called when the game is over
        /// </summary>
        private void OnGameOver(bool didWin)
        {
            this._isGameOver = true;
            var finalScore = this._scoreData.Last();
            SaveManager.CurrentInstance.OnLevelEnd(this.LevelId, finalScore, didWin);
            this.GameOverScreenObject.gameObject.SetActive(true);
            var enemyPassPenalty = (this.TotalCoreHealth - this._coreHealthData.Last()) * GeneralSettings.EnemySurvivalPenaltyMultiplier;
            this.GameOverScreenObject.OnGameOver(didWin, enemyPassPenalty, this._incomeData, this._costData, this._coreHealthData);
            this.SetBuildPhase();
        }
    }
}
