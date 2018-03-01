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
        public abstract void DrawGraph(IList<float> data, float? min = null, float? max = null);

        /// <summary>
        /// Removes all items inside the graph
        /// </summary>
        protected void ClearDrawnGraph()
        {
            for (int i = this.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Gets the max data point
        /// </summary>
        /// <param name="data">Data points</param>
        /// <returns>The max data point</returns>
        protected float GetMax(IList<float> data)
        {
            if (data.Count == 0)
            {
                return 1;
            }

            var max = data.Max();

            return max == 0 ? 1 : max;
        }

        /// <summary>
        /// Gets the  min data point
        /// </summary>
        /// <param name="data">Data points</param>
        /// <returns>The min data point</returns>
        protected float GetMin(IList<float> data)
        {
            if (data.Count == 0)
            {
                return 0;
            }

            return data.Min();
        }
    }
}
