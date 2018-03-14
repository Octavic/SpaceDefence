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

        private float _widthEach;
        private float _heightPerPoint;
        
        /// <summary>
        /// Draws the next bar
        /// </summary>
        protected override void DrawNext()
        {
            if (this._nextIndex >= this._data.Count)
            {
                this._doneDrawing = true;
                return;
            }

            var dataPoint = this._data[this._nextIndex] - this._minData;
            if (dataPoint != 0)
            {
                var newBar = Instantiate(this.BarPrefab, this.transform);
                var barHeight = this._heightPerPoint * dataPoint;
                newBar.transform.GetChild(0).GetComponent<Image>().color = this.GraphColor;
                newBar.transform.localScale = new Vector3(this._widthEach, barHeight);
                newBar.transform.localPosition = new Vector3(this._widthEach * this._nextIndex + this._widthEach / 2, barHeight / 2);
            }

            this._nextIndex++;
        }

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="data">target data to be represented</param>
        public override void DrawGraph(IList<float> data, float? min = null, float? max = null)
        {
            base.DrawGraph(data, min, max);

            var dataCount = data.Count;
            if (dataCount == 0)
            {
                this._doneDrawing = true;
            }
            else
            {
                this._heightPerPoint = this.Height / (this._maxData - this._minData);
                this._widthEach = this.Width / data.Count;
            }
        }
    }
}
