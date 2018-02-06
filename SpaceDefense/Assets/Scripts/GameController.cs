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

    /// <summary>
    /// Controls the overall game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
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
            Debug.Log("Enemy reached end!");
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
        }
    }
}
