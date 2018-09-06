//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="IHittable.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines an object that can be hit
    /// </summary>
    public interface IHittable
    {
        void OnHit(float damage, IDictionary<EffectEnum, float> carriedEffects = null);
    }
}
