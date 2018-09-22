//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapNodeInfoPanelResourcecs.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI.MapNodeInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Map;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A UI component that displays the map node resources
    /// </summary>
    public class MapNodeInfoPanelResource : MonoBehaviour
    {
        /// <summary>
        /// Image for the resource
        /// </summary>
        public Image ResourceImage;

        /// <summary>
        /// Text field for the amount
        /// </summary>
        public Text AmountText;

        /// <summary>
        /// Sets the resource
        /// </summary>
        /// <param name="resource">Target resource</param>
        public void SetResource(ResourceType resource, float amount)
        {
            this.ResourceImage.sprite = PrefabManager.CurrentInstance.GetResourceSprite(resource);
            this.AmountText.text = amount.ToString();
        }
    }
}
