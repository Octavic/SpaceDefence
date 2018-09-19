//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Projectile.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Visuals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines an object that will self destroy after the given seconds
    /// </summary>
    public class DelayedDestroy : MonoBehaviour
    {
        /// <summary>
        /// How many seconds to wait before destroy happens
        /// </summary>
        public float Duration;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected virtual void Start()
        {
            StartCoroutine(this.Destroy());
        }

        /// <summary>
        /// Delayed self destroy
        /// </summary>
        /// <returns></returns>
        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(this.Duration);
            GameObject.Destroy(this.gameObject);
        }
    }
}
