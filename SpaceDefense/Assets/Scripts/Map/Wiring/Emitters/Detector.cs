//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Detector.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Emitters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines a beam/cone/etc detector
    /// </summary>
    public class Detector : Emitter
    {
        /// <summary>
        /// The object that collides with enemies
        /// </summary>
        public DetectorArea Area;
    }
}
