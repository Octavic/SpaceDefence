//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameController.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;

    /// <summary>
    /// Controls the overall game flow
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public Vector2 PosA;
        public Vector2 PosB;
        public AttachedBeam Beam;

        protected void Update()
        {
            Beam.Attach(this.PosA, this.PosB);
        }
    }
}
