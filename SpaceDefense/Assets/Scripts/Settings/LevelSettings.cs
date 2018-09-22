//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelSettings.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A collection of level related settings
    /// </summary>
    public static class LevelSettings
    {
        /// <summary>
        /// Unity scene build index for the level select scene
        /// </summary>
        public const int LevelSelectSceneIndex = 0;

        /// <summary>
        /// The total  amount of time the player must defend for
        /// </summary>
        public const float TotalDefenseDuration = 30;

        /// <summary>
        /// Gets the unity scene build index for the given level
        /// </summary>
        /// <param name="levelId">Target level</param>
        /// <returns>The id for the given level</returns>
        public static int GetSceneIdForLevel(int levelId)
        {
            return levelId;
        }
    }
}
