//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MainCamera.cs">
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

    /// <summary>
    /// Defines the main camera
    /// </summary>
    public class MainCamera : MonoBehaviour
    {
        /// <summary>
        /// How fast the camera pans
        /// </summary>
        public float PanSpeed;

        /// <summary>
        /// How fast the camera zooms
        /// </summary>
        public float ZoomSpeed;

        /// <summary>
        /// The attached camera component
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The old mouse position
        /// </summary>
        private Vector2? _oldMousePos;

        /// <summary>
        /// The old distance between two fingers (Mobile touch only)
        /// </summary>
        private float? _oldDistance;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._camera = this.GetComponent<Camera>();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            // During build phase, only pan camera if the player is  dragging something 
            if (GameController.CurrentInstance.CurrentPhasee == GamePhases.Build)
            {
                return;
            }
        }
    }
}
