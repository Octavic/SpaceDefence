//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ScreenBoundary.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines the screen boundary
    /// </summary>
    public class ScreenBoundary : MonoBehaviour, IHittable
    {
        /// <summary>
        /// Do nothing here
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="carriedEffects"></param>
        public void OnHit(float damage, IDictionary<EffectEnum, float> carriedEffects = null)
        {
            return;
        }
    }
}
