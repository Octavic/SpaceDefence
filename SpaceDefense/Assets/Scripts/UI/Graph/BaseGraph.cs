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
        /// How much time to wait between drawing
        /// </summary>
        public float DrawInterval;

        /// <summary>
        /// How much time has passed since the last time the graph was drawn
        /// </summary>
        private float _timeSinceDraw = 0;

        /// <summary>
        /// If the graph is done rendering
        /// </summary>
        protected bool _doneDrawing = true;

        /// <summary>
        /// The data to be drawn
        /// </summary>
        protected IList<float> _data;

        /// <summary>
        /// Index of next point to be drawn
        /// </summary>
        protected int _nextIndex;

        /// <summary>
        /// Min and max for the data
        /// </summary>
        protected float _minData;
        protected float _maxData;

        /// <summary>
        /// Draws the graph
        /// </summary>
        /// <param name="data">target data to be represented</param>
        public virtual void DrawGraph(IList<float> data, float? min = null, float? max = null)
        {
            this.ClearDrawnGraph();
            this._data = new List<float>(data);
            this._doneDrawing = false;
            this._timeSinceDraw = 0;
            this._nextIndex = 0;
            this._minData = min ?? this.GetMin(data);
            this._maxData = max ?? this.GetMax(data);
        }

        /// <summary>
        /// Draw the next item
        /// </summary>
        protected abstract void DrawNext();

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

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (!this._doneDrawing)
            {
                this._timeSinceDraw += Time.deltaTime;
                if (this._timeSinceDraw > this.DrawInterval)
                {
                    this.DrawNext();
                    this._timeSinceDraw -= this.DrawInterval;
                }
            }
        }
    }
}
