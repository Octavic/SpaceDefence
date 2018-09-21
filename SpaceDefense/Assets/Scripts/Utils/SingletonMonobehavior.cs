//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SingletonMonobehavior.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines the singleton behavior
    /// </summary>
    public class SingletonMonobehavior : MonoBehaviour
    {
        protected SingletonMonobehavior _currentInstance;
    }
}
