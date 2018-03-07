//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UILine.cs">
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
    /// A single line that can be draw between two points
    /// </summary>
    public class UILine : MonoBehaviour
    {
        /// <summary>
        /// Moves the line so it connnects between two points
        /// </summary>
        /// <param name="pos1">Target position 1</param>
        /// <param name="pos2">Target position 2</param>
        public void Connect(Vector2 pos1, Vector2 pos2)
        {
            var diff = pos2 - pos1;
            var rect = this.GetComponent<RectTransform>();
            this.transform.localScale = new Vector3(diff.magnitude * 1.7f, 1, 1);
            this.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
            this.transform.position = (pos1 + pos2) / 2;
        }
    }
}
