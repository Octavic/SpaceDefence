//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameObjectExtensions.cs">
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
    /// Extends gameobject functionality
    /// </summary>
    public static class GameObjectExtensions
    {
        public static void DestroyAllChildren(this GameObject gameobject)
        {
            var children = gameobject.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                GameObject.Destroy(gameobject.transform.GetChild(i));
            }
        }
    }
}
