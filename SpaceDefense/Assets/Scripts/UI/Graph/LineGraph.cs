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

        private float _widthEach;
        private float _heightPerPoint;

        private GameObject _lastDot;

        /// <summary>
        /// Adds a new dot 
        /// </summary>
        /// <param name="posX">The X position</param>
        /// <param name="posY">The Y position</param>
        /// <param name="prevDot">The previous position</param>
        private GameObject PlotNewDot(float posX, float posY, GameObject prevDot = null)
        {
            var newDot = Instantiate(this.DotPrefab, this.transform);
            newDot.transform.GetChild(0).GetComponent<Image>().color = this.GraphColor;
            newDot.transform.localPosition = new Vector3(posX, posY);
            if (prevDot)
            {
                var newLine = Instantiate(this.LinePrefab, this.transform);
                newLine.transform.GetChild(0).GetComponent<Image>().color = this.GraphColor;
                newLine.Connect(prevDot.transform.position, newDot.transform.position);
            }

            return newDot;
        }

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="data">target data to be represented</param>
        /// <param name="min">override min point of data</param>
        /// <param name="max">override max point of data</param>
        public override void DrawGraph(IList<float> data, float? min = null, float? max = null)
        {
            base.DrawGraph(data, min, max);

            var dataCount = data.Count;
            if (dataCount == 0)
            {
                this._doneDrawing = true;
            }
            else if (dataCount == 1)
            {
                this.PlotNewDot(this.Width / 2, this.Height * 0.8f);
                this._doneDrawing = true;
            }
            else
            {
                this._widthEach = this.Width / (dataCount - 1);
                this._heightPerPoint = this.Height / (this._maxData - this._minData) ;
            }
        }


        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="data">target data to be represented</param>
        protected override void DrawNext()
        {
            // Check if the end has been reached
            if (this._nextIndex >= this._data.Count)
            {
                this._doneDrawing = true;
                return;
            }

            var newData = this._data[this._nextIndex] - this._minData;
            var newDot = this.PlotNewDot(this._nextIndex * this._widthEach, newData * this._heightPerPoint, this._lastDot);
            this._lastDot = newDot;
            this._nextIndex++;
        }
    }
}
