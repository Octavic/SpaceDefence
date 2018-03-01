//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="BarGraph.cs">
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
    using UnityEngine.UI;

    /// <summary>
    /// Defines a bar chart
    /// </summary>
    public class BarGraph : BaseGraph
    {
        /// <summary>
        /// Prefab for the bar
        /// </summary>
        public GameObject BarPrefab;

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="data">target data to be represented</param>
        public override void DrawGraph(IList<float> data)
        {
            this.ClearDrawnGraph();

            // Nothing to plot
            if (data.Count == 0)
            {
                return;
            }

            // use 1.1x max as the top
            // pixel height of each data point = data / max * height
            // pixel / data = height / max
            var max = data.Max();
            if (max == 0)
            {
                max = 1;
            }
            var heightPerPoint = this.Height / (data.Max() * 1.1f);
            var count = data.Count > 1 ? data.Count : 2;
            var widthEach = this.Width / (count - 1);

            for (int i = 0; i < data.Count; i++)
            {
                var dataPoint = data[i];
                if (dataPoint == 0)
                {
                    continue;
                }

                var newBar = Instantiate(this.BarPrefab, this.transform);
                newBar.GetComponent<Image>().color = this.GraphColor;
                var barHeight = heightPerPoint * dataPoint;
                newBar.GetComponent<RectTransform>().localScale = new Vector3(widthEach / 100, barHeight / 100);
                newBar.transform.localPosition = new Vector3(widthEach * i + widthEach / 2, barHeight / 2);
            }
        }
    }
}
