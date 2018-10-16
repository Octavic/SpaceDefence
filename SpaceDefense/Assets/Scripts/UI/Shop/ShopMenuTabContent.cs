//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ShopMenuTabContent.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI.Shop
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Defines a group of buttons in the shop menu
    /// </summary>
    public class ShopMenuTabContent : MonoBehaviour
    {
        /// <summary>
        /// If the content is still moving
        /// </summary>
        private bool _isLerping;

        /// <summary>
        /// Where to lerp to
        /// </summary>
        private Vector3 _destination;

        /// <summary>
        /// Moves the content to the destination
        /// </summary>
        /// <param name="localDestination">Destination in local position</param>
        public void LerpTo(Vector3 localDestination)
        {
            this._isLerping = true;
            this._destination = localDestination;
            this.StartCoroutine(this.StopLerpingAfter());
        }

        /// <summary>
        /// Called at set intervals
        /// </summary>
        protected void FixedUpdate()
        {
            if(this._isLerping)
            {
                var oldPos = this.transform.localPosition;
                this.transform.localPosition = Vector3.Lerp(oldPos, this._destination, 0.4f);
            }
        }

        private IEnumerator StopLerpingAfter()
        {
            yield return new WaitForSeconds(1.5f);
            this._isLerping = false;
            this.transform.localPosition = this._destination;
        }
    }
}
