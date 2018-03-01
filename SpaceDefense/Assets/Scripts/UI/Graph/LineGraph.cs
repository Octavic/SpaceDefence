//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LineGraph.cs">
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
    /// Describes a line graph
    /// </summary>
    public class LineGraph : BaseGraph
    {
        /// <summary>
        /// Prefab for the lines
        /// </summary>
        public UILine LinePrefab;

        /// <summary>
        /// Prefab for the dot 
        /// </summary>
        public GameObject DotPrefab;

        /// <summary>
        /// Adds a new dot 
        /// </summary>
        /// <param name="posX">The X position</param>
        /// <param name="posY">The Y position</param>
        /// <param name="prevDot">The previous position</param>
        private GameObject PlotNewDot(float posX, float posY, GameObject prevDot = null)
        {
            var newDot = Instantiate(this.DotPrefab, this.transform);
            newDot.GetComponent<Image>().color = this.GraphColor;
            newDot.transform.localPosition = new Vector3(posX, posY);
            if (prevDot)
            {
                var newLine = Instantiate(this.LinePrefab, this.transform);
                newLine.GetComponent<Image>().color = this.GraphColor;
                newLine.Connect(prevDot.transform.position, newDot.transform.position);
            }

            return newDot;
        }

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

            // Single dot graph
            if (data.Count == 1)
            {
                this.PlotNewDot(this.Width / 2, this.Height * 0.8f);
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
            var pixelPerPoint = this.Height / (max * 1.1f);
            var widthEach = this.Width / (data.Count - 1);

            GameObject prevDot = null;

            for (int i = 0; i < data.Count; i++)
            {
                var curData = data[i];
                prevDot = this.PlotNewDot(widthEach * i, curData * pixelPerPoint, prevDot);
            }
        }
    }
}
