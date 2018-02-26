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
        public LineGraph graphA;
        public LineGraph graphB;

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
            this.graphA.DrawGraph(new List<float>() { 0, 1, 2, 3, 4, 5, 4.5f });
            this.graphB.DrawGraph(new List<float>() { 3, 1, 2, 4.5f, 3, 1, 2.5f });
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
        }
    }
}
