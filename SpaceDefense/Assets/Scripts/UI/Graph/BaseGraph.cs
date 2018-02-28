//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BaseGraph.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI.Graph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines the base class for all graphs
    /// </summary>
    public abstract class BaseGraph : MonoBehaviour
    {
        /// <summary>
        /// Height of the graph
        /// </summary>
        public float Height;

        /// <summary>
        /// Width of the graph
        /// </summary>
        public float Width;

        /// <summary>
        /// Color of the graph
        /// </summary>
        public Color GraphColor;

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="data">target data to be represented</param>
        public abstract void DrawGraph(IList<float> data);
    }
}
