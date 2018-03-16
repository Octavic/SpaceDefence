﻿//  --------------------------------------------------------------------------------------------------------------------
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

    /// <summary>
    /// Controls the overall game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        #region Unity object links
        /// <summary>
        /// Text for the score number
        /// </summary>
        public Text ScoreText;

        /// <summary>
        /// The item shop game object
        /// </summary>
        public GameObject ItemShop;

        public GameObject SetLookModeButton;
        public GameObject SetBuildModeButton;
        public GameObject SetFightModeButton;
        public GameObject AbortFightButton;

        /// <summary>
        /// The game over screen 
        /// </summary>
        public GameOverScreen GameOverScreenObject;
        #endregion

        public float TotalCoreHealth;

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
                this.ItemShop.SetActive(isBuild);
                //this.SetLookModeButton.SetActive(isBuild);
                this.SetBuildModeButton.SetActive(isLook);
                this.SetFightModeButton.SetActive(!isFight);
                this.AbortFightButton.SetActive(isFight);

                // Reset score
                this._curIncome = 0;
                this._curCost = 0;
                this.UpdateScore();

                // Reset timer
                this.TimeSinceFightStart = 0;

                // Reset core health
                this.CurrentCoreHealth = this.TotalCoreHealth;

                // Update spawn manager
                SpawnManager.CurrntInstance.OnGamePhaseChange(value);

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
        public float CollectDataInterval;

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

        private bool isGameOver = false;

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
            this.CurrentPhasee = GamePhases.Fight;
        }

        public void Save()
        {
            SaveManager.CurrentInstance.SaveData(Grid.SaveState());
        }

        public void Load()
        {
            this.Grid.ResetBoard();
            var savedState = SaveManager.CurrentInstance.LoadState();
            if (savedState != null)
            {
                this.Grid.TryLoadFromState(savedState);
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
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            _currentInstane = this;
            this.CurrentCoreHealth = this.TotalCoreHealth;
            this._currentPhase = GamePhases.Look;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (this.CurrentPhasee == GamePhases.Fight && !this.isGameOver)
            {
                this.TimeSinceFightStart += Time.deltaTime;
                if (this.TimeSinceFightStart > GeneralSettings.TotalDefendDuration)
                {
                    this.OnGameOver(true);
                }

                this.UpdateScore();
            }

            this._timeSinceCollect += Time.deltaTime;

            // Collect data every x seconds
            if (this._timeSinceCollect >= this.CollectDataInterval)
            {
                this._timeSinceCollect -= this.CollectDataInterval;

                this._incomeData.Add(this._curIncome);
                this._costData.Add(this._curCost);
                this._coreHealthData.Add(this.CurrentCoreHealth);
            }
        }

        /// <summary>
        /// Called when the game is over
        /// </summary>
        private void OnGameOver(bool didWin)
        {
            this.isGameOver = true;
            this.GameOverScreenObject.gameObject.SetActive(true);
            var enemyPassPenalty = (this.TotalCoreHealth - this._coreHealthData.Last()) * GeneralSettings.EnemySurvivalPenaltyMultiplier;
            this.GameOverScreenObject.OnGameOver(didWin, enemyPassPenalty, this._incomeData, this._costData, this._coreHealthData);
            this.SetBuildPhase();
        }
    }
}
