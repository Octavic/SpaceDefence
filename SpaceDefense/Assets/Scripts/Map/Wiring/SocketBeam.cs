//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SocketBeam.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Describes a beam between two points
    /// </summary>
    public class SocketBeam : AttachableBeam
    {
        /// <summary>
        /// Sprite for when the beam is activated
        /// </summary>
        public Sprite ActivateSprite;

        /// <summary>
        /// Sprite for when the beam is inactive
        /// </summary>
        public Sprite InactiveSprite;

        /// <summary>
        /// The sprite renderer component
        /// </summary>
        public SpriteRenderer Renderer;

        /// <summary>
        /// Called when the source input changes
        /// </summary>
        /// <param name="newState">The new state</param>
        public void Trigger(bool newState)
        {
            if (newState)
            {
                this.Renderer.sprite = this.ActivateSprite;
            }
            else
            {
                this.Renderer.sprite = this.InactiveSprite;
            }
        }
    }   
}
