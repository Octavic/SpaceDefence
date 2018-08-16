//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelData.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// All information about a levels
    /// </summary>
    [Serializable]
    public class LevelData
    {
        public int GridSizeX;
        public int GridSizeY;

        public List<SpawnPath> SpawnPaths;
    }
}
