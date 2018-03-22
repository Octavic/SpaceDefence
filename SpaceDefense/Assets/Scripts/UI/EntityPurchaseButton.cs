//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EntityPurchaseButton.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Grid;
    using UnityEngine;

    /// <summary>
    /// Defines a button to purchase entities with
    /// </summary>
    public class EntityPurchaseButton : MonoBehaviour
    {
        /// <summary>
        /// Target entity prefab to be spawned when purchased
        /// </summary>
        public GridEntity TargetEntityPrefab;

        /// <summary>
        /// The price of purchase
        /// </summary>
        public float Price;

        /// <summary>
        /// TODO: add price check and what not
        /// </summary>
        public void OnPurchase()
        {
            if (!PlayerController.CanPurchase)
            {
                return;
            }
            var newEntity = Instantiate(this.TargetEntityPrefab);
            PlayerController.CurrentInstancce.OnCompletingPurchase(newEntity);
        }
    }
}
