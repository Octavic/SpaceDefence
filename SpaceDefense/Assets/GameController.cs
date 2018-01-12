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

    /// <summary>
    /// Controls the overall game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// The current singleton instance of the <see cref="GameController"/> class
        /// </summary>
        public static GameController CurrentInstancce { get; private set; }

        /// <summary>
        /// Called when the given enemy successfully reached the end of their path
        /// </summary>
        /// <param name="enemy">Target enemy</param>
        public void OnEnemyReachEnd(Enemy enemy)
        {
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            GameController.CurrentInstancce = this;
        }
    }
}
