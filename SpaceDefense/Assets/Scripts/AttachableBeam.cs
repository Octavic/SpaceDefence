//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AttachableBeam.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines a beam that can be attached to two positions
    /// </summary>
    public class AttachableBeam : MonoBehaviour
    {
        /// <summary>
        /// Attach the beam to two items
        /// </summary>
        /// <param name="posA">Position A</param>
        /// <param name="posB">Position B</param>
        public void Attach(Vector2 posA, Vector2 posB)
        {
            var diff = posB - posA;
            var length = diff.magnitude;
            var angle = Mathf.Atan2(diff.y, diff.x);
            this.transform.localScale = new Vector3(length, 1);
            this.transform.position = (posA + posB) / 2;
            this.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        }
    }
}
