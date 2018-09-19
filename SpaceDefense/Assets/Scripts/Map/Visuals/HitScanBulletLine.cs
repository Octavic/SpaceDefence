//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="HitScanBulletLine.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Visuals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// The effect to indicate a fired bullet
    /// </summary>
    public class HitScanBulletLine : DelayedDestroy
    {
        /// <summary>
        /// Prefab for the bullet splash
        /// </summary>
        public HitScanBulletSplash SplashPrefab;

        /// <summary>
        /// Draws the bullet line
        /// </summary>
        /// <param name="emitter">The one who fired</param>
        /// <param name="hit">The bullet hit</param>
        public void DrawHit(Vector2 sourcePos, IList<RaycastHit2D> hits, Color? color = null)
        {
            // Draw hit
            var lastHit = hits.Last();
            var diff = lastHit.point - sourcePos;
            this.transform.localScale = new Vector3(diff.magnitude, 1, 1);
            this.transform.position = sourcePos + diff / 2;
            this.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);

            // Draw splash
            foreach (var hit in hits)
            {
                var splash = Instantiate(this.SplashPrefab);
                splash.transform.position = hit.point;
                splash.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg);
            }

            if (color != null)
            {
                this.GetComponent<SpriteRenderer>().color = color.Value;
                this.GetComponentInChildren<SpriteRenderer>().color = color.Value;
            }
        }
    }
}
