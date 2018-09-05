//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="LevelSelectCamera.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Map;
    using Utils;

    /// <summary>
    /// A camera for the level select scene
    /// </summary>
    public class LevelSelectCamera : MonoBehaviour
    {
        /// <summary>
        /// The current instance of the <see cref="LevelSelectCamera"/> class
        /// </summary>
        public static LevelSelectCamera CurrentInstance { get; private set; }

        /// <summary>
        /// The node that the camera is focusing on
        /// </summary>
        [HideInInspector]
        public MapNodeBehavior TargetNode;

        /// <summary>
        /// Focuses onto  the level node
        /// </summary>
        public void OnSelectLevelNode(MapNodeBehavior targetNode)
        {
            this.TargetNode = targetNode;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            LevelSelectCamera.CurrentInstance = this;
        }

        /// <summary>
        /// Called at a set interval
        /// </summary>
        protected void Update()
        {
            if (this.TargetNode != null)
            {
                var oldPos = this.transform.position;
                var goalsPos = this.TargetNode.transform.position;
                this.transform.position = new Vector3(
                    Utils.Lerp(oldPos.x, goalsPos.x, 0.2f),
                    Utils.Lerp(oldPos.y, goalsPos.y, 0.2f),
                    Settings.GeneralSettings.CameraDefaultDepth);
            }
        }
    }
}
