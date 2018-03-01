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
    using Wiring;
    using Grid;
    using UI.Graph;

    /// <summary>
    /// Controls the overall game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// Graphs
        /// </summary>
        public LineGraph CostGraph;
        public LineGraph IncomeGraph;
        public BarGraph ScoreBar;

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

        private float _prevIncome = 0;
        private float _prevCost = 0;

        private float _curIncome = 0;
        private float _curCost = 0;

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
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            _currentInstane = this;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            this._timeSinceCollect += Time.deltaTime;
            if (this._timeSinceCollect >= this.CollectDataInterval)
            {
                this._timeSinceCollect -= this.CollectDataInterval;

                this._incomeData.Add(this._curIncome);
                this._costData.Add(this._curCost);
                this._scoreData.Add(this._curIncome - this._curCost);

                if (this._incomeData.Count > 20)
                {
                    this._incomeData.RemoveAt(0);
                    this._costData.RemoveAt(0);
                    this._scoreData.RemoveAt(0);
                }

                this.IncomeGraph.DrawGraph(this._incomeData);
                this.CostGraph.DrawGraph(this._costData);
                this.ScoreBar.DrawGraph(this._scoreData);

                this._prevCost = this._curCost;
                this._prevIncome = this._curIncome;
            }
        }
    }
}
