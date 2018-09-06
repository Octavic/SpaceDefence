//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNodeDependencyBeam.cs">
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
    /// Defines a dependency beam that links map  nodes together
    /// </summary>
    public class MapNodeDependencyBeam : AttachableBeam
    {
        /// <summary>
        /// Links two map nodes together
        /// </summary>
        /// <param name="mustComplete">The source node that must be completed  to unlock this route</param>
        /// <param name="toUnlock">The destination node that will be unclocked once source node is completed</param>
        public void ShowDepdendency(MapNodeBehavior mustComplete, MapNodeBehavior toUnlock)
        {
            this.Attach(mustComplete.transform.position, toUnlock.transform.position);
            if(!mustComplete.TargetNode.SaveData.IsBeat)
            {
                this.GetComponentInChildren<SpriteRenderer>().color = new Color(0.7f, 0.7f, 0.7f);
            }
        }
    }
}
