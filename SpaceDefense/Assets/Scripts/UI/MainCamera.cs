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
        /// Min and max zoom distance
        /// </summary>
        public float MinZoom;
        public float MaxZoom;

        /// <summary>
        /// The attached camera component
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// The old mouse position
        /// </summary>
        private Vector2? _oldPos;

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

            // Zoom
            if (Input.touchCount == 2)
            {
                var newDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
                if (this._oldDistance.HasValue)
                {
                    Debug.Log(this._camera.orthographicSize);
                    var newSize = this._camera.orthographicSize * newDistance / this._oldDistance.Value / 100;
                    newSize = Mathf.Max(newSize, this.MinZoom);
                    newSize = Mathf.Min(newSize, this.MaxZoom);
                    this._camera.orthographicSize = newSize;
                }

                this._oldDistance = newDistance;
            }
            // Pan
            else if (Input.touchCount == 1)
            {
                var newPos = Input.GetTouch(0).position;
                if (this._oldPos.HasValue)
                {
                    Debug.Log(this._camera.orthographicSize);
                    var diff = (newPos - this._oldPos.Value) * this._camera.orthographicSize;
                    this.transform.position -= new Vector3(diff.x, diff.y) * this.PanSpeed;
                }
                this._oldPos = newPos;
            }
            // Reset
            else if (Input.touchCount == 0)
            {
                this._oldPos = null;
                this._oldDistance = null;
            }
        }
    }
}
