//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AttachedBeam.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Wiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Describes a beam between two points
    /// </summary>
    public class AttachedBeam : MonoBehaviour
    {
        /// <summary>
        /// Sprite for when the beam is activated
        /// </summary>
        public Sprite ActivateSprite;

        /// <summary>
        /// Sprite for when the beam is inactive
        /// </summary>
        private Sprite _inactiveSprite;

        /// <summary>
        /// The sprite renderer component
        /// </summary>
        private SpriteRenderer _renderer;

        /// <summary>
        /// Attach the beam to two items
        /// </summary>
        /// <param name="posA">Position A</param>
        /// <param name="posB">Position B</param>
        public void Attach(Vector2 posA, Vector2 posB)
        {
            var diff = posB - posA;
            var length = diff.magnitude;
            var angle = Mathf.Atan2(diff.y, diff.x);
            this.transform.localScale = new Vector3(length, 1);
            this.transform.position = (posA + posB) / 2;
            this.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        }

        /// <summary>
        /// Called when the source input changes
        /// </summary>
        /// <param name="newState">The new state</param>
        public void Trigger(bool newState)
        {
            if (newState)
            {
                this._renderer.sprite = this.ActivateSprite;
            }
            else
            {
                this._renderer.sprite = this._inactiveSprite;
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._renderer = this.GetComponent<SpriteRenderer>();
            this._inactiveSprite = this._renderer.sprite;
        }
    }
}
