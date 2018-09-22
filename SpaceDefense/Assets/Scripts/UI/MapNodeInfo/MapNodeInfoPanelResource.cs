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
        /// Text to display the deficiency
        /// </summary>
        public Text DeficiencyText;

        /// <summary>
        /// Sets the resource
        /// </summary>
        /// <param name="resource">Target resource</param>
        public void SetResource(ResourceType resource, float totalAmount, float efficiency, bool isBeat)
        {
            this.ResourceImage.sprite = PrefabManager.CurrentInstance.GetResourceSprite(resource);
            this.AmountText.text = (totalAmount * efficiency).ToString();
            var deficiency = (1 - efficiency) * totalAmount;

            if (!isBeat && efficiency < 1)
            {
                this.DeficiencyText.gameObject.SetActive(true);
                this.DeficiencyText.text = "(-" + deficiency.ToString() + ")";
            }
            else
            {
                this.DeficiencyText.gameObject.SetActive(false);
            }
        }
    }
}
