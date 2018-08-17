//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelManager.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Map;

    /// <summary>
    /// Manages the current level
    /// </summary>
    public class LevelManager
    {
        /// <summary>
        /// Gets the current instance of the <see cref="LevelManager"/> class
        /// </summary>
        public static LevelManager CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = new LevelManager();
                }

                return _currentInstance;
            }
        }
        private static LevelManager _currentInstance;

        public MapNodeLevelData CurrentLevel { get; private set; }

        public void StartLevel(MapNode level)
        {

        }
    }
}
