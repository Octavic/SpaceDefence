//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameController.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets
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
        /// A saved state of the grid
        /// </summary>
        private MapGridState _gridState;

        /// <summary>
        /// The current singleton instance of the <see cref="GameController"/> class
        /// </summary>
        public static GameController CurrentInstancce { get; private set; }

        public void Save()
        {
            this._gridState = Grid.SaveState();
        }

        public void Load()
        {
            this.Grid.ResetBoard();
            this.Grid.TryLoadFromState(this._gridState);
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
            GameController.CurrentInstancce = this;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
        }
    }
}
