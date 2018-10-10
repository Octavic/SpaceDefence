//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EntityPurchaseResourceCost.cs">
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
    using UnityEngine.UI;
    using Map;

    /// <summary>
    /// UI for each entry of entity purchase cost
    /// </summary>
    public class EntityPurchaseResourceCost : MonoBehaviour
    {
        public Image ResourceImage;
        public Text ResourceAmount;

        /// <summary>
        /// Shows the target resource and amount
        /// </summary>
        public void ShowResource(ResourceType resource, float amount)
        {
            var resourceSprite = PrefabManager.CurrentInstance.GetResourceSprite(resource);
            this.ResourceImage.sprite = resourceSprite;
            this.ResourceAmount.text = amount.ToString();
        }
    }
}
