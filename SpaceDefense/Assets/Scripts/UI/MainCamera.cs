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
        /// Gets the current instance of the <see cref="MainCamera"/> class
        /// </summary>
        public static MainCamera CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = FindObjectOfType<MainCamera>();
                }

                return _currentInstance;
            }
        }
        private static MainCamera _currentInstance;

        /// <summary>
        /// The multiplier that determines how much to shake based on the damage
        /// </summary>
        public float DamageToShakeIntensityRatio;

        /// <summary>
        /// How much screen shake we lose per fixed frame
        /// </summary>
        public float ShakeIntensityDecay;

        /// <summary>
        /// The minimal screen shake intensity. Going under this will result in camera resetting to still
        /// </summary>
        public float MinShakeIntensity;

        /// <summary>
        /// The shake intensity
        /// </summary>
        private float _shakeIntensity;

        /// <summary>
        /// The attached camera component
        /// </summary>
        private Camera _camera;

        /// <summary>
        /// Shakes the camera from damage being dealt
        /// </summary>
        public void ShakeFromDamage(float damage)
        {
            this._shakeIntensity += damage * this.DamageToShakeIntensityRatio;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._camera = GetComponentInChildren<Camera>();
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void FixedUpdate()
        {
            if (this._shakeIntensity < this.MinShakeIntensity)
            {
                return;
            }

            float randomAngleRad = (float)Utils.GlobalRandom.random.NextDouble() * Mathf.PI * 2;
            this._camera.transform.localPosition = new Vector3(
                Mathf.Cos(randomAngleRad) * this._shakeIntensity,
                Mathf.Sin(randomAngleRad) * this._shakeIntensity,
                0);
            this._shakeIntensity *= this.ShakeIntensityDecay;
        }
    }
}
