//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNodeLevelData.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// All information about a level
    /// </summary>
    [Serializable]
    public class MapNodeLevelData
    {
        public int GridSizeX;
        public int GridSizeY;

        public List<SpawnPath> SpawnPaths;
    }
}
